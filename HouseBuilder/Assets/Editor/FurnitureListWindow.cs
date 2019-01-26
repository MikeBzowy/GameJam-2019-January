using UnityEngine;
using UnityEditor;
using System.IO;

public class FurnitureListWindow : EditorWindow
{
	string myColor = "";
	string myType = "";
	string mySet = "";

	string colorLabel;
	string typeLabel;
	string setLabel;

	bool groupEnabled;
	bool myBool = true;
	float myFloat = 1.23f;

	// Add menu named "My Window" to the Window menu
	[MenuItem("Furniture/Add Values")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		FurnitureListWindow window = (FurnitureListWindow)EditorWindow.GetWindow(typeof(FurnitureListWindow));
		window.Show();
	}

	void OnGUI()
	{
		myColor = EditorGUILayout.TextField("Color", myColor);
		if(GUILayout.Button("Add Color"))
		{
			if (myColor != "") {
				addColor ();
				colorLabel = "Added " + myColor;
				myColor = "";
				GUIUtility.keyboardControl = 0;
			}
		}

		EditorGUILayout.LabelField (colorLabel);

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		myType = EditorGUILayout.TextField("Object Type", myType);

		if(GUILayout.Button("Add Type"))
		{
			if (myType != "") {
				addType ();
				typeLabel = "Added " + myType;
				myType = "";
				GUIUtility.keyboardControl = 0;
			}
		}

		EditorGUILayout.LabelField (typeLabel);

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		mySet = EditorGUILayout.TextField("Set", mySet);

		if(GUILayout.Button("Add Set"))
		{
			if (mySet != "") {
				addSet ();
				colorLabel = "Added " + mySet;
				mySet = "";
				GUIUtility.keyboardControl = 0;
			}
		}

		EditorGUILayout.LabelField (setLabel);


	}

	void addColor() {
		string path = "Assets/Resources/values/colors.txt";

		using (StreamWriter sw = File.AppendText(path)) 
		{
			sw.WriteLine(myColor);
		}	
	}

	void addType() {
		string path = "Assets/Resources/values/Type.txt";

		using (StreamWriter sw = File.AppendText(path)) 
		{
			sw.WriteLine(myType);
		}	
	}

	void addSet() {
		string path = "Assets/Resources/values/sets.txt";

		using (StreamWriter sw = File.AppendText(path)) 
		{
			sw.WriteLine(mySet);
		}	
	}
}