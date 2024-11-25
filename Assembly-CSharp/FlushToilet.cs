using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020006D3 RID: 1747
public class FlushToilet : StateMachineComponent<FlushToilet.SMInstance>, IUsable, IGameObjectEffectDescriptor, IBasicBuilding
{
	// Token: 0x06002C3F RID: 11327 RVA: 0x000F8598 File Offset: 0x000F6798
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		ConduitFlow liquidConduitFlow = Game.Instance.liquidConduitFlow;
		liquidConduitFlow.onConduitsRebuilt += this.OnConduitsRebuilt;
		liquidConduitFlow.AddConduitUpdater(new Action<float>(this.OnConduitUpdate), ConduitFlowPriority.Default);
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		this.fillMeter = new MeterController(component2, "meter_target", "meter", this.meterOffset, Grid.SceneLayer.NoLayer, new Vector3(0.4f, 3.2f, 0.1f), Array.Empty<string>());
		this.contaminationMeter = new MeterController(component2, "meter_target", "meter_dirty", this.meterOffset, Grid.SceneLayer.NoLayer, new Vector3(0.4f, 3.2f, 0.1f), Array.Empty<string>());
		Components.Toilets.Add(this);
		Components.BasicBuildings.Add(this);
		base.smi.StartSM();
		base.smi.ShowFillMeter();
	}

	// Token: 0x06002C40 RID: 11328 RVA: 0x000F8699 File Offset: 0x000F6899
	protected override void OnCleanUp()
	{
		Game.Instance.liquidConduitFlow.onConduitsRebuilt -= this.OnConduitsRebuilt;
		Components.BasicBuildings.Remove(this);
		Components.Toilets.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06002C41 RID: 11329 RVA: 0x000F86D2 File Offset: 0x000F68D2
	private void OnConduitsRebuilt()
	{
		base.Trigger(-2094018600, null);
	}

	// Token: 0x06002C42 RID: 11330 RVA: 0x000F86E0 File Offset: 0x000F68E0
	public bool IsUsable()
	{
		return base.smi.HasTag(GameTags.Usable);
	}

	// Token: 0x06002C43 RID: 11331 RVA: 0x000F86F4 File Offset: 0x000F68F4
	private void Flush(WorkerBase worker)
	{
		ToiletWorkableUse component = base.GetComponent<ToiletWorkableUse>();
		ListPool<GameObject, Storage>.PooledList pooledList = ListPool<GameObject, Storage>.Allocate();
		this.storage.Find(FlushToilet.WaterTag, pooledList);
		float num = 0f;
		float num2 = this.massConsumedPerUse;
		foreach (GameObject gameObject in pooledList)
		{
			PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
			float num3 = Mathf.Min(component2.Mass, num2);
			component2.Mass -= num3;
			num2 -= num3;
			num += num3 * component2.Temperature;
		}
		pooledList.Recycle();
		float lastAmountOfWasteMassRemovedFromDupe = component.lastAmountOfWasteMassRemovedFromDupe;
		num += lastAmountOfWasteMassRemovedFromDupe * this.newPeeTemperature;
		float num4 = this.massConsumedPerUse + lastAmountOfWasteMassRemovedFromDupe;
		float temperature = num / num4;
		byte index = Db.Get().Diseases.GetIndex(this.diseaseId);
		this.storage.AddLiquid(component.lastElementRemovedFromDupe, num4, temperature, index, this.diseasePerFlush, false, true);
		if (worker != null)
		{
			worker.GetComponent<PrimaryElement>().AddDisease(index, this.diseaseOnDupePerFlush, "FlushToilet.Flush");
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(DUPLICANTS.DISEASES.ADDED_POPFX, Db.Get().Diseases[(int)index].Name, this.diseasePerFlush + this.diseaseOnDupePerFlush), base.transform, Vector3.up, 1.5f, false, false);
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_LotsOfGerms, true);
			return;
		}
		DebugUtil.LogWarningArgs(new object[]
		{
			"Tried to add disease on toilet use but worker was null"
		});
	}

	// Token: 0x06002C44 RID: 11332 RVA: 0x000F88AC File Offset: 0x000F6AAC
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(SimHashes.Water).tag.ProperName();
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement, false));
		return list;
	}

	// Token: 0x06002C45 RID: 11333 RVA: 0x000F8928 File Offset: 0x000F6B28
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag.ProperName();
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_TOILET, arg, GameUtil.GetFormattedMass(this.massEmittedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(this.newPeeTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_TOILET, arg, GameUtil.GetFormattedMass(this.massEmittedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(this.newPeeTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false));
		Disease disease = Db.Get().Diseases.Get(this.diseaseId);
		int units = this.diseasePerFlush + this.diseaseOnDupePerFlush;
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.DISEASEEMITTEDPERUSE, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DISEASEEMITTEDPERUSE, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.DiseaseSource, false));
		return list;
	}

	// Token: 0x06002C46 RID: 11334 RVA: 0x000F8A29 File Offset: 0x000F6C29
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.AddRange(this.RequirementDescriptors());
		list.AddRange(this.EffectDescriptors());
		return list;
	}

	// Token: 0x06002C47 RID: 11335 RVA: 0x000F8A48 File Offset: 0x000F6C48
	private void OnConduitUpdate(float dt)
	{
		if (this.GetSMI() == null)
		{
			return;
		}
		ConduitFlow liquidConduitFlow = Game.Instance.liquidConduitFlow;
		bool value = base.smi.master.requireOutput && liquidConduitFlow.GetContents(this.outputCell).mass > 0f && base.smi.HasContaminatedMass();
		base.smi.sm.outputBlocked.Set(value, base.smi, false);
	}

	// Token: 0x04001980 RID: 6528
	private MeterController fillMeter;

	// Token: 0x04001981 RID: 6529
	private MeterController contaminationMeter;

	// Token: 0x04001982 RID: 6530
	public Meter.Offset meterOffset = Meter.Offset.Behind;

	// Token: 0x04001983 RID: 6531
	[SerializeField]
	public float massConsumedPerUse = 5f;

	// Token: 0x04001984 RID: 6532
	[SerializeField]
	public float massEmittedPerUse = 5f;

	// Token: 0x04001985 RID: 6533
	[SerializeField]
	public float newPeeTemperature;

	// Token: 0x04001986 RID: 6534
	[SerializeField]
	public string diseaseId;

	// Token: 0x04001987 RID: 6535
	[SerializeField]
	public int diseasePerFlush;

	// Token: 0x04001988 RID: 6536
	[SerializeField]
	public int diseaseOnDupePerFlush;

	// Token: 0x04001989 RID: 6537
	[SerializeField]
	public bool requireOutput = true;

	// Token: 0x0400198A RID: 6538
	[MyCmpGet]
	private ConduitConsumer conduitConsumer;

	// Token: 0x0400198B RID: 6539
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400198C RID: 6540
	public static readonly Tag WaterTag = GameTagExtensions.Create(SimHashes.Water);

	// Token: 0x0400198D RID: 6541
	private int inputCell;

	// Token: 0x0400198E RID: 6542
	private int outputCell;

	// Token: 0x020014DB RID: 5339
	public class SMInstance : GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.GameInstance
	{
		// Token: 0x06008C46 RID: 35910 RVA: 0x00339B95 File Offset: 0x00337D95
		public SMInstance(FlushToilet master) : base(master)
		{
			this.activeUseChores = new List<Chore>();
			this.UpdateFullnessState();
			this.UpdateDirtyState();
		}

		// Token: 0x06008C47 RID: 35911 RVA: 0x00339BB8 File Offset: 0x00337DB8
		public bool HasValidConnections()
		{
			return Game.Instance.liquidConduitFlow.HasConduit(base.master.inputCell) && (!base.master.requireOutput || Game.Instance.liquidConduitFlow.HasConduit(base.master.outputCell));
		}

		// Token: 0x06008C48 RID: 35912 RVA: 0x00339C0C File Offset: 0x00337E0C
		public bool UpdateFullnessState()
		{
			float num = 0f;
			ListPool<GameObject, FlushToilet>.PooledList pooledList = ListPool<GameObject, FlushToilet>.Allocate();
			base.master.storage.Find(FlushToilet.WaterTag, pooledList);
			foreach (GameObject gameObject in pooledList)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				num += component.Mass;
			}
			pooledList.Recycle();
			bool flag = num >= base.master.massConsumedPerUse;
			base.master.conduitConsumer.enabled = !flag;
			float positionPercent = Mathf.Clamp01(num / base.master.massConsumedPerUse);
			base.master.fillMeter.SetPositionPercent(positionPercent);
			return flag;
		}

		// Token: 0x06008C49 RID: 35913 RVA: 0x00339CD8 File Offset: 0x00337ED8
		public void UpdateDirtyState()
		{
			float percentComplete = base.GetComponent<ToiletWorkableUse>().GetPercentComplete();
			base.master.contaminationMeter.SetPositionPercent(percentComplete);
		}

		// Token: 0x06008C4A RID: 35914 RVA: 0x00339D04 File Offset: 0x00337F04
		public void Flush()
		{
			base.master.fillMeter.SetPositionPercent(0f);
			base.master.contaminationMeter.SetPositionPercent(1f);
			base.smi.ShowFillMeter();
			WorkerBase worker = base.master.GetComponent<ToiletWorkableUse>().worker;
			base.master.Flush(worker);
		}

		// Token: 0x06008C4B RID: 35915 RVA: 0x00339D63 File Offset: 0x00337F63
		public void ShowFillMeter()
		{
			base.master.fillMeter.gameObject.SetActive(true);
			base.master.contaminationMeter.gameObject.SetActive(false);
		}

		// Token: 0x06008C4C RID: 35916 RVA: 0x00339D94 File Offset: 0x00337F94
		public bool HasContaminatedMass()
		{
			foreach (GameObject gameObject in base.GetComponent<Storage>().items)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && (component.ElementID == SimHashes.DirtyWater || component.ElementID == GunkMonitor.GunkElement) && component.Mass > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008C4D RID: 35917 RVA: 0x00339E24 File Offset: 0x00338024
		public void ShowContaminatedMeter()
		{
			base.master.fillMeter.gameObject.SetActive(false);
			base.master.contaminationMeter.gameObject.SetActive(true);
		}

		// Token: 0x04006B1D RID: 27421
		public List<Chore> activeUseChores;
	}

	// Token: 0x020014DC RID: 5340
	public class States : GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet>
	{
		// Token: 0x06008C4E RID: 35918 RVA: 0x00339E54 File Offset: 0x00338054
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disconnected;
			this.disconnected.PlayAnim("off").EventTransition(GameHashes.ConduitConnectionChanged, this.backedup, (FlushToilet.SMInstance smi) => smi.HasValidConnections()).Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.backedup.PlayAnim("off").ToggleStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, null).EventTransition(GameHashes.ConduitConnectionChanged, this.disconnected, (FlushToilet.SMInstance smi) => !smi.HasValidConnections()).ParamTransition<bool>(this.outputBlocked, this.fillingInactive, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsFalse).Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.filling.PlayAnim("off").Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
			}).EventTransition(GameHashes.ConduitConnectionChanged, this.disconnected, (FlushToilet.SMInstance smi) => !smi.HasValidConnections()).ParamTransition<bool>(this.outputBlocked, this.backedup, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue).EventTransition(GameHashes.OnStorageChange, this.ready, (FlushToilet.SMInstance smi) => smi.UpdateFullnessState()).EventTransition(GameHashes.OperationalChanged, this.fillingInactive, (FlushToilet.SMInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.fillingInactive.PlayAnim("off").Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			}).EventTransition(GameHashes.OperationalChanged, this.filling, (FlushToilet.SMInstance smi) => smi.GetComponent<Operational>().IsOperational).ParamTransition<bool>(this.outputBlocked, this.backedup, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue);
			this.ready.DefaultState(this.ready.idle).ToggleTag(GameTags.Usable).Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.master.fillMeter.SetPositionPercent(1f);
				smi.master.contaminationMeter.SetPositionPercent(0f);
			}).PlayAnim("off").EventTransition(GameHashes.ConduitConnectionChanged, this.disconnected, (FlushToilet.SMInstance smi) => !smi.HasValidConnections()).ParamTransition<bool>(this.outputBlocked, this.backedup, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue).ToggleChore(new Func<FlushToilet.SMInstance, Chore>(this.CreateUrgentUseChore), this.flushing).ToggleChore(new Func<FlushToilet.SMInstance, Chore>(this.CreateBreakUseChore), this.flushing);
			this.ready.idle.Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.FlushToilet, null).WorkableStartTransition((FlushToilet.SMInstance smi) => smi.master.GetComponent<ToiletWorkableUse>(), this.ready.inuse);
			this.ready.inuse.Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.ShowContaminatedMeter();
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.FlushToiletInUse, null).Update(delegate(FlushToilet.SMInstance smi, float dt)
			{
				smi.UpdateDirtyState();
			}, UpdateRate.SIM_200ms, false).WorkableCompleteTransition((FlushToilet.SMInstance smi) => smi.master.GetComponent<ToiletWorkableUse>(), this.flushing).WorkableStopTransition((FlushToilet.SMInstance smi) => smi.master.GetComponent<ToiletWorkableUse>(), this.flushed);
			this.flushing.Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.Flush();
			}).PlayAnim("flush").OnAnimQueueComplete(this.flushed);
			this.flushed.EventTransition(GameHashes.OnStorageChange, this.fillingInactive, (FlushToilet.SMInstance smi) => !smi.HasContaminatedMass()).ParamTransition<bool>(this.outputBlocked, this.backedup, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue);
		}

		// Token: 0x06008C4F RID: 35919 RVA: 0x0033A32F File Offset: 0x0033852F
		private Chore CreateUrgentUseChore(FlushToilet.SMInstance smi)
		{
			Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.Pee);
			chore.AddPrecondition(ChorePreconditions.instance.IsBladderFull, null);
			chore.AddPrecondition(ChorePreconditions.instance.NotCurrentlyPeeing, null);
			return chore;
		}

		// Token: 0x06008C50 RID: 35920 RVA: 0x0033A369 File Offset: 0x00338569
		private Chore CreateBreakUseChore(FlushToilet.SMInstance smi)
		{
			Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.BreakPee);
			chore.AddPrecondition(ChorePreconditions.instance.IsBladderNotFull, null);
			return chore;
		}

		// Token: 0x06008C51 RID: 35921 RVA: 0x0033A394 File Offset: 0x00338594
		private Chore CreateUseChore(FlushToilet.SMInstance smi, ChoreType choreType)
		{
			WorkChore<ToiletWorkableUse> workChore = new WorkChore<ToiletWorkableUse>(choreType, smi.master, null, true, null, null, null, false, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
			smi.activeUseChores.Add(workChore);
			WorkChore<ToiletWorkableUse> workChore2 = workChore;
			workChore2.onExit = (Action<Chore>)Delegate.Combine(workChore2.onExit, new Action<Chore>(delegate(Chore exiting_chore)
			{
				smi.activeUseChores.Remove(exiting_chore);
			}));
			workChore.AddPrecondition(ChorePreconditions.instance.IsPreferredAssignableOrUrgentBladder, smi.master.GetComponent<Assignable>());
			workChore.AddPrecondition(ChorePreconditions.instance.IsExclusivelyAvailableWithOtherChores, smi.activeUseChores);
			return workChore;
		}

		// Token: 0x04006B1E RID: 27422
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State disconnected;

		// Token: 0x04006B1F RID: 27423
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State backedup;

		// Token: 0x04006B20 RID: 27424
		public FlushToilet.States.ReadyStates ready;

		// Token: 0x04006B21 RID: 27425
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State fillingInactive;

		// Token: 0x04006B22 RID: 27426
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State filling;

		// Token: 0x04006B23 RID: 27427
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State flushing;

		// Token: 0x04006B24 RID: 27428
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State flushed;

		// Token: 0x04006B25 RID: 27429
		public StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.BoolParameter outputBlocked;

		// Token: 0x020024D6 RID: 9430
		public class ReadyStates : GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State
		{
			// Token: 0x0400A392 RID: 41874
			public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State idle;

			// Token: 0x0400A393 RID: 41875
			public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State inuse;
		}
	}
}
