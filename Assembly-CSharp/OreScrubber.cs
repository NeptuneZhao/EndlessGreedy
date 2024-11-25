using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000743 RID: 1859
public class OreScrubber : StateMachineComponent<OreScrubber.SMInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06003187 RID: 12679 RVA: 0x00110AD0 File Offset: 0x0010ECD0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.FindOrAddComponent<Workable>();
	}

	// Token: 0x06003188 RID: 12680 RVA: 0x00110AE4 File Offset: 0x0010ECE4
	private void RefreshMeters()
	{
		float positionPercent = 0f;
		PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(this.consumedElement);
		if (primaryElement != null)
		{
			positionPercent = Mathf.Clamp01(primaryElement.Mass / base.GetComponent<ConduitConsumer>().capacityKG);
		}
		this.cleanMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06003189 RID: 12681 RVA: 0x00110B38 File Offset: 0x0010ED38
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.cleanMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_clean_target", "meter_clean", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_clean_target"
		});
		this.RefreshMeters();
		base.Subscribe<OreScrubber>(-1697596308, OreScrubber.OnStorageChangeDelegate);
		DirectionControl component = base.GetComponent<DirectionControl>();
		component.onDirectionChanged = (Action<WorkableReactable.AllowedDirection>)Delegate.Combine(component.onDirectionChanged, new Action<WorkableReactable.AllowedDirection>(this.OnDirectionChanged));
		this.OnDirectionChanged(base.GetComponent<DirectionControl>().allowedDirection);
	}

	// Token: 0x0600318A RID: 12682 RVA: 0x00110BD1 File Offset: 0x0010EDD1
	private void OnDirectionChanged(WorkableReactable.AllowedDirection allowed_direction)
	{
		if (this.reactable != null)
		{
			this.reactable.allowedDirection = allowed_direction;
		}
	}

	// Token: 0x0600318B RID: 12683 RVA: 0x00110BE8 File Offset: 0x0010EDE8
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string name = ElementLoader.FindElementByHash(this.consumedElement).name;
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		return list;
	}

	// Token: 0x0600318C RID: 12684 RVA: 0x00110C60 File Offset: 0x0010EE60
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.outputElement != SimHashes.Vacuum)
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTEDPERUSE, ElementLoader.FindElementByHash(this.outputElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTEDPERUSE, ElementLoader.FindElementByHash(this.outputElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Effect, false));
		}
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.DISEASECONSUMEDPERUSE, GameUtil.GetFormattedDiseaseAmount(this.diseaseRemovalCount, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DISEASECONSUMEDPERUSE, GameUtil.GetFormattedDiseaseAmount(this.diseaseRemovalCount, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x0600318D RID: 12685 RVA: 0x00110D35 File Offset: 0x0010EF35
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.AddRange(this.RequirementDescriptors());
		list.AddRange(this.EffectDescriptors());
		return list;
	}

	// Token: 0x0600318E RID: 12686 RVA: 0x00110D54 File Offset: 0x0010EF54
	private void OnStorageChange(object data)
	{
		this.RefreshMeters();
	}

	// Token: 0x0600318F RID: 12687 RVA: 0x00110D5C File Offset: 0x0010EF5C
	private static PrimaryElement GetFirstInfected(Storage storage)
	{
		foreach (GameObject gameObject in storage.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.DiseaseIdx != 255 && !gameObject.HasTag(GameTags.Edible))
				{
					return component;
				}
			}
		}
		return null;
	}

	// Token: 0x04001D1C RID: 7452
	public float massConsumedPerUse = 1f;

	// Token: 0x04001D1D RID: 7453
	public SimHashes consumedElement = SimHashes.BleachStone;

	// Token: 0x04001D1E RID: 7454
	public int diseaseRemovalCount = 10000;

	// Token: 0x04001D1F RID: 7455
	public SimHashes outputElement = SimHashes.Vacuum;

	// Token: 0x04001D20 RID: 7456
	private WorkableReactable reactable;

	// Token: 0x04001D21 RID: 7457
	private MeterController cleanMeter;

	// Token: 0x04001D22 RID: 7458
	[Serialize]
	public int maxPossiblyRemoved;

	// Token: 0x04001D23 RID: 7459
	private static readonly EventSystem.IntraObjectHandler<OreScrubber> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<OreScrubber>(delegate(OreScrubber component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x020015AD RID: 5549
	private class ScrubOreReactable : WorkableReactable
	{
		// Token: 0x06008F78 RID: 36728 RVA: 0x00347B2B File Offset: 0x00345D2B
		public ScrubOreReactable(Workable workable, ChoreType chore_type, WorkableReactable.AllowedDirection allowed_direction = WorkableReactable.AllowedDirection.Any) : base(workable, "ScrubOre", chore_type, allowed_direction)
		{
		}

		// Token: 0x06008F79 RID: 36729 RVA: 0x00347B40 File Offset: 0x00345D40
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (base.InternalCanBegin(new_reactor, transition))
			{
				Storage component = new_reactor.GetComponent<Storage>();
				if (component != null && OreScrubber.GetFirstInfected(component) != null)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x020015AE RID: 5550
	public class SMInstance : GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.GameInstance
	{
		// Token: 0x06008F7A RID: 36730 RVA: 0x00347B78 File Offset: 0x00345D78
		public SMInstance(OreScrubber master) : base(master)
		{
		}

		// Token: 0x06008F7B RID: 36731 RVA: 0x00347B84 File Offset: 0x00345D84
		public bool HasSufficientMass()
		{
			bool result = false;
			PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(base.master.consumedElement);
			if (primaryElement != null)
			{
				result = (primaryElement.Mass > 0f);
			}
			return result;
		}

		// Token: 0x06008F7C RID: 36732 RVA: 0x00347BC2 File Offset: 0x00345DC2
		public Dictionary<Tag, float> GetNeededMass()
		{
			return new Dictionary<Tag, float>
			{
				{
					base.master.consumedElement.CreateTag(),
					base.master.massConsumedPerUse
				}
			};
		}

		// Token: 0x06008F7D RID: 36733 RVA: 0x00347BEA File Offset: 0x00345DEA
		public void OnCompleteWork(WorkerBase worker)
		{
		}

		// Token: 0x06008F7E RID: 36734 RVA: 0x00347BEC File Offset: 0x00345DEC
		public void DumpOutput()
		{
			Storage component = base.master.GetComponent<Storage>();
			if (base.master.outputElement != SimHashes.Vacuum)
			{
				component.Drop(ElementLoader.FindElementByHash(base.master.outputElement).tag);
			}
		}
	}

	// Token: 0x020015AF RID: 5551
	public class States : GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber>
	{
		// Token: 0x06008F7F RID: 36735 RVA: 0x00347C34 File Offset: 0x00345E34
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notready;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.notoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.notready, false);
			this.notready.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.ready, (OreScrubber.SMInstance smi) => smi.HasSufficientMass()).ToggleStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailable, (OreScrubber.SMInstance smi) => smi.GetNeededMass()).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.DefaultState(this.ready.free).ToggleReactable((OreScrubber.SMInstance smi) => smi.master.reactable = new OreScrubber.ScrubOreReactable(smi.master.GetComponent<OreScrubber.Work>(), Db.Get().ChoreTypes.ScrubOre, smi.master.GetComponent<DirectionControl>().allowedDirection)).EventTransition(GameHashes.OnStorageChange, this.notready, (OreScrubber.SMInstance smi) => !smi.HasSufficientMass()).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.free.PlayAnim("on").WorkableStartTransition((OreScrubber.SMInstance smi) => smi.GetComponent<OreScrubber.Work>(), this.ready.occupied);
			this.ready.occupied.PlayAnim("working_pre").QueueAnim("working_loop", true, null).WorkableStopTransition((OreScrubber.SMInstance smi) => smi.GetComponent<OreScrubber.Work>(), this.ready);
		}

		// Token: 0x04006D8C RID: 28044
		public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State notready;

		// Token: 0x04006D8D RID: 28045
		public OreScrubber.States.ReadyStates ready;

		// Token: 0x04006D8E RID: 28046
		public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State notoperational;

		// Token: 0x04006D8F RID: 28047
		public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State full;

		// Token: 0x04006D90 RID: 28048
		public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State empty;

		// Token: 0x02002514 RID: 9492
		public class ReadyStates : GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State
		{
			// Token: 0x0400A509 RID: 42249
			public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State free;

			// Token: 0x0400A50A RID: 42250
			public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State occupied;
		}
	}

	// Token: 0x020015B0 RID: 5552
	[AddComponentMenu("KMonoBehaviour/Workable/Work")]
	public class Work : Workable, IGameObjectEffectDescriptor
	{
		// Token: 0x06008F81 RID: 36737 RVA: 0x00347E0A File Offset: 0x0034600A
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.resetProgressOnStop = true;
			this.shouldTransferDiseaseWithWorker = false;
		}

		// Token: 0x06008F82 RID: 36738 RVA: 0x00347E20 File Offset: 0x00346020
		protected override void OnStartWork(WorkerBase worker)
		{
			base.OnStartWork(worker);
			this.diseaseRemoved = 0;
		}

		// Token: 0x06008F83 RID: 36739 RVA: 0x00347E30 File Offset: 0x00346030
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			base.OnWorkTick(worker, dt);
			OreScrubber component = base.GetComponent<OreScrubber>();
			Storage component2 = base.GetComponent<Storage>();
			PrimaryElement firstInfected = OreScrubber.GetFirstInfected(worker.GetComponent<Storage>());
			int num = 0;
			SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
			if (firstInfected != null)
			{
				num = Math.Min((int)(dt / this.workTime * (float)component.diseaseRemovalCount), firstInfected.DiseaseCount);
				this.diseaseRemoved += num;
				invalid.idx = firstInfected.DiseaseIdx;
				invalid.count = num;
				firstInfected.ModifyDiseaseCount(-num, "OreScrubber.OnWorkTick");
			}
			component.maxPossiblyRemoved += num;
			float amount = component.massConsumedPerUse * dt / this.workTime;
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
			float mass;
			float temperature;
			component2.ConsumeAndGetDisease(ElementLoader.FindElementByHash(component.consumedElement).tag, amount, out mass, out diseaseInfo, out temperature);
			if (component.outputElement != SimHashes.Vacuum)
			{
				diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(invalid, diseaseInfo);
				component2.AddLiquid(component.outputElement, mass, temperature, diseaseInfo.idx, diseaseInfo.count, false, true);
			}
			return this.diseaseRemoved > component.diseaseRemovalCount;
		}

		// Token: 0x06008F84 RID: 36740 RVA: 0x00347F4A File Offset: 0x0034614A
		protected override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);
		}

		// Token: 0x04006D91 RID: 28049
		private int diseaseRemoved;
	}
}
