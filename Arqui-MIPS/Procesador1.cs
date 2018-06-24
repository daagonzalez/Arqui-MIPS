using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Arqui_MIPS
{
    // esta es la clase para el procesador de 1 núcleo.
    class Procesador1
    {

        public const int CantidadRegistros = 32;

        public const int EstadoInvalido = 0;
        public const int EstadoCompartido = 1;
        public const int EstadoModificado = 2;

        private int tamCacheDatosFilas;
        private int tamCacheDatosColumnas;

        private int tamCacheInstFilas;
        private int tamCacheInstColumnas;

        // referente a sincronizacion
        public Barrier Sync;
        public int Quantum;
        public int Id, CicloActual;
        public List<Contexto> Contextos, ContextosFinalizados;

        public int[][] CacheDatos;
        public int[][] CacheInstrucc;

        // Struct para representar una direccion de memoria
        public struct Direccion
        {
            public int NumeroBloque;
            public int NumeroPalabra;
        }

        public Procesador1(int id, Barrier s, List<string> programas, int tamDatFilas, int tamDatColumnas, int tamInstFilas, int tamInstColumnas, int quantumRec)
        {
            Sync = s;
            Id = id;
            CicloActual = 1;
            tamCacheDatosFilas = tamDatFilas;
            tamCacheDatosColumnas = tamDatColumnas;
            tamCacheInstFilas = tamInstFilas;
            tamCacheInstColumnas = tamInstColumnas;
            Quantum = quantumRec;


            CacheDatos = new int[tamDatFilas,tamDatColumnas];
        }

        /// Genera la instrucción bonita basado en los códigos de operación
        public string GetStringInstruccion(int[] instruccion)
        {
            int codigoInstruccion = instruccion[0],
                regFuente1 = instruccion[1],
                regFuente2 = instruccion[2],
                regDest = instruccion[3];
            string res = "";

            switch (codigoInstruccion)
            {
                case 2:
                    /*
                    JR RX: PC=RX
                    CodOp: 2 RF1: X RF2 O RD: 0 RD o IMM:0
                    */
                    res = $"JR R{regFuente1}";
                    break;
                case 3:
                    /*
                    JAL n, R31=PC, PC = PC+n
                    CodOp: 3 RF1: 0 RF2 O RD: 0 RD o IMM:n
                    */
                    res = $"JAL {regDest}";
                    break;
                case 4:
                    /*
                    BEQZ RX, ETIQ : Si RX = 0 salta 
                    CodOp: 4 RF1: Y RF2 O RD: 0 RD o IMM:n
                    */
                    res = $"BEQZ R{regFuente1},{regDest}";
                    break;
                case 5:
                    /*
                     BEQNZ RX, ETIQ : Si RX != 0 salta 
                     CodOp: 5 RF1: x RF2 O RD: 0 RD o IMM:n
                     */
                    res = $"BNEQZ R{regFuente1},{regDest}";
                    break;
                case 8:
                    /*
                    DADDI RX, RY, #n : Rx <-- (Ry) + n
                    CodOp: 8 RF1: Y RF2 O RD: x RD O IMM:n
                    */
                    res = $"DADDI R{regFuente2},R{regFuente1},{regDest}";
                    break;
                case 12:
                    /*
                    DMUL RX, RY, #n : Rx <-- (Ry) * (Rz)
                    CodOp: 12 RF1: Y RF2 O RD: z RD o IMM:X
                    */
                    res = $"DMUL R{regDest},R{regFuente1},R{regFuente2}";
                    break;
                case 14:
                    /*
                    DDIV RX, RY, #n : Rx <-- (Ry) / (Rz)
                    CodOp: 14 RF1: Y RF2 O RD: z RD o IMM:X
                    */
                    res = $"DDIV R{regDest},R{regFuente1},R{regFuente2}";
                    break;
                case 32:
                    /*
                    DADD RX, RY, #n : Rx <-- (Ry) + (Rz)
                    CodOp: 32 RF1: Y RF2 O RD: x RD o IMM:Rz
                    */
                    res = $"DADD R{regDest},R{regFuente1},R{regFuente2}";
                    break;
                case 34:
                    /*
                    DSUB RX, RY, #n : Rx <-- (Ry) - (Rz)
                    CodOp: 34 RF1: Y RF2 O RD: z RD o IMM:X
                    */
                    res = $"DSUB R{regDest},R{regFuente1},R{regFuente2}";
                    break;
                case 35:
                    /* *
                    * LW Rx, n(Ry)
                    * Rx <- M(n + (Ry))
                    * 
                    * codOp: 35 RF1: Y RF2 O RD: X RD O IMM: n
                    * */
                    res = $"LW R{regFuente2} {regDest}(R{regFuente1})";
                    break;
                case 43:
                    /* *
                     * SW RX, n(rY)
                     * m(N+(RY)) = rX
                     * codOp: 51 RF1: Y RF2 O RD: X RD O IMM: n
                     * */
                    res = $"SW R{regFuente2} {regDest}(R{regFuente1})";
                    break;
                case 50:
                    /* *
                     * LL Rx, n(Ry)
                     * Rx <- M(n + (Ry))
                     * Rl <- n+(Ry)
                     * codOp: 50 RF1: Y RF2 O RD: X RD O IMM: n
                     * */
                    res = $"LL R{regFuente2} {regDest}(R{regFuente1})";
                    break;
                case 51:
                    /* *
                     * SC RX, n(rY)
                     * IF (rl = N+(Ry)) => m(N+(RY)) = rX
                     * ELSE Rx =0
                     *  codOp: 51 RF1: Y RF2 O RD: X RD O IMM: n
                     * */
                    res = $"SC R{regFuente2} {regDest}(R{regFuente1})";
                    break;
                case 63:
                    /*
                     fin
                     CodOp: 63 RF1: 0 RF2 O RD: 0 RD o IMM:0
                     */
                    res = "FIN";
                    break;
            }
            return res;
        }
    }

}
