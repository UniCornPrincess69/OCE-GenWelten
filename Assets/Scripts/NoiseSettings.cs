using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class NoiseSettings
{
    public float Strength;

    [Range(1, 8)]
    public int Layers;
    public float Persistence;

    public float BaseRoughness;
    public float Roughness;

    public Vector3 Center;
    public float GroundLevel;
}
