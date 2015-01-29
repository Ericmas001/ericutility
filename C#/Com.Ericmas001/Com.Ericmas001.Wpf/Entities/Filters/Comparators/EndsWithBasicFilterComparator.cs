﻿using System.Collections;
using System.Linq;
using Com.Ericmas001.Wpf.Entities.Filters.Attributes;
using Com.Ericmas001.Wpf.Entities.Filters.Enums;

namespace Com.Ericmas001.Wpf.Entities.Filters.Comparators
{
    [FilterComparator(FilterComparatorEnum.EndsWith)]
    public class EndsWithBasicFilterComparator : BasicFilterComparator
    {
        public override bool IsDataFiltered(object comparatorValue, object value)
        {
            return value.ToString().EndsWith(comparatorValue.ToString());
        }
    }
}
