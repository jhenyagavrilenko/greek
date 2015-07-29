using UnityEngine;
using System.Collections;

public class AnimationHelper
{
	static public void SetPosition(GameObject obj, Vector3 pos)
	{
		Vector3 position = obj.transform.position;
		position.x = pos.x;
		position.y = pos.y;
		position.z = pos.z;
		obj.transform.position = position;
	}

	static public void SetRotation(GameObject obj, Vector3 rot)
	{
		obj.transform.Rotate(rot.x, rot.y, rot.z, Space.World); // 180.0f
	}

	public static void MoveGO(GameObject TempGO, Vector3 StartPos, Vector3 EndPos, float length)
	{
		AnimationCurve curve1 = null, curve2 = null, curve3 = null;
		curve1 = AnimationCurve.EaseInOut(0, StartPos.x, length, EndPos.x);
		curve2 = AnimationCurve.EaseInOut(0, StartPos.y, length, EndPos.y);
		curve3 = AnimationCurve.EaseInOut(0, StartPos.z, length, EndPos.z);

		AnimationClip clip = new AnimationClip();
		clip.SetCurve("", typeof(Transform), "localPosition.x", curve1);
		clip.SetCurve("", typeof(Transform), "localPosition.y", curve2);
		clip.SetCurve("", typeof(Transform), "localPosition.z", curve3);
		clip.legacy = true;

		if (TempGO.GetComponent<Animation>() == null)
		{
			TempGO.AddComponent<Animation>();
		}

		string clipName = "MoveAnimation";

		Animation animation = TempGO.GetComponent<Animation>();
		if (animation.IsPlaying(clipName))
		{
			//TempGO.animation["AnimationDemo"].time = 0.5f ;
			animation.Sample();
			animation.RemoveClip(clipName);
		}
		
		animation.AddClip(clip, clipName);
		animation["MoveAnimation"].speed = 1.0F;
		animation.Play("MoveAnimation");
		//TempGO.animation.wrapMode=WrapMode.PingPong;
	}
}
