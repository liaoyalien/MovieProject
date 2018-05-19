using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FavoriteMovie.Models;

using Lck.Kernel;

using Model.TMDB;

namespace Spider.Service
{
    public class OmdbService : AbstractXmdbService
    {
        
        public OmdbService()
        {
            //兩組備用
            ApiKey = "c92f5d2a";
            //ApiKey = "5c65d29a";
            ApiUrl = "http://www.omdbapi.com";
            ApiName = "OMDB";
        }

        public OmdbModel SearchByTitle(string title, int maxRetryTimes =3)
        {
            int retryTimes = 0;
            while (retryTimes < maxRetryTimes)
            {
                try
                {
                    string url = $"{ApiUrl}/?s={title}&apikey={ApiKey}";
                    return new HTTP().SendData<OmdbModel>("GET", url, "");
                }
                catch (Exception e)
                {
                    retryTimes++;
                    System.Threading.Thread.Sleep(500);
                }
            }
            return null;
        }

    }
}
