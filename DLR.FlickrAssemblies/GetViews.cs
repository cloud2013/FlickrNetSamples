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
    public class CReadFlickr
    {
         CDB _DB;
         FlickrNet.Flickr _F = null;
         OAuthAccessToken AccessToken = new OAuthAccessToken();
            public CReadFlickr()
            {
                //CWorker.BasePath = @"C:\TEMP\";
                //CWorker.DataBaseRootName = "FLICKRDB";
                _DB = CWorker.ReadDB();
            }
            public CReadFlickr(string basePath, string rootName)
            {
                CWorker.BasePath = basePath;
                CWorker.DataBaseRootName = rootName;
                CDB db = CWorker.ReadDB();
            }
        public CDB Exec()
        {

           
            if (_DB.GetOAuthToken == null)
            {
                AccessToken.FullName = "Cloud2013";
                AccessToken.Token = "72157644879828272-04fe2e4af1f40866";
                AccessToken.UserId = "26095572@N07";
                AccessToken.TokenSecret = "02947f478d4b0cc1";
                FlickrManager.OAuthToken = AccessToken;
                _F = FlickrManager.GetAuthInstance("2c67273e05ae10a7001e5b569df4f7d1", "d8906735118cab71");
                _DB.SetUser(AccessToken.FullName, AccessToken.UserId);
                _DB.APIKey = "2c67273e05ae10a7001e5b569df4f7d1";
                _DB.SharedSecret = "d8906735118cab71";
                _DB.Token = AccessToken.Token;
                _DB.TokenSecret = AccessToken.TokenSecret;
            }
            else
            {
                FlickrManager.OAuthToken = _DB.GetOAuthToken;
                _F = FlickrManager.GetAuthInstance(_DB.APIKey, _DB.SharedSecret);
            }
            int ndx = 0;
            while (true)
            {
                PhotoCollection photo = _F.PeopleGetPhotos(PhotoSearchExtras.Views, ndx++, 500);
                if (photo.Count == 0)
                {
                    break;
                }
                foreach (FlickrNet.Photo p in photo)
                {
                    if (CWorker.NewEntry(p.PhotoId, _DB))
                    {
                        _DB.Photos.Add(new CPhoto(p.PhotoId, p.Title, p.ThumbnailUrl, p.LargeUrl, p.Views));
                    }
                    else
                    {
                        CPhoto thisPhoto = CWorker.GetPhotoRecord(p.PhotoId, _DB);
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
            return _DB;
        }
        public void Commit()
        {
            CWorker.StoreDB(_DB);
        }
        public void Commit(CDB pDB)
        {
            CWorker.StoreDB(pDB);
        }
    }
}