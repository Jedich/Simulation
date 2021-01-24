using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CarPreferences : MonoBehaviour {
	private StreamReader read;
	public static Dictionary<string, float> map = new Dictionary<string, float>();
	public void Start() {
		read = File.OpenText(Application.dataPath + "/baseSettings.ini");
		string allLines = read.ReadToEnd();
		read.Close();
		string[] currLine = allLines.Trim().Split('\n');
		for (int i = 0; i < currLine.Length; i++)
			if (currLine[i] != "<end>")
				map.Add(currLine[i].Substring(0, currLine[i].IndexOf("=")), float.Parse(currLine[i].Substring(currLine[i].IndexOf("=") + 1)));
	}
}