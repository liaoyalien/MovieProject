using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lck.Kernel;

using Model.DB;
using Model.TMDB;

namespace Spider.Service
{
    public class TmdbService : AbstractXmdbService
    {
        public TmdbService()
        {
            ApiUrl = "https://api.themoviedb.org/3";
            ApiKey = "466830daea311db3741c2008a458d3b6";
            ApiName = "TMDB";
        }

        public bool IsLatest(ApiLatestUpdateModel latestData, int totalPages)
        {
            if (latestData.TotalPages == totalPages)
                return true;

            ApiLatestUpdateRepository.UpdateTotalPage(latestData, totalPages);
            return false;
        }


        public bool IsDataEnd()
        {
            //先檢查是否為最新的，如果不是重新再爬
            var apiData = _GetPopularMovie();

            var keyFilter = GetApiLatestUpdateModel(ApiName, "GetPopularMovie");
            var latestData = ApiLatestUpdateRepository.GetByFilter(keyFilter).FirstOrDefault();

            bool isLatest = IsLatest(latestData, apiData.Total_pages);

            if (isLatest)
                //最新的還要再檢查是否current page > total pages
                return latestData.CurrentPages > apiData.Total_pages;
            else
                return false;
            
        }

        /// <summary>
        /// 只有tmdb才需要，允許中斷後再重新從上次失敗的頁數開始抓; omdb目前不需要
        /// </summary>
        /// <returns></returns>
        public int GetStartPage()
        {
            var keyFilter = GetApiLatestUpdateModel(ApiName, "GetPopularMovie");

            var latestData = ApiLatestUpdateRepository.GetByFilter(keyFilter).FirstOrDefault();

            return latestData == null ? 0 : latestData.CurrentPages;
        }

        public PopularMovieViewModel GetPopularMovie(int page = 1)
        {
            var result = _GetPopularMovie(page);

            UpdatePage(ApiName, "GetPopularMovie", page);

            return result;
        }

        public PopularMovieViewModel _GetPopularMovie(int page = 1)
        {
            string url = $"{ApiUrl}/movie/popular?api_key={ApiKey}&language=en-US&page={page}";

            return new HTTP().SendData<PopularMovieViewModel>("GET", url, "");
        }

    }
}
