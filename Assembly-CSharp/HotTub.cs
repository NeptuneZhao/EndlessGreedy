using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020008EF RID: 2287
[SerializationConfig(MemberSerialization.OptIn)]
public class HotTub : StateMachineComponent<HotTub.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x170004D6 RID: 1238
	// (get) Token: 0x060041CB RID: 16843 RVA: 0x00175084 File Offset: 0x00173284
	public float PercentFull
	{
		get
		{
			return 100f * this.waterStorage.GetMassAvailable(SimHashes.Water) / this.hotTubCapacity;
		}
	}

	// Token: 0x060041CC RID: 16844 RVA: 0x001750A4 File Offset: 0x001732A4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
		this.workables = new HotTubWorkable[this.choreOffsets.Length];
		this.chores = new Chore[this.choreOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.choreOffsets[i]), Grid.SceneLayer.Move);
			GameObject go = ChoreHelpers.CreateLocator("HotTubWorkable", pos);
			KSelectable kselectable = go.AddOrGet<KSelectable>();
			kselectable.SetName(this.GetProperName());
			kselectable.IsSelectable = false;
			HotTubWorkable hotTubWorkable = go.AddOrGet<HotTubWorkable>();
			int player_index = i;
			HotTubWorkable hotTubWorkable2 = hotTubWorkable;
			hotTubWorkable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(hotTubWorkable2.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(delegate(Workable workable, Workable.WorkableEvent ev)
			{
				this.OnWorkableEvent(player_index, ev);
			}));
			this.workables[i] = hotTubWorkable;
			this.workables[i].hotTub = this;
		}
		this.waterMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_water_target", "meter_water", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_water_target"
		});
		base.smi.UpdateWaterMeter();
		this.tempMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_temperature_target", "meter_temp", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_temperature_target"
		});
		base.smi.TestWaterTemperature();
		base.smi.StartSM();
	}

	// Token: 0x060041CD RID: 16845 RVA: 0x0017523C File Offset: 0x0017343C
	protected override void OnCleanUp()
	{
		this.UpdateChores(false);
		for (int i = 0; i < this.workables.Length; i++)
		{
			if (this.workables[i])
			{
				Util.KDestroyGameObject(this.workables[i]);
				this.workables[i] = null;
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x060041CE RID: 16846 RVA: 0x00175290 File Offset: 0x00173490
	private Chore CreateChore(int i)
	{
		Workable workable = this.workables[i];
		ChoreType relax = Db.Get().ChoreTypes.Relax;
		IStateMachineTarget target = workable;
		ChoreProvider chore_provider = null;
		bool run_until_complete = true;
		Action<Chore> on_complete = null;
		Action<Chore> on_begin = null;
		ScheduleBlockType recreation = Db.Get().ScheduleBlockTypes.Recreation;
		WorkChore<HotTubWorkable> workChore = new WorkChore<HotTubWorkable>(relax, target, chore_provider, run_until_complete, on_complete, on_begin, new Action<Chore>(this.OnSocialChoreEnd), false, recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, workable);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotABionic, workable);
		return workChore;
	}

	// Token: 0x060041CF RID: 16847 RVA: 0x00175309 File Offset: 0x00173509
	private void OnSocialChoreEnd(Chore chore)
	{
		if (base.gameObject.HasTag(GameTags.Operational))
		{
			this.UpdateChores(true);
		}
	}

	// Token: 0x060041D0 RID: 16848 RVA: 0x00175324 File Offset: 0x00173524
	public void UpdateChores(bool update = true)
	{
		for (int i = 0; i < this.choreOffsets.Length; i++)
		{
			Chore chore = this.chores[i];
			if (update)
			{
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = this.CreateChore(i);
				}
			}
			else if (chore != null)
			{
				chore.Cancel("locator invalidated");
				this.chores[i] = null;
			}
		}
	}

	// Token: 0x060041D1 RID: 16849 RVA: 0x00175384 File Offset: 0x00173584
	public void OnWorkableEvent(int player, Workable.WorkableEvent ev)
	{
		if (ev == Workable.WorkableEvent.WorkStarted)
		{
			this.occupants.Add(player);
		}
		else
		{
			this.occupants.Remove(player);
		}
		base.smi.sm.userCount.Set(this.occupants.Count, base.smi, false);
	}

	// Token: 0x060041D2 RID: 16850 RVA: 0x001753DC File Offset: 0x001735DC
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Element element = ElementLoader.FindElementByHash(SimHashes.Water);
		list.Add(new Descriptor(BUILDINGS.PREFABS.HOTTUB.WATER_REQUIREMENT.Replace("{element}", element.name).Replace("{amount}", GameUtil.GetFormattedMass(this.hotTubCapacity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), BUILDINGS.PREFABS.HOTTUB.WATER_REQUIREMENT_TOOLTIP.Replace("{element}", element.name).Replace("{amount}", GameUtil.GetFormattedMass(this.hotTubCapacity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		list.Add(new Descriptor(BUILDINGS.PREFABS.HOTTUB.TEMPERATURE_REQUIREMENT.Replace("{element}", element.name).Replace("{temperature}", GameUtil.GetFormattedTemperature(this.minimumWaterTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), BUILDINGS.PREFABS.HOTTUB.TEMPERATURE_REQUIREMENT_TOOLTIP.Replace("{element}", element.name).Replace("{temperature}", GameUtil.GetFormattedTemperature(this.minimumWaterTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Requirement, false));
		list.Add(new Descriptor(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_TOOLTIP"), Descriptor.DescriptorType.Effect, false));
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect, false));
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		return list;
	}

	// Token: 0x04002B97 RID: 11159
	public string specificEffect;

	// Token: 0x04002B98 RID: 11160
	public string trackingEffect;

	// Token: 0x04002B99 RID: 11161
	public int basePriority;

	// Token: 0x04002B9A RID: 11162
	public CellOffset[] choreOffsets = new CellOffset[]
	{
		new CellOffset(-1, 0),
		new CellOffset(1, 0),
		new CellOffset(0, 0),
		new CellOffset(2, 0)
	};

	// Token: 0x04002B9B RID: 11163
	private HotTubWorkable[] workables;

	// Token: 0x04002B9C RID: 11164
	private Chore[] chores;

	// Token: 0x04002B9D RID: 11165
	public HashSet<int> occupants = new HashSet<int>();

	// Token: 0x04002B9E RID: 11166
	public float waterCoolingRate;

	// Token: 0x04002B9F RID: 11167
	public float hotTubCapacity = 100f;

	// Token: 0x04002BA0 RID: 11168
	public float minimumWaterTemperature;

	// Token: 0x04002BA1 RID: 11169
	public float bleachStoneConsumption;

	// Token: 0x04002BA2 RID: 11170
	public float maxOperatingTemperature;

	// Token: 0x04002BA3 RID: 11171
	[MyCmpGet]
	public Storage waterStorage;

	// Token: 0x04002BA4 RID: 11172
	private MeterController waterMeter;

	// Token: 0x04002BA5 RID: 11173
	private MeterController tempMeter;

	// Token: 0x0200185C RID: 6236
	public class States : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub>
	{
		// Token: 0x0600980A RID: 38922 RVA: 0x00366AFC File Offset: 0x00364CFC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.ready;
			this.root.Update(delegate(HotTub.StatesInstance smi, float dt)
			{
				smi.SapHeatFromWater(dt);
				smi.TestWaterTemperature();
			}, UpdateRate.SIM_4000ms, false).EventHandler(GameHashes.OnStorageChange, delegate(HotTub.StatesInstance smi)
			{
				smi.UpdateWaterMeter();
				smi.TestWaterTemperature();
			});
			this.unoperational.TagTransition(GameTags.Operational, this.off, false).PlayAnim("off");
			this.off.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.off.filling);
			this.off.filling.DefaultState(this.off.filling.normal).Transition(this.ready, (HotTub.StatesInstance smi) => smi.master.waterStorage.GetMassAvailable(SimHashes.Water) >= smi.master.hotTubCapacity, UpdateRate.SIM_200ms).PlayAnim("off").Enter(delegate(HotTub.StatesInstance smi)
			{
				smi.GetComponent<ConduitConsumer>().SetOnState(true);
			}).Exit(delegate(HotTub.StatesInstance smi)
			{
				smi.GetComponent<ConduitConsumer>().SetOnState(false);
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.HotTubFilling, (HotTub.StatesInstance smi) => smi.master);
			this.off.filling.normal.ParamTransition<bool>(this.waterTooCold, this.off.filling.too_cold, GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.IsTrue);
			this.off.filling.too_cold.ParamTransition<bool>(this.waterTooCold, this.off.filling.normal, GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.HotTubWaterTooCold, (HotTub.StatesInstance smi) => smi.master);
			this.off.draining.Transition(this.off.filling, (HotTub.StatesInstance smi) => smi.master.waterStorage.GetMassAvailable(SimHashes.Water) <= 0f, UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.HotTubWaterTooCold, (HotTub.StatesInstance smi) => smi.master).PlayAnim("off").Enter(delegate(HotTub.StatesInstance smi)
			{
				smi.GetComponent<ConduitDispenser>().SetOnState(true);
			}).Exit(delegate(HotTub.StatesInstance smi)
			{
				smi.GetComponent<ConduitDispenser>().SetOnState(false);
			});
			this.off.too_hot.Transition(this.ready, (HotTub.StatesInstance smi) => !smi.IsTubTooHot(), UpdateRate.SIM_200ms).PlayAnim("overheated").ToggleMainStatusItem(Db.Get().BuildingStatusItems.HotTubTooHot, (HotTub.StatesInstance smi) => smi.master);
			this.off.awaiting_delivery.EventTransition(GameHashes.OnStorageChange, this.ready, (HotTub.StatesInstance smi) => smi.HasBleachStone());
			this.ready.DefaultState(this.ready.idle).Enter("CreateChore", delegate(HotTub.StatesInstance smi)
			{
				smi.master.UpdateChores(true);
			}).Exit("CancelChore", delegate(HotTub.StatesInstance smi)
			{
				smi.master.UpdateChores(false);
			}).TagTransition(GameTags.Operational, this.unoperational, true).ParamTransition<bool>(this.waterTooCold, this.off.draining, GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.IsTrue).EventTransition(GameHashes.OnStorageChange, this.off.awaiting_delivery, (HotTub.StatesInstance smi) => !smi.HasBleachStone()).Transition(this.off.filling, (HotTub.StatesInstance smi) => smi.master.waterStorage.IsEmpty(), UpdateRate.SIM_200ms).Transition(this.off.too_hot, (HotTub.StatesInstance smi) => smi.IsTubTooHot(), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null);
			this.ready.idle.PlayAnim("on").ParamTransition<int>(this.userCount, this.ready.on.pre, (HotTub.StatesInstance smi, int p) => p > 0);
			this.ready.on.Enter(delegate(HotTub.StatesInstance smi)
			{
				smi.SetActive(true);
			}).Update(delegate(HotTub.StatesInstance smi, float dt)
			{
				smi.ConsumeBleachstone(dt);
			}, UpdateRate.SIM_4000ms, false).Exit(delegate(HotTub.StatesInstance smi)
			{
				smi.SetActive(false);
			});
			this.ready.on.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.ready.on.relaxing);
			this.ready.on.relaxing.PlayAnim("working_loop", KAnim.PlayMode.Loop).ParamTransition<int>(this.userCount, this.ready.on.post, (HotTub.StatesInstance smi, int p) => p == 0).ParamTransition<int>(this.userCount, this.ready.on.relaxing_together, (HotTub.StatesInstance smi, int p) => p > 1);
			this.ready.on.relaxing_together.PlayAnim("working_loop", KAnim.PlayMode.Loop).ParamTransition<int>(this.userCount, this.ready.on.post, (HotTub.StatesInstance smi, int p) => p == 0).ParamTransition<int>(this.userCount, this.ready.on.relaxing, (HotTub.StatesInstance smi, int p) => p == 1);
			this.ready.on.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready.idle);
		}

		// Token: 0x0600980B RID: 38923 RVA: 0x0036720C File Offset: 0x0036540C
		private string GetRelaxingAnim(HotTub.StatesInstance smi)
		{
			bool flag = smi.master.occupants.Contains(0);
			bool flag2 = smi.master.occupants.Contains(1);
			if (flag && !flag2)
			{
				return "working_loop_one_p";
			}
			if (flag2 && !flag)
			{
				return "working_loop_two_p";
			}
			return "working_loop_coop_p";
		}

		// Token: 0x040075AB RID: 30123
		public StateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.IntParameter userCount;

		// Token: 0x040075AC RID: 30124
		public StateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.BoolParameter waterTooCold;

		// Token: 0x040075AD RID: 30125
		public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State unoperational;

		// Token: 0x040075AE RID: 30126
		public HotTub.States.OffStates off;

		// Token: 0x040075AF RID: 30127
		public HotTub.States.ReadyStates ready;

		// Token: 0x020025A7 RID: 9639
		public class OffStates : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State
		{
			// Token: 0x0400A7B2 RID: 42930
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State draining;

			// Token: 0x0400A7B3 RID: 42931
			public HotTub.States.FillingStates filling;

			// Token: 0x0400A7B4 RID: 42932
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State too_hot;

			// Token: 0x0400A7B5 RID: 42933
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State awaiting_delivery;
		}

		// Token: 0x020025A8 RID: 9640
		public class OnStates : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State
		{
			// Token: 0x0400A7B6 RID: 42934
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State pre;

			// Token: 0x0400A7B7 RID: 42935
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State relaxing;

			// Token: 0x0400A7B8 RID: 42936
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State relaxing_together;

			// Token: 0x0400A7B9 RID: 42937
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State post;
		}

		// Token: 0x020025A9 RID: 9641
		public class ReadyStates : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State
		{
			// Token: 0x0400A7BA RID: 42938
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State idle;

			// Token: 0x0400A7BB RID: 42939
			public HotTub.States.OnStates on;
		}

		// Token: 0x020025AA RID: 9642
		public class FillingStates : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State
		{
			// Token: 0x0400A7BC RID: 42940
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State normal;

			// Token: 0x0400A7BD RID: 42941
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State too_cold;
		}
	}

	// Token: 0x0200185D RID: 6237
	public class StatesInstance : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.GameInstance
	{
		// Token: 0x0600980D RID: 38925 RVA: 0x00367262 File Offset: 0x00365462
		public StatesInstance(HotTub smi) : base(smi)
		{
			this.operational = base.master.GetComponent<Operational>();
		}

		// Token: 0x0600980E RID: 38926 RVA: 0x0036727C File Offset: 0x0036547C
		public void SetActive(bool active)
		{
			this.operational.SetActive(this.operational.IsOperational && active, false);
		}

		// Token: 0x0600980F RID: 38927 RVA: 0x00367298 File Offset: 0x00365498
		public void UpdateWaterMeter()
		{
			base.smi.master.waterMeter.SetPositionPercent(Mathf.Clamp(base.smi.master.waterStorage.GetMassAvailable(SimHashes.Water) / base.smi.master.hotTubCapacity, 0f, 1f));
		}

		// Token: 0x06009810 RID: 38928 RVA: 0x003672F4 File Offset: 0x003654F4
		public void UpdateTemperatureMeter(float waterTemp)
		{
			Element element = ElementLoader.GetElement(SimHashes.Water.CreateTag());
			base.smi.master.tempMeter.SetPositionPercent(Mathf.Clamp((waterTemp - base.smi.master.minimumWaterTemperature) / (element.highTemp - base.smi.master.minimumWaterTemperature), 0f, 1f));
		}

		// Token: 0x06009811 RID: 38929 RVA: 0x00367360 File Offset: 0x00365560
		public void TestWaterTemperature()
		{
			GameObject gameObject = base.smi.master.waterStorage.FindFirst(new Tag(1836671383));
			float num = 0f;
			if (!gameObject)
			{
				this.UpdateTemperatureMeter(num);
				base.smi.sm.waterTooCold.Set(false, base.smi, false);
				return;
			}
			num = gameObject.GetComponent<PrimaryElement>().Temperature;
			this.UpdateTemperatureMeter(num);
			if (num < base.smi.master.minimumWaterTemperature)
			{
				base.smi.sm.waterTooCold.Set(true, base.smi, false);
				return;
			}
			base.smi.sm.waterTooCold.Set(false, base.smi, false);
		}

		// Token: 0x06009812 RID: 38930 RVA: 0x00367424 File Offset: 0x00365624
		public bool IsTubTooHot()
		{
			return base.smi.master.GetComponent<PrimaryElement>().Temperature > base.smi.master.maxOperatingTemperature;
		}

		// Token: 0x06009813 RID: 38931 RVA: 0x00367450 File Offset: 0x00365650
		public bool HasBleachStone()
		{
			GameObject gameObject = base.smi.master.waterStorage.FindFirst(new Tag(-839728230));
			return gameObject != null && gameObject.GetComponent<PrimaryElement>().Mass > 0f;
		}

		// Token: 0x06009814 RID: 38932 RVA: 0x0036749C File Offset: 0x0036569C
		public void SapHeatFromWater(float dt)
		{
			float num = base.smi.master.waterCoolingRate * dt / (float)base.smi.master.waterStorage.items.Count;
			foreach (GameObject gameObject in base.smi.master.waterStorage.items)
			{
				GameUtil.DeltaThermalEnergy(gameObject.GetComponent<PrimaryElement>(), -num, base.smi.master.minimumWaterTemperature);
				GameUtil.DeltaThermalEnergy(base.GetComponent<PrimaryElement>(), num, base.GetComponent<PrimaryElement>().Element.highTemp);
			}
		}

		// Token: 0x06009815 RID: 38933 RVA: 0x00367560 File Offset: 0x00365760
		public void ConsumeBleachstone(float dt)
		{
			base.smi.master.waterStorage.ConsumeIgnoringDisease(new Tag(-839728230), base.smi.master.bleachStoneConsumption * dt);
		}

		// Token: 0x040075B0 RID: 30128
		private Operational operational;
	}
}
