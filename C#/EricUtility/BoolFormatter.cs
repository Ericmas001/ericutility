using System;
using System.Collections.Generic;
using System.Text;

namespace EricUtility
{
    public static class BoolFormatter
    {
        private const string Yes = "Yes";
        private const string No = "No";

        public static bool FromString(string s)
        {
            if (s == Yes) return true;
            return false;
        }

        public static string ToString(bool v)
        {
            if (v) return Yes;
            return No;
        }
    }
}
