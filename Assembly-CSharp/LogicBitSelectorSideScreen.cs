using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000D79 RID: 3449
public class LogicBitSelectorSideScreen : SideScreenContent, IRenderEveryTick
{
	// Token: 0x06006C81 RID: 27777 RVA: 0x0028D1A3 File Offset: 0x0028B3A3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.activeColor = GlobalAssets.Instance.colorSet.logicOnText;
		this.inactiveColor = GlobalAssets.Instance.colorSet.logicOffText;
	}

	// Token: 0x06006C82 RID: 27778 RVA: 0x0028D1DF File Offset: 0x0028B3DF
	public void SelectToggle(int bit)
	{
		this.target.SetBitSelection(bit);
		this.target.UpdateVisuals();
		this.RefreshToggles();
	}

	// Token: 0x06006C83 RID: 27779 RVA: 0x0028D200 File Offset: 0x0028B400
	private void RefreshToggles()
	{
		for (int i = 0; i < this.target.GetBitDepth(); i++)
		{
			int n = i;
			if (!this.toggles_by_int.ContainsKey(i))
			{
				GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowPrefab.transform.parent.gameObject, true);
				gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("bitName").SetText(string.Format(UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.BIT, i + 1));
				gameObject.GetComponent<HierarchyReferences>().GetReference<KImage>("stateIcon").color = (this.target.IsBitActive(i) ? this.activeColor : this.inactiveColor);
				gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("stateText").SetText(this.target.IsBitActive(i) ? UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_ACTIVE : UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_INACTIVE);
				MultiToggle component = gameObject.GetComponent<MultiToggle>();
				this.toggles_by_int.Add(i, component);
			}
			this.toggles_by_int[i].onClick = delegate()
			{
				this.SelectToggle(n);
			};
		}
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.toggles_by_int)
		{
			if (this.target.GetBitSelection() == keyValuePair.Key)
			{
				keyValuePair.Value.ChangeState(0);
			}
			else
			{
				keyValuePair.Value.ChangeState(1);
			}
		}
	}

	// Token: 0x06006C84 RID: 27780 RVA: 0x0028D3A0 File Offset: 0x0028B5A0
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<ILogicRibbonBitSelector>() != null;
	}

	// Token: 0x06006C85 RID: 27781 RVA: 0x0028D3AC File Offset: 0x0028B5AC
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<ILogicRibbonBitSelector>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received is not an ILogicRibbonBitSelector");
			return;
		}
		this.titleKey = this.target.SideScreenTitle;
		this.readerDescriptionContainer.SetActive(this.target.SideScreenDisplayReaderDescription());
		this.writerDescriptionContainer.SetActive(this.target.SideScreenDisplayWriterDescription());
		this.RefreshToggles();
		this.UpdateInputOutputDisplay();
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.toggles_by_int)
		{
			this.UpdateStateVisuals(keyValuePair.Key);
		}
	}

	// Token: 0x06006C86 RID: 27782 RVA: 0x0028D484 File Offset: 0x0028B684
	public void RenderEveryTick(float dt)
	{
		if (this.target.Equals(null))
		{
			return;
		}
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.toggles_by_int)
		{
			this.UpdateStateVisuals(keyValuePair.Key);
		}
		this.UpdateInputOutputDisplay();
	}

	// Token: 0x06006C87 RID: 27783 RVA: 0x0028D4F4 File Offset: 0x0028B6F4
	private void UpdateInputOutputDisplay()
	{
		if (this.target.SideScreenDisplayReaderDescription())
		{
			this.outputDisplayIcon.color = ((this.target.GetOutputValue() > 0) ? this.activeColor : this.inactiveColor);
		}
		if (this.target.SideScreenDisplayWriterDescription())
		{
			this.inputDisplayIcon.color = ((this.target.GetInputValue() > 0) ? this.activeColor : this.inactiveColor);
		}
	}

	// Token: 0x06006C88 RID: 27784 RVA: 0x0028D56C File Offset: 0x0028B76C
	private void UpdateStateVisuals(int bit)
	{
		MultiToggle multiToggle = this.toggles_by_int[bit];
		multiToggle.gameObject.GetComponent<HierarchyReferences>().GetReference<KImage>("stateIcon").color = (this.target.IsBitActive(bit) ? this.activeColor : this.inactiveColor);
		multiToggle.gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("stateText").SetText(this.target.IsBitActive(bit) ? UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_ACTIVE : UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_INACTIVE);
	}

	// Token: 0x04004A04 RID: 18948
	private ILogicRibbonBitSelector target;

	// Token: 0x04004A05 RID: 18949
	public GameObject rowPrefab;

	// Token: 0x04004A06 RID: 18950
	public KImage inputDisplayIcon;

	// Token: 0x04004A07 RID: 18951
	public KImage outputDisplayIcon;

	// Token: 0x04004A08 RID: 18952
	public GameObject readerDescriptionContainer;

	// Token: 0x04004A09 RID: 18953
	public GameObject writerDescriptionContainer;

	// Token: 0x04004A0A RID: 18954
	[NonSerialized]
	public Dictionary<int, MultiToggle> toggles_by_int = new Dictionary<int, MultiToggle>();

	// Token: 0x04004A0B RID: 18955
	private Color activeColor;

	// Token: 0x04004A0C RID: 18956
	private Color inactiveColor;
}
