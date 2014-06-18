using DLR.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DLR.Flickr.Statistics.Version2
{
    public class CStatistics
    {
            CDB _DB=null;
            CXDB xDB = new CXDB();
            public CStatistics()
            {
                CWorker.BasePath = @"C:\TEMP\";
                CWorker.DataBaseRootName = "FLICKRDB";
                _DB = CWorker.ReadDB();
            }
        public CStatistics(CDB db) {
            _DB=db;
            CWorker.BasePath = @"C:\TEMP\";
            CWorker.DataBaseRootName = "FLICKRDB";
        }
        public CStatistics(string basePath, string dbRootname)
        {
            CWorker.BasePath = basePath;
            CWorker.DataBaseRootName = dbRootname;
            _DB=  CWorker.ReadDB();
        }
        public CXDB Exec(){
             if (_DB == null)
            {
                return null;
            }
            if (_DB.Photos == null)
            {
                return null;
            }
            if (_DB.Photos.Count == 0)
            {
                return null;
            }
            //Console.WriteLine("We Are Starting.");
            DateTime today = CWorker.FixDate( DateTime.Now);
           
            xDB.Totals = _DB.TotalViews();
            CDate dateRecord = new CDate();
            dateRecord.WeekDays = 6;
            dateRecord.PriorDays = 1;
            dateRecord.MonthDays = 30;
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

            foreach (CPhoto photo in _DB.Photos)
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

                thisToday = CWorker.Delta(today, photo.Stats, dateRecord.PriorDays);
                photosViewsToday += thisToday;
                if (thisToday != 0)
                {
                    photosWithViews_Today++;
                }


                thisWeek = CWorker.DeltaWeek(today, photo.Stats, dateRecord.WeekDays);
                photosViewsWeek += thisWeek;
                if (thisWeek != 0)
                {
                    photosWithViews_Week++;
                }

                thisMonth = CWorker.DeltaMonth(today, photo.Stats, dateRecord.MonthDays);
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
            return xDB;
        }
        public void Commit(){
            CWorker.StoreXDB(xDB);
        }
        public void Commit(CXDB xdb)
        {
            CWorker.StoreXDB(xdb);
        }
    }
}