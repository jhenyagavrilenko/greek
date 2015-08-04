using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonIcon 
{
	private static readonly string namePrefab = "UIButton"; 
	private Image backgroundCircl;
	private Button button;
	private GameObject uiButton;
	private string name;

	public ButtonIcon(Canvas parent, string name)
	{
		this.uiButton = GameObject.Instantiate(Resources.Load<GameObject>(namePrefab));
		this.backgroundCircl = uiButton.GetComponentInChildren<Image>();
		this.button = uiButton.GetComponentInChildren<Button>();
		this.name = name;
		changeButton (name);
		uiButton.transform.SetParent (parent.transform,false);

	}

	private void changeButton(string name)
	{
		button.GetComponentInChildren<Text>().text = name;
		backgroundCircl.sprite = (Sprite)Resources.Load ("IconButton/" + name, typeof(Sprite));
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
