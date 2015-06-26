using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroEmall.Models
{
    [Serializable]
    public class WMOrderState
    {
        private static readonly string groupName = "orderState";

        public int StateId { get; set; }
        public string StateName { get; set; }
        public int StateCount { get; set; }

        public static WMOrderState Get(int stateId)
        {
            WMOrderState state = null;

            using (WMContext context = new WMContext())
            {
                state = (
                    from r in context.Options
                    where r.Group.Equals(groupName)
                       && r.Id == stateId
                    select new WMOrderState
                    {
                        StateId = r.Id,
                        StateName = r.Name,
                        StateCount = context.Orders.Count(o => o.StatusId == r.Id)
                    }
                ).FirstOrDefault();
            }

            return state;
        }

        public static List<WMOrderState> GetList()
        {
            List<WMOrderState> list = null;

            using (WMContext context = new WMContext())
            {
                list = (
                    from r in context.Options
                    where r.Group.Equals(groupName)
                    select new WMOrderState
                    {
                        StateId = r.Id,
                        StateName = r.Name,
                        StateCount = context.Orders.Count(o => o.StatusId == r.Id)
                    }
                ).ToList();
            }

            return list;
        }
    }
}
