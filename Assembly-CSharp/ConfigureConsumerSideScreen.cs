using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D5D RID: 3421
public class ConfigureConsumerSideScreen : SideScreenContent
{
	// Token: 0x06006BC7 RID: 27591 RVA: 0x002896F7 File Offset: 0x002878F7
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IConfigurableConsumer>() != null;
	}

	// Token: 0x06006BC8 RID: 27592 RVA: 0x00289702 File Offset: 0x00287902
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetProducer = target.GetComponent<IConfigurableConsumer>();
		if (this.settings == null)
		{
			this.settings = this.targetProducer.GetSettingOptions();
		}
		this.PopulateOptions();
	}

	// Token: 0x06006BC9 RID: 27593 RVA: 0x00289738 File Offset: 0x00287938
	private void ClearOldOptions()
	{
		if (this.descriptor != null)
		{
			this.descriptor.gameObject.SetActive(false);
		}
		for (int i = 0; i < this.settingToggles.Count; i++)
		{
			this.settingToggles[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x06006BCA RID: 27594 RVA: 0x00289794 File Offset: 0x00287994
	private void PopulateOptions()
	{
		this.ClearOldOptions();
		for (int i = this.settingToggles.Count; i < this.settings.Length; i++)
		{
			IConfigurableConsumerOption setting = this.settings[i];
			HierarchyReferences component = Util.KInstantiateUI(this.consumptionSettingTogglePrefab, this.consumptionSettingToggleContainer.gameObject, true).GetComponent<HierarchyReferences>();
			this.settingToggles.Add(component);
			component.GetReference<LocText>("Label").text = setting.GetName();
			component.GetReference<Image>("Image").sprite = setting.GetIcon();
			MultiToggle reference = component.GetReference<MultiToggle>("Toggle");
			reference.onClick = (System.Action)Delegate.Combine(reference.onClick, new System.Action(delegate()
			{
				this.SelectOption(setting);
			}));
		}
		this.RefreshToggles();
		this.RefreshDetails();
	}

	// Token: 0x06006BCB RID: 27595 RVA: 0x0028987E File Offset: 0x00287A7E
	private void SelectOption(IConfigurableConsumerOption option)
	{
		this.targetProducer.SetSelectedOption(option);
		this.RefreshToggles();
		this.RefreshDetails();
	}

	// Token: 0x06006BCC RID: 27596 RVA: 0x00289898 File Offset: 0x00287A98
	private void RefreshToggles()
	{
		for (int i = 0; i < this.settingToggles.Count; i++)
		{
			MultiToggle reference = this.settingToggles[i].GetReference<MultiToggle>("Toggle");
			reference.ChangeState((this.settings[i] == this.targetProducer.GetSelectedOption()) ? 1 : 0);
			reference.gameObject.SetActive(true);
		}
	}

	// Token: 0x06006BCD RID: 27597 RVA: 0x002898FC File Offset: 0x00287AFC
	private void RefreshDetails()
	{
		if (this.descriptor == null)
		{
			GameObject gameObject = Util.KInstantiateUI(this.settingDescriptorPrefab, this.settingEffectRowsContainer.gameObject, true);
			this.descriptor = gameObject.GetComponent<LocText>();
		}
		IConfigurableConsumerOption selectedOption = this.targetProducer.GetSelectedOption();
		if (selectedOption != null)
		{
			this.descriptor.text = selectedOption.GetDetailedDescription();
			this.selectedOptionNameLabel.text = "<b>" + selectedOption.GetName() + "</b>";
			this.descriptor.gameObject.SetActive(true);
			return;
		}
		this.selectedOptionNameLabel.text = UI.UISIDESCREENS.FABRICATORSIDESCREEN.NORECIPESELECTED;
	}

	// Token: 0x06006BCE RID: 27598 RVA: 0x002899A2 File Offset: 0x00287BA2
	public override int GetSideScreenSortOrder()
	{
		return 1;
	}

	// Token: 0x04004985 RID: 18821
	[SerializeField]
	private RectTransform consumptionSettingToggleContainer;

	// Token: 0x04004986 RID: 18822
	[SerializeField]
	private GameObject consumptionSettingTogglePrefab;

	// Token: 0x04004987 RID: 18823
	[SerializeField]
	private RectTransform settingRequirementRowsContainer;

	// Token: 0x04004988 RID: 18824
	[SerializeField]
	private RectTransform settingEffectRowsContainer;

	// Token: 0x04004989 RID: 18825
	[SerializeField]
	private LocText selectedOptionNameLabel;

	// Token: 0x0400498A RID: 18826
	[SerializeField]
	private GameObject settingDescriptorPrefab;

	// Token: 0x0400498B RID: 18827
	private IConfigurableConsumer targetProducer;

	// Token: 0x0400498C RID: 18828
	private IConfigurableConsumerOption[] settings;

	// Token: 0x0400498D RID: 18829
	private LocText descriptor;

	// Token: 0x0400498E RID: 18830
	private List<HierarchyReferences> settingToggles = new List<HierarchyReferences>();

	// Token: 0x0400498F RID: 18831
	private List<GameObject> requirementRows = new List<GameObject>();
}
