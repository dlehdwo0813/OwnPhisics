using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Water : MonoBehaviour {

    float[] xPos;
    float[] yPos;
    float[] velocities;
    float[] accelerations;
    LineRenderer body;

    GameObject[] meshObjects;
    Mesh[] meshes;

    GameObject[] colliders;

    const float _springConstant = 0.02f;
    const float _damping = 0.04f;
    const float _spread = 0.05f;
    public float _z = -1f;

    public float waterGravity = .4f;

    float baseHeight;
    float left;
    float bottom;

    public GameObject splash;

    public Material material;

    public GameObject watherMesh;


    // Use this for initialization
    void Start()
    {
        SpawnWater(transform.position.x - transform.localScale.x / 2, transform.localScale.x
            , transform.position.y + transform.localScale.y / 2, transform.position.y - transform.localScale.y / 2);
        BoxCollider2D bc2D = GetComponent<BoxCollider2D>();
        bc2D.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnWater(float Left, float Width, float Top, float Bottom)
    {
        //gameObject.AddComponent<BoxCollider2D>();
        //gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(Left + Width / 2, Bottom + transform.localScale.y/2);
        //gameObject.GetComponent<BoxCollider2D>().size = new Vector2(Width, Top - Bottom);
        //gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

        int edgeCount = Mathf.RoundToInt(Width) * 5;
        int nodeCount = edgeCount + 1;

        body = gameObject.AddComponent<LineRenderer>();
        body.material = material;
        body.material.renderQueue = 1000;
        body.positionCount = nodeCount;
        body.startWidth = 0.1f;
        body.endWidth = 0.1f;

        xPos = new float[nodeCount];
        yPos = new float[nodeCount];
        velocities = new float[nodeCount];
        accelerations = new float[nodeCount];

        meshObjects = new GameObject[edgeCount];
        meshes = new Mesh[edgeCount];
        colliders = new GameObject[edgeCount];

        baseHeight = Top;
        bottom = Bottom;
        left = Left;


        for (int i = 0; i < nodeCount; i++)
        {
            yPos[i] = Top;
            xPos[i] = left + Width * i / edgeCount;
            accelerations[i] = 0;
            velocities[i] = 0;
            body.SetPosition(i, new Vector3(xPos[i], yPos[i], _z));
        }

        for (int i = 0; i < edgeCount; i++)
        {
            meshes[i] = new Mesh();

            Vector3[] Vertices = new Vector3[4];
            Vertices[0] = new Vector3(xPos[i], yPos[i], _z);
            Vertices[1] = new Vector3(xPos[i + 1], yPos[i + 1], _z);
            Vertices[2] = new Vector3(xPos[i], bottom, _z);
            Vertices[3] = new Vector3(xPos[i + 1], bottom, _z);

            Vector2[] UVs = new Vector2[4];
            UVs[0] = new Vector2(0, 1);
            UVs[0] = new Vector2(1, 1);
            UVs[0] = new Vector2(0, 0);
            UVs[0] = new Vector2(1, 0);

            int[] idx = new int[6] { 0, 1, 3, 3, 2, 0 };

            meshes[i].vertices = Vertices;
            meshes[i].uv = UVs;
            meshes[i].triangles = idx;

            meshObjects[i] = Instantiate(watherMesh, Vector3.zero, Quaternion.identity) /*as GameObject*/;
            meshObjects[i].GetComponent<MeshFilter>().mesh = meshes[i];
            meshObjects[i].transform.parent = transform;
            meshObjects[i].layer = LayerMask.NameToLayer("Water");

            colliders[i] = new GameObject();
            colliders[i].name = "Trigger";
            colliders[i].AddComponent<BoxCollider2D>();
            colliders[i].transform.parent = transform;
            colliders[i].transform.position = new Vector3(left + Width * (i + 0.5f) / edgeCount, (bottom + Top) / 2, 0);
            colliders[i].transform.localScale = new Vector3(Width / edgeCount, 1, 1);
            colliders[i].GetComponent<BoxCollider2D>().isTrigger = true;
            colliders[i].GetComponent<BoxCollider2D>().size = new Vector2(Width / edgeCount, 1);
            colliders[i].AddComponent<WaterDetector>();
            colliders[i].layer = LayerMask.NameToLayer("Water");
        }
    }

    void UpdateMeshes()
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            Vector3[] Vertices = new Vector3[4];
            Vertices[0] = new Vector3(xPos[i], yPos[i], _z);
            Vertices[1] = new Vector3(xPos[i + 1], yPos[i + 1], _z);
            Vertices[2] = new Vector3(xPos[i], bottom, _z);
            Vertices[3] = new Vector3(xPos[i + 1], bottom, _z);

            meshes[i].vertices = Vertices;
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < xPos.Length; i++)
        {
            float force = _springConstant * (yPos[i] - baseHeight) + velocities[i] * _damping;
            accelerations[i] = -force/*/mass*/;
            yPos[i] += velocities[i];
            velocities[i] += accelerations[i];
            body.SetPosition(i, new Vector3(xPos[i], yPos[i], _z));
        }

        float[] leftDeltas = new float[xPos.Length];
        float[] rightDeltas = new float[xPos.Length];

        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < xPos.Length; i++)
            {
                if (i > 0)
                {
                    leftDeltas[i] = _spread * (yPos[i] - yPos[i - 1]);
                    velocities[i - 1] += leftDeltas[i];
                }
                if (i < xPos.Length - 1)
                {
                    rightDeltas[i] = _spread * (yPos[i] - yPos[i + 1]);
                    velocities[i + 1] += rightDeltas[i];
                }
            }
        }

        for (int i = 0; i < xPos.Length; i++)
        {
            if (i > 0)
            {
                yPos[i - 1] += leftDeltas[i];
            }
            if (i < xPos.Length - 1)
            {
                yPos[i + 1] += rightDeltas[i];
            }
        }
        UpdateMeshes();
    }

    public void Splash(float xpos, float ypos, float velocity)
    {
        if (xpos >= xPos[0] && xpos <= xPos[xPos.Length - 1])
        {
            xpos -= xPos[0];

            int idx = Mathf.RoundToInt((xPos.Length - 1) * (xpos / (xPos[xPos.Length - 1] - xPos[0])));

            velocities[idx] += velocity;

            if (Mathf.Abs(ypos - baseHeight) < .7f)
            {
                float lifeTime = 0.93f + Mathf.Abs(velocity) + 0.07f;

                ParticleSystem.MainModule main = splash.GetComponent<ParticleSystem>().main;
                main.startSpeed = 8 + 2 * Mathf.Pow(Mathf.Abs(velocity), 0.5f);
                main.startSpeed = 9 + 2 * Mathf.Pow(Mathf.Abs(velocity), 0.5f);
                //유니티 파티클 시스템의 시작 시간은 두 수 사이의 랜덤한 값으로 정해진다.
                //스크립트상에서 파티클 시스템에 접근하는데 한계가 있기 때문에 startSpeed에 값을 두 번 넣어주는 것
                main.startLifetime = lifeTime;

                Vector3 position = new Vector3(xPos[idx], yPos[idx] - 0.35f, -.7f);
                Quaternion rotation = Quaternion.LookRotation(
                    new Vector3(xPos[Mathf.FloorToInt(xPos.Length / 2)], baseHeight + 8, 5) - position);

                GameObject splish = Instantiate(splash, position, rotation) as GameObject;
                Destroy(splish, lifeTime + 0.3f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb2d = collision.transform.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            Player player = rb2d.GetComponent<Player>();
            if (player)
            {
                if (player.gravityScale != waterGravity)
                {
                    Debug.Log("Water : " + player.ToString());
                    player.gravityScale = waterGravity;
                }

            }

            BoxObject box = collision.transform.GetComponent<BoxObject>();
            if (box)
            {
                if (box.gravityScale != waterGravity)
                {
                    Debug.Log("Water : " + box.ToString());
                    box.gravityScale = waterGravity;
                }

            }


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rb2d = collision.transform.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player)
            {
                if (player.gravityScale != 1f)
                {
                    Debug.Log("Water : " + player.ToString());
                    player.gravityScale = 1f;
                }

            }

            BoxObject box = collision.transform.GetComponent<BoxObject>();
            if (box)
            {
                if (box.gravityScale != 1f)
                {
                    Debug.Log("Water : " + box.ToString());
                    box.gravityScale = 1f;
                }

            }
        }


    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Rigidbody2D rb2d = collision.transform.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            Debug.Log("Water : " + rb2d.ToString());
            if (rb2d.mass < 2f)
            {
                Debug.Log("Water : AddForce :" + rb2d.ToString());
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

}
