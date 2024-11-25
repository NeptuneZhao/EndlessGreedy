using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000480 RID: 1152
public class SuperProductive : GameStateMachine<SuperProductive, SuperProductive.Instance>
{
	// Token: 0x060018DA RID: 6362 RVA: 0x00084990 File Offset: 0x00082B90
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).ToggleStatusItem(Db.Get().DuplicantStatusItems.BeingProductive, null).Enter(delegate(SuperProductive.Instance smi)
		{
			if (PopFXManager.Instance != null)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, DUPLICANTS.TRAITS.SUPERPRODUCTIVE.NAME, smi.master.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
			}
			smi.fx = new SuperProductiveFX.Instance(smi.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.FXFront)));
			smi.fx.StartSM();
		}).Exit(delegate(SuperProductive.Instance smi)
		{
			smi.fx.sm.destroyFX.Trigger(smi.fx);
		}).DefaultState(this.overjoyed.idle);
		this.overjoyed.idle.EventTransition(GameHashes.StartWork, this.overjoyed.working, null);
		this.overjoyed.working.ScheduleGoTo(0.33f, this.overjoyed.superProductive);
		this.overjoyed.superProductive.Enter(delegate(SuperProductive.Instance smi)
		{
			WorkerBase component = smi.GetComponent<WorkerBase>();
			if (component != null && component.GetState() == WorkerBase.State.Working)
			{
				Workable workable = component.GetWorkable();
				if (workable != null)
				{
					float num = workable.WorkTimeRemaining;
					if (workable.GetComponent<Diggable>() != null)
					{
						num = Diggable.GetApproximateDigTime(Grid.PosToCell(workable));
					}
					if (num > 1f && smi.ShouldSkipWork() && component.InstantlyFinish())
					{
						smi.ReactSuperProductive();
						smi.fx.sm.wasProductive.Trigger(smi.fx);
					}
				}
			}
			smi.GoTo(this.overjoyed.idle);
		});
	}

	// Token: 0x04000DD3 RID: 3539
	public GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000DD4 RID: 3540
	public SuperProductive.OverjoyedStates overjoyed;

	// Token: 0x02001257 RID: 4695
	public class OverjoyedStates : GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006307 RID: 25351
		public GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04006308 RID: 25352
		public GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State working;

		// Token: 0x04006309 RID: 25353
		public GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State superProductive;
	}

	// Token: 0x02001258 RID: 4696
	public new class Instance : GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060082E8 RID: 33512 RVA: 0x0031D933 File Offset: 0x0031BB33
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060082E9 RID: 33513 RVA: 0x0031D93C File Offset: 0x0031BB3C
		public bool ShouldSkipWork()
		{
			return UnityEngine.Random.Range(0f, 100f) <= TRAITS.JOY_REACTIONS.SUPER_PRODUCTIVE.INSTANT_SUCCESS_CHANCE;
		}

		// Token: 0x060082EA RID: 33514 RVA: 0x0031D958 File Offset: 0x0031BB58
		public void ReactSuperProductive()
		{
			ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
			if (smi != null)
			{
				smi.AddSelfEmoteReactable(base.gameObject, "SuperProductive", Db.Get().Emotes.Minion.ProductiveCheer, true, Db.Get().ChoreTypes.EmoteHighPriority, 0f, 1f, 1f, 0f, null);
			}
		}

		// Token: 0x0400630A RID: 25354
		public SuperProductiveFX.Instance fx;
	}
}
