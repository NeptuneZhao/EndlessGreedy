﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D84 RID: 3460
public class NToggleSideScreen : SideScreenContent
{
	// Token: 0x06006CDA RID: 27866 RVA: 0x0028F106 File Offset: 0x0028D306
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06006CDB RID: 27867 RVA: 0x0028F10E File Offset: 0x0028D30E
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<INToggleSideScreenControl>() != null;
	}

	// Token: 0x06006CDC RID: 27868 RVA: 0x0028F11C File Offset: 0x0028D31C
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.target = target.GetComponent<INToggleSideScreenControl>();
		if (this.target == null)
		{
			return;
		}
		this.titleKey = this.target.SidescreenTitleKey;
		base.gameObject.SetActive(true);
		this.Refresh();
	}

	// Token: 0x06006CDD RID: 27869 RVA: 0x0028F168 File Offset: 0x0028D368
	private void Refresh()
	{
		for (int i = 0; i < Mathf.Max(this.target.Options.Count, this.buttonList.Count); i++)
		{
			if (i >= this.target.Options.Count)
			{
				this.buttonList[i].gameObject.SetActive(false);
			}
			else
			{
				if (i >= this.buttonList.Count)
				{
					KToggle ktoggle = Util.KInstantiateUI<KToggle>(this.buttonPrefab.gameObject, this.ContentContainer, false);
					int idx = i;
					ktoggle.onClick += delegate()
					{
						this.target.QueueSelectedOption(idx);
						this.Refresh();
					};
					this.buttonList.Add(ktoggle);
				}
				this.buttonList[i].GetComponentInChildren<LocText>().text = this.target.Options[i];
				this.buttonList[i].GetComponentInChildren<ToolTip>().toolTip = this.target.Tooltips[i];
				if (this.target.SelectedOption == i && this.target.QueuedOption == i)
				{
					this.buttonList[i].isOn = true;
					ImageToggleState[] componentsInChildren = this.buttonList[i].GetComponentsInChildren<ImageToggleState>();
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						componentsInChildren[j].SetActive();
					}
					this.buttonList[i].GetComponent<ImageToggleStateThrobber>().enabled = false;
				}
				else if (this.target.QueuedOption == i)
				{
					this.buttonList[i].isOn = true;
					ImageToggleState[] componentsInChildren = this.buttonList[i].GetComponentsInChildren<ImageToggleState>();
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						componentsInChildren[j].SetActive();
					}
					this.buttonList[i].GetComponent<ImageToggleStateThrobber>().enabled = true;
				}
				else
				{
					this.buttonList[i].isOn = false;
					foreach (ImageToggleState imageToggleState in this.buttonList[i].GetComponentsInChildren<ImageToggleState>())
					{
						imageToggleState.SetInactive();
						imageToggleState.SetInactive();
					}
					this.buttonList[i].GetComponent<ImageToggleStateThrobber>().enabled = false;
				}
				this.buttonList[i].gameObject.SetActive(true);
			}
		}
		this.description.text = this.target.Description;
		this.description.gameObject.SetActive(!string.IsNullOrEmpty(this.target.Description));
	}

	// Token: 0x04004A3A RID: 19002
	[SerializeField]
	private KToggle buttonPrefab;

	// Token: 0x04004A3B RID: 19003
	[SerializeField]
	private LocText description;

	// Token: 0x04004A3C RID: 19004
	private INToggleSideScreenControl target;

	// Token: 0x04004A3D RID: 19005
	private List<KToggle> buttonList = new List<KToggle>();
}
