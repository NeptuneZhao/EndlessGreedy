using System;
using UnityEngine;

// Token: 0x0200044F RID: 1103
public class RecoverFromHeatChore : Chore<RecoverFromHeatChore.Instance>
{
	// Token: 0x0600174F RID: 5967 RVA: 0x0007E3A8 File Offset: 0x0007C5A8
	public RecoverFromHeatChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.RecoverFromHeat, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new RecoverFromHeatChore.Instance(this, target.gameObject);
		HeatImmunityMonitor.Instance chillyBones = target.gameObject.GetSMI<HeatImmunityMonitor.Instance>();
		Func<int> data = () => chillyBones.ShelterCell;
		this.AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCell, data);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x020011EA RID: 4586
	public class States : GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore>
	{
		// Token: 0x0600818A RID: 33162 RVA: 0x00317E04 File Offset: 0x00316004
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.entityRecovering);
			this.root.Enter("CreateLocator", delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.CreateLocator();
			}).Enter("UpdateImmunityProvider", delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.UpdateImmunityProvider();
			}).Exit("DestroyLocator", delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.DestroyLocator();
			}).Update("UpdateLocator", delegate(RecoverFromHeatChore.Instance smi, float dt)
			{
				smi.UpdateLocator();
			}, UpdateRate.SIM_200ms, true).Update("UpdateHeatImmunityProvider", delegate(RecoverFromHeatChore.Instance smi, float dt)
			{
				smi.UpdateImmunityProvider();
			}, UpdateRate.SIM_200ms, true);
			this.approach.InitializeStates(this.entityRecovering, this.locator, this.recover, null, null, null);
			this.recover.OnTargetLost(this.heatImmunityProvider, null).Enter("AnimOverride", delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.cachedAnimName = RecoverFromHeatChore.States.GetAnimFileName(smi);
				smi.GetComponent<KAnimControllerBase>().AddAnimOverrides(Assets.GetAnim(smi.cachedAnimName), 0f);
			}).Exit(delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(Assets.GetAnim(smi.cachedAnimName));
			}).DefaultState(this.recover.pre).ToggleTag(GameTags.RecoveringFromHeat);
			this.recover.pre.Face(this.heatImmunityProvider, 0f).PlayAnim(new Func<RecoverFromHeatChore.Instance, string>(RecoverFromHeatChore.States.GetPreAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover.loop);
			this.recover.loop.PlayAnim(new Func<RecoverFromHeatChore.Instance, string>(RecoverFromHeatChore.States.GetLoopAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover.pst);
			this.recover.pst.QueueAnim(new Func<RecoverFromHeatChore.Instance, string>(RecoverFromHeatChore.States.GetPstAnimName), false, null).OnAnimQueueComplete(this.complete);
			this.complete.DefaultState(this.complete.evaluate);
			this.complete.evaluate.EnterTransition(this.complete.success, new StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.Transition.ConditionCallback(RecoverFromHeatChore.States.IsImmunityProviderStillValid)).EnterTransition(this.complete.fail, GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.Not(new StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.Transition.ConditionCallback(RecoverFromHeatChore.States.IsImmunityProviderStillValid)));
			this.complete.success.Enter(new StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State.Callback(RecoverFromHeatChore.States.ApplyHeatImmunityEffect)).ReturnSuccess();
			this.complete.fail.ReturnFailure();
		}

		// Token: 0x0600818B RID: 33163 RVA: 0x003180C4 File Offset: 0x003162C4
		public static bool IsImmunityProviderStillValid(RecoverFromHeatChore.Instance smi)
		{
			HeatImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			return lastKnownImmunityProvider != null && lastKnownImmunityProvider.CanBeUsed;
		}

		// Token: 0x0600818C RID: 33164 RVA: 0x003180E4 File Offset: 0x003162E4
		public static void ApplyHeatImmunityEffect(RecoverFromHeatChore.Instance smi)
		{
			HeatImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			if (lastKnownImmunityProvider != null)
			{
				lastKnownImmunityProvider.ApplyImmunityEffect(smi.gameObject, true);
			}
		}

		// Token: 0x0600818D RID: 33165 RVA: 0x00318108 File Offset: 0x00316308
		public static HashedString GetAnimFileName(RecoverFromHeatChore.Instance smi)
		{
			return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (HeatImmunityProvider.Instance p) => p.GetAnimFileName(smi.sm.entityRecovering.Get(smi)));
		}

		// Token: 0x0600818E RID: 33166 RVA: 0x0031813E File Offset: 0x0031633E
		public static string GetPreAnimName(RecoverFromHeatChore.Instance smi)
		{
			return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (HeatImmunityProvider.Instance p) => p.PreAnimName);
		}

		// Token: 0x0600818F RID: 33167 RVA: 0x00318165 File Offset: 0x00316365
		public static string GetLoopAnimName(RecoverFromHeatChore.Instance smi)
		{
			return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (HeatImmunityProvider.Instance p) => p.LoopAnimName);
		}

		// Token: 0x06008190 RID: 33168 RVA: 0x0031818C File Offset: 0x0031638C
		public static string GetPstAnimName(RecoverFromHeatChore.Instance smi)
		{
			return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (HeatImmunityProvider.Instance p) => p.PstAnimName);
		}

		// Token: 0x06008191 RID: 33169 RVA: 0x003181B4 File Offset: 0x003163B4
		public static string GetAnimFromHeatImmunityProvider(RecoverFromHeatChore.Instance smi, Func<HeatImmunityProvider.Instance, string> getCallback)
		{
			HeatImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			if (lastKnownImmunityProvider != null)
			{
				return getCallback(lastKnownImmunityProvider);
			}
			return null;
		}

		// Token: 0x040061C7 RID: 25031
		public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x040061C8 RID: 25032
		public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.PreLoopPostState recover;

		// Token: 0x040061C9 RID: 25033
		public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State remove_suit;

		// Token: 0x040061CA RID: 25034
		public RecoverFromHeatChore.States.CompleteStates complete;

		// Token: 0x040061CB RID: 25035
		public StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.TargetParameter heatImmunityProvider;

		// Token: 0x040061CC RID: 25036
		public StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.TargetParameter entityRecovering;

		// Token: 0x040061CD RID: 25037
		public StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.TargetParameter locator;

		// Token: 0x020023DD RID: 9181
		public class CompleteStates : GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State
		{
			// Token: 0x0400A018 RID: 40984
			public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State evaluate;

			// Token: 0x0400A019 RID: 40985
			public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State fail;

			// Token: 0x0400A01A RID: 40986
			public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State success;
		}
	}

	// Token: 0x020011EB RID: 4587
	public class Instance : GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.GameInstance
	{
		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06008193 RID: 33171 RVA: 0x003181DC File Offset: 0x003163DC
		public HeatImmunityProvider.Instance lastKnownImmunityProvider
		{
			get
			{
				if (!(base.sm.heatImmunityProvider.Get(this) == null))
				{
					return base.sm.heatImmunityProvider.Get(this).GetSMI<HeatImmunityProvider.Instance>();
				}
				return null;
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06008194 RID: 33172 RVA: 0x0031820F File Offset: 0x0031640F
		public HeatImmunityMonitor.Instance heatImmunityMonitor
		{
			get
			{
				return base.sm.entityRecovering.Get(this).GetSMI<HeatImmunityMonitor.Instance>();
			}
		}

		// Token: 0x06008195 RID: 33173 RVA: 0x00318228 File Offset: 0x00316428
		public Instance(RecoverFromHeatChore master, GameObject entityRecovering) : base(master)
		{
			base.sm.entityRecovering.Set(entityRecovering, this, false);
			HeatImmunityMonitor.Instance heatImmunityMonitor = this.heatImmunityMonitor;
			if (heatImmunityMonitor.NearestImmunityProvider != null && !heatImmunityMonitor.NearestImmunityProvider.isMasterNull)
			{
				base.sm.heatImmunityProvider.Set(heatImmunityMonitor.NearestImmunityProvider.gameObject, this, false);
			}
		}

		// Token: 0x06008196 RID: 33174 RVA: 0x0031828C File Offset: 0x0031648C
		public void CreateLocator()
		{
			GameObject value = ChoreHelpers.CreateLocator("RecoverWarmthLocator", Vector3.zero);
			base.sm.locator.Set(value, this, false);
			this.UpdateLocator();
		}

		// Token: 0x06008197 RID: 33175 RVA: 0x003182C4 File Offset: 0x003164C4
		public void UpdateImmunityProvider()
		{
			HeatImmunityProvider.Instance nearestImmunityProvider = this.heatImmunityMonitor.NearestImmunityProvider;
			base.sm.heatImmunityProvider.Set((nearestImmunityProvider == null || nearestImmunityProvider.isMasterNull) ? null : nearestImmunityProvider.gameObject, this, false);
		}

		// Token: 0x06008198 RID: 33176 RVA: 0x00318304 File Offset: 0x00316504
		public void UpdateLocator()
		{
			int num = this.heatImmunityMonitor.ShelterCell;
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

		// Token: 0x06008199 RID: 33177 RVA: 0x0031837B File Offset: 0x0031657B
		public void DestroyLocator()
		{
			ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
			base.sm.locator.Set(null, this);
		}

		// Token: 0x040061CE RID: 25038
		private int targetCell;

		// Token: 0x040061CF RID: 25039
		public HashedString cachedAnimName;
	}
}
