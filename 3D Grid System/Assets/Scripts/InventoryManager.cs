using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{

	#region Singleton
	public static InventoryManager _instance;
	#endregion

	public Dictionary<string, int> inventoryDictionary;

	private void Awake()
	{
		_instance = this;
	}

	public void getObject()
	{
		/** Only spawn an object when there isnt an object that has been picked up **/
		if (MainManager._instance.objectPickedUp == false)
		{
			/** Get the tag of the button pressed which corresponds to the object's tag; which is to be spawned **/
			string tag = EventSystem.current.currentSelectedGameObject.tag;
			/** Check if there are more than 0 objects on the ground, but less than 5 **/
			if (inventoryDictionary[tag] > 0)
			{
				/** Send the tag to the object pooler script which will spawn the object. And reduce the number of said object by 1 **/
				ObjectPooler._instance.spawnObject(tag);
				inventoryDictionary[tag]--;
			}
		}
	}
}
