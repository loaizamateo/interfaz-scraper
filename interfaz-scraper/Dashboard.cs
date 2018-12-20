using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace interfaz_scraper
{
    public partial class Dashboard : Form
    {
        public int xClick = 0, yClick = 0;

        public Dashboard()
        {
            InitializeComponent();
            dataGridView1.Rows.Clear();
            actualizarTablaParametros();
            actualizarTablaScrapers();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btnMaximizar.Visible = false;
            btnRestaurar.Visible = true;
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            btnRestaurar.Visible = false;
            btnMaximizar.Visible = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void logout_Click(object sender, EventArgs e)
        {
            this.Hide();

            Login login = new Login();

            login.Show();
        }

        private void menuVertical_MouseMove(object sender, MouseEventArgs e) {}

        private void panelContenedor_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            { xClick = e.X; yClick = e.Y; }
            else
            { this.Left = this.Left + (e.X - xClick); this.Top = this.Top + (e.Y - yClick); }
        }

        private void barraTitulo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            { xClick = e.X; yClick = e.Y; }
            else
            { this.Left = this.Left + (e.X - xClick); this.Top = this.Top + (e.Y - yClick); }
        }

        string id = "";
        int ejecucion = 0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow selectRow = dataGridView1.Rows[index];
            comboBoxParametro.Items.Clear();
            comboBoxParametro.Enabled = true;
            actualizarEjecucion.Enabled = true;
            int ejecucionSiguiente = Int32.Parse(selectRow.Cells[1].Value.ToString()) + 1;
            comboBoxParametro.Items.Add(ejecucionSiguiente);
            comboBoxParametro.SelectedIndex = 0;

            id = selectRow.Cells[0].Value.ToString();
            ejecucion = ejecucionSiguiente;
            //MessageBox.Show(selectRow.Cells[0].Value.ToString());
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void inputCorreo_TextChanged(object sender, EventArgs e)
        {

        }

        void actualizarTablaParametros()
        {
            dataGridView1.DataSource = null;
            DataTable dt = GetParametros.getAllDocument();
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["ID"].Visible = false;
        }

        void actualizarTablaScrapers()
        {
            dataGridView2.DataSource = null;
            DataTable dataTable = new DataTable("Parametro");
            dataTable.Columns.Add("ID", typeof(string));
            dataTable.Columns.Add("Pagina", typeof(string));
            dataTable.Columns.Add("Acciones", typeof(string));
            dataTable.Columns.Add("# ejecuciones", typeof(string));
            dataTable.Columns.Add("# registros extraidos", typeof(string));


            dataTable.Rows.Add("1","idealista","","0", "0");
            dataTable.Rows.Add("2","imovirtual", "", "0", "0");
            dataTable.Rows.Add("3","casasapo", "", "0", "0");
            dataTable.Rows.Add("4","lardocelar", "", "0", "0");

            dataGridView2.DataSource = dataTable;
            dataGridView2.Cursor = Cursors.Hand;
            dataGridView2.Columns["ID"].Visible = false;
        }

        void actualizarParametroEjecucion(string id, int ejecucionSiguiente)
        {
            DialogResult result = MessageBox.Show("Seguro de seguir con la siguiente ejecucion?","",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);

            if(result == DialogResult.Yes)
            {
                var linkScraper = new Parametro
                {
                    parametro = ejecucionSiguiente
                };

                ConnectionToES.EsClient().Update(DocumentPath<Parametro>
                    .Id(id),
                     u => u
                        .Index("parametros")
                        .Type("scraper")
                        .DocAsUpsert(true)
                        .Doc(linkScraper)
                );

                actualizarTablaParametros();
                comboBoxParametro.Items.Clear();
                comboBoxParametro.Enabled = false;
                actualizarEjecucion.Enabled = false;
            }
        }

        private void actualizarEjecucion_Click(object sender, EventArgs e)
        {
            actualizarParametroEjecucion(id,ejecucion);
        }

        private void cargar_Click(object sender, EventArgs e)
        {
            actualizarTablaParametros();
        }

        private void dataGridView2_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && this.dataGridView2.Columns[e.ColumnIndex].Name == "Acciones" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                DataGridViewButtonCell celBoton = this.dataGridView2.Rows[e.RowIndex].Cells["Acciones"] as DataGridViewButtonCell;
                Icon icoAtomico = new Icon(Environment.CurrentDirectory + @"\\play.ico");/////Recuerden colocar su icono en la carpeta debug de su proyecto
                e.Graphics.DrawIcon(icoAtomico, e.CellBounds.Left + 36, e.CellBounds.Top + 3);

                this.dataGridView2.Rows[e.RowIndex].Height = icoAtomico.Height + 5;
                this.dataGridView2.Columns[e.ColumnIndex].Width = icoAtomico.Width + 5;

                e.Handled = true;
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView2.Columns[e.ColumnIndex].Name == "Acciones")
            {
                int index = e.RowIndex;
                DataGridViewRow selectRow = dataGridView2.Rows[index];

                switch (Int32.Parse(selectRow.Cells[0].Value.ToString()))
                {
                    case 1:
                        Process.Start(Environment.CurrentDirectory + @"\\wscasasapo\\wscraper-casa-sapo.exe");

                        Process[] processes = Process.GetProcesses();
                        int i = 0;

                        foreach (Process process in processes)
                        {
                            if(process.ProcessName.Contains("wscraper-casa-sapo"))
                            {
                                i++;
                            }
                        }
                        MessageBox.Show(i.ToString());

                        break;
                    case 2:
                        Process.Start("C:/Program Files/internet explorer/iexplore.exe");
                        break;
                    case 3:
                        Process.Start("C:/Program Files/internet explorer/iexplore.exe");
                        break;
                    case 4:
                        Process.Start("C:/Program Files/internet explorer/iexplore.exe");
                        break;
                    default:
                        MessageBox.Show("esta en el cauqlueira");
                        break;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void traerDatos_Click(object sender, EventArgs e)
        {
            
        }
    }
}
