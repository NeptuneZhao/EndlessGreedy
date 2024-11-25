using System;
using UnityEngine;

// Token: 0x02000451 RID: 1105
public class RemoveDischargedElectrobankChore : Chore<RemoveDischargedElectrobankChore.Instance>
{
	// Token: 0x06001758 RID: 5976 RVA: 0x0007E75C File Offset: 0x0007C95C
	public RemoveDischargedElectrobankChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.UnloadElectrobank, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new RemoveDischargedElectrobankChore.Instance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x06001759 RID: 5977 RVA: 0x0007E7B4 File Offset: 0x0007C9B4
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("RemoveDischargedElectrobankChore null context.consumer");
			return;
		}
		BionicBatteryMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicBatteryMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("RemoveDischargedElectrobankChore null RationMonitor.Instance");
			return;
		}
		GameObject firstDischargedElectrobankInInventory = smi.GetFirstDischargedElectrobankInInventory();
		if (firstDischargedElectrobankInInventory == null)
		{
			global::Debug.LogError("RemoveDischargedElectrobankChore dischargedElectrobank is null");
			return;
		}
		base.smi.sm.dischargedElectrobank.Set(firstDischargedElectrobankInInventory, base.smi, false);
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x0600175A RID: 5978 RVA: 0x0007E864 File Offset: 0x0007CA64
	private static string GetAnimName(RemoveDischargedElectrobankChore.Instance smi)
	{
		if (smi.GetComponent<Navigator>().CurrentNavType != NavType.Ladder)
		{
			return "discharge";
		}
		return "ladder_discharge";
	}

	// Token: 0x0600175B RID: 5979 RVA: 0x0007E884 File Offset: 0x0007CA84
	public static void SetOverrideAnimSymbol(RemoveDischargedElectrobankChore.Instance smi, bool overriding)
	{
		string text = "object";
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component2 = smi.gameObject.GetComponent<SymbolOverrideController>();
		GameObject gameObject = smi.sm.dischargedElectrobank.Get(smi);
		if (gameObject != null)
		{
			KBatchedAnimTracker component3 = gameObject.GetComponent<KBatchedAnimTracker>();
			if (component3 != null)
			{
				component3.enabled = !overriding;
				Storage.MakeItemInvisible(gameObject, overriding, false);
			}
		}
		if (!overriding)
		{
			component2.RemoveSymbolOverride(text, 0);
			component.SetSymbolVisiblity(text, false);
			return;
		}
		KAnim.Build.Symbol symbolByIndex = gameObject.GetComponent<KBatchedAnimController>().CurrentAnim.animFile.build.GetSymbolByIndex(0U);
		component2.AddSymbolOverride(text, symbolByIndex, 0);
		component.SetSymbolVisiblity(text, true);
	}

	// Token: 0x0600175C RID: 5980 RVA: 0x0007E944 File Offset: 0x0007CB44
	public static void RemoveDepletedElectrobank(RemoveDischargedElectrobankChore.Instance smi)
	{
		GameObject go = smi.sm.dischargedElectrobank.Get(smi);
		smi.batteryMonitor.storage.Drop(go, true);
	}

	// Token: 0x020011F0 RID: 4592
	public class States : GameStateMachine<RemoveDischargedElectrobankChore.States, RemoveDischargedElectrobankChore.Instance, RemoveDischargedElectrobankChore>
	{
		// Token: 0x060081A3 RID: 33187 RVA: 0x003185F0 File Offset: 0x003167F0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.working;
			base.Target(this.dupe);
			this.working.ToggleAnims("anim_bionic_kanim", 0f).PlayAnim(new Func<RemoveDischargedElectrobankChore.Instance, string>(RemoveDischargedElectrobankChore.GetAnimName), KAnim.PlayMode.Once).Enter("Add Symbol Override", delegate(RemoveDischargedElectrobankChore.Instance smi)
			{
				RemoveDischargedElectrobankChore.SetOverrideAnimSymbol(smi, true);
			}).Exit("Revert Symbol Override", delegate(RemoveDischargedElectrobankChore.Instance smi)
			{
				RemoveDischargedElectrobankChore.SetOverrideAnimSymbol(smi, false);
			}).OnAnimQueueComplete(this.complete);
			this.complete.Enter(new StateMachine<RemoveDischargedElectrobankChore.States, RemoveDischargedElectrobankChore.Instance, RemoveDischargedElectrobankChore, object>.State.Callback(RemoveDischargedElectrobankChore.RemoveDepletedElectrobank)).ReturnSuccess();
		}

		// Token: 0x040061DD RID: 25053
		public GameStateMachine<RemoveDischargedElectrobankChore.States, RemoveDischargedElectrobankChore.Instance, RemoveDischargedElectrobankChore, object>.State working;

		// Token: 0x040061DE RID: 25054
		public GameStateMachine<RemoveDischargedElectrobankChore.States, RemoveDischargedElectrobankChore.Instance, RemoveDischargedElectrobankChore, object>.State complete;

		// Token: 0x040061DF RID: 25055
		public StateMachine<RemoveDischargedElectrobankChore.States, RemoveDischargedElectrobankChore.Instance, RemoveDischargedElectrobankChore, object>.TargetParameter dupe;

		// Token: 0x040061E0 RID: 25056
		public StateMachine<RemoveDischargedElectrobankChore.States, RemoveDischargedElectrobankChore.Instance, RemoveDischargedElectrobankChore, object>.TargetParameter dischargedElectrobank;
	}

	// Token: 0x020011F1 RID: 4593
	public class Instance : GameStateMachine<RemoveDischargedElectrobankChore.States, RemoveDischargedElectrobankChore.Instance, RemoveDischargedElectrobankChore, object>.GameInstance
	{
		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x060081A5 RID: 33189 RVA: 0x003186BB File Offset: 0x003168BB
		public BionicBatteryMonitor.Instance batteryMonitor
		{
			get
			{
				return base.sm.dupe.Get(this).GetSMI<BionicBatteryMonitor.Instance>();
			}
		}

		// Token: 0x060081A6 RID: 33190 RVA: 0x003186D3 File Offset: 0x003168D3
		public Instance(RemoveDischargedElectrobankChore master, GameObject duplicant) : base(master)
		{
		}
	}
}
