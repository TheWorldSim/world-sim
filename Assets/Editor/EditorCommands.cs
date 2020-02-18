using UnityEditor;
using UnityEngine;

public static class EditorCommands
{
    [MenuItem("Commands/Toggle UI Layer")]
    public static void ToggleUILayers()
    {
        var uiLayer = LayerMask.GetMask("UI");

        if (Tools.visibleLayers != uiLayer)
            Tools.visibleLayers = uiLayer;
        else
            Tools.visibleLayers = ~uiLayer;
    }
}
