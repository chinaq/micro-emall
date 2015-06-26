using System;
using System.Collections.Generic;
using System.Linq;
using Jumpcity.Utility.Extend;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMOrders : Orders
    {
        private double _goodSum = 0;
        private int _goodCount = 0;
        private int _goodInteSum = 0;

        /// <summary>
        /// 获取该订单的商品总金额
        /// </summary>
        public double GoodSum
        {
            get 
            {
                if(_goodSum == 0)
                    _goodSum = WMOrderGoods.GetSum(this.Id);
                return _goodSum;
            }
        }

        /// <summary>
        /// 获取该订单的购买商品总数
        /// </summary>
        public int GoodCount
        {
            get
            {
                if (_goodCount == 0)
                    _goodCount = WMOrderGoods.GetCount(this.Id);
                return _goodCount;
            }
        }

        /// <summary>
        /// 获取该订单的购买商品总积分
        /// </summary>
        public int GoodInteSum
        {
            get
            {
                if (_goodInteSum == 0)
                    _goodInteSum = WMOrderGoods.GetIntegralSum(this.Id);
                return _goodInteSum;
            }
        }
        
        /// <summary>
        /// 获取该订单的总金额
        /// </summary>
        public double OrderSum
        {
            get { return this.GoodSum + this.Freight; }
        }

        public string NickName
        {
            get;
            protected set;
        }

        public string ProvinceName
        {
            get;
            protected set;
        }

        public string CityName
        {
            get;
            protected set;
        }

        public string AreaName
        {
            get;
            protected set;
        }

        public string PayName
        {
            get;
            protected set;
        }

        public string StatusName
        {
            get;
            protected set;
        }

        public bool Add()
        {
            if (this.Valid())
            {
                this.Id = Guid.NewGuid().ToUniqueNumber().ToString();
                this.AddDate = DateTime.Now;

                using (WMContext context = new WMContext())
                {
                    Orders model = new Orders { 
                        Id = this.Id,
                        UserId = this.UserId,
                        ProvinceId = this.ProvinceId,
                        CityId = this.CityId,
                        AreaId = this.AreaId,
                        Contact = this.Contact,
                        Address = this.Address,
                        Phone = this.Phone,
                        ZipCode = this.ZipCode,
                        PayId = this.PayId,
                        Paid = this.Paid,
                        Freight = this.Freight,
                        Message = this.Message,
                        StatusId = this.StatusId,
                        AddDate = this.AddDate
                    };

                    context.Orders.Add(model);
                    context.SaveChanges();
                }

                return true;
            }

            return false;
        }

        public static bool UpdateStatus(string orderId, int state)
        {
            if (!General.IsNullable(orderId) && state > 0)
            {
                using (WMContext context = new WMContext())
                {
                    Orders model = context.Orders.Find(orderId);

                    if (model != null)
                    {
                        model.StatusId = state;
                        context.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 为指定的订单付款，并将相应的购买积分添加到用户的账户中
        /// </summary>
        /// <param name="orderId">要付款的订单ID</param>
        /// <returns>付款成功返回TRUE，否则返回FALSE</returns>
        public static bool Payment(string orderId)
        {
            bool flag = false;

            if (!General.IsNullable(orderId))
            {
                string userId = null;

                using (WMContext context = new WMContext())
                {
                    Orders model = context.Orders.Find(orderId);

                    if (model != null)
                    {
                        userId = model.UserId;
                        model.Paid = true;
                        context.SaveChanges();
                        flag = true;
                    }
                }

                if (flag && !General.IsNullable(userId))
                {
                    int inteSum = WMOrderGoods.GetIntegralSum(orderId);
                    return new WMUserIntegrals { 
                        UserId = userId,
                        Integral = inteSum
                    }.Add();
                }
            }

            return flag;
        }

        public static int GetPayCount(string goodId)
        {
            int payConut = 0;

            if (!General.IsNullable(goodId))
            {
                using (WMContext context = new WMContext())
                {
                    payConut = (
                        from og in context.OrderGoods
                        join o in context.Orders on og.OrderId equals o.Id
                        where o.Paid == true
                           && og.GoodId.Equals(goodId)
                        select og.Count
                    ).Sum();
                }
            }

            return payConut;
        }

        public static WMOrders Get(string orderId)
        {
            WMOrders order = null;

            if (!General.IsNullable(orderId))
            {
                using (WMContext context = new WMContext())
                {
                    order = (
                        from o in context.Orders
                        join u in context.Users on o.UserId equals u.Id
                        join p in context.Options on o.PayId equals p.Id
                        join s in context.Options on o.StatusId equals s.Id
                        join pv in context.Regions on o.ProvinceId equals pv.Id
                        join city in context.Regions on o.CityId equals city.Id
                        join area in context.Regions on o.AreaId equals area.Id
                        where o.Id.Equals(orderId)
                        select new WMOrders 
                        { 
                            Id = o.Id,
                            UserId = o.UserId,
                            NickName = u.NickName,
                            ProvinceId = o.ProvinceId,
                            ProvinceName = pv.Name,
                            CityId = o.CityId,
                            CityName = city.Name,
                            AreaId = o.AreaId,
                            AreaName = area.Name,
                            Contact = o.Contact,
                            Address = o.Address,
                            Phone = o.Phone,
                            ZipCode = o.ZipCode,
                            PayId = o.PayId,
                            PayName = p.Name,
                            Paid = o.Paid,
                            Freight = o.Freight,
                            Message = o.Message,
                            StatusId = o.StatusId,
                            StatusName = s.Name,
                            AddDate = o.AddDate
                        }
                    ).FirstOrDefault();
                }
            }

            return order;
        }

        /// <summary>
        /// 将指定用户的全部订单根据状态进行分组，获取处于各个状态的订单数量，并以字典集合的形式获取
        /// </summary>
        /// <param name="userId">要进行分组查询的用户ID</param>
        /// <returns>返回一个字典集合，Key是状态ID(零即是全部)，Value是处于该状态下的订单数量</returns>
        public static Dictionary<int, int> GetStatusCount(string userId)
        {
            Dictionary<int, int> stateList = new Dictionary<int, int>();

            if (!General.IsNullable(userId))
            {
                using (WMContext context = new WMContext())
                {
                    //获取指定用户的订单总数
                    var c = (
                        from o in context.Orders
                        where o.UserId.Equals(userId)
                        select o.Id
                    ).Count();

                    //订单总数作为集合的第一项，Key为零
                    stateList.Add(0, c);

                    //获取各个状态下的订单总数
                    var list = (
                        from o in context.Orders
                        where o.UserId.Equals(userId)
                        group o by o.StatusId into os
                        select new 
                        {
                            Id = os.Key,
                            Count = os.Count()
                        }
                    );

                    if (list != null)
                    {
                        foreach (var item in list)
                            stateList.Add(item.Id, item.Count);
                    }
                }
            }

            return stateList;
        }

        public static List<WMOrders> GetList(out int pageCount, string userId = null, string orderId = null, DateTime? minDate = null, DateTime? maxDate = null, int payId = 0, int paid = -1, int state = 0, int pageIndex = 0, int pageSize = 0)
        {
            List<WMOrders> list = null;
            bool isUser = !General.IsNullable(userId);
            bool isOrder = !General.IsNullable(orderId);
            DateTime min = minDate.HasValue ? minDate.Value : DateTime.MinValue;
            DateTime max = maxDate.HasValue ? maxDate.Value : DateTime.MaxValue;
            pageCount = 0;

            using (WMContext context = new WMContext())
            {
                var query = (
                    from o in context.Orders
                    join u in context.Users on o.UserId equals u.Id
                    join p in context.Options on o.PayId equals p.Id
                    join s in context.Options on o.StatusId equals s.Id
                    join pv in context.Regions on o.ProvinceId equals pv.Id
                    join city in context.Regions on o.CityId equals city.Id
                    join area in context.Regions on o.AreaId equals area.Id
                    where (isUser ? o.UserId.Equals(userId) : true)
                       && (isOrder ? o.Id.Contains(orderId) : true)
                       && o.AddDate >= min
                       && o.AddDate <= max
                       && (payId > 0 ? o.PayId == payId : true)
                       && (state > 0 ? o.StatusId == state : true)
                       && (paid >= 0 ? (o.Paid == (paid == 1)) : true)
                    orderby o.AddDate descending
                    select new WMOrders
                    {
                        Id = o.Id,
                        UserId = o.UserId,
                        NickName = u.NickName,
                        ProvinceId = o.ProvinceId,
                        ProvinceName = pv.Name,
                        CityId = o.CityId,
                        CityName = city.Name,
                        AreaId = o.AreaId,
                        AreaName = area.Name,
                        Contact = o.Contact,
                        Address = o.Address,
                        Phone = o.Phone,
                        ZipCode = o.ZipCode,
                        PayId = o.PayId,
                        PayName = p.Name,
                        Paid = o.Paid,
                        Freight = o.Freight,
                        Message = o.Message,
                        StatusId = o.StatusId,
                        StatusName = s.Name,
                        AddDate = o.AddDate
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
             && !General.IsNullable(this.Address)
             && !General.IsNullable(this.Contact)
             && !General.IsNullable(this.Phone)
             && !General.IsNullable(this.ZipCode)
             && this.ProvinceId > 0
             && this.CityId > 0
             && this.AreaId > 0
             && this.PayId > 0
             && this.StatusId > 0
             && (checkId ? !General.IsNullable(this.Id) : true)
            );
        }
    }
}
