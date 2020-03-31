using Covida.Infrastructure.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Covida.Infrastructure.Definitions
{
    public static class CovidaExtensions
    {
        public static bool IsBetween(this DateTime @this, DateTime dateTime1, DateTime dateTime2)
            => dateTime1 <= @this && @this <= dateTime2;
    }
}
