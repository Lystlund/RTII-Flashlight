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
	private float valToBeLerped = 0;
	private float tParam = 0;
	private float Speed = 1f;
	private bool trigger = false;
	private bool trigger2 = false;
	private float currentVal;

	// // // // // // // // // // // // // 
	// Find en måde at reset tParam, tilbage til 0 når Q bliver trykket på/sluppet
	//
	// // // // // // // // // // // // // 


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

			currentVal = valToBeLerped;

			if (Input.GetKey (KeyCode.Q) || (int.Parse(ArdInput.vec[9])) == 1) {
				rotationY = 0;
				transform.localEulerAngles = new Vector3(0,0,0);

				if (trigger2 == true){
					trigger2 = false;
					tParam = 0;
					trigger = true;
				}
			}

			if (Input.GetKey(KeyCode.Q) == false && (int.Parse(ArdInput.vec[9])) == 0){
				if (trigger2 == false){
					trigger = false;
					tParam = 0;
					trigger2 = true;
				}
			}

			if (trigger){
				if (tParam < 1){
					tParam += Time.deltaTime * Speed;
					valToBeLerped = Mathf.Lerp(currentVal,10,tParam);

					flashlight.range = 25;
					flashlight.spotAngle = valToBeLerped;
				}
			}

			if (trigger == false){
				if (tParam < 1){
					tParam += Time.deltaTime * Speed;
					valToBeLerped = Mathf.Lerp(currentVal,45,tParam);

					flashlight.range = 25;
					flashlight.spotAngle = valToBeLerped;
				}
			}
		}
	}
}