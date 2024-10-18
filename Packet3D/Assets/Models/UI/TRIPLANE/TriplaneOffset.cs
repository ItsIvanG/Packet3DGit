using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriplaneOffset : MonoBehaviour
{
    // Start is called before the first frame update
    public Material mat;

    public float offset;
    public MeshRenderer mesh;
    [SerializeField] float speed;
    void Start()
    {
        mat = mesh.material;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        offset += speed;
        mat.SetTextureOffset("_BaseMap", new Vector2(0, offset));

    }
}
