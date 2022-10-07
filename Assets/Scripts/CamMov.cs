using UnityEngine;
using System.Collections;

public class CamMov : MonoBehaviour
{
	private float movSpeed { get; set; } = 60;
	private float sensitivity { get; set; } = 300;
	private float xRotation { get; set; } = 0;
	private float yRotation { get; set; } = 0;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		//makes camera follow mouse and prevents loop de loops
		xRotation += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
		yRotation += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
		yRotation = Mathf.Clamp(yRotation, -90, 90);

		//locks rotation i think? it doesnt work as intended without these 2 lines
		transform.localRotation = Quaternion.AngleAxis(xRotation, Vector3.up);
		transform.localRotation *= Quaternion.AngleAxis(yRotation, Vector3.left);

		transform.position += transform.forward * movSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
		transform.position += transform.right * movSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;

		//up and down movement
		if (Input.GetKey(KeyCode.Space)) { transform.position += transform.up * movSpeed * 0.5f * Time.deltaTime; }
		if (Input.GetKey(KeyCode.LeftShift)) { transform.position -= transform.up * movSpeed* 0.5f * Time.deltaTime; }

		//allows 'zooming in/out' by changing fov, changes sensitivity to compensate for zoom
		if (Input.GetAxis("Mouse ScrollWheel") > 0f && Camera.main.fieldOfView > 30)
		{
			Camera.main.fieldOfView -= 3;
			sensitivity -= 6;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f && Camera.main.fieldOfView < 120)
		{
			Camera.main.fieldOfView+= 3;
			sensitivity += 6;
		}
	}
}