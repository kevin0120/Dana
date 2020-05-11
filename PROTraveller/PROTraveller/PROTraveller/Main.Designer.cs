namespace PROTraveller
{
    partial class Main
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
            this.tabExecute = new System.Windows.Forms.TabPage();
            this.tabImport = new System.Windows.Forms.TabPage();
            this.tabCtrl = new System.Windows.Forms.TabControl();
            this.tabCtrl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabExecute
            // 
            this.tabExecute.Location = new System.Drawing.Point(4, 22);
            this.tabExecute.Name = "tabExecute";
            this.tabExecute.Padding = new System.Windows.Forms.Padding(3);
            this.tabExecute.Size = new System.Drawing.Size(792, 389);
            this.tabExecute.TabIndex = 1;
            this.tabExecute.Tag = "PROTraveller.Execute";
            this.tabExecute.Text = "执行";
            this.tabExecute.UseVisualStyleBackColor = true;
            this.tabExecute.Click += new System.EventHandler(this.tabExecute_Click);
            // 
            // tabImport
            // 
            this.tabImport.Location = new System.Drawing.Point(4, 22);
            this.tabImport.Name = "tabImport";
            this.tabImport.Padding = new System.Windows.Forms.Padding(3);
            this.tabImport.Size = new System.Drawing.Size(792, 389);
            this.tabImport.TabIndex = 0;
            this.tabImport.Tag = "PROTraveller.Import";
            this.tabImport.Text = "导入";
            this.tabImport.UseVisualStyleBackColor = true;
            // 
            // tabCtrl
            // 
            this.tabCtrl.Controls.Add(this.tabImport);
            this.tabCtrl.Controls.Add(this.tabExecute);
            this.tabCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrl.Location = new System.Drawing.Point(0, 0);
            this.tabCtrl.Name = "tabCtrl";
            this.tabCtrl.SelectedIndex = 0;
            this.tabCtrl.Size = new System.Drawing.Size(800, 415);
            this.tabCtrl.TabIndex = 0;
            this.tabCtrl.SelectedIndexChanged += new System.EventHandler(this.Tab_SelectedIndexChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 415);
            this.Controls.Add(this.tabCtrl);
            this.Name = "Main";
            this.Text = "PRO Traveller";
            this.Load += new System.EventHandler(this.Main_Load);
            this.tabCtrl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabExecute;
        private System.Windows.Forms.TabPage tabImport;
        private System.Windows.Forms.TabControl tabCtrl;
    }
}

