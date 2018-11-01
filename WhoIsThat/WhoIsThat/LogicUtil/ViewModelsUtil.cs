using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhoIsThat.Models;

namespace WhoIsThat.LogicUtil
{
    public static class ViewModelsUtil
    {
        public static List<ImageObject> SortList(List<ImageObject> list)
        {
            return list.OrderByDescending(c => c.Score).ToList();
        }
    }
}
