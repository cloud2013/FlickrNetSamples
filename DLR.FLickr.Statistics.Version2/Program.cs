using DLR.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DLR.Flickr.Statistics.Version2
{
    class Program
    {
        public class CRecordTotal
        {
            public string Date { get; set; }
            public int Views { get; set; }
        }
        static void Main(string[] args)
        {
            CWorker.BasePath = @"C:\TEMP\";
            CWorker.DataBaseRootName = "FLICKRDB";
            CDB db = CWorker.ReadDB();
            if (db == null)
            {
                CWorker.Exit(1, "No Data Base");
            }
            if (db.Photos == null)
            {
                CWorker.Exit(1, "No Photo data in Data Base");
            }
            if (db.Photos.Count == 0)
            {
                CWorker.Exit(1, "No Photo data in Data Base");
            }
            bool atleast2 = false;
            bool atleast7 = false;
            bool atleast30 = false;
            List<string> dates=db.MakeDateList();
            if (dates.Count > 1)
            {
                atleast2 = true; 
            }
            if (dates.Count > 7)
            {
                atleast7 = true;
            }
            if (dates.Count > 29)
            {
                atleast30 = true;
            }
               Console.WriteLine("We Are Starting.");
            if (!atleast2)
            {
                Console.WriteLine("No Prior Day Data Yet!");
            }
            if (!atleast7)
            {
                Console.WriteLine("No Weekly Data Yet!");
            }
            if (!atleast30)
            {
                Console.WriteLine("No 30 Day Data Yet!");
            }

            int weekDays = 7;// ((Convert.ToDateTime(currentDateString)) - (Convert.ToDateTime(firstDateString))).Days;
            int priorDays = 2;

            string firstDateString = CWorker.FormatDateString(dates[0]);
            string priorDateString = string.Empty;
            string weekDateString = string.Empty;
            string currentDateString = CWorker.FormatDateString(dates[dates.Count - 1]);

            CXDB xDB = new CXDB();
            xDB.Totals = db.TotalViews();
            List<CData> dataList = xDB.Data;

            firstDateString = CWorker.FormatDateString(dates[0]);
            if (atleast2)
            {
                priorDateString = CWorker.FormatDateString(dates[dates.Count - 2])+ " : " + CWorker.FormatDateString(dates[dates.Count - 1]);
            }
            if (atleast7)
            {
                weekDateString = CWorker.FormatDateString(dates[dates.Count - 8]) + " : " + CWorker.FormatDateString(dates[dates.Count - 1]);
            }
            
            CDate dateRecord = new CDate();
            dateRecord.Today = currentDateString;
            dateRecord.Prior = priorDateString;
            dateRecord.First = firstDateString;
            dateRecord.Week = weekDateString;
            dateRecord.WeekDays = weekDays;
            dateRecord.PriorDays = priorDays;
            xDB.Date = dateRecord;

            int viewstotal = 0;
            int viewstoday = 0;
            int viewsWeek = 0;
            foreach (CPhoto photo in db.Photos)
            {
                CStats current = null;
                CStats prior = null;
                CStats first = null;
                CStats week = null;

                int thisTotal=0;
                int thisWeek =0;
                int thisToday = 0;

                int thisPhotoCount=photo.Stats.Count;
                if (thisPhotoCount == 0)
                {
                    continue;
                }
                first = photo.Stats[0];
                current = photo.Stats[photo.Stats.Count - 1];
                if (thisPhotoCount > 1)
                {
                    prior = photo.Stats[photo.Stats.Count - 2];
                }
                if (thisPhotoCount > 6)
                {
                    week = photo.Stats[photo.Stats.Count - 8];
                }
                thisTotal = current.Views;
                if (prior != null)
                {
                    thisToday = current.Views - prior.Views;
                }
                else
                {
                    thisToday = 0;
                }

                viewstotal += thisTotal;
                viewstoday += thisToday;
                if (week != null)
                {
                    thisWeek = current.Views - week.Views;
                    viewsWeek += thisWeek;
                }

                CData data = new CData();
                    data.MetaData = new CMetaData(photo);
                    data.Total = thisTotal;
                    data.Week = thisWeek;
                    data.Today = thisToday;
                xDB.Data.Add(data);
            }
            xDB.Views = new CViews();
            xDB.Views.Total = viewstotal;
            xDB.Views.Today = viewstoday;
            xDB.Views.Week = viewsWeek;
            CWorker.StoreXDB(xDB);
        }
    }
}
