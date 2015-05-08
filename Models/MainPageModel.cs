using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bcpp.Models
{
    public class MainPageModel
    {
        public IEnumerable<akcie> AModel { get; set; }
        public IEnumerable<index_PX> IModel { get; set; }
    }
}