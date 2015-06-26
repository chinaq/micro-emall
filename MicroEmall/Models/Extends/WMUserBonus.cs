using System;
using System.Collections.Generic;
using System.Linq;
using Jumpcity.Utility.Extend;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMUserBonus : UserBonus
    {
        public bool Add()
        {
            if (this.Valid())
            {
                this.Id = General.UniqueString(this.Id);
                this.AddDate = DateTime.Now;

                using (WMContext context = new WMContext())
                {
                    UserBonus model = new UserBonus { 
                        Id = this.Id,
                        UserId = this.UserId,
                        OrderId = this.OrderId,
                        BonusSum = this.BonusSum,
                        AddDate = this.AddDate
                    };

                    context.UserBonus.Add(model);
                    context.SaveChanges();
                }

                return true;
            }

            return false;
        }

        public static WMUserBonus Get(string id)
        {
            WMUserBonus model = null;

            if (!General.IsNullable(id))
            {
                using(WMContext context = new WMContext())
	            {
                    model = (
                        from ub in context.UserBonus
                        where ub.Id.Equals(id)
                        select new WMUserBonus 
                        {
                            Id = ub.Id,
                            UserId = ub.UserId,
                            OrderId = ub.OrderId,
                            BonusSum = ub.BonusSum,
                            AddDate = ub.AddDate
                        }
                    ).FirstOrDefault();
	            }
            }

            return model;
        }

        public static List<WMUserBonus> GetList(out int pageCount, DateTime? minDate, DateTime? maxDate, string userId = null, int pageIndex = 0, int pageSize = 0)
        {
            List<WMUserBonus> list = null;
            bool isUser = !General.IsNullable(userId);
            DateTime min = (minDate.HasValue ? minDate.Value : DateTime.MinValue);
            DateTime max = (maxDate.HasValue ? maxDate.Value : DateTime.MaxValue);
            pageCount = 0;

            using (WMContext context = new WMContext())
            {
                var query = (
                    from ub in context.UserBonus
                    where (isUser ? ub.UserId.Equals(userId) : true)
                       && ub.AddDate >= min
                       && ub.AddDate <= max
                    orderby ub.AddDate descending
                    select new WMUserBonus
                    {
                        Id = ub.Id,
                        UserId = ub.UserId,
                        OrderId = ub.OrderId,
                        BonusSum = ub.BonusSum,
                        AddDate = ub.AddDate
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
                !General.IsNullable(this.UserId)
             && !General.IsNullable(this.OrderId)
             && this.BonusSum > 0
             && (checkId ? !General.IsNullable(this.Id) : true)
            );
        }
    }
}
