using System;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using System.Linq;
using FlickrNet;

namespace FManager
{
    public class FlickrManager
    {
        public const string ApiKey = "2c67273e05ae10a7001e5b569df4f7d1";
        public const string SharedSecret = "d8906735118cab71";
        static OAuthAccessToken _OAuthToken = null;

        public static Flickr GetInstance()
        {
            return new Flickr(ApiKey, SharedSecret);
        }

        public static Flickr GetAuthInstance()
        {
            var f = new Flickr(ApiKey, SharedSecret);
            if (OAuthToken != null)
            {
                f.OAuthAccessToken = OAuthToken.Token;
                f.OAuthAccessTokenSecret = OAuthToken.TokenSecret;
            }
            return f;
        }
        public static Flickr GetAuthInstance(string apiKey, string sharedSecreate)
        {
            var f = new Flickr(ApiKey, SharedSecret);
            if (OAuthToken != null)
            {
                f.OAuthAccessToken = OAuthToken.Token;
                f.OAuthAccessTokenSecret = OAuthToken.TokenSecret;
            }
            return f;
        }

        public static OAuthAccessToken OAuthToken
        {
            get
            {
                return _OAuthToken;
            }
            set
            {
                _OAuthToken = value;
            }
        }

    }
}
