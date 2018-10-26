using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracer
{
    class Renderer
    {
        private const int SAMPLE_COUNT = 16000;
        private readonly float[] _screenPoints;
        private readonly float _sigmaX;
        private readonly float _sigmaY;
        private readonly byte[] _data;

        public Renderer(int width, int height, int stride)
        {
            if (stride < width) throw new ArgumentException();
            if (stride % 4 != 0) throw new ArgumentException();

            _screenPoints = CalculateScreenPoints(width, height);
            _sigmaX = 0.5f / width;
            _sigmaY = 0.5f / height;
            _data = new byte[height * stride];

            Width = width;
            Height = height;
            Stride = stride;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Stride { get; private set; }

        public byte[] Render(Scene s)
        {
            var j = 0;
            for (int y = 0; y < Height; y++)
            {
                var i = y * Stride;
                for (int x = 0; x < Width; x++)
                {
                    var color = Vector3.Zero;
                    var u = _screenPoints[j++];
                    var v = _screenPoints[j++];

                    for (int n = 0; n < SAMPLE_COUNT; n++)
                    {
                        var u1 = Helper.RandomGaussValue(u, _sigmaX);
                        var v1 = Helper.RandomGaussValue(v, _sigmaY);
                        color += s.Sample(u1, v1);
                    }

                    color /= SAMPLE_COUNT;

                    _data[i++] = Helper.Clamp8(Helper.GammaCorrectionToSRGB(color.X) * 255);
                    _data[i++] = Helper.Clamp8(Helper.GammaCorrectionToSRGB(color.Y) * 255);
                    _data[i++] = Helper.Clamp8(Helper.GammaCorrectionToSRGB(color.Z) * 255);
                }
            }

            return _data;
        }

        private static float[] CalculateScreenPoints(int width, int height)
        {
            float[] points = new float[width * height * 2];
            int _centerX = width / 2;
            int _centerY = height / 2;

            int i = 0;
            for (int y = 0; y < height; y++)
            {
                var v = -(y - _centerY) / (float)_centerY;
                for (int x = 0; x < width; x++)
                {
                    var u = -(x - _centerX) / (float)_centerX;
                    points[i++] = u;
                    points[i++] = v;
                }
            }

            return points;
        }
    }
}
