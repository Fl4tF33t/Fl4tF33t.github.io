using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is used to to create a pool of objects, having them ready to be used
//Array of objects is created and filled with the prefabs that are set in the inspector
//The objects will be set to inactive and will be activated from another script
public class ObjectPool : Singleton<ObjectPool>
{
    private const string PooledObjs = "PooledObjects";
    [Serializable]
    public class PooledObjects
    {
        public List<GameObject> pooledObjects = new List<GameObject>();
    }
    public PooledObjects[] separatedPooledObjects;

    [Serializable]
    public struct ObjectsToPool
    {
        public GameObject prefab;
        public int amount; 
    }
    public ObjectsToPool[] objectsToPool;

    protected override void Awake()
    {
        base.Awake();
        // Create a new GameObject
        GameObject newObject = new GameObject();

        // Set the name of the GameObject
        newObject.name = PooledObjs;
    }

    void Start()
    {
        Transform location = GameObject.Find(PooledObjs).transform;

        // Initialize the separate object pools based on defined objects and amounts
        separatedPooledObjects = new PooledObjects[objectsToPool.Length];
        GameObject tmp;
        for (int typesOfObjects = 0; typesOfObjects < objectsToPool.Length; typesOfObjects++)
        {
            separatedPooledObjects[typesOfObjects] = new PooledObjects();
            for (int amountOfObjects = 0; amountOfObjects < objectsToPool[typesOfObjects].amount; amountOfObjects++)
            {
                // Instantiate objects and add them to the pool
                tmp = Instantiate(objectsToPool[typesOfObjects].prefab, location.position, Quaternion.identity);
                tmp.SetActive(false);
                tmp.transform.SetParent(location);
                separatedPooledObjects[typesOfObjects].pooledObjects.Add(tmp);
            }
        }
    }

    // Retrieve an inactive object from the pool based on bug type
    public GameObject GetPooledObject(string type)
    {
        for (int i = 0; i < separatedPooledObjects.Length; i++)
        {
            if (!separatedPooledObjects[i].pooledObjects[0].name.Contains(type))
            {
                continue;
            }else if (separatedPooledObjects[i].pooledObjects[0].name.Contains(type))
            {
                for(int j = 0; j < separatedPooledObjects[i].pooledObjects.Count; j++)
                {
                    if (!separatedPooledObjects[i].pooledObjects[j].activeInHierarchy)
                    {
                        return separatedPooledObjects[i].pooledObjects[j];
                    }
                }
            }  
        }
        return null;
    }

    // Check if any objects are currently active in the pool for a specific bug type
    public bool AnyPooledObjectsActive(string type)
    {
        for (int i = 0; i < separatedPooledObjects.Length; i++)
        {
            if (!separatedPooledObjects[i].pooledObjects[0].name.Contains(type))
            {
                continue;
            }
            else if (separatedPooledObjects[i].pooledObjects[0].name.Contains(type))
            {
                for (int j = 0; j < separatedPooledObjects[i].pooledObjects.Count; j++)
                {
                    if (separatedPooledObjects[i].pooledObjects[j].activeInHierarchy)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // Check if any objects are currently active in the pool for all bug types
    public bool AnyPooledObjectsActiveForAllTypes()
    {
        foreach (PooledObjects type in separatedPooledObjects)
        {
            if (AnyPooledObjectsActive(type.pooledObjects[0].name))
            {
                return true;
            }
        }
        return false;
    }
}
