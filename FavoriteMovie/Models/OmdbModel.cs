using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FavoriteMovie.Models
{
    public class OmdbModel
    {
        public List<Search> Search { get; set; }

        [JsonProperty("totalResults")]
        public string TotalResults { get; set; }

        public string Response { get; set; }
    }
    public class Search
    {
        [JsonProperty("Title")]
        public string MovieEngName { get; set; }
        public int Year { get; set; }
        public string imdbID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }

}