using UnityEngine;
using System.Collections;

public class MaleAnimation: BaseAnimation {
	
	override public Vector3 Start()
	{
		return new Vector3(1.5f, 0.0f, 0.0f);
	}

	override public Vector3 EStart()
	{
		return new Vector3(-1.5f, 0.0f, 0.0f);
	}

	override public Vector3 CStart()
	{
		return new Vector3(0.0f, 1.0f, 15.0f);
	}



	override public Vector3 HitBlock()
	{
		return new Vector3(0.64f, 0.0f, 0.0f);
	}
	
	override public Vector3 EHitBlock()
	{
		return new Vector3(-0.64f, 0.0f, 0.15f);
	}
	
	override public Vector3 CHitBlock()
	{
		return new Vector3(0.0f, 1.2f, 13.0f);
	}
	
	
	
	override public Vector3 BlockHit()
	{
		return new Vector3(-3.5f, 1.9f, 0.0f);
	}
	
	override public Vector3 EBlockHit()
	{
		return new Vector3(-3.5f, 1.9f, 0.0f);
	}
	
	override public Vector3 CBlockHit()
	{
		return new Vector3(-3.5f, 1.9f, 0.0f);
	}


	override public Vector3 HitSuc()
	{
		return new Vector3(0.64f, 0.0f, 0.0f);
	}

	override public Vector3 EHitSuc()
	{
		return new Vector3(-0.64f, 0.0f, 0.15f);
	}

	override public Vector3 CHitSuc()
	{
		return new Vector3(0.0f, 1.2f, 10.0f);
	}



	override public Vector3 SucHit()
	{
		return new Vector3(-3.5f, 1.9f, 0.0f);
	}

	override public Vector3 ESucHit()
	{
		return new Vector3(-3.5f, 1.9f, 0.0f);
	}

	override public Vector3 CSucHit()
	{
		return new Vector3(-3.5f, 1.9f, 0.0f);
	}
}
