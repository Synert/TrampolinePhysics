using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineManager : MonoBehaviour
{
    [SerializeField] Spring springPrefab;
    [SerializeField] private float distance = 1.0f; //distance between springs
    [SerializeField] private int size = 5;
    private Vector3 initPos = Vector3.zero;
    [SerializeField] private float minForce = 1000.0f;
    [SerializeField] private float maxMult = 3.0f;
    [SerializeField] private float boxHeight = 3.0f;
    private Mesh mesh;
    private Spring[,] springs;
    private Vector3[] vertices;
    private int[] triangles;

	void Start()
    {
        initPos = transform.position - new Vector3(0.0f, boxHeight, 0.0f);
        mesh = gameObject.AddComponent<MeshFilter>().mesh;
        mesh = new Mesh();
        springs = new Spring[size * 2, size * 2];
        vertices = new Vector3[(size * 2) * (size * 2)];
        triangles = new int[(size * 2) * (size * 2) * 6];
        int numTris = 0;
        for(int x = -size; x < size; x++)
        {
            for (int y = -size; y < size; y++)
            {
                Spring newSpring = Instantiate(springPrefab, transform);
                float scalar = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y)) / (float)size;
                scalar *= (maxMult - 1.0f);
                scalar += 1.0f;
                newSpring.SetForce(minForce * scalar, minForce);
                newSpring.SetDistance(distance);
                newSpring.transform.position = initPos + new Vector3(x * distance, 0, y * distance);
                newSpring.GetComponentInChildren<Rigidbody>().transform.localScale = new Vector3(distance * 0.75f, boxHeight, distance * 0.75f);
                springs[(x + size), (y + size)] = newSpring;
                vertices[(x + size) + (y + size) * (size * 2)] = new Vector3(x * distance, 0, y * distance);
            }
        }

        for (int x = 1; x < size * 2; x++)
        {
            for (int y = 1; y < size * 2; y++)
            {
                triangles[numTris] = (x - 1) + y * (size * 2);
                triangles[numTris + 1] = x + y * (size * 2);
                triangles[numTris + 2] = x + (y - 1) * (size * 2);

                triangles[numTris + 3] = x + (y - 1) * (size * 2);
                triangles[numTris + 4] = (x - 1) + (y - 1) * (size * 2);
                triangles[numTris + 5] = (x - 1) + y * (size * 2);

                numTris += 6;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;

        GetComponent<MeshCollider>().sharedMesh = mesh;

        GetComponent<SkinnedMeshRenderer>().sharedMesh = mesh;
	}
	
	void Update()
    {
        //mesh.Clear();
        Vector3 add = new Vector3(0.0f, boxHeight / 2.0f, 0.0f);
        add -= transform.position;

        for (int x = -size; x < size; x++)
        {
            for (int y = -size; y < size; y++)
            {
                Vector3 newPos = springs[(x + size), (y + size)].GetComponentInChildren<Rigidbody>().transform.position + add;
                vertices[(x + size) + (y + size) * (size * 2)] = Vector3.Lerp(vertices[(x + size) + (y + size) * (size * 2)], newPos, 0.15f);
            }
        }

        mesh.vertices = vertices;
        //mesh.triangles = triangles;
        mesh.RecalculateNormals();
	}
}
