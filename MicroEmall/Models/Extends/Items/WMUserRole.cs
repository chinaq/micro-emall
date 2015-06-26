using System;
using System.Collections.Generic;
using System.Linq;
using Jumpcity.Utility.Extend;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMUserRole
    {
        private static readonly string groupName = "userRole";

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public static List<WMUserRole> GetList()
        {
            List<WMUserRole> list = null;

            using (WMContext context = new WMContext())
            {
                list = (
                    from r in context.Options
                    where r.Group.Equals(groupName)
                    select new WMUserRole 
                    {
                        RoleId = r.Id,
                        RoleName = r.Name
                    }
                ).ToList();
            }

            return list;
        }
    }
}
