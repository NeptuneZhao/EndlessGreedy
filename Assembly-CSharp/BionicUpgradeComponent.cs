using System;
using STRINGS;

// Token: 0x02000660 RID: 1632
public class BionicUpgradeComponent : Assignable
{
	// Token: 0x170001EF RID: 495
	// (get) Token: 0x0600283D RID: 10301 RVA: 0x000E45B0 File Offset: 0x000E27B0
	// (set) Token: 0x0600283C RID: 10300 RVA: 0x000E45A7 File Offset: 0x000E27A7
	public BionicUpgradeComponent.IWattageController WattageController { get; private set; }

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x0600283E RID: 10302 RVA: 0x000E45B8 File Offset: 0x000E27B8
	public float CurrentWattage
	{
		get
		{
			if (!this.HasWattageController)
			{
				return 0f;
			}
			return this.WattageController.GetCurrentWattageCost();
		}
	}

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x0600283F RID: 10303 RVA: 0x000E45D3 File Offset: 0x000E27D3
	public string CurrentWattageName
	{
		get
		{
			if (!this.HasWattageController)
			{
				return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.STANDARD_INACTIVE_TEMPLATE, this.GetProperName(), GameUtil.GetFormattedWattage(this.PotentialWattage, GameUtil.WattageFormatterUnit.Automatic, true));
			}
			return this.WattageController.GetCurrentWattageCostName();
		}
	}

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x06002840 RID: 10304 RVA: 0x000E460B File Offset: 0x000E280B
	public bool HasWattageController
	{
		get
		{
			return this.WattageController != null;
		}
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06002841 RID: 10305 RVA: 0x000E4616 File Offset: 0x000E2816
	public float PotentialWattage
	{
		get
		{
			return this.data.WattageCost;
		}
	}

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06002842 RID: 10306 RVA: 0x000E4623 File Offset: 0x000E2823
	public BionicUpgradeComponentConfig.RarityType Rarity
	{
		get
		{
			return this.data.rarity;
		}
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x06002843 RID: 10307 RVA: 0x000E4630 File Offset: 0x000E2830
	public Func<StateMachine.Instance, StateMachine.Instance> StateMachine
	{
		get
		{
			return this.data.stateMachine;
		}
	}

	// Token: 0x06002844 RID: 10308 RVA: 0x000E463D File Offset: 0x000E283D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.data = BionicUpgradeComponentConfig.UpgradesData[base.gameObject.PrefabID()];
		base.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.AssignablePrecondition_OnlyOnBionics));
	}

	// Token: 0x06002845 RID: 10309 RVA: 0x000E4672 File Offset: 0x000E2872
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06002846 RID: 10310 RVA: 0x000E467A File Offset: 0x000E287A
	public void InformOfWattageChanged()
	{
		System.Action onWattageCostChanged = this.OnWattageCostChanged;
		if (onWattageCostChanged == null)
		{
			return;
		}
		onWattageCostChanged();
	}

	// Token: 0x06002847 RID: 10311 RVA: 0x000E468C File Offset: 0x000E288C
	public void SetWattageController(BionicUpgradeComponent.IWattageController wattageController)
	{
		this.WattageController = wattageController;
	}

	// Token: 0x06002848 RID: 10312 RVA: 0x000E4698 File Offset: 0x000E2898
	public override void Assign(IAssignableIdentity new_assignee)
	{
		AssignableSlotInstance specificSlotInstance = null;
		if (new_assignee == this.assignee)
		{
			return;
		}
		if (new_assignee != this.assignee && (new_assignee is MinionIdentity || new_assignee is StoredMinionIdentity || new_assignee is MinionAssignablesProxy))
		{
			Ownables soleOwner = new_assignee.GetSoleOwner();
			if (soleOwner != null)
			{
				BionicUpgradesMonitor.Instance smi = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetSMI<BionicUpgradesMonitor.Instance>();
				if (smi != null)
				{
					BionicUpgradesMonitor.UpgradeComponentSlot firstEmptySlot = smi.GetFirstEmptySlot();
					if (firstEmptySlot != null)
					{
						specificSlotInstance = firstEmptySlot.GetAssignableSlotInstance();
					}
				}
			}
		}
		base.Assign(new_assignee, specificSlotInstance);
		base.Trigger(1980521255, null);
	}

	// Token: 0x06002849 RID: 10313 RVA: 0x000E471B File Offset: 0x000E291B
	public override void Unassign()
	{
		base.Unassign();
		base.Trigger(1980521255, null);
	}

	// Token: 0x0600284A RID: 10314 RVA: 0x000E4730 File Offset: 0x000E2930
	private bool AssignablePrecondition_OnlyOnBionics(MinionAssignablesProxy worker)
	{
		bool result = false;
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			result = (minionIdentity.GetSMI<BionicUpgradesMonitor.Instance>() != null);
		}
		return result;
	}

	// Token: 0x04001733 RID: 5939
	private BionicUpgradeComponentConfig.BionicUpgradeData data;

	// Token: 0x04001734 RID: 5940
	public System.Action OnWattageCostChanged;

	// Token: 0x0200143A RID: 5178
	public interface IWattageController
	{
		// Token: 0x060089D0 RID: 35280
		float GetCurrentWattageCost();

		// Token: 0x060089D1 RID: 35281
		string GetCurrentWattageCostName();
	}
}
