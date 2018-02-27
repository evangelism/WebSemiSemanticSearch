using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSSS
{
    public static class Utils
    {
        public static T[] AsArray<T>(this T x)
        {
            return new T[] { x };
        }
    }
}