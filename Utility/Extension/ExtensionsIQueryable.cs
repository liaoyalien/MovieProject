using Lck.Enums;
using Lck.Models;
using Lck.Models.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lck.Utility.Extensions
{

    public static class ExtensionsIQueryable
    {
        public static IQueryable<T> DoPaging<T>(this IQueryable<T> iqJoin, PaginationModel pager) where T : new()
        {
            if (string.IsNullOrWhiteSpace(pager.SortBy))
                throw new Exception("No sort by");

            bool isDESC = pager.Order == SortDirectionEnum.DESC;

            int skipCount = (pager.Page - 1) * pager.PerPage;

            return iqJoin.OrderByField(pager.SortBy, isDESC).Skip(skipCount).Take(pager.PerPage);
        }
        
        /// <summary>
        /// 分頁
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iq"></param>
        /// <param name="pagination">TakeSize為0時取得所有資料</param>
        /// <returns></returns>
        public static PagingResult<T> ToPagingResult<T>(this IQueryable<T> iq, PaginationModel pagination = null)
            where T : new()
        {
            int count = iq.Count();
            if (pagination == null || pagination.PerPage == 0)
            {
                return new PagingResult<T>
                {
                    Items = iq.ToList(),
                    Pagination = new PaginationModel
                    {
                        Total = count,
                        PerPage = count,
                        TotalPage = 1,
                        Page = 1
                    }
                };
            }

            if (pagination.Page <= 0)
                pagination.Page = 1;

            //不開放取太多筆
            if (pagination.PerPage >= 1000)
                pagination.PerPage = 1000;

            int totalPage = count / pagination.PerPage;
            PagingResult<T> pagingResult = new PagingResult<T>()
            {
                Items = iq.DoPaging(pagination).ToList(),
                Pagination = new PaginationModel
                {
                    Order = pagination.Order,
                    Page = pagination.Page,
                    PerPage = pagination.PerPage,
                    SortBy = pagination.SortBy,
                    Total = count,
                    TotalPage = count % pagination.PerPage == 0 ? totalPage : totalPage + 1
                }
            };
            return pagingResult;
        }
        /// <summary>
        /// 取得分頁並回傳指定類別S
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="iq"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public static PagingResult<S> ToPagingResult<T, S>(this IQueryable<T> iq, PaginationModel pager = null)
            where T : new()
            where S : new()
        {
            PagingResult<T> pageing = iq.ToPagingResult(pager);

            PagingResult<S> pagingResult = new PagingResult<S>()
            {
                Items = pageing.Items.ReturnInjectListObject<T, S>(),
                Pagination = pageing.Pagination
            };
            return pagingResult;
        }
    }
}
