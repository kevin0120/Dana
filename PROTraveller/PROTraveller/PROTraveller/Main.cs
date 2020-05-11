using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROTraveller
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }
        public int[] s = { 0, 0, 0 };

        private void Tab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (s[tabCtrl.SelectedIndex] == 0)
            {
                btnX_Click(sender, e);
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            string formClass = "PROTraveller.Import";
            GenerateForm(formClass, tabCtrl);

        }

        public void GenerateForm(string form, object sender)
        {

            Form fm = (Form)Assembly.GetExecutingAssembly().CreateInstance(form);

            fm.FormBorderStyle = FormBorderStyle.None;
            fm.TopLevel = false;
            fm.Parent = ((TabControl)sender).SelectedTab;
            fm.ControlBox = false;
            fm.Dock = DockStyle.Fill;
            fm.Show();
            s[((TabControl)sender).SelectedIndex] = 1;
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            string formClass = ((TabControl)sender).SelectedTab.Tag.ToString();

            GenerateForm(formClass, sender);

        }

        private void tabExecute_Click(object sender, EventArgs e)
        {

        }
    }
}
