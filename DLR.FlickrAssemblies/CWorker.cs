using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlickrNet;
using FManager;
using System.IO;
using DLR.Statistics;
namespace DLR.Statistics
{
    public static class CWorker
    {

        public enum SampleTypeEnum { Today,Week,Month,Total}
        public static DateTime DTSub(DateTime dt, int gap)
        {
            return dt.Subtract(new TimeSpan(gap, 0, 0, 0));
        }
        public static string DT2Str(DateTime dt)
        {
            return dt.ToString("yyyyMMdd");
        }
        public static string DT2StrReadable(DateTime dt)
        {
            return dt.ToString("MM-dd-yyyy");
        }
        public static DateTime FixDate(DateTime dt)
        {
            return Str2DT(DT2Str(dt));
        }
        //public static int Month(CStats current, List<CStats> stats, List<string> dates, int gap)
        //{
        //    DateTime dt = Str2DT(dates[dates.Count - 1]);
        //    DateTime dtStart = dt.Subtract(new TimeSpan(gap, 0, 0, 0));
        //    CStats prior = CWorker.FindFirstRecordOnOrAfter(dtStart, stats);
        //    if (prior != null)
        //    {
        //        if (prior.Views != current.Views)
        //        {
        //            return current.Views - prior.Views;
        //        }
        //    }
        //    return 0;
        //}
        public static DateTime Str2DT(string dateString)
        {
            return Convert.ToDateTime(CWorker.FormatDateString(dateString));
        }
        public static int DeltaDaily(DateTime top, List<CStats> stats, int gap)
        {
            if (stats.Count < (gap + 1))
            {
                return 0;
            }
            CStats now=stats[stats.Count - 1];
            CStats prior = stats[stats.Count - (gap + 1)];
            int views= now.Views -  prior.Views;
            return views;
        }
        public static int Delta2(DateTime top, List<CStats> stats, int gap)
        {
            if (stats.Count < (gap + 1))
            {
                return 0;
            }
            //stats is an array of delta values
            int offsetStop = stats.Count - 1;
            int offsetStart = stats.Count - (gap + 1);
            int views = 0;
            for (int ndx = offsetStart; ndx <= offsetStop; ndx++)
            {
                views += stats[ndx].Views;
            }
            return views;
        }
        public static int Delta(DateTime top, List<CStats> stats, int gap)
        {
            if (stats.Count < (gap + 1))
            {
                return 0;
            }
            CStats now = stats[stats.Count - 1];
            CStats prior = stats[stats.Count - (gap + 1)];
            int views = now.Views - prior.Views;
            return views;
        }
        public static int DeltaWeek(DateTime top, List<CStats> stats, int gap)
        {
            if (stats.Count < 2)
            {
                return 0;
            }
            CStats now = stats[stats.Count - 1];
            if (Str2DT(now.Date) != top)
            {
                return 0;
            }
            CStats prior = null;
            DateTime priorDate;
            int offset = stats.Count - (gap + 1);
            if (offset > -1)
            {
                prior = stats[offset];
                priorDate = Str2DT(prior.Date);
            }
            else
            {
                return 0;
            }
            int thisGap = -(priorDate - top).Days;
            if (thisGap > gap)
            {
                return 0;
            }
            int views = now.Views - prior.Views;
            return views;
           
        }
        public static int DeltaMonth(DateTime top, List<CStats> stats, int gap)
        {
            if (stats.Count < 2)
            {
                return 0;
            }
            CStats now = stats[stats.Count - 1];
            if (Str2DT(now.Date) != top)
            {
                return 0;
            }
            CStats prior = null;
            DateTime priorDate;
            int offset = stats.Count - (gap + 1);
            if (offset > -1)
            {
                prior = stats[offset];
                priorDate = Str2DT(prior.Date);
            }
            else
            {
                return 0;
            }
            int thisGap = -(priorDate - top).Days;
            if (thisGap > gap)
            {
                return 0;
            }
            int views = now.Views - prior.Views;
            return views;
            //CStats mostRecent = Find(top, stats);
            //if (mostRecent == null)
            //{
            //    return 0;
            //}
            //if (stats == null)
            //{
            //    return 0;
            //}
            //DateTime myTop = Str2DT(mostRecent.Date);
            //DateTime bottom = DTSub(top, gap);
            //CStats myBottom = FindFirstRecordOnOrAfter(bottom, stats);
            ////CStats myBottom = FindFirstRecordOnOrBefore(bottom, stats);
            //if (myBottom == null)
            //{
            //    return 0;
            //}
            ////if (myBottom.Date == DT2Str(bottom))
            ////{
            ////    return 0;
            ////}
            //return mostRecent.Views - myBottom.Views;
        }
        //public static int Week(CStats current, List<CStats> stats, List<string> dates, int gap)
        //{
        //    DateTime dt = Str2DT(dates[dates.Count - 1]);
        //    DateTime dtWeekStart = DTSub(dt, gap);
        //    CStats prior = CWorker.FindFirstRecordOnOrAfter(dtWeekStart, stats);
        //    if (prior != null)
        //    {
        //        if (prior.Views != current.Views)
        //        {
        //            return current.Views - prior.Views;
        //        }
        //    }
        //    return 0;
        //}
        //public static int Today(CStats current, List<CStats> stats)
        //{
        //    CStats prior = CWorker.FindFirstRecordPrior(current, stats);
        //    if (prior != null)
        //    {
        //        if (prior.Views != current.Views)
        //        {
        //            return current.Views - prior.Views;
        //            //photosViewsToday += thisToday;
        //        }
        //    }
        //    return 0;
        //}
        //public static CStats FindFirstRecordPrior(CStats current, List<CStats> records)
        //{
        //    DateTime dt0 = Convert.ToDateTime(CWorker.FormatDateString(current.Date));
        //    for(int ndx=records.Count-1;ndx!=-1;ndx--)
        //    {
        //        DateTime dt = Convert.ToDateTime(CWorker.FormatDateString(records[ndx].Date));
        //        int days = (dt - dt0).Days;
        //        if (days < 0)
        //        {
        //            return records[ndx];
        //        }
        //    }
        //    return null;
        //}
        //public static CStats FindFirstRecordOnOrAfter(DateTime dt0, List<CStats> records)
        //{
        //    for (int ndx = 0; ndx != records.Count - 1; ndx++)
        //    {
        //        DateTime dt = Convert.ToDateTime(CWorker.FormatDateString(records[ndx].Date));
        //        int days = (dt0 - dt).Days;
        //        if (days <= 0)
        //        {
        //            return records[ndx];
        //        }
        //    }
        //    return null;
        //}
        //public static CStats FindFirstRecordOnOrBefore(DateTime dt0, List<CStats> records)
        //{
        //    for (int ndx = records.Count - 1; ndx != -1; ndx--)
        //    {
        //        DateTime dt = Convert.ToDateTime(CWorker.FormatDateString(records[ndx].Date));
        //        int days = (dt0 - dt).Days;
        //        if (days >= 0)
        //        {
        //            return records[ndx];
        //        }
        //    }
        //    return null;
        //}
        //public static CStats Find(DateTime dt0, List<CStats> records)
        //{
        //    string date = DT2Str(dt0);
        //    foreach (CStats record in records)
        //    {
        //        if (date == record.Date)
        //        {
        //            return record;
        //        }
        //    }
        //    return null;
        //}
        //public static string FindFirstRecordOnOrAfter(DateTime dt0, List<string> records)
        //{
        //    for (int ndx = 0; ndx != records.Count - 1; ndx++)
        //    {
        //        DateTime dt = Str2DT(records[ndx]);
        //        int days = (dt0 - dt).Days;
        //        if (days <= 0)
        //        {
        //            return records[ndx];
        //        }
        //    }
        //    return null;
        //}
        public static void Exit(int code, string txt)
        {
            Console.WriteLine(txt);
            Console.WriteLine("Press Any Key To Exit");
            Console.ReadKey();
            System.Environment.Exit(code);
        }
        public static string FormatInt(int num)
        {
            if (num == 0)
            {
                return string.Format("{0}", "0");
            }
            return string.Format("{0:#,#}", num);
        }
        public static string SuperFormatInt(int num)
        {
            if (num == 0)
            {
                return string.Format("{0}", "_");
            }
            return string.Format("{0:#,#}", num);
        }
        public static string FormatDateString(string date)
        {
            return string.Format("{0}-{1}-{2}", date.Substring(0, 4), date.Substring(4, 2), date.Substring(6, 2));
        }
        public static string BasePath { get; set; }
        public static string DataBaseRootName { get; set; }
        public static CDB ReadDB()
        {
            return _Read(DataBaseRootName);
        }
        //public static CDateDB ReadDateDB(string date)
        //{
        //    string title = date;
        //    string fqFile = _FQ(title);
        //    if (File.Exists(fqFile))
        //    {
        //        System.Xml.Serialization.XmlSerializer reader =
        //            new System.Xml.Serialization.XmlSerializer(typeof(CDateDB));
        //        System.IO.StreamReader file = new System.IO.StreamReader(fqFile);
        //        return (CDateDB)reader.Deserialize(file);
        //    }
        //    else
        //    {
        //        return null; ;
        //    }
        //}
        public static CXDB ReadXDB(string title2)
        {
            string title = title2;
            string fqFile = _FQ(title);
            if (File.Exists(fqFile))
            {
                System.Xml.Serialization.XmlSerializer reader =
                    new System.Xml.Serialization.XmlSerializer(typeof(CXDB));
                System.IO.StreamReader file = new System.IO.StreamReader(fqFile);
                return (CXDB)reader.Deserialize(file);
            }
            else
            {
                return null; ;
            }
        }
        public static void StoreXDB(CXDB db)
        {
            string title = "XTRACT";
            _Delete(title);
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(CXDB));
            System.IO.StreamWriter file = new System.IO.StreamWriter(_FQ(title));
            writer.Serialize(file, db);
            file.Close();
        }
        public static void StoreDateDB(CDateDB db)
        {
            string title = db.Date;
            _Delete(title);
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(CDateDB));
            System.IO.StreamWriter file = new System.IO.StreamWriter(_FQ(title));
            writer.Serialize(file, db);
            file.Close();
        }
        public static void StoreDB(CDB db)
        {
            _Store(DataBaseRootName, db);
        }
        static CDB _Read(string title)
        {

            string fqName = _FQ(title);
            if (File.Exists(fqName))
            {
                System.Xml.Serialization.XmlSerializer reader =
                    new System.Xml.Serialization.XmlSerializer(typeof(CDB));
                System.IO.StreamReader file = new System.IO.StreamReader(_FQ(title));
                CDB it= (CDB)reader.Deserialize(file);
                file.Dispose();
                return it;
            }
            else
            {
                return new CDB();
            }
        }
        static string _FQ(string root)
        {
            return BasePath + root + ".xml";
        }
        public static bool NewEntry(string id, CDB db)
        {
            foreach (CPhoto p in db.Photos)
            {
                if (p.ID == id)
                {
                    return false;
                }
            }
            return true;
        }
        public static CPhoto GetPhotoRecord(string id, CDB db)
        {
            foreach (CPhoto p in db.Photos)
            {
                if (p.ID == id)
                {
                    return p;
                }
            }
            throw new ApplicationException("BANG01");
        }
        public static CStats GetStatsRecord(CPhoto photo, DateTime dt)
        {
            string dateString = dt.ToString("yyyyMMdd");
            foreach (CStats record in photo.Stats)
            {
                if (record.Date == dateString)
                {
                    return record;
                }
            }
            return null;
        }
        static void _Store(string title, CDB db)
        {
            string fqTitle = _FQ(title);
            _Delete(title);
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(CDB));
            System.IO.StreamWriter file = new System.IO.StreamWriter(_FQ(title));
            writer.Serialize(file, db);
            file.Close();
        }
        static void _Delete(string title)
        {
            if (File.Exists(_FQ(title)))
            {
                File.Delete(_FQ(title));
            }
        }
        
        
       
    }

    public class CDBMan
    {
        CDB db = null;
        public CDBMan()
        {
            db = CWorker.ReadDB();

        }
        public  bool KillDates(string date)
        {
            bool anyDup = false;
            foreach (CPhoto photo in db.Photos)
            {
                string dateSTR = string.Empty;
                bool dupDate = false;
                foreach (CStats stats in photo.Stats)
                {
                    if (stats.Date == dateSTR)
                    {
                        dupDate = true;
                        break;
                    }
                    dateSTR = stats.Date;
                }
                if (dupDate)
                {
                    anyDup = true;
                    break;
                }
            }
            bool anychange = false;
            if (anyDup)
            {

                int count = 0;
                string killDate = "20140624";
                for (int ndx = 0; ndx != db.Photos.Count; ndx++)
                {
                    List<CStats> statsList = new List<CStats>();
                    count = 0;
                    for (int xdx = 0; xdx != db.Photos[ndx].Stats.Count; xdx++)
                    {
                        if (string.Compare(killDate, db.Photos[ndx].Stats[xdx].Date, true) == 0)
                        {
                            string y = db.Photos[ndx].Stats[xdx].Date;
                            count++;

                        }
                        else
                        {
                            statsList.Add(CStats.Clone(db.Photos[ndx].Stats[xdx]));
                        }
                    }
                    if (count != 0)
                    {
                        anychange = true;
                        db.Photos[ndx].Stats = statsList;
                    }
                }
            }
            return anychange;
        }
        public   bool TrimDataBase(int month, int capLimit)
        {
            bool anyChange = false;
            List<CStats> replacement = new List<CStats>();
            int max = month + capLimit;
            DateTime dtLimit = System.DateTime.Now.AddDays(-max);
            anyChange = false;
            for (int ndx = 0; ndx != db.Photos.Count; ndx++)
            {
                replacement = _Trim(db.Photos[ndx], max, dtLimit);
                if (replacement != null)
                {
                    anyChange = true;
                    db.Photos[ndx].Stats = replacement;
                }
            }
            //slim it
            replacement = new List<CStats>();
            for (int ndx = 0; ndx != db.Photos.Count; ndx++)
            {
                replacement = _Slim(db.Photos[ndx]);
                if (replacement != null)
                {
                    anyChange = true;
                    db.Photos[ndx].Stats = replacement;
                }
            }



            return anyChange;
        }
        List<CStats> _Slim(CPhoto photo)
        {
            List<CStats> replacement = new List<CStats>();
            int prior = 0;
                foreach (CStats stat in photo.Stats)
                {
                    if (stat.Views!=prior)
                    {
                        replacement.Add(CStats.Clone(stat));
                    }
                    prior = stat.Views;
                }
                if (replacement.Count != 0 & (replacement.Count != photo.Stats.Count))
            {
                return replacement;
            }
            return null;
        }
        List<CStats> _Trim(CPhoto photo, int max, DateTime dtLimit)
        {
            List<CStats> replacement = new List<CStats>();
            if (photo.Stats.Count > max)
            {
                foreach (CStats stat in photo.Stats)
                {
                    if (CWorker.Str2DT(stat.Date) > dtLimit)
                    {
                        replacement.Add(CStats.Clone(stat));
                    }
                }
            }
            if (replacement.Count!=0 & ( replacement.Count != photo.Stats.Count))
            {
                return replacement;
            }
            return null;
        }
        public void Commit()
        {
            CWorker.StoreDB(db);
        }
    }
}