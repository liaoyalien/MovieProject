//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FavoriteMovie.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserCommentMovie
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public decimal Score { get; set; }
        public System.DateTime AddTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
    }
}
