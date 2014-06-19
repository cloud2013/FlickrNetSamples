using DLR.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DLR.Flickr.Statistics.Version2
{
    class Program
    {
        //public class CRecordTotal
        //{
        //    public string Date { get; set; }
        //    public int Views { get; set; }
        //}
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
            Console.WriteLine("We Are Starting.");
            DateTime today = CWorker.FixDate( DateTime.Now);
            CXDB xDB = new CXDB();
            xDB.Totals = db.TotalViews();
            CDate dateRecord = new CDate();
            dateRecord.WeekDays = 7;
            dateRecord.PriorDays = 1;
            dateRecord.MonthDays = 31;
            string todayStr = CWorker.DT2StrReadable(CWorker.FixDate(today));
            dateRecord.Today = todayStr;
            dateRecord.Prior = todayStr + " : " + CWorker.DT2StrReadable(CWorker.FixDate((CWorker.DTSub(today, dateRecord.PriorDays))));
            dateRecord.First = todayStr;
            dateRecord.Week = CWorker.DT2StrReadable(CWorker.FixDate((CWorker.DTSub(today, dateRecord.WeekDays)))) + " : " + todayStr ;
            dateRecord.Month = CWorker.DT2StrReadable(CWorker.FixDate((CWorker.DTSub(today, dateRecord.MonthDays)))) + " : " + todayStr;
           
            xDB.Date = dateRecord;
            int photosViewsTotal = 0;
            int photosViewsToday = 0;
            int photosViewsWeek = 0;
            int photosViewsMonth = 0;

            int photosWithViews_Total = 0;
            int photosWithViews_Today = 0;
            int photosWithViews_Week = 0;
            int photosWithViews_Month = 0;

            foreach (CPhoto photo in db.Photos)
            {
                int thisWeek =0;
                int thisToday = 0;
                int thisMonth = 0;
                int thisPhotoCount=photo.Stats.Count;

                if (photo.Stats.Count == 0)
                {
                    continue;
                }
                CStats current = photo.Stats[photo.Stats.Count - 1];
                photosViewsTotal += current.Views;
                if (current.Views != 0)
                {
                    photosWithViews_Total++;
                }

                thisToday = CWorker.DeltaDaily(today, photo.Stats, dateRecord.PriorDays);
                photosViewsToday += thisToday;
                if (thisToday != 0)
                {
                    photosWithViews_Today++;
                }


                thisWeek = CWorker.DeltaDaily(today, photo.Stats, dateRecord.WeekDays);
                photosViewsWeek += thisWeek;
                if (thisWeek != 0)
                {
                    photosWithViews_Week++;
                }

                thisMonth = CWorker.DeltaDaily(today, photo.Stats, dateRecord.MonthDays);
                photosViewsMonth += thisMonth;
                if (thisMonth != 0)
                {
                    photosWithViews_Month++;
                }
                CData data = new CData();
                    data.MetaData = new CMetaData(photo);
                    data.Total = current.Views;
                    data.Week = thisWeek;
                    data.Today = thisToday;
                    data.Month = thisMonth;
                xDB.Data.Add(data);
            }
            xDB.Views = new CViews();
            xDB.Pictures = new CViews();
            xDB.Pictures.Today = photosWithViews_Today;
            xDB.Pictures.Total = photosWithViews_Total;
            xDB.Pictures.Week = photosWithViews_Week;
            xDB.Pictures.Month = photosWithViews_Month;
           
            xDB.Views.Total = photosViewsTotal;
            xDB.Views.Today = photosViewsToday;
            xDB.Views.Week = photosViewsWeek;
            xDB.Views.Month = photosViewsMonth;
            CWorker.StoreXDB(xDB);
            if (args.Length != 0)
            {
                CWorker.Exit(0, "Success");
            }
        }
    }
}
