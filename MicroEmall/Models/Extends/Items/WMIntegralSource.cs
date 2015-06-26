using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMIntegralSource
    {
        private static readonly string groupName = "inteSource";

        public int SourceId { get; set; }
        public string SourceName { get; set; }

        public static List<WMIntegralSource> GetList()
        {
            List<WMIntegralSource> list = null;

            using (WMContext context = new WMContext())
            {
                list = (
                    from r in context.Options
                    where r.Group.Equals(groupName)
                    select new WMIntegralSource
                    {
                        SourceId = r.Id,
                        SourceName = r.Name
                    }
                ).ToList();
            }

            return list;
        }
    }
}
