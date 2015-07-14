using UnityEngine;
using System.Collections;

abstract public class BaseAnimation {
			
	abstract public Vector3 StartPosition();
	abstract public Vector3 EnemyStartPosition();

	abstract public Vector3 HitPosition();
	abstract public Vector3 EnemyBlockPosition();
	
	abstract public Vector3 BlockPosition();
	abstract public Vector3 EnemyHitPosition();

}
