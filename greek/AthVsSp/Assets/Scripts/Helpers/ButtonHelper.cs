using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonHelper
{
	public void InitButtons(Action[] active, Action[] aura, Action[] belt, ObjectHolder holder)
	{
		if (holder.normalHit == null) {
			Action normal = new Action();
			normal.mana = "0";
			normal.type = "hit";
			normal.name = "Hit";
			normal.id = "4";
			holder.normalHit = new ButtonIcon(holder.canvas, normal);
		}
		if (holder.activeSkills.Length != active.Length) {
			holder.activeSkills = new ButtonIcon[active.Length];
			int x = 0;
			foreach (Action action in active)
			{
				holder.activeSkills[x] = new ButtonIcon(holder.canvas, action);
				x++;
			}
		}
		if (holder.auraSkills.Length != aura.Length) {
			holder.auraSkills = new ButtonIcon[aura.Length];
			int x = 0;
			foreach (Action action in aura)
			{
				holder.auraSkills[x] = new ButtonIcon(holder.canvas, action);
				x++;
			}
		}
		if (holder.belt.Length != belt.Length) {
			holder.belt = new ButtonIcon[belt.Length];
			int x = 0;
			foreach (Action action in belt)
			{
				holder.belt[x] = new ButtonIcon(holder.canvas, action);
				x++;
			}
		}
		setPositionForButtons(holder);
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
		pos.y = width - 100 - (count * (55 + 20) - 20) / 2 - (position * (55 + 20));
		button.setPosition(pos);
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
}
