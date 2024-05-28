using UnityEngine;

public class WindTurbine : MonoBehaviour
{
	public float speed = 3;
	Transform nacelle;
	Transform hub;
	// Start is called before the first frame update
	void Start()
    {
		var upright = transform.Find("StandUpright");
		nacelle = upright.Find("Nacelle");
		hub = nacelle.Find("Hub");
	}

	// Update is called once per frame
	void Update()
    {
		var rn = nacelle.localEulerAngles;
		nacelle.localEulerAngles = new Vector3(0, 0, rn.z + 10f * Time.deltaTime);

		var rh = hub.localEulerAngles;
		hub.localEulerAngles = new Vector3(0, rh.y + 360f * Time.deltaTime * speed, 0);
	}
}
