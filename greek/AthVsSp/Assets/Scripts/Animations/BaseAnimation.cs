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
}
