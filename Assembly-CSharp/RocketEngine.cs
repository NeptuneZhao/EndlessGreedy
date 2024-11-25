using System;
using KSerialization;
using STRINGS;

// Token: 0x02000ADF RID: 2783
[SerializationConfig(MemberSerialization.OptIn)]
public class RocketEngine : StateMachineComponent<RocketEngine.StatesInstance>
{
	// Token: 0x060052AE RID: 21166 RVA: 0x001DA334 File Offset: 0x001D8534
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.mainEngine)
		{
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new RequireAttachedComponent(base.gameObject.GetComponent<AttachableBuilding>(), typeof(FuelTank), UI.STARMAP.COMPONENT.FUEL_TANK));
		}
	}

	// Token: 0x0400368D RID: 13965
	public float exhaustEmitRate = 50f;

	// Token: 0x0400368E RID: 13966
	public float exhaustTemperature = 1500f;

	// Token: 0x0400368F RID: 13967
	public SpawnFXHashes explosionEffectHash;

	// Token: 0x04003690 RID: 13968
	public SimHashes exhaustElement = SimHashes.CarbonDioxide;

	// Token: 0x04003691 RID: 13969
	public Tag fuelTag;

	// Token: 0x04003692 RID: 13970
	public float efficiency = 1f;

	// Token: 0x04003693 RID: 13971
	public bool requireOxidizer = true;

	// Token: 0x04003694 RID: 13972
	public bool mainEngine = true;

	// Token: 0x02001B2E RID: 6958
	public class StatesInstance : GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.GameInstance
	{
		// Token: 0x0600A2C0 RID: 41664 RVA: 0x0038814A File Offset: 0x0038634A
		public StatesInstance(RocketEngine smi) : base(smi)
		{
		}
	}

	// Token: 0x02001B2F RID: 6959
	public class States : GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine>
	{
		// Token: 0x0600A2C1 RID: 41665 RVA: 0x00388154 File Offset: 0x00386354
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.IgniteEngine, this.burning, null);
			this.burning.EventTransition(GameHashes.RocketLanded, this.burnComplete, null).PlayAnim("launch_pre").QueueAnim("launch_loop", true, null).Update(delegate(RocketEngine.StatesInstance smi, float dt)
			{
				int num = Grid.PosToCell(smi.master.gameObject.transform.GetPosition() + smi.master.GetComponent<KBatchedAnimController>().Offset);
				if (Grid.IsValidCell(num))
				{
					SimMessages.EmitMass(num, ElementLoader.GetElementIndex(smi.master.exhaustElement), dt * smi.master.exhaustEmitRate, smi.master.exhaustTemperature, 0, 0, -1);
				}
				int num2 = 10;
				for (int i = 1; i < num2; i++)
				{
					int num3 = Grid.OffsetCell(num, -1, -i);
					int num4 = Grid.OffsetCell(num, 0, -i);
					int num5 = Grid.OffsetCell(num, 1, -i);
					if (Grid.IsValidCell(num3))
					{
						SimMessages.ModifyEnergy(num3, smi.master.exhaustTemperature / (float)(i + 1), 3200f, SimMessages.EnergySourceID.Burner);
					}
					if (Grid.IsValidCell(num4))
					{
						SimMessages.ModifyEnergy(num4, smi.master.exhaustTemperature / (float)i, 3200f, SimMessages.EnergySourceID.Burner);
					}
					if (Grid.IsValidCell(num5))
					{
						SimMessages.ModifyEnergy(num5, smi.master.exhaustTemperature / (float)(i + 1), 3200f, SimMessages.EnergySourceID.Burner);
					}
				}
			}, UpdateRate.SIM_200ms, false);
			this.burnComplete.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.IgniteEngine, this.burning, null);
		}

		// Token: 0x04007F04 RID: 32516
		public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State idle;

		// Token: 0x04007F05 RID: 32517
		public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State burning;

		// Token: 0x04007F06 RID: 32518
		public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State burnComplete;
	}
}
