using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace PROTraveller
{
    public partial class Execute : Form
    {
        public Execute()
        {
            InitializeComponent();
        }
        DataAccess da = new DataAccess();

        private void dataGridViewExecute_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {

                    if (dataGridViewExecute.Rows[e.RowIndex].Selected == false)
                    {
                        dataGridViewExecute.ClearSelection();
                        dataGridViewExecute.Rows[e.RowIndex].Selected = true;
                    }

                    if (dataGridViewExecute.SelectedRows.Count == 1)
                    {
                        dataGridViewExecute.CurrentCell = dataGridViewExecute.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }

                    ContextMenu.Show(MousePosition.X, MousePosition.Y);
                }
            }

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {

            string line = comboBoxLine.Text;
            string proNumber = txtProNumber.Text;
            string status = comboBoxStutus.Text;
            DataTable dt = da.GetProDb(line, proNumber, status).Tables[0];
            if (dt.Rows.Count != 0)
            {
                dataGridViewExecute.DataSource = dt;  //dataSet.Tables[0].DefaultView
                if (dt.Rows.Count <= 10)
                {
                    dataGridViewExecute.Height = (dataGridViewExecute.RowCount + 2) * dataGridViewExecute.Columns[0].HeaderCell.Size.Height;
                }
                dataGridViewExecute.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridViewExecute.Columns[0].Visible = false;
                dataGridViewExecute.EnableHeadersVisualStyles = false;
                dataGridViewExecute.EnableHeadersVisualStyles = false;
                dataGridViewExecute.ColumnHeadersDefaultCellStyle.BackColor = Color.Orange;
                dataGridViewExecute.Visible = true;
            }
            else
            {
                dataGridViewExecute.Visible = false;
                MessageBox.Show("未查询到数据");

            }

        }

        private void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = dataGridViewExecute.CurrentRow.Index;
            string id = dataGridViewExecute.Rows[index].Cells[0].Value.ToString();
            int totalQuantity = int.Parse(dataGridViewExecute.Rows[index].Cells[4].Value.ToString());
            string status = dataGridViewExecute.Rows[index].Cells[6].Value.ToString();
            string orderNumber = dataGridViewExecute.Rows[index].Cells[3].Value.ToString();
            string model = dataGridViewExecute.Rows[index].Cells[5].Value.ToString();
            string line1 = dataGridViewExecute.Rows[index].Cells[1].Value.ToString();
            string desc = dataGridViewExecute.Rows[index].Cells[9].Value.ToString();
            string belongs = dataGridViewExecute.Rows[index].Cells[7].Value.ToString().Substring(2,6);


            if (status.Contains("Not"))  // not started
            {
                da.Update(id);
                /*                string year = DateTime.Now.Year.ToString();
                                string month = DateTime.Now.Month.ToString();

                                switch (month)

                                {
                                    case "1":
                                        month = "Jan";
                                        break;
                                    case "2":
                                        month = "Feb";
                                        break;
                                    case "3":
                                        month = "Mar";
                                        break;
                                    case "4":
                                        month = "Apr";
                                        break;
                                    case "5":
                                        month = "May";
                                        break;
                                    case "6":
                                        month = "June";
                                        break;
                                    case "7":
                                        month = "July";
                                        break;
                                    case "8":
                                        month = "Aug";
                                        break;
                                    case "9":
                                        month = "Sep";
                                        break;
                                    case "10":
                                        month = "Oct";
                                        break;
                                    case "11":
                                        month = "Nov";
                                        break;
                                    case "12":
                                        month = "Dec";
                                        break;
                                    default:
                                        month = "Jan";
                                        break;
                                }
                                string yearMonthCode = da.GetYearMonthCode(year, month).ToString();*/
                SNModel item = new SNModel();
                int a=0;
                try
                {
                    a = int.Parse(orderNumber);
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                }

                for (int i = 0; i < totalQuantity; i++)
                {
                    string sn = belongs + (a + i).ToString().PadLeft(7, '0');
                    item.Pro_Id = int.Parse(id);
                    item.SN = sn;
                    item.Status = "Not started";
                    item.LatestModifyTime = DateTime.Now.ToString();
                    item.Model = model;
                    item.OrderNumber = orderNumber;
                    item.Line = line1;
                    item.Description = desc;
                    item.JobNumber = desc+ orderNumber;
                    item.Reserved4 = totalQuantity;

                    da.InsertSN(item);
                }
                string line = comboBoxLine.Text;
                string pronumber = txtProNumber.Text;
                string proStatus = comboBoxStutus.Text;
                DataTable dt = da.GetProDb(line, pronumber, proStatus).Tables[0];
                if (dt.Rows.Count != 0)
                {
                    dataGridViewExecute.DataSource = dt;  //dataSet.Tables[0].DefaultView
                    dataGridViewExecute.Height = (dataGridViewExecute.RowCount + 2) * dataGridViewExecute.Columns[0].HeaderCell.Size.Height;
                    dataGridViewExecute.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridViewExecute.Columns[0].Visible = false;
                    dataGridViewExecute.Visible = true;
                }
                MessageBox.Show("SN已生成！");

            }
            if (status.Contains("Completed"))
            {
                MessageBox.Show("此工单已全部完成！");
            }
            if (status.Contains("progress"))
            {
                MessageBox.Show("此工单正在执行！");
            }

        }

        private void SNQueryMenuItem_Click(object sender, EventArgs e)
        {
            int index = dataGridViewExecute.CurrentRow.Index;
            string id = dataGridViewExecute.Rows[index].Cells[0].Value.ToString();
            string status = dataGridViewExecute.Rows[index].Cells[6].Value.ToString();
            string pn = dataGridViewExecute.Rows[index].Cells[3].Value.ToString();
            if (status.Contains("Not"))
            {
                MessageBox.Show("此工单还未执行，未查询到SN！");
            }
            else
            {
                DataTable dt = da.GetSNbyProId(id).Tables[0];
                if (dt.Rows.Count != 0)
                {
                    dataGridViewSNQuery.DataSource = dt;  //dataSet.Tables[0].DefaultView
                    if (dt.Rows.Count <= 5)
                    {
                        dataGridViewSNQuery.Height = (dataGridViewSNQuery.RowCount + 2) * dataGridViewSNQuery.Columns[0].HeaderCell.Size.Height;
                    }

                    dataGridViewSNQuery.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    labProNumber.Text = "工单：" + pn + " 下的所有SN：";
                    dataGridViewSNQuery.EnableHeadersVisualStyles = false;
                    dataGridViewSNQuery.EnableHeadersVisualStyles = false;
                    dataGridViewSNQuery.ColumnHeadersDefaultCellStyle.BackColor = Color.Orange;
                    dataGridViewSNQuery.Visible = true;
                }
                else
                {
                    MessageBox.Show("未查询到数据");
                }

            }



        }

        private void comboBoxLine_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewExecute_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
