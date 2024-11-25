using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200018C RID: 396
public class UnstableEntombDefense : GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>
{
	// Token: 0x06000812 RID: 2066 RVA: 0x00035910 File Offset: 0x00033B10
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.disabled;
		this.disabled.EventTransition(GameHashes.Died, this.dead, null).ParamTransition<bool>(this.Active, this.active, GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.IsTrue);
		this.active.EventTransition(GameHashes.Died, this.dead, null).ParamTransition<bool>(this.Active, this.disabled, GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.IsFalse).DefaultState(this.active.safe);
		this.active.safe.DefaultState(this.active.safe.idle);
		this.active.safe.idle.ParamTransition<float>(this.TimeBeforeNextReaction, this.active.threatened, (UnstableEntombDefense.Instance smi, float p) => GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.IsGTZero(smi, p) && UnstableEntombDefense.IsEntombedByUnstable(smi)).EventTransition(GameHashes.EntombedChanged, this.active.safe.newThreat, new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.Transition.ConditionCallback(UnstableEntombDefense.IsEntombedByUnstable));
		this.active.safe.newThreat.Enter(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State.Callback(UnstableEntombDefense.ResetCooldown)).GoTo(this.active.threatened);
		this.active.threatened.EventTransition(GameHashes.Died, this.dead, null).Exit(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State.Callback(UnstableEntombDefense.ResetCooldown)).EventTransition(GameHashes.EntombedChanged, this.active.safe, GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.Not(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.Transition.ConditionCallback(UnstableEntombDefense.IsEntombedByUnstable))).DefaultState(this.active.threatened.inCooldown);
		this.active.threatened.inCooldown.ParamTransition<float>(this.TimeBeforeNextReaction, this.active.threatened.react, GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.IsLTEZero).Update(new Action<UnstableEntombDefense.Instance, float>(UnstableEntombDefense.CooldownTick), UpdateRate.SIM_200ms, false);
		this.active.threatened.react.TriggerOnEnter(GameHashes.EntombDefenseReactionBegins, null).PlayAnim((UnstableEntombDefense.Instance smi) => smi.UnentombAnimName, KAnim.PlayMode.Once).OnAnimQueueComplete(this.active.threatened.complete).ScheduleGoTo(2f, this.active.threatened.complete);
		this.active.threatened.complete.TriggerOnEnter(GameHashes.EntombDefenseReact, null).Enter(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State.Callback(UnstableEntombDefense.AttemptToBreakFree)).Enter(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State.Callback(UnstableEntombDefense.ResetCooldown)).GoTo(this.active.threatened.inCooldown);
		this.dead.DoNothing();
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x00035BD3 File Offset: 0x00033DD3
	public static void ResetCooldown(UnstableEntombDefense.Instance smi)
	{
		smi.sm.TimeBeforeNextReaction.Set(smi.def.Cooldown, smi, false);
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x00035BF3 File Offset: 0x00033DF3
	public static bool IsEntombedByUnstable(UnstableEntombDefense.Instance smi)
	{
		return smi.IsEntombed && smi.IsInPressenceOfUnstableSolids();
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x00035C05 File Offset: 0x00033E05
	public static void AttemptToBreakFree(UnstableEntombDefense.Instance smi)
	{
		smi.AttackUnstableCells();
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x00035C10 File Offset: 0x00033E10
	public static void CooldownTick(UnstableEntombDefense.Instance smi, float dt)
	{
		float value = smi.RemainingCooldown - dt;
		smi.sm.TimeBeforeNextReaction.Set(value, smi, false);
	}

	// Token: 0x040005B1 RID: 1457
	public UnstableEntombDefense.ActiveState active;

	// Token: 0x040005B2 RID: 1458
	public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State disabled;

	// Token: 0x040005B3 RID: 1459
	public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State dead;

	// Token: 0x040005B4 RID: 1460
	public StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.FloatParameter TimeBeforeNextReaction;

	// Token: 0x040005B5 RID: 1461
	public StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.BoolParameter Active = new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.BoolParameter(true);

	// Token: 0x020010D4 RID: 4308
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06007D37 RID: 32055 RVA: 0x00307C90 File Offset: 0x00305E90
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			UnstableEntombDefense.Instance smi = go.GetSMI<UnstableEntombDefense.Instance>();
			if (smi != null)
			{
				Descriptor stateDescriptor = smi.GetStateDescriptor();
				if (stateDescriptor.type == Descriptor.DescriptorType.Effect)
				{
					list.Add(stateDescriptor);
				}
			}
			return list;
		}

		// Token: 0x04005E28 RID: 24104
		public float Cooldown = 5f;

		// Token: 0x04005E29 RID: 24105
		public string defaultAnimName = "";
	}

	// Token: 0x020010D5 RID: 4309
	public class SafeStates : GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State
	{
		// Token: 0x04005E2A RID: 24106
		public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State idle;

		// Token: 0x04005E2B RID: 24107
		public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State newThreat;
	}

	// Token: 0x020010D6 RID: 4310
	public class ThreatenedStates : GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State
	{
		// Token: 0x04005E2C RID: 24108
		public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State inCooldown;

		// Token: 0x04005E2D RID: 24109
		public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State react;

		// Token: 0x04005E2E RID: 24110
		public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State complete;
	}

	// Token: 0x020010D7 RID: 4311
	public class ActiveState : GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State
	{
		// Token: 0x04005E2F RID: 24111
		public UnstableEntombDefense.SafeStates safe;

		// Token: 0x04005E30 RID: 24112
		public UnstableEntombDefense.ThreatenedStates threatened;
	}

	// Token: 0x020010D8 RID: 4312
	public new class Instance : GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.GameInstance
	{
		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06007D3C RID: 32060 RVA: 0x00307CFB File Offset: 0x00305EFB
		public float RemainingCooldown
		{
			get
			{
				return base.sm.TimeBeforeNextReaction.Get(this);
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06007D3D RID: 32061 RVA: 0x00307D0E File Offset: 0x00305F0E
		public bool IsEntombed
		{
			get
			{
				return this.entombVulnerable.GetEntombed;
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06007D3E RID: 32062 RVA: 0x00307D1B File Offset: 0x00305F1B
		public bool IsActive
		{
			get
			{
				return base.sm.Active.Get(this);
			}
		}

		// Token: 0x06007D3F RID: 32063 RVA: 0x00307D2E File Offset: 0x00305F2E
		public Instance(IStateMachineTarget master, UnstableEntombDefense.Def def) : base(master, def)
		{
			this.UnentombAnimName = ((this.UnentombAnimName == null) ? def.defaultAnimName : this.UnentombAnimName);
		}

		// Token: 0x06007D40 RID: 32064 RVA: 0x00307D54 File Offset: 0x00305F54
		public bool IsInPressenceOfUnstableSolids()
		{
			int cell = Grid.PosToCell(this);
			CellOffset[] occupiedCellsOffsets = this.occupyArea.OccupiedCellsOffsets;
			for (int i = 0; i < occupiedCellsOffsets.Length; i++)
			{
				int num = Grid.OffsetCell(cell, occupiedCellsOffsets[i]);
				if (Grid.IsValidCell(num) && Grid.Solid[num] && Grid.Element[num].IsUnstable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007D41 RID: 32065 RVA: 0x00307DB8 File Offset: 0x00305FB8
		public void AttackUnstableCells()
		{
			int cell = Grid.PosToCell(this);
			CellOffset[] occupiedCellsOffsets = this.occupyArea.OccupiedCellsOffsets;
			for (int i = 0; i < occupiedCellsOffsets.Length; i++)
			{
				int num = Grid.OffsetCell(cell, occupiedCellsOffsets[i]);
				if (Grid.IsValidCell(num) && Grid.Solid[num] && Grid.Element[num].IsUnstable)
				{
					SimMessages.Dig(num, -1, false);
				}
			}
		}

		// Token: 0x06007D42 RID: 32066 RVA: 0x00307E1F File Offset: 0x0030601F
		public void SetActive(bool active)
		{
			base.sm.Active.Set(active, this, false);
		}

		// Token: 0x06007D43 RID: 32067 RVA: 0x00307E38 File Offset: 0x00306038
		public Descriptor GetStateDescriptor()
		{
			if (base.IsInsideState(base.sm.disabled))
			{
				return new Descriptor(UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSEOFF, UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSEOFF, Descriptor.DescriptorType.Effect, false);
			}
			if (base.IsInsideState(base.sm.active.safe))
			{
				return new Descriptor(UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSEREADY, UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSEREADY, Descriptor.DescriptorType.Effect, false);
			}
			if (base.IsInsideState(base.sm.active.threatened.inCooldown))
			{
				return new Descriptor(UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSETHREATENED, UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSETHREATENED, Descriptor.DescriptorType.Effect, false);
			}
			if (base.IsInsideState(base.sm.active.threatened.react))
			{
				return new Descriptor(UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSEREACTING, UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSEREACTING, Descriptor.DescriptorType.Effect, false);
			}
			return new Descriptor
			{
				type = Descriptor.DescriptorType.Detail
			};
		}

		// Token: 0x04005E31 RID: 24113
		public string UnentombAnimName;

		// Token: 0x04005E32 RID: 24114
		[MyCmpGet]
		private EntombVulnerable entombVulnerable;

		// Token: 0x04005E33 RID: 24115
		[MyCmpGet]
		private OccupyArea occupyArea;
	}
}
