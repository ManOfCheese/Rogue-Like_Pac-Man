using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    //Pool class.
    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;                                   //Pool of objects. Needs to be assigned in editor.
    public Dictionary<string, Queue<GameObject>> objectPools;  //Dictionary of object pools. Needs to be assigned in editor.


    void Start () {
		objectPools = new Dictionary<string, Queue<GameObject>>();  //Initialize objectPools dictionary.

        foreach (Pool pool in pools) {
            Queue<GameObject> objectPool = new Queue<GameObject>();  //Make a queue for every objectPool.
            for (int i = 0; i < pool.size; i++) {           
                GameObject obj = Instantiate(pool.prefab);  //Initiliaze an amount of objects equal to the size of the object pool.
                obj.SetActive(false);                       //Set them to be inactive.
                objectPool.Enqueue(obj);                    //Enqueue them.
            }

            objectPools.Add(pool.tag, objectPool);  //The queue to the object pool.
        }
	}


    //Spawn an object from an object pool.
    public GameObject SpawnFromPool(string tag, Vector3 position, Vector3 scale) {
        if (!objectPools.ContainsKey(tag)) {  //First check if the requested object pool exists.
            Debug.Log("Pool with tag " + tag + " doesn't exist");
            return null;
        }
        GameObject objectToSpawn = objectPools[tag].Dequeue();  //Dequeue the object we are spawning.

        objectToSpawn.SetActive(true);                //Activate it.
        objectToSpawn.transform.position = position;  //Set its position.
        objectToSpawn.transform.localScale = scale;   //Set its scale.

        objectPools[tag].Enqueue(objectToSpawn);      //Enqueue the object again.
        return objectToSpawn;                         //Return the object.
    }
}
