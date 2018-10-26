using PathTracer.AccelerationStructures;
using PathTracer.Surfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;

namespace PathTracer
{
    public class Scene
    {
        private const float P = 0.2f;
        private const float P_INV = 1 - P;

        private readonly IAccelerationStructure _spheres;
        private readonly Camera _camera;
        private readonly Random rnd = new Random();

        public Scene(Camera camera, IAccelerationStructure spheres)
        {
            _camera = camera;
            _spheres = spheres;
        }

        public Vector3 Sample(float u, float v)
        {
            return CalcColor(_spheres.GetHitPoint(_camera.CreateEyeRay(u, v), _camera.Eye));
        }

        private Vector3 CalcColor(HitPoint hp)
        {
            if (!hp.IsHit())
                return Vector3.Zero;

            var sphere = _spheres[hp.SphereIndex];
            var hpP = hp.Position;
            var n = Vector3.Normalize(hpP - sphere.Position);

            if (rnd.NextDouble() < P)
                return sphere.EmissionColor;

            Vector3 color = Vector3.Zero;
            var wr = Helper.CalcRndHemisphereDirection(n);
            var hpWR = _spheres.GetHitPoint(wr, hpP + n * 0.01f);
            if (hpWR.IsHit())
            {
                var sphereWR = _spheres[hpWR.SphereIndex];
                var nWR = Vector3.Normalize(hpWR.Position - sphereWR.Position);
                color = CalcColor(hpWR) * P_INV * sphere.GetColor(n) * Vector3.Dot(wr, n) * sphereWR.GetColor(nWR) * Helper.PI2;
            }

            return sphere.EmissionColor + color;
        }
    }
}