using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	float rotationSmoothTime;
	float radius, y_Rotation, zoomValue;
	float maxRadius;

	[SerializeField]
	Transform ground;

	Vector3 currentRotation, rotationSmoothVelocity;

	private void Start()
	{
		y_Rotation = 0;
		maxRadius = MainManager._instance.groundSize * 11;
		radius = maxRadius * 0.5f;
		zoomValue = radius;
	}

	private void Update()
	{
		/** Rotation is done when these 2 keys are pressed. 'a': rotate right. 'd': rotate left **/
		if (Input.GetKey("a")) y_Rotation += 50 * Time.deltaTime;
		else if (Input.GetKey("d")) y_Rotation -= 50 * Time.deltaTime;

		/** Scroll wheel UP axis is GREATER than 0 ZOOM IN **/
		/** Scroll wheel DOWN axis is LESS than 0  ZOOM OUT **/

		/** Zoom in and out with the mouse scroll wheel **/
		if (Input.GetAxis("Mouse ScrollWheel") > 0 && Mathf.Abs(zoomValue) > (maxRadius * 0.5f)) zoomValue -= 20 * Time.deltaTime;
		else if (Input.GetAxis("Mouse ScrollWheel") < 0 && Mathf.Abs(zoomValue) < (maxRadius * 0.98f)) zoomValue += 20 * Time.deltaTime;

		/** Control the distance the camera is from the ground **/
		radius = Mathf.Lerp(radius, zoomValue, 8 * Time.deltaTime);

		currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(35, y_Rotation, 0), ref rotationSmoothVelocity, rotationSmoothTime);
		transform.eulerAngles = currentRotation;
		transform.position = ground.position - (radius * transform.forward);
	}
}
