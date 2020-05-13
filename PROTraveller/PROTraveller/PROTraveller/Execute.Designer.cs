namespace PROTraveller
{
    partial class Execute
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labLine = new System.Windows.Forms.Label();
            this.comboBoxLine = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtProNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxStutus = new System.Windows.Forms.ComboBox();
            this.dataGridViewExecute = new System.Windows.Forms.DataGridView();
            this.ContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SNQueryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnQuery = new System.Windows.Forms.Button();
            this.dataGridViewSNQuery = new System.Windows.Forms.DataGridView();
            this.labProNumber = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExecute)).BeginInit();
            this.ContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSNQuery)).BeginInit();
            this.SuspendLayout();
            // 
            // labLine
            // 
            this.labLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labLine.Location = new System.Drawing.Point(34, 51);
            this.labLine.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labLine.Name = "labLine";
            this.labLine.Size = new System.Drawing.Size(50, 26);
            this.labLine.TabIndex = 1;
            this.labLine.Text = "Line";
            // 
            // comboBoxLine
            // 
            this.comboBoxLine.FormattingEnabled = true;
            this.comboBoxLine.Items.AddRange(new object[] {
            "B4 & B5",
            "SH7&SH11",
            "Orbital motor",
            ""});
            this.comboBoxLine.Location = new System.Drawing.Point(119, 52);
            this.comboBoxLine.Name = "comboBoxLine";
            this.comboBoxLine.Size = new System.Drawing.Size(121, 28);
            this.comboBoxLine.TabIndex = 3;
            this.comboBoxLine.SelectedIndexChanged += new System.EventHandler(this.comboBoxLine_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(277, 51);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 26);
            this.label2.TabIndex = 4;
            this.label2.Text = "Order Number";
            // 
            // txtProNumber
            // 
            this.txtProNumber.Location = new System.Drawing.Point(401, 52);
            this.txtProNumber.Name = "txtProNumber";
            this.txtProNumber.Size = new System.Drawing.Size(111, 27);
            this.txtProNumber.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(533, 53);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 26);
            this.label3.TabIndex = 6;
            this.label3.Text = "Status";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // comboBoxStutus
            // 
            this.comboBoxStutus.FormattingEnabled = true;
            this.comboBoxStutus.Items.AddRange(new object[] {
            "Not started",
            "In progress",
            "Completed",
            ""});
            this.comboBoxStutus.Location = new System.Drawing.Point(631, 52);
            this.comboBoxStutus.Name = "comboBoxStutus";
            this.comboBoxStutus.Size = new System.Drawing.Size(121, 28);
            this.comboBoxStutus.TabIndex = 7;
            // 
            // dataGridViewExecute
            // 
            this.dataGridViewExecute.ContextMenuStrip = this.ContextMenu;
            this.dataGridViewExecute.Location = new System.Drawing.Point(12, 126);
            this.dataGridViewExecute.MultiSelect = false;
            this.dataGridViewExecute.Name = "dataGridViewExecute";
            this.dataGridViewExecute.ReadOnly = true;
            this.dataGridViewExecute.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewExecute.Size = new System.Drawing.Size(1337, 316);
            this.dataGridViewExecute.TabIndex = 8;
            this.dataGridViewExecute.Visible = false;
            this.dataGridViewExecute.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewExecute_CellContentClick);
            this.dataGridViewExecute.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewExecute_CellMouseDown);
            // 
            // ContextMenu
            // 
            this.ContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem,
            this.SNQueryMenuItem});
            this.ContextMenu.Name = "menuExecute";
            this.ContextMenu.Size = new System.Drawing.Size(125, 48);
            this.ContextMenu.Text = "Execute";
            // 
            // toolStripMenuItem
            // 
            this.toolStripMenuItem.Name = "toolStripMenuItem";
            this.toolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem.Text = "工单执行";
            this.toolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItem_Click);
            // 
            // SNQueryMenuItem
            // 
            this.SNQueryMenuItem.Name = "SNQueryMenuItem";
            this.SNQueryMenuItem.Size = new System.Drawing.Size(124, 22);
            this.SNQueryMenuItem.Text = "SN查询";
            this.SNQueryMenuItem.Click += new System.EventHandler(this.SNQueryMenuItem_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(824, 51);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 32);
            this.btnQuery.TabIndex = 10;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // dataGridViewSNQuery
            // 
            this.dataGridViewSNQuery.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSNQuery.Location = new System.Drawing.Point(12, 513);
            this.dataGridViewSNQuery.MultiSelect = false;
            this.dataGridViewSNQuery.Name = "dataGridViewSNQuery";
            this.dataGridViewSNQuery.ReadOnly = true;
            this.dataGridViewSNQuery.Size = new System.Drawing.Size(705, 127);
            this.dataGridViewSNQuery.TabIndex = 11;
            this.dataGridViewSNQuery.Visible = false;
            // 
            // labProNumber
            // 
            this.labProNumber.AutoSize = true;
            this.labProNumber.Location = new System.Drawing.Point(12, 479);
            this.labProNumber.Name = "labProNumber";
            this.labProNumber.Size = new System.Drawing.Size(0, 22);
            this.labProNumber.TabIndex = 12;
            // 
            // Execute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.labProNumber);
            this.Controls.Add(this.dataGridViewSNQuery);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.dataGridViewExecute);
            this.Controls.Add(this.comboBoxStutus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtProNumber);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxLine);
            this.Controls.Add(this.labLine);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Execute";
            this.Text = "Execute";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExecute)).EndInit();
            this.ContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSNQuery)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labLine;
        private System.Windows.Forms.ComboBox comboBoxLine;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtProNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxStutus;
        private System.Windows.Forms.DataGridView dataGridViewExecute;
        private System.Windows.Forms.ContextMenuStrip ContextMenu;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SNQueryMenuItem;
        private System.Windows.Forms.DataGridView dataGridViewSNQuery;
        private System.Windows.Forms.Label labProNumber;
    }
}