using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer.AccelerationStructures
{
    class NoAcceleration : IAccelerationStructure
    {
        private readonly List<Sphere> mSpheres;
        public NoAcceleration(List<Sphere> spheres)
        {
            mSpheres = new List<Sphere>(spheres);
        }

        public HitPoint GetHitPoint(Vector3 d, Vector3 OP)
        {
            var sphereIndex = HitPoint.NoHitPoint.SphereIndex;
            var shortestLambda = HitPoint.NoHitPoint.Lambda;

            for (int i = 0; i < mSpheres.Count; i++)
            {
                var lambda = Helper.VectorSphereHitPoint(d, OP, mSpheres[i]);
                if (lambda < shortestLambda)
                {
                    shortestLambda = lambda;
                    sphereIndex = i;
                }
            }

            if (HitPoint.Found(sphereIndex, shortestLambda))
                return new HitPoint(OP, shortestLambda, d, sphereIndex);
            else
                return HitPoint.NoHitPoint;
        }

        public Sphere this[int index]
        {
            get { return mSpheres[index]; }
        }
    }
}
