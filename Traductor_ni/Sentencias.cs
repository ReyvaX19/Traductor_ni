using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traductor_ni
{
    internal class Sentencias
    {
        public int DentroDeEstructura { get; set; }
        
        public Sentencias()
        {
            
        }
    }

    internal class Estructuras : Sentencias
    {
        public int linea { get; set; }
        public int ID { get; set; }
        public int Tipo { get; set; }
        public string Argumentos = String.Empty;

        public Estructuras()
        {
            
        }
    }

    internal class Funciones : Estructuras
    {
        public string nombre = string.Empty;
        
        public Funciones()
        {

        }
    }

    internal class CierreEstructura : Sentencias
    {
        public int Tabs { get; set; }
        public CierreEstructura() { }
    }

    internal class Declaraciones : Sentencias
    {
        public string Nombre = String.Empty;
        public int tipo;
        public string Contenido = String.Empty;
        public Declaraciones() { }
    }

    internal class Asignaciones : Sentencias
    {
        public string Nombre = String.Empty;
        public string Contenido = String.Empty;
        public Asignaciones()
        {

        }
    }

    internal class DeclaraConstantes : Declaraciones
    {

    }
}
