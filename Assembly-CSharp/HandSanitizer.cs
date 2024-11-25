using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006EB RID: 1771
public class HandSanitizer : StateMachineComponent<HandSanitizer.SMInstance>, IGameObjectEffectDescriptor, IBasicBuilding
{
	// Token: 0x06002D12 RID: 11538 RVA: 0x000FD3D3 File Offset: 0x000FB5D3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.FindOrAddComponent<Workable>();
	}

	// Token: 0x06002D13 RID: 11539 RVA: 0x000FD3E8 File Offset: 0x000FB5E8
	private void RefreshMeters()
	{
		float positionPercent = 0f;
		PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(this.consumedElement);
		float num = (float)this.maxUses * this.massConsumedPerUse;
		ConduitConsumer component = base.GetComponent<ConduitConsumer>();
		if (component != null)
		{
			num = component.capacityKG;
		}
		if (primaryElement != null)
		{
			positionPercent = Mathf.Clamp01(primaryElement.Mass / num);
		}
		float positionPercent2 = 0f;
		PrimaryElement primaryElement2 = base.GetComponent<Storage>().FindPrimaryElement(this.outputElement);
		if (primaryElement2 != null)
		{
			positionPercent2 = Mathf.Clamp01(primaryElement2.Mass / ((float)this.maxUses * this.massConsumedPerUse));
		}
		this.cleanMeter.SetPositionPercent(positionPercent);
		this.dirtyMeter.SetPositionPercent(positionPercent2);
	}

	// Token: 0x06002D14 RID: 11540 RVA: 0x000FD4A4 File Offset: 0x000FB6A4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.cleanMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_clean_target", "meter_clean", this.cleanMeterOffset, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_clean_target"
		});
		this.dirtyMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_dirty_target", "meter_dirty", this.dirtyMeterOffset, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_dirty_target"
		});
		this.RefreshMeters();
		Components.HandSanitizers.Add(this);
		Components.BasicBuildings.Add(this);
		base.Subscribe<HandSanitizer>(-1697596308, HandSanitizer.OnStorageChangeDelegate);
		DirectionControl component = base.GetComponent<DirectionControl>();
		component.onDirectionChanged = (Action<WorkableReactable.AllowedDirection>)Delegate.Combine(component.onDirectionChanged, new Action<WorkableReactable.AllowedDirection>(this.OnDirectionChanged));
		this.OnDirectionChanged(base.GetComponent<DirectionControl>().allowedDirection);
	}

	// Token: 0x06002D15 RID: 11541 RVA: 0x000FD589 File Offset: 0x000FB789
	protected override void OnCleanUp()
	{
		Components.BasicBuildings.Remove(this);
		Components.HandSanitizers.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06002D16 RID: 11542 RVA: 0x000FD5A7 File Offset: 0x000FB7A7
	private void OnDirectionChanged(WorkableReactable.AllowedDirection allowed_direction)
	{
		if (this.reactable != null)
		{
			this.reactable.allowedDirection = allowed_direction;
		}
	}

	// Token: 0x06002D17 RID: 11543 RVA: 0x000FD5C0 File Offset: 0x000FB7C0
	public List<Descriptor> RequirementDescriptors()
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, ElementLoader.FindElementByHash(this.consumedElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, ElementLoader.FindElementByHash(this.consumedElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x06002D18 RID: 11544 RVA: 0x000FD644 File Offset: 0x000FB844
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

	// Token: 0x06002D19 RID: 11545 RVA: 0x000FD719 File Offset: 0x000FB919
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.AddRange(this.RequirementDescriptors());
		list.AddRange(this.EffectDescriptors());
		return list;
	}

	// Token: 0x06002D1A RID: 11546 RVA: 0x000FD738 File Offset: 0x000FB938
	private void OnStorageChange(object data)
	{
		if (this.dumpWhenFull && base.smi.OutputFull())
		{
			base.smi.DumpOutput();
		}
		this.RefreshMeters();
	}

	// Token: 0x04001A03 RID: 6659
	public float massConsumedPerUse = 1f;

	// Token: 0x04001A04 RID: 6660
	public SimHashes consumedElement = SimHashes.BleachStone;

	// Token: 0x04001A05 RID: 6661
	public int diseaseRemovalCount = 10000;

	// Token: 0x04001A06 RID: 6662
	public int maxUses = 10;

	// Token: 0x04001A07 RID: 6663
	public SimHashes outputElement = SimHashes.Vacuum;

	// Token: 0x04001A08 RID: 6664
	public bool dumpWhenFull;

	// Token: 0x04001A09 RID: 6665
	public bool alwaysUse;

	// Token: 0x04001A0A RID: 6666
	public bool canSanitizeSuit;

	// Token: 0x04001A0B RID: 6667
	public bool canSanitizeStorage;

	// Token: 0x04001A0C RID: 6668
	private WorkableReactable reactable;

	// Token: 0x04001A0D RID: 6669
	private MeterController cleanMeter;

	// Token: 0x04001A0E RID: 6670
	private MeterController dirtyMeter;

	// Token: 0x04001A0F RID: 6671
	public Meter.Offset cleanMeterOffset;

	// Token: 0x04001A10 RID: 6672
	public Meter.Offset dirtyMeterOffset;

	// Token: 0x04001A11 RID: 6673
	[Serialize]
	public int maxPossiblyRemoved;

	// Token: 0x04001A12 RID: 6674
	private static readonly EventSystem.IntraObjectHandler<HandSanitizer> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<HandSanitizer>(delegate(HandSanitizer component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001512 RID: 5394
	private class WashHandsReactable : WorkableReactable
	{
		// Token: 0x06008D23 RID: 36131 RVA: 0x0033E106 File Offset: 0x0033C306
		public WashHandsReactable(Workable workable, ChoreType chore_type, WorkableReactable.AllowedDirection allowed_direction = WorkableReactable.AllowedDirection.Any) : base(workable, "WashHands", chore_type, allowed_direction)
		{
		}

		// Token: 0x06008D24 RID: 36132 RVA: 0x0033E11C File Offset: 0x0033C31C
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (base.InternalCanBegin(new_reactor, transition))
			{
				HandSanitizer component = this.workable.GetComponent<HandSanitizer>();
				if (!component.smi.IsReady())
				{
					return false;
				}
				if (component.alwaysUse)
				{
					return true;
				}
				PrimaryElement component2 = new_reactor.GetComponent<PrimaryElement>();
				if (component2 != null)
				{
					return component2.DiseaseIdx != byte.MaxValue;
				}
			}
			return false;
		}
	}

	// Token: 0x02001513 RID: 5395
	public class SMInstance : GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.GameInstance
	{
		// Token: 0x06008D25 RID: 36133 RVA: 0x0033E17A File Offset: 0x0033C37A
		public SMInstance(HandSanitizer master) : base(master)
		{
		}

		// Token: 0x06008D26 RID: 36134 RVA: 0x0033E184 File Offset: 0x0033C384
		private bool HasSufficientMass()
		{
			bool result = false;
			PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(base.master.consumedElement);
			if (primaryElement != null)
			{
				result = (primaryElement.Mass >= base.master.massConsumedPerUse);
			}
			return result;
		}

		// Token: 0x06008D27 RID: 36135 RVA: 0x0033E1CC File Offset: 0x0033C3CC
		public bool OutputFull()
		{
			PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(base.master.outputElement);
			return primaryElement != null && primaryElement.Mass >= (float)base.master.maxUses * base.master.massConsumedPerUse;
		}

		// Token: 0x06008D28 RID: 36136 RVA: 0x0033E21E File Offset: 0x0033C41E
		public bool IsReady()
		{
			return this.HasSufficientMass() && !this.OutputFull();
		}

		// Token: 0x06008D29 RID: 36137 RVA: 0x0033E238 File Offset: 0x0033C438
		public void DumpOutput()
		{
			Storage component = base.master.GetComponent<Storage>();
			if (base.master.outputElement != SimHashes.Vacuum)
			{
				component.Drop(ElementLoader.FindElementByHash(base.master.outputElement).tag);
			}
		}
	}

	// Token: 0x02001514 RID: 5396
	public class States : GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer>
	{
		// Token: 0x06008D2A RID: 36138 RVA: 0x0033E280 File Offset: 0x0033C480
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notready;
			this.root.Update(new Action<HandSanitizer.SMInstance, float>(this.UpdateStatusItems), UpdateRate.SIM_200ms, false);
			this.notoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.notready, false);
			this.notready.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.ready, (HandSanitizer.SMInstance smi) => smi.IsReady()).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.DefaultState(this.ready.free).ToggleReactable((HandSanitizer.SMInstance smi) => smi.master.reactable = new HandSanitizer.WashHandsReactable(smi.master.GetComponent<HandSanitizer.Work>(), Db.Get().ChoreTypes.WashHands, smi.master.GetComponent<DirectionControl>().allowedDirection)).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.free.PlayAnim("on").WorkableStartTransition((HandSanitizer.SMInstance smi) => smi.GetComponent<HandSanitizer.Work>(), this.ready.occupied);
			this.ready.occupied.PlayAnim("working_pre").QueueAnim("working_loop", true, null).Enter(delegate(HandSanitizer.SMInstance smi)
			{
				ConduitConsumer component = smi.GetComponent<ConduitConsumer>();
				if (component != null)
				{
					component.enabled = false;
				}
			}).Exit(delegate(HandSanitizer.SMInstance smi)
			{
				ConduitConsumer component = smi.GetComponent<ConduitConsumer>();
				if (component != null)
				{
					component.enabled = true;
				}
			}).WorkableStopTransition((HandSanitizer.SMInstance smi) => smi.GetComponent<HandSanitizer.Work>(), this.notready);
		}

		// Token: 0x06008D2B RID: 36139 RVA: 0x0033E448 File Offset: 0x0033C648
		private void UpdateStatusItems(HandSanitizer.SMInstance smi, float dt)
		{
			if (smi.OutputFull())
			{
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, this);
				return;
			}
			smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, false);
		}

		// Token: 0x04006BE5 RID: 27621
		public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State notready;

		// Token: 0x04006BE6 RID: 27622
		public HandSanitizer.States.ReadyStates ready;

		// Token: 0x04006BE7 RID: 27623
		public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State notoperational;

		// Token: 0x04006BE8 RID: 27624
		public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State full;

		// Token: 0x04006BE9 RID: 27625
		public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State empty;

		// Token: 0x020024EC RID: 9452
		public class ReadyStates : GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State
		{
			// Token: 0x0400A433 RID: 42035
			public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State free;

			// Token: 0x0400A434 RID: 42036
			public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State occupied;
		}
	}

	// Token: 0x02001515 RID: 5397
	[AddComponentMenu("KMonoBehaviour/Workable/Work")]
	public class Work : Workable, IGameObjectEffectDescriptor
	{
		// Token: 0x06008D2D RID: 36141 RVA: 0x0033E4A8 File Offset: 0x0033C6A8
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.resetProgressOnStop = true;
			this.shouldTransferDiseaseWithWorker = false;
			GameScheduler.Instance.Schedule("WaterFetchingTutorial", 2f, delegate(object obj)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_FetchingWater, true);
			}, null, null);
		}

		// Token: 0x06008D2E RID: 36142 RVA: 0x0033E500 File Offset: 0x0033C700
		public override Workable.AnimInfo GetAnim(WorkerBase worker)
		{
			KAnimFile[] overrideAnims = null;
			if (this.workerTypeOverrideAnims.TryGetValue(worker.PrefabID(), out overrideAnims))
			{
				this.overrideAnims = overrideAnims;
			}
			return base.GetAnim(worker);
		}

		// Token: 0x06008D2F RID: 36143 RVA: 0x0033E532 File Offset: 0x0033C732
		protected override void OnStartWork(WorkerBase worker)
		{
			base.OnStartWork(worker);
			this.diseaseRemoved = 0;
		}

		// Token: 0x06008D30 RID: 36144 RVA: 0x0033E544 File Offset: 0x0033C744
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			base.OnWorkTick(worker, dt);
			HandSanitizer component = base.GetComponent<HandSanitizer>();
			Storage component2 = base.GetComponent<Storage>();
			float massAvailable = component2.GetMassAvailable(component.consumedElement);
			if (massAvailable == 0f)
			{
				return true;
			}
			PrimaryElement component3 = worker.GetComponent<PrimaryElement>();
			float amount = Mathf.Min(component.massConsumedPerUse * dt / this.workTime, massAvailable);
			int num = Math.Min((int)(dt / this.workTime * (float)component.diseaseRemovalCount), component3.DiseaseCount);
			this.diseaseRemoved += num;
			SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
			invalid.idx = component3.DiseaseIdx;
			invalid.count = num;
			component3.ModifyDiseaseCount(-num, "HandSanitizer.OnWorkTick");
			component.maxPossiblyRemoved += num;
			if (component.canSanitizeStorage && worker.GetComponent<Storage>())
			{
				foreach (GameObject gameObject in worker.GetComponent<Storage>().GetItems())
				{
					PrimaryElement component4 = gameObject.GetComponent<PrimaryElement>();
					if (component4)
					{
						int num2 = Math.Min((int)(dt / this.workTime * (float)component.diseaseRemovalCount), component4.DiseaseCount);
						component4.ModifyDiseaseCount(-num2, "HandSanitizer.OnWorkTick");
						component.maxPossiblyRemoved += num2;
					}
				}
			}
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
			float mass;
			float temperature;
			component2.ConsumeAndGetDisease(ElementLoader.FindElementByHash(component.consumedElement).tag, amount, out mass, out diseaseInfo, out temperature);
			if (component.outputElement != SimHashes.Vacuum)
			{
				diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(invalid, diseaseInfo);
				component2.AddLiquid(component.outputElement, mass, temperature, diseaseInfo.idx, diseaseInfo.count, false, true);
			}
			return false;
		}

		// Token: 0x06008D31 RID: 36145 RVA: 0x0033E710 File Offset: 0x0033C910
		protected override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);
			if (this.removeIrritation && !worker.HasTag(GameTags.HasSuitTank))
			{
				GasLiquidExposureMonitor.Instance smi = worker.GetSMI<GasLiquidExposureMonitor.Instance>();
				if (smi != null)
				{
					smi.ResetExposure();
				}
			}
		}

		// Token: 0x04006BEA RID: 27626
		public Dictionary<Tag, KAnimFile[]> workerTypeOverrideAnims = new Dictionary<Tag, KAnimFile[]>();

		// Token: 0x04006BEB RID: 27627
		public bool removeIrritation;

		// Token: 0x04006BEC RID: 27628
		private int diseaseRemoved;
	}
}
