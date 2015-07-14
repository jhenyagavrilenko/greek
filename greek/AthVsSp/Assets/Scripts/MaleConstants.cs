using UnityEngine;
using System.Collections;

public class MaleConstants {
	
	static public void Equip(GameObject obj, Item[] items)
	{
		bool needUnderwear = true;
		bool needHair = true;

		GameObject leather = null;
		GameObject main = null;
		GameObject warrior = null;
		GameObject priest = null;

		Transform[] trans = obj.transform.GetComponentsInChildren<Transform>();
		foreach (Transform tran in trans)
		{
			if (tran.name == "male_Leather_set_1")
			{
				leather = tran.gameObject;
			}
			else if (tran.name == "male_base")
			{
				main = tran.gameObject;
			}
			else if (tran.name == "male warrior set 1")
			{
				warrior = tran.gameObject;
			}
			else if (tran.name == "male_priest_set_1")
			{
				priest = tran.gameObject;
			}
		}

		foreach (Item item in items)
		{
			if (item.type == "weapon")
			{
				if (item.sets == "Leather set")
				{
					simpleEquip("Spear Leather set 1", leather);
				}
				else if (item.sets == "Warrior set")
				{
					simpleEquip("Warrior Athenean sword", warrior); // fix
				}
				else if (item.sets == "Priest set")
				{
					simpleEquip("Double axe priest set 1", priest);
				}
			}
			else if (item.type == "shield")
			{
				if (item.sets == "Leather set")
				{
					simpleEquip(" Shield Leather set 1", leather);
				}
				else if (item.sets == "Warrior set")
				{
					simpleEquip("Warrior Athenean shield", warrior);
				}
				else if (item.sets == "Priest set")
				{
					simpleEquip("Priest set 1 Shield", priest);
				}
			}
			else if (item.type == "helmet")
			{
				if (item.sets == "Leather set")
				{
					simpleEquip("Helmet Leather set 1", leather);
				}
				else if (item.sets == "Warrior set")
				{
					simpleEquip("Warrior helmet", warrior);
				}
				else if (item.sets == "Priest set")
				{
					simpleEquip("Priest_set_1_Helmet", priest);
				}
				needHair = false;
			}
			else if (item.type == "armor")
			{
				if (item.sets == "Leather set")
				{
					simpleEquip("Armour Leather set 1", leather);
				}
				else if (item.sets == "Warrior set")
				{
					simpleEquip("Warrior Armour set 1", warrior);
				}
				else if (item.sets == "Priest set")
				{
					simpleEquip("Priest set 1 Armour", priest);
				}
				needUnderwear = false;
			}
			else if (item.type == "pauldrons")
			{
				if (item.sets == "Leather set")
				{
					simpleEquip("Pauldron leather set 1", leather);
				}
				else if (item.sets == "Warrior set")
				{
					simpleEquip("warrior Pauldrons set 1 NO CAPE", warrior);
				}
				else if (item.sets == "Priest set")
				{
					simpleEquip("Pauldron Priest set 1 NO CAPE", priest);
				}
			}
			else if (item.type == "gloves")
			{
				if (item.sets == "Leather set")
				{
					simpleEquip("Glove Left leather set 1", leather);
					simpleEquip("Glove right leather set 1", leather);
				}
				else if (item.sets == "Warrior set")
				{
					simpleEquip("warrior set 1 Glove Left", warrior);
					simpleEquip("warrior set 1 Glove right", warrior);
				}
				else if (item.sets == "Priest set")
				{
					simpleEquip("Priest set 1 Glove left", priest);
					simpleEquip("Priest set 1 Glove right", priest);
				}
			}
			else if (item.type == "cuisses")
			{
				if (item.sets == "Leather set")
				{
					simpleEquip("CuissesLeatherset1", leather);
				}
				else if (item.sets == "Warrior set")
				{
					simpleEquip("warrior set 1 Cuisses", warrior);
				}
				else if (item.sets == "Priest set")
				{
					simpleEquip("Priest set 1 Cuisses", priest);
				}
			}
		}
		if (needUnderwear)
		{
			simpleEquip("DEFAULT underwear male", leather);
			Debug.Log ("underwear");
		}
		if (needHair)
		{
			simpleEquip("short_hair", main);
			Debug.Log ("hair");
		}
	}

	static private void simpleEquip(string item, GameObject obj)
	{
		Transform[] trans = obj.transform.GetComponentsInChildren<Transform>();
		foreach (Transform tran in trans)
		{
			if (tran.name == item)
			{
				Renderer rend = tran.gameObject.GetComponent<Renderer>();
				rend.enabled = true;
				return;
			}
		}
		Debug.Log (item);
		Debug.Log (obj);
		Debug.Log("NOT++++++++++++++++");
	}
}
