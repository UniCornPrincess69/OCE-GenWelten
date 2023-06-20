using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    Mesh mesh;
    Vector3 upDirection;
    Vector3 AxisA;
    Vector3 AxisB;
    ShapeGenerator shapeGenerator;

    Matrix4x4 transformMatrix;

    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, Vector3 upDirection, Matrix4x4 transformMatrix)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.upDirection = upDirection;
        AxisA = new Vector3(upDirection.y, upDirection.z, upDirection.x);
        AxisB = Vector3.Cross(AxisA, upDirection);
        this.transformMatrix = transformMatrix;
    }

    public void GenerateMesh(int resolution)
    {
        Vector3[] verts = new Vector3[resolution * resolution];
        //*2 für die Anzahl an Triangles pro Quad
        //*3 für die Anzahl and Indices pro Triangle
        int[] tris = new int[(resolution - 1) * (resolution - 1) * 2 * 3];
        Vector3 startPos = upDirection -AxisA -AxisB;
        int triIdx = 0;
        for (int y = 0, i = 0; y < resolution; y++)
        { 
            for (int x = 0; x < resolution; x++, i++)
            {
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                //*2 at the end, cause the generated Terrainface is always 2x2 units
                Vector3 cubePos = startPos + (AxisA * percent.x + AxisB * percent.y) * 2;
                Vector3 spherePos = shapeGenerator.TransformCubeToSpherePos(cubePos);
                Vector3 planetPos = shapeGenerator.TransformSphereToPlanetPos(spherePos);
                verts[i] = planetPos;

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
