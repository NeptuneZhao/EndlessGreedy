using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200074E RID: 1870
[SerializationConfig(MemberSerialization.OptIn)]
public class Polymerizer : StateMachineComponent<Polymerizer.StatesInstance>
{
	// Token: 0x060031D5 RID: 12757 RVA: 0x00112348 File Offset: 0x00110548
	protected override void OnSpawn()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.plasticMeter = new MeterController(component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0f, 0f, 0f), null);
		this.oilMeter = new MeterController(component, "meter2_target", "meter2", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0f, 0f, 0f), null);
		component.SetSymbolVisiblity("meter_target", true);
		this.UpdateOilMeter();
		base.smi.StartSM();
		base.Subscribe<Polymerizer>(-1697596308, Polymerizer.OnStorageChangedDelegate);
	}

	// Token: 0x060031D6 RID: 12758 RVA: 0x001123EC File Offset: 0x001105EC
	private void TryEmit()
	{
		GameObject gameObject = this.storage.FindFirst(this.emitTag);
		if (gameObject != null)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			this.UpdatePercentDone(component);
			this.TryEmit(component);
		}
	}

	// Token: 0x060031D7 RID: 12759 RVA: 0x0011242C File Offset: 0x0011062C
	private void TryEmit(PrimaryElement primary_elem)
	{
		if (primary_elem.Mass >= this.emitMass)
		{
			this.plasticMeter.SetPositionPercent(0f);
			GameObject gameObject = this.storage.Drop(primary_elem.gameObject, true);
			Rotatable component = base.GetComponent<Rotatable>();
			Vector3 vector = component.transform.GetPosition() + component.GetRotatedOffset(this.emitOffset);
			int i = Grid.PosToCell(vector);
			if (Grid.Solid[i])
			{
				vector += component.GetRotatedOffset(Vector3.left);
			}
			gameObject.transform.SetPosition(vector);
			PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.exhaustElement);
			if (primaryElement != null)
			{
				SimMessages.AddRemoveSubstance(Grid.PosToCell(vector), primaryElement.ElementID, null, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount, true, -1);
				primaryElement.Mass = 0f;
				primaryElement.ModifyDiseaseCount(int.MinValue, "Polymerizer.Exhaust");
			}
		}
	}

	// Token: 0x060031D8 RID: 12760 RVA: 0x00112524 File Offset: 0x00110724
	private void UpdatePercentDone(PrimaryElement primary_elem)
	{
		float positionPercent = Mathf.Clamp01(primary_elem.Mass / this.emitMass);
		this.plasticMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x060031D9 RID: 12761 RVA: 0x00112550 File Offset: 0x00110750
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		if (gameObject.HasTag(PolymerizerConfig.INPUT_ELEMENT_TAG))
		{
			this.UpdateOilMeter();
		}
	}

	// Token: 0x060031DA RID: 12762 RVA: 0x00112584 File Offset: 0x00110784
	private void UpdateOilMeter()
	{
		float num = 0f;
		foreach (GameObject gameObject in this.storage.items)
		{
			if (gameObject.HasTag(PolymerizerConfig.INPUT_ELEMENT_TAG))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				num += component.Mass;
			}
		}
		float positionPercent = Mathf.Clamp01(num / this.consumer.capacityKG);
		this.oilMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x04001D54 RID: 7508
	[SerializeField]
	public float maxMass = 2.5f;

	// Token: 0x04001D55 RID: 7509
	[SerializeField]
	public float emitMass = 1f;

	// Token: 0x04001D56 RID: 7510
	[SerializeField]
	public Tag emitTag;

	// Token: 0x04001D57 RID: 7511
	[SerializeField]
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x04001D58 RID: 7512
	[SerializeField]
	public SimHashes exhaustElement = SimHashes.Vacuum;

	// Token: 0x04001D59 RID: 7513
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04001D5A RID: 7514
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001D5B RID: 7515
	[MyCmpGet]
	private ConduitConsumer consumer;

	// Token: 0x04001D5C RID: 7516
	[MyCmpGet]
	private ElementConverter converter;

	// Token: 0x04001D5D RID: 7517
	private MeterController plasticMeter;

	// Token: 0x04001D5E RID: 7518
	private MeterController oilMeter;

	// Token: 0x04001D5F RID: 7519
	private static readonly EventSystem.IntraObjectHandler<Polymerizer> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Polymerizer>(delegate(Polymerizer component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x020015BE RID: 5566
	public class StatesInstance : GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.GameInstance
	{
		// Token: 0x06008FA3 RID: 36771 RVA: 0x003482F9 File Offset: 0x003464F9
		public StatesInstance(Polymerizer smi) : base(smi)
		{
		}
	}

	// Token: 0x020015BF RID: 5567
	public class States : GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer>
	{
		// Token: 0x06008FA4 RID: 36772 RVA: 0x00348304 File Offset: 0x00346504
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.EventTransition(GameHashes.OperationalChanged, this.off, (Polymerizer.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.off.EventTransition(GameHashes.OperationalChanged, this.on, (Polymerizer.StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.EventTransition(GameHashes.OnStorageChange, this.converting, (Polymerizer.StatesInstance smi) => smi.master.converter.CanConvertAtAll());
			this.converting.Enter("Ready", delegate(Polymerizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).EventHandler(GameHashes.OnStorageChange, delegate(Polymerizer.StatesInstance smi)
			{
				smi.master.TryEmit();
			}).EventTransition(GameHashes.OnStorageChange, this.on, (Polymerizer.StatesInstance smi) => !smi.master.converter.CanConvertAtAll()).Exit("Ready", delegate(Polymerizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
		}

		// Token: 0x04006DA3 RID: 28067
		public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State off;

		// Token: 0x04006DA4 RID: 28068
		public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State on;

		// Token: 0x04006DA5 RID: 28069
		public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State converting;
	}
}
