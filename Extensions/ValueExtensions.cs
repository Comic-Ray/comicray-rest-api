using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicAPI
{
    public static class ValueExtensions
    {
        public static int ToInt(this string t) => Convert.ToInt32(t);
        public static long ToLong(this string t) => Convert.ToInt64(t);
        public static float ToFloat(this string t) => float.Parse(t);
    }
}
