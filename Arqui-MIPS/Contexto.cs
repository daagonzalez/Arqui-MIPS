using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arqui_MIPS
{
    class Contexto
    {
        private int PC;
        private int[] registros;
        private int ticks;
        private int quantum;
        private int id;

        public Contexto(int PC, int id)
        {
            this.PC = PC;
            this.id = id;
            registros = new int[32];
            ticks = 0;
            quantum = 0;
        }

        public int GetPC()
        {
            return PC;
        }

        public int GetRegistro(int i)
        {
            return registros[i];
        }

        public int GetTicks()
        {
            return ticks;
        }

        public int GetQuantum()
        {
            return quantum;
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

        public void SetTicks(int nTicks)
        {
            ticks = nTicks;
        }

        public void SetQuantum(int nQuantum)
        {
            quantum = nQuantum;
        }

        public void IncrementarQuantum()
        {
            quantum++;
        }

        public void IncrementarTicks()
        {
            ticks++;
        }

        public void AumentarPC(int aumento)
        {
            PC += aumento;
        }
    }
}
