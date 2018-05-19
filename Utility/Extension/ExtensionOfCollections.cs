using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Data;

namespace Lck.Utility.Extensions
{
    public static class ExtensionOfCollections
    {
        public static IEnumerable<T> GetPartialByTargetRate<T>(this IEnumerable<T> list, decimal rate)
        {
            //rate 請填寫 0.01~0.99 之間的數字  
            //e.g. 假如填寫0.6 代表是取前60%的資料
            if (rate > 1 || rate <= 0)
                return list;

            decimal total = list.Count();
            int takeSize = (int)(total * rate);

            if (takeSize <= 0)
                takeSize = 1;

            return list.Take(takeSize);
        }

        

        /// <summary>
        /// 將單筆 int、long、string 等型別轉成 list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static List<T> ToListCollection<T>(this T inputValue) where T: IComparable
        {
            List<T> listResult = new List<T>();
            listResult.Add(inputValue);

            return listResult;
        }
        


        /// <summary>
        /// 將集合插入指定的筆數，但會做防呆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="insertItems"></param>
        public static void InsertWithCatch<T>(this List<T> list, int index , List<T> insertItems)
        {
            try
            {
                list.InsertRange(index , insertItems);
            }
            catch
            {
                int numberOfList = list.Count();
                list.InsertRange((numberOfList), insertItems);
            }
        }



        public static T GetMatchOrFirstDefault<T>(this IEnumerable<T> list , Func<T, bool> func) where T: class
        {
            if (list.IsEmptyOrNull())
                return null;

            var matchFirst = list.FirstOrDefault(func);

            if (matchFirst != null)
                return matchFirst;


            return list.FirstOrDefault();
        }




        public static string GetDictionaryOrEmptyValue<T>(this Dictionary<T, string> dic , T key)
        {
            if (dic.ContainsKey(key) == false)
                return string.Empty;

            return dic[key];
        }


        /// <summary>
        /// 若找不到指定的值，返回預設第一筆
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetDictionaryOrDefaultFirstValue(this Dictionary<int, string> dic, int key)
        {
            if (dic.IsEmptyOrNull())
                return string.Empty;

            if (dic.ContainsKey(key) == false)
                return dic.FirstOrDefault().Value;


            return dic[key];
        }
        

        /// <summary>
        /// 轉成dictionary
        /// </summary>
        public static Dictionary<string ,object> ToObjectDictionary<T>(this T model) where T: class
        {
            return model.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(model, null));
        }
        

        /// <summary>
        /// 找出有重複的集合
        /// </summary>
        public static IList<T> FindDuplicateItems<T, TKey>(this IList<T> list, Func<T, TKey> func)
        {
            var query = list.GroupBy(func).Select(x => new 
            {
                count = x.Count(),
                model = x.First()

            }).ToList();

            return query.Where(x => x.count > 1).Select(x => x.model).ToList();
        }


        /// <summary>
        /// Distanct第一筆
        /// </summary>
        public static List<T> DistinctBy<T, TKey>(this IList<T> list, Func<T, TKey> func)
        {
            if (list.IsEmptyOrNull())
                return new List<T>();


            return list.GroupBy(func).Select(x=> x.First()).ToList();
        }


        /// <summary>
        /// Distanct第一筆
        /// </summary>
        public static List<T> DistinctOrderBy<T, TKey, TColumn>(this IList<T> list, Func<T, TKey> func, Func<T, TColumn> orderBy)
        {
            if (list.IsEmptyOrNull())
                return new List<T>();


            return list.GroupBy(func).Select(x => x.OrderBy(orderBy).First()).ToList();
        }


        /// <summary>
        /// Distanct 後 每組group隨機取一筆
        /// </summary>
        public static IList<T> SelectDistinctRandomOne<T, TKey>(this IList<T> list, Func<T, TKey> func)
        {
            if (list.IsEmptyOrNull())
                return new List<T>();

            return list.GroupBy(func).Select(x => x.ToList().SortByRandom().First()).ToList();
        }




        /// <summary>
        /// 取得這是哪個Type的List
        /// </summary>
        public static System.Type GetListType<T>(this IEnumerable<T> _)
        {
            return typeof(T);
        }


        /// <summary>
        /// 亂數排序List 
        /// </summary>
        public static List<T> SortByRandom<T>(this IList<T> input, int displayLength = 0)
        {
            if (input.IsEmptyOrNull())
                return new List<T>();

            Random r = new Random();

            if (displayLength > 0)
                return input.OrderBy(x => r.Next()).Take(displayLength).ToList();
            else
                return input.OrderBy(x => r.Next()).ToList();
        }



        /// <summary>
        /// 判斷List是否為null或Count == 0
        /// </summary>
        public static bool IsEmptyOrNull<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return true; // or throw an exception

            if (source.Any())
                return false;

            return true;
        }

        
        /// <summary>
        /// 判斷List是否不為空 (不為null ＆＆　筆數大於０)
        /// </summary>
        public static bool IsNotEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return false; // or throw an exception

            return source.Any();
        }


        /// <summary>
        /// Linq 進行 OrderBy 設定
        /// </summary>
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> q, string SortField, bool Descending)
        {
            SortField = SortField.Trim();
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, SortField);
            var exp = Expression.Lambda(prop, param);
            string method = Descending ? "OrderByDescending" : "OrderBy";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }

        public static DataTable ConvertToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
    }

}
