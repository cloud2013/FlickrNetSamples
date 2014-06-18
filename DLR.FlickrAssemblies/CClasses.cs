using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlickrNet;
using FManager;
using System.IO;
namespace DLR.Statistics
{
    public class CStats
    {
        public string Date { get; set; }
        public int Views { get; set; }
        public CStats() { }
        public CStats(DateTime dt, int? views)
        {
            Date = dt.ToString("yyyyMMdd");
            if (views != null)
            {
                Views = Convert.ToInt32(views);
            }
            else
            {
                Views = 0;
            }
        }
        public static CStats Clone(DateTime dt, int views)
        {
            CStats thisS = new CStats();
            thisS.Date = dt.ToString("yyyyMMdd");
            thisS.Views = views;
            return thisS;
        }
        public CStats(int? views)
        {
            Date = System.DateTime.Now.ToString("yyyyMMdd");
            if (views != null)
            {
                Views = Convert.ToInt32(views);
            }
            else
            {
                Views = 0;
            }
        }
        public int Get(string dateString,int span){
            if (dateString == null)
            {
                return 0;
            }
            DateTime dt0 = Convert.ToDateTime(CWorker.FormatDateString(dateString));
            DateTime dt1 = Convert.ToDateTime(CWorker.FormatDateString(Date));
            int days = (dt0 - dt1).Days;
            //string x = _Pack(dateString);
            if (days < span)
            {
                return Views;
            }
            return 0;
        }
        //static string _Pack(string dateString)
        //{
        //    return dateString.Substring(0, 4) + dateString.Substring(4, 2) + dateString.Substring(6, 2);
        //}
    }
    public class CPhoto
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string ThumbURL { get; set; }
        public string LargeURL { get; set; }
        public List<CStats> Stats { get; set; }
        public CPhoto()
        {
            ID = string.Empty;
            Stats = new List<CStats>();
        }
        public CPhoto(string id, string title, string thumb, string large)
        {
            ID = id;
            Title = title;
            ThumbURL = thumb;
            LargeURL = large;
            Stats = new List<CStats>();
        }
        public CPhoto(string id, string title, string thumb, string large, CStats item)
        {
            ID = id;
            Title = title;
            ThumbURL = thumb;
            LargeURL = large;
            Stats = new List<CStats>();
            Stats.Add(item);
        }
        public CPhoto(string id, string title, string thumb, string large, int? views)
        {
            ID = id;
            Title = title;
            ThumbURL = thumb;
            LargeURL = large;
            Stats = new List<CStats>();
            Stats.Add(new CStats(views));
        }
        public CPhoto(string id, string title, string thumb, string large, DateTime dt, int views)
        {
            ID = id;
            Title = title;
            ThumbURL = thumb;
            LargeURL = large;
            Stats = new List<CStats>();
            Stats.Add(new CStats(dt, views));
        }
       
    }
    public class CDB
    {
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string APIKey { get; set; }
        public string SharedSecret { get; set; }
        public string Token { get; set; }
        public string TokenSecret { get; set; }
        public OAuthAccessToken GetOAuthToken
        {
            get
            {
                OAuthAccessToken o = new OAuthAccessToken();
                if (string.IsNullOrEmpty(Token))
                {
                    return null;
                }
                o.Token = Token;
                o.TokenSecret = TokenSecret;
                o.UserId = UserID;
                o.Username = UserName;
                return o;
            }
        }
        public List<CPhoto> Photos { get; set; }
        public void SetUser(string userName, string userID)
        {
            UserName = userName;
            UserID = userID;
        }
        public CDB()
        {
            Photos = new List<CPhoto>();
        }
        public CDB(string apiKey, string sharedSecret, string token, string tokenSecret)
        {
            APIKey = apiKey;
            SharedSecret = sharedSecret;
            Token = token;
            TokenSecret = tokenSecret;
        }
        public CDB(string apiKey, string sharedSecret, OAuthAccessToken aToken)
        {
            APIKey = apiKey;
            SharedSecret = sharedSecret;
            Token = aToken.Token;
            TokenSecret = aToken.TokenSecret;
        }
        public List<string> MakeDateList()
        {//change this to use real calandar dates from NOW date
            List<string> things = new List<string>();
            foreach (CPhoto photo in Photos)
            {
                foreach (CStats stats in photo.Stats)
                {
                    _AddNewString(ref things, stats.Date);
                }
             }
            return things;
        }
        public List<CTotalViews> TotalViews()
        {
            List<CTotalViews> output = new List<CTotalViews>();
            foreach (string date in MakeDateList())
            {
                output.Add(new CTotalViews(date));
            }
            foreach (CPhoto photo in Photos)
            {
                foreach (CTotalViews record in output)
                {
                    CStats stat = _LookupStatRecord(photo, record.Date);
                    if (stat != null)
                    {
                        ////debug test
                        //if (stat.Date != record.Date)
                        //{
                        //    throw new ApplicationException("Bang");
                        //}
                        record.Views += stat.Views;
                    }
                }
            }
            return output;
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
    }
    public class CTotalViews
    {
        public string Date { get; set; }
        public int Views { get; set; }
        public CTotalViews(string date)
        {
            Date = date;
            Views = 0;
        }
        public CTotalViews() { }
    }
    public class CSummaryStats
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string ThumbURL { get; set; }
        public string LargeURL { get; set; }
        public string Date { get; set; }
        public int Views { get; set; }
        public int All { get; set; }
        public void SetMetaFields(CPhoto meta)
        {
            Title = meta.Title;
            ThumbURL = meta.ThumbURL;
            LargeURL = meta.LargeURL;
        }
        public void SetStatistics( CStats last, CStats prior)
        {
            Views = last.Views - prior.Views;
            All = last.Views;
        }
        public CSummaryStats(string id, string date)
        {
            ID = id;
            Date = date;
            Views = 0;
            All = 0;
        }
        public CSummaryStats() { }
    }
    public class CDateDB
    {
        public string Date { get; set; }
        public List<CSummaryStats> Stats { get; set; }
        public CDateDB(string date)
        {
            Date = date;
            Stats = new List<CSummaryStats>();
        }
        public List<CSummaryStats> SortByViews()
        {
           return (from x in Stats orderby x.Views descending select x).ToList();
        }
        public List<CSummaryStats> SortByTotal()
        {
            return (from x in Stats orderby x.All descending select x).ToList();
        }
        public CDateDB() { }
    }

    public class CDate
    {
        public string Today { get; set; }
        public string Prior { get; set; }
        public string First { get; set; }
        public string Week { get; set; }
        public string Month { get; set; }
        
        public int WeekDays { get; set; }
        public int PriorDays { get; set; }
        public int MonthDays { get; set; }
        public CDate() { }
    }
    public class CData
    {
        public CMetaData MetaData { get; set; }
        public int Total { get; set; }
        public int Week { get; set;}
        public int Today {get;set;}
        public int Month { get; set; }
        public CData() { }
    }
    public class CMetaData
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string ThumbURL { get; set; }
        public string LargeURL { get; set; }
        public CMetaData() { }
        public CMetaData(string id, string title, string thumb, string large)
        {
            ID = id;
            Title = title;
            ThumbURL = thumb;
            LargeURL = large;
        }
        public CMetaData(CPhoto photo)
        {
            ID = photo.ID;
            Title = photo.Title;
            ThumbURL = photo.ThumbURL;
            LargeURL = photo.LargeURL;
        }
    }
    public class CViews
    {
        public int Total { get; set; }
        public int Today {get;set;}
        public int Week {get;set;}
        public int Month { get; set; }
        public CViews() { }
    }
    public class CXDB
    {
        public CViews Views { get; set; }
        public CViews Pictures { get; set; }
        public CDate Date { get; set; }
        public List<CData> Data { get; set; }
        public List<CTotalViews> Totals { get; set; }
        public CXDB() {
            Data = new List<CData>();
        }
    }


}