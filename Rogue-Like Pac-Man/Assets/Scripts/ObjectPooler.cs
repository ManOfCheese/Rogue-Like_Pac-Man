using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    private static ObjectPooler _instance;

    public static ObjectPooler Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<ObjectPooler>();
            }

            return _instance;
        }
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> pelletPool;

	// Use this for initialization
	void Start () {
		pelletPool = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools) {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++) {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            pelletPool.Add(pool.tag, objectPool);
        }
	}

    public GameObject SpawnFromPool(string tag, Vector3 position, Vector3 scale) {
        if (!pelletPool.ContainsKey(tag)) {
            Debug.Log("Pool with tag " + tag + " doesn't exist");
            return null;
        }
        GameObject objectToSpawn = pelletPool[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.localScale = scale;

        pelletPool[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
}
