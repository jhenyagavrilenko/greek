using UnityEngine;
using System.Collections;

abstract public class BaseAnimation {
		

	abstract public Vector3 Start();
	abstract public Vector3 EStart();
	abstract public Vector3 CStart();

	abstract public Vector3 HitBlock();
	abstract public Vector3 EHitBlock();
	abstract public Vector3 CHitBlock();
	
	abstract public Vector3 BlockHit();
	abstract public Vector3 EBlockHit();
	abstract public Vector3 CBlockHit();

	abstract public Vector3 HitSuc();
	abstract public Vector3 EHitSuc();
	abstract public Vector3 CHitSuc();

	abstract public Vector3 SucHit();
	abstract public Vector3 ESucHit();
	abstract public Vector3 CSucHit();

	abstract public Vector3 HitDeath();
	abstract public Vector3 EHitDeath();
	abstract public Vector3 CHitDeath();

	public Vector3 GetCameraPosition(bool attacking, string attack, bool block)
	{
		if (attacking)
		{
			if (attack == "hit")
			{
				if (block)
					return CHitBlock();
				else 
					return CHitSuc();
			}
		}
		else
		{
			if (attack == "hit")
			{
				if (block)
					return CBlockHit();
				else 
					return CSucHit();
			}
		}
		return CStart();
	}

	public Vector3 GetPosition(bool attacking, string attack, bool block)
	{
		if (attacking)
		{
			if (attack == "hit")
			{
				if (block)
					return HitBlock();
				else 
					return HitSuc();
			}
		}
		else
		{
			if (attack == "hit")
			{
				if (block)
					return BlockHit();
				else 
					return SucHit();
			}
		}
		return Start();
	}

	public Vector3 GetEnemyPosition(bool attacking, string attack, bool block)
	{
		if (attacking)
		{
			if (attack == "hit")
			{
				if (block)
					return EHitBlock();
				else 
					return EHitSuc();
			}
		}
		else
		{
			if (attack == "hit")
			{
				if (block)
					return EBlockHit();
				else 
					return ESucHit();
			}
		}
		return EStart();
	}
}
