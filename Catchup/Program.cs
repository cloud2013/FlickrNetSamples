using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLR.Statistics;

namespace Catchup
{
    class Program
    {
        static void Main(string[] args)
        {
            string _BasePath = @"C:\temp\";
            string _BaseName = "FLICKRDB";
            CDB _DB;
            CDB _DB2;
            CWorker.BasePath = _BasePath;
            CWorker.DataBaseRootName = _BaseName;
            _DB = CWorker.ReadDB();
            CWorker.DataBaseRootName = "OLD";
            CWorker.BasePath = @"C:\Temp\";
            _DB2 = CWorker.ReadDB();

            foreach (CPhoto photo in _DB.Photos)
            {
                foreach (CPhoto catchUp in _DB2.Photos)
                {
                    if (photo.ID == catchUp.ID)
                    {
                        bool delta = false;
                        foreach (CStats stats in catchUp.Stats)
                        {
                            bool addIn = true;
                            for (int ndx = 0; ndx != photo.Stats.Count; ndx++)
                            {
                                if (stats.Date == photo.Stats[ndx].Date)
                                {
                                    addIn = false;
                                    break;
                                }
                            }
                            if (addIn)
                            {
                                delta = true;
                                photo.Stats.Add(new CStats(CWorker.Str2DT(stats.Date), stats.Views));
                            }
                        }
                        if (delta)
                        {

                            //(from x in xDB.Data orderby x.Total descending orderby x.Month descending select x).ToList();
                            photo.Stats = (from x in photo.Stats orderby x.Date select x).ToList();
                        }

                    }
                }
            }
            CWorker.DataBaseRootName = "Next";
            CWorker.StoreDB(_DB);
        }
    }
}
