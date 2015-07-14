﻿using UnityEngine;
using System.Collections;

public class User
{
	public enum SEX{MALE, FEMALE};

	public string SET_LEATHER = "";

	public bool isMonster;
	public string name = "";
	public int id = -1;

	public int health = 1;
	public int maxHealth = 1;
	public int mana = 1;
	public int maxMana = 1;

	public int side = 0;

	public SEX sex = SEX.MALE;

	public string set = "";

	public User()
	{
		isMonster = false;
	}

	public User(bool monster)
	{
		isMonster = monster;
	}

	public Item[] items;

	public bool needHair = true;
	public bool needUnderwear = true;
}

public class Item
{
	public string id;
	public string sets;
	public string type;
}