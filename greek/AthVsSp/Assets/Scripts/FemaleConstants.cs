using UnityEngine;
using System.Collections;

public class FemaleConstants 
{	
	static public void Equip(GameObject obj, Item[] items){
		bool needUnderwear = true;
		bool needHair = true;
		
		GameObject leather = null;
		GameObject main = null;
		//GameObject warrior = null;
		GameObject priest = null;
		
		Transform[] trans = obj.transform.GetComponentsInChildren<Transform>();
		foreach (Transform tran in trans)
		{
			if (tran.name == "female leather set 1")
			{
				leather = tran.gameObject;
			}
			else if (tran.name == "female_base")
			{
				main = tran.gameObject;
			}
			else if (tran.name == "female sorc set 1")
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
					simpleEquip("female spear leather set 1", leather);
				}
				else if (item.sets == "Sorcerer set")
				{
					simpleEquip("female sorcerer set 1 staff", priest);
				}
			}
			else if (item.type == "shield")
			{
				if (item.sets == "Leather set")
				{
					simpleEquip("Leather set 1 Shield female", leather);
				}
				else if (item.sets == "Sorcerer set")
				{
					simpleEquip("female sorcerer set 1 shield", priest);
				}
			}
			else if (item.type == "helmet")
			{
				if (item.sets == "Leather set")
				{
					simpleEquip("Leather set 1 Helmet female", leather);
				}
				else if (item.sets == "Sorcerer set")
				{
					simpleEquip("female sorcerer set 1 circlet", priest);
				}
				needHair = false;
			}
			else if (item.type == "armor")
			{
				if (item.sets == "Leather set")
				{
					simpleEquip("Leather set 1 female armour", leather);
				}
				else if (item.sets == "Sorcerer set")
				{
					simpleEquip("femle sorcerer set 2 armour", priest);
				}
				needUnderwear = false;
			}
			else if (item.type == "pauldrons")
			{
				if (item.sets == "Leather set")
				{
					simpleEquip("female Pauldrons leather set 1", leather);
				}
				else if (item.sets == "Sorcerer set")
				{
					simpleEquip("female sorcerer set 2 pauldrons", priest);
				}
			}
			else if (item.type == "gloves")
			{
				if (item.sets == "Leather set")
				{
					simpleEquip("female glove left Leather set 1", leather);
					simpleEquip("female glove right leather set 1", leather);
				}

				else if (item.sets == "Sorcerer set")
				{
					simpleEquip("femle sorcerer set 2 Glove left", priest);
					simpleEquip("femle sorcerer set 2 Glove right", priest);
				}
			}
			else if (item.type == "cuisses")
			{
				if (item.sets == "Leather set")
				{
					simpleEquip("female cuisses leather set 1", leather);
				}
				else if (item.sets == "Sorcerer set")
				{
					simpleEquip("femle sorcerer set 1 cuisses", priest);
				}
			}
		}
		if (needUnderwear)
		{
			simpleEquip("Female defaultwear", leather);
			Debug.Log ("underwear");
		}
		if (needHair)
		{
			simpleEquip("hair ponytail", main);
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
