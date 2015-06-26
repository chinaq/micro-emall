using System;
using System.Collections.Generic;
using System.Linq;
using Jumpcity.Utility.Extend;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMGoods : Goods
    {
        private double _clickBonuses = 0;
        private List<WMGoodImages> _images = null;
        private List<WMGoodCategories> _categories = null;

        public List<WMGoodImages> Images
        {
            get 
            {
                if (General.IsNullable(_images))
                    _images = WMGoodImages.GetList(this.Id);
                return _images;
            }
        }

        public List<WMGoodCategories> Categories
        {
            get
            {
                if (General.IsNullable(_categories))
                    _categories = WMGoodCategories.GetTree(this.CategoryId);
                return _categories;
            }
        }

        /// <summary>
        /// 获取当前商品分类的类似面包屑导航的字符串格式(上级分类 > 下级分类)
        /// </summary>
        public string CategoriesMap
        {
            get 
            {
                if (!General.IsNullable(this.Categories))
                    return string.Join(" > ", this.Categories.Select(g => g.Name).ToArray());
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取该商品的奖金池总额
        /// </summary>
        public double ClickBonuses
        {
            get 
            {
                if (_clickBonuses == 0)
                    _clickBonuses = (((this.PresentPrice.HasValue ? this.PresentPrice.Value : this.OriginalPrice) * this.Bonus) / 100.0) * WMOrders.GetPayCount(this.Id);
                return _clickBonuses;
            }
        }

        public string FirstImage
        {
            get
            {
                if (!General.IsNullable(this.Images))
                    return this.Images[0].URL;
                return string.Empty;
            }
        }

        public string BrandName
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

                if (!this.PresentPrice.HasValue)
                    this.PresentPrice = this.OriginalPrice;

                using (WMContext context = new WMContext())
                {
                    Goods model = new Goods { 
                        Id = this.Id,
                        CategoryId = this.CategoryId,
                        BrandId = this.BrandId,
                        Name = this.Name,
                        OriginalPrice = this.OriginalPrice,
                        PresentPrice = this.PresentPrice,
                        Unit = this.Unit,
                        Desc = this.Desc,
                        Spec = this.Spec,
                        Service = this.Service,
                        Integral = this.Integral,
                        Clicks = this.Clicks,
                        Saves = this.Saves,
                        Bonus = this.Bonus,
                        GoldPool = this.GoldPool,
                        StatusId = this.StatusId,
                        AddDate = this.AddDate
                    };

                    context.Goods.Add(model);
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
                    Goods model = context.Goods.Find(this.Id);

                    if (model != null)
                    {
                        model.CategoryId = this.CategoryId;
                        model.BrandId = this.BrandId;
                        model.Name = this.Name;
                        model.OriginalPrice = this.OriginalPrice;
                        model.PresentPrice = this.PresentPrice;
                        model.Unit = this.Unit;
                        model.Desc = this.Desc;
                        model.Spec = this.Spec;
                        model.Service = this.Service;
                        model.Integral = this.Integral;
                        model.Clicks = this.Clicks;
                        model.Saves = this.Saves;
                        model.Bonus = this.Bonus;
                        model.GoldPool = this.GoldPool;
                        model.StatusId = this.StatusId;

                        context.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool Delete(string id)
        {
            return UpdateStatus(id, 502);
        }

        public static bool HaveMark(string id)
        {
            return UpdateStatus(id, 501);
        }

        public static bool Restore(string id)
        {
            return UpdateStatus(id, 503);
        }

        public static bool UpdateStatus(string id, int stateId)
        {
            if (!General.IsNullable(id) && stateId > 0)
            {
                using (WMContext context = new WMContext())
                {
                    Goods model = context.Goods.Find(id);

                    if (model != null)
                    {
                        model.StatusId = stateId;

                        //恢复上架的商品，上架时间从恢复之时算起
                        if (stateId == 503)
                            model.AddDate = DateTime.Now;
                        
                        context.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        public static WMGoods Get(string id)
        {
            WMGoods good = null;

            if (!General.IsNullable(id))
            {
                using (WMContext context = new WMContext())
                {
                    good = (
                        from g in context.Goods
                        join s in context.Options on g.StatusId equals s.Id
                        join ba in context.GoodBrands on g.BrandId equals ba.Id into brand
                        from b in brand.DefaultIfEmpty()
                        where g.Id.Equals(id)
                        select new WMGoods
                        {
                            Id = g.Id,
                            CategoryId = g.CategoryId,
                            BrandId = g.BrandId,
                            BrandName = (b != null ? b.Name : ""),
                            Name = g.Name,
                            OriginalPrice = g.OriginalPrice,
                            PresentPrice = g.PresentPrice,
                            Unit = g.Unit,
                            Desc = g.Desc,
                            Spec = g.Spec,
                            Service = g.Service,
                            Integral = g.Integral,
                            Clicks = g.Clicks,
                            Saves = g.Saves,
                            Bonus = g.Bonus,
                            GoldPool = g.GoldPool,
                            StatusId = g.StatusId,
                            StatusName = s.Name,
                            AddDate = g.AddDate
                        }
                    ).FirstOrDefault();
                }
            }

            return good;
        }

        public static List<WMGoods> GetList(out int pageCount, string categoryId = null, string brandId = null, string name = null, int stateId = 0, string orderBy = "date desc", int pageIndex = 0, int pageSize = 0)
        {
            List<WMGoods> list = null;
            bool isCategory = !General.IsNullable(categoryId);
            bool isBrand = !General.IsNullable(brandId);
            bool isName = !General.IsNullable(name);
            pageCount = 0;

            using (WMContext context = new WMContext())
            {
                //用传入的分类ID尝试获取其子分类ID
                //如果能获取到，则证明传入的是一个大分类ID
                //这样的话其所有的子分类ID都应作为查询条件
                //所以在查询商品时，关于分类的条件使用的是IN关键字
                string[] cidList = null;

                //注意：如果传入root将会导致无法查询到商品信息
                if (categoryId != "root")
                    cidList = context.GoodCategories.Where(gc => gc.ParentId.Equals(categoryId)).Select(gc => gc.Id).ToArray();
                
                //如果获取失败，则证明当前传入的就是子分类ID
                //这样的话，要满足的分类条件就只有传入的这一个了
                if (General.IsNullable(cidList))
                    cidList = new string[] { categoryId };

                var query = (
                    from g in context.Goods
                    join s in context.Options on g.StatusId equals s.Id
                    join ba in context.GoodBrands on g.BrandId equals ba.Id into brand
                    from b in brand.DefaultIfEmpty()
                    where (stateId > 0 ? g.StatusId == stateId : true)
                       && (isCategory ? cidList.AsQueryable().Contains(g.CategoryId) : true)
                       && (isBrand ? g.BrandId == brandId : true)
                       && (isName ? g.Name.Contains(name) : true)
                    select new WMGoods
                    {
                        Id = g.Id,
                        CategoryId = g.CategoryId,
                        BrandId = g.BrandId,
                        BrandName = (b != null ? b.Name : ""),
                        Name = g.Name,
                        OriginalPrice = g.OriginalPrice,
                        PresentPrice = g.PresentPrice,
                        Unit = g.Unit,
                        Desc = g.Desc,
                        Spec = g.Spec,
                        Service = g.Service,
                        Integral = g.Integral,
                        Clicks = g.Clicks,
                        Saves = g.Saves,
                        Bonus = g.Bonus,
                        GoldPool = g.GoldPool,
                        StatusId = g.StatusId,
                        StatusName = s.Name,
                        AddDate = g.AddDate
                    }
                );

                if (query != null)
                {
                    pageCount = query.Count();

                    switch (orderBy)
                    {
                        case "date asc":
                            query = query.OrderBy(g => g.AddDate);
                            break;
                        case "date desc":
                            query = query.OrderByDescending(g => g.AddDate);
                            break;
                        case "price asc":
                            query = query.OrderBy(g => g.PresentPrice);
                            break;
                        case "price desc":
                            query = query.OrderByDescending(g => g.PresentPrice);
                            break;
                        case "click asc":
                            query = query.OrderBy(g => g.Clicks);
                            break;
                        case "click desc":
                            query = query.OrderByDescending(g => g.Clicks);
                            break;
                        default:
                            query = query.OrderByDescending(g => g.AddDate);
                            break;
                    }

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
                !General.IsNullable(this.CategoryId)
             && !General.IsNullable(this.Name)
             && !General.IsNullable(this.Unit)
             && this.OriginalPrice > 0
             && this.StatusId > 0
             && (checkId ? !General.IsNullable(this.Id) : true)
            );
        }

    }
}
