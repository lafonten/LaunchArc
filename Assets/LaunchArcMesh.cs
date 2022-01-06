using System;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class LaunchArcMesh : MonoBehaviour
{
    private Mesh mesh;
    public float meshWidth;

    public float velocity;
    public float angle;
    public int resolution = 10;

    private float gravity; //force of gravity on Y axis;
    private float radianAngle;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        gravity = Mathf.Abs(Physics.gravity.y);
    }

    private void OnValidate()
    {
        //check that mesh is not null and that the game is playing
        if (mesh != null && Application.isPlaying)
        {
            MakeArcMesh(CalculateArcArray());
        }
    }

    void Start()
    {
        MakeArcMesh(CalculateArcArray());
    }

    void MakeArcMesh(Vector3[] arcVector3s)
    {
        mesh.Clear();
        Vector3[] vertices = new Vector3[(resolution + 1) * 2];
        int[] triangles = new int[resolution * 6 *2 ];

        for (int i = 0; i <= resolution; i++)
        {
            //set vertices

            vertices[i * 2] = new Vector3(meshWidth * 0.5f, arcVector3s[i].y, arcVector3s[i].x);
            vertices[i * 2 + 1] = new Vector3(meshWidth * -0.5f, arcVector3s[i].y, arcVector3s[i].x);


            //set triangles

            if (i != resolution)
            {
                triangles[i * 12] = i * 2;
                triangles[i * 12 + 1] = triangles[i * 12 + 4] = i * 2 + 1;
                triangles[i * 12 + 2] = triangles[i * 12 + 3] = (i + 1) * 2; ;
                triangles[i * 12 + 5] = (i + 1) * 2 + 1;

                triangles[i * 12 +6] = i * 2;
                triangles[i * 12 + 7] = triangles[i * 12 + 10] = (i + 1) * 2;
                triangles[i * 12 + 8] = triangles[i * 12 + 9] = i * 2 + 1;
                triangles[i * 12 + 11] = (i + 1) * 2 + 1;
                
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
        }

    }

   //create an array of vector3 positions for arc
    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];

        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / gravity;
        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }

        return arcArray;

    }

    //calculate height and distance of each vertex

    Vector3 CalculateArcPoint(float t, float maxDistance)
    {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - ((gravity * x * x) /
                                                (2 * velocity * velocity * Mathf.Cos(radianAngle) *
                                                 Mathf.Cos(radianAngle)));
        return new Vector3(x, y);
    }

}