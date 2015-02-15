﻿using Com.Ericmas001.Util.Entities;
using Com.Ericmas001.Wpf.Entities.Filters.Attributes;
using Com.Ericmas001.Wpf.Entities.Filters.Comparators;
using Com.Ericmas001.Wpf.Entities.Filters.Enums;

namespace Com.Ericmas001.Wpf.Entities.Filters.Commands
{
    [FilterCommand(FilterCommandEnum.MustNot)]
    public class MustNotSimpleFilterCommand : SimpleFilterCommand
    {
        public override bool IsDataFiltered(IFilterComparator comparator, object comparatorValue, object value, IDataItem item)
        {
            return !comparator.IsDataFiltered(comparatorValue,value,item);
        }
    }
}
