namespace JobSearcher_16._11
{
    partial class Form1
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
            this.btn_JobMaster = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.l_pageID = new System.Windows.Forms.Label();
            this.l_status = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_JobMaster
            // 
            this.btn_JobMaster.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btn_JobMaster.Location = new System.Drawing.Point(23, 608);
            this.btn_JobMaster.Name = "btn_JobMaster";
            this.btn_JobMaster.Size = new System.Drawing.Size(173, 29);
            this.btn_JobMaster.TabIndex = 0;
            this.btn_JobMaster.Text = "JobMaster";
            this.btn_JobMaster.UseVisualStyleBackColor = true;
            this.btn_JobMaster.Click += new System.EventHandler(this.btn_JobMaster_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1210, 617);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(142, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "Enter New Page Number";
            //this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // l_pageID
            // 
            this.l_pageID.AutoSize = true;
            this.l_pageID.Location = new System.Drawing.Point(1070, 623);
            this.l_pageID.Name = "l_pageID";
            this.l_pageID.Size = new System.Drawing.Size(0, 13);
            this.l_pageID.TabIndex = 3;
            // 
            // l_status
            // 
            this.l_status.AutoSize = true;
            this.l_status.Location = new System.Drawing.Point(856, 623);
            this.l_status.Name = "l_status";
            this.l_status.Size = new System.Drawing.Size(0, 13);
            this.l_status.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1375, 649);
            this.Controls.Add(this.l_status);
            this.Controls.Add(this.l_pageID);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btn_JobMaster);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_JobMaster;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label l_pageID;
        private System.Windows.Forms.Label l_status;
    }
}

