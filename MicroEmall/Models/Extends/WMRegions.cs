using System;
using System.Collections.Generic;
using System.Linq;
using Jumpcity.Utility.Extend;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMRegions : Regions
    {
        public static List<WMRegions> GetList(int parentId = 0)
        {
            List<WMRegions> list = null;

            if (parentId >= 0)
            {
                using (WMContext context = new WMContext())
                {
                    list = (
                        from r in context.Regions
                        where r.ParentId == parentId
                        orderby r.Id ascending
                        select new WMRegions 
                        {
                            Id = r.Id,
                            ParentId = r.ParentId,
                            Name = r.Name,
                            Level = r.Level,
                            AreaCode = r.AreaCode
                        }
                    ).ToList();
                }
            }

            return list;
        }

        public static List<WMRegions> GetProvinces()
        {
            return GetList();
        }

        public static List<WMRegions> GetCities(int provinceId)
        {
            return GetList(provinceId);
        }

        public static List<WMRegions> GetAreas(int cityId)
        {
            return GetList(cityId);
        }
    }
}
