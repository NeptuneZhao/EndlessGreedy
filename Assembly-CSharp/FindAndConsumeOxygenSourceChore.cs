using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200043D RID: 1085
public class FindAndConsumeOxygenSourceChore : Chore<FindAndConsumeOxygenSourceChore.Instance>
{
	// Token: 0x0600171D RID: 5917 RVA: 0x0007CC38 File Offset: 0x0007AE38
	public FindAndConsumeOxygenSourceChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.FindOxygenSourceItem, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new FindAndConsumeOxygenSourceChore.Instance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(FindAndConsumeOxygenSourceChore.OxygenSourceItemIsNotNull, null);
	}

	// Token: 0x0600171E RID: 5918 RVA: 0x0007CC9C File Offset: 0x0007AE9C
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("FindAndConsumeOxygenSourceChore null context.consumer");
			return;
		}
		BionicOxygenTankMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicOxygenTankMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("FindAndConsumeOxygenSourceChore null BionicOxygenTankMonitor.Instance");
			return;
		}
		Pickupable closestOxygenSource = smi.GetClosestOxygenSource();
		if (closestOxygenSource == null)
		{
			global::Debug.LogError("FindAndConsumeOxygenSourceChore null oxygenSourceItem.gameObject");
			return;
		}
		base.smi.sm.oxygenSourceItem.Set(closestOxygenSource.gameObject, base.smi, false);
		base.smi.sm.amountRequested.Set(Mathf.Min(smi.SpaceAvailableInTank, closestOxygenSource.UnreservedAmount), base.smi, false);
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x0600171F RID: 5919 RVA: 0x0007CD7F File Offset: 0x0007AF7F
	private static string GetConsumePreAnimName(FindAndConsumeOxygenSourceChore.Instance smi)
	{
		if (smi.GetComponent<Navigator>().CurrentNavType != NavType.Ladder)
		{
			return "consume_canister_pre";
		}
		return "ladder_consume";
	}

	// Token: 0x06001720 RID: 5920 RVA: 0x0007CD9C File Offset: 0x0007AF9C
	private static string GetConsumeLoopAnimName(FindAndConsumeOxygenSourceChore.Instance smi)
	{
		if (smi.GetComponent<Navigator>().CurrentNavType != NavType.Ladder)
		{
			return "consume_canister_loop";
		}
		return "ladder_consume";
	}

	// Token: 0x06001721 RID: 5921 RVA: 0x0007CDB9 File Offset: 0x0007AFB9
	private static string GetConsumePstAnimName(FindAndConsumeOxygenSourceChore.Instance smi)
	{
		if (smi.GetComponent<Navigator>().CurrentNavType != NavType.Ladder)
		{
			return "consume_canister_pst";
		}
		return "ladder_consume";
	}

	// Token: 0x06001722 RID: 5922 RVA: 0x0007CDD8 File Offset: 0x0007AFD8
	public static void ExtractOxygenFromItem(FindAndConsumeOxygenSourceChore.Instance smi)
	{
		GameObject gameObject = smi.sm.pickedUpItem.Get(smi);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		if (component.Element.IsGas)
		{
			Storage[] components = smi.gameObject.GetComponents<Storage>();
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i] != smi.oxygenTankMonitor.storage)
				{
					List<GameObject> list = new List<GameObject>();
					components[i].Find(GameTags.Breathable, list);
					foreach (GameObject gameObject2 in list)
					{
						if (gameObject2 != null)
						{
							components[i].Transfer(gameObject2, smi.oxygenTankMonitor.storage, false, false);
							break;
						}
					}
				}
			}
			return;
		}
		SimHashes element = SimHashes.Oxygen;
		if (ElementLoader.GetElement(component.Element.sublimateId.CreateTag()).HasTag(GameTags.Breathable))
		{
			element = component.Element.sublimateId;
		}
		smi.oxygenTankMonitor.storage.AddGasChunk(element, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, true);
		Util.KDestroyGameObject(gameObject);
	}

	// Token: 0x06001723 RID: 5923 RVA: 0x0007CF24 File Offset: 0x0007B124
	public static void SetOverrideAnimSymbol(FindAndConsumeOxygenSourceChore.Instance smi, bool overriding)
	{
		string text = "object";
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component2 = smi.gameObject.GetComponent<SymbolOverrideController>();
		GameObject gameObject = smi.sm.pickedUpItem.Get(smi);
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
		KAnim.Build.Symbol symbolByIndex = gameObject.GetComponent<KBatchedAnimController>().CurrentAnim.animFile.build.GetSymbolByIndex(0U);
		component2.AddSymbolOverride(text, symbolByIndex, 0);
		component.SetSymbolVisiblity(text, true);
	}

	// Token: 0x06001724 RID: 5924 RVA: 0x0007CFE3 File Offset: 0x0007B1E3
	public static void TriggerOxygenItemLostSignal(FindAndConsumeOxygenSourceChore.Instance smi)
	{
		if (smi.oxygenTankMonitor != null)
		{
			smi.oxygenTankMonitor.sm.OxygenSourceItemLostSignal.Trigger(smi.oxygenTankMonitor);
		}
	}

	// Token: 0x04000D04 RID: 3332
	public const float LOOP_LENGTH = 4.333f;

	// Token: 0x04000D05 RID: 3333
	public static readonly Chore.Precondition OxygenSourceItemIsNotNull = new Chore.Precondition
	{
		id = "OxygenSourceIsNotNull",
		description = DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Pickupable closestOxygenSource = context.consumerState.consumer.GetSMI<BionicOxygenTankMonitor.Instance>().GetClosestOxygenSource();
			return closestOxygenSource != null && closestOxygenSource.UnreservedAmount > 0f;
		}
	};

	// Token: 0x020011BF RID: 4543
	public class States : GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore>
	{
		// Token: 0x060080FD RID: 33021 RVA: 0x00314ABC File Offset: 0x00312CBC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.dupe);
			this.fetch.InitializeStates(this.dupe, this.oxygenSourceItem, this.pickedUpItem, this.amountRequested, this.actualunits, this.install, null).OnTargetLost(this.oxygenSourceItem, this.oxygenSourceLost);
			this.install.Target(this.pickedUpItem).OnTargetLost(this.pickedUpItem, this.oxygenSourceLost).Target(this.dupe).DefaultState(this.install.pre).ToggleAnims("anim_bionic_kanim", 0f).Enter("Add Symbol Override", delegate(FindAndConsumeOxygenSourceChore.Instance smi)
			{
				FindAndConsumeOxygenSourceChore.SetOverrideAnimSymbol(smi, true);
			}).Exit("Revert Symbol Override", delegate(FindAndConsumeOxygenSourceChore.Instance smi)
			{
				FindAndConsumeOxygenSourceChore.SetOverrideAnimSymbol(smi, false);
			});
			this.install.pre.PlayAnim(new Func<FindAndConsumeOxygenSourceChore.Instance, string>(FindAndConsumeOxygenSourceChore.GetConsumePreAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.install.loop).ScheduleGoTo(3f, this.install.loop);
			this.install.loop.PlayAnim(new Func<FindAndConsumeOxygenSourceChore.Instance, string>(FindAndConsumeOxygenSourceChore.GetConsumeLoopAnimName), KAnim.PlayMode.Loop).ScheduleGoTo(4.333f, this.install.pst);
			this.install.pst.PlayAnim(new Func<FindAndConsumeOxygenSourceChore.Instance, string>(FindAndConsumeOxygenSourceChore.GetConsumePstAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3f, this.complete);
			this.complete.Enter(new StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State.Callback(FindAndConsumeOxygenSourceChore.ExtractOxygenFromItem)).ReturnSuccess();
			this.oxygenSourceLost.Target(this.dupe).Enter(new StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State.Callback(FindAndConsumeOxygenSourceChore.TriggerOxygenItemLostSignal)).ReturnFailure();
		}

		// Token: 0x0400613F RID: 24895
		public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.FetchSubState fetch;

		// Token: 0x04006140 RID: 24896
		public FindAndConsumeOxygenSourceChore.States.InstallState install;

		// Token: 0x04006141 RID: 24897
		public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State complete;

		// Token: 0x04006142 RID: 24898
		public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State oxygenSourceLost;

		// Token: 0x04006143 RID: 24899
		public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.TargetParameter dupe;

		// Token: 0x04006144 RID: 24900
		public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.TargetParameter oxygenSourceItem;

		// Token: 0x04006145 RID: 24901
		public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.TargetParameter pickedUpItem;

		// Token: 0x04006146 RID: 24902
		public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.FloatParameter actualunits;

		// Token: 0x04006147 RID: 24903
		public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.FloatParameter amountRequested;

		// Token: 0x020023C2 RID: 9154
		public class InstallState : GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State
		{
			// Token: 0x04009FA9 RID: 40873
			public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State pre;

			// Token: 0x04009FAA RID: 40874
			public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State loop;

			// Token: 0x04009FAB RID: 40875
			public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State pst;
		}
	}

	// Token: 0x020011C0 RID: 4544
	public class Instance : GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.GameInstance
	{
		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x060080FF RID: 33023 RVA: 0x00314CBA File Offset: 0x00312EBA
		public BionicOxygenTankMonitor.Instance oxygenTankMonitor
		{
			get
			{
				return base.sm.dupe.Get(this).GetSMI<BionicOxygenTankMonitor.Instance>();
			}
		}

		// Token: 0x06008100 RID: 33024 RVA: 0x00314CD2 File Offset: 0x00312ED2
		public Instance(FindAndConsumeOxygenSourceChore master, GameObject duplicant) : base(master)
		{
		}
	}
}
