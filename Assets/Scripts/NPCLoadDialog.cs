using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class NPCLoadDialog : MonoBehaviour 
{
	public string twineFileName;


	private Dictionary<string, DialogNode> dialog = new Dictionary<string, DialogNode>();
	void Start () 
	{
		string path = System.IO.Path.Combine(Application.streamingAssetsPath, twineFileName);
		string lines;
		using (StreamReader sr = new StreamReader(path))
		{
			lines = sr.ReadToEnd();
		}
		int index = lines.IndexOf("<tw-storydata");
		int index2 = lines.IndexOf("</tw-storydata>");
		lines = lines.Substring(index, index2 - index + "</tw-storydata>".Length);
		int index3 = lines.IndexOf("hidden");
		lines = lines.Remove(index3, "hidden".Length);
		Debug.Log(lines);

		XmlReader reader = XmlReader.Create(new StringReader(lines));
		bool isPassageNode = false;
		string nodeName = "";
		while (reader.Read())
		{
			if (reader.Name == "tw-passagedata" && reader.NodeType == XmlNodeType.Element)
			{
				nodeName = reader.GetAttribute("name").Trim();
				isPassageNode = true;
			}
			else if(isPassageNode)
			{
				dialog.Add(nodeName, new DialogNode(nodeName, reader.Value));
				isPassageNode = false;
				nodeName = "";
			}

			/*switch (reader.NodeType)
			{
				case XmlNodeType.Element: // The node is an element.
					Debug.Log("<" + reader.Name + ">");
					break;
				case XmlNodeType.Text: //Display the text in each element.
					Debug.Log(reader.Value);
					break;
				case XmlNodeType.EndElement: //Display the end of the element.
					Debug.Log("</" + reader.Name + ">");
					break;
			}*/
		}
		reader.Close();
		ConnectNodes();
	}

	private void ConnectNodes()
	{
		List<string> nodes = new List<string>(dialog.Keys);
		foreach (string node in nodes)
		{
			List<string> neighbours = new List<string>(dialog[node].neighbourList.Keys);
			foreach(string neighbour in neighbours)
			{
				dialog[node].neighbourList[neighbour] = dialog[neighbour];
			}
		}

		foreach (var node in dialog.Values)
		{
			Debug.Log(node.name + ": "+ node.message);
			foreach (KeyValuePair<string, DialogNode> neighbour in node.neighbourList)
			{
				Debug.Log("LINK: "+neighbour.Key+" ->"+neighbour.Value.message);
			}
		}
	}
	
}
