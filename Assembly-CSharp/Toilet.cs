using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000787 RID: 1927
public class Toilet : StateMachineComponent<Toilet.StatesInstance>, ISaveLoadable, IUsable, IGameObjectEffectDescriptor, IBasicBuilding
{
	// Token: 0x17000398 RID: 920
	// (get) Token: 0x06003479 RID: 13433 RVA: 0x0011E18B File Offset: 0x0011C38B
	// (set) Token: 0x0600347A RID: 13434 RVA: 0x0011E193 File Offset: 0x0011C393
	public int FlushesUsed
	{
		get
		{
			return this._flushesUsed;
		}
		set
		{
			this._flushesUsed = value;
			base.smi.sm.flushes.Set(value, base.smi, false);
		}
	}

	// Token: 0x0600347B RID: 13435 RVA: 0x0011E1BC File Offset: 0x0011C3BC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Toilets.Add(this);
		Components.BasicBuildings.Add(this);
		base.smi.StartSM();
		base.GetComponent<ToiletWorkableUse>().trackUses = true;
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_arrow",
			"meter_scale"
		});
		this.meter.SetPositionPercent((float)this.FlushesUsed / (float)this.maxFlushes);
		this.FlushesUsed = this._flushesUsed;
		base.Subscribe<Toilet>(493375141, Toilet.OnRefreshUserMenuDelegate);
	}

	// Token: 0x0600347C RID: 13436 RVA: 0x0011E26F File Offset: 0x0011C46F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.BasicBuildings.Remove(this);
		Components.Toilets.Remove(this);
	}

	// Token: 0x0600347D RID: 13437 RVA: 0x0011E28D File Offset: 0x0011C48D
	public bool IsUsable()
	{
		return base.smi.HasTag(GameTags.Usable);
	}

	// Token: 0x0600347E RID: 13438 RVA: 0x0011E29F File Offset: 0x0011C49F
	public void Flush(WorkerBase worker)
	{
		this.FlushMultiple(worker, 1);
	}

	// Token: 0x0600347F RID: 13439 RVA: 0x0011E2AC File Offset: 0x0011C4AC
	public void FlushMultiple(WorkerBase worker, int flushCount)
	{
		int b = this.maxFlushes - this.FlushesUsed;
		int num = Mathf.Min(flushCount, b);
		this.FlushesUsed += num;
		this.meter.SetPositionPercent((float)this.FlushesUsed / (float)this.maxFlushes);
		float num2 = 0f;
		Tag tag = ElementLoader.FindElementByHash(SimHashes.Dirt).tag;
		float num3;
		SimUtil.DiseaseInfo diseaseInfo;
		this.storage.ConsumeAndGetDisease(tag, base.smi.DirtUsedPerFlush() * (float)num, out num3, out diseaseInfo, out num2);
		byte index = Db.Get().Diseases.GetIndex(this.diseaseId);
		int num4 = this.diseasePerFlush * num;
		float mass = base.smi.MassPerFlush() + num3;
		GameObject gameObject = ElementLoader.FindElementByHash(this.solidWastePerUse.elementID).substance.SpawnResource(base.transform.GetPosition(), mass, this.solidWasteTemperature, index, num4, true, false, false);
		gameObject.GetComponent<PrimaryElement>().AddDisease(diseaseInfo.idx, diseaseInfo.count, "Toilet.Flush");
		num4 += diseaseInfo.count;
		this.storage.Store(gameObject, false, false, true, false);
		int num5 = this.diseaseOnDupePerFlush * num;
		worker.GetComponent<PrimaryElement>().AddDisease(index, num5, "Toilet.Flush");
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(DUPLICANTS.DISEASES.ADDED_POPFX, Db.Get().Diseases[(int)index].Name, num4 + num5), base.transform, Vector3.up, 1.5f, false, false);
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_LotsOfGerms, true);
	}

	// Token: 0x06003480 RID: 13440 RVA: 0x0011E458 File Offset: 0x0011C658
	private void OnRefreshUserMenu(object data)
	{
		if (base.smi.GetCurrentState() == base.smi.sm.full || !base.smi.IsSoiled || base.smi.cleanChore != null)
		{
			return;
		}
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("status_item_toilet_needs_emptying", UI.USERMENUACTIONS.CLEANTOILET.NAME, delegate()
		{
			base.smi.GoTo(base.smi.sm.earlyclean);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CLEANTOILET.TOOLTIP, true), 1f);
	}

	// Token: 0x06003481 RID: 13441 RVA: 0x0011E4EA File Offset: 0x0011C6EA
	private void SpawnMonster()
	{
		GameUtil.KInstantiate(Assets.GetPrefab(new Tag("Glom")), base.smi.transform.GetPosition(), Grid.SceneLayer.Creatures, null, 0).SetActive(true);
	}

	// Token: 0x06003482 RID: 13442 RVA: 0x0011E51C File Offset: 0x0011C71C
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = base.GetComponent<ManualDeliveryKG>().RequestedItemTag.ProperName();
		float mass = base.smi.DirtUsedPerFlush();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		list.Add(item);
		return list;
	}

	// Token: 0x06003483 RID: 13443 RVA: 0x0011E5A0 File Offset: 0x0011C7A0
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(this.solidWastePerUse.elementID).tag.ProperName();
		float mass = base.smi.MassPerFlush() + base.smi.DirtUsedPerFlush();
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_TOILET, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(this.solidWasteTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_TOILET, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(this.solidWasteTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false));
		Disease disease = Db.Get().Diseases.Get(this.diseaseId);
		int units = this.diseasePerFlush + this.diseaseOnDupePerFlush;
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.DISEASEEMITTEDPERUSE, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DISEASEEMITTEDPERUSE, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.DiseaseSource, false));
		return list;
	}

	// Token: 0x06003484 RID: 13444 RVA: 0x0011E6B5 File Offset: 0x0011C8B5
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.AddRange(this.RequirementDescriptors());
		list.AddRange(this.EffectDescriptors());
		return list;
	}

	// Token: 0x04001EFD RID: 7933
	[SerializeField]
	public Toilet.SpawnInfo solidWastePerUse;

	// Token: 0x04001EFE RID: 7934
	[SerializeField]
	public float solidWasteTemperature;

	// Token: 0x04001EFF RID: 7935
	[SerializeField]
	public Toilet.SpawnInfo gasWasteWhenFull;

	// Token: 0x04001F00 RID: 7936
	[SerializeField]
	public int maxFlushes = 15;

	// Token: 0x04001F01 RID: 7937
	[SerializeField]
	public string diseaseId;

	// Token: 0x04001F02 RID: 7938
	[SerializeField]
	public int diseasePerFlush;

	// Token: 0x04001F03 RID: 7939
	[SerializeField]
	public int diseaseOnDupePerFlush;

	// Token: 0x04001F04 RID: 7940
	[SerializeField]
	public float dirtUsedPerFlush = 13f;

	// Token: 0x04001F05 RID: 7941
	[Serialize]
	public int _flushesUsed;

	// Token: 0x04001F06 RID: 7942
	private MeterController meter;

	// Token: 0x04001F07 RID: 7943
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04001F08 RID: 7944
	[MyCmpReq]
	private ManualDeliveryKG manualdeliverykg;

	// Token: 0x04001F09 RID: 7945
	private static readonly EventSystem.IntraObjectHandler<Toilet> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Toilet>(delegate(Toilet component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x02001630 RID: 5680
	[Serializable]
	public struct SpawnInfo
	{
		// Token: 0x06009157 RID: 37207 RVA: 0x0034FC6D File Offset: 0x0034DE6D
		public SpawnInfo(SimHashes element_id, float mass, float interval)
		{
			this.elementID = element_id;
			this.mass = mass;
			this.interval = interval;
		}

		// Token: 0x04006EF0 RID: 28400
		[HashedEnum]
		public SimHashes elementID;

		// Token: 0x04006EF1 RID: 28401
		public float mass;

		// Token: 0x04006EF2 RID: 28402
		public float interval;
	}

	// Token: 0x02001631 RID: 5681
	public class StatesInstance : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.GameInstance
	{
		// Token: 0x06009158 RID: 37208 RVA: 0x0034FC84 File Offset: 0x0034DE84
		public StatesInstance(Toilet master) : base(master)
		{
			this.activeUseChores = new List<Chore>();
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06009159 RID: 37209 RVA: 0x0034FCA3 File Offset: 0x0034DEA3
		public bool IsSoiled
		{
			get
			{
				return base.master.FlushesUsed > 0;
			}
		}

		// Token: 0x0600915A RID: 37210 RVA: 0x0034FCB3 File Offset: 0x0034DEB3
		public int GetFlushesRemaining()
		{
			return base.master.maxFlushes - base.master.FlushesUsed;
		}

		// Token: 0x0600915B RID: 37211 RVA: 0x0034FCCC File Offset: 0x0034DECC
		public bool RequiresDirtDelivery()
		{
			return base.master.storage.IsEmpty() || !base.master.storage.Has(GameTags.Dirt) || (base.master.storage.GetAmountAvailable(GameTags.Dirt) < base.master.manualdeliverykg.capacity && !this.IsSoiled);
		}

		// Token: 0x0600915C RID: 37212 RVA: 0x0034FD3D File Offset: 0x0034DF3D
		public float MassPerFlush()
		{
			return base.master.solidWastePerUse.mass;
		}

		// Token: 0x0600915D RID: 37213 RVA: 0x0034FD4F File Offset: 0x0034DF4F
		public float DirtUsedPerFlush()
		{
			return base.master.dirtUsedPerFlush;
		}

		// Token: 0x0600915E RID: 37214 RVA: 0x0034FD5C File Offset: 0x0034DF5C
		public bool IsToxicSandRemoved()
		{
			Tag tag = GameTagExtensions.Create(base.master.solidWastePerUse.elementID);
			return base.master.storage.FindFirst(tag) == null;
		}

		// Token: 0x0600915F RID: 37215 RVA: 0x0034FD98 File Offset: 0x0034DF98
		public void CreateCleanChore()
		{
			if (this.cleanChore != null)
			{
				this.cleanChore.Cancel("dupe");
			}
			ToiletWorkableClean component = base.master.GetComponent<ToiletWorkableClean>();
			this.cleanChore = new WorkChore<ToiletWorkableClean>(Db.Get().ChoreTypes.CleanToilet, component, null, true, new Action<Chore>(this.OnCleanComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
		}

		// Token: 0x06009160 RID: 37216 RVA: 0x0034FE00 File Offset: 0x0034E000
		public void CancelCleanChore()
		{
			if (this.cleanChore != null)
			{
				this.cleanChore.Cancel("Cancelled");
				this.cleanChore = null;
			}
		}

		// Token: 0x06009161 RID: 37217 RVA: 0x0034FE24 File Offset: 0x0034E024
		private void DropFromStorage(Tag tag)
		{
			ListPool<GameObject, Toilet>.PooledList pooledList = ListPool<GameObject, Toilet>.Allocate();
			base.master.storage.Find(tag, pooledList);
			foreach (GameObject go in pooledList)
			{
				base.master.storage.Drop(go, true);
			}
			pooledList.Recycle();
		}

		// Token: 0x06009162 RID: 37218 RVA: 0x0034FEA0 File Offset: 0x0034E0A0
		private void OnCleanComplete(Chore chore)
		{
			this.cleanChore = null;
			Tag tag = GameTagExtensions.Create(base.master.solidWastePerUse.elementID);
			Tag tag2 = ElementLoader.FindElementByHash(SimHashes.Dirt).tag;
			this.DropFromStorage(tag);
			this.DropFromStorage(tag2);
			base.master.meter.SetPositionPercent((float)base.master.FlushesUsed / (float)base.master.maxFlushes);
		}

		// Token: 0x06009163 RID: 37219 RVA: 0x0034FF14 File Offset: 0x0034E114
		public void Flush()
		{
			WorkerBase worker = base.master.GetComponent<ToiletWorkableUse>().worker;
			base.master.Flush(worker);
		}

		// Token: 0x06009164 RID: 37220 RVA: 0x0034FF40 File Offset: 0x0034E140
		public void FlushAll()
		{
			WorkerBase worker = base.master.GetComponent<ToiletWorkableUse>().worker;
			base.master.FlushMultiple(worker, base.master.maxFlushes - base.master.FlushesUsed);
		}

		// Token: 0x04006EF3 RID: 28403
		public Chore cleanChore;

		// Token: 0x04006EF4 RID: 28404
		public List<Chore> activeUseChores;

		// Token: 0x04006EF5 RID: 28405
		public float monsterSpawnTime = 1200f;
	}

	// Token: 0x02001632 RID: 5682
	public class States : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet>
	{
		// Token: 0x06009165 RID: 37221 RVA: 0x0034FF84 File Offset: 0x0034E184
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.needsdirt;
			this.root.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.needsdirt, (Toilet.StatesInstance smi) => smi.RequiresDirtDelivery()).EventTransition(GameHashes.OperationalChanged, this.notoperational, (Toilet.StatesInstance smi) => !smi.Get<Operational>().IsOperational);
			this.needsdirt.Enter(delegate(Toilet.StatesInstance smi)
			{
				if (smi.RequiresDirtDelivery())
				{
					smi.master.manualdeliverykg.RequestDelivery();
				}
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null).EventTransition(GameHashes.OnStorageChange, this.ready, (Toilet.StatesInstance smi) => !smi.RequiresDirtDelivery());
			this.ready.ParamTransition<int>(this.flushes, this.full, (Toilet.StatesInstance smi, int p) => smi.GetFlushesRemaining() <= 0).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Toilet, null).ToggleRecurringChore(new Func<Toilet.StatesInstance, Chore>(this.CreateUrgentUseChore), null).ToggleRecurringChore(new Func<Toilet.StatesInstance, Chore>(this.CreateBreakUseChore), null).ToggleTag(GameTags.Usable).EventHandler(GameHashes.Flush, delegate(Toilet.StatesInstance smi, object data)
			{
				smi.Flush();
			}).EventHandler(GameHashes.FlushAll, delegate(Toilet.StatesInstance smi, object data)
			{
				smi.FlushAll();
			});
			this.earlyclean.PlayAnims((Toilet.StatesInstance smi) => Toilet.States.FULL_ANIMS, KAnim.PlayMode.Once).OnAnimQueueComplete(this.earlyWaitingForClean);
			this.earlyWaitingForClean.Enter(delegate(Toilet.StatesInstance smi)
			{
				smi.CreateCleanChore();
			}).Exit(delegate(Toilet.StatesInstance smi)
			{
				smi.CancelCleanChore();
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.ToiletNeedsEmptying, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null).EventTransition(GameHashes.OnStorageChange, this.empty, (Toilet.StatesInstance smi) => smi.IsToxicSandRemoved());
			this.full.PlayAnims((Toilet.StatesInstance smi) => Toilet.States.FULL_ANIMS, KAnim.PlayMode.Once).OnAnimQueueComplete(this.fullWaitingForClean);
			this.fullWaitingForClean.Enter(delegate(Toilet.StatesInstance smi)
			{
				smi.CreateCleanChore();
			}).Exit(delegate(Toilet.StatesInstance smi)
			{
				smi.CancelCleanChore();
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.ToiletNeedsEmptying, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null).EventTransition(GameHashes.OnStorageChange, this.empty, (Toilet.StatesInstance smi) => smi.IsToxicSandRemoved()).Enter(delegate(Toilet.StatesInstance smi)
			{
				smi.Schedule(smi.monsterSpawnTime, delegate
				{
					smi.master.SpawnMonster();
				}, null);
			});
			this.empty.PlayAnim("off").Enter("ClearFlushes", delegate(Toilet.StatesInstance smi)
			{
				smi.master.FlushesUsed = 0;
			}).GoTo(this.needsdirt);
			this.notoperational.EventTransition(GameHashes.OperationalChanged, this.needsdirt, (Toilet.StatesInstance smi) => smi.Get<Operational>().IsOperational).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null);
		}

		// Token: 0x06009166 RID: 37222 RVA: 0x003503B5 File Offset: 0x0034E5B5
		private Chore CreateUrgentUseChore(Toilet.StatesInstance smi)
		{
			Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.Pee);
			chore.AddPrecondition(ChorePreconditions.instance.IsBladderFull, null);
			chore.AddPrecondition(ChorePreconditions.instance.NotCurrentlyPeeing, null);
			return chore;
		}

		// Token: 0x06009167 RID: 37223 RVA: 0x003503F0 File Offset: 0x0034E5F0
		private Chore CreateBreakUseChore(Toilet.StatesInstance smi)
		{
			Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.BreakPee);
			chore.AddPrecondition(ChorePreconditions.instance.IsBladderNotFull, null);
			chore.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Hygiene);
			return chore;
		}

		// Token: 0x06009168 RID: 37224 RVA: 0x00350444 File Offset: 0x0034E644
		private Chore CreateUseChore(Toilet.StatesInstance smi, ChoreType choreType)
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

		// Token: 0x04006EF6 RID: 28406
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State needsdirt;

		// Token: 0x04006EF7 RID: 28407
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State empty;

		// Token: 0x04006EF8 RID: 28408
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State notoperational;

		// Token: 0x04006EF9 RID: 28409
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State ready;

		// Token: 0x04006EFA RID: 28410
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State earlyclean;

		// Token: 0x04006EFB RID: 28411
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State earlyWaitingForClean;

		// Token: 0x04006EFC RID: 28412
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State full;

		// Token: 0x04006EFD RID: 28413
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State fullWaitingForClean;

		// Token: 0x04006EFE RID: 28414
		private static readonly HashedString[] FULL_ANIMS = new HashedString[]
		{
			"full_pre",
			"full"
		};

		// Token: 0x04006EFF RID: 28415
		public StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.IntParameter flushes = new StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.IntParameter(0);

		// Token: 0x0200254A RID: 9546
		public class ReadyStates : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State
		{
			// Token: 0x0400A626 RID: 42534
			public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State idle;

			// Token: 0x0400A627 RID: 42535
			public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State inuse;

			// Token: 0x0400A628 RID: 42536
			public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State flush;
		}
	}
}
