using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traductor_ni
{
    internal class UnidadesLexicas
    {
        readonly Dictionary<string, int> PalabrasReservadas = new Dictionary<string, int>();

        public void LlenarDiccionario(int longitud)
        {
            if (longitud == 0)
            {
                PalabrasReservadas.Add("fin", 1);
                PalabrasReservadas.Add("caso", 2);
                PalabrasReservadas.Add("constante", 3);
                PalabrasReservadas.Add("hacer", 4);
                PalabrasReservadas.Add("sino",5);
                PalabrasReservadas.Add("repite",6);
                PalabrasReservadas.Add("si",7);
                PalabrasReservadas.Add("devuelve",8);
                PalabrasReservadas.Add("switch",9);
                PalabrasReservadas.Add("void",10);
                PalabrasReservadas.Add("nada",10);
                PalabrasReservadas.Add("mientras",11);
                PalabrasReservadas.Add("es",12);
                PalabrasReservadas.Add("ciclo_num",13);
                PalabrasReservadas.Add("funcion",14);
                PalabrasReservadas.Add("recibe",15);
                PalabrasReservadas.Add("true", 16);
                PalabrasReservadas.Add("verdadero", 16);
                PalabrasReservadas.Add("false", 17);
                PalabrasReservadas.Add("falso", 17);

                PalabrasReservadas.Add("char",21);
                PalabrasReservadas.Add("float",22);
                PalabrasReservadas.Add("int",23);
                PalabrasReservadas.Add("bool",24);
                PalabrasReservadas.Add("string",25);

                PalabrasReservadas.Add("+",31);
                PalabrasReservadas.Add("mas",31);
                PalabrasReservadas.Add("-",32);
                PalabrasReservadas.Add("menos",32);
                PalabrasReservadas.Add("*",33);
                PalabrasReservadas.Add("por",33);
                PalabrasReservadas.Add("/",34);
                PalabrasReservadas.Add("sobre",34);
                PalabrasReservadas.Add("%",35);
                PalabrasReservadas.Add("=",36);
                PalabrasReservadas.Add("vale",36);
                PalabrasReservadas.Add("==",37);
                PalabrasReservadas.Add("!=",38);
                PalabrasReservadas.Add("<",39);
                PalabrasReservadas.Add("menor_que",39);
                PalabrasReservadas.Add(">",40);
                PalabrasReservadas.Add("mayor_que",40);
                PalabrasReservadas.Add("<=",41);
                PalabrasReservadas.Add("menor_igual",41);
                PalabrasReservadas.Add(">=",42);
                PalabrasReservadas.Add("mayor_igual",42);

                PalabrasReservadas.Add("&",43);
                PalabrasReservadas.Add("Y",43);
                PalabrasReservadas.Add("|",44);
                PalabrasReservadas.Add("O",44);
                PalabrasReservadas.Add("!",45);
                PalabrasReservadas.Add("NO",45);
                PalabrasReservadas.Add("(",46);
                PalabrasReservadas.Add(")",47);
                PalabrasReservadas.Add(":",48);
                PalabrasReservadas.Add(".",49);
                PalabrasReservadas.Add("\"",50);

                PalabrasReservadas.Add("\t",51);
                PalabrasReservadas.Add("\n",52);
                //Nombres de Variables ID =100

                //valores numericos id
                // 111 entero
                // 112 flotante
                //valores char id = 113
            }
        }

        public Tokens ObtenerIdPlabra (string lexema, int linea)
        {
            LlenarDiccionario(PalabrasReservadas.Count);

            foreach (KeyValuePair<string,int> Lex in PalabrasReservadas)
            {
                if (lexema == Lex.Key)
                {
                    return new Tokens(Lex.Value, lexema, linea, TipoTokken.PalabraReservada);
                }
            }
            return new Tokens(100, lexema, linea, TipoTokken.Variables);
        }

    }
}
