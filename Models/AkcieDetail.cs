using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bcpp.Models
{
    public class AkcieDetail
    {
        public akcie Akcie { get; set; }
        public IEnumerable<historie_akcie> Historie { get; set; }
    }

    public class MainItem
    {
        public Item Prodej { get; set; }
        public Item Nakup { get; set; }
    }

    public class Item
    {
        public string datum { get; set; }
        public float hodnota { get; set; }
    }
}
