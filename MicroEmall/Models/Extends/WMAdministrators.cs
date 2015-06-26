using System;
using System.Collections.Generic;
using System.Linq;
using Jumpcity.Utility.Extend;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMAdministrators : Administartors
    {
        public string RoleName
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
                this.AddDate = DateTime.Now;

                using (WMContext context = new WMContext())
                {
                    Administartors model = new Administartors {
                        RoleId = this.RoleId,
                        UserName = this.UserName,
                        Password = this.Password,
                        StatusId = this.StatusId,
                        AddDate = this.AddDate
                    };

                    context.Administartors.Add(model);
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
                    Administartors model = context.Administartors.Find(this.Id);

                    if (model != null)
                    {
                        model.UserName = this.UserName;
                        model.Password = this.Password;
                        model.RoleId = this.RoleId;
                        model.StatusId = this.StatusId;
                        context.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool UpdatePassword(int adminId, string oldPwd, string newPwd)
        {
            if (adminId > 0 && !General.IsNullable(oldPwd) && !General.IsNullable(newPwd))
            {
                if (oldPwd.Equals(newPwd))
                    return true;

                using (WMContext context = new WMContext())
                {
                    Administartors model = context.Administartors.Find(adminId);

                    if (model != null)
                    {
                        model.Password = newPwd;
                        context.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool Delete(int adminId)
        {
            if (adminId > 0)
            {
                using (WMContext context = new WMContext())
                {
                    Administartors model = context.Administartors.Find(adminId);

                    if (model != null)
                    {
                        model.StatusId = 203;
                        context.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        public static WMAdministrators Login(string userName, string pwd)
        {
            WMAdministrators admin = null;

            if (!General.IsNullable(userName) && !General.IsNullable(pwd))
            {
                int adminId = 0;

                using (WMContext context = new WMContext())
                {
                    adminId = (
                        from ad in context.Administartors
                        where ad.StatusId == 201
                           && ad.Password.Equals(pwd)
                           && ad.UserName.Equals(userName)
                        select ad.Id
                    ).FirstOrDefault();
                }

                if (adminId > 0)
                    admin = Get(adminId);
            }

            return admin;
        }

        public static WMAdministrators Get(int adminId)
        {
            WMAdministrators admin = null;

            if (adminId > 0)
            {
                using (WMContext context = new WMContext())
                {
                    admin = (
                        from ad in context.Administartors
                        join r in context.Options on ad.RoleId equals r.Id
                        join s in context.Options on ad.StatusId equals s.Id
                        where ad.Id == adminId
                        select new WMAdministrators 
                        {
                            Id = ad.Id,
                            RoleId = ad.RoleId,
                            RoleName = r.Name,
                            UserName = ad.UserName,
                            Password = ad.Password,
                            StatusId = ad.StatusId,
                            StatusName = s.Name,
                            AddDate = ad.AddDate
                        }
                    ).FirstOrDefault();
                }
            }

            return admin;
        }

        public static List<WMAdministrators> GetList(out int pageCount, int roleId = 0, int stateId = 0, int pageIndex = 0, int pageSize = 0)
        {
            List<WMAdministrators> list = null;
            pageCount = 0;

            using (WMContext context = new WMContext())
            {
                var query = (
                    from ad in context.Administartors
                    join r in context.Options on ad.RoleId equals r.Id
                    join s in context.Options on ad.StatusId equals s.Id
                    where ad.StatusId != 203
                       && (roleId > 0 ? ad.RoleId == roleId : true)
                       && (stateId > 0 ? ad.StatusId == stateId : true)
                    orderby ad.StatusId ascending, ad.AddDate descending
                    select new WMAdministrators
                    {
                        Id = ad.Id,
                        RoleId = ad.RoleId,
                        RoleName = r.Name,
                        UserName = ad.UserName,
                        Password = ad.Password,
                        StatusId = ad.StatusId,
                        StatusName = s.Name,
                        AddDate = ad.AddDate
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
                this.RoleId > 0
             && !General.IsNullable(this.UserName)
             && !General.IsNullable(this.Password)
             && (checkId ? this.Id > 0 : true)
            );
        }
    }
}
