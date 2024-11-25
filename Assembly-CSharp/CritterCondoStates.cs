using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class CritterCondoStates : GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>
{
	// Token: 0x0600039B RID: 923 RVA: 0x0001DDD0 File Offset: 0x0001BFD0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingToCondo;
		this.root.Enter(new StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State.Callback(CritterCondoStates.ReserveCondo)).Exit(new StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State.Callback(CritterCondoStates.UnreserveCondo));
		this.goingToCondo.MoveTo(new Func<CritterCondoStates.Instance, int>(CritterCondoStates.GetCondoInteractCell), this.interact, null, false).ToggleMainStatusItem((CritterCondoStates.Instance smi) => CritterCondoStates.GetTargetCondo(smi).def.moveToStatusItem, null).OnTargetLost(this.targetCondo, null);
		this.interact.DefaultState(this.interact.pre).OnTargetLost(this.targetCondo, null).Enter(delegate(CritterCondoStates.Instance smi)
		{
			this.SetFacing(smi);
			smi.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
		}).Exit(delegate(CritterCondoStates.Instance smi)
		{
			smi.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
		}).ToggleMainStatusItem((CritterCondoStates.Instance smi) => CritterCondoStates.GetTargetCondo(smi).def.interactStatusItem, null);
		this.interact.pre.PlayAnim("cc_working_pre").Enter(delegate(CritterCondoStates.Instance smi)
		{
			CritterCondoStates.PlayCondoBuildingAnim(smi, "cc_working_pre");
		}).OnAnimQueueComplete(this.interact.loop);
		this.interact.loop.PlayAnim("cc_working").Enter(delegate(CritterCondoStates.Instance smi)
		{
			CritterCondoStates.PlayCondoBuildingAnim(smi, smi.def.working_anim);
		}).OnAnimQueueComplete(this.interact.pst);
		this.interact.pst.PlayAnim("cc_working_pst").Enter(delegate(CritterCondoStates.Instance smi)
		{
			CritterCondoStates.PlayCondoBuildingAnim(smi, "cc_working_pst");
		}).OnAnimQueueComplete(this.behaviourComplete);
		this.behaviourComplete.BehaviourComplete(GameTags.Creatures.Behaviour_InteractWithCritterCondo, false).Exit(new StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State.Callback(CritterCondoStates.ApplyEffects));
	}

	// Token: 0x0600039C RID: 924 RVA: 0x0001DFD8 File Offset: 0x0001C1D8
	private void SetFacing(CritterCondoStates.Instance smi)
	{
		bool isRotated = CritterCondoStates.GetTargetCondo(smi).Get<Rotatable>().IsRotated;
		smi.Get<Facing>().SetFacing(isRotated);
	}

	// Token: 0x0600039D RID: 925 RVA: 0x0001E004 File Offset: 0x0001C204
	private static CritterCondo.Instance GetTargetCondo(CritterCondoStates.Instance smi)
	{
		GameObject gameObject = smi.sm.targetCondo.Get(smi);
		CritterCondo.Instance instance = (gameObject != null) ? gameObject.GetSMI<CritterCondo.Instance>() : null;
		if (instance.IsNullOrStopped())
		{
			return null;
		}
		return instance;
	}

	// Token: 0x0600039E RID: 926 RVA: 0x0001E044 File Offset: 0x0001C244
	private static void ReserveCondo(CritterCondoStates.Instance smi)
	{
		CritterCondo.Instance instance = smi.GetSMI<CritterCondoInteractMontior.Instance>().targetCondo;
		if (instance == null)
		{
			return;
		}
		smi.sm.targetCondo.Set(instance.gameObject, smi, false);
		instance.SetReserved(true);
	}

	// Token: 0x0600039F RID: 927 RVA: 0x0001E084 File Offset: 0x0001C284
	private static void UnreserveCondo(CritterCondoStates.Instance smi)
	{
		CritterCondo.Instance instance = CritterCondoStates.GetTargetCondo(smi);
		if (instance == null)
		{
			return;
		}
		instance.GetComponent<KBatchedAnimController>().Play("on", KAnim.PlayMode.Loop, 1f, 0f);
		smi.sm.targetCondo.Set(null, smi);
		instance.SetReserved(false);
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x0001E0D8 File Offset: 0x0001C2D8
	private static int GetCondoInteractCell(CritterCondoStates.Instance smi)
	{
		CritterCondo.Instance instance = CritterCondoStates.GetTargetCondo(smi);
		if (instance == null)
		{
			return Grid.InvalidCell;
		}
		return instance.GetInteractStartCell();
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x0001E0FB File Offset: 0x0001C2FB
	private static void ApplyEffects(CritterCondoStates.Instance smi)
	{
		smi.Get<Effects>().Add(CritterCondoStates.GetTargetCondo(smi).def.effectId, true);
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x0001E11A File Offset: 0x0001C31A
	private static void PlayCondoBuildingAnim(CritterCondoStates.Instance smi, string anim_name)
	{
		if (smi.def.entersBuilding)
		{
			smi.sm.targetCondo.Get<KBatchedAnimController>(smi).Play(anim_name, KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x04000280 RID: 640
	public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State goingToCondo;

	// Token: 0x04000281 RID: 641
	public CritterCondoStates.InteractState interact;

	// Token: 0x04000282 RID: 642
	public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State behaviourComplete;

	// Token: 0x04000283 RID: 643
	public StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.TargetParameter targetCondo;

	// Token: 0x02001018 RID: 4120
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005C2C RID: 23596
		public bool entersBuilding = true;

		// Token: 0x04005C2D RID: 23597
		public string working_anim = "cc_working";
	}

	// Token: 0x02001019 RID: 4121
	public new class Instance : GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.GameInstance
	{
		// Token: 0x06007B38 RID: 31544 RVA: 0x003033AC File Offset: 0x003015AC
		public Instance(Chore<CritterCondoStates.Instance> chore, CritterCondoStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviour_InteractWithCritterCondo);
		}
	}

	// Token: 0x0200101A RID: 4122
	public class InteractState : GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State
	{
		// Token: 0x04005C2E RID: 23598
		public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State pre;

		// Token: 0x04005C2F RID: 23599
		public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State loop;

		// Token: 0x04005C30 RID: 23600
		public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State pst;
	}
}
