using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class CarPreferences : MonoBehaviour {
	private StreamReader read;
	public static Preferences current = null;

	public void Awake() 
	{
		read = File.OpenText(Application.dataPath + "/baseSettings.ini");
		string allLines = read.ReadToEnd();
		read.Close();
		current = JsonUtility.FromJson<Preferences>(allLines);
		Debug.Log(current.sensors);
	}
}