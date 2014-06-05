using DLR.Statistics;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace DLR.FlickrHTMLWriter
{

   
    class Program
    {
        
        //HTMLWriter
        static void Main(string[] args)
        {
            int _DisplayLimit = 25;
            CWorker.BasePath = @"C:\TEMP\";
            CWorker.DataBaseRootName = "FLICKRDB";
            string xtractBaseName = "XTRACT";
            CXDB xDB = CWorker.ReadXDB(xtractBaseName);
            if (xDB == null)
            {
                Console.WriteLine("No X Database.");
                System.Environment.Exit(1);
            }
            #region  Today
            string fqFile = CWorker.BasePath + "TODAY.HTML";
            System.IO.File.WriteAllText(fqFile, _Today(xDB,_DisplayLimit));
            #endregion
            #region TOTAL
            fqFile = CWorker.BasePath + "Total.HTML";
            System.IO.File.WriteAllText(fqFile, _Total(xDB, _DisplayLimit));
            #endregion
            #region Sample
            fqFile = CWorker.BasePath + "Week.HTML";
            System.IO.File.WriteAllText(fqFile, _Week(xDB, _DisplayLimit));
            #endregion
            #region TOTALS
            fqFile = CWorker.BasePath + "DailyTotals.HTML";
            System.IO.File.WriteAllText(fqFile, _MakeDailyTotals(xDB));
            #endregion
        }
        static string _MakeDailyTotals(CXDB xDB)
        {
            StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine(_CSS());
            sb.AppendLine("<body>");
            sb.AppendLine(string.Format("<p>Total Views By Day</h2></p>"));
            sb.AppendLine(_DailyTotalsTable(xDB));
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }
        static string _DailyTotalsTable(CXDB xDB)
        {
            StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<table>");
            int prior = -1;
            foreach (CTotalViews record in xDB.Totals)
            {
                if (prior !=-1)
                {
                    sb.AppendLine(_MakeRowViewTotals(record,prior));
                }
                else
                {
                    sb.AppendLine(_MakeRowViewTotals(record));
                }

                prior = record.Views;
            }
            sb.AppendLine("</table>");
            return sb.ToString();
        }
        static string _MakeRowViewTotals(CTotalViews record)
        {
            return string.Format("<tr><td>{0}</td><td>{1:#,#}</td><td></td></tr>", CWorker.FormatDateString(record.Date),record.Views);
        }
        static string _MakeRowViewTotals(CTotalViews record, int prior)
        {
            return string.Format("<tr><td>{0}</td><td>{1:#,#}</td><td>{2:#,#}</td></tr>",CWorker.FormatDateString( record.Date), record.Views, record.Views - prior);
        }
        static string _MakeRow(CData record)
        {
            string img = string.Format("<img src='{0}'>", record.MetaData.ThumbURL);
            string href = string.Format("<a href='{1}'>{0}</a>", record.MetaData.Title, record.MetaData.LargeURL);
            return string.Format("<tr><td>{3}</td><td>{0}</td><td>{1:#,#}</td><td>{2}</td></tr>", href, record.Total, record.Today, img);
        }
        static string _MakeRowSample(CData record)
        {
            string img = string.Format("<img src='{0}'>", record.MetaData.ThumbURL);
            string href = string.Format("<a href='{1}'>{0}</a>", record.MetaData.Title, record.MetaData.LargeURL);
            return string.Format("<tr><td>{3}</td><td>{0}</td><td>{1:#,#}</td><td>{2}</td></tr>", href, record.Total, record.Week, img);
        }
        static string _Week(CXDB xDB, int limit)
        {
            StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine(_CSS());
            sb.AppendLine("<body>");
            sb.AppendLine(string.Format("<p><h2>Top {0} Total Views Last {1} Days ({2})</h2></p>", limit, xDB.Date.WeekDays,xDB.Date.Week));
            sb.AppendLine(_TableWeek((from x in xDB.Data orderby x.Week descending select x).ToList(), limit));
            sb.AppendLine(string.Format("<p><h2>Pictures With Views in this Sample: {0}</h2></p>", CWorker.FormatInt(_SampleSum(xDB))));
            sb.AppendLine(string.Format("<p><h2>Total Views during Sample: {0}</h2></p>", CWorker.FormatInt(xDB.Views.Week)));
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }
        
        static string _Total(CXDB xDB, int limit)
        {
            StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine(_CSS());
            sb.AppendLine("<body>");
            sb.AppendLine(string.Format("<p><h2>Top {0} Total Views as of {1}</h2></p>", limit, xDB.Date.Today));
            sb.AppendLine(_Table((from x in xDB.Data orderby x.Total descending select x).ToList(), limit));
            sb.AppendLine(string.Format("<p><h2>Pictures With Views: {0:#,#}</h2></p>", CWorker.FormatInt(_TotalSum(xDB))));
            sb.AppendLine(string.Format("<p><h2>Total Views: {0:#,#}</h2></p>", CWorker.FormatInt(xDB.Views.Total)));
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }
        static string _Today(CXDB xDB, int limit)
        {
            StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine(_CSS());
            sb.AppendLine("<body>");
            sb.AppendLine(string.Format("<p><h2>Top {0} Views Today: {1}</h2></p>", limit, xDB.Date.Today));
            sb.AppendLine(_Table((from x in xDB.Data orderby x.Today descending select x).ToList(), limit));
            sb.AppendLine(string.Format("<p><h2>Pictures With Views: {0:#,#}</h2></p>", CWorker.FormatInt(_TodaySum(xDB))));
            sb.AppendLine(string.Format("<p><h2>Views Today: {0:#,#}</h2></p>", CWorker.FormatInt(xDB.Views.Today)));
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }
        static int _SampleSum(CXDB xDB)
        {
            int hits = 0;
            foreach (CData record in xDB.Data)
            {
                if (record.Week == 0)
                {
                    continue;
                }
                hits++;
            }
            return hits;
        }
        static int _TodaySum(CXDB xDB)
        {
            int hits = 0;
            foreach (CData record in xDB.Data)
            {
                if (record.Today == 0)
                {
                    continue;
                }
                hits++;
            }
            return hits;
        }
        static int _TotalSum(CXDB xDB)
        {
            int hits = 0;
            foreach (CData record in xDB.Data)
            {
                if (record.Total == 0)
                {
                    continue;
                }
                hits++;
            }
            return hits;
        }
        static string _Table(List<CData> data, int limit)
        {
            StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<table>");
            int hits = 0;
            foreach (CData record in data)
            {
                if (record.Total == 0)
                {
                    continue;
                }
                hits++;
                if (hits < limit)
                {
                    sb.AppendLine(_MakeRow(record));
                }
            }
            sb.AppendLine("</table>");
            return sb.ToString();
        }
        static string _TableWeek(List<CData> data, int limit)
        {
            StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<table>");
            int hits = 0;
            foreach (CData record in data)
            {
                if (record.Total == 0)
                {
                    continue;
                }
                hits++;
                if (hits < limit)
                {
                    sb.AppendLine(_MakeRowSample(record));
                }
            }
            sb.AppendLine("</table>");
            return sb.ToString();
        }
        static string _CSS()
        {
            StringBuilder sb=new StringBuilder();
           sb.AppendLine("<head>");
           sb.AppendLine("<style>");
            sb.AppendLine("h2 { color: blue;  text-align: left; font-size: 24pt;}");
            sb.AppendLine("table,td,th {border: 1px solid black;}");
            sb.AppendLine("td {padding:15px;text-align:right;}");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            return sb.ToString();
        }
        
        }
    }

