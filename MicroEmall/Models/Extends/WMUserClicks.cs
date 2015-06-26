using System;
using System.Collections.Generic;
using System.Linq;
using Jumpcity.Utility.Extend;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMUserClicks : UserClicks
    {
        public string PromoterName
        {
            get;
            protected set;
        }

        public string CustomerName
        {
            get;
            protected set;
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
                this.Id = General.UniqueString(this.Id);
                this.AddDate = DateTime.Now;

                using (WMContext context = new WMContext())
                {
                    Goods good = context.Goods.Find(this.GoodId);

                    if (good != null)
                    {
                        UserClicks model = (
                            from uc in context.UserClicks
                            where uc.PromoterId.Equals(this.PromoterId)
                               && uc.CustomerId.Equals(this.CustomerId)
                               && uc.GoodId.Equals(this.GoodId)
                            select uc
                        ).FirstOrDefault();

                        if (model == null)
                        {
                            model = new UserClicks
                            {
                                Id = this.Id,
                                PromoterId = this.PromoterId,
                                CustomerId = this.CustomerId,
                                GoodId = this.GoodId,
                                AddDate = this.AddDate
                            };

                            good.Clicks++;
                            context.UserClicks.Add(model);
                            context.SaveChanges();
                        }
                    }
                }

                return true;
            }

            return false;
        }

        public static int GetClickCount(string promoterId, string goodId = null)
        {
            int count = 0;

            if (!General.IsNullable(promoterId))
            {
                bool isGood = !General.IsNullable(goodId);

                using (WMContext context = new WMContext())
                {
                    count = (
                        from uc in context.UserClicks
                        where uc.PromoterId.Equals(promoterId)
                           && (isGood ? uc.GoodId.Equals(goodId) : true)
                        select uc.Id
                    ).Count();
                }
            }

            return count;
        }

        public static double GetClickBonus(string promoterId, string goodId)
        {
            double clickBonus = 0;

            if (!General.IsNullable(promoterId) && !General.IsNullable(goodId))
            {
                int count = GetClickCount(promoterId, goodId);
                WMGoods good = WMGoods.Get(goodId);

                if (good != null && good.Clicks > 0)
                    clickBonus = ((count / good.Clicks) * good.ClickBonuses);
            }

            return clickBonus;
        }

        public static List<WMUserClicks> GetList(out int pageCount, string promoterId = null, int pageIndex = 0, int pageSize = 0)
        {
            List<WMUserClicks> list = null;
            bool flag = !General.IsNullable(promoterId);
            pageCount = 0;

            using (WMContext context = new WMContext())
            {
                var query = (
                    from uc in context.UserClicks
                    join p in context.Users on uc.PromoterId equals p.Id
                    join c in context.Users on uc.CustomerId equals c.Id
                    join g in context.Goods on uc.GoodId equals g.Id
                    where (flag ? uc.PromoterId.Equals(promoterId) : true)
                    orderby uc.AddDate descending
                    select new WMUserClicks
                    {
                        Id = uc.Id,
                        PromoterId = uc.PromoterId,
                        PromoterName = p.NickName,
                        CustomerId = uc.CustomerId,
                        CustomerName = c.NickName,
                        GoodId = uc.GoodId,
                        GoodName = g.Name,
                        GoodFigure = context.GoodImages.Where(gi => gi.GoodId.Equals(uc.GoodId) && gi.IsCover).Select(gi => gi.URL).FirstOrDefault(),
                        AddDate = uc.AddDate
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
                !General.IsNullable(this.PromoterId)
             && !General.IsNullable(this.CustomerId)
             && !General.IsNullable(this.GoodId)
             && (checkId ? !General.IsNullable(this.Id) : true)
            );
        }
    }
}
