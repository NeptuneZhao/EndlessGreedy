using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D7F RID: 3455
public class ModuleFlightUtilitySideScreen : SideScreenContent
{
	// Token: 0x1700079D RID: 1949
	// (get) Token: 0x06006CB6 RID: 27830 RVA: 0x0028E578 File Offset: 0x0028C778
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x06006CB7 RID: 27831 RVA: 0x0028E585 File Offset: 0x0028C785
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06006CB8 RID: 27832 RVA: 0x0028E595 File Offset: 0x0028C795
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x06006CB9 RID: 27833 RVA: 0x0028E59C File Offset: 0x0028C79C
	public override bool IsValidForTarget(GameObject target)
	{
		if (target.GetComponent<Clustercraft>() != null && this.HasFlightUtilityModule(target.GetComponent<CraftModuleInterface>()))
		{
			return true;
		}
		RocketControlStation component = target.GetComponent<RocketControlStation>();
		return component != null && this.HasFlightUtilityModule(component.GetMyWorld().GetComponent<Clustercraft>().ModuleInterface);
	}

	// Token: 0x06006CBA RID: 27834 RVA: 0x0028E5F0 File Offset: 0x0028C7F0
	private bool HasFlightUtilityModule(CraftModuleInterface craftModuleInterface)
	{
		using (IEnumerator<Ref<RocketModuleCluster>> enumerator = craftModuleInterface.ClusterModules.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get().GetSMI<IEmptyableCargo>() != null)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06006CBB RID: 27835 RVA: 0x0028E648 File Offset: 0x0028C848
	public override void SetTarget(GameObject target)
	{
		if (target != null)
		{
			foreach (int id in this.refreshHandle)
			{
				target.Unsubscribe(id);
			}
			this.refreshHandle.Clear();
		}
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		if (this.targetCraft == null && target.GetComponent<RocketControlStation>() != null)
		{
			this.targetCraft = target.GetMyWorld().GetComponent<Clustercraft>();
		}
		this.refreshHandle.Add(this.targetCraft.gameObject.Subscribe(-1298331547, new Action<object>(this.RefreshAll)));
		this.refreshHandle.Add(this.targetCraft.gameObject.Subscribe(1792516731, new Action<object>(this.RefreshAll)));
		this.BuildModules();
	}

	// Token: 0x06006CBC RID: 27836 RVA: 0x0028E750 File Offset: 0x0028C950
	private void ClearModules()
	{
		foreach (KeyValuePair<IEmptyableCargo, HierarchyReferences> keyValuePair in this.modulePanels)
		{
			Util.KDestroyGameObject(keyValuePair.Value.gameObject);
		}
		this.modulePanels.Clear();
	}

	// Token: 0x06006CBD RID: 27837 RVA: 0x0028E7B8 File Offset: 0x0028C9B8
	private void BuildModules()
	{
		this.ClearModules();
		foreach (Ref<RocketModuleCluster> @ref in this.craftModuleInterface.ClusterModules)
		{
			IEmptyableCargo smi = @ref.Get().GetSMI<IEmptyableCargo>();
			if (smi != null)
			{
				HierarchyReferences value = Util.KInstantiateUI<HierarchyReferences>(this.modulePanelPrefab, this.moduleContentContainer, true);
				this.modulePanels.Add(smi, value);
				this.RefreshModulePanel(smi);
			}
		}
	}

	// Token: 0x06006CBE RID: 27838 RVA: 0x0028E840 File Offset: 0x0028CA40
	private void RefreshAll(object data = null)
	{
		this.BuildModules();
	}

	// Token: 0x06006CBF RID: 27839 RVA: 0x0028E848 File Offset: 0x0028CA48
	private void RefreshModulePanel(IEmptyableCargo module)
	{
		HierarchyReferences hierarchyReferences = this.modulePanels[module];
		hierarchyReferences.GetReference<Image>("icon").sprite = Def.GetUISprite(module.master.gameObject, "ui", false).first;
		KButton reference = hierarchyReferences.GetReference<KButton>("button");
		reference.isInteractable = module.CanEmptyCargo();
		reference.ClearOnClick();
		reference.onClick += module.EmptyCargo;
		KButton reference2 = hierarchyReferences.GetReference<KButton>("repeatButton");
		if (module.CanAutoDeploy)
		{
			this.StyleRepeatButton(module);
			reference2.ClearOnClick();
			reference2.onClick += delegate()
			{
				this.OnRepeatClicked(module);
			};
			reference2.gameObject.SetActive(true);
		}
		else
		{
			reference2.gameObject.SetActive(false);
		}
		DropDown reference3 = hierarchyReferences.GetReference<DropDown>("dropDown");
		reference3.targetDropDownContainer = GameScreenManager.Instance.ssOverlayCanvas;
		reference3.Close();
		CrewPortrait reference4 = hierarchyReferences.GetReference<CrewPortrait>("selectedPortrait");
		WorldContainer component = (module as StateMachine.Instance).GetMaster().GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<WorldContainer>();
		if (component != null && module.ChooseDuplicant)
		{
			int id = component.id;
			reference3.gameObject.SetActive(true);
			reference3.Initialize(Components.LiveMinionIdentities.GetWorldItems(id, false), new Action<IListableOption, object>(this.OnDuplicantEntryClick), null, new Action<DropDownEntry, object>(this.DropDownEntryRefreshAction), true, module);
			reference3.selectedLabel.text = ((module.ChosenDuplicant != null) ? this.GetDuplicantRowName(module.ChosenDuplicant) : UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.SELECT_DUPLICANT.ToString());
			reference4.gameObject.SetActive(true);
			reference4.SetIdentityObject(module.ChosenDuplicant, false);
			reference3.openButton.isInteractable = !module.ModuleDeployed;
		}
		else
		{
			reference3.gameObject.SetActive(false);
			reference4.gameObject.SetActive(false);
		}
		hierarchyReferences.GetReference<LocText>("label").SetText(module.master.gameObject.GetProperName());
	}

	// Token: 0x06006CC0 RID: 27840 RVA: 0x0028EAA0 File Offset: 0x0028CCA0
	private string GetDuplicantRowName(MinionIdentity minion)
	{
		MinionResume component = minion.GetComponent<MinionResume>();
		if (component != null && component.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
		{
			return string.Format(UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.PILOT_FMT, minion.GetProperName());
		}
		return minion.GetProperName();
	}

	// Token: 0x06006CC1 RID: 27841 RVA: 0x0028EAF0 File Offset: 0x0028CCF0
	private void OnRepeatClicked(IEmptyableCargo module)
	{
		module.AutoDeploy = !module.AutoDeploy;
		this.StyleRepeatButton(module);
	}

	// Token: 0x06006CC2 RID: 27842 RVA: 0x0028EB08 File Offset: 0x0028CD08
	private void OnDuplicantEntryClick(IListableOption option, object data)
	{
		MinionIdentity chosenDuplicant = (MinionIdentity)option;
		IEmptyableCargo emptyableCargo = (IEmptyableCargo)data;
		emptyableCargo.ChosenDuplicant = chosenDuplicant;
		HierarchyReferences hierarchyReferences = this.modulePanels[emptyableCargo];
		hierarchyReferences.GetReference<DropDown>("dropDown").selectedLabel.text = ((emptyableCargo.ChosenDuplicant != null) ? this.GetDuplicantRowName(emptyableCargo.ChosenDuplicant) : UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.SELECT_DUPLICANT.ToString());
		hierarchyReferences.GetReference<CrewPortrait>("selectedPortrait").SetIdentityObject(emptyableCargo.ChosenDuplicant, false);
		this.RefreshAll(null);
	}

	// Token: 0x06006CC3 RID: 27843 RVA: 0x0028EB90 File Offset: 0x0028CD90
	private void DropDownEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		MinionIdentity minionIdentity = (MinionIdentity)entry.entryData;
		entry.label.text = this.GetDuplicantRowName(minionIdentity);
		entry.portrait.SetIdentityObject(minionIdentity, false);
		bool flag = false;
		foreach (Ref<RocketModuleCluster> @ref in this.targetCraft.ModuleInterface.ClusterModules)
		{
			RocketModuleCluster rocketModuleCluster = @ref.Get();
			if (!(rocketModuleCluster == null))
			{
				IEmptyableCargo smi = rocketModuleCluster.GetSMI<IEmptyableCargo>();
				if (smi != null && !(((IEmptyableCargo)targetData).ChosenDuplicant == minionIdentity))
				{
					flag = (flag || smi.ChosenDuplicant == minionIdentity);
				}
			}
		}
		entry.button.isInteractable = !flag;
	}

	// Token: 0x06006CC4 RID: 27844 RVA: 0x0028EC60 File Offset: 0x0028CE60
	private void StyleRepeatButton(IEmptyableCargo module)
	{
		KButton reference = this.modulePanels[module].GetReference<KButton>("repeatButton");
		reference.bgImage.colorStyleSetting = (module.AutoDeploy ? this.repeatOn : this.repeatOff);
		reference.bgImage.ApplyColorStyleSetting();
	}

	// Token: 0x04004A28 RID: 18984
	private Clustercraft targetCraft;

	// Token: 0x04004A29 RID: 18985
	public GameObject moduleContentContainer;

	// Token: 0x04004A2A RID: 18986
	public GameObject modulePanelPrefab;

	// Token: 0x04004A2B RID: 18987
	public ColorStyleSetting repeatOff;

	// Token: 0x04004A2C RID: 18988
	public ColorStyleSetting repeatOn;

	// Token: 0x04004A2D RID: 18989
	private Dictionary<IEmptyableCargo, HierarchyReferences> modulePanels = new Dictionary<IEmptyableCargo, HierarchyReferences>();

	// Token: 0x04004A2E RID: 18990
	private List<int> refreshHandle = new List<int>();
}
