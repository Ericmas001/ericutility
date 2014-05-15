﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Ericmas001.Util;
using Com.Ericmas001.Wpf.ViewModels;
using Com.Ericmas001.Wpf.XAML;

namespace Com.Ericmas001.Wpf
{
    public static class BgTaskManager
    {
        public static T RunTask<T>(T task) where T : IWorkInBackground
        {
            var window = new SingleBgTaskWindow();
            var vm = window.DataContext as SingleBgTaskViewModel;
            vm.OnRequestClose += (s, e) => window.Close();
            vm.Task = task;
            vm.Start();
            window.ShowDialog();
            return (T)task;
        }
    }
}
