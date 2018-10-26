using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer.Surfaces
{
    public interface ISurface
    {
        Vector3 GetTextureColor(Vector3 n);
    }
}
