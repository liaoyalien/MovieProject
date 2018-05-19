using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lck.Kernel;

using Model.DB;

using Repository;

namespace Spider.Service
{
    public abstract class AbstractXmdbService
    {
        public string ApiUrl { set; get; }
        public string ApiKey { set; get; }

        public string ApiName { set; get; }
        public ApiLatestUpdateRepository ApiLatestUpdateRepository = new ApiLatestUpdateRepository();

        /// <summary>
        /// 檢查totalPages是否跟現在抓到的api totalPages是一樣的值，如果不是就要重新再爬資料
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="methodName"></param>
        /// <param name="totalPages"></param>
        /// <returns></returns>
        
        public void UpdatePage(string apiName, string methodName, int currentPage)
        {
            var keyFilter = GetApiLatestUpdateModel(apiName, methodName);
            this.ApiLatestUpdateRepository.UpdatePage(keyFilter, currentPage);
        }

        /// <summary>
        /// 一定會有資料回傳，否則throw exception
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public ApiLatestUpdateModel GetApiLatestUpdateModel(string apiName, string methodName)
        {
            var filter = new ApiLatestUpdateModel { ApiName = apiName, MethodName = methodName };
            
            return filter;
        }

    }
}
