using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B59 RID: 2905
[SerializationConfig(MemberSerialization.OptIn)]
public class WaterCooler : StateMachineComponent<WaterCooler.StatesInstance>, IApproachable, IGameObjectEffectDescriptor, FewOptionSideScreen.IFewOptionSideScreen
{
	// Token: 0x17000670 RID: 1648
	// (get) Token: 0x060056C6 RID: 22214 RVA: 0x001F026B File Offset: 0x001EE46B
	// (set) Token: 0x060056C7 RID: 22215 RVA: 0x001F0274 File Offset: 0x001EE474
	public Tag ChosenBeverage
	{
		get
		{
			return this.chosenBeverage;
		}
		set
		{
			if (this.chosenBeverage != value)
			{
				this.chosenBeverage = value;
				base.GetComponent<ManualDeliveryKG>().RequestedItemTag = this.chosenBeverage;
				this.storage.DropAll(false, false, default(Vector3), true, null);
			}
		}
	}

	// Token: 0x060056C8 RID: 22216 RVA: 0x001F02C0 File Offset: 0x001EE4C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<ManualDeliveryKG>().RequestedItemTag = this.chosenBeverage;
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
		this.workables = new SocialGatheringPointWorkable[this.socializeOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.socializeOffsets[i]), Grid.SceneLayer.Move);
			SocialGatheringPointWorkable socialGatheringPointWorkable = ChoreHelpers.CreateLocator("WaterCoolerWorkable", pos).AddOrGet<SocialGatheringPointWorkable>();
			socialGatheringPointWorkable.specificEffect = "Socialized";
			socialGatheringPointWorkable.SetWorkTime(this.workTime);
			this.workables[i] = socialGatheringPointWorkable;
		}
		this.chores = new Chore[this.socializeOffsets.Length];
		Extents extents = new Extents(Grid.PosToCell(this), this.socializeOffsets);
		this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("WaterCooler", this, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnCellChanged));
		base.Subscribe<WaterCooler>(-1697596308, WaterCooler.OnStorageChangeDelegate);
		base.smi.StartSM();
	}

	// Token: 0x060056C9 RID: 22217 RVA: 0x001F0400 File Offset: 0x001EE600
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
		this.CancelDrinkChores();
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

	// Token: 0x060056CA RID: 22218 RVA: 0x001F0464 File Offset: 0x001EE664
	public void UpdateDrinkChores(bool force = true)
	{
		if (!force && !this.choresDirty)
		{
			return;
		}
		float num = this.storage.GetMassAvailable(this.ChosenBeverage);
		int num2 = 0;
		for (int i = 0; i < this.socializeOffsets.Length; i++)
		{
			CellOffset offset = this.socializeOffsets[i];
			Chore chore = this.chores[i];
			if (num2 < this.choreCount && this.IsOffsetValid(offset) && num >= 1f)
			{
				num2++;
				num -= 1f;
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = new WaterCoolerChore(this, this.workables[i], null, null, new Action<Chore>(this.OnChoreEnd));
				}
			}
			else if (chore != null)
			{
				chore.Cancel("invalid");
				this.chores[i] = null;
			}
		}
		this.choresDirty = false;
	}

	// Token: 0x060056CB RID: 22219 RVA: 0x001F0544 File Offset: 0x001EE744
	public void CancelDrinkChores()
	{
		for (int i = 0; i < this.socializeOffsets.Length; i++)
		{
			Chore chore = this.chores[i];
			if (chore != null)
			{
				chore.Cancel("cancelled");
				this.chores[i] = null;
			}
		}
	}

	// Token: 0x060056CC RID: 22220 RVA: 0x001F0584 File Offset: 0x001EE784
	private bool IsOffsetValid(CellOffset offset)
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(this), offset);
		int anchor_cell = Grid.CellBelow(cell);
		return GameNavGrids.FloorValidator.IsWalkableCell(cell, anchor_cell, false);
	}

	// Token: 0x060056CD RID: 22221 RVA: 0x001F05AB File Offset: 0x001EE7AB
	private void OnChoreEnd(Chore chore)
	{
		this.choresDirty = true;
	}

	// Token: 0x060056CE RID: 22222 RVA: 0x001F05B4 File Offset: 0x001EE7B4
	private void OnCellChanged(object data)
	{
		this.choresDirty = true;
	}

	// Token: 0x060056CF RID: 22223 RVA: 0x001F05BD File Offset: 0x001EE7BD
	private void OnStorageChange(object data)
	{
		this.choresDirty = true;
	}

	// Token: 0x060056D0 RID: 22224 RVA: 0x001F05C6 File Offset: 0x001EE7C6
	public CellOffset[] GetOffsets()
	{
		return this.drinkOffsets;
	}

	// Token: 0x060056D1 RID: 22225 RVA: 0x001F05CE File Offset: 0x001EE7CE
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x060056D2 RID: 22226 RVA: 0x001F05D8 File Offset: 0x001EE7D8
	private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
	{
		string arg = tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

	// Token: 0x060056D3 RID: 22227 RVA: 0x001F0640 File Offset: 0x001EE840
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, "Socialized", true);
		foreach (global::Tuple<Tag, string> tuple in WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS)
		{
			this.AddRequirementDesc(list, tuple.first, 1f);
		}
		return list;
	}

	// Token: 0x060056D4 RID: 22228 RVA: 0x001F06C0 File Offset: 0x001EE8C0
	public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions()
	{
		Effect.CreateTooltip(Db.Get().effects.Get("DuplicantGotMilk"), true, "\n    • ", true);
		FewOptionSideScreen.IFewOptionSideScreen.Option[] array = new FewOptionSideScreen.IFewOptionSideScreen.Option[WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string text = Strings.Get("STRINGS.BUILDINGS.PREFABS.WATERCOOLER.OPTION_TOOLTIPS." + WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first.ToString().ToUpper());
			if (!WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].second.IsNullOrWhiteSpace())
			{
				text = text + "\n\n" + Effect.CreateTooltip(Db.Get().effects.Get(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].second), false, "\n    • ", true);
			}
			array[i] = new FewOptionSideScreen.IFewOptionSideScreen.Option(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first, ElementLoader.GetElement(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first).name, Def.GetUISprite(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first, "ui", false), text);
		}
		return array;
	}

	// Token: 0x060056D5 RID: 22229 RVA: 0x001F07D2 File Offset: 0x001EE9D2
	public void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option)
	{
		this.ChosenBeverage = option.tag;
	}

	// Token: 0x060056D6 RID: 22230 RVA: 0x001F07E0 File Offset: 0x001EE9E0
	public Tag GetSelectedOption()
	{
		return this.ChosenBeverage;
	}

	// Token: 0x040038DD RID: 14557
	public const float DRINK_MASS = 1f;

	// Token: 0x040038DE RID: 14558
	public const string SPECIFIC_EFFECT = "Socialized";

	// Token: 0x040038DF RID: 14559
	public CellOffset[] socializeOffsets = new CellOffset[]
	{
		new CellOffset(-1, 0),
		new CellOffset(2, 0),
		new CellOffset(0, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x040038E0 RID: 14560
	public int choreCount = 2;

	// Token: 0x040038E1 RID: 14561
	public float workTime = 5f;

	// Token: 0x040038E2 RID: 14562
	private CellOffset[] drinkOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x040038E3 RID: 14563
	public static Action<GameObject, GameObject> OnDuplicantDrank;

	// Token: 0x040038E4 RID: 14564
	private Chore[] chores;

	// Token: 0x040038E5 RID: 14565
	private HandleVector<int>.Handle validNavCellChangedPartitionerEntry;

	// Token: 0x040038E6 RID: 14566
	private SocialGatheringPointWorkable[] workables;

	// Token: 0x040038E7 RID: 14567
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040038E8 RID: 14568
	public bool choresDirty;

	// Token: 0x040038E9 RID: 14569
	[Serialize]
	private Tag chosenBeverage = GameTags.Water;

	// Token: 0x040038EA RID: 14570
	private static readonly EventSystem.IntraObjectHandler<WaterCooler> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<WaterCooler>(delegate(WaterCooler component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001BAD RID: 7085
	public class States : GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler>
	{
		// Token: 0x0600A421 RID: 42017 RVA: 0x0038B8B8 File Offset: 0x00389AB8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.TagTransition(GameTags.Operational, this.waitingfordelivery, false).PlayAnim("off");
			this.waitingfordelivery.TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.dispensing, (WaterCooler.StatesInstance smi) => smi.HasMinimumMass(), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.dispensing, (WaterCooler.StatesInstance smi) => smi.HasMinimumMass()).PlayAnim("off");
			this.dispensing.Enter("StartMeter", delegate(WaterCooler.StatesInstance smi)
			{
				smi.StartMeter();
			}).Enter("Set Active", delegate(WaterCooler.StatesInstance smi)
			{
				smi.SetOperationalActiveState(true);
			}).Enter("UpdateDrinkChores.force", delegate(WaterCooler.StatesInstance smi)
			{
				smi.master.UpdateDrinkChores(true);
			}).Update("UpdateDrinkChores", delegate(WaterCooler.StatesInstance smi, float dt)
			{
				smi.master.UpdateDrinkChores(true);
			}, UpdateRate.SIM_200ms, false).Exit("CancelDrinkChores", delegate(WaterCooler.StatesInstance smi)
			{
				smi.master.CancelDrinkChores();
			}).Exit("Set Inactive", delegate(WaterCooler.StatesInstance smi)
			{
				smi.SetOperationalActiveState(false);
			}).TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.waitingfordelivery, (WaterCooler.StatesInstance smi) => !smi.HasMinimumMass()).PlayAnim("working");
		}

		// Token: 0x04008065 RID: 32869
		public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State unoperational;

		// Token: 0x04008066 RID: 32870
		public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State waitingfordelivery;

		// Token: 0x04008067 RID: 32871
		public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State dispensing;
	}

	// Token: 0x02001BAE RID: 7086
	public class StatesInstance : GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.GameInstance
	{
		// Token: 0x0600A423 RID: 42019 RVA: 0x0038BABC File Offset: 0x00389CBC
		public StatesInstance(WaterCooler smi) : base(smi)
		{
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_bottle", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_bottle"
			});
			this.storage = base.master.GetComponent<Storage>();
			base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
		}

		// Token: 0x0600A424 RID: 42020 RVA: 0x0038BB24 File Offset: 0x00389D24
		public void Drink(GameObject druplicant, bool triggerOnDrinkCallback = true)
		{
			if (!this.HasMinimumMass())
			{
				return;
			}
			Tag tag = this.storage.items[0].PrefabID();
			float num;
			SimUtil.DiseaseInfo diseaseInfo;
			float num2;
			this.storage.ConsumeAndGetDisease(tag, 1f, out num, out diseaseInfo, out num2);
			GermExposureMonitor.Instance smi = druplicant.GetSMI<GermExposureMonitor.Instance>();
			if (smi != null)
			{
				smi.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, tag, Sickness.InfectionVector.Digestion);
			}
			Effects component = druplicant.GetComponent<Effects>();
			if (tag == SimHashes.Milk.CreateTag())
			{
				component.Add("DuplicantGotMilk", true);
			}
			if (triggerOnDrinkCallback)
			{
				Action<GameObject, GameObject> onDuplicantDrank = WaterCooler.OnDuplicantDrank;
				if (onDuplicantDrank == null)
				{
					return;
				}
				onDuplicantDrank(druplicant, base.gameObject);
			}
		}

		// Token: 0x0600A425 RID: 42021 RVA: 0x0038BBCC File Offset: 0x00389DCC
		private void OnStorageChange(object data)
		{
			float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
			this.meter.SetPositionPercent(positionPercent);
		}

		// Token: 0x0600A426 RID: 42022 RVA: 0x0038BC02 File Offset: 0x00389E02
		public void SetOperationalActiveState(bool isActive)
		{
			this.operational.SetActive(isActive, false);
		}

		// Token: 0x0600A427 RID: 42023 RVA: 0x0038BC14 File Offset: 0x00389E14
		public void StartMeter()
		{
			PrimaryElement primaryElement = this.storage.FindFirstWithMass(base.smi.master.ChosenBeverage, 0f);
			if (primaryElement == null)
			{
				return;
			}
			this.meter.SetSymbolTint(new KAnimHashedString("meter_water"), primaryElement.Element.substance.colour);
			this.OnStorageChange(null);
		}

		// Token: 0x0600A428 RID: 42024 RVA: 0x0038BC78 File Offset: 0x00389E78
		public bool HasMinimumMass()
		{
			return this.storage.GetMassAvailable(ElementLoader.GetElement(base.smi.master.ChosenBeverage).id) >= 1f;
		}

		// Token: 0x04008068 RID: 32872
		[MyCmpGet]
		private Operational operational;

		// Token: 0x04008069 RID: 32873
		private Storage storage;

		// Token: 0x0400806A RID: 32874
		private MeterController meter;
	}
}
