using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectHolder : MonoBehaviour {
		
	// prefabs
	public GameObject male;
	public Button actionButton;

	// UI elements
	public Text statusLabel;
	
	public Text leftName;
	public Text rightName;

	public GameObject panel;
	public Slider leftMana;
	public Slider leftHealth;
	public Slider rightMana;
	public Slider rightHealth;

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
