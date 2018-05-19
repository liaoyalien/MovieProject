using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lck.Utility.Extensions;

using Model.DB;

using Tables;

namespace Repository
{
    public class ApiLatestUpdateRepository: BaseRepository
    {
        public List<ApiLatestUpdateModel> GetByFilter(ApiLatestUpdateModel model)
        {
           return _GetByFilter(model)
               .Select(m => m.ReturnInjectObjectNullable<ApiLatestUpdateModel>())
                .ToList();
        }

        public void UpdatePage(ApiLatestUpdateModel model, int currentPage)
        {
            var result = _GetByFilter(model).FirstOrDefault();
            result.CurrentPages = currentPage;
            this.db.SaveChanges();
        }

        public void UpdateTotalPage(ApiLatestUpdateModel model, int totalPage)
        {
            var result = _GetByFilter(model).FirstOrDefault();
            result.TotalPages = totalPage;
            result.CurrentPages = 1;
            this.db.SaveChanges();
        }

        private IEnumerable<ApiLatestUpdate> _GetByFilter(ApiLatestUpdateModel model)
        {
            IQueryable<ApiLatestUpdate> iq = this.db.ApiLatestUpdates;

            if (!string.IsNullOrWhiteSpace(model.ApiName))
                iq = iq.Where(m => m.ApiName == model.ApiName);

            if (!string.IsNullOrWhiteSpace(model.MethodName))
                iq = iq.Where(m => m.MethodName == model.MethodName);

            return iq;
        }
    }
}
