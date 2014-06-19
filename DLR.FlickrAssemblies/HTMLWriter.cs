using DLR.Statistics;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace DLR.FlickrHTMLWriter
{
    public class CHTML
    {
        public const string FMTHREF = "href='https://www.flickr.com/photos/dennisredfield/{0}/in/photostream/'";
        public const string FMTSRC = "src='{0}'";
        int _DisplayLimit = 25;
        CXDB _XDB = null;
        string _XtractBaseName = string.Empty;
        public CHTML(CXDB xDB, int displayLimit)
        {
            _XDB = xDB;
            //CWorker.BasePath = @"C:\TEMP\";
            _DisplayLimit = displayLimit;
        }
        public CHTML(int displayLimit) {
            _DisplayLimit = displayLimit;
            //CWorker.BasePath = @"C:\TEMP\";
            //CWorker.DataBaseRootName = "FLICKRDB";
            _XtractBaseName = "XTRACT";
            _XDB = CWorker.ReadXDB(_XtractBaseName);
        }
        public CHTML(string basePath,string rootName, string baseName,int displayLimit)
        {
            _DisplayLimit = displayLimit;
            CWorker.BasePath = rootName;
            CWorker.DataBaseRootName = rootName;
            _XtractBaseName = baseName;
        }
        public void Exec()
        {
            if (_XDB == null)
            {
                return;
            }
            #region  Today
            string fqFile = CWorker.BasePath + "TODAY.HTML";
            System.IO.File.WriteAllText(fqFile, _Sample(_XDB, CWorker.SampleTypeEnum.Today, _DisplayLimit));// _Today(xDB,_DisplayLimit));
            #endregion
            #region TOTAL
            fqFile = CWorker.BasePath + "Total.HTML";
            System.IO.File.WriteAllText(fqFile, _Sample(_XDB, CWorker.SampleTypeEnum.Total, _DisplayLimit));// _Total(xDB, _DisplayLimit));
            #endregion
            #region Month
            fqFile = CWorker.BasePath + "Month.HTML";
            System.IO.File.WriteAllText(fqFile, _Sample(_XDB, CWorker.SampleTypeEnum.Month, _DisplayLimit));// _Total(xDB, _DisplayLimit));
            #endregion
            #region Sample
            fqFile = CWorker.BasePath + "Week.HTML";
            System.IO.File.WriteAllText(fqFile, _Sample(_XDB, CWorker.SampleTypeEnum.Week, _DisplayLimit));//
            #endregion
            #region TOTALS
            fqFile = CWorker.BasePath + "DailyTotals.HTML";
            System.IO.File.WriteAllText(fqFile, _MakeDailyTotals(_XDB));
            #endregion
        }
        static string _MakeDailyTotals(CXDB xDB)
        {
            StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine(_CSS());
            sb.AppendLine("<body>");
            sb.AppendLine(string.Format("<p><h2>Total Views</h2></p>"));
            sb.AppendLine(_DailyTotalsTable(xDB));
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }
        static string _DailyTotalsTable(CXDB xDB)
        {
            StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine(string.Format("<h3><p>Sample Days: {0}</p></h3>",xDB.Totals.Count -1));
            sb.AppendLine("<table>");
            int prior = -1;
            //(from x in xDB.Totals orderby x.Date descending select x).ToList()
            List<string> strings = new List<string>();
            foreach (CTotalViews record in xDB.Totals)
            {
                strings.Add(_MakeRowViewTotals(record, prior));
                //sb.AppendLine(_MakeRowViewTotals(record, prior));
                prior = record.Views;
            }
            
            for(int ndx=strings.Count-1;ndx!=-1;ndx--)
            {
                sb.AppendLine(strings[ndx]);
            }
            sb.AppendLine("</table>");
            
            return sb.ToString();
        }

        static string _MakeRowViewTotals(CTotalViews record, int prior)
        {
            if (prior != -1)
            {
                return string.Format("<tr><td>{0}</td><td>{1:#,#}</td><td>{2:#,#}</td></tr>", CWorker.FormatDateString(record.Date), record.Views, record.Views - prior);
            }
            return string.Format("<tr><td>{0}</td><td>{1:#,#}</td><td></td></tr>", CWorker.FormatDateString(record.Date), record.Views);
        }
        static string _MakeRow(CData record, int unitCount)
        {
            string href = string.Format(FMTHREF, record.MetaData.ID);
            string src = string.Format(FMTSRC, record.MetaData.ThumbURL);
            string y = string.Empty;
            if (unitCount == 0)
            {
                y = string.Format("<tr><td><img {2}/></td><td><a {3} target='_blank'>{4}</a></td><td>{0:#,#}</td><td></td></tr>", record.Total, unitCount, src, href, record.MetaData.Title);
            }
            else
            {
                y = string.Format("<tr><td><img {2}/></td><td><a {3} target='_blank'>{4}</a></td><td>{0:#,#}</td><td>{1:#,#}</td></tr>", record.Total, unitCount, src, href, record.MetaData.Title);
            }
            return y;
        }
        static string _Sample(CXDB xDB, CWorker.SampleTypeEnum sType, int limit)
        {
            StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine(_CSS());
            sb.AppendLine("<body>");
            string viewsString = string.Empty;
            switch (sType)
            {
                case CWorker.SampleTypeEnum.Month:
                    sb.Append(_HeadLine(xDB.Date.Month, xDB.Pictures.Month, xDB.Views.Month, limit));
                    sb.AppendLine(_Table((from x in xDB.Data orderby x.Month descending select x).ToList(), sType, limit));
                    break;
                case CWorker.SampleTypeEnum.Week:
                    sb.Append(_HeadLine(xDB.Date.Week, xDB.Pictures.Week, xDB.Views.Week, limit));
                    sb.AppendLine(_Table((from x in xDB.Data orderby x.Week descending select x).ToList(), sType, limit));
                    break;
                case CWorker.SampleTypeEnum.Today:
                    sb.Append(_HeadLine(xDB.Date.Today, xDB.Pictures.Today, xDB.Views.Today, limit));
                     sb.AppendLine(_Table((from x in xDB.Data orderby x.Today descending select x).ToList(), sType, limit));
                    break;
                case CWorker.SampleTypeEnum.Total:
                    sb.Append(_HeadLineTotal(xDB, limit));
                    sb.AppendLine(_Table((from x in xDB.Data orderby x.Total descending select x).ToList(), sType, limit));
                    break;
                default:
                    throw new ApplicationException("Program Error");
            }
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }

        static string _HeadLine(string date,int pictures, int views, int limit)
        {
            const string fmt = "<h2>{0}</h2>";
            const string fmtOne = "<p>Top {0} Views For: {1}</p>{2}";
            return string.Format(fmt, string.Format(fmtOne, limit, date, _PicturePhrase(pictures, views)));
        }
        static string _HeadLineTotal(CXDB xdb, int limit)
        {
            const string fmt = "<h2>{0}</h2>";
            const string fmtOne = "<p>Top {0} Views as of: {1}</p>{2}";
            return string.Format(fmt, string.Format(fmtOne, limit, xdb.Date.Today, _PicturePhrase(xdb.Pictures.Total, xdb.Views.Total)));
        }

        static string _PicturePhrase(int pictures, int views)
        {
            const string fmtPictureAndViews = "<h3><p>Pictures: {0:#,#} Views: {1:#,#}</p></h3>";
            return string.Format(fmtPictureAndViews, pictures, views);
        }
        static string _Table(List<CData> data, CWorker.SampleTypeEnum sType, int limit)
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
                    switch (sType)
                    {
                        case CWorker.SampleTypeEnum.Today:
                            sb.AppendLine(_MakeRow(record, record.Today));
                            break;
                        case CWorker.SampleTypeEnum.Total:
                            sb.AppendLine(_MakeRow(record, 0));
                            break;
                        case CWorker.SampleTypeEnum.Week:
                            sb.AppendLine(_MakeRow(record, record.Week));
                            break;
                        case CWorker.SampleTypeEnum.Month:
                            sb.AppendLine(_MakeRow(record, record.Month));
                            break;
                        default:
                            throw new ApplicationException("BANG");
                    }

                }
            }
            sb.AppendLine("</table>");
            return sb.ToString();
        }
        public static string _CSS()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<head>");
            sb.AppendLine("<style>");
            sb.AppendLine("h2 { color: blue;  text-align: left; font-size: 18pt;}");
            sb.AppendLine("h3 { color: black;  text-align: left; font-size: 14pt;}");
            sb.AppendLine("table,td,th {border: 1px solid black;}");
            sb.AppendLine("td {padding:15px;text-align:right;}");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            return sb.ToString();
        }
    }
}