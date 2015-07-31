using UnityEngine;
using System.Collections;

public class Action 
{
	public string id = "";
	public string type = "";
	public string name = "";
	public string mana = "";
	public string itemId = "";
}

public class FightInfo 
{
	public int attackerId = 0;
	public int defenderId = 0;
	public bool block = false;
	public bool critical = false;
	public bool stun = false;
	public int hitPoints = 0;

	public string type = "";
	public string param = "";

	public static bool operator ==(FightInfo a, FightInfo b) {
		Debug.Log ("comp");
		return compare(a, b);
	}

	public static bool operator !=(FightInfo a, FightInfo b) {
		Debug.Log ("not comp");
		return !compare(a, b);
	}

	public override bool Equals(System.Object obj)
	{
		FightInfo p = obj as FightInfo;
		if ((object)p == null)
		{
			return false;
		}
		
		return base.Equals(obj) && compare(this, p);
	}
	
	public bool Equals(FightInfo p)
	{
		return base.Equals((FightInfo)p) && compare(this, p);
	}
	
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
	
	private static bool compare(FightInfo a, FightInfo b) {
		if (System.Object.ReferenceEquals(a, b)) {
			return true;
		}
		if (((object)a == null) || ((object)b == null)) {
			return false;
		}
		if (a.attackerId != b.attackerId || a.defenderId != b.defenderId) {
			return false;
		}
		if (a.block != b.block || a.critical != b.critical || a.stun != b.stun || a.hitPoints != b.hitPoints) {
			return false;
		}
		if (a.type != b.type || a.param != b.param) {
			return false;
		}
		return true;
	}
}