using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D50 RID: 3408
public class ButtonMenuSideScreen : SideScreenContent
{
	// Token: 0x06006B45 RID: 27461 RVA: 0x00285EA0 File Offset: 0x002840A0
	public override bool IsValidForTarget(GameObject target)
	{
		ISidescreenButtonControl sidescreenButtonControl = target.GetComponent<ISidescreenButtonControl>();
		if (sidescreenButtonControl == null)
		{
			sidescreenButtonControl = target.GetSMI<ISidescreenButtonControl>();
		}
		return sidescreenButtonControl != null && sidescreenButtonControl.SidescreenEnabled();
	}

	// Token: 0x06006B46 RID: 27462 RVA: 0x00285EC9 File Offset: 0x002840C9
	public override int GetSideScreenSortOrder()
	{
		if (this.targets == null)
		{
			return 20;
		}
		return this.targets[0].ButtonSideScreenSortOrder();
	}

	// Token: 0x06006B47 RID: 27463 RVA: 0x00285EE7 File Offset: 0x002840E7
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.targets = new_target.GetAllSMI<ISidescreenButtonControl>();
		this.targets.AddRange(new_target.GetComponents<ISidescreenButtonControl>());
		this.Refresh();
	}

	// Token: 0x06006B48 RID: 27464 RVA: 0x00285F20 File Offset: 0x00284120
	public GameObject GetHorizontalGroup(int id)
	{
		if (!this.horizontalGroups.ContainsKey(id))
		{
			this.horizontalGroups.Add(id, Util.KInstantiateUI(this.horizontalGroupPrefab, this.buttonContainer.gameObject, true));
		}
		return this.horizontalGroups[id];
	}

	// Token: 0x06006B49 RID: 27465 RVA: 0x00285F60 File Offset: 0x00284160
	public void CopyLayoutSettings(LayoutElement to, LayoutElement from)
	{
		to.ignoreLayout = from.ignoreLayout;
		to.minWidth = from.minWidth;
		to.minHeight = from.minHeight;
		to.preferredWidth = from.preferredWidth;
		to.preferredHeight = from.preferredHeight;
		to.flexibleWidth = from.flexibleWidth;
		to.flexibleHeight = from.flexibleHeight;
		to.layoutPriority = from.layoutPriority;
	}

	// Token: 0x06006B4A RID: 27466 RVA: 0x00285FD0 File Offset: 0x002841D0
	private void Refresh()
	{
		while (this.liveButtons.Count < this.targets.Count)
		{
			this.liveButtons.Add(Util.KInstantiateUI(this.buttonPrefab.gameObject, this.buttonContainer.gameObject, true));
		}
		foreach (int key in this.horizontalGroups.Keys)
		{
			this.horizontalGroups[key].SetActive(false);
		}
		for (int i = 0; i < this.liveButtons.Count; i++)
		{
			if (i >= this.targets.Count)
			{
				this.liveButtons[i].SetActive(false);
			}
			else
			{
				if (!this.liveButtons[i].activeSelf)
				{
					this.liveButtons[i].SetActive(true);
				}
				int num = this.targets[i].HorizontalGroupID();
				LayoutElement component = this.liveButtons[i].GetComponent<LayoutElement>();
				KButton componentInChildren = this.liveButtons[i].GetComponentInChildren<KButton>();
				ToolTip componentInChildren2 = this.liveButtons[i].GetComponentInChildren<ToolTip>();
				LocText componentInChildren3 = this.liveButtons[i].GetComponentInChildren<LocText>();
				if (num >= 0)
				{
					GameObject horizontalGroup = this.GetHorizontalGroup(num);
					horizontalGroup.SetActive(true);
					this.liveButtons[i].transform.SetParent(horizontalGroup.transform, false);
					this.CopyLayoutSettings(component, this.horizontalButtonPrefab);
				}
				else
				{
					this.liveButtons[i].transform.SetParent(this.buttonContainer, false);
					this.CopyLayoutSettings(component, this.buttonPrefab);
				}
				componentInChildren.isInteractable = this.targets[i].SidescreenButtonInteractable();
				componentInChildren.ClearOnClick();
				componentInChildren.onClick += this.targets[i].OnSidescreenButtonPressed;
				componentInChildren.onClick += this.Refresh;
				componentInChildren3.SetText(this.targets[i].SidescreenButtonText);
				componentInChildren2.SetSimpleTooltip(this.targets[i].SidescreenButtonTooltip);
			}
		}
	}

	// Token: 0x04004925 RID: 18725
	public const int DefaultButtonMenuSideScreenSortOrder = 20;

	// Token: 0x04004926 RID: 18726
	public LayoutElement buttonPrefab;

	// Token: 0x04004927 RID: 18727
	public LayoutElement horizontalButtonPrefab;

	// Token: 0x04004928 RID: 18728
	public GameObject horizontalGroupPrefab;

	// Token: 0x04004929 RID: 18729
	public RectTransform buttonContainer;

	// Token: 0x0400492A RID: 18730
	private List<GameObject> liveButtons = new List<GameObject>();

	// Token: 0x0400492B RID: 18731
	private Dictionary<int, GameObject> horizontalGroups = new Dictionary<int, GameObject>();

	// Token: 0x0400492C RID: 18732
	private List<ISidescreenButtonControl> targets;
}
