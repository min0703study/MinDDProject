using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditor.Callbacks;

public class DialogueEditor : EditorWindow
{
	Dialogue dialogue = null;
	
	[MenuItem("Window/Dialogue Editor")]
	public static void ShowEditorWindow()
	{
		GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
	}

	[OnOpenAssetAttribute(1)]	
	public static bool OnOpenAsset(int instanceID, int line)
	{
		var dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
		if(dialogue != null)
		{
			return true;
		}
		return false;
	}
	
	private void OnGUI() {
		if(dialogue == null)
		{
		}
		EditorGUI.LabelField(new Rect(10, 10, 200, 200), "Hello World");
	}
}
