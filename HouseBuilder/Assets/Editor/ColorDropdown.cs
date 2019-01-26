using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(Furniture))]
public class EditorGUIEnumPopup : Editor
{

	Furniture test;
	public int colorIndex = 0;
	public string[] colors;
	public int typeIndex = 0;
	public string[] types;
	public int setIndex = 0;
	public string[] sets;

	public void OnEnable(){
		test = (Furniture)target;
		if(colors == null)
			ReadStrings ();

		colorIndex = System.Array.IndexOf(colors,test.myColor);
		if (colorIndex < 0)
			colorIndex = 0;

		typeIndex = System.Array.IndexOf(types,test.myType);
		if (typeIndex < 0)
			typeIndex = 0;

		setIndex = System.Array.IndexOf(sets,test.mySet);
		if (setIndex < 0)
			setIndex = 0;
	}

	void ReadStrings()
	{
		readColors ();
		readTypes ();
		readSets ();
	}

	void readColors() {
		string path = "Assets/Resources/values/colors.txt";

		//Read the text from directly from the test.txt file
		StreamReader reader = new StreamReader(path); 
		colors = reader.ReadToEnd ().Split(new string[] {"\n"}, System.StringSplitOptions.None);
		reader.Close();
	}

	void readTypes() {
		string path = "Assets/Resources/values/Type.txt";

		//Read the text from directly from the test.txt file
		StreamReader reader = new StreamReader(path); 
		types = reader.ReadToEnd ().Split(new string[] {"\n"}, System.StringSplitOptions.None);
		reader.Close();
	}

	void readSets() {
		string path = "Assets/Resources/values/sets.txt";

		//Read the text from directly from the test.txt file
		StreamReader reader = new StreamReader(path); 
		sets = reader.ReadToEnd ().Split(new string[] {"\n"}, System.StringSplitOptions.None);
		reader.Close();
	}

	public override void OnInspectorGUI()
	{
		if(colors == null)
			ReadStrings ();
		EditorGUILayout.LabelField ("Color:");

		colorIndex = EditorGUILayout.Popup(colorIndex, colors);
		test.myColor = colors[colorIndex];

		EditorGUILayout.LabelField ("Object Type:");
		typeIndex = EditorGUILayout.Popup(typeIndex, types);
		test.myType = types[typeIndex];

		EditorGUILayout.LabelField ("Set:");
		setIndex = EditorGUILayout.Popup(setIndex, sets);
		test.mySet = sets[setIndex];

		EditorGUILayout.LabelField ("Points:");
		test.points = EditorGUILayout.FloatField (test.points);
	}
}
