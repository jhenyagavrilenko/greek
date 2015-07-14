using UnityEngine;
using System.Collections;

public class Action {

	public string id = "";
	public string type = "";

	public string name = "";
	public string mana = "";

	public string itemId = "";
}

public class FightInfo {

	public bool block = false;
	public bool critical = false;
	public bool stun = false;
	public string hitPoints = "";
}