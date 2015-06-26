using System;
using System.Collections.Generic;
using System.Linq;
using Jumpcity.Utility.Extend;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMAccountState
    {
        private static readonly string groupName = "userState";

        public int StateId { get; set; }
        public string StateName { get; set; }

        public static List<WMAccountState> GetList()
        {
            List<WMAccountState> list = null;

            using (WMContext context = new WMContext())
            {
                list = (
                    from r in context.Options
                    where r.Group.Equals(groupName)
                       && r.Value.Equals("1")
                    select new WMAccountState
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
