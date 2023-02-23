using System;
using UnityEngine;

namespace Rogue.Map
{
    public static class Noise
    {
        public static float[,] Generate(float xoffset, float yoffset, int width, int height, int seed, float scale, int octaves, float persistance, float lacunarity)
        {
            var random   = new System.Random(seed);
            var xOffsets = new float[octaves];
            var yOffsets = new float[octaves];

            float maxPossibleHeight = 0.0f;
            float amplitude         = 1.0f;
            float frequency         = 1.0f;

            for (int i = 0; i < octaves; i++)
            {
                xOffsets[i] = random.Next(-10000, 10000) + xoffset;
                yOffsets[i] = random.Next(-10000, 10000) + yoffset;

                maxPossibleHeight += amplitude;
                amplitude         *= frequency;
            }

            float halfWidth   = width  * 0.5f;
            float halfHeight  = height * 0.5f;
            float scaleFactor = 1.0f / (scale <= 0 ? 0.0001f : scale);

            float [,] map = new float[width, height];
            float minLocalNoiseHeight = float.MaxValue;
            float maxLocalNoiseHeight = float.MinValue;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    amplitude = 1;
                    frequency = 1;

                    float noise = 0.0f;

                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x - halfWidth  + xOffsets[i]) * scaleFactor * frequency;
                        float sampleY = (y - halfHeight + yOffsets[i]) * scaleFactor * frequency;
                        // Moves the perlin noise to the range [-1, 1].
                        float perlin  = Mathf.PerlinNoise(sampleX, sampleY) * 2.0f - 1.0f;

                        noise     += perlin;
                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    if (noise > maxLocalNoiseHeight) { maxLocalNoiseHeight = noise; }
                    if (noise < minLocalNoiseHeight) { minLocalNoiseHeight = noise; }

                    map[x, y] = noise;
                }
            }
            // Normalization.
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    map[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, map[x, y]);
                }
            }

            return map;
        }
    }
}
