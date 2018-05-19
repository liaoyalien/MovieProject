using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
    public class MovieModel
    {
        public int MovieId { get; set; }
        public string MovieEngName { get; set; }
        public string MovieLocalName { get; set; }
        /// <summary>
        /// 因為omdb有非數字的爛來源資料格式
        /// </summary>
        public string Year { get; set; }
        public string imdbID { get; set; }
        public string MovieType { get; set; }
        public string Poster { get; set; }
    }
}
