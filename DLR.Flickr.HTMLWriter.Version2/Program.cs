﻿using DLR.Statistics;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace DLR.FlickrHTMLWriter
{

   
    class Program
    {
        public const string FMTHREF = "href='https://www.flickr.com/photos/dennisredfield/{0}/in/photostream/'";
        public const string FMTSRC = "src='{0}'";
        static void Main(string[] args)
        {
            int _DisplayLimit = 25;
            CWorker.BasePath = @"C:\TEMP\";
            CWorker.DataBaseRootName = "FLICKRDB";
            string xtractBaseName = "XTRACT";
            CXDB xDB = CWorker.ReadXDB(xtractBaseName);
            if (xDB == null)
            {
                CWorker.Exit(1, "No X Database");
            }
            #region  Today
            string fqFile = CWorker.BasePath + "TODAY.HTML";
            System.IO.File.WriteAllText(fqFile,_Sample(xDB,CWorker.SampleTypeEnum.Today,_DisplayLimit));// _Today(xDB,_DisplayLimit));
            #endregion
            #region TOTAL
            fqFile = CWorker.BasePath + "Total.HTML";
            System.IO.File.WriteAllText(fqFile,_Sample(xDB,CWorker.SampleTypeEnum.Total,_DisplayLimit));// _Total(xDB, _DisplayLimit));
            #endregion
            #region Month
            fqFile = CWorker.BasePath + "Month.HTML";
            System.IO.File.WriteAllText(fqFile, _Sample(xDB, CWorker.SampleTypeEnum.Month, _DisplayLimit));// _Total(xDB, _DisplayLimit));
            #endregion
            #region Sample
            fqFile = CWorker.BasePath + "Week.HTML";
            System.IO.File.WriteAllText(fqFile, _Sample(xDB, CWorker.SampleTypeEnum.Week, _DisplayLimit));//
            #endregion
            #region TOTALS
            fqFile = CWorker.BasePath + "DailyTotals.HTML";
            System.IO.File.WriteAllText(fqFile, _MakeDailyTotals(xDB));
            #endregion
            CWorker.Exit(0, "Success");
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
            sb.AppendLine("<table>");
            int prior = -1;
            foreach (CTotalViews record in xDB.Totals)
            {
                    sb.AppendLine(_MakeRowViewTotals(record,prior));
                    prior = record.Views;
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
            string href =string.Format(FMTHREF,record.MetaData.ID);
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
            if (sType == CWorker.SampleTypeEnum.Month)
            {
                sb.AppendLine(string.Format("<p><h2>Top {0} Views For: {1} Pictures: {2} Views: {3}</h2></p>", limit, xDB.Date.Month, CWorker.FormatInt(xDB.Pictures.Month), CWorker.FormatInt(xDB.Views.Month)));
                sb.AppendLine(_Table((from x in xDB.Data orderby x.Month descending select x).ToList(),sType, limit));
            }
            if (sType == CWorker.SampleTypeEnum.Week)
            {
                sb.AppendLine(string.Format("<p><h2>Top {0} Views For: {1} Pictures: {2} Views: {3}</h2></p>", limit, xDB.Date.Week, CWorker.FormatInt(xDB.Pictures.Today), CWorker.FormatInt(xDB.Views.Week)));
                sb.AppendLine(_Table((from x in xDB.Data orderby x.Week descending select x).ToList(), sType, limit));
               
            }
            if (sType == CWorker.SampleTypeEnum.Today)
            {
                sb.AppendLine(string.Format("<p><h2>Top {0} Views For: {1} Pictures: {2} Views: {3}</h2></p>", limit, xDB.Date.Today, CWorker.FormatInt(xDB.Pictures.Today), CWorker.FormatInt(xDB.Views.Today)));
                //sb.AppendLine(string.Format("<p><h2>Views: {0}</h2></p>", CWorker.FormatInt(xDB.Views.Today)));
                sb.AppendLine(_Table((from x in xDB.Data orderby x.Today descending select x).ToList(), sType, limit));
              
            }

            if (sType == CWorker.SampleTypeEnum.Total)
            {
                sb.AppendLine(string.Format("<p><h2>Top {0} Total Views As Of: {1} Pictures: {2} Views: {3}</h2></p>", limit, xDB.Date.Today, CWorker.FormatInt(xDB.Pictures.Total), CWorker.FormatInt(xDB.Views.Total)));
                sb.AppendLine(_Table((from x in xDB.Data orderby x.Total descending select x).ToList(), sType, limit));
            }
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }
        static string _Table(List<CData> data,CWorker.SampleTypeEnum sType, int limit)
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

