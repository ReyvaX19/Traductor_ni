using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Traductor_ni
{
    internal class AnalizadoresSintacticos
    {
        List<List<Tokens>> Lineas;
        int LineaActual = 0;
        int TABsActivos = 0;
        int Lexema = 0;
        int ContadorEstructuras = 1;
        string Grupo1 = String.Empty;
        //tabla de variables
        DataTable tVariables;
        //tabla de estructuras
        DataTable tEstructuras;
        //Stack de estructuras
        Stack<Estructuras> estructuras = new Stack<Estructuras>();
        //salida: Una lista de Sentencias.
        List<Sentencias> Salida = new List<Sentencias>();
        
        
        public AnalizadoresSintacticos()
        {
            Lineas = new List<List<Tokens>>();

            tVariables = new DataTable();
            tEstructuras = new DataTable();

            tVariables.Columns.Add("Nombre", typeof(string));
            tVariables.Columns.Add("Tipo", typeof(int));
            tVariables.Columns.Add("Constante", typeof(bool));
            tVariables.Columns.Add("Pertenece a", typeof(int));

            tEstructuras.Columns.Add("id", typeof(int));
            tEstructuras.Columns.Add("Tipo", typeof(int));
        }

        protected bool ErrorManager (int error)
        {
            switch (error)
            {
                case 1:
                    MessageBox.Show("Error sintactico!\nLa estructura de la linea " + Lineas[LineaActual][Lexema].linea + " no esta siendo usada.");
                    return false;
                case 2:
                    MessageBox.Show("Error sintactico!\nEn la linea " + Lineas[LineaActual][Lexema].linea + " se esperaba la palabra \"vale\" o un signo \"=\"");
                    return false;
                case 3:
                    MessageBox.Show("Error sintactico!\nEn la linea " + Lineas[LineaActual][Lexema].linea + " se esperaba la palabra \"es\"");
                    return false;
                case 4:
                    MessageBox.Show("Error sintactico!\nEn la linea " + Lineas[LineaActual][Lexema].linea + " se esperaba recibir un tipo de variable");
                    return false;
                case 5:
                    MessageBox.Show("Error sintactico!\nEn la linea " + Lineas[LineaActual][Lexema].linea + " se esperaba un signo \".\"");
                    return false;
                case 6:
                    MessageBox.Show("Error semantico!\nEn la linea " + Lineas[LineaActual][Lexema].linea + "\nNo se puede modificar el valor de una constante");
                    return false;
                case 7:
                    MessageBox.Show("Error sintactico!\nEn la linea " + Lineas[LineaActual][Lexema].linea + " se esperaba un nombre de variable");
                    return false;
                case 8:
                    MessageBox.Show("Error sintactico!\nEn la linea " + Lineas[LineaActual][Lexema].linea + " se esta recibiendo un Token invalido");
                    return false;
                default: return true;

            }
        }

        public List<List<Tokens>> ObtenerSentencias(List<Tokens> lexemas)
        {
            List<List<Tokens>> Lineas = new List<List<Tokens>>();
            int linea = 1;
            int cuenta = 0;

            while (cuenta < lexemas.Count)
            {
                if (linea == lexemas[cuenta].linea)
                {
                    List<Tokens> sentencia = new List<Tokens>();
                    while ( cuenta < lexemas.Count && lexemas[cuenta].linea == linea)
                    {
                        sentencia.Add(lexemas[cuenta]);
                        cuenta++;
                    }
                    Lineas.Add(sentencia);
                }
                else if (linea > lexemas[cuenta].linea)
                {
                    MessageBox.Show("Se aumento la linea de mas");
                }
                linea++;
            }

            return Lineas;
        }

        private int RevizaTABs()
        {
            for (int i = 0; i < TABsActivos; i++)
            {
                if (Lineas[LineaActual][i].id != 60)
                {
                    //esto significa que almenos 1 ciclo se cierra
                    int ciclosCerrados = TABsActivos - i;
                    for (int j = 0; j < ciclosCerrados; j++)
                    {
                        if (Lineas[LineaActual][0].linea == estructuras.Peek().linea)
                        {
                            Lexema = i;
                            return 1;
                        }
                        estructuras.Pop();
                        Salida.Add(new CierreEstructura() { Tabs = TABsActivos - j, DentroDeEstructura = estructuras.Peek().ID});
                    }
                    TABsActivos -= ciclosCerrados;
                    //posible problema aqui, Revizar mas tarde.
                    
                }
            }
            Lexema = TABsActivos;
            return 0;
        }

        //Esta funcion reviza que la sintaxis de una operacion artimetica sea correcta
        //Si es correcta transforma los Tokens en una sola cadena string que acaba en ";" 
        //en lugar de ".", llamada Grupo1. El automata es distinto para cada tipo de variable.
        protected int SintaxisArtitmetica(List<Tokens> SentenciaAcutual, int tipo)
        {
            switch (tipo)
            {
                case 1: //variables char

                    break;

                case 2: //variables float

                    break;

                case 3: //variable int

                    break;

                case 4: //variable bool

                    break;

                case 5: //variable string

                    break;  

            }
            return 0;
        }

        protected int AutomataVariables(List<Tokens> SentenciaAcutual)
        {
            //aqui se acaba de leer el token con el nombre de una variable.
            //Revizo si la variable ya existe en la tabla de Variables.
            string Nombre = SentenciaAcutual[Lexema].contenido;
            DataRow[] registro = tVariables.Select("Nombre == " + Nombre);
            Lexema++;
            if (registro.Count() > 0)
            {
                //La variable esta declarada y se espera que el siguiente token sea la palabra "vale"
                if ( Lexema < SentenciaAcutual.Count && SentenciaAcutual[Lexema].id == 36)
                {
                    if (registro[0].Field<bool>("Constante")== true)
                    {
                        return 6;
                    }
                    Lexema++;
                    int tipo = registro[0].Field<int>("Tipo");
                    int error = SintaxisArtitmetica(SentenciaAcutual, tipo);
                    if (error == 0)
                    {
                        //crear Objeto sentencia y anadirlo a la salida
                        Asignaciones asignacion = new Asignaciones();
                        asignacion.Nombre = Nombre;
                        asignacion.Contenido = Grupo1;
                        Grupo1 = string.Empty;
                    }
                    else
                    {
                        return error;
                    }
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                //La variable no existe y se espera que el siguiente token sea la palabra "es"
                if (Lexema < SentenciaAcutual.Count && SentenciaAcutual[Lexema].id == 12)
                {
                    Lexema++;
                    //aqui se espera que el siguiente token sea un tipo de variable.
                    int tipo = SentenciaAcutual[Lexema].id - 20;
                    if (Lexema < SentenciaAcutual.Count && tipo > 0 && tipo < 11)
                    {
                        Lexema++;
                        //aqui solo hay 2 opciones validas mas. que el token siguiente sea un "."
                        //o que sea un "vale".
                        if (Lexema < SentenciaAcutual.Count && SentenciaAcutual[Lexema].id == 49)
                        {
                            //49 es "."
                            //aqui es donde debe acabar la sentencia.
                            //aqui anadimos la variable a la tabla de variables.
                            DataRow nuevaVariable = tVariables.NewRow();
                            nuevaVariable["Nombre"] = Nombre;
                            nuevaVariable["Tipo"] = tipo;
                            nuevaVariable["Constante"] = false;
                            if (estructuras.Count > 0)
                            {
                                nuevaVariable["Pertenece a"] = estructuras.Peek().ID;
                            }
                            else
                            {
                                nuevaVariable["Pertenece a"] = 0;
                            }
                            //Ahora ya se puede hacer referencia a esta variable en el analisis Sintactico y Semantico.
                            //Lo que sigue es crear el objeto sentencia
                            Declaraciones declaracion = new Declaraciones();
                            declaracion.Nombre = Nombre;
                            declaracion.tipo = tipo;

                            Salida.Add(declaracion);
                        }
                        else if (Lexema < SentenciaAcutual.Count && SentenciaAcutual[Lexema].id == 36)
                        {
                            //36 es "vale" o "="
                            Lexema++;
                            int error = SintaxisArtitmetica(SentenciaAcutual, tipo);
                            if (error == 0)
                            {
                                DataRow nuevaVariable = tVariables.NewRow();
                                nuevaVariable["Nombre"] = Nombre;
                                nuevaVariable["Tipo"] = tipo;
                                nuevaVariable["Constante"] = false;
                                if (estructuras.Count > 0)
                                {
                                    nuevaVariable["Pertenece a"] = estructuras.Peek().ID;
                                }
                                else
                                {
                                    nuevaVariable["Pertenece a"] = 0;
                                }

                                Declaraciones declaracion = new Declaraciones();
                                declaracion.Nombre = Nombre;
                                declaracion.tipo = tipo;
                                declaracion.Contenido = Grupo1;
                                
                                Grupo1 = string.Empty;
                            }
                            else
                            {
                                return error;
                            }
                        }
                        else
                        {
                            return 5;
                        }
                        
                    }
                    else
                    {
                        return 4;
                    }
                }
                else
                {
                    return 3;
                }
            }

            return 0;
        }

        protected int AutomataConstantes(List<Tokens> SentenciaActual)
        {
            Lexema++;
            if (Lexema < SentenciaActual.Count() && SentenciaActual[Lexema].id == 100)
            {
                //revisamos que no exista ya esa constante en las variables.
                string Nombre = SentenciaActual[Lexema].contenido;
                DataRow[] registro = tVariables.Select("Nombre == " + Nombre);
                if (registro.Length > 0)
                {
                    //significa que ya existe esta constante
                }
                else
                {
                    //no existe una constante con este nombre.
                }
            }
            else
            {
                return 7;
            }

            return 0;
        }
        public bool AnalisisSintactico(List<Tokens> lexemas)
        {
            tVariables.Clear();
            tEstructuras.Clear();

            Lineas = ObtenerSentencias(lexemas);
            LineaActual = 0;

            bool compilacion = true;

            while (LineaActual < Lineas.Count && compilacion)
            {
                Lexema = 0;
                //Primero reviso que los TABs esten bien.
                if (TABsActivos > 0)
                {
                    compilacion = ErrorManager(RevizaTABs());
                }
                if (compilacion)
                {
                    switch (Lineas[LineaActual][Lexema].id)
                    {
                        case 100: //Nombre de Variable
                            compilacion = ErrorManager(AutomataVariables(Lineas[LineaActual]));
                            break;
                        case 3: //Palabra Reservada "Constante"
                            compilacion = ErrorManager(AutomataConstantes(Lineas[LineaActual]));
                            break;

                        case 7: //Palabra reservada "si" o "if"
                            break;

                        case 11: //Palabra reservada "mientras" o "while"
                            break;

                        case 4: //Palabra reservada "hacer" o "do"
                            break;

                        case 6: //Palabra reservada "repite" o "for"
                            break;

                        case 9: //Palabra reservada "switch"
                            break;

                        case 14: //Palabra reservada "funcion"
                            break;  

                        case 2: //Palabra reservada "caso" o "case"
                            break;

                        case 5: //Palabra reservada "sino" o "else"
                            break;

                        case 18: //Palabra reservada "imprime" de Printf
                            break;

                        case 1: //Palabra reservada "fin" o "break"
                            break;

                        default:
                            compilacion = ErrorManager(8);
                            break;
                    }
                }
                LineaActual++;


            }
             
            return compilacion;
        }

    }
}
