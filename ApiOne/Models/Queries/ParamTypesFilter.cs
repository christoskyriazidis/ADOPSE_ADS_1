﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Queries
{
    public class ParamTypesFilter
    {
        //private string _type="type=";
        public string Type { get; set; }
        public string Category { get; set; }
        public string Manufacturer { get; set; }
        public string State { get; set; }

    }
}
