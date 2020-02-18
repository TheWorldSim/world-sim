using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/*
[CustomEditor(typeof(RenderCountry))]
public class CountryEditor : Editor
{
	RenderCountry render_country;

	public override void OnInspectorGUI()
	{
		using (var check = new EditorGUI.ChangeCheckScope())
		{
			base.OnInspectorGUI();

			if (check.changed)
			{
				render_country.refresh();
			}
		}

		if (GUILayout.Button("Generate Country"))
		{
			render_country.refresh();
		}
	}

	void draw_settings_editor(Object settings, System.Action on_settings_updated, ref bool foldout, ref Editor editor)
	{
		if (settings == null) return;
		foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
		if (!foldout) return;

		using (var check = new EditorGUI.ChangeCheckScope())
		{
			CreateCachedEditor(settings, null, ref editor);
			editor.OnInspectorGUI();

			if (check.changed)
			{
				on_settings_updated?.Invoke();
			}
		}
	}

	private void OnEnable()
	{
		render_country = (RenderCountry)target;
	}
}
*/