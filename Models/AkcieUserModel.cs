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
        public double lin_reg { get; set; }
        public int? pocet { get; set; }
    }
    public class akcie2
    {
        public int akcie_id { get; set; }
        public string nazev { get; set; }
        public string zkratka { get; set; }
        public double cena_nakup { get; set; }
        public double cena_prodej { get; set; }
        public DateTime datum { get; set; }
    }
    public class PortfolioChangeSumModel
    {
        public portfolio pModel { get; set; }
        public int sumToSell { get; set; }
        public int sumToBuy { get; set; }
        public float? wallet { get; set; }
        public string akcieName { get; set; }
    }

    public class PagedList<T>
    {
        public List<T> Content { get; set; }
        public Int32 CurrentPage { get; set; }
        public Int32 PageSize { get; set; }
        public int TotalRecords { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalRecords / PageSize); }
        }
    }
}