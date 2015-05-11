using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bcpp.Models
{
    public class AkcieDetail
    {
        public akcie Akcie { get; set; }
        public IEnumerable<historie_akcie> Historie { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public double Lin_reg { get; set; }
    }

    public class myAxes
    {
        public double x { get; set; }
        public float y { get; set; }
    }
}
