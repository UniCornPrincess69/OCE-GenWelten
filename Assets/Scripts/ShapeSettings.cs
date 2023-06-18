using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new ShapeSettings", menuName = "Create Shape Settings")]
public class ShapeSettings : ScriptableObject
{
    public bool useFancySphere;
    public float PlanetRadius;
    public NoiseLayer[] NoiseLayers;

    [Serializable]
    public class NoiseLayer
    {
        public bool Enabled;
        public bool UseFirstLayerAsMask;
        public NoiseSettings noiseSettings;
    }
}
