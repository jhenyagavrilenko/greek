using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonHelper 
{
	public void AddButtons(Action[] active, Action[] aura, Action[] belt, ObjectHolder holder)
	{
		if (holder.normalHit == null) {
			Action normal = new Action();
			normal.mana = "0";
			normal.type = "hit";
			normal.name = "Hit";
			normal.id = "0";
			holder.normalHit = new ButtonIcon(holder.canvas, normal.name);
		}
		if (holder.activeSkills.Length != active.Length) {
			holder.activeSkills = new ButtonIcon[active.Length];
			int x = 0;
			foreach (Action action in active)
			{
				holder.activeSkills[x] = new ButtonIcon(holder.canvas, action.name);
				x++;
			}
		}
		if (holder.auraSkills.Length != aura.Length) {
			holder.auraSkills = new ButtonIcon[aura.Length];
			int x = 0;
			foreach (Action action in aura)
			{
				holder.auraSkills[x] = new ButtonIcon(holder.canvas, action.name);
				x++;
			}
		}
		if (holder.belt.Length != belt.Length) {
			holder.belt = new ButtonIcon[belt.Length];
			int x = 0;
			foreach (Action action in belt)
			{
				holder.belt[x] = new ButtonIcon(holder.canvas, action.name);
				x++;
			}
		}
		setPositionForButtons(holder);
	}

	public void SetState(ObjectHolder holder, string state)
	{
		bool interactable = true;
		if (state == "It is not your turn") {
			interactable = true;
		}
		if (holder.normalHit != null) {
			holder.normalHit.getButton().interactable = interactable;
			holder.normalHit.setIconInvisible(interactable);
		}
		foreach (ButtonIcon btn in holder.activeSkills) {
			btn.getButton().interactable = interactable;
			btn.setIconInvisible(interactable);
		}
		foreach (ButtonIcon btn in holder.auraSkills) {
			btn.getButton().interactable = interactable;
			btn.setIconInvisible(interactable);
		}
		foreach (ButtonIcon btn in holder.belt)	{
			btn.getButton().interactable = interactable;
			btn.setIconInvisible(interactable);
		}
	}

	private void setPositionForButtons(ObjectHolder holder)
	{
		int count = holder.activeSkills.Length + holder.auraSkills.Length + holder.belt.Length;
		if (holder.normalHit != null)
			count ++;
		int x = 0;
		count--;
		RectTransform objectRectTransform = holder.canvas.GetComponent<RectTransform>();
		int height = (int)objectRectTransform.rect.height;

		setPositionForButtonIcon(x, height, count, holder.normalHit);
		x++;
		foreach (ButtonIcon btn in holder.activeSkills)
		{
			setPositionForButtonIcon(x, height, count, btn);
			x++;
		}
		foreach (ButtonIcon btn in holder.auraSkills)
		{
			setPositionForButtonIcon(x, height, count, btn);
			x++;
		}
		foreach (ButtonIcon btn in holder.belt)
		{
			setPositionForButtonIcon(x, height, count, btn);
			x++;
		}
	}

	private void setPositionForButtonIcon(int position, int width, int count, ButtonIcon button)
	{
		Vector3 pos = button.getObjectPosition();
		pos.y = width / 2 - (count * (60 + 20) - 20) / 2 + (position * (60 + 20));
		button.setPosition(pos);
	}

//	private void setPositionForButton(int position, int width, int count, Button button)
//	{
//		Vector3 pos = button.transform.position;
//		pos.y = width / 2 - (count * (60 + 20) - 20) / 2 + (position * (60 + 20));
//		button.transform.position = pos;
//	}

//	private Button createButton(Canvas parent, Action action)
//	{
//		Button button = (Button)GameObject.Instantiate(Resources.Load<Button>("Button"));
//		button.transform.SetParent(parent.transform, false);
//		Text text = button.transform.GetComponentInChildren<Text>();
//		text.text = action.name;
//
//		Image image = button.GetComponent<Image>();
//		if (action.type == "active")
//		{
//			image.color = new Color(0.5f, 0.5f, 1f, 1f);
//		}
//		else if (action.type == "aura")
//		{
//			image.color = new Color(0.5f, 1f, 0.5f, 1f);
//		}
//		else if (action.type == "potion")
//		{
//			image.color = new Color(1f, 0.5f, 0.5f, 1f);
//		}
//		else if (action.type == "normal")
//		{
//			image.color = new Color(1f, 1f, 1f, 1f);
//		}
//
//		ActionButton scr = (button.GetComponent<ActionButton>() as ActionButton);
//		scr.action = action;
//
//		return button;
//	}
//	
}
