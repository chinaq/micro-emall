using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMOrderPay
    {
        private static readonly string groupName = "pay";

        public int PayId { get; set; }
        public string PayName { get; set; }

        public static List<WMOrderPay> GetList()
        {
            List<WMOrderPay> list = null;

            using (WMContext context = new WMContext())
            {
                list = (
                    from r in context.Options
                    where r.Group.Equals(groupName)
                    select new WMOrderPay
                    {
                        PayId = r.Id,
                        PayName = r.Name
                    }
                ).ToList();
            }

            return list;
        }
    }
}
