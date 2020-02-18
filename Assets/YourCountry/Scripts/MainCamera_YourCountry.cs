using UnityEngine;

/*
public class MainCamera_YourCountry : MonoBehaviour
{
	public Vector3 get_position_of_screen_centre ()
	{
		RaycastHit hit;
		Vector3 centre_of_screen = new Vector3(Screen.width / 2f, Screen.height / 2f);
		Ray ray = GetComponent<Camera>().ScreenPointToRay(centre_of_screen);

		if (Physics.Raycast(ray, out hit))
		{
			//Transform objectHit = hit.transform;
			return hit.point;
		}
		return default(Vector3);
	}

	public Vector3 get_position_of_mouse()
	{
		RaycastHit hit;
		Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

		//int layer_mask = 1 << 8; // only for terrain but not working at the moment
		//// TOOD fix this

		//if (Physics.Raycast(ray, out hit, layer_mask))
		//{
		//	//Transform objectHit = hit.transform;
		//	Debug.Log("hit: " + hit.transform + " layer: " + hit.transform.gameObject.layer);
		//	return hit.point;
		//}

		var hits = Physics.RaycastAll(ray);
		if (hits.Length > 0)
		{
			for (int i = 0; i < hits.Length; ++i)
			{
				hit = hits[i];
				if (hit.transform.gameObject.layer == 8)
				{
					return hit.point;
				}
			}
		}

		return default;
	}

	[Range(1, 100)]
	public const float max_camera_move_speed = 60f;
	private const float min_camera_move_speed = max_camera_move_speed / 3f;
	private const float camera_max_acceleration = 2f;
	private float current_camera_move_speed = 0;

	private Vector3 camera_target_position;
	private Vector2 mouse_down_at;
	private Vector3 camera_location_at_scroll_start;

	void Awake()
	{
		camera_target_position = transform.localPosition;
		set_target_altitude(40);
	}

	void Update()
	{
		// Zoom camera
		float wheel = Input.GetAxis("Mouse ScrollWheel");
		if (wheel != 0)
		{
			set_target_altitude(camera_target_position.z + wheel * -5f * get_altitude());
		}

		if (transform.localPosition != camera_target_position)
		{
			var diff = Vector3.Distance(transform.localPosition, camera_target_position);
			var accel = (diff > 10 ? 1 : -1) * camera_max_acceleration;
			current_camera_move_speed += accel;
			current_camera_move_speed = Mathf.Clamp(current_camera_move_speed, min_camera_move_speed, max_camera_move_speed);
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, camera_target_position, current_camera_move_speed * Time.deltaTime);

			GameManagerYourCountry.CameraAltitudeChangedInvoke(this, transform.localPosition.z);
		}
		else
		{
			current_camera_move_speed = 0;
		}

		// Move camera
		if (mouse_down_at == default && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2)))
		{
			mouse_down_at = get_mouse_position();
			camera_location_at_scroll_start = transform.parent.position;
		}

		if (mouse_down_at != default)
		{
			var diff = (get_mouse_position() - mouse_down_at) * get_altitude();
			var x_factor = ((float)Screen.width / (float)Screen.height) * 1.3f;
			var y_factor = ((float)Screen.height / (float)Screen.width) * 2f;
			Vector3 new_location = new Vector3(camera_location_at_scroll_start.x - (diff.x * x_factor), 0, camera_location_at_scroll_start.z + (diff.y * y_factor));
			transform.parent.position = new_location;

			if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(2)) mouse_down_at = default;
		}
	}

	private void set_target_altitude (float altitude)
	{
		camera_target_position.z = Mathf.Clamp(altitude, GameManagerYourCountry.min_altitude, GameManagerYourCountry.max_altitude);
	}

	private float get_altitude()
	{
		return transform.localPosition.z;
	}

	private Vector2 get_mouse_position()
	{
		var p = Input.mousePosition;
		return new Vector2(p.x / Screen.width, 1f - p.y / Screen.height);
	}
}
*/