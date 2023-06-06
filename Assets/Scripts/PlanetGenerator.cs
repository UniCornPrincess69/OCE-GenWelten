using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    private static Vector3[] directions = new Vector3[]
    {
        Vector3.right,
        Vector3.left,
        Vector3.up,
        Vector3.down,
        Vector3.forward,
        Vector3.back
    };

    ShapeGenerator shapeGenerator = new ShapeGenerator();

    TerrainFace[] terrainFaces;
    MeshRenderer[] terrainFaceRenderer;

    [SerializeField]
    private Material terrainFaceMaterial;

    [SerializeField, Range(2, 255)]
    private int resolution;

        [SerializeField]
    private ShapeSettings shapeSettings;
    public ShapeSettings Shapesettings => shapeSettings;
    [HideInInspector]
    public bool ShapeSettingsFoldout;

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
    }

    private void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        //6, because a cube needs to be generated
        if (terrainFaces == null || terrainFaces.Length != 6)
            terrainFaces = new TerrainFace[6];
        terrainFaceRenderer = new MeshRenderer[6];
        for (int i = 0; i < 6; i++)
        {
            if (terrainFaces[i] == null)
            {
                //Object generation
                GameObject newFace = new GameObject();
                newFace.name = $"Terrainface_{i}";
                newFace.transform.SetParent(this.transform);
                newFace.transform.localPosition = Vector3.zero;
                newFace.transform.localRotation = Quaternion.identity;

                //Component distribution
                terrainFaceRenderer[i] = newFace.AddComponent<MeshRenderer>();
                MeshFilter filter = newFace.AddComponent<MeshFilter>();
                Mesh mesh = new Mesh();
                mesh.name = $"TerrainMesh_{i}";

                terrainFaceRenderer[i].sharedMaterial = terrainFaceMaterial;
                filter.sharedMesh = mesh;

                //Give info to terrain faces
                terrainFaces[i] = new TerrainFace(shapeGenerator, mesh, directions[i]);
            }
        }
    }

    

    private void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            terrainFaces[i].GenerateMesh(resolution);
        }
    }

    public void OnShapeSettingsUpdate()
    {
        GenerateMesh();
    }
}
