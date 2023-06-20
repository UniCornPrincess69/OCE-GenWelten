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

    [SerializeField]
    private Vector3 position;

    [SerializeField]
    private Vector3 rotation;

    [SerializeField]
    private Vector3 scale;

    private Matrix4x4 matrix;

    ShapeGenerator shapeGenerator = new ShapeGenerator();

    TerrainFace[] terrainFaces;
    MeshRenderer[] terrainFaceRenderer;
    MeshFilter[] terrainFaceFilter;

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
        if (terrainFaces == null || terrainFaces.Length != 6 || 
            terrainFaceFilter == null || terrainFaceFilter.Length != 6)
        {
            terrainFaceRenderer = new MeshRenderer[6];
            terrainFaceFilter = new MeshFilter[6];
        }

        GenerateTransformMatrix();
         
        terrainFaces = new TerrainFace[6];

        for (int i = 0; i < 6; i++)
        {
            if (terrainFaceRenderer[i] == null)
            {
                //Object generation
                GameObject newFace = new GameObject();
                newFace.name = $"Terrainface_{i}";
                newFace.transform.SetParent(this.transform);
                newFace.transform.localPosition = Vector3.zero;
                newFace.transform.localRotation = Quaternion.identity;

                //Component distribution
                terrainFaceRenderer[i] = newFace.AddComponent<MeshRenderer>();
                terrainFaceFilter[i] = newFace.AddComponent<MeshFilter>();
                Mesh mesh = new Mesh();
                mesh.name = $"TerrainMesh_{i}";

                terrainFaceFilter[i].sharedMesh = mesh;

                //Give info to terrain faces
            }
            terrainFaces[i] = new TerrainFace(shapeGenerator, terrainFaceFilter[i].sharedMesh, directions[i], matrix);
            terrainFaceRenderer[i].sharedMaterial = terrainFaceMaterial;
        }
    }



    private void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            terrainFaces[i].GenerateMesh(resolution);
        }
    }

    private void GenerateTransformMatrix()
    {
        Matrix4x4 positionMatrix = new Matrix4x4();
        positionMatrix.SetRow(0, new Vector4(0, 0, 0, position.x));
        positionMatrix.SetRow(1, new Vector4(0, 0, 0, position.y));
        positionMatrix.SetRow(2, new Vector4(0, 0, 0, position.z));
        positionMatrix.SetRow(3, new Vector4(0, 0, 0, 1));

        float cosX = Mathf.Cos(rotation.x * Mathf.Deg2Rad);
        float sinX = Mathf.Sin(rotation.x * Mathf.Deg2Rad);
        Matrix4x4 rotationMatrix_X = new Matrix4x4();
        positionMatrix.SetRow(0, new Vector4(1, 0, 0, 0));
        positionMatrix.SetRow(1, new Vector4(0, cosX, -sinX, 0));
        positionMatrix.SetRow(2, new Vector4(0, sinX, cosX, 0));
        positionMatrix.SetRow(3, new Vector4(0, 0, 0, 1));

        float cosY = Mathf.Cos(rotation.x * Mathf.Deg2Rad);
        float sinY = Mathf.Sin(rotation.x * Mathf.Deg2Rad);
        Matrix4x4 rotationMatrix_Y = new Matrix4x4();
        positionMatrix.SetRow(0, new Vector4(cosY, 0, sinY, 0));
        positionMatrix.SetRow(1, new Vector4(0, 0, 0, 0));
        positionMatrix.SetRow(2, new Vector4(-sinY, 0, cosY, 0));
        positionMatrix.SetRow(3, new Vector4(0, 0, 0, 1));

        float cosZ = Mathf.Cos(rotation.x * Mathf.Deg2Rad);
        float sinZ = Mathf.Sin(rotation.x * Mathf.Deg2Rad);
        Matrix4x4 rotationMatrix_Z = new Matrix4x4();
        positionMatrix.SetRow(0, new Vector4(cosZ, -sinZ, 0, 0));
        positionMatrix.SetRow(1, new Vector4(sinZ, cosZ, 0, 0));
        positionMatrix.SetRow(2, new Vector4(0, 0, 0, 0));
        positionMatrix.SetRow(3, new Vector4(0, 0, 0, 1));

        Matrix4x4 rotationMatrixFull = rotationMatrix_X * rotationMatrix_Y * rotationMatrix_Z;

        Matrix4x4 scaleMatrix = new Matrix4x4();
        scaleMatrix.SetRow(0, new Vector4(scale.x, 0, 0, 0));
        scaleMatrix.SetRow(1, new Vector4(0, scale.y, 0, 0));
        scaleMatrix.SetRow(2, new Vector4(0, 0, scale.z, 0));
        scaleMatrix.SetRow(3, new Vector4(0, 0, 0, 1));

        matrix = rotationMatrixFull * scaleMatrix;
    }

    public void OnShapeSettingsUpdate()
    {
        GenerateMesh();
    }
}
