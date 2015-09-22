using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System;
using UnityEngine.UI;
using CielaSpike;

public class Logic : MonoBehaviour
{
	public ObjectHolder objectHolder;
	public GameObject testMale;
	public bool playerTurn;
	public int playerSide;

	private const int REQUEST_SYNC = 0;
	private const int REQUEST_ACTION = 1;
	private const int REQUEST_EQUIP = 2;

	private int _battleId = 0;
	private string _userName = null;
	private int _opponentId;
	private IEnumerator coroutine;
	private GameObject leftPlayer;
	private GameObject rightPlayer;
	private User leftUser;
	private User rightUser;
	private FightInfo lastFight = new FightInfo ();
	private BaseAnimation enemyAnimation = null;
	private bool isAnimation = false;
	private float movePlayers = 0.01F;
	private float moveCamera = 0.5F;
	private Button attackButton;  //Zanko

	void Start()
	{
		attackButton = GameObject.Find ("attack").GetComponent<Button>(); //Zanko
		attackButton.onClick.AddListener (() => { attack(); });           //Zanko

		SetBattle(18, "lennondtps@gmail.com"); //lennondtps@gmail.com  player@email.gr
	}

	private void SetBattle(int battleId, string userName)
	{
		_battleId = battleId;
		_userName = userName;
		_opponentId = -1;
		InvokeRepeating("SyncRequest", 1.0F, 1.0F);
	}

	private void SyncRequest()
	{
		if (isAnimation)
		{
			return;
		}
		string req = "http://api.gw.monospacelabs.com/v1/battle/sync";
		req += "?battle_id=" + _battleId + "&email=" + _userName;
		GET(req, REQUEST_SYNC);
	}

	public void GET(string url, int type)
	{
		WWW www = new WWW (url);
		coroutine = WaitForRequest(www, type);
		StartCoroutine(coroutine);
	}

	private IEnumerator WaitForRequest(WWW www, int type)
	{
		yield return www;
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

	IEnumerator ProcessSync(string text)
	{
		String status = "";
		bool exception = false;
		int timer = 0;
		
		Action[] belt = new Action[0];
		Action[] aurSkills = new Action[0];
		Action[] actSkills = new Action[0];
		FightInfo fInfo = null;
		
		try {
			JsonData data = JsonMapper.ToObject (text);
			Debug.Log ("WWW Sync Ok!: " + text);
			
			string result = (string)data["status"];
			Debug.Log ("result: " + result);
			if (result != "success") {
				throw new Exception();
			}
			
			JsonData gameData = data["data"];
			
			processSync(gameData, out status, out timer, out actSkills, out aurSkills, out belt);
			fInfo = processSyncAction(gameData["action"]);
			
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
			
			bool needUpdate = true;
			
			
			if (fInfo != null && leftPlayer != null && rightPlayer != null)
			{
				if (fInfo == lastFight)
				{
					needUpdate = true;
				}
				else
				{
					needUpdate = false;
					lastFight = fInfo;
					startAction(fInfo);
				}
			}
			
			if (needUpdate)
			{
				objectHolder.statusLabel.text = status;
				objectHolder.timer.text = timer.ToString();
				ApplyNewUIData();
			}
			else
			{
				objectHolder.statusLabel.text = "";
				objectHolder.timer.text = "";
			}
			objectHolder.bHelper.InitButtons(actSkills, aurSkills, belt, objectHolder);
			
			objectHolder.bHelper.SetState(objectHolder, status);
			
			yield return Ninja.JumpBack;
		}
		
		yield return false;
	}

	IEnumerator ProcessAction(string text)
	{	
		bool exception = false;
		try
		{
			JsonData data = JsonMapper.ToObject(text);
			Debug.Log("WWW Action Ok!: " + text);
			
			string result = (string)data["status"];
			if (result != "success") {			
				throw new Exception();
			}
		}
		catch (Exception e)
		{
			Debug.Log("WWW Exception!: " + e.Message);;
			exception = true;
		}
		
		if (exception == false)
		{
			yield return Ninja.JumpToUnity;

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

	private void attack(){
		float time = 0.01F;
		AnimationHelper.MoveGO(leftPlayer, enemyAnimation.Start(), enemyAnimation.HitSuc(), time);
		AnimationHelper.MoveGO(rightPlayer, enemyAnimation.EStart(), enemyAnimation.EHitSuc(), time);
		AnimationHelper.MoveGO(objectHolder.mainCamera.gameObject, enemyAnimation.CStart(), enemyAnimation.CHitSuc(), 0.5F);
		StartCoroutine(ExecuteAfterTime(time));
	}

	IEnumerator ExecuteAfterTime(float time)
	{
		yield return new WaitForSeconds(time);

		Animator[] anims;
		anims = leftPlayer.GetComponentsInChildren<Animator>();
		foreach (Animator anim in anims)
		{
			anim.SetTrigger("NormalHit");
		}
		anims = rightPlayer.GetComponentsInChildren<Animator>();
		foreach (Animator anim in anims)
		{
			anim.SetTrigger("NormalHitAttackSuccessful");
		}

		yield return new WaitForSeconds(4.0F);

		AnimationHelper.MoveGO(leftPlayer, enemyAnimation.HitSuc(), enemyAnimation.Start(), time);
		AnimationHelper.MoveGO(rightPlayer, enemyAnimation.EHitSuc(), enemyAnimation.EStart(), time);
		AnimationHelper.MoveGO(objectHolder.mainCamera.gameObject, enemyAnimation.CHitSuc(), enemyAnimation.CStart(), time);
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
	
	void AddModels()
	{
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
				AnimationHelper.SetPosition(rightPlayer, enemyAnimation.EStart());
				AnimationHelper.SetRotation(rightPlayer, enRot);
				MaleConstants.Equip(rightPlayer, rightUser.items);
			}
			else
			{
				enemyAnimation = new MaleAnimation();
				rightPlayer = Instantiate(objectHolder.female);
				AnimationHelper.SetPosition(rightPlayer, enemyAnimation.EStart());
				AnimationHelper.SetRotation(rightPlayer, enRot);
				FemaleConstants.Equip(rightPlayer, rightUser.items);
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
			AnimationHelper.SetPosition(leftPlayer, enemyAnimation.Start());
			MaleConstants.Equip(leftPlayer, leftUser.items);
		}
		else
		{
			leftPlayer = Instantiate(objectHolder.female);
			AnimationHelper.SetPosition(leftPlayer, enemyAnimation.Start());
			FemaleConstants.Equip(leftPlayer, leftUser.items);
		}

		AnimationHelper.SetPosition(objectHolder.mainCamera.gameObject, enemyAnimation.CStart());
	}

	void ApplyNewUIData()
	{
		objectHolder.leftName.text = leftUser.name;
		objectHolder.rightName.text = rightUser.name;
		objectHolder.leftHealthText.text = "Health:" + '\n' + leftUser.health + "/" + leftUser.maxHealth;
		objectHolder.leftMagicText.text = "Magic:" + '\n' + leftUser.mana + "/" + leftUser.maxMana;
		objectHolder.rightHealthText.text = "Health:" + '\n' + rightUser.health + "/" + rightUser.maxHealth;;
		objectHolder.rightMagicText.text = "Magic:" + '\n' + rightUser.mana + "/" + rightUser.maxMana;
		RectTransformExtensions.SetHeight(objectHolder.leftMana, leftUser.mana * 272 / leftUser.maxMana);
		RectTransformExtensions.SetHeight(objectHolder.leftHealth, leftUser.health * 272 / leftUser.maxHealth);
		RectTransformExtensions.SetHeight(objectHolder.rightMana, rightUser.mana * 272 / rightUser.maxMana);
		RectTransformExtensions.SetHeight(objectHolder.rightHealth, rightUser.health * 272 / rightUser.maxHealth);
	}

	public void ButtonPostAction(Action action)
	{
		objectHolder.bHelper.SetState(objectHolder, "It is not your turn");
		string req = "http://api.gw.monospacelabs.com/v1/battle/action";
		Dictionary<string,string> dict = new Dictionary<string,string>();
		dict["email"] = _userName; //  
		string battleId = "";
		battleId = _battleId.ToString();
		dict["battle_id"] = battleId; // 16
		dict["type"] = action.type;
		switch (action.type) 
		{
			case "hit":  
				dict["param"] = "normal";
				break;
			case "skill":
				dict["param"] = action.id;
				break;
			case "item":
				dict["param"] = action.itemId;
				break;
			default: break;
		}
		POST(req, dict, REQUEST_ACTION);
	}

	void startAction(FightInfo info)
	{
		bool attacking = (info.attackerId == leftUser.id);
		bool block = info.block;

		AnimationHelper.MoveGO(leftPlayer, leftPlayer.transform.position, enemyAnimation.HitSuc(), movePlayers);
		AnimationHelper.MoveGO(rightPlayer, rightPlayer.transform.position, enemyAnimation.EHitSuc(), movePlayers);
		AnimationHelper.MoveGO(objectHolder.mainCamera.gameObject, objectHolder.mainCamera.gameObject.transform.position, enemyAnimation.CHitSuc(), moveCamera);
	}


	FightInfo processSyncAction(JsonData gameData)
	{
		FightInfo fInfo = new FightInfo();

		int attacker = 0;
		int defender = 0;
		if (gameData.IsObject == false)
		{
			return null;
		}
		int.TryParse((string)gameData["attacker"], out attacker);
		int.TryParse((string)gameData["defender"], out defender);
		fInfo.attackerId = attacker;
		fInfo.defenderId = defender;
		fInfo.block = (bool)gameData["block"];
		fInfo.stun = (bool)gameData["stun"];
		fInfo.critical = (bool)gameData["critical"];
		fInfo.hitPoints = (int)gameData["hit_points"];
		fInfo.type = (string)gameData["type"];
		fInfo.param = (string)gameData["param"];

		return fInfo;
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
		int.TryParse ((string)enemy["pivot"]["side"], out rightUser.side);


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

		parseOthers (gameData);
	}

	void parseOthers(JsonData gameData)
	{
		string[] users = new string[gameData["others"].Count];
		int[] sides = new int[gameData["others"].Count];
		
		for (int x = 0; x < users.Length; x++)
		{
			users[x] = (string)gameData["others"][x]["username"];
			int.TryParse ((string)gameData["others"][x]["pivot"]["side"], out sides[x]);
		}

		for (int x = 0; x < users.Length; x++)
		{
			if(leftUser.side == sides[x]){
				leftUser.side++;
			}
			if(rightUser.side == sides[x]){
				rightUser.side++;
			}
			
		}
		leftUser.addSideToName (leftUser.side);
		rightUser.addSideToName (rightUser.side);

	}

	void CloseApp()
	{
		Debug.Log("Fatal Error");
		StopCoroutine(coroutine);
		CancelInvoke("SyncRequest");
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
