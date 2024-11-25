using System;
using KSerialization;
using UnityEngine;

// Token: 0x020006EC RID: 1772
public class HeatCompressor : StateMachineComponent<HeatCompressor.StatesInstance>
{
	// Token: 0x06002D1D RID: 11549 RVA: 0x000FD7B8 File Offset: 0x000FB9B8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimController>().SetDirty();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("HeatCube"), base.transform.GetPosition());
		gameObject.SetActive(true);
		this.heatCubeStorage.Store(gameObject, true, false, true, false);
		base.smi.StartSM();
	}

	// Token: 0x06002D1E RID: 11550 RVA: 0x000FD867 File Offset: 0x000FBA67
	public void SetStorage(Storage inputStorage, Storage outputStorage, Storage heatCubeStorage)
	{
		this.inputStorage = inputStorage;
		this.outputStorage = outputStorage;
		this.heatCubeStorage = heatCubeStorage;
	}

	// Token: 0x06002D1F RID: 11551 RVA: 0x000FD880 File Offset: 0x000FBA80
	public void CompressHeat(HeatCompressor.StatesInstance smi, float dt)
	{
		smi.heatRemovalTimer -= dt;
		float num = this.heatRemovalRate * dt / (float)this.inputStorage.items.Count;
		foreach (GameObject gameObject in this.inputStorage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			float lowTemp = component.Element.lowTemp;
			GameUtil.DeltaThermalEnergy(component, -num, lowTemp);
			this.energyCompressed += num;
		}
		if (smi.heatRemovalTimer <= 0f)
		{
			for (int i = this.inputStorage.items.Count; i > 0; i--)
			{
				GameObject gameObject2 = this.inputStorage.items[i - 1];
				if (gameObject2)
				{
					this.inputStorage.Transfer(gameObject2, this.outputStorage, false, true);
				}
			}
			smi.StartNewHeatRemoval();
		}
		foreach (GameObject gameObject3 in this.heatCubeStorage.items)
		{
			GameUtil.DeltaThermalEnergy(gameObject3.GetComponent<PrimaryElement>(), this.energyCompressed / (float)this.heatCubeStorage.items.Count, 100000f);
		}
		this.energyCompressed = 0f;
	}

	// Token: 0x06002D20 RID: 11552 RVA: 0x000FD9F4 File Offset: 0x000FBBF4
	public void EjectHeatCube()
	{
		this.heatCubeStorage.DropAll(base.transform.GetPosition(), false, false, default(Vector3), true, null);
	}

	// Token: 0x04001A13 RID: 6675
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001A14 RID: 6676
	private MeterController meter;

	// Token: 0x04001A15 RID: 6677
	public Storage inputStorage;

	// Token: 0x04001A16 RID: 6678
	public Storage outputStorage;

	// Token: 0x04001A17 RID: 6679
	public Storage heatCubeStorage;

	// Token: 0x04001A18 RID: 6680
	public float heatRemovalRate = 100f;

	// Token: 0x04001A19 RID: 6681
	public float heatRemovalTime = 100f;

	// Token: 0x04001A1A RID: 6682
	[Serialize]
	public float energyCompressed;

	// Token: 0x04001A1B RID: 6683
	public float heat_sink_active_time = 9000f;

	// Token: 0x04001A1C RID: 6684
	[Serialize]
	public float time_active;

	// Token: 0x04001A1D RID: 6685
	public float MAX_CUBE_TEMPERATURE = 3000f;

	// Token: 0x02001517 RID: 5399
	public class StatesInstance : GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor, object>.GameInstance
	{
		// Token: 0x06008D36 RID: 36150 RVA: 0x0033E779 File Offset: 0x0033C979
		public StatesInstance(HeatCompressor master) : base(master)
		{
		}

		// Token: 0x06008D37 RID: 36151 RVA: 0x0033E784 File Offset: 0x0033C984
		public void UpdateMeter()
		{
			float remainingCharge = this.GetRemainingCharge();
			base.master.meter.SetPositionPercent(remainingCharge);
		}

		// Token: 0x06008D38 RID: 36152 RVA: 0x0033E7AC File Offset: 0x0033C9AC
		public float GetRemainingCharge()
		{
			PrimaryElement primaryElement = base.smi.master.heatCubeStorage.FindFirstWithMass(GameTags.IndustrialIngredient, 0f);
			float result = 1f;
			if (primaryElement != null)
			{
				result = Mathf.Clamp01(primaryElement.GetComponent<PrimaryElement>().Temperature / base.smi.master.MAX_CUBE_TEMPERATURE);
			}
			return result;
		}

		// Token: 0x06008D39 RID: 36153 RVA: 0x0033E80B File Offset: 0x0033CA0B
		public bool CanWork()
		{
			return this.GetRemainingCharge() < 1f && base.smi.master.heatCubeStorage.items.Count > 0;
		}

		// Token: 0x06008D3A RID: 36154 RVA: 0x0033E839 File Offset: 0x0033CA39
		public void StartNewHeatRemoval()
		{
			this.heatRemovalTimer = base.smi.master.heatRemovalTime;
		}

		// Token: 0x04006BEE RID: 27630
		[Serialize]
		public float heatRemovalTimer;
	}

	// Token: 0x02001518 RID: 5400
	public class States : GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor>
	{
		// Token: 0x06008D3B RID: 36155 RVA: 0x0033E854 File Offset: 0x0033CA54
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inactive;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.EventTransition(GameHashes.OperationalChanged, this.inactive, (HeatCompressor.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.inactive.Enter(delegate(HeatCompressor.StatesInstance smi)
			{
				smi.UpdateMeter();
			}).PlayAnim("idle").Transition(this.dropCube, (HeatCompressor.StatesInstance smi) => smi.GetRemainingCharge() >= 1f, UpdateRate.SIM_200ms).Transition(this.active, (HeatCompressor.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational && smi.CanWork(), UpdateRate.SIM_200ms);
			this.active.Enter(delegate(HeatCompressor.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
				smi.StartNewHeatRemoval();
			}).PlayAnim("working_loop", KAnim.PlayMode.Loop).Update(delegate(HeatCompressor.StatesInstance smi, float dt)
			{
				smi.master.time_active += dt;
				smi.UpdateMeter();
				smi.master.CompressHeat(smi, dt);
			}, UpdateRate.SIM_200ms, false).Transition(this.dropCube, (HeatCompressor.StatesInstance smi) => smi.GetRemainingCharge() >= 1f, UpdateRate.SIM_200ms).Transition(this.inactive, (HeatCompressor.StatesInstance smi) => !smi.CanWork(), UpdateRate.SIM_200ms).Exit(delegate(HeatCompressor.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.dropCube.Enter(delegate(HeatCompressor.StatesInstance smi)
			{
				smi.master.EjectHeatCube();
				smi.GoTo(this.inactive);
			});
		}

		// Token: 0x04006BEF RID: 27631
		public GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor, object>.State active;

		// Token: 0x04006BF0 RID: 27632
		public GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor, object>.State inactive;

		// Token: 0x04006BF1 RID: 27633
		public GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor, object>.State dropCube;
	}
}
