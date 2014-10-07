﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Ericmas001.Wpf.ViewCommands
{
    /// <summary>
    /// Defines a property to access the ViewCommandManager instance of an object.
    /// </summary>
    public interface IViewCommandSource
    {
        /// <summary>
        /// Gets the ViewCommandManager instance of the object.
        /// </summary>
        ViewCommandManager ViewCommandManager { get; }
    }
}
