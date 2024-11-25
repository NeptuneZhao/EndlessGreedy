﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005C6 RID: 1478
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Storage")]
public class Storage : Workable, ISaveLoadableDetails, IGameObjectEffectDescriptor, IStorage
{
	// Token: 0x1700019C RID: 412
	// (get) Token: 0x06002388 RID: 9096 RVA: 0x000C6203 File Offset: 0x000C4403
	public bool ShouldOnlyTransferFromLowerPriority
	{
		get
		{
			return this.onlyTransferFromLowerPriority || this.allowItemRemoval;
		}
	}

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x06002389 RID: 9097 RVA: 0x000C6215 File Offset: 0x000C4415
	// (set) Token: 0x0600238A RID: 9098 RVA: 0x000C621D File Offset: 0x000C441D
	public bool allowUIItemRemoval { get; set; }

	// Token: 0x1700019E RID: 414
	public GameObject this[int idx]
	{
		get
		{
			return this.items[idx];
		}
	}

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x0600238C RID: 9100 RVA: 0x000C6234 File Offset: 0x000C4434
	public int Count
	{
		get
		{
			return this.items.Count;
		}
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x0600238D RID: 9101 RVA: 0x000C6241 File Offset: 0x000C4441
	// (set) Token: 0x0600238E RID: 9102 RVA: 0x000C6249 File Offset: 0x000C4449
	public bool ShouldSaveItems
	{
		get
		{
			return this.shouldSaveItems;
		}
		set
		{
			this.shouldSaveItems = value;
		}
	}

	// Token: 0x0600238F RID: 9103 RVA: 0x000C6252 File Offset: 0x000C4452
	public bool ShouldShowInUI()
	{
		return this.showInUI;
	}

	// Token: 0x06002390 RID: 9104 RVA: 0x000C625A File Offset: 0x000C445A
	public List<GameObject> GetItems()
	{
		return this.items;
	}

	// Token: 0x06002391 RID: 9105 RVA: 0x000C6262 File Offset: 0x000C4462
	public void SetDefaultStoredItemModifiers(List<Storage.StoredItemModifier> modifiers)
	{
		this.defaultStoredItemModifers = modifiers;
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x06002392 RID: 9106 RVA: 0x000C626B File Offset: 0x000C446B
	public PrioritySetting masterPriority
	{
		get
		{
			if (this.prioritizable)
			{
				return this.prioritizable.GetMasterPriority();
			}
			return Chore.DefaultPrioritySetting;
		}
	}

	// Token: 0x06002393 RID: 9107 RVA: 0x000C628C File Offset: 0x000C448C
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		if (this.useGunForDelivery && worker.UsesMultiTool())
		{
			Workable.AnimInfo anim = base.GetAnim(worker);
			anim.smi = new MultitoolController.Instance(this, worker, "store", Assets.GetPrefab(EffectConfigs.OreAbsorbId));
			return anim;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x06002394 RID: 9108 RVA: 0x000C62E4 File Offset: 0x000C44E4
	public override Vector3 GetTargetPoint()
	{
		Vector3 vector = base.GetTargetPoint();
		if (this.useGunForDelivery && this.gunTargetOffset != Vector2.zero)
		{
			if (this.rotatable != null)
			{
				vector += this.rotatable.GetRotatedOffset(this.gunTargetOffset);
			}
			else
			{
				vector += new Vector3(this.gunTargetOffset.x, this.gunTargetOffset.y, 0f);
			}
		}
		return vector;
	}

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x06002395 RID: 9109 RVA: 0x000C6368 File Offset: 0x000C4568
	// (remove) Token: 0x06002396 RID: 9110 RVA: 0x000C63A0 File Offset: 0x000C45A0
	public event System.Action OnStorageIncreased;

	// Token: 0x06002397 RID: 9111 RVA: 0x000C63D8 File Offset: 0x000C45D8
	protected override void OnPrefabInit()
	{
		if (this.useWideOffsets)
		{
			base.SetOffsetTable(OffsetGroups.InvertedWideTable);
		}
		else
		{
			base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		}
		this.showProgressBar = false;
		this.faceTargetWhenWorking = true;
		base.OnPrefabInit();
		GameUtil.SubscribeToTags<Storage>(this, Storage.OnDeadTagAddedDelegate, true);
		base.Subscribe<Storage>(1502190696, Storage.OnQueueDestroyObjectDelegate);
		base.Subscribe<Storage>(-905833192, Storage.OnCopySettingsDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Storing;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.SetupStorageStatusItems();
	}

	// Token: 0x06002398 RID: 9112 RVA: 0x000C6480 File Offset: 0x000C4680
	private void SetupStorageStatusItems()
	{
		if (Storage.capacityStatusItem == null)
		{
			Storage.capacityStatusItem = new StatusItem("StorageLocker", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			Storage.capacityStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				Storage storage = (Storage)data;
				float num = storage.MassStored();
				float num2 = storage.capacityKg;
				if (num > num2 - storage.storageFullMargin && num < num2)
				{
					num = num2;
				}
				else
				{
					num = Mathf.Floor(num);
				}
				string newValue = Util.FormatWholeNumber(num);
				IUserControlledCapacity component = storage.GetComponent<IUserControlledCapacity>();
				if (component != null)
				{
					num2 = Mathf.Min(component.UserMaxCapacity, num2);
				}
				string newValue2 = Util.FormatWholeNumber(num2);
				str = str.Replace("{Stored}", newValue);
				str = str.Replace("{Capacity}", newValue2);
				if (component != null)
				{
					str = str.Replace("{Units}", component.CapacityUnits);
				}
				else
				{
					str = str.Replace("{Units}", GameUtil.GetCurrentMassUnit(false));
				}
				return str;
			};
		}
		if (this.showCapacityStatusItem)
		{
			if (this.showCapacityAsMainStatus)
			{
				base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Storage.capacityStatusItem, this);
				return;
			}
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Stored, Storage.capacityStatusItem, this);
		}
	}

	// Token: 0x06002399 RID: 9113 RVA: 0x000C6538 File Offset: 0x000C4738
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (!this.allowSettingOnlyFetchMarkedItems)
		{
			this.onlyFetchMarkedItems = false;
		}
		this.UpdateFetchCategory();
	}

	// Token: 0x0600239A RID: 9114 RVA: 0x000C6550 File Offset: 0x000C4750
	protected override void OnSpawn()
	{
		base.SetWorkTime(this.storageWorkTime);
		foreach (GameObject go in this.items)
		{
			this.ApplyStoredItemModifiers(go, true, true);
			if (this.sendOnStoreOnSpawn)
			{
				go.Trigger(856640610, this);
			}
		}
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.SetSymbolVisiblity("sweep", this.onlyFetchMarkedItems);
		}
		Prioritizable component2 = base.GetComponent<Prioritizable>();
		if (component2 != null)
		{
			Prioritizable prioritizable = component2;
			prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnPriorityChanged));
		}
		this.UpdateFetchCategory();
		if (this.showUnreachableStatus)
		{
			base.Subscribe<Storage>(-1432940121, Storage.OnReachableChangedDelegate);
			new ReachabilityMonitor.Instance(this).StartSM();
		}
	}

	// Token: 0x0600239B RID: 9115 RVA: 0x000C6648 File Offset: 0x000C4848
	public GameObject Store(GameObject go, bool hide_popups = false, bool block_events = false, bool do_disease_transfer = true, bool is_deserializing = false)
	{
		if (go == null)
		{
			return null;
		}
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		GameObject result = go;
		if (!hide_popups && PopFXManager.Instance != null)
		{
			LocString loc_string;
			Transform transform;
			if (this.fxPrefix == Storage.FXPrefix.Delivered)
			{
				loc_string = UI.DELIVERED;
				transform = base.transform;
			}
			else
			{
				loc_string = UI.PICKEDUP;
				transform = go.transform;
			}
			string text;
			if (!Assets.IsTagCountable(go.PrefabID()))
			{
				text = string.Format(loc_string, GameUtil.GetFormattedMass(component.Units, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), go.GetProperName());
			}
			else
			{
				text = string.Format(loc_string, (int)component.Units, go.GetProperName());
			}
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, text, transform, this.storageFXOffset, 1.5f, false, false);
		}
		go.transform.parent = base.transform;
		Vector3 position = Grid.CellToPosCCC(Grid.PosToCell(this), Grid.SceneLayer.Move);
		position.z = go.transform.GetPosition().z;
		go.transform.SetPosition(position);
		if (!block_events && do_disease_transfer)
		{
			this.TransferDiseaseWithObject(go);
		}
		if (!is_deserializing)
		{
			Pickupable component2 = go.GetComponent<Pickupable>();
			if (component2 != null)
			{
				if (component2 != null && component2.prevent_absorb_until_stored)
				{
					component2.prevent_absorb_until_stored = false;
				}
				foreach (GameObject gameObject in this.items)
				{
					if (gameObject != null)
					{
						Pickupable component3 = gameObject.GetComponent<Pickupable>();
						if (component3 != null && component3.TryAbsorb(component2, hide_popups, true))
						{
							if (!block_events)
							{
								base.Trigger(-1697596308, go);
								Action<GameObject> onStorageChange = this.OnStorageChange;
								if (onStorageChange != null)
								{
									onStorageChange(go);
								}
								base.Trigger(-778359855, this);
								if (this.OnStorageIncreased != null)
								{
									this.OnStorageIncreased();
								}
							}
							this.ApplyStoredItemModifiers(go, true, false);
							result = gameObject;
							go = null;
							break;
						}
					}
				}
			}
		}
		if (go != null)
		{
			this.items.Add(go);
			if (!is_deserializing)
			{
				this.ApplyStoredItemModifiers(go, true, false);
			}
			if (!block_events)
			{
				go.Trigger(856640610, this);
				base.Trigger(-1697596308, go);
				Action<GameObject> onStorageChange2 = this.OnStorageChange;
				if (onStorageChange2 != null)
				{
					onStorageChange2(go);
				}
				base.Trigger(-778359855, this);
				if (this.OnStorageIncreased != null)
				{
					this.OnStorageIncreased();
				}
			}
		}
		return result;
	}

	// Token: 0x0600239C RID: 9116 RVA: 0x000C68DC File Offset: 0x000C4ADC
	public PrimaryElement AddElement(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass = false, bool do_disease_transfer = true)
	{
		Element element2 = ElementLoader.FindElementByHash(element);
		if (element2.IsGas)
		{
			return this.AddGasChunk(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer);
		}
		if (element2.IsLiquid)
		{
			return this.AddLiquid(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer);
		}
		if (element2.IsSolid)
		{
			return this.AddOre(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer);
		}
		return null;
	}

	// Token: 0x0600239D RID: 9117 RVA: 0x000C6940 File Offset: 0x000C4B40
	public PrimaryElement AddOre(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass = false, bool do_disease_transfer = true)
	{
		if (mass <= 0f)
		{
			return null;
		}
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, mass);
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			primaryElement.Mass += mass;
			primaryElement.Temperature = finalTemperature;
			primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddOre");
			base.Trigger(-1697596308, primaryElement.gameObject);
			Action<GameObject> onStorageChange = this.OnStorageChange;
			if (onStorageChange != null)
			{
				onStorageChange(primaryElement.gameObject);
			}
		}
		else
		{
			Element element2 = ElementLoader.FindElementByHash(element);
			GameObject gameObject = element2.substance.SpawnResource(base.transform.GetPosition(), mass, temperature, disease_idx, disease_count, true, false, true);
			gameObject.GetComponent<Pickupable>().prevent_absorb_until_stored = true;
			element2.substance.ActivateSubstanceGameObject(gameObject, disease_idx, disease_count);
			this.Store(gameObject, true, false, do_disease_transfer, false);
		}
		return primaryElement;
	}

	// Token: 0x0600239E RID: 9118 RVA: 0x000C6A24 File Offset: 0x000C4C24
	public PrimaryElement AddLiquid(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass = false, bool do_disease_transfer = true)
	{
		if (mass <= 0f)
		{
			return null;
		}
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, mass);
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			primaryElement.Mass += mass;
			primaryElement.Temperature = finalTemperature;
			primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddLiquid");
			base.Trigger(-1697596308, primaryElement.gameObject);
			Action<GameObject> onStorageChange = this.OnStorageChange;
			if (onStorageChange != null)
			{
				onStorageChange(primaryElement.gameObject);
			}
		}
		else
		{
			SubstanceChunk substanceChunk = LiquidSourceManager.Instance.CreateChunk(element, mass, temperature, disease_idx, disease_count, base.transform.GetPosition());
			primaryElement = substanceChunk.GetComponent<PrimaryElement>();
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			this.Store(substanceChunk.gameObject, true, false, do_disease_transfer, false);
		}
		return primaryElement;
	}

	// Token: 0x0600239F RID: 9119 RVA: 0x000C6AF8 File Offset: 0x000C4CF8
	public PrimaryElement AddGasChunk(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass, bool do_disease_transfer = true)
	{
		if (mass <= 0f)
		{
			return null;
		}
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float mass2 = primaryElement.Mass;
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, mass2, temperature, mass);
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			primaryElement.SetMassTemperature(mass2 + mass, finalTemperature);
			primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddGasChunk");
			base.Trigger(-1697596308, primaryElement.gameObject);
			Action<GameObject> onStorageChange = this.OnStorageChange;
			if (onStorageChange != null)
			{
				onStorageChange(primaryElement.gameObject);
			}
		}
		else
		{
			SubstanceChunk substanceChunk = GasSourceManager.Instance.CreateChunk(element, mass, temperature, disease_idx, disease_count, base.transform.GetPosition());
			primaryElement = substanceChunk.GetComponent<PrimaryElement>();
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			this.Store(substanceChunk.gameObject, true, false, do_disease_transfer, false);
		}
		return primaryElement;
	}

	// Token: 0x060023A0 RID: 9120 RVA: 0x000C6BC0 File Offset: 0x000C4DC0
	public void Transfer(Storage target, bool block_events = false, bool hide_popups = false)
	{
		while (this.items.Count > 0)
		{
			this.Transfer(this.items[0], target, block_events, hide_popups);
		}
	}

	// Token: 0x060023A1 RID: 9121 RVA: 0x000C6BE8 File Offset: 0x000C4DE8
	public bool TransferMass(Storage dest_storage, Tag tag, float amount, bool flatten = false, bool block_events = false, bool hide_popups = false)
	{
		float num = amount;
		while (num > 0f && this.GetAmountAvailable(tag) > 0f)
		{
			num -= this.Transfer(dest_storage, tag, num, block_events, hide_popups);
		}
		if (flatten)
		{
			dest_storage.Flatten(tag);
		}
		return num <= 0f;
	}

	// Token: 0x060023A2 RID: 9122 RVA: 0x000C6C38 File Offset: 0x000C4E38
	public float Transfer(Storage dest_storage, Tag tag, float amount, bool block_events = false, bool hide_popups = false)
	{
		GameObject gameObject = this.FindFirst(tag);
		if (gameObject != null)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (amount < component.Units)
			{
				Pickupable component2 = gameObject.GetComponent<Pickupable>();
				Pickupable pickupable = component2.Take(amount);
				dest_storage.Store(pickupable.gameObject, hide_popups, block_events, true, false);
				if (!block_events)
				{
					base.Trigger(-1697596308, component2.gameObject);
					Action<GameObject> onStorageChange = this.OnStorageChange;
					if (onStorageChange != null)
					{
						onStorageChange(component2.gameObject);
					}
				}
			}
			else
			{
				this.Transfer(gameObject, dest_storage, block_events, hide_popups);
				amount = component.Units;
			}
			return amount;
		}
		return 0f;
	}

	// Token: 0x060023A3 RID: 9123 RVA: 0x000C6CD4 File Offset: 0x000C4ED4
	public bool Transfer(GameObject go, Storage target, bool block_events = false, bool hide_popups = false)
	{
		this.items.RemoveAll((GameObject it) => it == null);
		int count = this.items.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.items[i] == go)
			{
				this.items.RemoveAt(i);
				this.ApplyStoredItemModifiers(go, false, false);
				target.Store(go, hide_popups, block_events, true, false);
				if (!block_events)
				{
					base.Trigger(-1697596308, go);
					Action<GameObject> onStorageChange = this.OnStorageChange;
					if (onStorageChange != null)
					{
						onStorageChange(go);
					}
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x060023A4 RID: 9124 RVA: 0x000C6D80 File Offset: 0x000C4F80
	public bool DropSome(Tag tag, float amount, bool ventGas = false, bool dumpLiquid = false, Vector3 offset = default(Vector3), bool doDiseaseTransfer = true, bool showInWorldNotification = false)
	{
		bool result = false;
		float num = amount;
		ListPool<GameObject, Storage>.PooledList pooledList = ListPool<GameObject, Storage>.Allocate();
		this.Find(tag, pooledList);
		foreach (GameObject gameObject in pooledList)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			if (component)
			{
				Pickupable pickupable = component.Take(num);
				if (pickupable != null)
				{
					bool flag = false;
					if (ventGas || dumpLiquid)
					{
						Dumpable component2 = pickupable.GetComponent<Dumpable>();
						if (component2 != null)
						{
							if (ventGas && pickupable.GetComponent<PrimaryElement>().Element.IsGas)
							{
								component2.Dump(base.transform.GetPosition() + offset);
								flag = true;
								num -= pickupable.GetComponent<PrimaryElement>().Mass;
								base.Trigger(-1697596308, pickupable.gameObject);
								Action<GameObject> onStorageChange = this.OnStorageChange;
								if (onStorageChange != null)
								{
									onStorageChange(pickupable.gameObject);
								}
								result = true;
								if (showInWorldNotification)
								{
									PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, pickupable.GetComponent<PrimaryElement>().Element.name + " " + GameUtil.GetFormattedMass(pickupable.TotalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), pickupable.transform, this.storageFXOffset, 1.5f, false, false);
								}
							}
							if (dumpLiquid && pickupable.GetComponent<PrimaryElement>().Element.IsLiquid)
							{
								component2.Dump(base.transform.GetPosition() + offset);
								flag = true;
								num -= pickupable.GetComponent<PrimaryElement>().Mass;
								base.Trigger(-1697596308, pickupable.gameObject);
								Action<GameObject> onStorageChange2 = this.OnStorageChange;
								if (onStorageChange2 != null)
								{
									onStorageChange2(pickupable.gameObject);
								}
								result = true;
								if (showInWorldNotification)
								{
									PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, pickupable.GetComponent<PrimaryElement>().Element.name + " " + GameUtil.GetFormattedMass(pickupable.TotalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), pickupable.transform, this.storageFXOffset, 1.5f, false, false);
								}
							}
						}
					}
					if (!flag)
					{
						Vector3 position = Grid.CellToPosCCC(Grid.PosToCell(this), Grid.SceneLayer.Ore) + offset;
						pickupable.transform.SetPosition(position);
						KBatchedAnimController component3 = pickupable.GetComponent<KBatchedAnimController>();
						if (component3)
						{
							component3.SetSceneLayer(Grid.SceneLayer.Ore);
						}
						num -= pickupable.GetComponent<PrimaryElement>().Mass;
						this.MakeWorldActive(pickupable.gameObject);
						base.Trigger(-1697596308, pickupable.gameObject);
						Action<GameObject> onStorageChange3 = this.OnStorageChange;
						if (onStorageChange3 != null)
						{
							onStorageChange3(pickupable.gameObject);
						}
						result = true;
						if (showInWorldNotification)
						{
							PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, pickupable.GetComponent<PrimaryElement>().Element.name + " " + GameUtil.GetFormattedMass(pickupable.TotalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), pickupable.transform, this.storageFXOffset, 1.5f, false, false);
						}
					}
				}
			}
			if (num <= 0f)
			{
				break;
			}
		}
		pooledList.Recycle();
		return result;
	}

	// Token: 0x060023A5 RID: 9125 RVA: 0x000C70D4 File Offset: 0x000C52D4
	public void DropAll(Vector3 position, bool vent_gas = false, bool dump_liquid = false, Vector3 offset = default(Vector3), bool do_disease_transfer = true, List<GameObject> collect_dropped_items = null)
	{
		while (this.items.Count > 0)
		{
			GameObject gameObject = this.items[0];
			if (do_disease_transfer)
			{
				this.TransferDiseaseWithObject(gameObject);
			}
			this.items.RemoveAt(0);
			if (gameObject != null)
			{
				bool flag = false;
				if (vent_gas || dump_liquid)
				{
					Dumpable component = gameObject.GetComponent<Dumpable>();
					if (component != null)
					{
						if (vent_gas && gameObject.GetComponent<PrimaryElement>().Element.IsGas)
						{
							component.Dump(position + offset);
							flag = true;
						}
						if (dump_liquid && gameObject.GetComponent<PrimaryElement>().Element.IsLiquid)
						{
							component.Dump(position + offset);
							flag = true;
						}
					}
				}
				if (!flag)
				{
					gameObject.transform.SetPosition(position + offset);
					KBatchedAnimController component2 = gameObject.GetComponent<KBatchedAnimController>();
					if (component2)
					{
						component2.SetSceneLayer(Grid.SceneLayer.Ore);
					}
					this.MakeWorldActive(gameObject);
					if (collect_dropped_items != null)
					{
						collect_dropped_items.Add(gameObject);
					}
				}
			}
		}
	}

	// Token: 0x060023A6 RID: 9126 RVA: 0x000C71C9 File Offset: 0x000C53C9
	public void DropAll(bool vent_gas = false, bool dump_liquid = false, Vector3 offset = default(Vector3), bool do_disease_transfer = true, List<GameObject> collect_dropped_items = null)
	{
		this.DropAll(Grid.CellToPosCCC(Grid.PosToCell(this), Grid.SceneLayer.Ore), vent_gas, dump_liquid, offset, do_disease_transfer, collect_dropped_items);
	}

	// Token: 0x060023A7 RID: 9127 RVA: 0x000C71E8 File Offset: 0x000C53E8
	public void Drop(Tag t, List<GameObject> obj_list)
	{
		this.Find(t, obj_list);
		foreach (GameObject go in obj_list)
		{
			this.Drop(go, true);
		}
	}

	// Token: 0x060023A8 RID: 9128 RVA: 0x000C7244 File Offset: 0x000C5444
	public void Drop(Tag t)
	{
		ListPool<GameObject, Storage>.PooledList pooledList = ListPool<GameObject, Storage>.Allocate();
		this.Find(t, pooledList);
		foreach (GameObject go in pooledList)
		{
			this.Drop(go, true);
		}
		pooledList.Recycle();
	}

	// Token: 0x060023A9 RID: 9129 RVA: 0x000C72AC File Offset: 0x000C54AC
	public void DropUnlessMatching(FetchChore chore)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null))
			{
				KPrefabID component = this.items[i].GetComponent<KPrefabID>();
				if (!(((chore.criteria == FetchChore.MatchCriteria.MatchID && chore.tags.Contains(component.PrefabTag)) || (chore.criteria == FetchChore.MatchCriteria.MatchTags && component.HasTag(chore.tagsFirst))) & (!chore.requiredTag.IsValid || component.HasTag(chore.requiredTag)) & !component.HasAnyTags(chore.forbiddenTags)))
				{
					GameObject gameObject = this.items[i];
					this.items.RemoveAt(i);
					i--;
					this.TransferDiseaseWithObject(gameObject);
					this.MakeWorldActive(gameObject);
				}
			}
		}
	}

	// Token: 0x060023AA RID: 9130 RVA: 0x000C7390 File Offset: 0x000C5590
	public GameObject[] DropUnlessHasTag(Tag tag)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null) && !this.items[i].GetComponent<KPrefabID>().HasTag(tag))
			{
				GameObject gameObject = this.items[i];
				this.items.RemoveAt(i);
				i--;
				this.TransferDiseaseWithObject(gameObject);
				this.MakeWorldActive(gameObject);
				Dumpable component = gameObject.GetComponent<Dumpable>();
				if (component != null)
				{
					component.Dump(base.transform.GetPosition());
				}
				list.Add(gameObject);
			}
		}
		return list.ToArray();
	}

	// Token: 0x060023AB RID: 9131 RVA: 0x000C7448 File Offset: 0x000C5648
	public GameObject[] DropHasTags(Tag[] tag)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null) && this.items[i].GetComponent<KPrefabID>().HasAllTags(tag))
			{
				GameObject gameObject = this.items[i];
				this.items.RemoveAt(i);
				i--;
				this.TransferDiseaseWithObject(gameObject);
				this.MakeWorldActive(gameObject);
				Dumpable component = gameObject.GetComponent<Dumpable>();
				if (component != null)
				{
					component.Dump(base.transform.GetPosition());
				}
				list.Add(gameObject);
			}
		}
		return list.ToArray();
	}

	// Token: 0x060023AC RID: 9132 RVA: 0x000C7500 File Offset: 0x000C5700
	public GameObject Drop(GameObject go, bool do_disease_transfer = true)
	{
		if (go == null)
		{
			return null;
		}
		int count = this.items.Count;
		for (int i = 0; i < count; i++)
		{
			if (!(go != this.items[i]))
			{
				this.items[i] = this.items[count - 1];
				this.items.RemoveAt(count - 1);
				if (do_disease_transfer)
				{
					this.TransferDiseaseWithObject(go);
				}
				this.MakeWorldActive(go);
				break;
			}
		}
		return go;
	}

	// Token: 0x060023AD RID: 9133 RVA: 0x000C7580 File Offset: 0x000C5780
	public void RenotifyAll()
	{
		this.items.RemoveAll((GameObject it) => it == null);
		foreach (GameObject go in this.items)
		{
			go.Trigger(856640610, this);
		}
	}

	// Token: 0x060023AE RID: 9134 RVA: 0x000C7604 File Offset: 0x000C5804
	private void TransferDiseaseWithObject(GameObject obj)
	{
		if (obj == null || !this.doDiseaseTransfer || this.primaryElement == null)
		{
			return;
		}
		PrimaryElement component = obj.GetComponent<PrimaryElement>();
		if (component == null)
		{
			return;
		}
		SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
		invalid.idx = component.DiseaseIdx;
		invalid.count = (int)((float)component.DiseaseCount * 0.05f);
		SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
		invalid2.idx = this.primaryElement.DiseaseIdx;
		invalid2.count = (int)((float)this.primaryElement.DiseaseCount * 0.05f);
		component.ModifyDiseaseCount(-invalid.count, "Storage.TransferDiseaseWithObject");
		this.primaryElement.ModifyDiseaseCount(-invalid2.count, "Storage.TransferDiseaseWithObject");
		if (invalid.count > 0)
		{
			this.primaryElement.AddDisease(invalid.idx, invalid.count, "Storage.TransferDiseaseWithObject");
		}
		if (invalid2.count > 0)
		{
			component.AddDisease(invalid2.idx, invalid2.count, "Storage.TransferDiseaseWithObject");
		}
	}

	// Token: 0x060023AF RID: 9135 RVA: 0x000C770C File Offset: 0x000C590C
	private void MakeWorldActive(GameObject go)
	{
		go.transform.parent = null;
		if (this.dropOffset != Vector2.zero)
		{
			go.transform.Translate(this.dropOffset);
		}
		go.Trigger(856640610, null);
		base.Trigger(-1697596308, go);
		Action<GameObject> onStorageChange = this.OnStorageChange;
		if (onStorageChange != null)
		{
			onStorageChange(go);
		}
		this.ApplyStoredItemModifiers(go, false, false);
		if (go != null)
		{
			PrimaryElement component = go.GetComponent<PrimaryElement>();
			if (component != null && component.KeepZeroMassObject)
			{
				component.KeepZeroMassObject = false;
				if (component.Mass <= 0f)
				{
					Util.KDestroyGameObject(go);
				}
			}
		}
	}

	// Token: 0x060023B0 RID: 9136 RVA: 0x000C77BC File Offset: 0x000C59BC
	public List<GameObject> Find(Tag tag, List<GameObject> result)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				result.Add(gameObject);
			}
		}
		return result;
	}

	// Token: 0x060023B1 RID: 9137 RVA: 0x000C7808 File Offset: 0x000C5A08
	public GameObject FindFirst(Tag tag)
	{
		GameObject result = null;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				result = gameObject;
				break;
			}
		}
		return result;
	}

	// Token: 0x060023B2 RID: 9138 RVA: 0x000C7854 File Offset: 0x000C5A54
	public PrimaryElement FindFirstWithMass(Tag tag, float mass = 0f)
	{
		PrimaryElement result = null;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.Mass > 0f && component.Mass >= mass)
				{
					result = component;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060023B3 RID: 9139 RVA: 0x000C78BC File Offset: 0x000C5ABC
	private void Flatten(Tag tag_to_combine)
	{
		GameObject gameObject = this.FindFirst(tag_to_combine);
		if (gameObject == null)
		{
			return;
		}
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		for (int i = this.items.Count - 1; i >= 0; i--)
		{
			GameObject gameObject2 = this.items[i];
			if (gameObject2.HasTag(tag_to_combine) && gameObject2 != gameObject)
			{
				PrimaryElement component2 = gameObject2.GetComponent<PrimaryElement>();
				component.Mass += component2.Mass;
				this.ConsumeIgnoringDisease(gameObject2);
			}
		}
	}

	// Token: 0x060023B4 RID: 9140 RVA: 0x000C793C File Offset: 0x000C5B3C
	public HashSet<Tag> GetAllIDsInStorage()
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject go = this.items[i];
			hashSet.Add(go.PrefabID());
		}
		return hashSet;
	}

	// Token: 0x060023B5 RID: 9141 RVA: 0x000C7980 File Offset: 0x000C5B80
	public GameObject Find(int ID)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (ID == gameObject.PrefabID().GetHashCode())
			{
				return gameObject;
			}
		}
		return null;
	}

	// Token: 0x060023B6 RID: 9142 RVA: 0x000C79CA File Offset: 0x000C5BCA
	public void ConsumeAllIgnoringDisease()
	{
		this.ConsumeAllIgnoringDisease(Tag.Invalid);
	}

	// Token: 0x060023B7 RID: 9143 RVA: 0x000C79D8 File Offset: 0x000C5BD8
	public void ConsumeAllIgnoringDisease(Tag tag)
	{
		for (int i = this.items.Count - 1; i >= 0; i--)
		{
			if (!(tag != Tag.Invalid) || this.items[i].HasTag(tag))
			{
				this.ConsumeIgnoringDisease(this.items[i]);
			}
		}
	}

	// Token: 0x060023B8 RID: 9144 RVA: 0x000C7A30 File Offset: 0x000C5C30
	public void ConsumeAndGetDisease(Tag tag, float amount, out float amount_consumed, out SimUtil.DiseaseInfo disease_info, out float aggregate_temperature)
	{
		DebugUtil.Assert(tag.IsValid);
		amount_consumed = 0f;
		disease_info = SimUtil.DiseaseInfo.Invalid;
		aggregate_temperature = 0f;
		bool flag = false;
		int num = 0;
		while (num < this.items.Count && amount > 0f)
		{
			GameObject gameObject = this.items[num];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.Units > 0f)
				{
					flag = true;
					float num2 = Math.Min(component.Units, amount);
					global::Debug.Assert(num2 > 0f, "Delta amount was zero, which should be impossible.");
					aggregate_temperature = SimUtil.CalculateFinalTemperature(amount_consumed, aggregate_temperature, num2, component.Temperature);
					SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(component, num2 / component.Units);
					disease_info = SimUtil.CalculateFinalDiseaseInfo(disease_info, percentOfDisease);
					component.Units -= num2;
					component.ModifyDiseaseCount(-percentOfDisease.count, "Storage.ConsumeAndGetDisease");
					amount -= num2;
					amount_consumed += num2;
				}
				if (component.Units <= 0f && !component.KeepZeroMassObject)
				{
					if (this.deleted_objects == null)
					{
						this.deleted_objects = new List<GameObject>();
					}
					this.deleted_objects.Add(gameObject);
				}
				base.Trigger(-1697596308, gameObject);
				Action<GameObject> onStorageChange = this.OnStorageChange;
				if (onStorageChange != null)
				{
					onStorageChange(gameObject);
				}
			}
			num++;
		}
		if (!flag)
		{
			aggregate_temperature = base.GetComponent<PrimaryElement>().Temperature;
		}
		if (this.deleted_objects != null)
		{
			for (int i = 0; i < this.deleted_objects.Count; i++)
			{
				this.items.Remove(this.deleted_objects[i]);
				Util.KDestroyGameObject(this.deleted_objects[i]);
			}
			this.deleted_objects.Clear();
		}
	}

	// Token: 0x060023B9 RID: 9145 RVA: 0x000C7C14 File Offset: 0x000C5E14
	public void ConsumeAndGetDisease(Recipe.Ingredient ingredient, out SimUtil.DiseaseInfo disease_info, out float temperature)
	{
		float num;
		this.ConsumeAndGetDisease(ingredient.tag, ingredient.amount, out num, out disease_info, out temperature);
	}

	// Token: 0x060023BA RID: 9146 RVA: 0x000C7C38 File Offset: 0x000C5E38
	public void ConsumeIgnoringDisease(Tag tag, float amount)
	{
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		this.ConsumeAndGetDisease(tag, amount, out num, out diseaseInfo, out num2);
	}

	// Token: 0x060023BB RID: 9147 RVA: 0x000C7C54 File Offset: 0x000C5E54
	public void ConsumeIgnoringDisease(GameObject item_go)
	{
		if (this.items.Contains(item_go))
		{
			PrimaryElement component = item_go.GetComponent<PrimaryElement>();
			if (component != null && component.KeepZeroMassObject)
			{
				component.Units = 0f;
				component.ModifyDiseaseCount(-component.DiseaseCount, "consume item");
				base.Trigger(-1697596308, item_go);
				Action<GameObject> onStorageChange = this.OnStorageChange;
				if (onStorageChange == null)
				{
					return;
				}
				onStorageChange(item_go);
				return;
			}
			else
			{
				this.items.Remove(item_go);
				base.Trigger(-1697596308, item_go);
				Action<GameObject> onStorageChange2 = this.OnStorageChange;
				if (onStorageChange2 != null)
				{
					onStorageChange2(item_go);
				}
				item_go.DeleteObject();
			}
		}
	}

	// Token: 0x060023BC RID: 9148 RVA: 0x000C7CF6 File Offset: 0x000C5EF6
	public GameObject Drop(int ID)
	{
		return this.Drop(this.Find(ID), true);
	}

	// Token: 0x060023BD RID: 9149 RVA: 0x000C7D08 File Offset: 0x000C5F08
	private void OnDeath(object data)
	{
		List<GameObject> list = new List<GameObject>();
		bool vent_gas = true;
		bool dump_liquid = true;
		List<GameObject> collect_dropped_items = list;
		this.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
		if (this.onDestroyItemsDropped != null)
		{
			this.onDestroyItemsDropped(list);
		}
	}

	// Token: 0x060023BE RID: 9150 RVA: 0x000C7D44 File Offset: 0x000C5F44
	public bool IsFull()
	{
		return this.RemainingCapacity() <= 0f;
	}

	// Token: 0x060023BF RID: 9151 RVA: 0x000C7D56 File Offset: 0x000C5F56
	public bool IsEmpty()
	{
		return this.items.Count == 0;
	}

	// Token: 0x060023C0 RID: 9152 RVA: 0x000C7D66 File Offset: 0x000C5F66
	public float Capacity()
	{
		return this.capacityKg;
	}

	// Token: 0x060023C1 RID: 9153 RVA: 0x000C7D6E File Offset: 0x000C5F6E
	public bool IsEndOfLife()
	{
		return this.endOfLife;
	}

	// Token: 0x060023C2 RID: 9154 RVA: 0x000C7D78 File Offset: 0x000C5F78
	public float ExactMassStored()
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null))
			{
				PrimaryElement component = this.items[i].GetComponent<PrimaryElement>();
				if (component != null)
				{
					num += component.Units * component.MassPerUnit;
				}
			}
		}
		return num;
	}

	// Token: 0x060023C3 RID: 9155 RVA: 0x000C7DE1 File Offset: 0x000C5FE1
	public float MassStored()
	{
		return (float)Mathf.RoundToInt(this.ExactMassStored() * 1000f) / 1000f;
	}

	// Token: 0x060023C4 RID: 9156 RVA: 0x000C7DFC File Offset: 0x000C5FFC
	public float UnitsStored()
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null))
			{
				PrimaryElement component = this.items[i].GetComponent<PrimaryElement>();
				if (component != null)
				{
					num += component.Units;
				}
			}
		}
		return (float)Mathf.RoundToInt(num * 1000f) / 1000f;
	}

	// Token: 0x060023C5 RID: 9157 RVA: 0x000C7E70 File Offset: 0x000C6070
	public bool Has(Tag tag)
	{
		bool result = false;
		foreach (GameObject gameObject in this.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.HasTag(tag) && component.Mass > 0f)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060023C6 RID: 9158 RVA: 0x000C7EEC File Offset: 0x000C60EC
	public PrimaryElement AddToPrimaryElement(SimHashes element, float additional_mass, float temperature)
	{
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, additional_mass);
			primaryElement.Mass += additional_mass;
			primaryElement.Temperature = finalTemperature;
		}
		return primaryElement;
	}

	// Token: 0x060023C7 RID: 9159 RVA: 0x000C7F34 File Offset: 0x000C6134
	public PrimaryElement FindPrimaryElement(SimHashes element)
	{
		PrimaryElement result = null;
		foreach (GameObject gameObject in this.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.ElementID == element)
				{
					result = component;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060023C8 RID: 9160 RVA: 0x000C7FA0 File Offset: 0x000C61A0
	public float RemainingCapacity()
	{
		return this.capacityKg - this.MassStored();
	}

	// Token: 0x060023C9 RID: 9161 RVA: 0x000C7FAF File Offset: 0x000C61AF
	public bool GetOnlyFetchMarkedItems()
	{
		return this.onlyFetchMarkedItems;
	}

	// Token: 0x060023CA RID: 9162 RVA: 0x000C7FB7 File Offset: 0x000C61B7
	public void SetOnlyFetchMarkedItems(bool is_set)
	{
		if (is_set != this.onlyFetchMarkedItems)
		{
			this.onlyFetchMarkedItems = is_set;
			this.UpdateFetchCategory();
			base.Trigger(644822890, null);
			base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("sweep", is_set);
		}
	}

	// Token: 0x060023CB RID: 9163 RVA: 0x000C7FF1 File Offset: 0x000C61F1
	private void UpdateFetchCategory()
	{
		if (this.fetchCategory == Storage.FetchCategory.Building)
		{
			return;
		}
		this.fetchCategory = (this.onlyFetchMarkedItems ? Storage.FetchCategory.StorageSweepOnly : Storage.FetchCategory.GeneralStorage);
	}

	// Token: 0x060023CC RID: 9164 RVA: 0x000C800E File Offset: 0x000C620E
	protected override void OnCleanUp()
	{
		if (this.items.Count != 0)
		{
			global::Debug.LogWarning("Storage for [" + base.gameObject.name + "] is being destroyed but it still contains items!", base.gameObject);
		}
		base.OnCleanUp();
	}

	// Token: 0x060023CD RID: 9165 RVA: 0x000C8048 File Offset: 0x000C6248
	private void OnQueueDestroyObject(object data)
	{
		this.endOfLife = true;
		List<GameObject> list = new List<GameObject>();
		bool vent_gas = true;
		bool dump_liquid = false;
		List<GameObject> collect_dropped_items = list;
		this.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
		if (this.onDestroyItemsDropped != null)
		{
			this.onDestroyItemsDropped(list);
		}
		this.OnCleanUp();
	}

	// Token: 0x060023CE RID: 9166 RVA: 0x000C8091 File Offset: 0x000C6291
	public void Remove(GameObject go, bool do_disease_transfer = true)
	{
		this.items.Remove(go);
		if (do_disease_transfer)
		{
			this.TransferDiseaseWithObject(go);
		}
		base.Trigger(-1697596308, go);
		Action<GameObject> onStorageChange = this.OnStorageChange;
		if (onStorageChange != null)
		{
			onStorageChange(go);
		}
		this.ApplyStoredItemModifiers(go, false, false);
	}

	// Token: 0x060023CF RID: 9167 RVA: 0x000C80D4 File Offset: 0x000C62D4
	public bool ForceStore(Tag tag, float amount)
	{
		global::Debug.Assert(amount < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT);
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				gameObject.GetComponent<PrimaryElement>().Mass += amount;
				return true;
			}
		}
		return false;
	}

	// Token: 0x060023D0 RID: 9168 RVA: 0x000C813C File Offset: 0x000C633C
	public float GetAmountAvailable(Tag tag)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				num += gameObject.GetComponent<PrimaryElement>().Units;
			}
		}
		return num;
	}

	// Token: 0x060023D1 RID: 9169 RVA: 0x000C8194 File Offset: 0x000C6394
	public float GetAmountAvailable(Tag tag, Tag[] forbiddenTags = null)
	{
		if (forbiddenTags == null)
		{
			return this.GetAmountAvailable(tag);
		}
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag) && !gameObject.HasAnyTags(forbiddenTags))
			{
				num += gameObject.GetComponent<PrimaryElement>().Units;
			}
		}
		return num;
	}

	// Token: 0x060023D2 RID: 9170 RVA: 0x000C8200 File Offset: 0x000C6400
	public float GetUnitsAvailable(Tag tag)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				num += gameObject.GetComponent<PrimaryElement>().Units;
			}
		}
		return num;
	}

	// Token: 0x060023D3 RID: 9171 RVA: 0x000C8258 File Offset: 0x000C6458
	public float GetMassAvailable(Tag tag)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				num += gameObject.GetComponent<PrimaryElement>().Mass;
			}
		}
		return num;
	}

	// Token: 0x060023D4 RID: 9172 RVA: 0x000C82B0 File Offset: 0x000C64B0
	public float GetMassAvailable(SimHashes element)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.ElementID == element)
				{
					num += component.Mass;
				}
			}
		}
		return num;
	}

	// Token: 0x060023D5 RID: 9173 RVA: 0x000C830C File Offset: 0x000C650C
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (this.showDescriptor)
		{
			descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.STORAGECAPACITY, GameUtil.GetFormattedMass(this.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.STORAGECAPACITY, GameUtil.GetFormattedMass(this.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Effect, false));
		}
		return descriptors;
	}

	// Token: 0x060023D6 RID: 9174 RVA: 0x000C837C File Offset: 0x000C657C
	public static void MakeItemTemperatureInsulated(GameObject go, bool is_stored, bool is_initializing)
	{
		SimTemperatureTransfer component = go.GetComponent<SimTemperatureTransfer>();
		if (component == null)
		{
			return;
		}
		component.enabled = !is_stored;
	}

	// Token: 0x060023D7 RID: 9175 RVA: 0x000C83A4 File Offset: 0x000C65A4
	public static void MakeItemInvisible(GameObject go, bool is_stored, bool is_initializing)
	{
		if (is_initializing)
		{
			return;
		}
		bool flag = !is_stored;
		KAnimControllerBase component = go.GetComponent<KAnimControllerBase>();
		if (component != null && component.enabled != flag)
		{
			component.enabled = flag;
		}
		KSelectable component2 = go.GetComponent<KSelectable>();
		if (component2 != null && component2.enabled != flag)
		{
			component2.enabled = flag;
		}
	}

	// Token: 0x060023D8 RID: 9176 RVA: 0x000C83FA File Offset: 0x000C65FA
	public static void MakeItemSealed(GameObject go, bool is_stored, bool is_initializing)
	{
		if (go != null)
		{
			if (is_stored)
			{
				go.GetComponent<KPrefabID>().AddTag(GameTags.Sealed, false);
				return;
			}
			go.GetComponent<KPrefabID>().RemoveTag(GameTags.Sealed);
		}
	}

	// Token: 0x060023D9 RID: 9177 RVA: 0x000C842A File Offset: 0x000C662A
	public static void MakeItemPreserved(GameObject go, bool is_stored, bool is_initializing)
	{
		if (go != null)
		{
			if (is_stored)
			{
				go.GetComponent<KPrefabID>().AddTag(GameTags.Preserved, false);
				return;
			}
			go.GetComponent<KPrefabID>().RemoveTag(GameTags.Preserved);
		}
	}

	// Token: 0x060023DA RID: 9178 RVA: 0x000C845C File Offset: 0x000C665C
	private void ApplyStoredItemModifiers(GameObject go, bool is_stored, bool is_initializing)
	{
		List<Storage.StoredItemModifier> list = this.defaultStoredItemModifers;
		for (int i = 0; i < list.Count; i++)
		{
			Storage.StoredItemModifier storedItemModifier = list[i];
			for (int j = 0; j < Storage.StoredItemModifierHandlers.Count; j++)
			{
				Storage.StoredItemModifierInfo storedItemModifierInfo = Storage.StoredItemModifierHandlers[j];
				if (storedItemModifierInfo.modifier == storedItemModifier)
				{
					storedItemModifierInfo.toggleState(go, is_stored, is_initializing);
					break;
				}
			}
		}
	}

	// Token: 0x060023DB RID: 9179 RVA: 0x000C84C8 File Offset: 0x000C66C8
	protected virtual void OnCopySettings(object data)
	{
		Storage component = ((GameObject)data).GetComponent<Storage>();
		if (component != null)
		{
			this.SetOnlyFetchMarkedItems(component.onlyFetchMarkedItems);
		}
	}

	// Token: 0x060023DC RID: 9180 RVA: 0x000C84F8 File Offset: 0x000C66F8
	private void OnPriorityChanged(PrioritySetting priority)
	{
		foreach (GameObject go in this.items)
		{
			go.Trigger(-1626373771, this);
		}
	}

	// Token: 0x060023DD RID: 9181 RVA: 0x000C8550 File Offset: 0x000C6750
	private void OnReachableChanged(object data)
	{
		bool flag = (bool)data;
		KSelectable component = base.GetComponent<KSelectable>();
		if (flag)
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.StorageUnreachable, false);
			return;
		}
		component.AddStatusItem(Db.Get().BuildingStatusItems.StorageUnreachable, this);
	}

	// Token: 0x060023DE RID: 9182 RVA: 0x000C859C File Offset: 0x000C679C
	public void SetContentsDeleteOffGrid(bool delete_off_grid)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			Pickupable component = this.items[i].GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = delete_off_grid;
			}
			Storage component2 = this.items[i].GetComponent<Storage>();
			if (component2 != null)
			{
				component2.SetContentsDeleteOffGrid(delete_off_grid);
			}
		}
	}

	// Token: 0x060023DF RID: 9183 RVA: 0x000C8604 File Offset: 0x000C6804
	private bool ShouldSaveItem(GameObject go)
	{
		if (!this.shouldSaveItems)
		{
			return false;
		}
		bool result = false;
		if (go != null && go.GetComponent<SaveLoadRoot>() != null && go.GetComponent<PrimaryElement>().Mass > 0f)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x060023E0 RID: 9184 RVA: 0x000C864C File Offset: 0x000C684C
	public void Serialize(BinaryWriter writer)
	{
		int num = 0;
		int count = this.items.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.ShouldSaveItem(this.items[i]))
			{
				num++;
			}
		}
		writer.Write(num);
		if (num == 0)
		{
			return;
		}
		if (this.items != null && this.items.Count > 0)
		{
			for (int j = 0; j < this.items.Count; j++)
			{
				GameObject gameObject = this.items[j];
				if (this.ShouldSaveItem(gameObject))
				{
					SaveLoadRoot component = gameObject.GetComponent<SaveLoadRoot>();
					if (component != null)
					{
						string name = gameObject.GetComponent<KPrefabID>().GetSaveLoadTag().Name;
						writer.WriteKleiString(name);
						component.Save(writer);
					}
					else
					{
						global::Debug.Log("Tried to save obj in storage but obj has no SaveLoadRoot", gameObject);
					}
				}
			}
		}
	}

	// Token: 0x060023E1 RID: 9185 RVA: 0x000C8728 File Offset: 0x000C6928
	public void Deserialize(IReader reader)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		this.ClearItems();
		int num4 = reader.ReadInt32();
		this.items = new List<GameObject>(num4);
		for (int i = 0; i < num4; i++)
		{
			float realtimeSinceStartup2 = Time.realtimeSinceStartup;
			Tag tag = TagManager.Create(reader.ReadKleiString());
			SaveLoadRoot saveLoadRoot = SaveLoadRoot.Load(tag, reader);
			num += Time.realtimeSinceStartup - realtimeSinceStartup2;
			if (saveLoadRoot != null)
			{
				KBatchedAnimController component = saveLoadRoot.GetComponent<KBatchedAnimController>();
				if (component != null)
				{
					component.enabled = false;
				}
				saveLoadRoot.SetRegistered(false);
				float realtimeSinceStartup3 = Time.realtimeSinceStartup;
				GameObject gameObject = this.Store(saveLoadRoot.gameObject, true, true, false, true);
				num2 += Time.realtimeSinceStartup - realtimeSinceStartup3;
				if (gameObject != null)
				{
					Pickupable component2 = gameObject.GetComponent<Pickupable>();
					if (component2 != null)
					{
						float realtimeSinceStartup4 = Time.realtimeSinceStartup;
						component2.OnStore(this);
						num3 += Time.realtimeSinceStartup - realtimeSinceStartup4;
					}
					Storable component3 = gameObject.GetComponent<Storable>();
					if (component3 != null)
					{
						float realtimeSinceStartup5 = Time.realtimeSinceStartup;
						component3.OnStore(this);
						num3 += Time.realtimeSinceStartup - realtimeSinceStartup5;
					}
					if (this.dropOnLoad)
					{
						this.Drop(saveLoadRoot.gameObject, true);
					}
				}
			}
			else
			{
				global::Debug.LogWarning("Tried to deserialize " + tag.ToString() + " into storage but failed", base.gameObject);
			}
		}
	}

	// Token: 0x060023E2 RID: 9186 RVA: 0x000C88A4 File Offset: 0x000C6AA4
	private void ClearItems()
	{
		foreach (GameObject go in this.items)
		{
			go.DeleteObject();
		}
		this.items.Clear();
	}

	// Token: 0x060023E3 RID: 9187 RVA: 0x000C8900 File Offset: 0x000C6B00
	public void UpdateStoredItemCachedCells()
	{
		foreach (GameObject gameObject in this.items)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			if (component != null)
			{
				component.UpdateCachedCellFromStoragePosition();
			}
		}
	}

	// Token: 0x04001434 RID: 5172
	public bool allowItemRemoval;

	// Token: 0x04001435 RID: 5173
	public bool ignoreSourcePriority;

	// Token: 0x04001436 RID: 5174
	public bool onlyTransferFromLowerPriority;

	// Token: 0x04001437 RID: 5175
	public float capacityKg = 20000f;

	// Token: 0x04001438 RID: 5176
	public bool showDescriptor;

	// Token: 0x0400143A RID: 5178
	public bool doDiseaseTransfer = true;

	// Token: 0x0400143B RID: 5179
	public List<Tag> storageFilters;

	// Token: 0x0400143C RID: 5180
	public bool useGunForDelivery = true;

	// Token: 0x0400143D RID: 5181
	public bool sendOnStoreOnSpawn;

	// Token: 0x0400143E RID: 5182
	public bool showInUI = true;

	// Token: 0x0400143F RID: 5183
	public bool storeDropsFromButcherables;

	// Token: 0x04001440 RID: 5184
	public bool allowClearable;

	// Token: 0x04001441 RID: 5185
	public bool showCapacityStatusItem;

	// Token: 0x04001442 RID: 5186
	public bool showCapacityAsMainStatus;

	// Token: 0x04001443 RID: 5187
	public bool showUnreachableStatus;

	// Token: 0x04001444 RID: 5188
	public bool showSideScreenTitleBar;

	// Token: 0x04001445 RID: 5189
	public bool useWideOffsets;

	// Token: 0x04001446 RID: 5190
	public Action<List<GameObject>> onDestroyItemsDropped;

	// Token: 0x04001447 RID: 5191
	public Action<GameObject> OnStorageChange;

	// Token: 0x04001448 RID: 5192
	public Vector2 dropOffset = Vector2.zero;

	// Token: 0x04001449 RID: 5193
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x0400144A RID: 5194
	public Vector2 gunTargetOffset;

	// Token: 0x0400144B RID: 5195
	public Storage.FetchCategory fetchCategory;

	// Token: 0x0400144C RID: 5196
	public int storageNetworkID = -1;

	// Token: 0x0400144D RID: 5197
	public Tag storageID = GameTags.StoragesIds.DefaultStorage;

	// Token: 0x0400144E RID: 5198
	public float storageFullMargin;

	// Token: 0x0400144F RID: 5199
	public Vector3 storageFXOffset = Vector3.zero;

	// Token: 0x04001450 RID: 5200
	private static readonly EventSystem.IntraObjectHandler<Storage> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Storage>(delegate(Storage component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x04001451 RID: 5201
	public Storage.FXPrefix fxPrefix;

	// Token: 0x04001452 RID: 5202
	public List<GameObject> items = new List<GameObject>();

	// Token: 0x04001453 RID: 5203
	[MyCmpGet]
	public Prioritizable prioritizable;

	// Token: 0x04001454 RID: 5204
	[MyCmpGet]
	public Automatable automatable;

	// Token: 0x04001455 RID: 5205
	[MyCmpGet]
	protected PrimaryElement primaryElement;

	// Token: 0x04001456 RID: 5206
	public bool dropOnLoad;

	// Token: 0x04001457 RID: 5207
	protected float maxKGPerItem = float.MaxValue;

	// Token: 0x04001458 RID: 5208
	private bool endOfLife;

	// Token: 0x04001459 RID: 5209
	public bool allowSettingOnlyFetchMarkedItems = true;

	// Token: 0x0400145A RID: 5210
	[Serialize]
	private bool onlyFetchMarkedItems;

	// Token: 0x0400145B RID: 5211
	[Serialize]
	private bool shouldSaveItems = true;

	// Token: 0x0400145C RID: 5212
	public float storageWorkTime = 1.5f;

	// Token: 0x0400145D RID: 5213
	private static readonly List<Storage.StoredItemModifierInfo> StoredItemModifierHandlers = new List<Storage.StoredItemModifierInfo>
	{
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Hide, new Action<GameObject, bool, bool>(Storage.MakeItemInvisible)),
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Insulate, new Action<GameObject, bool, bool>(Storage.MakeItemTemperatureInsulated)),
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Seal, new Action<GameObject, bool, bool>(Storage.MakeItemSealed)),
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Preserve, new Action<GameObject, bool, bool>(Storage.MakeItemPreserved))
	};

	// Token: 0x0400145E RID: 5214
	[SerializeField]
	private List<Storage.StoredItemModifier> defaultStoredItemModifers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide
	};

	// Token: 0x0400145F RID: 5215
	public static readonly List<Storage.StoredItemModifier> StandardSealedStorage = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal
	};

	// Token: 0x04001460 RID: 5216
	public static readonly List<Storage.StoredItemModifier> StandardFabricatorStorage = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve
	};

	// Token: 0x04001461 RID: 5217
	public static readonly List<Storage.StoredItemModifier> StandardInsulatedStorage = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal,
		Storage.StoredItemModifier.Insulate
	};

	// Token: 0x04001463 RID: 5219
	private static StatusItem capacityStatusItem;

	// Token: 0x04001464 RID: 5220
	private static readonly EventSystem.IntraObjectHandler<Storage> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<Storage>(GameTags.Dead, delegate(Storage component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x04001465 RID: 5221
	private static readonly EventSystem.IntraObjectHandler<Storage> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<Storage>(delegate(Storage component, object data)
	{
		component.OnQueueDestroyObject(data);
	});

	// Token: 0x04001466 RID: 5222
	private static readonly EventSystem.IntraObjectHandler<Storage> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Storage>(delegate(Storage component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001467 RID: 5223
	private List<GameObject> deleted_objects;

	// Token: 0x020013BD RID: 5053
	public enum StoredItemModifier
	{
		// Token: 0x040067C2 RID: 26562
		Insulate,
		// Token: 0x040067C3 RID: 26563
		Hide,
		// Token: 0x040067C4 RID: 26564
		Seal,
		// Token: 0x040067C5 RID: 26565
		Preserve
	}

	// Token: 0x020013BE RID: 5054
	public enum FetchCategory
	{
		// Token: 0x040067C7 RID: 26567
		Building,
		// Token: 0x040067C8 RID: 26568
		GeneralStorage,
		// Token: 0x040067C9 RID: 26569
		StorageSweepOnly
	}

	// Token: 0x020013BF RID: 5055
	public enum FXPrefix
	{
		// Token: 0x040067CB RID: 26571
		Delivered,
		// Token: 0x040067CC RID: 26572
		PickedUp
	}

	// Token: 0x020013C0 RID: 5056
	private struct StoredItemModifierInfo
	{
		// Token: 0x06008833 RID: 34867 RVA: 0x0032E0AA File Offset: 0x0032C2AA
		public StoredItemModifierInfo(Storage.StoredItemModifier modifier, Action<GameObject, bool, bool> toggle_state)
		{
			this.modifier = modifier;
			this.toggleState = toggle_state;
		}

		// Token: 0x040067CD RID: 26573
		public Storage.StoredItemModifier modifier;

		// Token: 0x040067CE RID: 26574
		public Action<GameObject, bool, bool> toggleState;
	}
}
