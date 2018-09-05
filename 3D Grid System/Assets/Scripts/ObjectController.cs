using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
	Transform spaceIndicator;
	float rotateSpeed, y_Rotation;
	BoxCollider pickedObjectCollider;
	GameObject pickedObject;

	private void Start()
	{
		/** Set the rotation speed for the object. When they are picked up, they will be rotated with this speed. **/
		rotateSpeed = 10;
		spaceIndicator = transform.GetChild(0);
		/** Subscribe one of the functions to the delegate event from the object pooler script **/
		ObjectPooler.OnObjectChosenFromInventory += selectObject;
	}

	private void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		/** If the cursor is over any of the quads on the ground, move the object picker to that location. **/
		if (Physics.Raycast(ray, out hit, 50, 1 << 9)) transform.position = new Vector3(hit.collider.transform.position.x, 0.05f, hit.collider.transform.position.z);

		/** When an object has been picked up **/
		if (MainManager._instance.objectPickedUp == true)
		{
			rotatePickedObject();

			/** Keep the picked object's position same as that of the child of the object picker i.e. the green quad, but a little over the ground **/
			pickedObject.transform.position = new Vector3(spaceIndicator.position.x, 1, spaceIndicator.position.z);

			/** If left mouse button is clicked, set the picked object on the ground. ElseIf the 'delete' key is pressed, remove it from the scene. **/
			if (Input.GetMouseButtonDown(1) && MainManager._instance.validSpace == true) dropPickedObject(true, false);
			else if (Input.GetKeyDown("delete")) dropPickedObject(false, true);
		}
		/** Checks if the mouse clicked on a pickable object **/
		else if (Physics.Raycast(ray, out hit, 50, 1 << 8) && Input.GetMouseButtonDown(0)) selectObject(hit.collider.gameObject);
	}

	private void selectObject(GameObject obj)
	{
		/** Assign the selected game object to the global game object. Disable it's collider so that it can be moved around the ground. Set the green quad's localscale to that of the collider on the object **/
		pickedObject = obj;
		y_Rotation = pickedObject.transform.eulerAngles.y;
		pickedObjectCollider = pickedObject.GetComponent<BoxCollider>();
		pickedObjectCollider.enabled = false;
		spaceIndicator.localScale = new Vector3(Mathf.RoundToInt(pickedObjectCollider.size.x * 0.5f), Mathf.RoundToInt(pickedObjectCollider.size.z * 0.5f), 1);
		MainManager._instance.objectPickedUp = true;
	}

	private void dropPickedObject(bool placeIt, bool removeIt)
	{
		/** If place, then set the position of the object on the ground. else, send the object's information to the object pooler class and invetory capacity of the said object is increased by 1 **/
		if (placeIt == true) pickedObject.transform.position = new Vector3(spaceIndicator.position.x, 0, spaceIndicator.position.z);
		else
		{
			ObjectPooler._instance.deactivateObject(ref pickedObject, pickedObject.tag);
			InventoryManager._instance.inventoryDictionary[pickedObject.tag]++;
		}

		/** Reset the properties/values when the object was picked up **/
		pickedObject.GetComponent<Collider>().enabled = true;
		pickedObject = null;
		spaceIndicator.localScale = Vector3.one;
		MainManager._instance.objectPickedUp = false;
	}

	private void rotatePickedObject()
	{
		/** Rotate the object when the 'space' bar is pressed and make sure that the object's pitch does not go 360 degrees **/
		if (Input.GetKeyDown("space"))
		{
			if (y_Rotation < 360)
			{
				y_Rotation += 90;
				if (y_Rotation >= 360) y_Rotation = 0;
			}
		}

		pickedObject.transform.rotation = Quaternion.Slerp(pickedObject.transform.rotation, Quaternion.Euler(new Vector3(0, y_Rotation, 0)), rotateSpeed * Time.deltaTime);
	}

	private void OnDisable()
	{
		/** Unsubscribe this script from any event it subscribed to when first enabled **/
		ObjectPooler.OnObjectChosenFromInventory -= selectObject;
	}
}
