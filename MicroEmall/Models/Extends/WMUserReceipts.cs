using System;
using System.Collections.Generic;
using System.Linq;
using Jumpcity.Utility.Extend;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMUserReceipts : UserReceipts
    {
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

        public bool Add()
        {
            if (this.Valid())
            {
                this.Id = General.UniqueString(this.Id);
                this.AddDate = DateTime.Now;

                using (WMContext context = new WMContext())
                {
                    UserReceipts model = new UserReceipts { 
                        Id = this.Id,
                        UserId = this.UserId,
                        ProvinceId = this.ProvinceId,
                        CityId = this.CityId,
                        AreaId = this.AreaId,
                        Address = this.Address,
                        Contact = this.Contact,
                        Phone = this.Phone,
                        ZipCode = this.ZipCode,
                        AddDate = this.AddDate
                    };

                    context.UserReceipts.Add(model);
                    context.SaveChanges();
                }

                return true;
            }

            return false;
        }

        public bool Update()
        {
            if (this.Valid(true))
            {
                using (WMContext context = new WMContext())
                {
                    UserReceipts model = context.UserReceipts.Find(this.Id);

                    if (model != null)
                    {
                        model.ProvinceId = this.ProvinceId;
                        model.CityId = this.CityId;
                        model.AreaId = this.AreaId;
                        model.Address = this.Address;
                        model.Contact = this.Contact;
                        model.Phone = this.Phone;
                        model.ZipCode = this.ZipCode;

                        context.SaveChanges();
                        return true;
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
                    UserReceipts model = context.UserReceipts.Find(id);

                    if (model != null)
                    {
                        context.UserReceipts.Remove(model);
                        context.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        public static WMUserReceipts Get(string id)
        {
            WMUserReceipts receipt = null;

            if (!General.IsNullable(id))
            {
                using (WMContext context = new WMContext())
                {
                    receipt = (
                        from ur in context.UserReceipts
                        join pv in context.Regions on ur.ProvinceId equals pv.Id
                        join city in context.Regions on ur.CityId equals city.Id
                        join area in context.Regions on ur.AreaId equals area.Id
                        where ur.Id.Equals(id)
                        select new WMUserReceipts 
                        {
                            Id = ur.Id,
                            UserId = ur.UserId,
                            ProvinceId = ur.ProvinceId,
                            ProvinceName = pv.Name,
                            CityId = ur.CityId,
                            CityName = city.Name,
                            AreaId = ur.AreaId,
                            AreaName = area.Name,
                            Address = ur.Address,
                            Contact = ur.Contact,
                            Phone = ur.Phone,
                            ZipCode = ur.ZipCode,
                            AddDate = ur.AddDate
                        }
                    ).FirstOrDefault();
                }
            }

            return receipt;
        }

        public static List<WMUserReceipts> GetList(out int pageCount, string userId = null, int pageIndex = 0, int pageSize = 0)
        {
            List<WMUserReceipts> list = null;
            bool flag = !General.IsNullable(userId);
            pageCount = 0;

            using (WMContext context = new WMContext())
            {
                var query = (
                    from ur in context.UserReceipts
                    join pv in context.Regions on ur.ProvinceId equals pv.Id
                    join city in context.Regions on ur.CityId equals city.Id
                    join area in context.Regions on ur.AreaId equals area.Id
                    where (flag ? ur.UserId.Equals(userId) : true)
                    orderby ur.AddDate descending
                    select new WMUserReceipts
                    {
                        Id = ur.Id,
                        UserId = ur.UserId,
                        ProvinceId = ur.ProvinceId,
                        ProvinceName = pv.Name,
                        CityId = ur.CityId,
                        CityName = city.Name,
                        AreaId = ur.AreaId,
                        AreaName = area.Name,
                        Address = ur.Address,
                        Contact = ur.Contact,
                        Phone = ur.Phone,
                        ZipCode = ur.ZipCode,
                        AddDate = ur.AddDate
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
        public static List<WMUserReceipts> GetList(string userId = null, int pageIndex = 0, int pageSize = 0)
        {
            int pageCount = 0;
            return GetList(out pageCount, userId, pageIndex, pageSize);
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
             && (checkId ? !General.IsNullable(this.Id) : true)
            );
        }
    }
}
