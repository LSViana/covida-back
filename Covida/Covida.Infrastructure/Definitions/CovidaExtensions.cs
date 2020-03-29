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

        public static PointD ToPointD(this NetTopologySuite.Geometries.Point @this)
            => new PointD((float)@this.X, (float)@this.Y);

        public static NetTopologySuite.Geometries.Point ToPoint(this PointD @this)
            => new NetTopologySuite.Geometries.Point(@this.X, @this.Y);
    }
}
