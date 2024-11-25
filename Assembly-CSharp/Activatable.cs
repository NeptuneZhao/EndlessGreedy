using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000524 RID: 1316
[AddComponentMenu("KMonoBehaviour/Workable/Activatable")]
public class Activatable : Workable, ISidescreenButtonControl
{
	// Token: 0x17000115 RID: 277
	// (get) Token: 0x06001D82 RID: 7554 RVA: 0x000A43AF File Offset: 0x000A25AF
	public bool IsActivated
	{
		get
		{
			return this.activated;
		}
	}

	// Token: 0x06001D83 RID: 7555 RVA: 0x000A43B7 File Offset: 0x000A25B7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06001D84 RID: 7556 RVA: 0x000A43BF File Offset: 0x000A25BF
	protected override void OnSpawn()
	{
		this.UpdateFlag();
		if (this.awaitingActivation && this.activateChore == null)
		{
			this.CreateChore();
		}
	}

	// Token: 0x06001D85 RID: 7557 RVA: 0x000A43DD File Offset: 0x000A25DD
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.activated = true;
		if (this.onActivate != null)
		{
			this.onActivate();
		}
		this.awaitingActivation = false;
		this.UpdateFlag();
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCompleteWork(worker);
	}

	// Token: 0x06001D86 RID: 7558 RVA: 0x000A4418 File Offset: 0x000A2618
	private void UpdateFlag()
	{
		base.GetComponent<Operational>().SetFlag(this.Required ? Activatable.activatedFlagRequirement : Activatable.activatedFlagFunctional, this.activated);
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.DuplicantActivationRequired, !this.activated, null);
		base.Trigger(-1909216579, this.IsActivated);
	}

	// Token: 0x06001D87 RID: 7559 RVA: 0x000A4488 File Offset: 0x000A2688
	private void CreateChore()
	{
		if (this.activateChore != null)
		{
			return;
		}
		Prioritizable.AddRef(base.gameObject);
		this.activateChore = new WorkChore<Activatable>(Db.Get().ChoreTypes.Toggle, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		if (!string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			this.shouldShowSkillPerkStatusItem = true;
			this.requireMinionToWork = true;
			this.UpdateStatusItem(null);
		}
	}

	// Token: 0x06001D88 RID: 7560 RVA: 0x000A44F7 File Offset: 0x000A26F7
	private void CancelChore()
	{
		if (this.activateChore == null)
		{
			return;
		}
		this.activateChore.Cancel("User cancelled");
		this.activateChore = null;
	}

	// Token: 0x06001D89 RID: 7561 RVA: 0x000A4519 File Offset: 0x000A2719
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06001D8A RID: 7562 RVA: 0x000A451C File Offset: 0x000A271C
	public string SidescreenButtonText
	{
		get
		{
			if (this.activateChore != null)
			{
				return this.textOverride.IsValid ? this.textOverride.CancelText : UI.USERMENUACTIONS.ACTIVATEBUILDING.ACTIVATE_CANCEL;
			}
			return this.textOverride.IsValid ? this.textOverride.Text : UI.USERMENUACTIONS.ACTIVATEBUILDING.ACTIVATE;
		}
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06001D8B RID: 7563 RVA: 0x000A457C File Offset: 0x000A277C
	public string SidescreenButtonTooltip
	{
		get
		{
			if (this.activateChore != null)
			{
				return this.textOverride.IsValid ? this.textOverride.CancelToolTip : UI.USERMENUACTIONS.ACTIVATEBUILDING.TOOLTIP_CANCEL;
			}
			return this.textOverride.IsValid ? this.textOverride.ToolTip : UI.USERMENUACTIONS.ACTIVATEBUILDING.TOOLTIP_ACTIVATE;
		}
	}

	// Token: 0x06001D8C RID: 7564 RVA: 0x000A45DA File Offset: 0x000A27DA
	public bool SidescreenEnabled()
	{
		return !this.activated;
	}

	// Token: 0x06001D8D RID: 7565 RVA: 0x000A45E5 File Offset: 0x000A27E5
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		this.textOverride = text;
	}

	// Token: 0x06001D8E RID: 7566 RVA: 0x000A45EE File Offset: 0x000A27EE
	public void OnSidescreenButtonPressed()
	{
		if (this.activateChore == null)
		{
			this.CreateChore();
		}
		else
		{
			this.CancelChore();
		}
		this.awaitingActivation = (this.activateChore != null);
	}

	// Token: 0x06001D8F RID: 7567 RVA: 0x000A4615 File Offset: 0x000A2815
	public bool SidescreenButtonInteractable()
	{
		return !this.activated;
	}

	// Token: 0x06001D90 RID: 7568 RVA: 0x000A4620 File Offset: 0x000A2820
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x04001096 RID: 4246
	public bool Required = true;

	// Token: 0x04001097 RID: 4247
	private static readonly Operational.Flag activatedFlagRequirement = new Operational.Flag("activated", Operational.Flag.Type.Requirement);

	// Token: 0x04001098 RID: 4248
	private static readonly Operational.Flag activatedFlagFunctional = new Operational.Flag("activated", Operational.Flag.Type.Functional);

	// Token: 0x04001099 RID: 4249
	[Serialize]
	private bool activated;

	// Token: 0x0400109A RID: 4250
	[Serialize]
	private bool awaitingActivation;

	// Token: 0x0400109B RID: 4251
	private Guid statusItem;

	// Token: 0x0400109C RID: 4252
	private Chore activateChore;

	// Token: 0x0400109D RID: 4253
	public System.Action onActivate;

	// Token: 0x0400109E RID: 4254
	[SerializeField]
	private ButtonMenuTextOverride textOverride;
}
