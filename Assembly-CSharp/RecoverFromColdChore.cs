using System;
using UnityEngine;

// Token: 0x0200044E RID: 1102
public class RecoverFromColdChore : Chore<RecoverFromColdChore.Instance>
{
	// Token: 0x0600174E RID: 5966 RVA: 0x0007E31C File Offset: 0x0007C51C
	public RecoverFromColdChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.RecoverWarmth, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new RecoverFromColdChore.Instance(this, target.gameObject);
		ColdImmunityMonitor.Instance coldImmunityMonitor = target.gameObject.GetSMI<ColdImmunityMonitor.Instance>();
		Func<int> data = () => coldImmunityMonitor.WarmUpCell;
		this.AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCell, data);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x020011E7 RID: 4583
	public class States : GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore>
	{
		// Token: 0x06008178 RID: 33144 RVA: 0x00317888 File Offset: 0x00315A88
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.entityRecovering);
			this.root.Enter("CreateLocator", delegate(RecoverFromColdChore.Instance smi)
			{
				smi.CreateLocator();
			}).Enter("UpdateImmunityProvider", delegate(RecoverFromColdChore.Instance smi)
			{
				smi.UpdateImmunityProvider();
			}).Exit("DestroyLocator", delegate(RecoverFromColdChore.Instance smi)
			{
				smi.DestroyLocator();
			}).Update("UpdateLocator", delegate(RecoverFromColdChore.Instance smi, float dt)
			{
				smi.UpdateLocator();
			}, UpdateRate.SIM_200ms, true).Update("UpdateColdImmunityProvider", delegate(RecoverFromColdChore.Instance smi, float dt)
			{
				smi.UpdateImmunityProvider();
			}, UpdateRate.SIM_200ms, true);
			this.approach.InitializeStates(this.entityRecovering, this.locator, this.recover, null, null, null);
			this.recover.OnTargetLost(this.coldImmunityProvider, null).ToggleAnims(new Func<RecoverFromColdChore.Instance, HashedString>(RecoverFromColdChore.States.GetAnimFileName)).DefaultState(this.recover.pre).ToggleTag(GameTags.RecoveringWarmnth);
			this.recover.pre.Face(this.coldImmunityProvider, 0f).PlayAnim(new Func<RecoverFromColdChore.Instance, string>(RecoverFromColdChore.States.GetPreAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover.loop);
			this.recover.loop.PlayAnim(new Func<RecoverFromColdChore.Instance, string>(RecoverFromColdChore.States.GetLoopAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover.pst);
			this.recover.pst.QueueAnim(new Func<RecoverFromColdChore.Instance, string>(RecoverFromColdChore.States.GetPstAnimName), false, null).OnAnimQueueComplete(this.complete);
			this.complete.DefaultState(this.complete.evaluate);
			this.complete.evaluate.EnterTransition(this.complete.success, new StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.Transition.ConditionCallback(RecoverFromColdChore.States.IsImmunityProviderStillValid)).EnterTransition(this.complete.fail, GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.Not(new StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.Transition.ConditionCallback(RecoverFromColdChore.States.IsImmunityProviderStillValid)));
			this.complete.success.Enter(new StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State.Callback(RecoverFromColdChore.States.ApplyColdImmunityEffect)).ReturnSuccess();
			this.complete.fail.ReturnFailure();
		}

		// Token: 0x06008179 RID: 33145 RVA: 0x00317B0C File Offset: 0x00315D0C
		public static bool IsImmunityProviderStillValid(RecoverFromColdChore.Instance smi)
		{
			ColdImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			return lastKnownImmunityProvider != null && lastKnownImmunityProvider.CanBeUsed;
		}

		// Token: 0x0600817A RID: 33146 RVA: 0x00317B2C File Offset: 0x00315D2C
		public static void ApplyColdImmunityEffect(RecoverFromColdChore.Instance smi)
		{
			ColdImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			if (lastKnownImmunityProvider != null)
			{
				lastKnownImmunityProvider.ApplyImmunityEffect(smi.gameObject, true);
			}
		}

		// Token: 0x0600817B RID: 33147 RVA: 0x00317B50 File Offset: 0x00315D50
		public static HashedString GetAnimFileName(RecoverFromColdChore.Instance smi)
		{
			return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.GetAnimFileName(smi.sm.entityRecovering.Get(smi)));
		}

		// Token: 0x0600817C RID: 33148 RVA: 0x00317B86 File Offset: 0x00315D86
		public static string GetPreAnimName(RecoverFromColdChore.Instance smi)
		{
			return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.PreAnimName);
		}

		// Token: 0x0600817D RID: 33149 RVA: 0x00317BAD File Offset: 0x00315DAD
		public static string GetLoopAnimName(RecoverFromColdChore.Instance smi)
		{
			return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.LoopAnimName);
		}

		// Token: 0x0600817E RID: 33150 RVA: 0x00317BD4 File Offset: 0x00315DD4
		public static string GetPstAnimName(RecoverFromColdChore.Instance smi)
		{
			return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.PstAnimName);
		}

		// Token: 0x0600817F RID: 33151 RVA: 0x00317BFC File Offset: 0x00315DFC
		public static string GetAnimFromColdImmunityProvider(RecoverFromColdChore.Instance smi, Func<ColdImmunityProvider.Instance, string> getCallback)
		{
			ColdImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			if (lastKnownImmunityProvider != null)
			{
				return getCallback(lastKnownImmunityProvider);
			}
			return null;
		}

		// Token: 0x040061BE RID: 25022
		public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x040061BF RID: 25023
		public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.PreLoopPostState recover;

		// Token: 0x040061C0 RID: 25024
		public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State remove_suit;

		// Token: 0x040061C1 RID: 25025
		public RecoverFromColdChore.States.CompleteStates complete;

		// Token: 0x040061C2 RID: 25026
		public StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.TargetParameter coldImmunityProvider;

		// Token: 0x040061C3 RID: 25027
		public StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.TargetParameter entityRecovering;

		// Token: 0x040061C4 RID: 25028
		public StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.TargetParameter locator;

		// Token: 0x020023DA RID: 9178
		public class CompleteStates : GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State
		{
			// Token: 0x0400A00B RID: 40971
			public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State evaluate;

			// Token: 0x0400A00C RID: 40972
			public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State fail;

			// Token: 0x0400A00D RID: 40973
			public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State success;
		}
	}

	// Token: 0x020011E8 RID: 4584
	public class Instance : GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.GameInstance
	{
		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06008181 RID: 33153 RVA: 0x00317C24 File Offset: 0x00315E24
		public ColdImmunityProvider.Instance lastKnownImmunityProvider
		{
			get
			{
				if (!(base.sm.coldImmunityProvider.Get(this) == null))
				{
					return base.sm.coldImmunityProvider.Get(this).GetSMI<ColdImmunityProvider.Instance>();
				}
				return null;
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06008182 RID: 33154 RVA: 0x00317C57 File Offset: 0x00315E57
		public ColdImmunityMonitor.Instance coldImmunityMonitor
		{
			get
			{
				return base.sm.entityRecovering.Get(this).GetSMI<ColdImmunityMonitor.Instance>();
			}
		}

		// Token: 0x06008183 RID: 33155 RVA: 0x00317C70 File Offset: 0x00315E70
		public Instance(RecoverFromColdChore master, GameObject entityRecovering) : base(master)
		{
			base.sm.entityRecovering.Set(entityRecovering, this, false);
			ColdImmunityMonitor.Instance coldImmunityMonitor = this.coldImmunityMonitor;
			if (coldImmunityMonitor.NearestImmunityProvider != null && !coldImmunityMonitor.NearestImmunityProvider.isMasterNull)
			{
				base.sm.coldImmunityProvider.Set(coldImmunityMonitor.NearestImmunityProvider.gameObject, this, false);
			}
		}

		// Token: 0x06008184 RID: 33156 RVA: 0x00317CD4 File Offset: 0x00315ED4
		public void CreateLocator()
		{
			GameObject value = ChoreHelpers.CreateLocator("RecoverWarmthLocator", Vector3.zero);
			base.sm.locator.Set(value, this, false);
			this.UpdateLocator();
		}

		// Token: 0x06008185 RID: 33157 RVA: 0x00317D0C File Offset: 0x00315F0C
		public void UpdateImmunityProvider()
		{
			ColdImmunityProvider.Instance nearestImmunityProvider = this.coldImmunityMonitor.NearestImmunityProvider;
			base.sm.coldImmunityProvider.Set((nearestImmunityProvider == null || nearestImmunityProvider.isMasterNull) ? null : nearestImmunityProvider.gameObject, this, false);
		}

		// Token: 0x06008186 RID: 33158 RVA: 0x00317D4C File Offset: 0x00315F4C
		public void UpdateLocator()
		{
			int num = this.coldImmunityMonitor.WarmUpCell;
			if (num == Grid.InvalidCell)
			{
				num = Grid.PosToCell(base.sm.entityRecovering.Get<Transform>(base.smi).GetPosition());
				this.DestroyLocator();
			}
			else
			{
				Vector3 position = Grid.CellToPosCBC(num, Grid.SceneLayer.Move);
				base.sm.locator.Get<Transform>(base.smi).SetPosition(position);
			}
			this.targetCell = num;
		}

		// Token: 0x06008187 RID: 33159 RVA: 0x00317DC3 File Offset: 0x00315FC3
		public void DestroyLocator()
		{
			ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
			base.sm.locator.Set(null, this);
		}

		// Token: 0x040061C5 RID: 25029
		private int targetCell;
	}
}
