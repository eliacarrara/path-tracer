using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer
{
    public struct HitPoint
    {
        public HitPoint(Vector3 origin, float lambda, Vector3 direction, int sphereIndex)
        {
            Lambda = lambda;
            Direction = direction;
            SphereIndex = sphereIndex;
            Origin = origin;
        }

        public Vector3 Position { get { return Origin + Lambda * Direction; } }
        public Vector3 Origin { get; private set; }
        public float Lambda { get; private set; }
        public Vector3 Direction { get; private set; }
        public int SphereIndex { get; private set; }

        public static readonly HitPoint NoHitPoint = 
            new HitPoint(Vector3.Zero, float.PositiveInfinity, Vector3.Zero, -1);

        public bool IsHit()
        {
            return Found(SphereIndex, Lambda);
        }

        public static bool Found(int sphereIndex, float lambda)
        {
            return sphereIndex != NoHitPoint.SphereIndex || lambda != NoHitPoint.Lambda;
        }
    }
}
