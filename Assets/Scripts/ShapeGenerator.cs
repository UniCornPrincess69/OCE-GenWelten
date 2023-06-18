using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings shapeSettings;
    SimplexNoise noise = new SimplexNoise();

    public void UpdateSettings(ShapeSettings shapeSettings)
    {
        this.shapeSettings = shapeSettings;
    }

    public Vector3 TransformCubeToSpherePos(Vector3 cubePos)
    {
        if (shapeSettings.useFancySphere)
        {
            float x2 = cubePos.x * cubePos.x;
            float y2 = cubePos.y * cubePos.y;
            float z2 = cubePos.z * cubePos.z;

            float xPrime = cubePos.x * Mathf.Sqrt(1f - (y2 + z2) / 2f + (y2 * z2) / 3f);
            float yPrime = cubePos.y * Mathf.Sqrt(1f - (x2 + z2) / 2f + (x2 * z2) / 3f);
            float zPrime = cubePos.z * Mathf.Sqrt(1f - (x2 + y2) / 2f + (x2 * y2) / 3f);

            return new Vector3(xPrime, yPrime, zPrime);
        }
        else
        {
            return cubePos.normalized;
        }
    }

    public Vector3 TransformSphereToPlanetPos(Vector3 spherePos)
    {
        float firstLayerValue = 0f;
        float elevation = 0f;

        if (shapeSettings.NoiseLayers.Length >0) 
        {
            firstLayerValue = EvaluateLayer(spherePos, shapeSettings.NoiseLayers[0].noiseSettings);
            if (shapeSettings.NoiseLayers[0].Enabled) elevation = firstLayerValue;
        }

        for (int i = 0; i < shapeSettings.NoiseLayers.Length; i++)
        {
            if (shapeSettings.NoiseLayers[i].Enabled)
            {
                NoiseSettings layersettings = shapeSettings.NoiseLayers[i].noiseSettings;

                float mask = shapeSettings.NoiseLayers[i].UseFirstLayerAsMask ? firstLayerValue : 1;
                float noiseValue = EvaluateLayer(spherePos, layersettings) * mask;
                elevation += noiseValue;
            }
        }


        return spherePos * shapeSettings.PlanetRadius * (1f + elevation);
    }

    private float EvaluateLayer(Vector3 spherePos, NoiseSettings layersettings)
    {
        float noiseValue = 0f;
        float frequency = layersettings.BaseRoughness;
        float amplitude = 1f;

        for (int i = 0; i < layersettings.Layers; i++)
        {
            float v = noise.Evaluate(spherePos * frequency + layersettings.Center);
            noiseValue += (v + 1) * 0.5f * amplitude;

            frequency *= layersettings.Roughness;
            amplitude *= layersettings.Persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - layersettings.GroundLevel);
        return noiseValue * layersettings.Strength;
    }
}
