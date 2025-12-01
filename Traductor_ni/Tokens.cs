using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traductor_ni
{
    enum TipoTokken { PalabraReservada, Variables, ValoresNumericos, SimbolosEspaciosYPuntuaciones}
    internal class Tokens
    {
        public int id;
        public TipoTokken tipo;
        public string contenido = string.Empty;
        public int linea;

        public Tokens(int ID, string Lexema, int Linea, TipoTokken tipo)
        {
            this.id = ID;
            this.tipo = tipo;
            this.contenido = Lexema;
            this.linea = Linea;
        }

        public Tokens(int ID, int Linea, TipoTokken Tipo)
        {
            this.id = ID;
            this.tipo = Tipo;
            this.linea= Linea;
        }

    }
}
