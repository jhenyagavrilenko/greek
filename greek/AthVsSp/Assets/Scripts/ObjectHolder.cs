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

	public Button normalHit = null;
	public Button[] activeSkills = new Button[0];
	public Button[] auraSkills = new Button[0];
	public Button[] belt = new Button[0];

	public Canvas canvas;

	public Text timer;

	public Camera camera;

	// Helpers
	public ButtonHelper bHelper = new ButtonHelper();
}
