using System;
using UnityEngine;

// Token: 0x02000455 RID: 1109
public class SeekAndInstallBionicUpgradeChore : Chore<SeekAndInstallBionicUpgradeChore.Instance>
{
	// Token: 0x06001769 RID: 5993 RVA: 0x0007EE60 File Offset: 0x0007D060
	public SeekAndInstallBionicUpgradeChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.SeekAndInstallUpgrade, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new SeekAndInstallBionicUpgradeChore.Instance(this, target.gameObject);
		BionicUpgradeComponent assignedUpgradeComponent = target.gameObject.GetSMI<BionicUpgradesMonitor.Instance>().GetAnyReachableAssignedSlot().assignedUpgradeComponent;
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, assignedUpgradeComponent.GetComponent<Pickupable>());
		this.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, assignedUpgradeComponent);
	}

	// Token: 0x0600176A RID: 5994 RVA: 0x0007EEF8 File Offset: 0x0007D0F8
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("SeekAndInstallBionicUpgradeChore null context.consumer");
			return;
		}
		BionicUpgradesMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicUpgradesMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("SeekAndInstallBionicUpgradeChore null BionicUpgradesMonitor.Instance");
			return;
		}
		BionicUpgradesMonitor.UpgradeComponentSlot anyReachableAssignedSlot = smi.GetAnyReachableAssignedSlot();
		BionicUpgradeComponent bionicUpgradeComponent = (anyReachableAssignedSlot == null) ? null : anyReachableAssignedSlot.assignedUpgradeComponent;
		if (bionicUpgradeComponent == null)
		{
			global::Debug.LogError("SeekAndInstallBionicUpgradeChore null upgradeComponent.gameObject");
			return;
		}
		base.smi.sm.initialUpgradeComponent.Set(bionicUpgradeComponent.gameObject, base.smi, false);
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x0600176B RID: 5995 RVA: 0x0007EFBC File Offset: 0x0007D1BC
	public static void SetOverrideAnimSymbol(SeekAndInstallBionicUpgradeChore.Instance smi, bool overriding)
	{
		string text = "object";
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component2 = smi.gameObject.GetComponent<SymbolOverrideController>();
		GameObject gameObject = smi.sm.pickedUpgrade.Get(smi);
		if (gameObject != null)
		{
			gameObject.GetComponent<KBatchedAnimTracker>().enabled = !overriding;
			Storage.MakeItemInvisible(gameObject, overriding, false);
		}
		if (!overriding)
		{
			component2.RemoveSymbolOverride(text, 0);
			component.SetSymbolVisiblity(text, false);
			return;
		}
		string animStateName = BionicUpgradeComponentConfig.UpgradesData[gameObject.PrefabID()].animStateName;
		KAnim.Build.Symbol symbol = gameObject.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol(animStateName);
		component2.AddSymbolOverride(text, symbol, 0);
		component.SetSymbolVisiblity(text, true);
	}

	// Token: 0x0600176C RID: 5996 RVA: 0x0007F08C File Offset: 0x0007D28C
	public static bool IsBionicUpgradeAssignedTo(GameObject bionicUpgradeGameObject, GameObject ownerInQuestion)
	{
		Assignable component = bionicUpgradeGameObject.GetComponent<BionicUpgradeComponent>();
		IAssignableIdentity component2 = ownerInQuestion.GetComponent<IAssignableIdentity>();
		return component.IsAssignedTo(component2);
	}

	// Token: 0x0600176D RID: 5997 RVA: 0x0007F0AC File Offset: 0x0007D2AC
	public static void InstallUpgrade(SeekAndInstallBionicUpgradeChore.Instance smi)
	{
		Storage storage = smi.gameObject.GetComponents<Storage>().FindFirst((Storage s) => s.storageID == GameTags.StoragesIds.DefaultStorage);
		GameObject gameObject = storage.FindFirst(GameTags.BionicUpgrade);
		if (gameObject != null)
		{
			BionicUpgradeComponent component = gameObject.GetComponent<BionicUpgradeComponent>();
			storage.Remove(component.gameObject, true);
			smi.upgradeMonitor.InstallUpgrade(component);
		}
	}

	// Token: 0x020011FA RID: 4602
	public class States : GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore>
	{
		// Token: 0x060081C7 RID: 33223 RVA: 0x003194AC File Offset: 0x003176AC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.dupe);
			this.fetch.InitializeStates(this.dupe, this.initialUpgradeComponent, this.pickedUpgrade, this.amountRequested, this.actualunits, this.install, null).Target(this.initialUpgradeComponent).EventHandlerTransition(GameHashes.AssigneeChanged, null, (SeekAndInstallBionicUpgradeChore.Instance smi, object obj) => !SeekAndInstallBionicUpgradeChore.IsBionicUpgradeAssignedTo(smi.sm.initialUpgradeComponent.Get(smi), smi.gameObject));
			this.install.Target(this.dupe).ToggleAnims("anim_bionic_booster_installation_kanim", 0f).PlayAnim("installation", KAnim.PlayMode.Once).Enter(delegate(SeekAndInstallBionicUpgradeChore.Instance smi)
			{
				SeekAndInstallBionicUpgradeChore.SetOverrideAnimSymbol(smi, true);
			}).Exit(delegate(SeekAndInstallBionicUpgradeChore.Instance smi)
			{
				SeekAndInstallBionicUpgradeChore.SetOverrideAnimSymbol(smi, false);
			}).OnAnimQueueComplete(this.complete).ScheduleGoTo(10f, this.complete).Target(this.pickedUpgrade).EventHandlerTransition(GameHashes.AssigneeChanged, null, (SeekAndInstallBionicUpgradeChore.Instance smi, object obj) => !SeekAndInstallBionicUpgradeChore.IsBionicUpgradeAssignedTo(smi.sm.pickedUpgrade.Get(smi), smi.gameObject));
			this.complete.Target(this.dupe).Enter(new StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State.Callback(SeekAndInstallBionicUpgradeChore.InstallUpgrade)).ReturnSuccess();
		}

		// Token: 0x040061F6 RID: 25078
		public GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FetchSubState fetch;

		// Token: 0x040061F7 RID: 25079
		public GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State install;

		// Token: 0x040061F8 RID: 25080
		public GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State complete;

		// Token: 0x040061F9 RID: 25081
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.TargetParameter dupe;

		// Token: 0x040061FA RID: 25082
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.TargetParameter initialUpgradeComponent;

		// Token: 0x040061FB RID: 25083
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.TargetParameter pickedUpgrade;

		// Token: 0x040061FC RID: 25084
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FloatParameter actualunits;

		// Token: 0x040061FD RID: 25085
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FloatParameter amountRequested = new StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FloatParameter(1f);
	}

	// Token: 0x020011FB RID: 4603
	public class Instance : GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.GameInstance
	{
		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x060081C9 RID: 33225 RVA: 0x00319637 File Offset: 0x00317837
		public BionicUpgradesMonitor.Instance upgradeMonitor
		{
			get
			{
				return base.sm.dupe.Get(this).GetSMI<BionicUpgradesMonitor.Instance>();
			}
		}

		// Token: 0x060081CA RID: 33226 RVA: 0x0031964F File Offset: 0x0031784F
		public Instance(SeekAndInstallBionicUpgradeChore master, GameObject duplicant) : base(master)
		{
		}
	}
}
