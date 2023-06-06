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
        float noiseValue = noise.Evaluate(spherePos);
        
        return spherePos * (1f + noiseValue);
    }
}
