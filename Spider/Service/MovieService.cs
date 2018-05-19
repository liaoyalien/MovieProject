using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.DB;

using Repository;

namespace Spider.Service
{
    public class MovieService
    {
        MovieRepository movieRepository = new MovieRepository();

        public void bulkInsert(List<tmpMovieModel> datas)
        {
            movieRepository.BulkInsert(datas);
        }

        public void TransferDataFromTempTable()
        {
            movieRepository.TransferDataFromTempTable();
        }

    }
}
