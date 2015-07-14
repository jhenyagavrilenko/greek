using UnityEngine;
using System.Collections;

public class MaleAnimation: BaseAnimation {

	override public Vector3 StartPosition()
	{
		return new Vector3(0.0f, 0.0f, 1.5f);
	}

	override public Vector3 EnemyStartPosition()
	{
		return new Vector3(0.0f, 0.0f, -1.5f);
	}
	
	override public Vector3 HitPosition()
	{
		return new Vector3(-3.5f, 1.9f, 0.0f);
	}

	override public Vector3 EnemyBlockPosition()
	{
		return new Vector3(-3.5f, 1.9f, 0.0f);
	}
	
	override public Vector3 BlockPosition()
	{
		return new Vector3(-3.5f, 1.9f, 0.0f);
	}

	override public Vector3 EnemyHitPosition()
	{
		return new Vector3(-3.5f, 1.9f, 0.0f);
	}
}
