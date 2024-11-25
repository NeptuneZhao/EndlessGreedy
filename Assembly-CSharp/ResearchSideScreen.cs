using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D99 RID: 3481
public class ResearchSideScreen : SideScreenContent
{
	// Token: 0x06006DC5 RID: 28101 RVA: 0x0029453C File Offset: 0x0029273C
	public ResearchSideScreen()
	{
		this.refreshDisplayStateDelegate = new Action<object>(this.RefreshDisplayState);
	}

	// Token: 0x06006DC6 RID: 28102 RVA: 0x00294558 File Offset: 0x00292758
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.selectResearchButton.onClick += delegate()
		{
			ManagementMenu.Instance.ToggleResearch();
		};
		Research.Instance.Subscribe(-1914338957, this.refreshDisplayStateDelegate);
		Research.Instance.Subscribe(-125623018, this.refreshDisplayStateDelegate);
		this.RefreshDisplayState(null);
	}

	// Token: 0x06006DC7 RID: 28103 RVA: 0x002945C8 File Offset: 0x002927C8
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.RefreshDisplayState(null);
		this.target = SelectTool.Instance.selected.GetComponent<KMonoBehaviour>().gameObject;
		this.target.gameObject.Subscribe(-1852328367, this.refreshDisplayStateDelegate);
		this.target.gameObject.Subscribe(-592767678, this.refreshDisplayStateDelegate);
	}

	// Token: 0x06006DC8 RID: 28104 RVA: 0x00294634 File Offset: 0x00292834
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.target)
		{
			this.target.gameObject.Unsubscribe(-1852328367, this.refreshDisplayStateDelegate);
			this.target.gameObject.Unsubscribe(187661686, this.refreshDisplayStateDelegate);
			this.target = null;
		}
	}

	// Token: 0x06006DC9 RID: 28105 RVA: 0x00294694 File Offset: 0x00292894
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Research.Instance.Unsubscribe(-1914338957, this.refreshDisplayStateDelegate);
		Research.Instance.Unsubscribe(-125623018, this.refreshDisplayStateDelegate);
		if (this.target)
		{
			this.target.gameObject.Unsubscribe(-1852328367, this.refreshDisplayStateDelegate);
			this.target.gameObject.Unsubscribe(187661686, this.refreshDisplayStateDelegate);
			this.target = null;
		}
	}

	// Token: 0x06006DCA RID: 28106 RVA: 0x0029471B File Offset: 0x0029291B
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<ResearchCenter>() != null || target.GetComponent<NuclearResearchCenter>() != null;
	}

	// Token: 0x06006DCB RID: 28107 RVA: 0x0029473C File Offset: 0x0029293C
	private void RefreshDisplayState(object data = null)
	{
		if (SelectTool.Instance.selected == null)
		{
			return;
		}
		string text = "";
		ResearchCenter component = SelectTool.Instance.selected.GetComponent<ResearchCenter>();
		NuclearResearchCenter component2 = SelectTool.Instance.selected.GetComponent<NuclearResearchCenter>();
		if (component != null)
		{
			text = component.research_point_type_id;
		}
		if (component2 != null)
		{
			text = component2.researchTypeID;
		}
		if (component == null && component2 == null)
		{
			return;
		}
		this.researchButtonIcon.sprite = Research.Instance.researchTypes.GetResearchType(text).sprite;
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch == null)
		{
			this.DescriptionText.text = "<b>" + UI.UISIDESCREENS.RESEARCHSIDESCREEN.NOSELECTEDRESEARCH + "</b>";
			return;
		}
		string text2 = "";
		if (!activeResearch.tech.costsByResearchTypeID.ContainsKey(text) || activeResearch.tech.costsByResearchTypeID[text] <= 0f)
		{
			text2 += "<color=#7f7f7f>";
		}
		text2 = text2 + "<b>" + activeResearch.tech.Name + "</b>";
		if (!activeResearch.tech.costsByResearchTypeID.ContainsKey(text) || activeResearch.tech.costsByResearchTypeID[text] <= 0f)
		{
			text2 += "</color>";
		}
		foreach (KeyValuePair<string, float> keyValuePair in activeResearch.tech.costsByResearchTypeID)
		{
			if (keyValuePair.Value != 0f)
			{
				bool flag = keyValuePair.Key == text;
				text2 += "\n   ";
				text2 += "<b>";
				if (!flag)
				{
					text2 += "<color=#7f7f7f>";
				}
				text2 = string.Concat(new string[]
				{
					text2,
					"- ",
					Research.Instance.researchTypes.GetResearchType(keyValuePair.Key).name,
					": ",
					activeResearch.progressInventory.PointsByTypeID[keyValuePair.Key].ToString(),
					"/",
					activeResearch.tech.costsByResearchTypeID[keyValuePair.Key].ToString()
				});
				if (!flag)
				{
					text2 += "</color>";
				}
				text2 += "</b>";
			}
		}
		this.DescriptionText.text = text2;
	}

	// Token: 0x04004AE7 RID: 19175
	public KButton selectResearchButton;

	// Token: 0x04004AE8 RID: 19176
	public Image researchButtonIcon;

	// Token: 0x04004AE9 RID: 19177
	public GameObject content;

	// Token: 0x04004AEA RID: 19178
	private GameObject target;

	// Token: 0x04004AEB RID: 19179
	private Action<object> refreshDisplayStateDelegate;

	// Token: 0x04004AEC RID: 19180
	public LocText DescriptionText;
}
