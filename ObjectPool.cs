/* Object Pooler created by David Numa for the Tiger Tail project: https://github.com/dnuma
 * This class controls the pooling for snowballs (bullets) and zombies.
 * 
 * Date: 01/04/2022 
 */

using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Tooltip("Pooled Objets"), SerializeField] private List<GameObject> pooledObjects;
    [Tooltip("Object To Pool"), SerializeField] private GameObject objectToPool;
    [Tooltip("Quantity"), SerializeField] private int amountToPool;

    /// <summary> Instatiate the number of gameobjects entered in the Inspector </summary>
    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject temporalGameObject;
        for(int i = 0; i < amountToPool; i++)
        {
            temporalGameObject = Instantiate(objectToPool);
            temporalGameObject.SetActive(false);
            pooledObjects.Add(temporalGameObject);
        }
    }

    /// <summary> Finds the gameobject that is not enable in the hierarchy and return it </summary>
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
                return pooledObjects[i];
        }
        return null;
    }
}
