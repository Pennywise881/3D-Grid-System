using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
	#region Singleton
	public static MainManager _instance;
	#endregion

	public bool objectPickedUp, validSpace;

	[SerializeField]
	Transform ground;

	[Range(1, 5)]
	public int groundSize;

	[HideInInspector]
	public float dimension;

	private void Awake()
	{
		_instance = this;
		validSpace = true;
	}

	private void Start()
	{
		setUpGround();
	}

	private void setUpGround()
	{
		float xPos, zPos;

		/**Set the position of the ground and calculate it's dimensions based on it's scale. A ground with scale 1 means its 10X10 **/
		ground.localScale = new Vector3(groundSize, 1, groundSize);
		dimension = ground.localScale.x * 10;
		zPos = (dimension / 2) - 0.5f;
		xPos = -(dimension / 2) + 0.5f;


		/** Go across the ground and set up quads on each row and column. Set the layer of each of the quads. **/
		for (int i = 0; i < dimension; i++)
		{
			for (int j = 0; j < dimension; j++)
			{
				GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
				quad.transform.position = new Vector3(xPos, 0.01f, zPos);
				quad.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
				quad.GetComponent<Renderer>().material.color = Color.black;
				quad.transform.parent = ground;
				quad.layer = 9;
				xPos += 1;
			}
			/** Go to the next row. Column starts from the beginning again **/
			zPos -= 1;
			xPos = -(dimension / 2) + 0.5f;
		}

		ground.position = new Vector3(0, 0, 0);
	}
}
