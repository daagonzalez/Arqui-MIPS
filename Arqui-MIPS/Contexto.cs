namespace Arqui_MIPS
{
    public class Contexto
    {
        private int PC;
        private int[] registros;
        private readonly int cicloLlegada;
        private int cicloSalida;
        private readonly int id;

        public Contexto(int PC, int id, int cicloLlegada)
        {
            this.PC = PC;
            this.id = id;
            registros = new int[32];
            registros[0] = 0;
            this.cicloLlegada = cicloLlegada;
            cicloSalida = -1;
        }

        public int GetPC()
        {
            return PC;
        }

        public int GetRegistro(int i)
        {
            return registros[i];
        }

        public int GetDuracion()
        {
            return cicloSalida - cicloLlegada;
        }

        public int GetId()
        {
            return id;
        }

        public void SetPC(int nPC)
        {
            PC = nPC;
        }

        public void SetRegistro(int i, int nValor)
        {
            registros[i] = nValor;
        }

        public void SetCicloSalida(int cicloSalida)
        {
            this.cicloSalida = cicloSalida;
        }

        public void AumentarPC(int aumento)
        {
            PC += aumento;
        }
    }
}
