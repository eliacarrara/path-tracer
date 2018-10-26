using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer.AccelerationStructures
{

    public interface IAccelerationStructure
    {
        HitPoint GetHitPoint(Vector3 d, Vector3 OP);

        Sphere this[int index] { get; }
    }
}
