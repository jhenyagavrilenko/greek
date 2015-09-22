using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionButton : MonoBehaviour {

	public void ButtonPressed(Action action)
	{
		Debug.Log(action.name);
		GameObject logic = GameObject.Find("Logic");
		Logic scr = (logic.GetComponent<Logic>() as Logic);
		scr.ButtonPostAction(action);
		Debug.Log (action.type);
	}
}
