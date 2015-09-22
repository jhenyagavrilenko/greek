using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonIcon
{
	private static readonly string namePrefab = "ButtonIcon/ButtonObject"; 
	private Image backgroundCircl;
	private Button button;
	private GameObject uiButton;
	private string name;

	public ButtonIcon(Canvas parent, Action action)
	{
		this.uiButton = GameObject.Instantiate(Resources.Load<GameObject>(namePrefab));
		this.backgroundCircl = uiButton.GetComponentInChildren<Image>();
		this.button = uiButton.GetComponentInChildren<Button>();
		this.name = action.name;
		changeButton (action);
		uiButton.transform.SetParent (parent.transform,false);
		uiButton.AddComponent<ActionButton> ();
		button.onClick.AddListener(() => uiButton.GetComponent<ActionButton>().ButtonPressed(action));

	}

	private void changeButton(Action action)
	{
		button.GetComponentInChildren<Text>().text = action.name;
		backgroundCircl.sprite = (Sprite)Resources.Load ("ButtonIcon/CirclIcons/" + action.id, typeof(Sprite));
	}

	public Button getButton()
	{
		return button;
	}

	public string getName()
	{
		return name;
	}

	public Vector3 getObjectPosition()
	{
		return uiButton.transform.position;
	}

	public void setPosition(Vector3 position)
	{
		uiButton.transform.position = position;
	}

	public void setIconInvisible(bool invisible)
	{
		backgroundCircl.enabled = invisible;
	}
}
