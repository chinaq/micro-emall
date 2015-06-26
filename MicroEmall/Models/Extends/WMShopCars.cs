using System;
using System.Collections.Generic;
using System.Linq;
using Jumpcity.Utility.Extend;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMShopCars : ShopCars
    {
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

        public double SubTotal
        {
            get { return this.Price * this.Count; }
        }

        public bool Add()
        {
            if (this.Valid())
            {
                this.Id = General.UniqueString(this.Id);
                this.AddDate = DateTime.Now;

                using (WMContext context = new WMContext())
                {
                    ShopCars model = (
                        from car in context.ShopCars
                        where car.UserId.Equals(this.UserId)
                           && car.GoodId.Equals(this.GoodId)
                        select car
                    ).FirstOrDefault();

                    if (model != null)
                    {
                        model.Price = this.Price;
                        model.Count += this.Count;
                    }
                    else
                    {
                        model = new ShopCars { 
                            Id = this.Id,
                            GoodId = this.GoodId,
                            UserId = this.UserId,
                            Price = this.Price,
                            Count = this.Count,
                            AddDate = this.AddDate
                        };

                        context.ShopCars.Add(model);
                    }

                    context.SaveChanges();
                }

                return true;
            }

            return false;
        }

        public static bool UpdateCount(string id, int count = 1)
        {
            if (!General.IsNullable(id))
            {
                using (WMContext context = new WMContext())
                {
                    ShopCars model = context.ShopCars.Find(id);

                    if (model != null)
                    {
                        if ((model.Count + count) > 0)
                        {
                            model.Count += count;
                            context.SaveChanges();
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool Delete(string id)
        {
            if (!General.IsNullable(id))
            {
                using (WMContext context = new WMContext())
                {
                    ShopCars model = context.ShopCars.Find(id);

                    if (model != null)
                    {
                        context.ShopCars.Remove(model);
                        context.SaveChanges();
                        return false;
                    }
                }
            }

            return false;
        }

        public static bool Clear(string userId)
        {
            if (!General.IsNullable(userId))
            {
                using (WMContext context = new WMContext())
                {
                    var list = (
                        from car in context.ShopCars
                        where car.UserId.Equals(userId)
                        select car
                    );

                    if (list != null)
                    {
                        foreach (var item in list)
                            context.ShopCars.Remove(item);

                        context.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        public static double GetSum(string userId)
        {
            double sum = 0;

            if (!General.IsNullable(userId))
            {
                using (WMContext context = new WMContext())
                {
                    sum = (
                        from car in context.ShopCars
                        where car.UserId.Equals(userId)
                        select car.Price * car.Count
                    ).Sum();
                }
            }

            return sum;
        }

        public static WMShopCars Get(string id)
        {
            WMShopCars model = null;

            if (!General.IsNullable(id))
            {
                using (WMContext context = new WMContext())
                {
                    model = (
                        from car in context.ShopCars
                        join g in context.Goods on car.GoodId equals g.Id
                        where car.Id.Equals(id)
                        select new WMShopCars
                        {
                            Id = car.Id,
                            GoodId = car.GoodId,
                            GoodName = g.Name,
                            GoodFigure = context.GoodImages.Where(gi => gi.GoodId.Equals(car.GoodId) && gi.IsCover).Select(gi => gi.URL).FirstOrDefault(),
                            UserId = car.UserId,
                            Price = car.Price,
                            Count = car.Count,
                            AddDate = car.AddDate
                        }
                    ).FirstOrDefault();
                }
            }

            return model;
        }

        public static List<WMShopCars> GetList(out int pageCount, string userId, int pageIndex = 0, int pageSize = 0)
        {
            List<WMShopCars> list = null;
            pageCount = 0;

            using (WMContext context = new WMContext())
            {
                var query = (
                    from car in context.ShopCars
                    join g in context.Goods on car.GoodId equals g.Id
                    where car.UserId.Equals(userId)
                    orderby car.AddDate descending
                    select new WMShopCars
                    {
                        Id = car.Id,
                        GoodId = car.GoodId,
                        GoodName = g.Name,
                        GoodFigure = context.GoodImages.Where(gi => gi.GoodId.Equals(car.GoodId) && gi.IsCover).Select(gi => gi.URL).FirstOrDefault(),
                        UserId = car.UserId,
                        Price = car.Price,
                        Count = car.Count,
                        AddDate = car.AddDate
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
                !General.IsNullable(this.GoodId)
             && !General.IsNullable(this.UserId)
             && this.Price >= 0
             && this.Count > 0
             && (checkId ? !General.IsNullable(this.Id) : true)
            );
        }
    }
}
