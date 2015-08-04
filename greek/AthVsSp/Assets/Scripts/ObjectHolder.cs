using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectHolder : MonoBehaviour 
{
	public RectTransform leftMana;
	public RectTransform leftHealth;
	public RectTransform rightMana;
	public RectTransform rightHealth;
	public Text leftName;
	public Text rightName;
	public Text leftHealthText;
	public Text rightHealthText;
	public Text leftMagicText;
	public Text rightMagicText;

	// prefabs
	public GameObject male;
	public Button actionButton;

	// UI elements
	public Text statusLabel;

	public ButtonIcon normalHit = null;
	public ButtonIcon[] activeSkills = new ButtonIcon[0];
	public ButtonIcon[] auraSkills = new ButtonIcon[0];
	public ButtonIcon[] belt = new ButtonIcon[0];

	public Canvas canvas;
	public Text timer;
	public Camera camera;

	// Helpers
	public ButtonHelper bHelper = new ButtonHelper();
}

