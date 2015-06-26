using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMGoodState
    {
        private static readonly string groupName = "goodState";

        public int StateId { get; set; }
        public string StateName { get; set; }
        

        public static List<WMGoodState> GetList()
        {
            List<WMGoodState> list = null;

            using (WMContext context = new WMContext())
            {
                list = (
                    from r in context.Options
                    where r.Group.Equals(groupName)
                    select new WMGoodState
                    {
                        StateId = r.Id,
                        StateName = r.Name
                    }
                ).ToList();
            }

            return list;
        }
    }
}
