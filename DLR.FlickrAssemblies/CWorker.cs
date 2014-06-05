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
        public static void Exit(int code, string txt)
        {
            Console.WriteLine(txt);
            Console.WriteLine("Press Any Key To Exit");
            System.Environment.Exit(code);
        }
        public static string FormatInt(int num)
        {
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
        public static CDateDB ReadDateDB(string date)
        {
            string title = date;
            string fqFile = _FQ(title);
            if (File.Exists(fqFile))
            {
                System.Xml.Serialization.XmlSerializer reader =
                    new System.Xml.Serialization.XmlSerializer(typeof(CDateDB));
                System.IO.StreamReader file = new System.IO.StreamReader(fqFile);
                return (CDateDB)reader.Deserialize(file);
            }
            else
            {
                return null; ;
            }
        }
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
            if (File.Exists(_FQ(title)))
            {
                System.Xml.Serialization.XmlSerializer reader =
                    new System.Xml.Serialization.XmlSerializer(typeof(CDB));
                System.IO.StreamReader file = new System.IO.StreamReader(_FQ(title));
                return (CDB)reader.Deserialize(file);
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
}