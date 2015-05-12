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

    public class SledovaneMy
    {
        public IEnumerable<MyModelSledovane> sled { get; set; }
        public List<int> akcieIds { get; set; }
        public List<string> akcieNames { get; set; }
    }

    public class MyModelSledovane
    {
        public int idSledovane { get; set; }
        public string nazevSledovane { get; set; }
        public string nazevAkcie { get; set; }
        public string nazevFirmy { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public float cenaProdej { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public float cenaNakup { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime datum { get; set; }
    }
}
