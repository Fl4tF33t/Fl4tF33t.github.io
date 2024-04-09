using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class CreateMesh : MonoBehaviour
{
    //plug in the sprite that you want to create the custom mesh for
    //ensure that the sprite has gone through the sprite editor and has custom physics shape
    //reset the polygon collider 2d to ensure that the collider is the same as the sprite
    //in the child object, a new mesh is available, click save as instance

    PolygonCollider2D polygonCollider2D;
    GameObject child;
    Mesh mesh;
    private void Awake()
    {
        child = new GameObject("MeshCreator");
        mesh = new Mesh();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        child.transform.parent = transform;
        mesh = polygonCollider2D.CreateMesh(true, true);
        InitializeChild(child, mesh);
    }

    void InitializeChild(GameObject obj, Mesh mesh)
    {
        obj.AddComponent<MeshFilter>();
        obj.GetComponent<MeshFilter>().mesh = mesh;
    }
}
