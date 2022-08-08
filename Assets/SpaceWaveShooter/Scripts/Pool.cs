using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    // List of all current pools.
    private List<PoolContainer> pools = new List<PoolContainer>();

    public static Pool inst;

    void Awake () { inst = this; }

    // Spawns in an object from the pool
    public GameObject Spawn (GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        // Get the pool for the requested prefab.
        PoolContainer pool = pools.Find(x => x.sourcePrefab == prefab);

        // If the pool doesn't exist - create it.
        if(pool == null)
        {
            pool = new PoolContainer(prefab);
            pools.Add(pool);
        }

        // Get an available object if there's any.
        GameObject obj = pool.pooledObjects.Find(x => !x.activeInHierarchy);

        // If there are no available objects - create a new one.
        if(obj == null)
        {
            obj = CreateNewObject(prefab);
            pool.pooledObjects.Add(obj);
        }

        // Set position, rotation and parent.
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.transform.parent = parent;

        obj.SetActive(true);

        return obj;
    }

    // Instantiates a new object and returns it.
    GameObject CreateNewObject (GameObject prefab)
    {
        return Instantiate(prefab);
    }

    // Deactivates the requested object.
    public void Destroy (GameObject obj, float time = 0.0f)
    {
        if(time != 0.0f)
            StartCoroutine(DestroyTimer(obj, time));
        else
            obj.SetActive(false);
    }

    IEnumerator DestroyTimer (GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
}

// Data class to pool objects for a specific prefab.
public class PoolContainer
{
    public GameObject sourcePrefab;
    public List<GameObject> pooledObjects = new List<GameObject>();

    public PoolContainer (GameObject prefab)
    {
        sourcePrefab = prefab;
    }
}