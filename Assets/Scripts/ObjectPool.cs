using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject cubeSource;
    public GameObject CubePrefab;

    [Range(0.001f, 10)]
    public float SpawnTime;

    private Queue<GameObject> q1 = new Queue<GameObject>();
    private Queue<GameObject> q2 = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 200; i++)
        {
            GameObject startCube = Instantiate(CubePrefab, cubeSource.transform.position, RandomRot());
            q1.Enqueue(startCube);
            startCube.SetActive(false);
        }
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {
        PoolCube();
    }

    void PoolCube()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (q1.Count != 0)
            {
                GameObject genCube = q1.Dequeue();
                q2.Enqueue(genCube);
                genCube.SetActive(true);
                genCube.transform.rotation = RandomRot();
            }
            else
            {
                for (int i = 0; i < 50; i++)
                {
                    GameObject startCube = Instantiate(CubePrefab, cubeSource.transform.position, RandomRot());
                    q1.Enqueue(startCube);
                    startCube.SetActive(false);
                }
            }
        }

        if (Input.GetKey(KeyCode.Return))
        {
            if (q2.Count != 0)
            {
                GameObject genCube = q2.Dequeue();
                q1.Enqueue(genCube);
                genCube.transform.position = cubeSource.transform.position;
                genCube.SetActive(false);
            }
        }
    }

    IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(SpawnTime);
            if (q1.Count != 0)
            {
                GameObject genCube = q1.Dequeue();
                q2.Enqueue(genCube);
                genCube.SetActive(true);
                genCube.transform.rotation = RandomRot();
            }
        }
    }

    Quaternion RandomRot()
    {
        float x = Random.Range(0, 360);
        float y = Random.Range(0, 360);
        float z = Random.Range(0, 360);

        Quaternion ret = Quaternion.Euler(x, y, z);
        return ret;
    }
}