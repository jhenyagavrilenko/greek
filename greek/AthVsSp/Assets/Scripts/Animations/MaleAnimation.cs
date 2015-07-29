using UnityEngine;
using System.Collections;

public class MaleAnimation: BaseAnimation {

	override public Vector3 StartPosition()
	{
		return new Vector3(1.5f, 0.0f, 0.0f);
	}

	override public Vector3 EnemyStartPosition()
	{
		return new Vector3(-1.5f, 0.0f, 0.0f);
	}

	override public Vector3 CameraStartPosition()
	{
		return new Vector3(0.0f, 1.0f, 15.0f);
	}



	override public Vector3 HitPosition()
	{
		return new Vector3(0.64f, 0.0f, 0.0f);
	}

	override public Vector3 EnemyBlockPosition()
	{
		return new Vector3(-0.64f, 0.0f, 0.15f);
	}

	override public Vector3 CameraHitPosition()
	{
		return new Vector3(0.0f, 1.2f, 13.0f);
	}



	override public Vector3 BlockPosition()
	{
		return new Vector3(-3.5f, 1.9f, 0.0f);
	}

	override public Vector3 EnemyHitPosition()
	{
		return new Vector3(-3.5f, 1.9f, 0.0f);
	}

	override public Vector3 CameraBlockPosition()
	{
		return new Vector3(-3.5f, 1.9f, 0.0f);
	}
}
