﻿using System;
using System.Collections.Generic;
using Com.Ericmas001.Wpf.Entities.Enums;

namespace Com.Ericmas001.Wpf.Entities.Attributes
{
    public class FilterCommandAttribute : Attribute
    {
        public IEnumerable<FilterCommandEnum> Commands{get; private set; }
        public FilterCommandAttribute(params FilterCommandEnum[] commands)
        {
            Commands = commands;
        }

    }
}
