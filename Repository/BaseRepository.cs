using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tables;

namespace Repository
{
    public class BaseRepository
    {
        public MovieEntities db = null;
        public BaseRepository()
        {
            this.db= new MovieEntities();
        }
    }
}
