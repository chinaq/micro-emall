using System;
using System.Collections.Generic;
using System.Linq;
using Jumpcity.Utility.Extend;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMUserIntegrals : UserIntegrals
    {
        public string SourceName
        {
            get;
            protected set;
        }

        public bool Add()
        {
            if (this.Valid())
            {
                this.Id = General.UniqueString(this.Id);
                this.AddDate = DateTime.Now;

                using (WMContext context = new WMContext())
                {
                    UserIntegrals model = new UserIntegrals
                    {
                        Id = this.Id,
                        SourceId = this.SourceId,
                        UserId = this.UserId,
                        Integral = this.Integral,
                        AddDate = this.AddDate
                    };

                    context.UserIntegrals.Add(model);
                    context.SaveChanges();
                }

                return WMUsers.UpdateIntegral(this.UserId, this.Integral);
            }

            return false;
        }

        public static bool Delete(string id)
        {
            if (!General.IsNullable(id))
            {
                using (WMContext context = new WMContext())
                {
                    UserIntegrals model = context.UserIntegrals.Find(id);

                    if (model != null)
                    {
                        context.UserIntegrals.Remove(model);
                        context.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        public static UserIntegrals Get(string id)
        {
            UserIntegrals integral = null;

            if (!General.IsNullable(id))
            {
                using (WMContext context = new WMContext())
                {
                    integral = (
                        from ui in context.UserIntegrals
                        join s in context.Options on ui.SourceId equals s.Id
                        where ui.Id.Equals(id)
                        select new WMUserIntegrals
                        {
                            Id = ui.Id,
                            SourceId = ui.SourceId,
                            SourceName = s.Name,
                            UserId = ui.UserId,
                            Integral = ui.Integral,
                            AddDate = ui.AddDate
                        }
                    ).FirstOrDefault();
                }
            }

            return integral;
        }

        public static List<WMUserIntegrals> GetList(out int pageCount, int sourceId = 0, string userId = null, int pageIndex = 0, int pageSize = 0)
        {
            List<WMUserIntegrals> list = null;
            bool flag = !General.IsNullable(userId);
            pageCount = 0;

            using (WMContext context = new WMContext())
            {
                var query = (
                    from ui in context.UserIntegrals
                    join s in context.Options on ui.SourceId equals s.Id
                    where (sourceId > 0 ? ui.SourceId.Equals(sourceId) : true)
                       && (flag ? ui.UserId.Equals(userId) : true)
                    orderby ui.AddDate descending
                    select new WMUserIntegrals
                    {
                        Id = ui.Id,
                        SourceId = ui.SourceId,
                        SourceName = s.Name,
                        UserId = ui.UserId,
                        Integral = ui.Integral,
                        AddDate = ui.AddDate
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
                this.SourceId > 0
             && !General.IsNullable(this.UserId)
             && (checkId ? !General.IsNullable(this.Id) : true)
            );
        }
    }
}
