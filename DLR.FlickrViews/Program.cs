using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlickrNet;
using FManager;
using System.IO;
using DLR.Statistics;

namespace DLR.FlickrViews
{
    class Program
    {
        static void Main(string[] args)
        {
            OAuthAccessToken accessToken = new OAuthAccessToken();
            FlickrNet.Flickr f = null;
            CWorker.BasePath = @"C:\TEMP\";
            CWorker.DataBaseRootName = "FLICKRDB";
            CDB db = CWorker.ReadDB();
            if (db.GetOAuthToken == null)
            {
                accessToken.FullName = "Cloud2013";
                accessToken.Token = "72157644879828272-04fe2e4af1f40866";
                accessToken.UserId = "26095572@N07";
                accessToken.TokenSecret = "02947f478d4b0cc1";
                FlickrManager.OAuthToken = accessToken;
                f = FlickrManager.GetAuthInstance("2c67273e05ae10a7001e5b569df4f7d1", "d8906735118cab71");
                db.SetUser(accessToken.FullName, accessToken.UserId);
                db.APIKey = "2c67273e05ae10a7001e5b569df4f7d1";
                db.SharedSecret = "d8906735118cab71";
                db.Token = accessToken.Token;
                db.TokenSecret = accessToken.TokenSecret;
            }
            else
            {
                FlickrManager.OAuthToken = db.GetOAuthToken;
                f = FlickrManager.GetAuthInstance(db.APIKey,db.SharedSecret);
            }
            int ndx = 0;
            while (true)
            {
                PhotoCollection photo = f.PeopleGetPhotos(PhotoSearchExtras.Views, ndx++, 500);
                if (photo.Count == 0)
                {
                    break;
                }
                foreach (FlickrNet.Photo p in photo)
                {
                    if (CWorker.NewEntry(p.PhotoId, db))
                    {
                        db.Photos.Add(new CPhoto(p.PhotoId, p.Title, p.ThumbnailUrl, p.LargeUrl, p.Views));
                    }else
                    {
                        CPhoto thisPhoto = CWorker.GetPhotoRecord(p.PhotoId, db);
                        CStats record = CWorker.GetStatsRecord(thisPhoto, System.DateTime.Now);
                        if (record == null)
                        {
                            thisPhoto.Stats.Add(new CStats(p.Views));
                        }
                        else
                        {
                            if (p.Views == null)
                            {
                                p.Views = 0;
                            }
                            if (record.Views != Convert.ToInt32(p.Views))
                            {
                                record.Views = Convert.ToInt32(p.Views);
                            }
                        }
                    }
                }         
            }
            CWorker.StoreDB(db);
            if (args.Length != 0)
            {
                CWorker.Exit(0, "Success");
            }
        }
    }
}
