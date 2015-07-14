using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonHelper {

	private void setPositionForButtons(ObjectHolder holder)
	{
		int count = holder.activeSkills.Length + holder.auraSkills.Length + holder.belt.Length;
		if (holder.normalHit != null)
			count ++;
		int x = 0;
		count --;
		RectTransform objectRectTransform = holder.canvas.GetComponent<RectTransform>();

		setPositionForButton(x, (int)objectRectTransform.rect.width, count, holder.normalHit);
		x++;
		foreach (Button btn in holder.activeSkills)
		{
			Text text = btn.transform.GetComponentInChildren<Text>();
			setPositionForButton(x, (int)objectRectTransform.rect.width, count, btn);
			x++;
		}
		foreach (Button btn in holder.auraSkills)
		{
			Text text = btn.transform.GetComponentInChildren<Text>();
			setPositionForButton(x, (int)objectRectTransform.rect.width, count, btn);
			x++;
		}
		foreach (Button btn in holder.belt)
		{
			Text text = btn.transform.GetComponentInChildren<Text>();
			setPositionForButton(x, (int)objectRectTransform.rect.width, count, btn);
			x++;
		}
	}

	private void setPositionForButton(int position, int width, int count, Button button)
	{
		Vector3 pos = button.transform.position;
		pos.x = width / 2 - (count * (60 + 20) - 20) / 2 + (position * (60 + 20));
		button.transform.position = pos;
	}

	private Button createButton(Canvas parent, Action action)
	{
		Button button = (Button)GameObject.Instantiate(Resources.Load<Button>("Button"));
		button.transform.SetParent(parent.transform, false);
		Text text = button.transform.GetComponentInChildren<Text>();
		text.text = action.name;

		Image image = button.GetComponent<Image>();
		if (action.type == "active")
		{
			image.color = new Color(0.5f, 0.5f, 1f, 1f);
		}
		else if (action.type == "aura")
		{
			image.color = new Color(0.5f, 1f, 0.5f, 1f);
		}
		else if (action.type == "potion")
		{
			image.color = new Color(1f, 0.5f, 0.5f, 1f);
		}
		else if (action.type == "normal")
		{
			image.color = new Color(1f, 1f, 1f, 1f);
		}

		ActionButton scr = (button.GetComponent<ActionButton>() as ActionButton);
		scr.action = action;

		return button;
	}

	public void AddButtons(Action[] active, Action[] aura, Action[] belt, ObjectHolder holder)
	{
		if (holder.normalHit == null)
		{
			Action normal = new Action();
			normal.mana = "0";
			normal.type = "hit";
			normal.name = "Hit";
			normal.id = "0";
			holder.normalHit = createButton(holder.canvas, normal);
		}
		if (holder.activeSkills.Length != active.Length)
		{
			holder.activeSkills = new Button[active.Length];
			int x = 0;
			foreach (Action action in active)
			{
				holder.activeSkills[x] = createButton(holder.canvas, action);
				x++;
			}
		}
		if (holder.auraSkills.Length != aura.Length)
		{
			holder.auraSkills = new Button[aura.Length];
			int x = 0;
			foreach (Action action in aura)
			{
				holder.auraSkills[x] = createButton(holder.canvas, action);
				x++;
			}
		}
		if (holder.belt.Length != belt.Length)
		{
			holder.belt = new Button[belt.Length];
			int x = 0;
			foreach (Action action in belt)
			{
				holder.belt[x] = createButton(holder.canvas, action);
				x++;
			}
		}

		setPositionForButtons(holder);
	}

	public void SetState(ObjectHolder holder, string state)
	{
		bool interactable = true;
		if (state == "It is not your turn")
		{
			interactable = false;
		}

		if (holder.normalHit != null)
			holder.normalHit.interactable = interactable;

		foreach (Button btn in holder.activeSkills)
		{
			btn.interactable = interactable;
		}
		foreach (Button btn in holder.auraSkills)
		{
			btn.interactable = interactable;
		}
		foreach (Button btn in holder.belt)
		{
			btn.interactable = interactable;
		}
	}
}
