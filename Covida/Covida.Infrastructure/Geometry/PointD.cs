using System;
using System.Collections.Generic;
using System.Text;

namespace Covida.Infrastructure.Geometry
{
    public struct PointD
    {
        public PointD(float x, float y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }
}
