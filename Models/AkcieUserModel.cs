using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bcpp.Models
{
    public class AkcieUserModel
    {
        public int akcie_id {get; set;}
        public string nazev { get; set; }
        public string zkratka {get; set;}
        public double cena_nakup {get; set;}
        public double cena_prodej {get; set;}
        public DateTime datum { get; set; }
        public int? pocet { get; set; }
    }
}