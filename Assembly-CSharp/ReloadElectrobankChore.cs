using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000450 RID: 1104
public class ReloadElectrobankChore : Chore<ReloadElectrobankChore.Instance>
{
	// Token: 0x06001750 RID: 5968 RVA: 0x0007E434 File Offset: 0x0007C634
	public ReloadElectrobankChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.ReloadElectrobank, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new ReloadElectrobankChore.Instance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ReloadElectrobankChore.ElectrobankIsNotNull, null);
	}

	// Token: 0x06001751 RID: 5969 RVA: 0x0007E498 File Offset: 0x0007C698
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null context.consumer");
			return;
		}
		BionicBatteryMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicBatteryMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null RationMonitor.Instance");
			return;
		}
		Electrobank closestElectrobank = smi.GetClosestElectrobank();
		if (closestElectrobank == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null electrobank.gameObject");
			return;
		}
		base.smi.cachedElectrobankSourcePrefabRef = Assets.GetPrefab(closestElectrobank.PrefabID());
		base.smi.sm.electrobankSource.Set(closestElectrobank.gameObject, base.smi, false);
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x06001752 RID: 5970 RVA: 0x0007E563 File Offset: 0x0007C763
	private static string GetConsumePreAnimName(ReloadElectrobankChore.Instance smi)
	{
		if (smi.GetComponent<Navigator>().CurrentNavType != NavType.Ladder)
		{
			return "consume_pre";
		}
		return "ladder_consume";
	}

	// Token: 0x06001753 RID: 5971 RVA: 0x0007E580 File Offset: 0x0007C780
	private static string GetConsumeLoopAnimName(ReloadElectrobankChore.Instance smi)
	{
		if (smi.GetComponent<Navigator>().CurrentNavType != NavType.Ladder)
		{
			return "consume_loop";
		}
		return "ladder_consume";
	}

	// Token: 0x06001754 RID: 5972 RVA: 0x0007E59D File Offset: 0x0007C79D
	private static string GetConsumePstAnimName(ReloadElectrobankChore.Instance smi)
	{
		if (smi.GetComponent<Navigator>().CurrentNavType != NavType.Ladder)
		{
			return "consume_pst";
		}
		return "ladder_consume";
	}

	// Token: 0x06001755 RID: 5973 RVA: 0x0007E5BC File Offset: 0x0007C7BC
	public static void InstallElectrobank(ReloadElectrobankChore.Instance smi)
	{
		Storage[] components = smi.gameObject.GetComponents<Storage>();
		for (int i = 0; i < components.Length; i++)
		{
			if (components[i] != smi.batteryMonitor.storage && components[i].FindFirst(GameTags.ChargedPortableBattery) != null)
			{
				components[i].Transfer(smi.batteryMonitor.storage, false, false);
				break;
			}
		}
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_BionicBattery, true);
	}

	// Token: 0x06001756 RID: 5974 RVA: 0x0007E634 File Offset: 0x0007C834
	public static void SetOverrideAnimSymbol(ReloadElectrobankChore.Instance smi, bool overriding)
	{
		string text = "object";
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component2 = smi.gameObject.GetComponent<SymbolOverrideController>();
		GameObject gameObject = smi.sm.pickedUpElectrobank.Get(smi);
		if (gameObject != null)
		{
			KBatchedAnimTracker component3 = gameObject.GetComponent<KBatchedAnimTracker>();
			if (component3 != null)
			{
				component3.enabled = !overriding;
			}
			Storage.MakeItemInvisible(gameObject, overriding, false);
		}
		if (!overriding)
		{
			component2.RemoveSymbolOverride(text, 0);
			component.SetSymbolVisiblity(text, false);
			return;
		}
		KAnim.Build.Symbol symbolByIndex = ((gameObject != null) ? gameObject.GetComponent<KBatchedAnimController>() : smi.cachedElectrobankSourcePrefabRef.GetComponent<KBatchedAnimController>()).AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
		component2.AddSymbolOverride(text, symbolByIndex, 0);
		component.SetSymbolVisiblity(text, true);
	}

	// Token: 0x04000D14 RID: 3348
	public const float LOOP_LENGTH = 4.333f;

	// Token: 0x04000D15 RID: 3349
	public static readonly Chore.Precondition ElectrobankIsNotNull = new Chore.Precondition
	{
		id = "ElectrobankIsNotNull",
		description = DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return null != context.consumerState.consumer.GetSMI<BionicBatteryMonitor.Instance>().GetClosestElectrobank();
		}
	};

	// Token: 0x020011ED RID: 4589
	public class States : GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore>
	{
		// Token: 0x0600819C RID: 33180 RVA: 0x003183BC File Offset: 0x003165BC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.dupe);
			this.fetch.InitializeStates(this.dupe, this.electrobankSource, this.pickedUpElectrobank, this.amountRequested, this.actualunits, this.install, null).OnTargetLost(this.electrobankSource, this.electrobankLost);
			this.install.DefaultState(this.install.pre).ToggleAnims("anim_bionic_kanim", 0f).Enter("Add Symbol Override", delegate(ReloadElectrobankChore.Instance smi)
			{
				ReloadElectrobankChore.SetOverrideAnimSymbol(smi, true);
			}).Exit("Revert Symbol Override", delegate(ReloadElectrobankChore.Instance smi)
			{
				ReloadElectrobankChore.SetOverrideAnimSymbol(smi, false);
			});
			this.install.pre.PlayAnim(new Func<ReloadElectrobankChore.Instance, string>(ReloadElectrobankChore.GetConsumePreAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.install.loop).ScheduleGoTo(3f, this.install.loop);
			this.install.loop.PlayAnim(new Func<ReloadElectrobankChore.Instance, string>(ReloadElectrobankChore.GetConsumeLoopAnimName), KAnim.PlayMode.Loop).ScheduleGoTo(4.333f, this.install.pst);
			this.install.pst.PlayAnim(new Func<ReloadElectrobankChore.Instance, string>(ReloadElectrobankChore.GetConsumePstAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3f, this.complete);
			this.complete.Enter(new StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback(ReloadElectrobankChore.InstallElectrobank)).ReturnSuccess();
			this.electrobankLost.Target(this.dupe).TriggerOnEnter(GameHashes.TargetElectrobankLost, null).ReturnFailure();
		}

		// Token: 0x040061D1 RID: 25041
		public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FetchSubState fetch;

		// Token: 0x040061D2 RID: 25042
		public ReloadElectrobankChore.States.InstallState install;

		// Token: 0x040061D3 RID: 25043
		public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State complete;

		// Token: 0x040061D4 RID: 25044
		public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State electrobankLost;

		// Token: 0x040061D5 RID: 25045
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter dupe;

		// Token: 0x040061D6 RID: 25046
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter electrobankSource;

		// Token: 0x040061D7 RID: 25047
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter pickedUpElectrobank;

		// Token: 0x040061D8 RID: 25048
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter messstation;

		// Token: 0x040061D9 RID: 25049
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FloatParameter actualunits;

		// Token: 0x040061DA RID: 25050
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FloatParameter amountRequested = new StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FloatParameter(1f);

		// Token: 0x020023E0 RID: 9184
		public class InstallState : GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State
		{
			// Token: 0x0400A027 RID: 40999
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State pre;

			// Token: 0x0400A028 RID: 41000
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State loop;

			// Token: 0x0400A029 RID: 41001
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State pst;
		}
	}

	// Token: 0x020011EE RID: 4590
	public class Instance : GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.GameInstance
	{
		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x0600819E RID: 33182 RVA: 0x0031859D File Offset: 0x0031679D
		public BionicBatteryMonitor.Instance batteryMonitor
		{
			get
			{
				return base.sm.dupe.Get(this).GetSMI<BionicBatteryMonitor.Instance>();
			}
		}

		// Token: 0x0600819F RID: 33183 RVA: 0x003185B5 File Offset: 0x003167B5
		public Instance(ReloadElectrobankChore master, GameObject duplicant) : base(master)
		{
		}

		// Token: 0x040061DB RID: 25051
		public GameObject cachedElectrobankSourcePrefabRef;
	}
}
