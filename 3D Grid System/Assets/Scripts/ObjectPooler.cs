using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
	[SerializeField]
	List<Pool> poolOfObjects;

	public delegate void objectFromInventory(GameObject obj);
	public static event objectFromInventory OnObjectChosenFromInventory;

	#region Singleton
	public static ObjectPooler _instance;
	#endregion

	Dictionary<string, Queue<GameObject>> poolDictionary;

	private void Awake()
	{
		_instance = this;
	}

	void Start()
	{
		createObjectPool();
	}

	private void createObjectPool()
	{
		/** Initialize the dictionary for the pool and also for the inventory manager **/
		poolDictionary = new Dictionary<string, Queue<GameObject>>();
		InventoryManager._instance.inventoryDictionary = new Dictionary<string, int>();

		foreach (Pool pool in poolOfObjects)
		{
			/** Initialize the queue for each individual gameobject and set the tag of the Pool class the same as the tag that the object has **/
			Queue<GameObject> queueOfObjects = new Queue<GameObject>();
			pool.tag = pool.prefab.tag;

			/** Create a number of disabled gameobjects for each pool **/
			for (int i = 0; i < pool.numberOfObjects; i++)
			{
				GameObject obj = Instantiate(pool.prefab);
				obj.transform.position = transform.position;
				obj.SetActive(false);
				queueOfObjects.Enqueue(obj);
			}

			/** Add the necessary information of the pooled objects to the dictionaries **/
			poolDictionary.Add(pool.tag, queueOfObjects);
			InventoryManager._instance.inventoryDictionary.Add(pool.tag, 5);
		}
	}

	/** This is called whenever a new object is to be spawned on scene **/
	public void spawnObject(string tag)
	{
		/** Repost error to programmer when tag of object does not exist **/
		if (poolDictionary.ContainsKey(tag) == false)
		{
			Debug.LogError("ObjectPooler does not contain key: " + tag);
			return;
		}


		GameObject objectToSpawn = poolDictionary[tag].Dequeue();
		objectToSpawn.SetActive(true);
		objectToSpawn.transform.rotation = Quaternion.identity;

		/** Fire off the event to let the object controller script know that a new object has been spawned on screen **/
		if (OnObjectChosenFromInventory != null) OnObjectChosenFromInventory(objectToSpawn);
	}


	/** This function is called when an object is to be removed from the scene. Deactivate the object and enqueue it into it's quueue of pooled objects **/
	public void deactivateObject(ref GameObject obj, string tag)
	{
		obj.SetActive(false);
		obj.transform.position = transform.position;
		poolDictionary[tag].Enqueue(obj);
	}
}


/** This class contains the information of the individual objects that will be pooled **/
[System.Serializable]
public class Pool
{
	public string tag;
	public GameObject prefab;
	public int numberOfObjects;
}

