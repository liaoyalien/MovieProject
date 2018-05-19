using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lck.Models.Base
{
    public class PagingParameterModel
    {
        /// <summary>
        /// 每頁筆數 若為0筆取得所有資料
        /// </summary>
        public int TakeSize { get; set; }
        /// <summary>
        /// 當前第幾頁
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 排序方法
        /// </summary>
        public string OrderBy { get; set; }
        /// <summary>
        /// 是否為ASC排序方式
        /// </summary>
        public bool IsASC { get; set; }//排序列    
        /// <summary>
        /// 過濾筆數
        /// </summary>
        public int SkipSize
        {
            get
            {
                if (Page <= 1)
                    return 0;

                return (Page - 1) * TakeSize;
            }
        }
    }
}
