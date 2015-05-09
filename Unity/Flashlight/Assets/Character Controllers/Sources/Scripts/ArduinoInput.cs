using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class ArduinoInput : MonoBehaviour {

	SerialPort stream = new SerialPort("COM4", 38400);
	public string value;
	public string[] vec;

	// Use this for initialization
	void Start () {
		stream.Open ();
	}

	// Update is called once per frame
	void Update () {
		value = stream.ReadLine ();
		vec = value.Split (',');

		for (int i = 0; i <= 10; i++) {
			if (vec [i] != "") {
			} else {
				Debug.LogError("One or more value was not passed from Arduino to Unity");
			}
		}

		print ("Ax: " + vec[0] + "  " + "Ay: " + vec[1] + "  " + "Az: " + vec[2] + "  " +
		       "Gx: " + vec[3] + "  " + "Gy: " + vec[4] + "  " + "Gz: " + vec[5] + "  " +
		       "Mx: " + vec[6] + "  " + "My: " + vec[7] + "  " + "Mz: " + vec[8] + "  " +
		       "Butt: "+ vec[9] + "  " + "Pot: " + vec[10]);

	}
}