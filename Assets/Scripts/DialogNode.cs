using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DialogNode {

	public string name;
	public string message;
	public Dictionary<string, DialogNode> neighbourList;

	public DialogNode()
	{
		name = "";
		message = "";
		neighbourList = new Dictionary<string, DialogNode>();
	}

	public DialogNode(string nn, string mm)
	{
		name = nn;
		mm = mm.Replace("]]","");
		mm = mm.Replace("\n", "");
		string[] splitted = Regex.Split(mm,"\\[\\[");
		message = splitted[0];
		neighbourList = new Dictionary<string, DialogNode>();
		//Debug.Log("NAME: "+name);
		//Debug.Log("MESSAGE: "+message);
		for (int i = 1; i < splitted.Length; i++ )
		{
			neighbourList.Add(splitted[i],new DialogNode());
			Debug.Log("OPTION: " + splitted[i]);
		}
	}
}
