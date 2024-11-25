using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000777 RID: 1911
public class StorageTile : GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>
{
	// Token: 0x060033A9 RID: 13225 RVA: 0x0011AF24 File Offset: 0x00119124
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.root.PlayAnim("on").EventHandler(GameHashes.OnStorageChange, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.OnStorageChanged)).EventHandler(GameHashes.StorageTileTargetItemChanged, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals));
		this.idle.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals)).EventTransition(GameHashes.OnStorageChange, this.awaitingDelivery, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingDelivery)).EventTransition(GameHashes.StorageTileTargetItemChanged, this.change, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingForSettingChange));
		this.change.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals)).EventTransition(GameHashes.StorageTileTargetItemChanged, this.idle, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.NoLongerAwaitingForSettingChange)).DefaultState(this.change.awaitingSettingsChange);
		this.change.awaitingSettingsChange.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.StartWorkChore)).Exit(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.CancelWorkChore)).ToggleStatusItem(Db.Get().BuildingStatusItems.ChangeStorageTileTarget, null).WorkableCompleteTransition((StorageTile.Instance smi) => smi.GetWorkable(), this.change.complete);
		this.change.complete.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.ApplySettings)).Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.DropUndesiredItems)).EnterTransition(this.idle, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.HasAnyDesiredItemStored)).EnterTransition(this.awaitingDelivery, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingDelivery));
		this.awaitingDelivery.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals)).EventTransition(GameHashes.OnStorageChange, this.idle, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.HasAnyDesiredItemStored)).EventTransition(GameHashes.StorageTileTargetItemChanged, this.change, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingForSettingChange));
	}

	// Token: 0x060033AA RID: 13226 RVA: 0x0011B12F File Offset: 0x0011932F
	public static void DropUndesiredItems(StorageTile.Instance smi)
	{
		smi.DropUndesiredItems();
	}

	// Token: 0x060033AB RID: 13227 RVA: 0x0011B137 File Offset: 0x00119337
	public static void ApplySettings(StorageTile.Instance smi)
	{
		smi.ApplySettings();
	}

	// Token: 0x060033AC RID: 13228 RVA: 0x0011B13F File Offset: 0x0011933F
	public static void StartWorkChore(StorageTile.Instance smi)
	{
		smi.StartChangeSettingChore();
	}

	// Token: 0x060033AD RID: 13229 RVA: 0x0011B147 File Offset: 0x00119347
	public static void CancelWorkChore(StorageTile.Instance smi)
	{
		smi.CanceChangeSettingChore();
	}

	// Token: 0x060033AE RID: 13230 RVA: 0x0011B14F File Offset: 0x0011934F
	public static void RefreshContentVisuals(StorageTile.Instance smi)
	{
		smi.UpdateContentSymbol();
	}

	// Token: 0x060033AF RID: 13231 RVA: 0x0011B157 File Offset: 0x00119357
	public static bool IsAwaitingForSettingChange(StorageTile.Instance smi)
	{
		return smi.IsPendingChange;
	}

	// Token: 0x060033B0 RID: 13232 RVA: 0x0011B15F File Offset: 0x0011935F
	public static bool NoLongerAwaitingForSettingChange(StorageTile.Instance smi)
	{
		return !smi.IsPendingChange;
	}

	// Token: 0x060033B1 RID: 13233 RVA: 0x0011B16A File Offset: 0x0011936A
	public static bool HasAnyDesiredItemStored(StorageTile.Instance smi)
	{
		return smi.HasAnyDesiredContents;
	}

	// Token: 0x060033B2 RID: 13234 RVA: 0x0011B172 File Offset: 0x00119372
	public static void OnStorageChanged(StorageTile.Instance smi)
	{
		smi.PlayDoorAnimation();
		StorageTile.RefreshContentVisuals(smi);
	}

	// Token: 0x060033B3 RID: 13235 RVA: 0x0011B180 File Offset: 0x00119380
	public static bool IsAwaitingDelivery(StorageTile.Instance smi)
	{
		return !smi.IsPendingChange && !smi.HasAnyDesiredContents;
	}

	// Token: 0x04001E99 RID: 7833
	public const string METER_TARGET = "meter_target";

	// Token: 0x04001E9A RID: 7834
	public const string METER_ANIMATION = "meter";

	// Token: 0x04001E9B RID: 7835
	public static HashedString DOOR_SYMBOL_NAME = new HashedString("storage_door");

	// Token: 0x04001E9C RID: 7836
	public static HashedString ITEM_SYMBOL_TARGET = new HashedString("meter_target_object");

	// Token: 0x04001E9D RID: 7837
	public static HashedString ITEM_SYMBOL_NAME = new HashedString("object");

	// Token: 0x04001E9E RID: 7838
	public const string ITEM_SYMBOL_ANIMATION = "meter_object";

	// Token: 0x04001E9F RID: 7839
	public static HashedString ITEM_PREVIEW_SYMBOL_TARGET = new HashedString("meter_target_object_ui");

	// Token: 0x04001EA0 RID: 7840
	public static HashedString ITEM_PREVIEW_SYMBOL_NAME = new HashedString("object_ui");

	// Token: 0x04001EA1 RID: 7841
	public const string ITEM_PREVIEW_SYMBOL_ANIMATION = "meter_object_ui";

	// Token: 0x04001EA2 RID: 7842
	public static HashedString ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME = new HashedString("placeholder");

	// Token: 0x04001EA3 RID: 7843
	public const string DEFAULT_ANIMATION_NAME = "on";

	// Token: 0x04001EA4 RID: 7844
	public const string STORAGE_CHANGE_ANIMATION_NAME = "door";

	// Token: 0x04001EA5 RID: 7845
	public const string SYMBOL_ANIMATION_NAME_AWAITING_DELIVERY = "ui";

	// Token: 0x04001EA6 RID: 7846
	public static Tag INVALID_TAG = GameTags.Void;

	// Token: 0x04001EA7 RID: 7847
	private StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.TagParameter TargetItemTag = new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.TagParameter(StorageTile.INVALID_TAG);

	// Token: 0x04001EA8 RID: 7848
	public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State idle;

	// Token: 0x04001EA9 RID: 7849
	public StorageTile.SettingsChangeStates change;

	// Token: 0x04001EAA RID: 7850
	public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State awaitingDelivery;

	// Token: 0x02001613 RID: 5651
	public class SpecificItemTagSizeInstruction
	{
		// Token: 0x060090C6 RID: 37062 RVA: 0x0034D2E7 File Offset: 0x0034B4E7
		public SpecificItemTagSizeInstruction(Tag tag, float size)
		{
			this.tag = tag;
			this.sizeMultiplier = size;
		}

		// Token: 0x04006E8B RID: 28299
		public Tag tag;

		// Token: 0x04006E8C RID: 28300
		public float sizeMultiplier;
	}

	// Token: 0x02001614 RID: 5652
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060090C7 RID: 37063 RVA: 0x0034D300 File Offset: 0x0034B500
		public StorageTile.SpecificItemTagSizeInstruction GetSizeInstructionForObject(GameObject obj)
		{
			if (this.specialItemCases == null)
			{
				return null;
			}
			KPrefabID component = obj.GetComponent<KPrefabID>();
			foreach (StorageTile.SpecificItemTagSizeInstruction specificItemTagSizeInstruction in this.specialItemCases)
			{
				if (component.HasTag(specificItemTagSizeInstruction.tag))
				{
					return specificItemTagSizeInstruction;
				}
			}
			return null;
		}

		// Token: 0x04006E8D RID: 28301
		public float MaxCapacity;

		// Token: 0x04006E8E RID: 28302
		public StorageTile.SpecificItemTagSizeInstruction[] specialItemCases;
	}

	// Token: 0x02001615 RID: 5653
	public class SettingsChangeStates : GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State
	{
		// Token: 0x04006E8F RID: 28303
		public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State awaitingSettingsChange;

		// Token: 0x04006E90 RID: 28304
		public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State complete;
	}

	// Token: 0x02001616 RID: 5654
	public new class Instance : GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.GameInstance, IUserControlledCapacity
	{
		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x060090CA RID: 37066 RVA: 0x0034D358 File Offset: 0x0034B558
		public Tag TargetTag
		{
			get
			{
				return base.smi.sm.TargetItemTag.Get(base.smi);
			}
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x060090CB RID: 37067 RVA: 0x0034D375 File Offset: 0x0034B575
		public bool HasContents
		{
			get
			{
				return this.storage.MassStored() > 0f;
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x060090CC RID: 37068 RVA: 0x0034D389 File Offset: 0x0034B589
		public bool HasAnyDesiredContents
		{
			get
			{
				if (!(this.TargetTag == StorageTile.INVALID_TAG))
				{
					return this.AmountOfDesiredContentStored > 0f;
				}
				return !this.HasContents;
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x060090CD RID: 37069 RVA: 0x0034D3B4 File Offset: 0x0034B5B4
		public float AmountOfDesiredContentStored
		{
			get
			{
				if (!(this.TargetTag == StorageTile.INVALID_TAG))
				{
					return this.storage.GetMassAvailable(this.TargetTag);
				}
				return 0f;
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x060090CE RID: 37070 RVA: 0x0034D3DF File Offset: 0x0034B5DF
		public bool IsPendingChange
		{
			get
			{
				return this.GetTreeFilterableCurrentTag() != this.TargetTag;
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x060090CF RID: 37071 RVA: 0x0034D3F2 File Offset: 0x0034B5F2
		// (set) Token: 0x060090D0 RID: 37072 RVA: 0x0034D40A File Offset: 0x0034B60A
		public float UserMaxCapacity
		{
			get
			{
				return Mathf.Min(this.userMaxCapacity, this.storage.capacityKg);
			}
			set
			{
				this.userMaxCapacity = value;
				this.filteredStorage.FilterChanged();
				this.RefreshAmountMeter();
			}
		}

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x060090D1 RID: 37073 RVA: 0x0034D424 File Offset: 0x0034B624
		public float AmountStored
		{
			get
			{
				return this.storage.MassStored();
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x060090D2 RID: 37074 RVA: 0x0034D431 File Offset: 0x0034B631
		public float MinCapacity
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x060090D3 RID: 37075 RVA: 0x0034D438 File Offset: 0x0034B638
		public float MaxCapacity
		{
			get
			{
				return base.def.MaxCapacity;
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x060090D4 RID: 37076 RVA: 0x0034D445 File Offset: 0x0034B645
		public bool WholeValues
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x060090D5 RID: 37077 RVA: 0x0034D448 File Offset: 0x0034B648
		public LocString CapacityUnits
		{
			get
			{
				return GameUtil.GetCurrentMassUnit(false);
			}
		}

		// Token: 0x060090D6 RID: 37078 RVA: 0x0034D450 File Offset: 0x0034B650
		private Tag GetTreeFilterableCurrentTag()
		{
			if (this.treeFilterable.GetTags() != null && this.treeFilterable.GetTags().Count != 0)
			{
				return this.treeFilterable.GetTags().GetRandom<Tag>();
			}
			return StorageTile.INVALID_TAG;
		}

		// Token: 0x060090D7 RID: 37079 RVA: 0x0034D487 File Offset: 0x0034B687
		public StorageTileSwitchItemWorkable GetWorkable()
		{
			return base.smi.gameObject.GetComponent<StorageTileSwitchItemWorkable>();
		}

		// Token: 0x060090D8 RID: 37080 RVA: 0x0034D49C File Offset: 0x0034B69C
		public Instance(IStateMachineTarget master, StorageTile.Def def) : base(master, def)
		{
			this.itemSymbol = this.CreateSymbolOverrideCapsule(StorageTile.ITEM_SYMBOL_TARGET, StorageTile.ITEM_SYMBOL_NAME, "meter_object");
			this.itemSymbol.usingNewSymbolOverrideSystem = true;
			this.itemSymbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(this.itemSymbol.gameObject);
			this.itemPreviewSymbol = this.CreateSymbolOverrideCapsule(StorageTile.ITEM_PREVIEW_SYMBOL_TARGET, StorageTile.ITEM_PREVIEW_SYMBOL_NAME, "meter_object_ui");
			this.defaultItemSymbolScale = this.itemSymbol.transform.localScale.x;
			this.defaultItemLocalPosition = this.itemSymbol.transform.localPosition;
			this.doorSymbol = this.CreateEmptyKAnimController(StorageTile.DOOR_SYMBOL_NAME.ToString());
			this.doorSymbol.initialAnim = "on";
			foreach (KAnim.Build.Symbol symbol in this.doorSymbol.AnimFiles[0].GetData().build.symbols)
			{
				this.doorSymbol.SetSymbolVisiblity(symbol.hash, symbol.hash == StorageTile.DOOR_SYMBOL_NAME);
			}
			this.doorSymbol.transform.SetParent(this.animController.transform, false);
			this.doorSymbol.transform.SetLocalPosition(-Vector3.forward * 0.05f);
			this.doorSymbol.onAnimComplete += this.OnDoorAnimationCompleted;
			this.doorSymbol.gameObject.SetActive(true);
			this.animController.SetSymbolVisiblity(StorageTile.DOOR_SYMBOL_NAME, false);
			this.doorAnimLink = new KAnimLink(this.animController, this.doorSymbol);
			this.amountMeter = new MeterController(this.animController, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
			ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(this.choreTypeID);
			this.filteredStorage = new FilteredStorage(this.storage, null, this, false, fetch_chore_type);
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			base.Subscribe(1606648047, new Action<object>(this.OnObjectReplaced));
		}

		// Token: 0x060090D9 RID: 37081 RVA: 0x0034D703 File Offset: 0x0034B903
		public override void StartSM()
		{
			base.StartSM();
			this.filteredStorage.FilterChanged();
		}

		// Token: 0x060090DA RID: 37082 RVA: 0x0034D718 File Offset: 0x0034B918
		private void OnObjectReplaced(object data)
		{
			Constructable.ReplaceCallbackParameters replaceCallbackParameters = (Constructable.ReplaceCallbackParameters)data;
			List<GameObject> list = new List<GameObject>();
			Storage storage = this.storage;
			bool vent_gas = false;
			bool dump_liquid = false;
			List<GameObject> collect_dropped_items = list;
			storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
			if (replaceCallbackParameters.Worker != null)
			{
				foreach (GameObject gameObject in list)
				{
					gameObject.GetComponent<Pickupable>().Trigger(580035959, replaceCallbackParameters.Worker);
				}
			}
		}

		// Token: 0x060090DB RID: 37083 RVA: 0x0034D7AC File Offset: 0x0034B9AC
		private void OnDoorAnimationCompleted(HashedString animName)
		{
			if (animName == "door")
			{
				this.doorSymbol.Play("on", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x060090DC RID: 37084 RVA: 0x0034D7E0 File Offset: 0x0034B9E0
		private KBatchedAnimController CreateEmptyKAnimController(string name)
		{
			GameObject gameObject = new GameObject(base.gameObject.name + "-" + name);
			gameObject.SetActive(false);
			KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
			kbatchedAnimController.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("storagetile_kanim")
			};
			kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingFront;
			return kbatchedAnimController;
		}

		// Token: 0x060090DD RID: 37085 RVA: 0x0034D83C File Offset: 0x0034BA3C
		private KBatchedAnimController CreateSymbolOverrideCapsule(HashedString symbolTarget, HashedString symbolName, string animationName)
		{
			KBatchedAnimController kbatchedAnimController = this.CreateEmptyKAnimController(symbolTarget.ToString());
			kbatchedAnimController.initialAnim = animationName;
			bool flag;
			Matrix4x4 symbolTransform = this.animController.GetSymbolTransform(symbolTarget, out flag);
			bool flag2;
			Matrix2x3 symbolLocalTransform = this.animController.GetSymbolLocalTransform(symbolTarget, out flag2);
			Vector3 position = symbolTransform.GetColumn(3);
			Vector3 localScale = Vector3.one * symbolLocalTransform.m00;
			kbatchedAnimController.transform.SetParent(base.transform, false);
			kbatchedAnimController.transform.SetPosition(position);
			Vector3 localPosition = kbatchedAnimController.transform.localPosition;
			localPosition.z = -0.0025f;
			kbatchedAnimController.transform.localPosition = localPosition;
			kbatchedAnimController.transform.localScale = localScale;
			kbatchedAnimController.gameObject.SetActive(false);
			this.animController.SetSymbolVisiblity(symbolTarget, false);
			return kbatchedAnimController;
		}

		// Token: 0x060090DE RID: 37086 RVA: 0x0034D914 File Offset: 0x0034BB14
		private void OnCopySettings(object sourceOBJ)
		{
			if (sourceOBJ != null)
			{
				StorageTile.Instance smi = ((GameObject)sourceOBJ).GetSMI<StorageTile.Instance>();
				if (smi != null)
				{
					this.SetTargetItem(smi.TargetTag);
					this.UserMaxCapacity = smi.UserMaxCapacity;
				}
			}
		}

		// Token: 0x060090DF RID: 37087 RVA: 0x0034D94C File Offset: 0x0034BB4C
		public void RefreshAmountMeter()
		{
			float positionPercent = (this.UserMaxCapacity == 0f) ? 0f : Mathf.Clamp(this.AmountOfDesiredContentStored / this.UserMaxCapacity, 0f, 1f);
			this.amountMeter.SetPositionPercent(positionPercent);
		}

		// Token: 0x060090E0 RID: 37088 RVA: 0x0034D996 File Offset: 0x0034BB96
		public void PlayDoorAnimation()
		{
			this.doorSymbol.Play("door", KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x060090E1 RID: 37089 RVA: 0x0034D9B8 File Offset: 0x0034BBB8
		public void SetTargetItem(Tag tag)
		{
			base.sm.TargetItemTag.Set(tag, this, false);
			base.gameObject.Trigger(-2076953849, null);
		}

		// Token: 0x060090E2 RID: 37090 RVA: 0x0034D9E0 File Offset: 0x0034BBE0
		public void ApplySettings()
		{
			Tag treeFilterableCurrentTag = this.GetTreeFilterableCurrentTag();
			this.treeFilterable.RemoveTagFromFilter(treeFilterableCurrentTag);
		}

		// Token: 0x060090E3 RID: 37091 RVA: 0x0034DA00 File Offset: 0x0034BC00
		public void DropUndesiredItems()
		{
			Vector3 position = Grid.CellToPos(this.GetWorkable().LastCellWorkerUsed) + Vector3.right * Grid.CellSizeInMeters * 0.5f + Vector3.up * Grid.CellSizeInMeters * 0.5f;
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			if (this.TargetTag != StorageTile.INVALID_TAG)
			{
				this.treeFilterable.AddTagToFilter(this.TargetTag);
				GameObject[] array = this.storage.DropUnlessHasTag(this.TargetTag);
				if (array != null)
				{
					GameObject[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						array2[i].transform.SetPosition(position);
					}
				}
			}
			else
			{
				this.storage.DropAll(position, false, false, default(Vector3), true, null);
			}
			this.storage.DropUnlessHasTag(this.TargetTag);
		}

		// Token: 0x060090E4 RID: 37092 RVA: 0x0034DAF0 File Offset: 0x0034BCF0
		public void UpdateContentSymbol()
		{
			this.RefreshAmountMeter();
			bool flag = this.TargetTag == StorageTile.INVALID_TAG;
			if (flag && !this.HasContents)
			{
				this.itemSymbol.gameObject.SetActive(false);
				this.itemPreviewSymbol.gameObject.SetActive(false);
				this.animController.SetSymbolVisiblity(StorageTile.ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME, false);
				return;
			}
			bool flag2 = !flag && (this.IsPendingChange || !this.HasAnyDesiredContents);
			string text = "";
			GameObject gameObject = (this.TargetTag == StorageTile.INVALID_TAG) ? Assets.GetPrefab(this.storage.items[0].PrefabID()) : Assets.GetPrefab(this.TargetTag);
			KAnimFile animFileFromPrefabWithTag = global::Def.GetAnimFileFromPrefabWithTag(gameObject, flag2 ? "ui" : "", out text);
			this.animController.SetSymbolVisiblity(StorageTile.ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME, flag2);
			this.itemPreviewSymbol.gameObject.SetActive(flag2);
			this.itemSymbol.gameObject.SetActive(!flag2);
			if (flag2)
			{
				this.itemPreviewSymbol.SwapAnims(new KAnimFile[]
				{
					animFileFromPrefabWithTag
				});
				this.itemPreviewSymbol.Play(text, KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			if (gameObject.HasTag(GameTags.Egg))
			{
				string text2 = text;
				if (!string.IsNullOrEmpty(text2))
				{
					this.itemSymbolOverrideController.ApplySymbolOverridesByAffix(animFileFromPrefabWithTag, text2, null, 0);
				}
				text = gameObject.GetComponent<KBatchedAnimController>().initialAnim;
			}
			else
			{
				this.itemSymbolOverrideController.RemoveAllSymbolOverrides(0);
				text = gameObject.GetComponent<KBatchedAnimController>().initialAnim;
			}
			this.itemSymbol.SwapAnims(new KAnimFile[]
			{
				animFileFromPrefabWithTag
			});
			this.itemSymbol.Play(text, KAnim.PlayMode.Once, 1f, 0f);
			StorageTile.SpecificItemTagSizeInstruction sizeInstructionForObject = base.def.GetSizeInstructionForObject(gameObject);
			this.itemSymbol.transform.localScale = Vector3.one * ((sizeInstructionForObject != null) ? sizeInstructionForObject.sizeMultiplier : this.defaultItemSymbolScale);
			KCollider2D component = gameObject.GetComponent<KCollider2D>();
			Vector3 localPosition = this.defaultItemLocalPosition;
			localPosition.y += ((component == null || component is KCircleCollider2D) ? 0f : (-component.offset.y * 0.5f));
			this.itemSymbol.transform.localPosition = localPosition;
		}

		// Token: 0x060090E5 RID: 37093 RVA: 0x0034DD5B File Offset: 0x0034BF5B
		private void AbortChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("Change settings Chore aborted");
				this.chore = null;
			}
		}

		// Token: 0x060090E6 RID: 37094 RVA: 0x0034DD7C File Offset: 0x0034BF7C
		public void StartChangeSettingChore()
		{
			this.AbortChore();
			this.chore = new WorkChore<StorageTileSwitchItemWorkable>(Db.Get().ChoreTypes.Toggle, this.GetWorkable(), null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x060090E7 RID: 37095 RVA: 0x0034DDC0 File Offset: 0x0034BFC0
		public void CanceChangeSettingChore()
		{
			this.AbortChore();
		}

		// Token: 0x04006E91 RID: 28305
		[Serialize]
		private float userMaxCapacity = float.PositiveInfinity;

		// Token: 0x04006E92 RID: 28306
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04006E93 RID: 28307
		[MyCmpGet]
		private KBatchedAnimController animController;

		// Token: 0x04006E94 RID: 28308
		[MyCmpGet]
		private TreeFilterable treeFilterable;

		// Token: 0x04006E95 RID: 28309
		private FilteredStorage filteredStorage;

		// Token: 0x04006E96 RID: 28310
		private Chore chore;

		// Token: 0x04006E97 RID: 28311
		private MeterController amountMeter;

		// Token: 0x04006E98 RID: 28312
		private KBatchedAnimController doorSymbol;

		// Token: 0x04006E99 RID: 28313
		private KBatchedAnimController itemSymbol;

		// Token: 0x04006E9A RID: 28314
		private SymbolOverrideController itemSymbolOverrideController;

		// Token: 0x04006E9B RID: 28315
		private KBatchedAnimController itemPreviewSymbol;

		// Token: 0x04006E9C RID: 28316
		private KAnimLink doorAnimLink;

		// Token: 0x04006E9D RID: 28317
		private string choreTypeID = Db.Get().ChoreTypes.StorageFetch.Id;

		// Token: 0x04006E9E RID: 28318
		private float defaultItemSymbolScale = -1f;

		// Token: 0x04006E9F RID: 28319
		private Vector3 defaultItemLocalPosition = Vector3.zero;
	}
}
