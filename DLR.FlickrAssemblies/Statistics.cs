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

        List<CTotalViews> TotalViews(CDB db)
        {

            List<CTotalViews> output = new List<CTotalViews>();
            foreach (string date in MakeDateList(db))
            {
                output.Add(new CTotalViews(date));
            }

            foreach (CPhoto photo in db.Photos)
            {
                int offset = 0;
                int running = 0;
                CTotalViews prior = null;
                foreach (CTotalViews record in output)
                {
                    CStats stat= FindIf(photo.Stats, CWorker.Str2DT( record.Date));
                    if (stat != null )
                    {
                        record.Views += stat.Views;
                        running += stat.Views;
                    }
                    if (stat == null )
                    {
                        if (prior != null)
                        {
                            record.Views += prior.Views;
                        }
                    }
                    offset++;
                    prior = new CTotalViews(record.Date);
                    prior.Views = record.Views;
                }
            }
            return output;
        }
        public List<string> MakeDateList(CDB db)
        {//change this to use real calandar dates from NOW date
            List<string> things = new List<string>();
            foreach (CPhoto photo in db.Photos)
            {
                foreach (CStats stats in photo.Stats)
                {
                    _AddNewString(ref things, stats.Date);
                }
            }
            return things;
        }
        void _AddNewString(ref List<string> things, string thing)
        {
            bool hit = false;
            foreach (string x in things)
            {
                if (x == thing)
                {
                    hit = true;
                    break;
                }
            }
            if (!hit)
            {
                things.Add(thing);
                things.Sort();
            }
        }
        static CStats _LookupStatRecord(CPhoto photo, string date)
        {
            foreach (CStats stats in photo.Stats)
            {
                if (stats.Date == date)
                {
                    return stats;
                }
            }
            return null;
        }

        List<CTotalViews> TotalDelta(CXDB xDB)
        {
            List<CTotalViews> output = new List<CTotalViews>();
            CTotalViews prior = null;
            foreach (CTotalViews record in xDB.Totals)
            {

                CTotalViews recordOut = new CTotalViews(record.Date);
                recordOut.Date = record.Date;
                if (prior != null)
                {
                    recordOut.Views = record.Views;
                    recordOut.Total = record.Views - prior.Views;
                }
                else
                {
                    recordOut.Total = 0;
                }
                prior = record;
                output.Add(recordOut);
            }
            return output;
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
            //xDB.Totals = _DB.TotalViews();
            //xDB.Totals = TotalViews(_DB);
            //xDB.Totals = TotalDelta(xDB);
            xDB.Totals=new List<CTotalViews>();
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

            int ViewsTotal = 0;
            int ViewsToday = 0;
            int ViewsWeek = 0;
            int ViewsMonth = 0;

            int picturesTotal = 0;
            xDB.Pictures.Today = 0;
            int picturesWeek = 0;
            int picturesMonth = 0;

            List<DateTime> range = FindRange();


            xDB.Pictures.Today = 0;
            xDB.Pictures.Total = 0;
            xDB.Pictures.Week = 0;
            xDB.Pictures.Month = 0;

            xDB.Views.Total = 0;
            xDB.Views.Today = 0;
            xDB.Views.Week = 0;
            xDB.Views.Month = 0;
            xDB.MaxCount = 0;




            int maxCount = -1;
            foreach (CPhoto photo in _DB.Photos)
            {

                if (photo.ID == "4633792226")
                {
                    string x = "y";
                }
                List<CStats> fullStat = FillArray(photo.Stats, range);//
                List<CStats> deltaList = DeltaArray(fullStat);
                if (fullStat.Count < 2)
                {
                    continue;
                }
                if (fullStat.Count > xDB.MaxCount)
                {
                    xDB.MaxCount = fullStat.Count;
                }
                
                
                int thisWeek = 0;
                int thisToday = 0;
                int thisMonth = 0;
                
               
                //CStats urRecord = fullStat[0];
                CStats current = FindIf(fullStat, today);//
                

               
                ViewsTotal += current.Views;
                if (current.Views != 0)
                {
                    picturesTotal++;
                }
                

                //thisToday = CWorker.Delta2(today, deltaList, dateRecord.PriorDays);
                CStats temp = FindIf(deltaList, today);
                if (temp == null)
                {
                    continue;
                }
                thisToday = temp.Views;
                
                ViewsToday += thisToday;
                if (thisToday != 0)
                {
                    xDB.Pictures.Today++;
                }
                
                thisWeek = CWorker.Delta2(today, deltaList, dateRecord.WeekDays);
                ViewsWeek += thisWeek;
                if (thisWeek != 0)
                {
                    picturesWeek++;
                }
                
                thisMonth = CWorker.Delta2(today, deltaList, dateRecord.MonthDays);
                ViewsMonth += thisMonth;
                if (thisMonth != 0)
                {
                    picturesMonth++;
                }
                UpdateViews(xDB, deltaList);
                UpdateTotals(xDB, fullStat);
                CData data = new CData();
                data.MetaData = new CMetaData(photo);
                data.Total = current.Views;
                data.Week = thisWeek;
                data.Today = thisToday;
                data.Month = thisMonth;
                xDB.Data.Add(data);
            }

           
            xDB.Pictures.Total = picturesTotal;
            xDB.Pictures.Week = picturesWeek;
            xDB.Pictures.Month = picturesMonth;

            xDB.Views.Total = ViewsTotal;
            xDB.Views.Today = ViewsToday;
            xDB.Views.Week = ViewsWeek;
            xDB.Views.Month = ViewsMonth;
            xDB.MaxCount = maxCount;
            return xDB;
        }
        public CTotalViews FindTotalViews(string dateSTR, CXDB xDB)
        {
            DateTime dt = CWorker.Str2DT(dateSTR);
            foreach (CTotalViews record in xDB.Totals)
            {
                if (dt == CWorker.Str2DT(record.Date))
                {
                    return record;
                }
            }
            return null;
        }
        public void UpdateViews(CXDB xDB,List<CStats> stats)
        {
            foreach (CStats stat in stats)
            {
                CTotalViews totalView = FindTotalViews(stat.Date, xDB);
                if (totalView != null)
                {
                    totalView.Views += stat.Views;
                }
                else
                {
                    CTotalViews tView = new CTotalViews(stat.Date);
                    tView.Views = stat.Views;
                    xDB.Totals.Add(tView);
                }
            }
        }
        public void UpdateTotals(CXDB xDB, List<CStats> stats)
        {
            foreach (CStats stat in stats)
            {
                CTotalViews totalView = FindTotalViews(stat.Date, xDB);
                if (totalView != null)
                {
                    totalView.Total += stat.Views;
                }
                else
                {
                    CTotalViews tView = new CTotalViews(stat.Date);
                    tView.Total = stat.Views;
                    xDB.Totals.Add(tView);
                }
            }
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

        List<CStats> DeltaArray(List<CStats> stats)
        {
            List<CStats> statOut = new List<CStats>();
            statOut.Add(new CStats(CWorker.Str2DT(stats[0].Date), 0));
            for (int ndx = 1; ndx != stats.Count; ndx++)
            {
                CStats priorObject = stats[ndx - 1];
                int prior = stats[ndx - 1].Views;
                statOut.Add(new CStats(CWorker.Str2DT(stats[ndx].Date), (stats[ndx].Views - priorObject.Views)));
            }
            int xdx = stats.Count - 1;
            
            return statOut;
        }

        List<CStats> FillArray(List<CStats> stats, List<DateTime> rangeList)
        {
            int count = ((rangeList[1].Subtract(rangeList[0])).Days) + 1;
            List<CStats> statOut = new List<CStats>(count);
            int prior = 0;
            for (int ndx = 0; ndx != count; ndx++)
            {
                DateTime target = rangeList[0].Add(new TimeSpan(ndx, 0, 0, 0));
                CStats ifStat = FindIf(stats, target);
                if (ifStat != null)
                {
                    statOut.Add(new CStats(target, ifStat.Views));
                }
                else
                {
                    statOut.Add(new CStats(target, prior));
                }
                prior = FindIf(statOut, target).Views;
                //prior = statOut[statOut.Count - 1].Views;
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