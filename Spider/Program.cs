using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

using Lck.Utility.Extensions;

using Model.DB;
using Model.TMDB;

using Repository;

using Spider.Service;

namespace Spider
{
    class Program
    {
        static void Main(string[] args)
        {
            var tmdb = new TmdbService();
            var omdb = new OmdbService();
            var movieService = new MovieService();
            if (tmdb.IsDataEnd())
            {
                Console.WriteLine("Data is the latest!");
                return;
            }

            //從上次的頁數開始
            int currentPage = tmdb.GetStartPage();
            PopularMovieViewModel tmdbMovie = tmdb.GetPopularMovie(currentPage);
            int? totalPages = tmdbMovie?.Total_pages;

            List<tmpMovieModel> awaitingInsertDatas = new List<tmpMovieModel>();

            

            while (totalPages.HasValue && currentPage<= totalPages)
            {
                foreach (var popurlarMovie in tmdbMovie.Results)
                {
                    //利用tmdb去抓omdb的資料
                    var omdbMovie = omdb.SearchByTitle(popurlarMovie.title);

                    if (omdbMovie != null && omdbMovie.Search != null)
                        awaitingInsertDatas.AddRange(
                            omdbMovie.Search.Select(m => m.ReturnInjectObjectNullable<tmpMovieModel>())
                                );

                    Console.WriteLine(popurlarMovie.title);

                    if (awaitingInsertDatas.Count >= 100)
                    {
                        movieService.bulkInsert(awaitingInsertDatas);
                        awaitingInsertDatas.Clear();
                    }
                }

                currentPage++;
                tmdbMovie = tmdb.GetPopularMovie(currentPage);
            }

            //剩下的再做一次
            movieService.bulkInsert(awaitingInsertDatas);
            
            //將 temp的資料全部轉移
            movieService.TransferDataFromTempTable(); 

            Console.WriteLine("done");

        }
        


        
    }
}
