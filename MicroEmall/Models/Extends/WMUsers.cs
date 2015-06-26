using System;
using System.Collections.Generic;
using System.Linq;
using Jumpcity.Utility.Extend;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMUsers : Users
    {
        /// <summary>
        /// 获取当前用户推广信息总的被点击量
        /// </summary>
        public int ClickCount
        {
            get
            {
                if (this.RoleId == 102)
                    return WMUserClicks.GetClickCount(this.Id);
                return 0;
            }
        }

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
                this.Id = General.UniqueString(this.Id);
                this.AddDate = DateTime.Now;

                using (WMContext context = new WMContext())
                {
                    Users model = new Users {
                        Id = this.Id,
                        RoleId = this.RoleId,
                        OpenId = this.OpenId,
                        UserName = this.UserName,
                        Password = this.Password,
                        NickName = this.NickName,
                        Image = this.Image,
                        Mobile = this.Mobile,
                        BankCard = this.BankCard,
                        StatusId = this.StatusId,
                        AddDate = this.AddDate
                    };

                    context.Users.Add(model);
                    context.SaveChanges();
                }

                return true;
            }

            return false;
        }

        public bool Update(bool updatePwd = false)
        {
            if (this.Valid(true))
            {
                using (WMContext context = new WMContext())
                {
                    Users model = context.Users.Find(this.Id);

                    if (model != null)
                    {
                        model.UserName = this.UserName;
                        model.RoleId = this.RoleId;
                        model.NickName = this.NickName;
                        model.Integral = this.Integral;
                        model.Image = this.Image;
                        model.Mobile = this.Mobile;
                        model.BankCard = this.BankCard;
                        model.StatusId = this.StatusId;
                        if (updatePwd)
                            model.Password = this.Password;

                        context.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool UpdatePassword(string userId, string oldPwd, string newPwd)
        {
            if (!General.IsNullable(userId) && !General.IsNullable(oldPwd) && !General.IsNullable(newPwd))
            {
                if (oldPwd.Equals(newPwd))
                    return true;

                using (WMContext context = new WMContext())
                {
                    Users model = context.Users.Find(userId);

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

        public static bool UpdateIntegral(string userId, int addInte)
        {
            if (!General.IsNullable(userId))
            {
                using (WMContext context = new WMContext())
                {
                    Users model = context.Users.Find(userId);

                    if (model != null)
                    {
                        model.Integral += addInte;
                        context.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool Delete(string userId)
        {
            if (!General.IsNullable(userId))
            {
                using (WMContext context = new WMContext())
                {
                    Users model = context.Users.Find(userId);

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

        public static bool IsExitis(string userId, string userName)
        {
            int count = 0;

            if (!General.IsNullable(userName))
            {
                using (WMContext context = new WMContext())
                {
                    count = (
                        from u in context.Users
                        where u.UserName.Equals(userName)
                           && u.Id != userId
                           && u.StatusId != 203
                        select u.Id
                    ).Count();
                }
            }

            return count > 0;
        }

        public static WMUsers Get(string userId)
        {
            WMUsers user = null;

            if (!General.IsNullable(userId))
            {
                using (WMContext context = new WMContext())
                {
                    user = (
                        from u in context.Users
                        join r in context.Options on u.RoleId equals r.Id
                        join s in context.Options on u.StatusId equals s.Id
                        where u.Id.Equals(userId)
                        select new WMUsers 
                        {
                            Id = u.Id,
                            RoleId = u.RoleId,
                            RoleName = r.Name,
                            OpenId = u.OpenId,
                            UserName = u.UserName,
                            Password = u.Password,
                            NickName = u.NickName,
                            Integral = u.Integral,
                            Image = u.Image,
                            Mobile = u.Mobile,
                            BankCard = u.BankCard,
                            StatusId = u.StatusId,
                            StatusName = s.Name,
                            AddDate = u.AddDate
                        }
                    ).FirstOrDefault();
                }
            }

            return user;
        }

        public static WMUsers Login(string userKey, string pwd)
        {
            WMUsers user = null;

            if (!General.IsNullable(userKey) && !General.IsNullable(pwd))
            {
                string userId = null;

                using (WMContext context = new WMContext())
                {
                    userId = (
                        from u in context.Users
                        where u.StatusId == 201
                           && u.Password.Equals(pwd)
                           && (u.UserName.Equals(userKey) || u.Mobile == userKey)
                        select u.Id
                    ).FirstOrDefault();
                }

                if (userId != null)
                    user = Get(userId);
            }

            return user;
        }
        public static WMUsers Login(string openId)
        {
            WMUsers user = null;

            if (!General.IsNullable(openId))
            {
                string userId = null;

                using (WMContext context = new WMContext())
                {
                    userId = (
                        from u in context.Users
                        where u.StatusId == 201
                           && u.OpenId == openId
                        select u.Id
                    ).FirstOrDefault();
                }

                if (userId != null)
                    user = Get(userId);
            }

            return user;
        }

        public static List<WMUsers> GetList(out int pageCount, int roleId = 0, int statusId = 0, string userKey = null, int pageIndex = 0, int pageSize = 0)
        {
            bool flag = !General.IsNullable(userKey);
            List<WMUsers> list = new List<WMUsers>();
            pageCount = 0;

            using (WMContext context = new WMContext())
            {
                var query = (
                    from u in context.Users
                    join r in context.Options on u.RoleId equals r.Id
                    join s in context.Options on u.StatusId equals s.Id
                    where (roleId > 0 ? u.RoleId == roleId : true)
                       && (statusId > 0 ? u.StatusId == statusId : true)
                       && (flag ? (u.UserName.Contains(userKey) || u.NickName.Contains(userKey) || u.Mobile.Contains(userKey)) : true)
                       && u.StatusId != 203
                    orderby u.StatusId ascending, u.AddDate descending
                    select new WMUsers
                    {
                        Id = u.Id,
                        RoleId = u.RoleId,
                        RoleName = r.Name,
                        OpenId = u.OpenId,
                        UserName = u.UserName,
                        Password = u.Password,
                        NickName = u.NickName,
                        Integral = u.Integral,
                        Image = u.Image,
                        Mobile = u.Mobile,
                        BankCard = u.BankCard,
                        StatusId = u.StatusId,
                        StatusName = s.Name,
                        AddDate = u.AddDate
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
                !General.IsNullable(this.UserName)
             && !General.IsNullable(this.Password)
             && !General.IsNullable(this.NickName)
             && !IsExitis(this.Id, this.UserName)
             && this.RoleId > 0
             && (checkId ? !General.IsNullable(this.Id) : true)
            );
        }
    }
}
