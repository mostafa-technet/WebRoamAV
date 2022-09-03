namespace WebRoamAV
{
    partial class frmVlApplication
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
            this.lblReportF = new System.Windows.Forms.Label();
            this.lblReportF2 = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblDate2 = new System.Windows.Forms.Label();
            this.lblVDBDate = new System.Windows.Forms.Label();
            this.lblTime2 = new System.Windows.Forms.Label();
            this.lblCaption = new System.Windows.Forms.Label();
            this.lblPatch = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.btnReferesh = new System.Windows.Forms.Button();
            this.lblTimer1 = new System.Windows.Forms.Label();
            this.lblTimer2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lblReportF
            // 
            this.lblReportF.AutoSize = true;
            this.lblReportF.Location = new System.Drawing.Point(12, 24);
            this.lblReportF.Name = "lblReportF";
            this.lblReportF.Size = new System.Drawing.Size(60, 13);
            this.lblReportF.TabIndex = 0;
            this.lblReportF.Text = "Report For:";
            // 
            // lblReportF2
            // 
            this.lblReportF2.AutoSize = true;
            this.lblReportF2.Location = new System.Drawing.Point(111, 24);
            this.lblReportF2.Name = "lblReportF2";
            this.lblReportF2.Size = new System.Drawing.Size(234, 13);
            this.lblReportF2.TabIndex = 0;
            this.lblReportF2.Text = "Vulnerabilities found in Applications and Settings";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(12, 83);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(33, 13);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "Date:";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(12, 147);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(33, 13);
            this.lblTime.TabIndex = 0;
            this.lblTime.Text = "Time:";
            // 
            // lblDate2
            // 
            this.lblDate2.AutoSize = true;
            this.lblDate2.Location = new System.Drawing.Point(111, 83);
            this.lblDate2.Name = "lblDate2";
            this.lblDate2.Size = new System.Drawing.Size(65, 13);
            this.lblDate2.TabIndex = 0;
            this.lblDate2.Text = "27/05/2019";
            // 
            // lblVDBDate
            // 
            this.lblVDBDate.AutoSize = true;
            this.lblVDBDate.Location = new System.Drawing.Point(471, 83);
            this.lblVDBDate.Name = "lblVDBDate";
            this.lblVDBDate.Size = new System.Drawing.Size(156, 13);
            this.lblVDBDate.TabIndex = 0;
            this.lblVDBDate.Text = "Virus Database - 05 June  2017";
            // 
            // lblTime2
            // 
            this.lblTime2.AutoSize = true;
            this.lblTime2.Location = new System.Drawing.Point(111, 147);
            this.lblTime2.Name = "lblTime2";
            this.lblTime2.Size = new System.Drawing.Size(49, 13);
            this.lblTime2.TabIndex = 0;
            this.lblTime2.Text = "15:05:58";
            // 
            // lblCaption
            // 
            this.lblCaption.AutoSize = true;
            this.lblCaption.Location = new System.Drawing.Point(471, 24);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(162, 13);
            this.lblCaption.TabIndex = 0;
            this.lblCaption.Text = "Webroam Anti Virus - Version 1.0";
            // 
            // lblPatch
            // 
            this.lblPatch.AutoSize = true;
            this.lblPatch.Location = new System.Drawing.Point(12, 495);
            this.lblPatch.Name = "lblPatch";
            this.lblPatch.Size = new System.Drawing.Size(518, 13);
            this.lblPatch.TabIndex = 1;
            this.lblPatch.Text = "if patch is not available, you can consider upgrading the application or contact " +
    "support of application vendor.";
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(572, 532);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(15, 186);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(632, 275);
            this.webBrowser1.TabIndex = 4;
            // 
            // btnReferesh
            // 
            this.btnReferesh.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnReferesh.Location = new System.Drawing.Point(474, 137);
            this.btnReferesh.Name = "btnReferesh";
            this.btnReferesh.Size = new System.Drawing.Size(153, 23);
            this.btnReferesh.TabIndex = 5;
            this.btnReferesh.Text = "Refresh";
            this.btnReferesh.UseVisualStyleBackColor = true;
            this.btnReferesh.Click += new System.EventHandler(this.btnReferesh_Click);
            // 
            // lblTimer1
            // 
            this.lblTimer1.AutoSize = true;
            this.lblTimer1.Location = new System.Drawing.Point(266, 137);
            this.lblTimer1.Name = "lblTimer1";
            this.lblTimer1.Size = new System.Drawing.Size(36, 13);
            this.lblTimer1.TabIndex = 6;
            this.lblTimer1.Text = "Timer:";
            this.lblTimer1.Visible = false;
            // 
            // lblTimer2
            // 
            this.lblTimer2.AutoSize = true;
            this.lblTimer2.Location = new System.Drawing.Point(309, 136);
            this.lblTimer2.Name = "lblTimer2";
            this.lblTimer2.Size = new System.Drawing.Size(0, 13);
            this.lblTimer2.TabIndex = 7;
            this.lblTimer2.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmVlApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(675, 567);
            this.Controls.Add(this.lblTimer2);
            this.Controls.Add(this.lblTimer1);
            this.Controls.Add(this.btnReferesh);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblPatch);
            this.Controls.Add(this.lblCaption);
            this.Controls.Add(this.lblReportF2);
            this.Controls.Add(this.lblTime2);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblVDBDate);
            this.Controls.Add(this.lblDate2);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblReportF);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmVlApplication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Application and Settings Vulnerability Scan ";
            this.Load += new System.EventHandler(this.frmVlApplication_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblReportF;
        private System.Windows.Forms.Label lblReportF2;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblDate2;
        private System.Windows.Forms.Label lblVDBDate;
        private System.Windows.Forms.Label lblTime2;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.Label lblPatch;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button btnReferesh;
        private System.Windows.Forms.Label lblTimer1;
        private System.Windows.Forms.Label lblTimer2;
        private System.Windows.Forms.Timer timer1;
    }
}