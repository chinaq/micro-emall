using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.SqlClient;
using Jumpcity.Utility.Extend;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMUserSets : UserSets
    {
        public string TypeName
        {
            get;
            protected set;
        }

        /// <summary>
        /// 获取用户对指定商品的当前操作是否已过期
        /// </summary>
        public bool Expired
        {
            get { return (DateTime.Now - this.AddDate).TotalHours >= 720.0; }
        }

        public string GoodName
        {
            get;
            protected set;
        }

        public string GoodFigure
        {
            get;
            protected set;
        }

        public bool Add()
        {
            if (this.Valid())
            {
                this.AddDate = DateTime.Now;

                using (WMContext context = new WMContext())
                {
                    UserSets model = (
                        from us in context.UserSets
                        where us.TypeId == this.TypeId
                            && us.UserId.Equals(this.UserId)
                            && us.GoodId.Equals(this.GoodId)
                        select us
                    ).FirstOrDefault();

                    if (model != null)
                        model.AddDate = this.AddDate;
                    else
                    {
                        model = new UserSets
                        {
                            Id = General.UniqueString(this.Id),
                            TypeId = this.TypeId,
                            UserId = this.UserId,
                            GoodId = this.GoodId,
                            AddDate = this.AddDate
                        };

                        context.UserSets.Add(model);

                        if (this.TypeId == 401)
                        {
                            //商品的被收藏数量获得递增
                            Goods good = context.Goods.Find(this.GoodId);
                            if (good != null)
                                good.Saves++;
                        }
                    }

                    context.SaveChanges();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 判断指定用户是否对指定商品执行过指定的操作
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="goodId">商品ID</param>
        /// <param name="typeId">操作ID</param>
        /// <returns>已操作过返回TRUE，否则返回FALSE</returns>
        public static bool Operated(string userId, string goodId, int typeId)
        {
            int count = 0;

            if (!General.IsNullable(userId) && !General.IsNullable(goodId) && typeId > 0)
            {
                using (WMContext context = new WMContext())
                {
                    count = (
                        from us in context.UserSets
                        where us.TypeId == typeId
                           && us.UserId.Equals(userId)
                           && us.GoodId.Equals(goodId)
                        select us.Id
                    ).Count();
                }
            }

            return count > 0;
        }

        /// <summary>
        /// 判断指定用户是否对指定商品执行过收藏操作
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="goodId">商品ID</param>
        /// <returns>已操作过返回TRUE，否则返回FALSE</returns>
        public static bool Saved(string userId, string goodId)
        {
            return Operated(userId, goodId, 401);
        }

        /// <summary>
        /// 判断指定用户是否对指定商品执行过分享操作
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="goodId">商品ID</param>
        /// <returns>已操作过返回TRUE，否则返回FALSE</returns>
        public static bool Shared(string userId, string goodId)
        {
            return Operated(userId, goodId, 402);
        }

        public static bool Delete(string id)
        {
            if (!General.IsNullable(id))
            {
                using (WMContext context = new WMContext())
                {
                    UserSets model = context.UserSets.Find(id);

                    if (model != null)
                    {
                        context.UserSets.Remove(model);
                        context.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        public static WMUserSets Get(string id)
        {
            WMUserSets userSet = null;

            if (!General.IsNullable(id))
            {
                DateTime now = DateTime.Now;
                using (WMContext context = new WMContext())
                {
                    userSet = (
                        from us in context.UserSets
                        join g in context.Goods on us.GoodId equals g.Id
                        join t in context.Options on us.TypeId equals t.Id
                        where us.Id.Equals(id)
                        select new WMUserSets
                        {
                            Id = us.Id,
                            TypeId = us.TypeId,
                            TypeName = t.Name,
                            UserId = us.UserId,
                            GoodId = us.GoodId,
                            GoodName = g.Name,
                            GoodFigure = context.GoodImages.Where(gi => gi.GoodId.Equals(us.GoodId) && gi.IsCover).Select(gi => gi.URL).FirstOrDefault(),
                            AddDate = us.AddDate
                        }
                    ).FirstOrDefault();
                }
            }

            return userSet;
        }

        public static List<WMUserSets> GetList(out int pageCount, int typeId = 0, string userId = null, int pageIndex = 0, int pageSize = 0)
        {
            List<WMUserSets> list = null;
            bool flag = !General.IsNullable(userId);
            pageCount = 0;
            DateTime now = DateTime.Now;

            using (WMContext context = new WMContext())
            {
                var query = (
                    from us in context.UserSets
                    join g in context.Goods on us.GoodId equals g.Id
                    join t in context.Options on us.TypeId equals t.Id
                    where (typeId > 0 ? us.TypeId.Equals(typeId) : true)
                       && (flag ? us.UserId.Equals(userId) : true)
                    orderby us.AddDate descending
                    select new WMUserSets
                    {
                        Id = us.Id,
                        TypeId = us.TypeId,
                        TypeName = t.Name,
                        UserId = us.UserId,
                        GoodId = us.GoodId,
                        GoodName = g.Name,
                        GoodFigure = context.GoodImages.Where(gi => gi.GoodId.Equals(us.GoodId) && gi.IsCover).Select(gi => gi.URL).FirstOrDefault(),
                        AddDate = us.AddDate
                    }
                );

                if (query != null)
                {
                    pageCount = query.Count();

                    if (pageIndex >= 0 && pageSize > 0)
                        query = query.Skip(pageIndex * pageSize).Take(pageSize);

                    list = query.ToList();
                }
            }

            return list;
        }

        private bool Valid(bool checkId = false)
        {
            return (
                this.TypeId > 0
             && !General.IsNullable(this.UserId)
             && !General.IsNullable(this.GoodId)
             && (checkId ? !General.IsNullable(this.Id) : true)
            );
        }
    }
}
