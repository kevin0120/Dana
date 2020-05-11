using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROTraveller
{
    public partial class Import : Form
    {
        DataAccess da = new DataAccess();
        public Import()
        {
            InitializeComponent();
        }

        private void Import_Load(object sender, EventArgs e)
        {
            if( txtPROtravllerLoc.Text == String.Empty)
            {
                btnImport.Enabled = false;
            }


        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*xls*)|*.xls*"; 
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = fileDialog.FileName;
                txtPROtravllerLoc.Text = file;
                DataTable dt = Util.ExcelToTable(txtPROtravllerLoc.Text);
                foreach(DataRow dr in dt.Rows)
                {
                    string startDateValue = dr["Start date"].ToString();
                    string endDateValue = dr["End date"].ToString();
                    string startDate = DateTime.FromOADate(Convert.ToInt32(startDateValue)).ToString("d");
                    string endDate = DateTime.FromOADate(Convert.ToInt32(endDateValue)).ToString("d");
                    dr["Start date"] = startDate;
                    dr["End date"] = endDate;
                }
                dataGridViewImport.DataSource = dt;
                dataGridViewImport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                if (dt.Rows.Count <= 10)
                {
                    dataGridViewImport.Height = (dataGridViewImport.RowCount + 2) * dataGridViewImport.Columns[0].HeaderCell.Size.Height;
                }
                dataGridViewImport.EnableHeadersVisualStyles = false;
                dataGridViewImport.EnableHeadersVisualStyles = false;
                dataGridViewImport.ColumnHeadersDefaultCellStyle.BackColor = Color.Orange;
                dataGridViewImport.Visible = true;
                btnImport.Enabled = true;

            }



        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            List<PROTravellerModel> proTras = new List<PROTravellerModel>();
            PROTravellerModel item = new PROTravellerModel();
            DataTable dt = Util.ExcelToTable(txtPROtravllerLoc.Text);
            dataGridViewImport.DataSource = dt;
            foreach (DataRow dr in dt.Rows)
            {
                string startDateValue = dr["Start date"].ToString();
                string endDateValue = dr["End date"].ToString();
                string startDate = DateTime.FromOADate(Convert.ToInt32(startDateValue)).ToString("d");
                string endDate = DateTime.FromOADate(Convert.ToInt32(endDateValue)).ToString("d");
                dr["Start date"] = startDate;
                dr["End date"] = endDate;
            }
            try {
            foreach (DataRow dr in dt.Rows)
            {
                item.Line = dr["Line"].ToString();
                item.Year = dr["Year"].ToString();
                item.OrderNumber = dr["Order Number"].ToString();
                item.TotalQuantity = dr["Total quantity"].ToString();
                item.Model = dr["Model"].ToString();
                item.StartDate = dr["Start date"].ToString();
                item.EndDate = dr["End date"].ToString();
                item.Description = dr["description"].ToString();
                item.CreateTime = DateTime.Now.ToString().Substring(0,9);
                item.Status = "Not started"; 
                da.InsertResult(item);
            }
                MessageBox.Show("导入成功");
            }
            catch(Exception e1){
                MessageBox.Show(e1.Message);
            }
            finally { 
            txtPROtravllerLoc.Text = String.Empty;
            dataGridViewImport.Visible = false;
            }
        }

        private void dataGridViewImport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

