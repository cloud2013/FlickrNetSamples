using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DLR.Flickr.Statistics.Version2;
using DLR.Statistics;
using DLR.FlickrHTMLWriter;
using DLR.FlickrViews;

namespace DLR.Flickr.Driver
{
    public partial class Form1 : Form
    {

        string _BasePath=@"C:\temp\";
        //string _BasePath = System.IO.Directory.GetCurrentDirectory()+@"\";
        string _BaseName = "FLICKRDB";
        
       
        public Form1()
        {
            
            InitializeComponent();
            var z = Screen.PrimaryScreen.Bounds.Size;
            this.MinimumSize = z;
            this.MaximumSize = z;
            this.WindowState = FormWindowState.Maximized;
            webBrowser1.Size = z;
            txtbxBasePath.Text = _BasePath;
            CWorker.DataBaseRootName = _BaseName;
            CWorker.BasePath = _BasePath;
        }
        private void btnStatistics_Click(object sender, EventArgs e)
        {
            CStatistics xStatistics = new CStatistics();
            CXDB xDB= xStatistics.Exec();
            xStatistics.Commit();
            MessageBox.Show("Statistic Generation Completed.");
        }
        private void btnHTML_Click(object sender, EventArgs e)
        {
            CHTML html = new CHTML(25);
            html.Exec();
            btnDaily.PerformClick();
        }
        private void btnDaily_Click(object sender, EventArgs e)
        {
            CWorker.BasePath = _BasePath;
            string uriString = CWorker.BasePath + "TODAY.HTML";
            webBrowser1.Navigate(uriString);
            
        }
        private void btnWeek_Click(object sender, EventArgs e)
        {
            CWorker.BasePath = _BasePath;
            string uriString = CWorker.BasePath + "Week.HTML";
            webBrowser1.Navigate(uriString);
        }
        private void btnMonth_Click(object sender, EventArgs e)
        {
            CWorker.BasePath = _BasePath;
            string uriString = CWorker.BasePath + "Month.HTML";
            webBrowser1.Navigate(uriString);
        }
        private void btnTotal_Click(object sender, EventArgs e)
        {
            CWorker.BasePath = _BasePath;
            string uriString = CWorker.BasePath + "Total.HTML";
            webBrowser1.Navigate(uriString);
        }
        private void btnDailyTotal_Click(object sender, EventArgs e)
        {
            CWorker.BasePath = _BasePath;
            string uriString = CWorker.BasePath + "DailyTotals.HTML";
            webBrowser1.Navigate(uriString);
        }
        private void btnCombo_Click(object sender, EventArgs e)
        {
            txtbxStatus.Text = "Calculate Statistics and Create HTML";
            txtbxStatus.Refresh();
            CStatistics xStatistics = new CStatistics();
            CXDB xDB = xStatistics.Exec();
            txtbxStatus.Text += System.Environment.NewLine + "Max Statistics Collected per Photo: " + xDB.MaxCount;
            //xStatistics.Commit();
            CHTML html = new CHTML(xDB,25);
            html.Exec();
            //MessageBox.Show("Ready.","Statistics plus HTML",MessageBoxButtons.OK);
            btnDaily.PerformClick();
        }
        private void btn_Click(object sender, EventArgs e)
        {
            btnClearBrowser.PerformClick();
            CWorker.BasePath = _BasePath;
            txtbxStatus.Text = "Read Flickr Data";
            txtbxStatus.Refresh();
           
            CReadFlickr reader = new CReadFlickr();
            CDB cDB = reader.Exec();
            txtbxStatus.Text += System.Environment.NewLine + "Update DB Store";
            txtbxStatus.Refresh();
            reader.Commit();
            txtbxStatus.Text += System.Environment.NewLine + "Calculate Statistics";
            txtbxStatus.Refresh();
            CStatistics xStatistics = new CStatistics(cDB);
            CXDB xDB = xStatistics.Exec();
            txtbxStatus.Text += System.Environment.NewLine + "Max Statistics Collected per Photo: " + xDB.MaxCount;
            CHTML html = new CHTML(xDB, 25);
            txtbxStatus.Text += System.Environment.NewLine + "Create HTML";
            txtbxStatus.Refresh();
            html.Exec();
            //MessageBox.Show("Ready.", "Read Flickr plus Statistics plus HTML", MessageBoxButtons.OK);
            btnDaily.PerformClick();
        }

        private void btnReadFlickr_Click(object sender, EventArgs e)
        {
            txtbxStatus.Text = "Read Flickr Data";
            txtbxStatus.Refresh();
            btnClearBrowser.PerformClick();
            CReadFlickr reader = new CReadFlickr();
            CDB cDB = reader.Exec();
            reader.Commit();
            MessageBox.Show("Ready.", "Read Flickr", MessageBoxButtons.OK);

        }

        private void btnClearBrowser_Click(object sender, EventArgs e)
        {
            string uriString = @"http://images6.fanpop.com/image/photos/33100000/Rainbow-Dash-my-little-pony-friendship-is-magic-rainbow-dash-33121844-640-585.png";
            webBrowser1.Navigate(uriString);
            webBrowser1.Refresh();
       
        }

        private void txtbxBasePath_ModifiedChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("You are in the TextBoxBase.ModifiedChanged event.");
        }

        private void txtbxBasePath_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("You are in the TextBoxBase.TextChanged event.");
        }

        private void btnChangeBasePath_Click(object sender, EventArgs e)
        {
            _BasePath = txtbxBasePath.Text;
            MessageBox.Show("Base Path Changed: " + _BasePath);

        }

        private void btnDBMan_Click(object sender, EventArgs e)
        {
            CDBMan dbMan = new CDBMan();
            if (dbMan.KillDates("20140624"))
            //if (dbMan.TrimDataBase(32,5))
            {
                dbMan.Commit();
            }
        }

        private void btnMaxCount_Click(object sender, EventArgs e)
        {
            //
        }
    }
}
