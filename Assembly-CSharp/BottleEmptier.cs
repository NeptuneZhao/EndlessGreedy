using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000668 RID: 1640
[SerializationConfig(MemberSerialization.OptIn)]
public class BottleEmptier : StateMachineComponent<BottleEmptier.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06002873 RID: 10355 RVA: 0x000E50DB File Offset: 0x000E32DB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.DefineManualPumpingAffectedBuildings();
		base.Subscribe<BottleEmptier>(493375141, BottleEmptier.OnRefreshUserMenuDelegate);
		base.Subscribe<BottleEmptier>(-905833192, BottleEmptier.OnCopySettingsDelegate);
	}

	// Token: 0x06002874 RID: 10356 RVA: 0x000E5116 File Offset: 0x000E3316
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x06002875 RID: 10357 RVA: 0x000E511C File Offset: 0x000E331C
	private void DefineManualPumpingAffectedBuildings()
	{
		if (BottleEmptier.manualPumpingAffectedBuildings.ContainsKey(this.isGasEmptier))
		{
			return;
		}
		List<string> list = new List<string>();
		Tag tag = this.isGasEmptier ? GameTags.GasSource : GameTags.LiquidSource;
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			if (buildingDef.BuildingComplete.HasTag(tag))
			{
				list.Add(buildingDef.Name);
			}
		}
		BottleEmptier.manualPumpingAffectedBuildings.Add(this.isGasEmptier, list.ToArray());
	}

	// Token: 0x06002876 RID: 10358 RVA: 0x000E51C8 File Offset: 0x000E33C8
	private void OnChangeAllowManualPumpingStationFetching()
	{
		this.allowManualPumpingStationFetching = !this.allowManualPumpingStationFetching;
		base.smi.RefreshChore();
	}

	// Token: 0x06002877 RID: 10359 RVA: 0x000E51E4 File Offset: 0x000E33E4
	private void OnRefreshUserMenu(object data)
	{
		string text = this.isGasEmptier ? UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED_GAS.TOOLTIP : UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED.TOOLTIP;
		string text2 = this.isGasEmptier ? UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED_GAS.TOOLTIP : UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED.TOOLTIP;
		if (BottleEmptier.manualPumpingAffectedBuildings.ContainsKey(this.isGasEmptier))
		{
			foreach (string arg in BottleEmptier.manualPumpingAffectedBuildings[this.isGasEmptier])
			{
				string str = string.Format(UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED.ITEM, arg);
				text += str;
				text2 += str;
			}
		}
		if (this.isGasEmptier)
		{
			KIconButtonMenu.ButtonInfo button = this.allowManualPumpingStationFetching ? new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED_GAS.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, text2, true) : new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED_GAS.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, text, true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 0.4f);
			return;
		}
		KIconButtonMenu.ButtonInfo button2 = this.allowManualPumpingStationFetching ? new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, text2, true) : new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, text, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button2, 0.4f);
	}

	// Token: 0x06002878 RID: 10360 RVA: 0x000E5384 File Offset: 0x000E3584
	private void OnCopySettings(object data)
	{
		BottleEmptier component = ((GameObject)data).GetComponent<BottleEmptier>();
		this.allowManualPumpingStationFetching = component.allowManualPumpingStationFetching;
		base.smi.RefreshChore();
	}

	// Token: 0x0400173E RID: 5950
	public float emptyRate = 10f;

	// Token: 0x0400173F RID: 5951
	[Serialize]
	public bool allowManualPumpingStationFetching;

	// Token: 0x04001740 RID: 5952
	[Serialize]
	public bool emit = true;

	// Token: 0x04001741 RID: 5953
	public bool isGasEmptier;

	// Token: 0x04001742 RID: 5954
	private static Dictionary<bool, string[]> manualPumpingAffectedBuildings = new Dictionary<bool, string[]>();

	// Token: 0x04001743 RID: 5955
	private static readonly EventSystem.IntraObjectHandler<BottleEmptier> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<BottleEmptier>(delegate(BottleEmptier component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001744 RID: 5956
	private static readonly EventSystem.IntraObjectHandler<BottleEmptier> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<BottleEmptier>(delegate(BottleEmptier component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0200144C RID: 5196
	public class StatesInstance : GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.GameInstance
	{
		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06008A0D RID: 35341 RVA: 0x00332251 File Offset: 0x00330451
		// (set) Token: 0x06008A0E RID: 35342 RVA: 0x00332259 File Offset: 0x00330459
		public MeterController meter { get; private set; }

		// Token: 0x06008A0F RID: 35343 RVA: 0x00332264 File Offset: 0x00330464
		public StatesInstance(BottleEmptier smi) : base(smi)
		{
			TreeFilterable component = base.master.GetComponent<TreeFilterable>();
			component.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(component.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target",
				"meter_arrow",
				"meter_scale"
			});
			this.meter.meterController.GetComponent<KBatchedAnimTracker>().synchronizeEnabledState = false;
			this.meter.meterController.enabled = false;
			base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
			base.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		}

		// Token: 0x06008A10 RID: 35344 RVA: 0x00332338 File Offset: 0x00330538
		public void CreateChore()
		{
			HashSet<Tag> tags = base.GetComponent<TreeFilterable>().GetTags();
			Tag[] forbidden_tags;
			if (!base.master.allowManualPumpingStationFetching)
			{
				forbidden_tags = new Tag[]
				{
					GameTags.LiquidSource,
					GameTags.GasSource
				};
			}
			else
			{
				forbidden_tags = new Tag[0];
			}
			Storage component = base.GetComponent<Storage>();
			this.chore = new FetchChore(Db.Get().ChoreTypes.StorageFetch, component, component.Capacity(), tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, forbidden_tags, null, true, null, null, null, Operational.State.Operational, 0);
		}

		// Token: 0x06008A11 RID: 35345 RVA: 0x003323BD File Offset: 0x003305BD
		public void CancelChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("Storage Changed");
				this.chore = null;
			}
		}

		// Token: 0x06008A12 RID: 35346 RVA: 0x003323DE File Offset: 0x003305DE
		public void RefreshChore()
		{
			this.GoTo(base.sm.unoperational);
		}

		// Token: 0x06008A13 RID: 35347 RVA: 0x003323F1 File Offset: 0x003305F1
		private void OnFilterChanged(HashSet<Tag> tags)
		{
			this.RefreshChore();
		}

		// Token: 0x06008A14 RID: 35348 RVA: 0x003323FC File Offset: 0x003305FC
		private void OnStorageChange(object data)
		{
			this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.RemainingCapacity() / this.storage.capacityKg));
			this.meter.meterController.enabled = (this.storage.MassStored() > 0f);
		}

		// Token: 0x06008A15 RID: 35349 RVA: 0x00332452 File Offset: 0x00330652
		private void OnOnlyFetchMarkedItemsSettingChanged(object data)
		{
			this.RefreshChore();
		}

		// Token: 0x06008A16 RID: 35350 RVA: 0x0033245C File Offset: 0x0033065C
		public void StartMeter()
		{
			PrimaryElement firstPrimaryElement = this.GetFirstPrimaryElement();
			if (firstPrimaryElement == null)
			{
				return;
			}
			base.GetComponent<KBatchedAnimController>().SetSymbolTint(new KAnimHashedString("leak_ceiling"), firstPrimaryElement.Element.substance.colour);
			this.meter.meterController.SwapAnims(firstPrimaryElement.Element.substance.anims);
			this.meter.meterController.Play("empty", KAnim.PlayMode.Paused, 1f, 0f);
			Color32 colour = firstPrimaryElement.Element.substance.colour;
			colour.a = byte.MaxValue;
			this.meter.SetSymbolTint(new KAnimHashedString("meter_fill"), colour);
			this.meter.SetSymbolTint(new KAnimHashedString("water1"), colour);
			this.meter.SetSymbolTint(new KAnimHashedString("substance_tinter"), colour);
			this.OnStorageChange(null);
		}

		// Token: 0x06008A17 RID: 35351 RVA: 0x00332550 File Offset: 0x00330750
		private PrimaryElement GetFirstPrimaryElement()
		{
			for (int i = 0; i < this.storage.Count; i++)
			{
				GameObject gameObject = this.storage[i];
				if (!(gameObject == null))
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					if (!(component == null))
					{
						return component;
					}
				}
			}
			return null;
		}

		// Token: 0x06008A18 RID: 35352 RVA: 0x0033259C File Offset: 0x0033079C
		public void Emit(float dt)
		{
			if (!base.smi.master.emit)
			{
				return;
			}
			PrimaryElement firstPrimaryElement = this.GetFirstPrimaryElement();
			if (firstPrimaryElement == null)
			{
				return;
			}
			float num = Mathf.Min(firstPrimaryElement.Mass, base.master.emptyRate * dt);
			if (num <= 0f)
			{
				return;
			}
			Tag prefabTag = firstPrimaryElement.GetComponent<KPrefabID>().PrefabTag;
			float num2;
			SimUtil.DiseaseInfo diseaseInfo;
			float temperature;
			this.storage.ConsumeAndGetDisease(prefabTag, num, out num2, out diseaseInfo, out temperature);
			Vector3 position = base.transform.GetPosition();
			position.y += 1.8f;
			bool flag = base.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH;
			position.x += (flag ? -0.2f : 0.2f);
			int num3 = Grid.PosToCell(position) + (flag ? -1 : 1);
			if (Grid.Solid[num3])
			{
				num3 += (flag ? 1 : -1);
			}
			Element element = firstPrimaryElement.Element;
			ushort idx = element.idx;
			if (element.IsLiquid)
			{
				FallingWater.instance.AddParticle(num3, idx, num2, temperature, diseaseInfo.idx, diseaseInfo.count, true, false, false, false);
				return;
			}
			SimMessages.ModifyCell(num3, idx, temperature, num2, diseaseInfo.idx, diseaseInfo.count, SimMessages.ReplaceType.None, false, -1);
		}

		// Token: 0x04006953 RID: 26963
		[MyCmpGet]
		public Storage storage;

		// Token: 0x04006954 RID: 26964
		private FetchChore chore;
	}

	// Token: 0x0200144D RID: 5197
	public class States : GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier>
	{
		// Token: 0x06008A19 RID: 35353 RVA: 0x003326D8 File Offset: 0x003308D8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.waitingfordelivery;
			this.statusItem = new StatusItem("BottleEmptier", "", "", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItem.resolveStringCallback = delegate(string str, object data)
			{
				BottleEmptier bottleEmptier = (BottleEmptier)data;
				if (bottleEmptier == null)
				{
					return str;
				}
				if (bottleEmptier.allowManualPumpingStationFetching)
				{
					return bottleEmptier.isGasEmptier ? BUILDING.STATUSITEMS.CANISTER_EMPTIER.ALLOWED.NAME : BUILDING.STATUSITEMS.BOTTLE_EMPTIER.ALLOWED.NAME;
				}
				return bottleEmptier.isGasEmptier ? BUILDING.STATUSITEMS.CANISTER_EMPTIER.DENIED.NAME : BUILDING.STATUSITEMS.BOTTLE_EMPTIER.DENIED.NAME;
			};
			this.statusItem.resolveTooltipCallback = delegate(string str, object data)
			{
				BottleEmptier bottleEmptier = (BottleEmptier)data;
				if (bottleEmptier == null)
				{
					return str;
				}
				string result;
				if (bottleEmptier.allowManualPumpingStationFetching)
				{
					if (bottleEmptier.isGasEmptier)
					{
						result = BUILDING.STATUSITEMS.CANISTER_EMPTIER.ALLOWED.TOOLTIP;
					}
					else
					{
						result = BUILDING.STATUSITEMS.BOTTLE_EMPTIER.ALLOWED.TOOLTIP;
					}
				}
				else if (bottleEmptier.isGasEmptier)
				{
					result = BUILDING.STATUSITEMS.CANISTER_EMPTIER.DENIED.TOOLTIP;
				}
				else
				{
					result = BUILDING.STATUSITEMS.BOTTLE_EMPTIER.DENIED.TOOLTIP;
				}
				return result;
			};
			this.root.ToggleStatusItem(this.statusItem, (BottleEmptier.StatesInstance smi) => smi.master);
			this.unoperational.TagTransition(GameTags.Operational, this.waitingfordelivery, false).PlayAnim("off");
			this.waitingfordelivery.TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.emptying, (BottleEmptier.StatesInstance smi) => smi.GetComponent<Storage>().MassStored() > 0f).Enter("CreateChore", delegate(BottleEmptier.StatesInstance smi)
			{
				smi.CreateChore();
			}).Exit("CancelChore", delegate(BottleEmptier.StatesInstance smi)
			{
				smi.CancelChore();
			}).PlayAnim("on");
			this.emptying.TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.waitingfordelivery, (BottleEmptier.StatesInstance smi) => smi.GetComponent<Storage>().MassStored() == 0f).Enter("StartMeter", delegate(BottleEmptier.StatesInstance smi)
			{
				smi.StartMeter();
			}).Update("Emit", delegate(BottleEmptier.StatesInstance smi, float dt)
			{
				smi.Emit(dt);
			}, UpdateRate.SIM_200ms, false).PlayAnim("working_loop", KAnim.PlayMode.Loop);
		}

		// Token: 0x04006956 RID: 26966
		private StatusItem statusItem;

		// Token: 0x04006957 RID: 26967
		public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State unoperational;

		// Token: 0x04006958 RID: 26968
		public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State waitingfordelivery;

		// Token: 0x04006959 RID: 26969
		public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State emptying;
	}
}
