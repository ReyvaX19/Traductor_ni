using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traductor_ni
{
    internal class AnalizadoresLexicos
    {
        UnidadesLexicas UL = new UnidadesLexicas();
        List<Tokens> lexemas = new List<Tokens>();
        string lexemaActivo = string.Empty;
        int cuentaCaracter = 0;

        protected bool EstaEn(char c, string cadena)
        {
            foreach (char C in cadena)
                if (C == c)
                    return true;
            return false;
        }

        protected bool ErrorManager(int error, int linea, string Archivo)
        {
            switch (error)
            {
                case 1:
                    MessageBox.Show("Error Lexico!\nCaracter no valido \"" + Archivo[cuentaCaracter] + "\" en linea:" + linea + " cercas de lexema \"" + lexemaActivo + "\".");
                    return false;
                case 2:
                    MessageBox.Show("Error Lexico!\nPunto decimal extra en valor numerico " + lexemaActivo + " en linea: " + linea);
                    return false;
                case 3:
                    MessageBox.Show("Error Lexico!\nCaracter no valido \"" + Archivo[cuentaCaracter] + "\" en linea:" + linea);
                    return false;
                default: 
                    return true;
            }
        }

        protected void AutomataLetras(string Archivo, int linea)
        {
            char c;

            do
            {
                c = Archivo[cuentaCaracter];
                if (char.IsLetterOrDigit(c))
                {
                    lexemaActivo += c;
                    cuentaCaracter++;
                }
                else if (c == '_')
                {
                    lexemaActivo += c;
                    cuentaCaracter++;
                }
                else 
                {
                    cuentaCaracter++;
                    break;
                }
            }while(true);

            Tokens resultado = UL.ObtenerIdPlabra(lexemaActivo, linea);

            lexemas.Add(resultado);

            lexemaActivo = string.Empty;
        }

        protected int AutomataDigitos(string Archivo, int linea)
        {
            char c;
            int estado = 1;

            do
            {
                c = Archivo[cuentaCaracter];
                if (char.IsDigit(c))
                {
                    lexemaActivo += c;
                    cuentaCaracter++;
                }
                else if (c == '.')
                {
                    if (estado == 1)
                    {
                        lexemaActivo += c;
                        estado++;
                        cuentaCaracter++;
                    }
                    else
                    {
                        lexemaActivo += c;
                        return 2;
                    }
                }
                else if (c == ' ')
                {
                    cuentaCaracter++;
                    break;
                }
                else
                {
                    return 1;
                }
            } while (true);

            Tokens resultado = new Tokens(110 + estado, lexemaActivo, linea, TipoTokken.ValoresNumericos);

            lexemas.Add(resultado);

            lexemaActivo = string.Empty;
            return 0;
        }

        protected void AutomataMenorQue(string Archivo, int linea)
        {
            cuentaCaracter++;

            if (Archivo[cuentaCaracter] == '=')
            {
                cuentaCaracter++;
                lexemas.Add(new Tokens(41,"<=", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
            }
            else
            {
                lexemas.Add(new Tokens(39,"<", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
            }
        }

        protected void AutomataMayorQue(string Archivo, int linea)
        {
            cuentaCaracter++;

            if (Archivo[cuentaCaracter] == '=')
            {
                cuentaCaracter++;
                lexemas.Add(new Tokens(42,">=", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
            }
            else
            {
                lexemas.Add(new Tokens(40, ">", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
            }
        }

        protected void AutomataExclamacion(string Archivo, int linea)
        {
            cuentaCaracter++;

            if (Archivo[cuentaCaracter] == '=')
            {
                cuentaCaracter++;
                lexemas.Add(new Tokens(38, "!=", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
            }
            else
            {
                lexemas.Add(new Tokens(45, "!", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
            }
        }

        protected void AutomataIgual(string Archivo, int linea)
        {
            cuentaCaracter++;

            if (Archivo[cuentaCaracter] == '=')
            {
                cuentaCaracter++;
                lexemas.Add(new Tokens(37, "==", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
            }
            else
            {
                lexemas.Add(new Tokens(36, "=", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
            }
        }

        public bool AnalisisLexico(string Archivo)
        {
            lexemas.Clear();
            cuentaCaracter = 0;
            lexemaActivo = string.Empty;
            char caracterActual;
            int linea = 1;
            bool compilacion = true;

            while (cuentaCaracter < Archivo.Length && compilacion)
            {
                caracterActual = Archivo[cuentaCaracter];

                switch(caracterActual)
                {
                    case ' ':
                        cuentaCaracter++;
                        break;
                    case '\t':
                        lexemas.Add(new Tokens(51, linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
                        cuentaCaracter++;
                        break;
                    case '\n':
                        cuentaCaracter++;
                        linea++;
                        break;
                    case '<':
                        AutomataMenorQue(Archivo,linea);
                        break;
                    case '>':
                        AutomataMayorQue(Archivo,linea);
                        break;
                    case '!':
                        AutomataExclamacion(Archivo,linea);
                        break;
                    case '=':
                        AutomataIgual(Archivo,linea);
                        break;
                    case '\"':
                        lexemas.Add(new Tokens(50, "\"", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
                        cuentaCaracter++;
                        break;
                    case '+':
                        lexemas.Add(new Tokens(31, "+", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
                        cuentaCaracter++;
                        break;
                    case '-':
                        lexemas.Add(new Tokens(32, "-", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
                        cuentaCaracter++;
                        break;
                    case '*':
                        lexemas.Add(new Tokens(33, "*", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
                        cuentaCaracter++;
                        break;
                    case '/':
                        lexemas.Add(new Tokens(34, "/", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
                        cuentaCaracter++;
                        break;
                    case '%':
                        lexemas.Add(new Tokens(35, "%", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
                        cuentaCaracter++;
                        break;
                    case '(':
                        lexemas.Add(new Tokens(46, "(", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
                        cuentaCaracter++;
                        break;
                    case ')':
                        lexemas.Add(new Tokens(47, ")", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
                        cuentaCaracter++;
                        break;
                    case ':':
                        lexemas.Add(new Tokens(48, ":", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
                        cuentaCaracter++;
                        break;
                    case '.':
                        lexemas.Add(new Tokens(49, ".", linea, TipoTokken.SimbolosEspaciosYPuntuaciones));
                        cuentaCaracter++;
                        break;

                    default:
                        if (char.IsLetter(caracterActual))
                        {
                            AutomataLetras(Archivo, linea);
                        } 
                        else if (char.IsDigit(caracterActual)) 
                        {
                            compilacion = ErrorManager(AutomataDigitos(Archivo, linea), linea, Archivo);
                        }
                        else
                        {
                            compilacion = ErrorManager(3, linea, Archivo);
                        }
                        break;
                }
            }
            return compilacion;
        }
    }
}
