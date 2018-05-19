using FavoriteMovie.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FavoriteMovie.Repository
{
    public class UserCommentMovieRepository
    {
        public void InsertOrUpdateComment(int userId, int movieId, decimal score)
        {
            DateTime now = DateTime.UtcNow;

            using (var db = new MovieEntities())
            {
                var target = db.UserCommentMovies.FirstOrDefault(m => m.UserId == userId &&
                m.MovieId == movieId);


                if (target == null)
                {
                    db.UserCommentMovies.Add(new UserCommentMovie
                    {
                        UserId = userId,
                        MovieId = movieId,
                        Score = score,
                        AddTime = now,
                        UpdateTime = now
                    });
                }
                else
                {
                    target.Score = score;
                    target.UpdateTime = now;
                }

                db.SaveChanges();
            }
        }
    }
}