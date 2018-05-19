using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lck.Utility.Extensions;

using Model.DB;

using Tables;

using EntityFramework.BulkInsert.Extensions;

namespace Repository
{
    public class MovieRepository : BaseRepository
    {
        public void BulkInsert(List<tmpMovieModel> datas)
        {
            var insertData = datas
                    //字數超過100應該也不是什麼正常的資料了
                    .Where(m => m.MovieEngName.Length <= 100)
                .Select(m => m.ReturnInjectObjectNullable<tmpMovie>()).ToList();

            foreach (var data in insertData)
                data.MovieLocalName = "";


            db.BulkInsert<tmpMovie>(insertData);
        }

        public void TransferDataFromTempTable()
        {
            this.db.SP_TransferDataFromTempTable();
        }

    }
}
