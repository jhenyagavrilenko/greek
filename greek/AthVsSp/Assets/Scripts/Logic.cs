using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System;
using UnityEngine.UI;
using CielaSpike;

public class Logic : MonoBehaviour
{
	private int REQUEST_SYNC = 0;
	private int REQUEST_ACTION = 1;
	private int REQUEST_EQUIP = 2;

	private int _battleId = 0;
	private string _userName = null;
	private int _opponentId;

	private IEnumerator coroutine;
	
	GameObject leftPlayer = null;
	GameObject rightPlayer = null;
	User leftUser = null;
	User rightUser = null;

	public ObjectHolder objectHolder;

	public GameObject testMale;

	public bool playerTurn;
	public int playerSide;

	private BaseAnimation enemyAnimation = null;

	bool isAnimation = false;

	// Use this for initialization
	void Start()
	{
		CanvasGroup group = objectHolder.panel.GetComponent<CanvasGroup>();
		group.alpha = 0;

		SetBattle(17, "lennondtps@gmail.com"); //lennondtps@gmail.com  player@email.gr
	}

	public void SetUserName(string userName)
	{
		_userName = userName;
		if (_battleId > 0)
		{
			SetBattle(_battleId, _userName);
		}
	}

	public void SetBattleId(string battleId)
	{
		int.TryParse(battleId, out _battleId);
		if (_userName != null)
		{
			SetBattle(_battleId, _userName);
		}
	}

	public void SetBattle(int battleId, string userName)
	{
		_battleId = battleId;
		_userName = userName;
		_opponentId = -1;
		InvokeRepeating("SyncRequest", 1.0F, 1.0F);
	}
	
	void AddModels()
	{
		CanvasGroup group = objectHolder.panel.GetComponent<CanvasGroup>();
		group.alpha = 1;
		if (leftPlayer != null)
		{
			Destroy(leftPlayer);
		}
		if (rightPlayer != null)
		{
			Destroy(rightPlayer);
		}

		enemyAnimation = null;

		Vector3 enRot = new Vector3(0.0f, 180.0f, 0.0f);

		if (rightUser.isMonster == true)
		{
		}
		else
		{
			if (rightUser.sex == User.SEX.MALE)
			{
				enemyAnimation = new MaleAnimation();
				rightPlayer = Instantiate(objectHolder.male);
				AnimationHelper.SetPosition(rightPlayer, enemyAnimation.EnemyStartPosition());
				AnimationHelper.SetRotation(rightPlayer, enRot);
				MaleConstants.Equip(rightPlayer, rightUser.items);
			}
			else
			{

			}
		}

		if (enemyAnimation == null)
		{
			Debug.Log("ERROR enemyAnimation == null");
			enemyAnimation = new MaleAnimation();
		}

		if (leftUser.sex == User.SEX.MALE)
		{
			leftPlayer = Instantiate(objectHolder.male);
			AnimationHelper.SetPosition(leftPlayer, enemyAnimation.StartPosition());
			MaleConstants.Equip(leftPlayer, leftUser.items);
		}
		else
		{
			
		}

		AnimationHelper.SetPosition(objectHolder.camera.gameObject, enemyAnimation.CameraStartPosition());
	}

	void ApplyNewUIData()
	{
		objectHolder.leftMana.maxValue = leftUser.maxMana;
		objectHolder.leftHealth.maxValue = leftUser.maxHealth;
		objectHolder.rightMana.maxValue = rightUser.maxMana;
		objectHolder.rightHealth.maxValue = rightUser.maxHealth;

		objectHolder.leftMana.value = leftUser.mana;
		objectHolder.leftHealth.value = leftUser.health;
		objectHolder.rightMana.value = rightUser.mana;
		objectHolder.rightHealth.value = rightUser.health;

		objectHolder.leftName.text = leftUser.name;
		objectHolder.rightName.text = rightUser.name;
	}

	public void ButtonPressed(Action action)
	{
		Debug.Log(action.type);

		objectHolder.bHelper.SetState(objectHolder, "It is not your turn");

		string req = "http://api.gw.monospacelabs.com/v1/battle/action";
		
		Dictionary<string,string> dict = new Dictionary<string,string>();
		dict["email"] = _userName; //  
		string battleId = "";
		battleId = _battleId.ToString();
		dict["battle_id"] = battleId; // 16
		dict["type"] = action.type;
		dict["param"] = "normal";
		
		POST(req, dict, REQUEST_ACTION);
	}
	
	void SyncRequest()
	{
		Debug.Log("SyncRequest");
		if (isAnimation)
		{
			return;
		}
	
		string req = "http://api.gw.monospacelabs.com/v1/battle/sync";
		req += "?battle_id=" + _battleId + "&email=" + _userName;

		GET(req, REQUEST_SYNC);
	}

	IEnumerator ProcessSync(string text)
	{
		String status = "";
		bool exception = false;
		int timer = 0;

		Action[] belt = new Action[0];
		Action[] aurSkills = new Action[0];
		Action[] actSkills = new Action[0];

		try {
			JsonData data = JsonMapper.ToObject (text);
			Debug.Log ("WWW Sync Ok!: " + text);

			string result = (string)data["status"];
			if (result != "success") {
				throw new Exception();
			}

			JsonData gameData = data["data"];

			processSync(gameData, out status, out timer, out actSkills, out aurSkills, out belt);

		} catch (Exception e) {
			Debug.Log("WWW Exception!: " + e.Message);
			CloseApp();
			exception = true;
		}

		if (exception == false)
		{
			if (rightUser.id != _opponentId)
			{
				string req = "http://api.gw.monospacelabs.com/v1/battle/equipment";
				req += "?opponent_id=" + rightUser.id + "&email=" + _userName;
				
				GET(req, REQUEST_EQUIP);
				_opponentId = rightUser.id;
			}
			
			// UI Thread
			yield return Ninja.JumpToUnity;

			objectHolder.statusLabel.text = status;
			objectHolder.timer.text = timer.ToString();
			ApplyNewUIData();
			objectHolder.bHelper.AddButtons(actSkills, aurSkills, belt, objectHolder);
			objectHolder.bHelper.SetState(objectHolder, status);
			
			yield return Ninja.JumpBack;
		}

		yield return false;
	}
	
	void processSync(JsonData gameData, out string status, out int timer, out Action[] actSkills, out Action[] aurSkills, out Action[] belt)
	{
		if (leftUser == null)
		{
			leftUser = new User();
		}
		if (rightUser == null)
		{
			rightUser = new User();
		}
		
		status = (string)gameData["status"]["msg"];
		playerTurn = false;
		if (status == "It is your turn") {
			playerTurn = true;
		}
		
		JsonData attr = (JsonData)gameData["you"]["attributes"];
		
		int.TryParse ((string)attr["life"], out leftUser.maxHealth);
		int.TryParse ((string)attr["life_current"], out leftUser.health);
		int.TryParse ((string)attr["mana"], out leftUser.maxMana);
		int.TryParse ((string)attr["mana_current"], out leftUser.mana);
		
		int.TryParse ((string)attr["side"], out leftUser.side);
		
		leftUser.name = "You";
		
		JsonData enemy = gameData["enemy"];
		
		int.TryParse ((string)enemy["life"], out rightUser.maxHealth);
		int.TryParse ((string)enemy["life_current"], out rightUser.health);
		int.TryParse ((string)enemy["mana"], out rightUser.maxMana);
		int.TryParse ((string)enemy["mana_current"], out rightUser.mana);
		
		rightUser.name = (string)enemy["username"];

		timer = (int)gameData["turn"]["time"];
		Debug.Log ("time = " + timer.ToString ());

		JsonData enemyPivot = enemy["pivot"];
		int.TryParse((string)enemyPivot["user_id"], out rightUser.id);
		
		actSkills = new Action[gameData["you"]["active_skills"].Count];
		
		for (int x = 0; x < actSkills.Length; x++)
		{
			Action skill = new Action();
			skill.id = (string)gameData["you"]["active_skills"][x]["id"];
			skill.name = (string)gameData["you"]["active_skills"][x]["name"];
			skill.type = (string)gameData["you"]["active_skills"][x]["type"];
			skill.mana = (string)gameData["you"]["active_skills"][x]["mana"];
			actSkills[x] = skill;
		}
		
		aurSkills = new Action[gameData["you"]["aura_skills"].Count];
		
		for (int x = 0; x < aurSkills.Length; x++)
		{
			Action skill = new Action();
			skill.id = (string)gameData["you"]["aura_skills"][x]["id"];
			skill.name = (string)gameData["you"]["aura_skills"][x]["name"];
			skill.type = (string)gameData["you"]["aura_skills"][x]["type"];
			skill.mana = (string)gameData["you"]["aura_skills"][x]["mana"];
			aurSkills[x] = skill;
		}
		
		belt = new Action[gameData["you"]["belt"].Count];
		
		for (int x = 0; x < belt.Length; x++)
		{
			Action skill = new Action();
			skill.id = (string)gameData["you"]["belt"][x]["id"];
			skill.name = (string)gameData["you"]["belt"][x]["item"]["name"];
			skill.type = (string)gameData["you"]["belt"][x]["type"];
			skill.itemId = (string)gameData["you"]["belt"][x]["item_id"];
			belt[x] = skill;
		}
	}

	IEnumerator ProcessAction(string text)
	{
		string msg = "";
		FightInfo fInfo = new FightInfo();

		bool exception = false;
		try
		{
			JsonData data = JsonMapper.ToObject(text);
			Debug.Log("WWW Action Ok!: " + text);
			
			string result = (string)data["status"];
			if (result != "success") {			
				throw new Exception();
			}

			JsonData gameData = data["data"];

			msg = (string)data["msg"];

			JsonData fight = gameData["fight"];
			fInfo.block = (bool)fight["block"];
			fInfo.stun = (bool)fight["stun"];
			fInfo.critical = (bool)fight["critical"];
			int hit = (int)fight["hit_points"];
			fInfo.hitPoints = hit.ToString();

		}
		catch (Exception e)
		{
			Debug.Log("WWW Exception!: " + e.Message);
			//CloseApp();
			exception = true;
		}

		if (exception == false)
		{
			// UI Thread
			yield return Ninja.JumpToUnity;
			
			Animator[] anims = leftPlayer.GetComponentsInChildren<Animator>();
			foreach (Animator anim in anims)
			{
				anim.SetTrigger("NormalHit");
			}
			
			anims = rightPlayer.GetComponentsInChildren<Animator>();
			foreach (Animator anim in anims)
			{
				anim.SetTrigger("NormalHitBlock");//AttackSuccessful");
			}

//			Vector3 from = 
//			Vector3 to = 
//			AnimationHelper.MoveGO(objectHolder.camera.gameObject, from, to, 1.0f);

			yield return Ninja.JumpBack;
		}

		yield return false;
	}

	IEnumerator ProcessEquip(string text)
	{
		bool exception = false;
		try
		{
			JsonData data = JsonMapper.ToObject(text);
			Debug.Log("WWW Equip  Ok!: " + text);
			
			string result = (string)data["status"];
			if (result != "success") {
				throw new Exception();
			}

			JsonData gameData = data["data"];
			JsonData you = gameData["you"];
			JsonData opponent = gameData["opponent"];

			string sex = (string)you["sex"];
			if (sex == "m")
			{
				leftUser.sex = User.SEX.MALE;
			}
			else
			{
				leftUser.sex = User.SEX.FEMALE;
			}
			sex = (string)opponent["sex"];
			if (sex == "m")
			{
				rightUser.sex = User.SEX.MALE;
			}
			else
			{
				rightUser.sex = User.SEX.FEMALE;
			}

			leftUser.items = new Item[you["items"].Count];
			rightUser.items = new Item[opponent["items"].Count];

			for (int x = 0; x < leftUser.items.Length; x++)
			{
				Item item = new Item();
				item.id = (string)you["items"][x]["id"];
				item.sets = (string)you["items"][x]["sets"];
				item.type = (string)you["items"][x]["type"];
				leftUser.items[x] = item;
			}
			for (int x = 0; x < rightUser.items.Length; x++)
			{
				Item item = new Item();
				item.id = (string)opponent["items"][x]["id"];
				item.sets = (string)opponent["items"][x]["sets"];
				item.type = (string)opponent["items"][x]["type"];
				rightUser.items[x] = item;
			}


		}
		catch (Exception e)
		{
			Debug.Log("WWW Exception!: " + e.Message);
			CloseApp();
			exception = true;
		}

		if (exception == false)
		{
			// UI Thread
			yield return Ninja.JumpToUnity;
			
			AddModels();
			
			yield return Ninja.JumpBack;
		}
		yield return false;
	}

	void CloseApp()
	{
		Debug.Log("Fatal Error");
		StopCoroutine(coroutine);
		CancelInvoke("SyncRequest");
	}

	public void GET(string url, int type)
	{
		WWW www = new WWW (url);
		coroutine = WaitForRequest(www, type);
		StartCoroutine(coroutine);
	}
	
	public void POST(string url, Dictionary<string,string> post, int type)
	{
		WWWForm form = new WWWForm();
		foreach(KeyValuePair<string,string> post_arg in post)
		{
			form.AddField(post_arg.Key, post_arg.Value);
		}
		WWW www = new WWW(url, form);
		
		StartCoroutine(WaitForRequest(www, type)); 
	}
	
	private IEnumerator WaitForRequest(WWW www, int type)
	{
		yield return www;
		
		// check for errors
		if (www.error == null)
		{
			if (type == REQUEST_SYNC)
			{
				StartCoroutine(ProcessSync(www.text));
			}
			else if (type == REQUEST_ACTION)
			{
				StartCoroutine(ProcessAction(www.text));
			}
			else if (type == REQUEST_EQUIP)
			{
				StartCoroutine(ProcessEquip(www.text));
			}
		}
		else
		{
			Debug.Log("WWW Error: "+ www.error);
		}    
	}

	static public bool JsonDataContainsKey(JsonData data,string key)
	{
		bool result = false;
		if(data == null)
			return result;
		if(!data.IsObject)
		{
			return result;
		}
		IDictionary tdictionary = data as IDictionary;
		if(tdictionary == null)
			return result;
		if(tdictionary.Contains(key))
		{
			result = true;
		}
		return result;
	}
}
