using System;
using System.Collections.Generic;
using System.Linq;
using Jumpcity.IO;
using Jumpcity.Utility.Extend;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMGoodImages : GoodImages
    {
        public bool Add()
        {
            if (this.Valid())
            {
                this.Id = General.UniqueString(this.Id);
                this.AddDate = DateTime.Now;

                using (WMContext context = new WMContext())
                {
                    this.IsCover = (context.GoodImages.Where(gi => gi.GoodId.Equals(this.GoodId)).Count() == 0);

                    GoodImages model = new GoodImages { 
                        Id = this.Id,
                        GoodId = this.GoodId,
                        URL = this.URL,
                        IsCover = this.IsCover,
                        AddDate = this.AddDate
                    };

                    context.GoodImages.Add(model);
                    context.SaveChanges();
                }

                return true;
            }

            return false;
        }

        public static bool UpdateCover(string id)
        {
            if (!General.IsNullable(id))
            {
                using (WMContext context = new WMContext())
                {
                    GoodImages model = context.GoodImages.Find(id);
                    
                    if (model != null)
                    {
                        GoodImages old = context.GoodImages.Where(gi => gi.GoodId.Equals(model.GoodId) && gi.IsCover).FirstOrDefault();
                        if (old != null)
                            old.IsCover = false;
                        model.IsCover = true;

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

                using (WMContext context =new WMContext())
                {
                    GoodImages model = context.GoodImages.Find(id);

                    if (model != null)
                    {
                        fileName = "~" + model.URL;
                        GoodImages newCover = null;

                        if (model.IsCover)
                        {
                            newCover = (
                                from gi in context.GoodImages
                                where gi.GoodId.Equals(model.GoodId)
                                   && gi.Id != model.Id
                                orderby gi.AddDate descending
                                select gi
                            ).FirstOrDefault();

                            if (newCover != null)
                                newCover.IsCover = true;
                        }

                        context.GoodImages.Remove(model);
                        context.SaveChanges();  
                    }
                }

                return FileHelper.DeleteFile(fileName); 
            }

            return false;
        }

        public static WMGoodImages Get(string id)
        {
            WMGoodImages image = null;

            if (!General.IsNullable(id))
            {
                using (WMContext context = new WMContext())
                {
                    image = (
                        from gi in context.GoodImages
                        where gi.Id.Equals(id)
                        select new WMGoodImages 
                        {
                            Id = gi.Id,
                            GoodId = gi.GoodId,
                            URL = gi.URL,
                            IsCover = gi.IsCover,
                            AddDate = gi.AddDate
                        }
                    ).FirstOrDefault();
                }
            }

            return image;
        }

        public static List<WMGoodImages> GetList(string goodId)
        {
            List<WMGoodImages> list = null;

            if (!General.IsNullable(goodId))
            {
                using (WMContext context = new WMContext())
                {
                    list = (
                        from gi in context.GoodImages
                        where gi.GoodId.Equals(goodId)
                        orderby gi.IsCover descending, gi.AddDate descending
                        select new WMGoodImages
                        {
                            Id = gi.Id,
                            GoodId = gi.GoodId,
                            URL = gi.URL,
                            IsCover = gi.IsCover,
                            AddDate = gi.AddDate
                        }
                    ).ToList();
                }
            }

            return list;
        }

        private bool Valid(bool checkId = false)
        {
            return (
                !General.IsNullable(this.GoodId)
             && !General.IsNullable(this.URL)
             && (checkId ? !General.IsNullable(this.Id) : true)
            );
        }
    }
}
