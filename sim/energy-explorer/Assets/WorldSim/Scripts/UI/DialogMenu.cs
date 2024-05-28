using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogMenu : MonoBehaviour
{
	protected List<Button> button_options = new List<Button>();
	protected Button button_confirm;

	public float delay_from_start = 0f;
	public float scale = 1f;
	public float animation_speed = 1f;

	protected bool confirm_enabled = true;

	protected int chosen_option = -1;

	protected virtual void Awake()
	{
		transform.Find("Background").GetComponent<Image>().color = new Color(1, 1, 1, 0.6f);
	}

	protected virtual void Start()
	{
		// Set up buttons
		int button_index = 1;
		while(true)
		{
			var option = transform.Find("Option" + button_index);
			if (option == default) break;
			button_options.Add(option.GetComponent<Button>());
			++button_index;
		}

		var i = 0;
		button_options.ForEach(button_option =>
		{
			var j = i++;
			button_option.onClick.AddListener(() => handle_click_option(j));
		});

		button_confirm = transform.Find("Confirm").GetComponent<Button>();
		button_confirm.onClick.AddListener(handle_click_confirm);
		button_confirm.interactable = false;
		if (!confirm_enabled) button_confirm.gameObject.SetActive(false);

		update_dialog();
	}

	protected virtual void OnEnable() { }

	protected virtual void OnDisable() { }

	protected virtual void Update()
	{
		delay_from_start -= Time.deltaTime;
		if (delay_from_start < 0)
		{
			scale = Mathf.Clamp01(scale + (animation_speed * Time.deltaTime));
			update_dialog();
		}
	}

	private void update_dialog()
	{
		transform.localScale = (Vector3.one * scale);
	}

	protected virtual void handle_click_option(int option)
	{
		chosen_option = option;

		var i = 0;
		button_options.ForEach(button_option =>
		{
			if (i == option) Colours.SetColour(button_option, Colours.button_selected);
			else Colours.SetColour(button_option, Colours.button_unselected);
			++i;
		});

		button_confirm.interactable = true;
	}

	protected virtual void handle_click_confirm()
	{
	}
}
