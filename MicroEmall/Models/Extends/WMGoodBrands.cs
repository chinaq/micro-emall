using System;
using System.Collections.Generic;
using System.Linq;
using Jumpcity.Utility.Extend;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMGoodBrands : GoodBrands
    {
        public bool Add()
        {
            if (this.Valid())
            {
                this.Id = General.UniqueString(this.Id);

                using (WMContext context = new WMContext())
                {
                    GoodBrands model = new GoodBrands { 
                        Id = this.Id,
                        Name = this.Name,
                        Logo = this.Logo,
                        URL = this.URL
                    };

                    context.GoodBrands.Add(model);
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
                    GoodBrands model = context.GoodBrands.Find(this.Id);

                    if (model != null)
                    {
                        model.Name = this.Name;
                        model.Logo = this.Logo;
                        model.URL = this.URL;

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
                string fileName = null;

                using (WMContext context = new WMContext())
                {
                    GoodBrands model = context.GoodBrands.Find(id);

                    if (model != null)
                    {
                        if (!General.IsNullable(model.Logo))
                            fileName = "~" + model.Logo;

                        context.GoodBrands.Remove(model);
                        context.SaveChanges();
                    }
                }

                if (!General.IsNullable(fileName))
                    return Jumpcity.IO.FileHelper.DeleteFile(fileName);

                return true;
            }

            return false;
        }

        public static WMGoodBrands Get(string id)
        {
            WMGoodBrands brand = null;

            if (!General.IsNullable(id))
            {
                using (WMContext context = new WMContext())
                {
                    brand = (
                        from gb in context.GoodBrands
                        where gb.Id.Equals(id)
                        select new WMGoodBrands 
                        {
                            Id = gb.Id,
                            Name = gb.Name,
                            Logo = gb.Logo,
                            URL = gb.URL
                        }
                    ).FirstOrDefault();
                }
            }

            return brand;
        }

        public static List<WMGoodBrands> GetList(out int pageCount, string name = null, int pageIndex = 0, int pageSize = 0)
        {
            List<WMGoodBrands> list = null;
            bool isName = !General.IsNullable(name);
            pageCount = 0;

            using (WMContext context = new WMContext())
            {
                var query = (
                    from gb in context.GoodBrands
                    where (isName ? gb.Name.Contains(name) : true)
                    orderby gb.Logo descending
                    select new WMGoodBrands
                    {
                        Id = gb.Id,
                        Name = gb.Name,
                        Logo = gb.Logo,
                        URL = gb.URL
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

        public static List<WMGoodBrands> GetList(string name = null, int pageIndex = 0, int pageSize = 0)
        {
            int count = 0;
            return GetList(out count, name, pageIndex, pageSize);
        }

        private bool Valid(bool checkId = false)
        {
            return (
                !General.IsNullable(this.Name)
             && (checkId ? !General.IsNullable(this.Id) : true)
            );
        }
    }
}
