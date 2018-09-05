using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsController : MonoBehaviour
{
	Renderer myRenderer;

	private void Awake()
	{
		myRenderer = GetComponent<Renderer>();
	}

	private void Update()
	{
		stayWithinGroundBounds();
	}

	private void stayWithinGroundBounds()
	{
		/** Calculate the space between this and it's parent **/
		float offset_X = (transform.localScale.x / 2) - 0.5f;
		float offset_Z = (transform.localScale.y / 2) - 0.5f;

		/** Calculate the space between the center of the ground and this object in addition to the offset **/
		float centerOffset_X = Mathf.Abs(transform.parent.position.x) + offset_X;
		float centerOffset_Z = Mathf.Abs(transform.parent.position.z) + offset_Z;

		/** Check if out of bounds **/
		if (centerOffset_X > (MainManager._instance.dimension / 2) - 0.5f || centerOffset_Z > (MainManager._instance.dimension / 2) - 0.5f)
		{
			float xPos = 0, yPos = 0;

			/** Check if out of bounds on the X axis **/
			if (centerOffset_X > (MainManager._instance.dimension / 2) - 0.5f)
			{
				/** Calculte the amount of space that went out of bounds based on this object's scale **/
				xPos = centerOffset_X - ((MainManager._instance.dimension / 2) - 0.5f);
				/** Set negative/positive value based on which quadrant the parent is in **/
				if (transform.parent.position.x > 0) xPos = -xPos;
			}

			/** Check if out of bounds on the Z axis **/
			if (centerOffset_Z > (MainManager._instance.dimension / 2) - 0.5f)
			{
				/** Calculte how much the child went out of bounds based on it's scale **/
				yPos = centerOffset_Z - ((MainManager._instance.dimension / 2) - 0.5f);
				/** Set negative/positive value based on which quadrant the parent is in **/
				if (transform.parent.position.z > 0) yPos = -yPos;
			}

			/** Set the 'adjusted' position **/
			transform.localPosition = new Vector3(xPos, yPos, 0.02f);
		}
		/** Keep this object centerd around the anchor **/
		else transform.localPosition = new Vector3(0, 0, 0.02f);
	}

	private void OnTriggerStay(Collider other)
	{
		/** Check if the trigger collided with a pickable object while an object has been picked up **/
		if (other.gameObject.layer == 8 && MainManager._instance.objectPickedUp == true)
		{
			/** Make the space that the anchor/parent is in invalid and change this object's color to red**/
			MainManager._instance.validSpace = false;
			myRenderer.material.color = Color.red;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 8)
		{
			/** Reset/Do the opposite of what is done above **/
			MainManager._instance.validSpace = true;
			myRenderer.material.color = Color.green;
		}
	}
}
