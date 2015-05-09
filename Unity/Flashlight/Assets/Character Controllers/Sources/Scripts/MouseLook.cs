using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;
	float rotationY = 0F;

	private ArduinoInput ArdInput;
	public Light flashlight;
	public GameObject Player;
	public int Potentiometer;


	void Start ()
	{
		ArdInput = (ArduinoInput)Player.GetComponent(typeof(ArduinoInput));

		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}

	void Update ()
	{		
		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
			
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			//transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
			transform.Rotate(0, int.Parse(ArdInput.vec[4]) * sensitivityX, 0);
		}
		else
		{
			//rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY += (int.Parse(ArdInput.vec[3])) * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);


			if (Input.GetKey (KeyCode.Q)|| (int.Parse(ArdInput.vec[9])) == 1) {
				flashlight.enabled = !flashlight.enabled;
			}
		}

		Potentiometer = int.Parse (ArdInput.vec [10]);

		flashlight.spotAngle = Mathf.Lerp (45, 15, Remap(Potentiometer, 0, 99, 0, 1));
		flashlight.range = Mathf.Lerp (15, 45, Remap(Potentiometer, 0, 99, 0, 1));

	}

	private float Remap(float input, float from1, float to1, float from2, float to2){
		return (input - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
}