using System;
using Klei.AI;

// Token: 0x020007AE RID: 1966
public class ChilledBones : GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>
{
	// Token: 0x060035C4 RID: 13764 RVA: 0x0012491C File Offset: 0x00122B1C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.normal;
		this.normal.UpdateTransition(this.chilled, new Func<ChilledBones.Instance, float, bool>(this.IsChilling), UpdateRate.SIM_200ms, false);
		this.chilled.ToggleEffect("ChilledBones").UpdateTransition(this.normal, new Func<ChilledBones.Instance, float, bool>(this.IsNotChilling), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x060035C5 RID: 13765 RVA: 0x00124982 File Offset: 0x00122B82
	public bool IsNotChilling(ChilledBones.Instance smi, float dt)
	{
		return !this.IsChilling(smi, dt);
	}

	// Token: 0x060035C6 RID: 13766 RVA: 0x0012498F File Offset: 0x00122B8F
	public bool IsChilling(ChilledBones.Instance smi, float dt)
	{
		return smi.IsChilled;
	}

	// Token: 0x0400200B RID: 8203
	public const string EFFECT_NAME = "ChilledBones";

	// Token: 0x0400200C RID: 8204
	public GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>.State normal;

	// Token: 0x0400200D RID: 8205
	public GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>.State chilled;

	// Token: 0x02001665 RID: 5733
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006F87 RID: 28551
		public float THRESHOLD = -1f;
	}

	// Token: 0x02001666 RID: 5734
	public new class Instance : GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>.GameInstance
	{
		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06009218 RID: 37400 RVA: 0x00353057 File Offset: 0x00351257
		public float TemperatureTransferAttribute
		{
			get
			{
				return this.minionModifiers.GetAttributes().GetValue(this.bodyTemperatureTransferAttribute.Id) * 600f;
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06009219 RID: 37401 RVA: 0x0035307A File Offset: 0x0035127A
		public bool IsChilled
		{
			get
			{
				return this.TemperatureTransferAttribute < base.def.THRESHOLD;
			}
		}

		// Token: 0x0600921A RID: 37402 RVA: 0x0035308F File Offset: 0x0035128F
		public Instance(IStateMachineTarget master, ChilledBones.Def def) : base(master, def)
		{
			this.bodyTemperatureTransferAttribute = Db.Get().Attributes.TryGet("TemperatureDelta");
		}

		// Token: 0x04006F88 RID: 28552
		[MyCmpGet]
		public MinionModifiers minionModifiers;

		// Token: 0x04006F89 RID: 28553
		public Klei.AI.Attribute bodyTemperatureTransferAttribute;
	}
}
