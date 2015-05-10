using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace bcpp.Models
{
    public class MainPageModel
    {
        public IEnumerable<akcie2> AModel { get; set; }
        public PagedList.IPagedList<index_PX> IModel { get; set; }
    }
}