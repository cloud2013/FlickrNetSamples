namespace DLR.Flickr.Driver
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
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.btn = new System.Windows.Forms.Button();
            this.btnDaily = new System.Windows.Forms.Button();
            this.btnWeek = new System.Windows.Forms.Button();
            this.btnMonth = new System.Windows.Forms.Button();
            this.btnTotal = new System.Windows.Forms.Button();
            this.btnDailyTotal = new System.Windows.Forms.Button();
            this.btnCombo = new System.Windows.Forms.Button();
            this.btnReadFlickr = new System.Windows.Forms.Button();
            this.btnClearBrowser = new System.Windows.Forms.Button();
            this.txtbxBasePath = new System.Windows.Forms.TextBox();
            this.lblBasePath = new System.Windows.Forms.Label();
            this.btnChangeBasePath = new System.Windows.Forms.Button();
            this.txtbxStatus = new System.Windows.Forms.TextBox();
            this.btnDBMan = new System.Windows.Forms.Button();
            this.btnMaxCount = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(12, 41);
            this.webBrowser1.MaximumSize = new System.Drawing.Size(800, 900);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(800, 650);
            this.webBrowser1.TabIndex = 2;
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(12, 12);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(80, 23);
            this.btn.TabIndex = 3;
            this.btn.Text = "Refresh ALL";
            this.btn.UseVisualStyleBackColor = true;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnDaily
            // 
            this.btnDaily.Location = new System.Drawing.Point(425, 12);
            this.btnDaily.Name = "btnDaily";
            this.btnDaily.Size = new System.Drawing.Size(65, 23);
            this.btnDaily.TabIndex = 4;
            this.btnDaily.Text = "Today";
            this.btnDaily.UseVisualStyleBackColor = true;
            this.btnDaily.Click += new System.EventHandler(this.btnDaily_Click);
            // 
            // btnWeek
            // 
            this.btnWeek.Location = new System.Drawing.Point(496, 12);
            this.btnWeek.Name = "btnWeek";
            this.btnWeek.Size = new System.Drawing.Size(75, 23);
            this.btnWeek.TabIndex = 5;
            this.btnWeek.Text = "Week";
            this.btnWeek.UseVisualStyleBackColor = true;
            this.btnWeek.Click += new System.EventHandler(this.btnWeek_Click);
            // 
            // btnMonth
            // 
            this.btnMonth.Location = new System.Drawing.Point(577, 12);
            this.btnMonth.Name = "btnMonth";
            this.btnMonth.Size = new System.Drawing.Size(75, 23);
            this.btnMonth.TabIndex = 6;
            this.btnMonth.Text = "Month";
            this.btnMonth.UseVisualStyleBackColor = true;
            this.btnMonth.Click += new System.EventHandler(this.btnMonth_Click);
            // 
            // btnTotal
            // 
            this.btnTotal.Location = new System.Drawing.Point(658, 12);
            this.btnTotal.Name = "btnTotal";
            this.btnTotal.Size = new System.Drawing.Size(75, 23);
            this.btnTotal.TabIndex = 7;
            this.btnTotal.Text = "Total";
            this.btnTotal.UseVisualStyleBackColor = true;
            this.btnTotal.Click += new System.EventHandler(this.btnTotal_Click);
            // 
            // btnDailyTotal
            // 
            this.btnDailyTotal.Location = new System.Drawing.Point(739, 12);
            this.btnDailyTotal.Name = "btnDailyTotal";
            this.btnDailyTotal.Size = new System.Drawing.Size(75, 23);
            this.btnDailyTotal.TabIndex = 8;
            this.btnDailyTotal.Text = "Daily Totals";
            this.btnDailyTotal.UseVisualStyleBackColor = true;
            this.btnDailyTotal.Click += new System.EventHandler(this.btnDailyTotal_Click);
            // 
            // btnCombo
            // 
            this.btnCombo.Location = new System.Drawing.Point(214, 12);
            this.btnCombo.Name = "btnCombo";
            this.btnCombo.Size = new System.Drawing.Size(102, 23);
            this.btnCombo.TabIndex = 9;
            this.btnCombo.Text = "Stats + HTML";
            this.btnCombo.UseVisualStyleBackColor = true;
            this.btnCombo.Click += new System.EventHandler(this.btnCombo_Click);
            // 
            // btnReadFlickr
            // 
            this.btnReadFlickr.Location = new System.Drawing.Point(133, 12);
            this.btnReadFlickr.Name = "btnReadFlickr";
            this.btnReadFlickr.Size = new System.Drawing.Size(75, 23);
            this.btnReadFlickr.TabIndex = 10;
            this.btnReadFlickr.Text = "Read Flickr";
            this.btnReadFlickr.UseVisualStyleBackColor = true;
            this.btnReadFlickr.Click += new System.EventHandler(this.btnReadFlickr_Click);
            // 
            // btnClearBrowser
            // 
            this.btnClearBrowser.Location = new System.Drawing.Point(878, 221);
            this.btnClearBrowser.Name = "btnClearBrowser";
            this.btnClearBrowser.Size = new System.Drawing.Size(75, 23);
            this.btnClearBrowser.TabIndex = 11;
            this.btnClearBrowser.Text = "Clear ";
            this.btnClearBrowser.UseVisualStyleBackColor = true;
            this.btnClearBrowser.Click += new System.EventHandler(this.btnClearBrowser_Click);
            // 
            // txtbxBasePath
            // 
            this.txtbxBasePath.Location = new System.Drawing.Point(878, 15);
            this.txtbxBasePath.Name = "txtbxBasePath";
            this.txtbxBasePath.Size = new System.Drawing.Size(170, 20);
            this.txtbxBasePath.TabIndex = 12;
            this.txtbxBasePath.ModifiedChanged += new System.EventHandler(this.txtbxBasePath_ModifiedChanged);
            this.txtbxBasePath.TextChanged += new System.EventHandler(this.txtbxBasePath_TextChanged);
            // 
            // lblBasePath
            // 
            this.lblBasePath.AutoSize = true;
            this.lblBasePath.Location = new System.Drawing.Point(820, 22);
            this.lblBasePath.Name = "lblBasePath";
            this.lblBasePath.Size = new System.Drawing.Size(52, 13);
            this.lblBasePath.TabIndex = 13;
            this.lblBasePath.Text = "Basepath";
            // 
            // btnChangeBasePath
            // 
            this.btnChangeBasePath.Location = new System.Drawing.Point(1054, 13);
            this.btnChangeBasePath.Name = "btnChangeBasePath";
            this.btnChangeBasePath.Size = new System.Drawing.Size(96, 23);
            this.btnChangeBasePath.TabIndex = 14;
            this.btnChangeBasePath.Text = "Change DB Path";
            this.btnChangeBasePath.UseVisualStyleBackColor = true;
            this.btnChangeBasePath.Click += new System.EventHandler(this.btnChangeBasePath_Click);
            // 
            // txtbxStatus
            // 
            this.txtbxStatus.Location = new System.Drawing.Point(878, 42);
            this.txtbxStatus.Multiline = true;
            this.txtbxStatus.Name = "txtbxStatus";
            this.txtbxStatus.Size = new System.Drawing.Size(272, 153);
            this.txtbxStatus.TabIndex = 15;
            // 
            // btnDBMan
            // 
            this.btnDBMan.Location = new System.Drawing.Point(878, 201);
            this.btnDBMan.Name = "btnDBMan";
            this.btnDBMan.Size = new System.Drawing.Size(75, 23);
            this.btnDBMan.TabIndex = 16;
            this.btnDBMan.Text = "DB Main";
            this.btnDBMan.UseVisualStyleBackColor = true;
            this.btnDBMan.Click += new System.EventHandler(this.btnDBMan_Click);
            // 
            // btnMaxCount
            // 
            this.btnMaxCount.Location = new System.Drawing.Point(1054, 701);
            this.btnMaxCount.Name = "btnMaxCount";
            this.btnMaxCount.Size = new System.Drawing.Size(75, 23);
            this.btnMaxCount.TabIndex = 17;
            this.btnMaxCount.Text = "Show Max";
            this.btnMaxCount.UseVisualStyleBackColor = true;
            this.btnMaxCount.Visible = false;
            this.btnMaxCount.Click += new System.EventHandler(this.btnMaxCount_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1162, 768);
            this.Controls.Add(this.btnMaxCount);
            this.Controls.Add(this.btnDBMan);
            this.Controls.Add(this.txtbxStatus);
            this.Controls.Add(this.btnChangeBasePath);
            this.Controls.Add(this.lblBasePath);
            this.Controls.Add(this.txtbxBasePath);
            this.Controls.Add(this.btnClearBrowser);
            this.Controls.Add(this.btnReadFlickr);
            this.Controls.Add(this.btnCombo);
            this.Controls.Add(this.btnDailyTotal);
            this.Controls.Add(this.btnTotal);
            this.Controls.Add(this.btnMonth);
            this.Controls.Add(this.btnWeek);
            this.Controls.Add(this.btnDaily);
            this.Controls.Add(this.btn);
            this.Controls.Add(this.webBrowser1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.Button btnDaily;
        private System.Windows.Forms.Button btnWeek;
        private System.Windows.Forms.Button btnMonth;
        private System.Windows.Forms.Button btnTotal;
        private System.Windows.Forms.Button btnDailyTotal;
        private System.Windows.Forms.Button btnCombo;
        private System.Windows.Forms.Button btnReadFlickr;
        private System.Windows.Forms.Button btnClearBrowser;
        private System.Windows.Forms.TextBox txtbxBasePath;
        private System.Windows.Forms.Label lblBasePath;
        private System.Windows.Forms.Button btnChangeBasePath;
        private System.Windows.Forms.TextBox txtbxStatus;
        private System.Windows.Forms.Button btnDBMan;
        private System.Windows.Forms.Button btnMaxCount;
    }
}

