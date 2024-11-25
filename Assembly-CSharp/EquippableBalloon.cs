using System;
using Database;
using KSerialization;
using TUNING;

// Token: 0x02000894 RID: 2196
public class EquippableBalloon : StateMachineComponent<EquippableBalloon.StatesInstance>
{
	// Token: 0x06003D8B RID: 15755 RVA: 0x00154753 File Offset: 0x00152953
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.smi.transitionTime = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.JOY_REACTION_DURATION;
	}

	// Token: 0x06003D8C RID: 15756 RVA: 0x00154776 File Offset: 0x00152976
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.ApplyBalloonOverrideToBalloonFx();
	}

	// Token: 0x06003D8D RID: 15757 RVA: 0x0015478F File Offset: 0x0015298F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003D8E RID: 15758 RVA: 0x00154797 File Offset: 0x00152997
	public void SetBalloonOverride(BalloonOverrideSymbol balloonOverride)
	{
		base.smi.facadeAnim = balloonOverride.animFileID;
		base.smi.symbolID = balloonOverride.animFileSymbolID;
		this.ApplyBalloonOverrideToBalloonFx();
	}

	// Token: 0x06003D8F RID: 15759 RVA: 0x001547C4 File Offset: 0x001529C4
	public void ApplyBalloonOverrideToBalloonFx()
	{
		Equippable component = base.GetComponent<Equippable>();
		if (!component.IsNullOrDestroyed() && !component.assignee.IsNullOrDestroyed())
		{
			Ownables soleOwner = component.assignee.GetSoleOwner();
			if (soleOwner.IsNullOrDestroyed())
			{
				return;
			}
			BalloonFX.Instance smi = ((KMonoBehaviour)soleOwner.GetComponent<MinionAssignablesProxy>().target).GetSMI<BalloonFX.Instance>();
			if (!smi.IsNullOrDestroyed())
			{
				new BalloonOverrideSymbol(base.smi.facadeAnim, base.smi.symbolID).ApplyTo(smi);
			}
		}
	}

	// Token: 0x02001794 RID: 6036
	public class StatesInstance : GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon, object>.GameInstance
	{
		// Token: 0x06009623 RID: 38435 RVA: 0x00361015 File Offset: 0x0035F215
		public StatesInstance(EquippableBalloon master) : base(master)
		{
		}

		// Token: 0x04007322 RID: 29474
		[Serialize]
		public float transitionTime;

		// Token: 0x04007323 RID: 29475
		[Serialize]
		public string facadeAnim;

		// Token: 0x04007324 RID: 29476
		[Serialize]
		public string symbolID;
	}

	// Token: 0x02001795 RID: 6037
	public class States : GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon>
	{
		// Token: 0x06009624 RID: 38436 RVA: 0x00361020 File Offset: 0x0035F220
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Transition(this.destroy, (EquippableBalloon.StatesInstance smi) => GameClock.Instance.GetTime() >= smi.transitionTime, UpdateRate.SIM_200ms);
			this.destroy.Enter(delegate(EquippableBalloon.StatesInstance smi)
			{
				smi.master.GetComponent<Equippable>().Unassign();
			});
		}

		// Token: 0x04007325 RID: 29477
		public GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon, object>.State destroy;
	}
}
