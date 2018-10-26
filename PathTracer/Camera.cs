using System;
using System.Numerics;

namespace PathTracer
{
    public class Camera
    {
        private Vector3 _speedupX;
        private Vector3 _speedupY;
        public Camera(Vector3 up, Vector3 eye, Vector3 lookAt, float fov)
        {
            Eye = eye;
            F = Vector3.Normalize(lookAt - eye);
            var r = Vector3.Normalize(Vector3.Cross(F, Vector3.Normalize(up)));
            _speedupX = (float)Math.Tan(fov / 2) * r;
            _speedupY = (float)Math.Tan(fov / 2) * Vector3.Normalize(Vector3.Cross(F, r));
        }

        public Vector3 Eye { get; private set; }
        private Vector3 F { get; set; }

        public Vector3 CreateEyeRay(float x, float y)
        {
            return Vector3.Normalize(F + (x * _speedupX) + (y * _speedupY));
        }
    }
}