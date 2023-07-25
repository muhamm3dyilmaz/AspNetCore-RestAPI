﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.LinkModels
{
    public class Link
    {
        public string? Href { get; set; }
        public string? Rel { get; set; }
        public string? Method { get; set; }

        //Link ifadeleri serileştirilip Json ifadesine dönüştürürken boş const lazım olacak
        public Link()
        {
            
        }

        //Uygulama içinde kullanuırken full const lazım olacak
        public Link(string? href, string? rel, string? method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}
