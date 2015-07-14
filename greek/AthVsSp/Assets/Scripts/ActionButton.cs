using UnityEngine;
using System.Collections;

public class ActionButton : MonoBehaviour {

	public Action action = null;

	public void ButtonPressed()
	{
		Debug.Log("button pressed");

		Debug.Log(action);
		GameObject logic = GameObject.Find("Logic");
		Logic scr = (logic.GetComponent<Logic>() as Logic);
		scr.ButtonPressed(action);
	}
}
