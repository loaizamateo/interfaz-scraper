using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace interfaz_scraper
{
    public partial class Login : Form
    {
        public int xClick = 0, yClick = 0;

        public Login()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizarLogin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void ingresarDashboard_Click(object sender, EventArgs e)
        {
            if (inputCorreo.Text == "admin" && inputPass.Text == "admin123")
            {
                this.Hide();

                Dashboard dashboard = new Dashboard();

                dashboard.Show();
            }
            else
            {
                labelErrorLogin.Visible = true;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

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
    }
}
