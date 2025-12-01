namespace Traductor_ni
{
    public partial class CompiladorNi : Form
    {
        string RutaArchivo;
        AnalizadoresLexicos AL = new AnalizadoresLexicos();

        public CompiladorNi()
        {
            InitializeComponent();
        }

        private void btnComplilar_Click(object sender, EventArgs e)
        {
            bool Compilacion;
            //corre primero el analizador Lexico
            Compilacion = AL.AnalisisLexico(rtbCodigo.Text);
            if (Compilacion)
            {
                //imprimimos en Consola.
                List<Tokens> tokens = AL.lexemas;
                string MensajeConsola = string.Empty;
                foreach (Tokens t in tokens)
                {
                    MensajeConsola += ("{" + t.contenido + "},");
                }
                Consola.Text = MensajeConsola;
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            rtbCodigo.Clear();
            RutaArchivo = string.Empty;
            CompiladorNi.ActiveForm.Text = "TraductorNi";
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            OpenFileDialog AbrirArchivo = new OpenFileDialog();
            AbrirArchivo.Filter = "Texto|*.*";

            if(AbrirArchivo.ShowDialog() == DialogResult.OK)
            {
                RutaArchivo = AbrirArchivo.FileName;

                using(StreamReader sr = new StreamReader(RutaArchivo))
                {
                    rtbCodigo.Text = sr.ReadToEnd();
                }
                CompiladorNi.ActiveForm.Text = "TraductorNi - " + RutaArchivo;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            SaveFileDialog GuardarArchivo = new SaveFileDialog()
            {
                Title = "Guardar: ",
                Filter = "TraductorNi|*.ni",
                AddExtension = true
            };

            if (RutaArchivo!=string.Empty)
            {
                using (StreamWriter sw = new StreamWriter(RutaArchivo))
                {
                    sw.Write(rtbCodigo.Text);
                }
            }
            else
            {
                if(GuardarArchivo.ShowDialog()==DialogResult.OK)
                {
                    RutaArchivo = GuardarArchivo.FileName;
                    using (StreamWriter sw = new StreamWriter(RutaArchivo))
                    {
                        sw.Write(rtbCodigo.Text);
                    }
                }
            }
        }

        private void btnGuardarComo_Click(object sender, EventArgs e)
        {
            SaveFileDialog GuardarComo = new SaveFileDialog()
            {
                Title = "Guardar como: ",
                Filter = "TraductorNi|*.ni",
                AddExtension = true
            };
            GuardarComo.ShowDialog();

            if(GuardarComo.FileName != string.Empty)
            {
                RutaArchivo = GuardarComo.FileName;
                using(StreamWriter sw = new StreamWriter(RutaArchivo))
                {
                    sw.Write(rtbCodigo.Text);
                    CompiladorNi.ActiveForm.Text = "TraductorNi - " + RutaArchivo;
                    sw.Close();
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}