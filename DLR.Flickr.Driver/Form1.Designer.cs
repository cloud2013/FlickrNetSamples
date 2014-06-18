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
            this.btnStatistics = new System.Windows.Forms.Button();
            this.btnHTML = new System.Windows.Forms.Button();
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
            this.SuspendLayout();
            // 
            // btnStatistics
            // 
            this.btnStatistics.Location = new System.Drawing.Point(1014, 12);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(65, 23);
            this.btnStatistics.TabIndex = 0;
            this.btnStatistics.Text = "Statistics";
            this.btnStatistics.UseVisualStyleBackColor = true;
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // btnHTML
            // 
            this.btnHTML.Location = new System.Drawing.Point(1085, 12);
            this.btnHTML.Name = "btnHTML";
            this.btnHTML.Size = new System.Drawing.Size(65, 23);
            this.btnHTML.TabIndex = 1;
            this.btnHTML.Text = "HTML";
            this.btnHTML.UseVisualStyleBackColor = true;
            this.btnHTML.Click += new System.EventHandler(this.btnHTML_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(12, 41);
            this.webBrowser1.MaximumSize = new System.Drawing.Size(800, 900);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(800, 677);
            this.webBrowser1.TabIndex = 2;
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(12, 12);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(80, 23);
            this.btn.TabIndex = 3;
            this.btn.Text = "Do All";
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
            this.btnClearBrowser.Location = new System.Drawing.Point(933, 12);
            this.btnClearBrowser.Name = "btnClearBrowser";
            this.btnClearBrowser.Size = new System.Drawing.Size(75, 23);
            this.btnClearBrowser.TabIndex = 11;
            this.btnClearBrowser.Text = "Clear ";
            this.btnClearBrowser.UseVisualStyleBackColor = true;
            this.btnClearBrowser.Click += new System.EventHandler(this.btnClearBrowser_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1162, 768);
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
            this.Controls.Add(this.btnHTML);
            this.Controls.Add(this.btnStatistics);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStatistics;
        private System.Windows.Forms.Button btnHTML;
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
    }
}

