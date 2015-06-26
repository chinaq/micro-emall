using System;
using System.Collections.Generic;
using System.Linq;
using Jumpcity.Utility.Extend;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMOrderGoods : OrderGoods
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

        /// <summary>
        /// 获取商品的积分小计
        /// </summary>
        public int GoodInteSubTotal
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

                using (WMContext context = new WMContext())
                {
                    OrderGoods model = new OrderGoods {
                        Id = this.Id,
                        OrderId = this.OrderId,
                        GoodId = this.GoodId,
                        Price = this.Price,
                        Count = this.Count
                    };

                    context.OrderGoods.Add(model);
                    context.SaveChanges();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 用购物车的数据填充订单商品表
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="shopCars">购物车数据</param>
        /// <returns>添加成功返回TRUE，否则返回FALSE</returns>
        public static bool AddList(string orderId, List<WMShopCars> shopCars)
        {
            if (!General.IsNullable(orderId) && !General.IsNullable(shopCars))
            {
                WMOrderGoods model = null;
                using (WMContext context = new WMContext())
                {
                    foreach (WMShopCars item in shopCars)
                    {
                        model = new WMOrderGoods { 
                            Id = General.UniqueString(),
                            OrderId = orderId,
                            GoodId = item.GoodId,
                            Price = item.Price,
                            Count = item.Count
                        };

                        context.OrderGoods.Add(model);
                    }

                    context.SaveChanges();
                }

                return true;
            }

            return false;
        }

        public static double GetSum(string orderId)
        {
            double sum = 0;

            if (!General.IsNullable(orderId))
            {
                using (WMContext context = new WMContext())
                {
                    sum = (
                        from og in context.OrderGoods
                        where og.OrderId.Equals(orderId)
                        select og.Price * og.Count
                    ).Sum();
                }
            }

            return sum;
        }

        /// <summary>
        /// 获取指定订单的购买商品总数
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns>返回指定订单的购买商品总数</returns>
        public static int GetCount(string orderId)
        {
            int count = 0;

            if (!General.IsNullable(orderId))
            {
                using (WMContext context = new WMContext())
                {
                    count = (
                        from og in context.OrderGoods
                        where og.OrderId.Equals(orderId)
                        select og.Count
                    ).Sum();
                }
            }

            return count;
        }

        /// <summary>
        /// 获取指定订单的购买商品总积分
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns>返回指定订单的购买商品总积分</returns>
        public static int GetIntegralSum(string orderId)
        {
            int inteSum = 0;

            if (!General.IsNullable(orderId))
            {
                using (WMContext context = new WMContext())
                {
                    inteSum = (
                        from og in context.OrderGoods
                        join g in context.Goods on og.GoodId equals g.Id
                        where og.OrderId.Equals(orderId)
                        select g.Integral * og.Count
                    ).Sum();
                }
            }

            return inteSum;
        }

        /// <summary>
        /// 获取指定订单的商品分红总金额
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns>返回指定订单的商品分红总金额</returns>
        public static double GetBonusSum(string orderId)
        {
            double bonusSum = 0;

            if (!General.IsNullable(orderId))
            {
                using (WMContext context = new WMContext())
                {
                    bonusSum = (
                        from og in context.OrderGoods
                        join g in context.Goods on og.GoodId equals g.Id
                        where og.OrderId.Equals(orderId)
                        select ((og.Price * g.Bonus) / 100.0) * og.Count
                    ).Sum();
                }
            }

            return bonusSum;
        }

        public static WMOrderGoods Get(string id)
        {
            WMOrderGoods model = null;

            if (!General.IsNullable(id))
            {
                using (WMContext context = new WMContext())
                {
                    model = (
                        from og in context.OrderGoods
                        join g in context.Goods on og.GoodId equals g.Id
                        where og.Id.Equals(id)
                        select new WMOrderGoods
                        {
                            Id = og.Id,
                            OrderId = og.OrderId,
                            GoodId = og.GoodId,
                            GoodName = g.Name,
                            GoodFigure = context.GoodImages.Where(gi => gi.GoodId.Equals(og.GoodId) && gi.IsCover).Select(gi => gi.URL).FirstOrDefault(),
                            GoodInteSubTotal = (g.Integral * og.Count),
                            Price = og.Price,
                            Count = og.Count
                        }
                    ).FirstOrDefault();
                }
            }

            return model;
        }

        public static List<WMOrderGoods> GetList(out int pageCount, string orderId, int pageIndex = 0, int pageSize = 0)
        {
            List<WMOrderGoods> list = null;
            pageCount = 0;

            using (WMContext context = new WMContext())
            {
                var query = (
                    from og in context.OrderGoods
                    join g in context.Goods on og.GoodId equals g.Id
                    where og.OrderId.Equals(orderId)
                    orderby (og.Price * og.Count) descending
                    select new WMOrderGoods
                    {
                        Id = og.Id,
                        OrderId = og.OrderId,
                        GoodId = og.GoodId,
                        GoodName = g.Name,
                        GoodFigure = context.GoodImages.Where(gi => gi.GoodId.Equals(og.GoodId) && gi.IsCover).Select(gi => gi.URL).FirstOrDefault(),
                        GoodInteSubTotal = (g.Integral * og.Count),
                        Price = og.Price,
                        Count = og.Count
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
             && !General.IsNullable(this.OrderId)
             && this.Price >= 0
             && this.Count > 0
             && (checkId ? !General.IsNullable(this.Id) : true)
            );
        }
    }
}
