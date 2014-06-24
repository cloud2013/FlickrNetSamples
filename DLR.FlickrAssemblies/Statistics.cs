using DLR.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DLR.Flickr.Statistics.Version2
{
    public class CStatistics
    {
        CDB _DB = null;
        CXDB xDB = new CXDB();
        public CStatistics()
        {
            _DB = CWorker.ReadDB();
        }
        public CStatistics(CDB db)
        {
            _DB = db;
        }
        public CStatistics(string basePath, string dbRootname)
        {
            CWorker.BasePath = basePath;
            CWorker.DataBaseRootName = dbRootname;
            _DB = CWorker.ReadDB();
        }
        public CXDB Exec()
        {
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

            DateTime today = CWorker.FixDate(DateTime.Now);
            xDB.Totals = _DB.TotalViews();
            CDate dateRecord = new CDate();
            dateRecord.WeekDays = 6;
            dateRecord.PriorDays = 1;
            dateRecord.MonthDays = 30;
            string todayStr = CWorker.DT2StrReadable(CWorker.FixDate(today));
            dateRecord.Today = todayStr;
            dateRecord.Prior = todayStr + " : " + CWorker.DT2StrReadable(CWorker.FixDate((CWorker.DTSub(today, dateRecord.PriorDays))));
            dateRecord.First = todayStr;
            dateRecord.Week = CWorker.DT2StrReadable(CWorker.FixDate((CWorker.DTSub(today, dateRecord.WeekDays)))) + " : " + todayStr;
            dateRecord.Month = CWorker.DT2StrReadable(CWorker.FixDate((CWorker.DTSub(today, dateRecord.MonthDays)))) + " : " + todayStr;

            xDB.Date = dateRecord;


            xDB.Views = new CViews();
            xDB.Pictures = new CViews();

            int photosViewsTotal = 0;
            int photosViewsToday = 0;
            int photosViewsWeek = 0;
            int photosViewsMonth = 0;

            int photosWithViews_Total = 0;
            xDB.Pictures.Today = 0;
            int photosWithViews_Week = 0;
            int photosWithViews_Month = 0;

            List<DateTime> range = FindRange();

            int maxCount = -1;
            foreach (CPhoto photo in _DB.Photos)
            {

                if (photo.ID == "4633792226")
                {
                    string x = "y";
                }
                List<CStats> fullStat = FillArray(photo.Stats, range);//this allows us to remove checks
                int thisWeek = 0;
                int thisToday = 0;
                int thisMonth = 0;

                //fullStat = photo.Stats;
                int thisPhotoCount = fullStat.Count;
                if (thisPhotoCount > maxCount)
                {
                    maxCount = thisPhotoCount;
                }
                if (thisPhotoCount == 0)
                {
                    continue;
                }
                CStats current = fullStat[thisPhotoCount - 1];
                photosViewsTotal += current.Views;
                if (current.Views != 0)
                {
                    photosWithViews_Total++;
                }

                thisToday = CWorker.DeltaDaily(today, fullStat, dateRecord.PriorDays);
                photosViewsToday += thisToday;
                if (thisToday != 0)
                {
                    xDB.Pictures.Today++;
                }
                thisWeek = CWorker.DeltaWeek(today, fullStat, dateRecord.WeekDays);
                photosViewsWeek += thisWeek;
                if (thisWeek != 0)
                {
                    photosWithViews_Week++;
                }
                thisMonth = CWorker.DeltaMonth(today, fullStat, dateRecord.MonthDays);
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


            xDB.Pictures.Total = photosWithViews_Total;
            xDB.Pictures.Week = photosWithViews_Week;
            xDB.Pictures.Month = photosWithViews_Month;

            xDB.Views.Total = photosViewsTotal;
            xDB.Views.Today = photosViewsToday;
            xDB.Views.Week = photosViewsWeek;
            xDB.Views.Month = photosViewsMonth;
            xDB.MaxCount = maxCount;
            return xDB;
        }
        public void Commit()
        {
            CWorker.StoreXDB(xDB);
        }
        public void Commit(CXDB xdb)
        {
            CWorker.StoreXDB(xdb);
        }
        List<DateTime> FindRange()
        {
            DateTime low = new DateTime(1900, 01, 01);
            DateTime hi = new DateTime(2099, 01, 01);
            foreach (CPhoto photo in _DB.Photos)
            {
                foreach (CStats stat in photo.Stats)
                {
                    if (CWorker.Str2DT(stat.Date) > low)
                    {
                        low = CWorker.Str2DT(stat.Date);
                    }
                    if (CWorker.Str2DT(stat.Date) < hi)
                    {
                        hi = CWorker.Str2DT(stat.Date);
                    }
                }
            }
            List<DateTime> lDate = new List<DateTime>();
            lDate.Add(low);
            lDate.Add(hi);
            return lDate.OrderBy(q => q).ToList();
        }
        List<CStats> FillArray(List<CStats> stats, List<DateTime> rangeList)
        {
            int count = ((rangeList[1].Subtract(rangeList[0])).Days) + 1;
            List<CStats> statOut = new List<CStats>(count);
            for (int ndx = 0; ndx != count; ndx++)
            {
                DateTime target = rangeList[0].Add(new TimeSpan(ndx, 0, 0, 0));
                int prior = 0;
                CStats ifStat = FindIf(stats, target);
                if (ifStat != null)
                {
                    statOut.Add(new CStats(target, ifStat.Views));
                }
                else
                {
                    statOut.Add(new CStats(target, prior));
                }
                prior = statOut[statOut.Count - 1].Views;
            }
            return statOut;
        }
        CStats FindIf(List<CStats> stats,DateTime dt)
        {
            string dtStr = CWorker.DT2Str(dt);
            foreach (CStats stat in stats)
            {
                if (dtStr == stat.Date)
                {
                    return stat;
                }
            }
            return null;
        }
    }
}