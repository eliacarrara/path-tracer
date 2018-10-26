using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer.Surfaces
{
    class SingleColorSurface : ISurface
    {
        Vector3 color;

        public SingleColorSurface(Vector3 color, float colorCorrectionFactor = 1.0f)
        {
            this.color = color * colorCorrectionFactor;
        }

        public Vector3 GetTextureColor(Vector3 n)
        {
            return color;
        }
    }
}
