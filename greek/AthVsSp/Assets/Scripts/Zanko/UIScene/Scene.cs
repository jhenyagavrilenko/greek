using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Scene : MonoBehaviour 
{
	private ActionPanel timer;
	private PlayerPanel left, right;
	private Image background;

	public Scene()
	{
		timer = new ActionPanel ();
		left = new PlayerPanel ();
		right = new PlayerPanel ();
	}
}
