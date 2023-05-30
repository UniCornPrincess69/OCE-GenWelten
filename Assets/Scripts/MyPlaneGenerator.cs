using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MyPlaneGenerator : MonoBehaviour
{
    private MeshFilter filter = null;
    private MeshRenderer meshRenderer = null;
    [SerializeField]
    private Material meshMaterial = null;
    private Mesh mesh = null;

    [SerializeField]
    private float planeSize;
    [SerializeField, Range(2, 255)]
    private int resolution;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        filter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        mesh.name = "PlaneGeneratorMesh";

        meshRenderer.material = meshMaterial;
        filter.sharedMesh = mesh;
    }

    private void Update()
    {
        GeneratePlane();
    }

    ////private void GenerateQuadPlane()
    //{
    //    Vector3[] verts = new Vector3[4];

    //    verts[0] = (Vector3.left + Vector3.down) * quadSize * 0.5f;
    //    verts[1] = (Vector3.right + Vector3.down) * quadSize * 0.5f;
    //    verts[2] = (Vector3.left + Vector3.up) * quadSize * 0.5f;
    //    verts[3] = (Vector3.right + Vector3.up) * quadSize * 0.5f;

    //    int[] tris = new int[]
    //    {
    //        0,2,3,
    //        0,3,1
    //    };

    //    mesh.Clear();
    //    mesh.vertices = verts;
    //    mesh.triangles = tris;
    //    mesh.RecalculateNormals();
    //}

    private void GeneratePlane()
    {
        Vector3[] verts = new Vector3[resolution * resolution];
        //*2 für die Anzahl an Triangles pro Quad
        //*3 für die Anzahl and Indices pro Triangle
        int[] tris = new int[(resolution -1) * (resolution - 1) * 2 * 3];
        Vector3 startPos = (Vector3.left + Vector3.back) * planeSize * 0.5f;
        int triIdx = 0;
        for (int y = 0, i = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++, i++)
            {
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 vertPos = startPos + (Vector3.right * percent.x + Vector3.forward * percent.y) * planeSize;

                verts[i] = vertPos;

                if (x < resolution - 1 && y < resolution - 1)
                {
                    //Vertex kann ein Quad generieren!
                    tris[triIdx + 0] = i;
                    tris[triIdx + 1] = i + resolution;
                    tris[triIdx + 2] = i + resolution + 1;

                    tris[triIdx + 3] = i;
                    tris[triIdx + 4] = i + resolution + 1;
                    tris[triIdx + 5] = i + 1;

                    //+6 weil wir 6 neue Indices hinzugefügt haben
                    triIdx += 6;
                }
            }
        }

        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }
}
