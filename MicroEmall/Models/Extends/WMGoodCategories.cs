using System;
using System.Collections.Generic;
using System.Linq;
using Jumpcity.Utility.Extend;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMGoodCategories : GoodCategories
    {
        public string ParentName
        {
            get;
            protected set;
        }

        public bool Add()
        {
            if (this.Valid())
            {
                this.Id = General.UniqueString(this.Id);

                using (WMContext context = new WMContext())
                {
                    GoodCategories model = new GoodCategories
                    {
                        Id = this.Id,
                        ParentId = this.ParentId,
                        Name = this.Name,
                        Level = 1,
                        Sort = 0
                    };

                    GoodCategories level = context.GoodCategories.Find(this.ParentId);
                    if (level == null)
                        model.Level = 1;
                    else
                        model.Level = level.Level + 1;

                    var sort = context.GoodCategories.Where(gc => gc.ParentId.Equals(this.ParentId)).ToList();
                    if (General.IsNullable(sort))
                        model.Sort = 1;
                    else
                        model.Sort = sort.Max(gc => gc.Sort) + 1;

                    context.GoodCategories.Add(model);
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
                    GoodCategories model = context.GoodCategories.Find(this.Id);

                    if (model != null)
                    {
                        model.ParentId = this.ParentId;
                        model.Name = this.Name;
                        model.Level = this.Level;
                        model.Sort = this.Sort;

                        context.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Delete(string id)
        {
            if (!General.IsNullable(id))
            {
                using (WMContext context = new WMContext())
                {
                    var list = (
                        from gc in context.GoodCategories
                        where gc.Id.Equals(id)
                           || gc.ParentId.Equals(id)
                        select gc
                    );

                    if (list != null)
                    {
                        foreach (var model in list)
                            context.GoodCategories.Remove(model);

                        context.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        public static WMGoodCategories Get(string id)
        {
            WMGoodCategories category = null;

            if (!General.IsNullable(id))
            {
                using(WMContext context = new WMContext())
	            {
                    category = (
                        from gc in context.GoodCategories
                        where gc.Id.Equals(id)
                        select new WMGoodCategories
                        {
                            Id = gc.Id,
                            ParentId = gc.ParentId,
                            ParentName = (!gc.ParentId.Equals("root") ? context.GoodCategories.Where(g => g.Id.Equals(gc.ParentId)).Select(g => g.Name).FirstOrDefault() : "root"),
                            Name = gc.Name,
                            Level = gc.Level,
                            Sort = gc.Sort
                        }
                    ).FirstOrDefault();
	            }    
            }

            return category;
        }

        public static List<WMGoodCategories> GetList(string parentId = "root")
        {
            List<WMGoodCategories> list = null;

            using (WMContext context = new WMContext())
            {
                list = (
                    from gc in context.GoodCategories
                    where gc.ParentId.Equals(parentId)
                    orderby gc.Sort ascending
                    select new WMGoodCategories
                    {
                        Id = gc.Id,
                        ParentId = gc.ParentId,
                        ParentName = (!gc.ParentId.Equals("root") ? context.GoodCategories.Where(g => g.Id.Equals(gc.ParentId)).Select(g => g.Name).FirstOrDefault() : "root"),
                        Name = gc.Name,
                        Level = gc.Level,
                        Sort = gc.Sort
                    }        
                ).ToList();
            }

            return list;
        }

        public static List<WMGoodCategories> GetTree(string id)
        {
            List<WMGoodCategories> list = new List<WMGoodCategories>();

            if (!General.IsNullable(id))
            {
                WMGoodCategories model = Get(id);

                if (model != null)
                {
                    list.Add(model);

                    while (!model.ParentId.Equals("root"))
                    {
                        model = Get(model.ParentId);
                        list.Insert(0, model);
                    }                   
                }
            }

            return list;
        }

        private bool Valid(bool checkId = false)
        {
            return (
                !General.IsNullable(this.ParentId)
             && !General.IsNullable(this.Name)
             && (checkId ? !General.IsNullable(this.Id) : true)
            );
        }
    }
}
