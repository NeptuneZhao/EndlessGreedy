using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using FMOD.Studio;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005A1 RID: 1441
[AddComponentMenu("KMonoBehaviour/Workable/Pickupable")]
public class Pickupable : Workable, IHasSortOrder
{
	// Token: 0x17000178 RID: 376
	// (get) Token: 0x060021FF RID: 8703 RVA: 0x000BCFF9 File Offset: 0x000BB1F9
	public PrimaryElement PrimaryElement
	{
		get
		{
			return this.primaryElement;
		}
	}

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x06002200 RID: 8704 RVA: 0x000BD001 File Offset: 0x000BB201
	// (set) Token: 0x06002201 RID: 8705 RVA: 0x000BD009 File Offset: 0x000BB209
	public int sortOrder
	{
		get
		{
			return this._sortOrder;
		}
		set
		{
			this._sortOrder = value;
		}
	}

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x06002202 RID: 8706 RVA: 0x000BD012 File Offset: 0x000BB212
	// (set) Token: 0x06002203 RID: 8707 RVA: 0x000BD01A File Offset: 0x000BB21A
	public Storage storage { get; set; }

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x06002204 RID: 8708 RVA: 0x000BD023 File Offset: 0x000BB223
	public float MinTakeAmount
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002205 RID: 8709 RVA: 0x000BD02A File Offset: 0x000BB22A
	public bool isChoreAllowedToPickup(ChoreType choreType)
	{
		return this.allowedChoreTypes == null || this.allowedChoreTypes.Contains(choreType);
	}

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06002206 RID: 8710 RVA: 0x000BD042 File Offset: 0x000BB242
	// (set) Token: 0x06002207 RID: 8711 RVA: 0x000BD04A File Offset: 0x000BB24A
	public bool prevent_absorb_until_stored { get; set; }

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06002208 RID: 8712 RVA: 0x000BD053 File Offset: 0x000BB253
	// (set) Token: 0x06002209 RID: 8713 RVA: 0x000BD05B File Offset: 0x000BB25B
	public bool isKinematic { get; set; }

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x0600220A RID: 8714 RVA: 0x000BD064 File Offset: 0x000BB264
	// (set) Token: 0x0600220B RID: 8715 RVA: 0x000BD06C File Offset: 0x000BB26C
	public bool wasAbsorbed { get; private set; }

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x0600220C RID: 8716 RVA: 0x000BD075 File Offset: 0x000BB275
	// (set) Token: 0x0600220D RID: 8717 RVA: 0x000BD07D File Offset: 0x000BB27D
	public int cachedCell { get; private set; }

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x0600220E RID: 8718 RVA: 0x000BD086 File Offset: 0x000BB286
	// (set) Token: 0x0600220F RID: 8719 RVA: 0x000BD090 File Offset: 0x000BB290
	public bool IsEntombed
	{
		get
		{
			return this.isEntombed;
		}
		set
		{
			if (value != this.isEntombed)
			{
				this.isEntombed = value;
				if (this.isEntombed)
				{
					base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
				}
				else
				{
					base.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
				}
				base.Trigger(-1089732772, null);
				this.UpdateEntombedVisualizer();
			}
		}
	}

	// Token: 0x06002210 RID: 8720 RVA: 0x000BD0EA File Offset: 0x000BB2EA
	private bool CouldBePickedUpCommon(GameObject carrier)
	{
		return this.UnreservedAmount >= this.MinTakeAmount && (this.UnreservedAmount > 0f || this.FindReservedAmount(carrier) > 0f);
	}

	// Token: 0x06002211 RID: 8721 RVA: 0x000BD11C File Offset: 0x000BB31C
	public bool CouldBePickedUpByMinion(GameObject carrier)
	{
		return this.CouldBePickedUpCommon(carrier) && (this.storage == null || !this.storage.automatable || !this.storage.automatable.GetAutomationOnly());
	}

	// Token: 0x06002212 RID: 8722 RVA: 0x000BD169 File Offset: 0x000BB369
	public bool CouldBePickedUpByTransferArm(GameObject carrier)
	{
		return this.CouldBePickedUpCommon(carrier) && (this.fetchable_monitor == null || this.fetchable_monitor.IsFetchable());
	}

	// Token: 0x06002213 RID: 8723 RVA: 0x000BD18C File Offset: 0x000BB38C
	public float FindReservedAmount(GameObject reserver)
	{
		for (int i = 0; i < this.reservations.Count; i++)
		{
			if (this.reservations[i].reserver == reserver)
			{
				return this.reservations[i].amount;
			}
		}
		return 0f;
	}

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x06002214 RID: 8724 RVA: 0x000BD1DF File Offset: 0x000BB3DF
	public float UnreservedAmount
	{
		get
		{
			return this.TotalAmount - this.ReservedAmount;
		}
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x06002215 RID: 8725 RVA: 0x000BD1EE File Offset: 0x000BB3EE
	// (set) Token: 0x06002216 RID: 8726 RVA: 0x000BD1F6 File Offset: 0x000BB3F6
	public float ReservedAmount { get; private set; }

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x06002217 RID: 8727 RVA: 0x000BD1FF File Offset: 0x000BB3FF
	// (set) Token: 0x06002218 RID: 8728 RVA: 0x000BD20C File Offset: 0x000BB40C
	public float TotalAmount
	{
		get
		{
			return this.primaryElement.Units;
		}
		set
		{
			DebugUtil.Assert(this.primaryElement != null);
			this.primaryElement.Units = value;
			if (value < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT && !this.primaryElement.KeepZeroMassObject)
			{
				base.gameObject.DeleteObject();
			}
			this.NotifyChanged(Grid.PosToCell(this));
		}
	}

	// Token: 0x06002219 RID: 8729 RVA: 0x000BD264 File Offset: 0x000BB464
	private void RefreshReservedAmount()
	{
		this.ReservedAmount = 0f;
		for (int i = 0; i < this.reservations.Count; i++)
		{
			this.ReservedAmount += this.reservations[i].amount;
		}
	}

	// Token: 0x0600221A RID: 8730 RVA: 0x000BD2B0 File Offset: 0x000BB4B0
	[Conditional("UNITY_EDITOR")]
	private void Log(string evt, string param, float value)
	{
	}

	// Token: 0x0600221B RID: 8731 RVA: 0x000BD2B2 File Offset: 0x000BB4B2
	public void ClearReservations()
	{
		this.reservations.Clear();
		this.RefreshReservedAmount();
	}

	// Token: 0x0600221C RID: 8732 RVA: 0x000BD2C8 File Offset: 0x000BB4C8
	[ContextMenu("Print Reservations")]
	public void PrintReservations()
	{
		foreach (Pickupable.Reservation reservation in this.reservations)
		{
			global::Debug.Log(reservation.ToString());
		}
	}

	// Token: 0x0600221D RID: 8733 RVA: 0x000BD328 File Offset: 0x000BB528
	public int Reserve(string context, GameObject reserver, float amount)
	{
		int num = this.nextTicketNumber;
		this.nextTicketNumber = num + 1;
		int num2 = num;
		Pickupable.Reservation reservation = new Pickupable.Reservation(reserver, amount, num2);
		this.reservations.Add(reservation);
		this.RefreshReservedAmount();
		if (this.OnReservationsChanged != null)
		{
			this.OnReservationsChanged(this, true, reservation);
		}
		return num2;
	}

	// Token: 0x0600221E RID: 8734 RVA: 0x000BD37C File Offset: 0x000BB57C
	public void Unreserve(string context, int ticket)
	{
		int i = 0;
		while (i < this.reservations.Count)
		{
			if (this.reservations[i].ticket == ticket)
			{
				Pickupable.Reservation arg = this.reservations[i];
				this.reservations.RemoveAt(i);
				this.RefreshReservedAmount();
				if (this.OnReservationsChanged != null)
				{
					this.OnReservationsChanged(this, false, arg);
					return;
				}
				break;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x0600221F RID: 8735 RVA: 0x000BD3EC File Offset: 0x000BB5EC
	private Pickupable()
	{
		this.showProgressBar = false;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.shouldTransferDiseaseWithWorker = false;
	}

	// Token: 0x06002220 RID: 8736 RVA: 0x000BD46C File Offset: 0x000BB66C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.log = new LoggerFSSF("Pickupable");
		this.workerStatusItem = Db.Get().DuplicantStatusItems.PickingUp;
		base.SetWorkTime(1.5f);
		this.targetWorkable = this;
		this.resetProgressOnStop = true;
		base.gameObject.layer = Game.PickupableLayer;
		Vector3 position = base.transform.GetPosition();
		this.UpdateCachedCell(Grid.PosToCell(position));
		base.Subscribe<Pickupable>(856640610, Pickupable.OnStoreDelegate);
		base.Subscribe<Pickupable>(1188683690, Pickupable.OnLandedDelegate);
		base.Subscribe<Pickupable>(1807976145, Pickupable.OnOreSizeChangedDelegate);
		base.Subscribe<Pickupable>(-1432940121, Pickupable.OnReachableChangedDelegate);
		base.Subscribe<Pickupable>(-778359855, Pickupable.RefreshStorageTagsDelegate);
		base.Subscribe<Pickupable>(580035959, Pickupable.OnWorkableEntombOffset);
		this.KPrefabID.AddTag(GameTags.Pickupable, false);
		Components.Pickupables.Add(this);
	}

	// Token: 0x06002221 RID: 8737 RVA: 0x000BD575 File Offset: 0x000BB775
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
	}

	// Token: 0x06002222 RID: 8738 RVA: 0x000BD580 File Offset: 0x000BB780
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int num = Grid.PosToCell(this);
		if (!Grid.IsValidCell(num) && this.deleteOffGrid)
		{
			base.gameObject.DeleteObject();
			return;
		}
		if (base.GetComponent<Health>() != null)
		{
			this.handleFallerComponents = false;
		}
		this.UpdateCachedCell(num);
		new ReachabilityMonitor.Instance(this).StartSM();
		this.fetchable_monitor = new FetchableMonitor.Instance(this);
		this.fetchable_monitor.StartSM();
		base.SetWorkTime(1.5f);
		this.faceTargetWhenWorking = true;
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.SetStatusIndicatorOffset(new Vector3(0f, -0.65f, 0f));
		}
		this.OnTagsChanged(null);
		this.TryToOffsetIfBuried(CellOffset.none);
		DecorProvider component2 = base.GetComponent<DecorProvider>();
		if (component2 != null && string.IsNullOrEmpty(component2.overrideName))
		{
			component2.overrideName = UI.OVERLAYS.DECOR.CLUTTER;
		}
		this.UpdateEntombedVisualizer();
		base.Subscribe<Pickupable>(-1582839653, Pickupable.OnTagsChangedDelegate);
		this.NotifyChanged(num);
	}

	// Token: 0x06002223 RID: 8739 RVA: 0x000BD690 File Offset: 0x000BB890
	[OnDeserialized]
	public void OnDeserialize()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 28) && base.transform.position.z == 0f)
		{
			KBatchedAnimController component = base.transform.GetComponent<KBatchedAnimController>();
			component.SetSceneLayer(component.sceneLayer);
		}
	}

	// Token: 0x06002224 RID: 8740 RVA: 0x000BD6E4 File Offset: 0x000BB8E4
	public void UpdateListeners(bool worldSpace)
	{
		if (this.cleaningUp)
		{
			return;
		}
		int num = Grid.PosToCell(this);
		if (worldSpace)
		{
			if (this.solidPartitionerEntry.IsValid())
			{
				return;
			}
			GameScenePartitioner.Instance.Free(ref this.storedPartitionerEntry);
			this.objectLayerListItem = new ObjectLayerListItem(base.gameObject, ObjectLayer.Pickupables, num);
			this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterSolidListener", base.gameObject, num, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
			this.worldPartitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterPickupable", this, num, GameScenePartitioner.Instance.pickupablesLayer, null);
			Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "Pickupable.OnCellChange");
			Singleton<CellChangeMonitor>.Instance.MarkDirty(base.transform);
			Singleton<CellChangeMonitor>.Instance.ClearLastKnownCell(base.transform);
			return;
		}
		else
		{
			if (this.storedPartitionerEntry.IsValid())
			{
				return;
			}
			this.storedPartitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterStoredPickupable", this, num, GameScenePartitioner.Instance.storedPickupablesLayer, null);
			if (this.objectLayerListItem != null)
			{
				this.objectLayerListItem.Clear();
				this.objectLayerListItem = null;
			}
			GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
			GameScenePartitioner.Instance.Free(ref this.worldPartitionerEntry);
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
			return;
		}
	}

	// Token: 0x06002225 RID: 8741 RVA: 0x000BD855 File Offset: 0x000BBA55
	public void RegisterListeners()
	{
		this.UpdateListeners(true);
	}

	// Token: 0x06002226 RID: 8742 RVA: 0x000BD860 File Offset: 0x000BBA60
	public void UnregisterListeners()
	{
		if (this.objectLayerListItem != null)
		{
			this.objectLayerListItem.Clear();
			this.objectLayerListItem = null;
		}
		GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.worldPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.storedPartitionerEntry);
		base.Unsubscribe<Pickupable>(856640610, Pickupable.OnStoreDelegate, false);
		base.Unsubscribe<Pickupable>(1188683690, Pickupable.OnLandedDelegate, false);
		base.Unsubscribe<Pickupable>(1807976145, Pickupable.OnOreSizeChangedDelegate, false);
		base.Unsubscribe<Pickupable>(-1432940121, Pickupable.OnReachableChangedDelegate, false);
		base.Unsubscribe<Pickupable>(-778359855, Pickupable.RefreshStorageTagsDelegate, false);
		base.Unsubscribe<Pickupable>(580035959, Pickupable.OnWorkableEntombOffset, false);
		if (base.isSpawned)
		{
			base.Unsubscribe<Pickupable>(-1582839653, Pickupable.OnTagsChangedDelegate, false);
		}
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
	}

	// Token: 0x06002227 RID: 8743 RVA: 0x000BD952 File Offset: 0x000BBB52
	private void OnSolidChanged(object data)
	{
		this.TryToOffsetIfBuried(CellOffset.none);
	}

	// Token: 0x06002228 RID: 8744 RVA: 0x000BD960 File Offset: 0x000BBB60
	private void SetWorkableOffset(object data)
	{
		CellOffset offset = CellOffset.none;
		WorkerBase workerBase = data as WorkerBase;
		if (workerBase != null)
		{
			int num = Grid.PosToCell(workerBase);
			int base_cell = Grid.PosToCell(this);
			offset = (Grid.IsValidCell(num) ? Grid.GetCellOffsetDirection(base_cell, num) : CellOffset.none);
		}
		this.TryToOffsetIfBuried(offset);
	}

	// Token: 0x06002229 RID: 8745 RVA: 0x000BD9B0 File Offset: 0x000BBBB0
	private CellOffset[] GetPreferedOffsets(CellOffset preferedDirectionOffset)
	{
		if (preferedDirectionOffset == CellOffset.left || preferedDirectionOffset == CellOffset.leftup)
		{
			return new CellOffset[]
			{
				CellOffset.up,
				CellOffset.left,
				CellOffset.leftup
			};
		}
		if (preferedDirectionOffset == CellOffset.right || preferedDirectionOffset == CellOffset.rightup)
		{
			return new CellOffset[]
			{
				CellOffset.up,
				CellOffset.right,
				CellOffset.rightup
			};
		}
		if (preferedDirectionOffset == CellOffset.up)
		{
			return new CellOffset[]
			{
				CellOffset.up,
				CellOffset.rightup,
				CellOffset.leftup
			};
		}
		if (preferedDirectionOffset == CellOffset.leftdown)
		{
			return new CellOffset[]
			{
				CellOffset.down,
				CellOffset.leftdown,
				CellOffset.left
			};
		}
		if (preferedDirectionOffset == CellOffset.rightdown)
		{
			return new CellOffset[]
			{
				CellOffset.down,
				CellOffset.rightdown,
				CellOffset.right
			};
		}
		if (preferedDirectionOffset == CellOffset.down)
		{
			return new CellOffset[]
			{
				CellOffset.down,
				CellOffset.leftdown,
				CellOffset.rightdown
			};
		}
		return new CellOffset[0];
	}

	// Token: 0x0600222A RID: 8746 RVA: 0x000BDB30 File Offset: 0x000BBD30
	public void TryToOffsetIfBuried(CellOffset offset)
	{
		if (this.KPrefabID.HasTag(GameTags.Stored) || this.KPrefabID.HasTag(GameTags.Equipped))
		{
			return;
		}
		int num = Grid.PosToCell(this);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		DeathMonitor.Instance smi = base.gameObject.GetSMI<DeathMonitor.Instance>();
		if ((smi == null || smi.IsDead()) && ((Grid.Solid[num] && Grid.Foundation[num]) || Grid.Properties[num] != 0))
		{
			CellOffset[] array = this.GetPreferedOffsets(offset).Concat(Pickupable.displacementOffsets);
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = Grid.OffsetCell(num, array[i]);
				if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
				{
					Vector3 position = Grid.CellToPosCBC(num2, Grid.SceneLayer.Move);
					KCollider2D component = base.GetComponent<KCollider2D>();
					if (component != null)
					{
						position.y += base.transform.GetPosition().y - component.bounds.min.y;
					}
					base.transform.SetPosition(position);
					num = num2;
					this.RemoveFaller();
					this.AddFaller(Vector2.zero);
					break;
				}
			}
		}
		this.HandleSolidCell(num);
	}

	// Token: 0x0600222B RID: 8747 RVA: 0x000BDC84 File Offset: 0x000BBE84
	private bool HandleSolidCell(int cell)
	{
		bool flag = this.IsEntombed;
		bool flag2 = false;
		if (Grid.IsValidCell(cell) && Grid.Solid[cell])
		{
			DeathMonitor.Instance smi = base.gameObject.GetSMI<DeathMonitor.Instance>();
			if (smi == null || smi.IsDead())
			{
				this.Clearable.CancelClearing();
				flag2 = true;
			}
		}
		if (flag2 != flag && !this.KPrefabID.HasTag(GameTags.Stored))
		{
			this.IsEntombed = flag2;
			base.GetComponent<KSelectable>().IsSelectable = !this.IsEntombed;
		}
		this.UpdateEntombedVisualizer();
		return this.IsEntombed;
	}

	// Token: 0x0600222C RID: 8748 RVA: 0x000BDD14 File Offset: 0x000BBF14
	private void OnCellChange()
	{
		Vector3 position = base.transform.GetPosition();
		int num = Grid.PosToCell(position);
		if (!Grid.IsValidCell(num))
		{
			Vector2 vector = new Vector2(-0.1f * (float)Grid.WidthInCells, 1.1f * (float)Grid.WidthInCells);
			Vector2 vector2 = new Vector2(-0.1f * (float)Grid.HeightInCells, 1.1f * (float)Grid.HeightInCells);
			if (this.deleteOffGrid && (position.x < vector.x || vector.y < position.x || position.y < vector2.x || vector2.y < position.y))
			{
				this.DeleteObject();
				return;
			}
		}
		else
		{
			this.ReleaseEntombedVisualizerAndAddFaller(true);
			if (this.HandleSolidCell(num))
			{
				return;
			}
			this.objectLayerListItem.Update(num);
			bool flag = false;
			if (this.absorbable && !this.KPrefabID.HasTag(GameTags.Stored))
			{
				int num2 = Grid.CellBelow(num);
				if (Grid.IsValidCell(num2) && Grid.Solid[num2])
				{
					ObjectLayerListItem nextItem = this.objectLayerListItem.nextItem;
					while (nextItem != null)
					{
						GameObject gameObject = nextItem.gameObject;
						nextItem = nextItem.nextItem;
						Pickupable component = gameObject.GetComponent<Pickupable>();
						if (component != null)
						{
							flag = component.TryAbsorb(this, false, false);
							if (flag)
							{
								break;
							}
						}
					}
				}
			}
			GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, num);
			GameScenePartitioner.Instance.UpdatePosition(this.worldPartitionerEntry, num);
			int cachedCell = this.cachedCell;
			this.UpdateCachedCell(num);
			if (!flag)
			{
				this.NotifyChanged(num);
			}
			if (Grid.IsValidCell(cachedCell) && num != cachedCell)
			{
				this.NotifyChanged(cachedCell);
			}
		}
	}

	// Token: 0x0600222D RID: 8749 RVA: 0x000BDEC0 File Offset: 0x000BC0C0
	private void OnTagsChanged(object data)
	{
		if (!this.KPrefabID.HasTag(GameTags.Stored) && !this.KPrefabID.HasTag(GameTags.Equipped))
		{
			this.UpdateListeners(true);
			this.AddFaller(Vector2.zero);
			return;
		}
		this.UpdateListeners(false);
		this.RemoveFaller();
	}

	// Token: 0x0600222E RID: 8750 RVA: 0x000BDF11 File Offset: 0x000BC111
	private void NotifyChanged(int new_cell)
	{
		GameScenePartitioner.Instance.TriggerEvent(new_cell, GameScenePartitioner.Instance.pickupablesChangedLayer, this);
	}

	// Token: 0x0600222F RID: 8751 RVA: 0x000BDF2C File Offset: 0x000BC12C
	public bool TryAbsorb(Pickupable other, bool hide_effects, bool allow_cross_storage = false)
	{
		if (other == null)
		{
			return false;
		}
		if (other.wasAbsorbed)
		{
			return false;
		}
		if (this.wasAbsorbed)
		{
			return false;
		}
		if (!other.CanAbsorb(this))
		{
			return false;
		}
		if (this.prevent_absorb_until_stored)
		{
			return false;
		}
		if (!allow_cross_storage && this.storage == null != (other.storage == null))
		{
			return false;
		}
		this.Absorb(other);
		if (!hide_effects && EffectPrefabs.Instance != null && !this.storage)
		{
			Vector3 position = base.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
			global::Util.KInstantiate(Assets.GetPrefab(EffectConfigs.OreAbsorbId), position, Quaternion.identity, null, null, true, 0).SetActive(true);
		}
		return true;
	}

	// Token: 0x06002230 RID: 8752 RVA: 0x000BDFF4 File Offset: 0x000BC1F4
	protected override void OnCleanUp()
	{
		this.cleaningUp = true;
		this.ReleaseEntombedVisualizerAndAddFaller(false);
		this.RemoveFaller();
		if (this.storage)
		{
			this.storage.Remove(base.gameObject, true);
		}
		this.UnregisterListeners();
		this.fetchable_monitor = null;
		Components.Pickupables.Remove(this);
		if (this.reservations.Count > 0)
		{
			Pickupable.Reservation[] array = this.reservations.ToArray();
			this.reservations.Clear();
			if (this.OnReservationsChanged != null)
			{
				foreach (Pickupable.Reservation arg in array)
				{
					this.OnReservationsChanged(this, false, arg);
				}
			}
		}
		if (Grid.IsValidCell(this.cachedCell))
		{
			this.NotifyChanged(this.cachedCell);
		}
		base.OnCleanUp();
	}

	// Token: 0x06002231 RID: 8753 RVA: 0x000BE0C0 File Offset: 0x000BC2C0
	public Pickupable Take(float amount)
	{
		if (amount <= 0f)
		{
			return null;
		}
		if (this.OnTake == null)
		{
			if (this.storage != null)
			{
				this.storage.Remove(base.gameObject, true);
			}
			return this;
		}
		if (amount >= this.TotalAmount && this.storage != null && !this.primaryElement.KeepZeroMassObject)
		{
			this.storage.Remove(base.gameObject, true);
		}
		float num = Math.Min(this.TotalAmount, amount);
		if (num <= 0f)
		{
			return null;
		}
		return this.OnTake(this, num);
	}

	// Token: 0x06002232 RID: 8754 RVA: 0x000BE15C File Offset: 0x000BC35C
	private void Absorb(Pickupable pickupable)
	{
		global::Debug.Assert(!this.wasAbsorbed);
		global::Debug.Assert(!pickupable.wasAbsorbed);
		base.Trigger(-2064133523, pickupable);
		pickupable.Trigger(-1940207677, base.gameObject);
		pickupable.wasAbsorbed = true;
		KSelectable component = base.GetComponent<KSelectable>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == pickupable.GetComponent<KSelectable>())
		{
			SelectTool.Instance.Select(component, false);
		}
		pickupable.gameObject.DeleteObject();
		this.NotifyChanged(Grid.PosToCell(this));
	}

	// Token: 0x06002233 RID: 8755 RVA: 0x000BE20C File Offset: 0x000BC40C
	private void RefreshStorageTags(object data = null)
	{
		bool flag = data is Storage || (data != null && (bool)data);
		if (flag && data is Storage && ((Storage)data).gameObject == base.gameObject)
		{
			return;
		}
		if (!flag)
		{
			this.KPrefabID.RemoveTag(GameTags.Stored);
			this.KPrefabID.RemoveTag(GameTags.StoredPrivate);
			return;
		}
		this.KPrefabID.AddTag(GameTags.Stored, false);
		if (this.storage == null || !this.storage.allowItemRemoval)
		{
			this.KPrefabID.AddTag(GameTags.StoredPrivate, false);
			return;
		}
		this.KPrefabID.RemoveTag(GameTags.StoredPrivate);
	}

	// Token: 0x06002234 RID: 8756 RVA: 0x000BE2C8 File Offset: 0x000BC4C8
	public void OnStore(object data)
	{
		this.storage = (data as Storage);
		bool flag = data is Storage || (data != null && (bool)data);
		SaveLoadRoot component = base.GetComponent<SaveLoadRoot>();
		if (this.carryAnimOverride != null && this.lastCarrier != null)
		{
			this.lastCarrier.RemoveAnimOverrides(this.carryAnimOverride);
			this.lastCarrier = null;
		}
		KSelectable component2 = base.GetComponent<KSelectable>();
		if (component2)
		{
			component2.IsSelectable = !flag;
		}
		if (flag)
		{
			int cachedCell = this.cachedCell;
			this.RefreshStorageTags(data);
			this.RemoveFaller();
			if (this.storage != null)
			{
				if (this.carryAnimOverride != null && this.storage.GetComponent<Navigator>() != null)
				{
					this.lastCarrier = this.storage.GetComponent<KBatchedAnimController>();
					if (this.lastCarrier != null && this.lastCarrier.HasTag(GameTags.BaseMinion))
					{
						this.lastCarrier.AddAnimOverrides(this.carryAnimOverride, 0f);
					}
				}
				this.UpdateCachedCell(Grid.PosToCell(this.storage));
			}
			this.NotifyChanged(cachedCell);
			if (component != null)
			{
				component.SetRegistered(false);
				return;
			}
		}
		else
		{
			if (component != null)
			{
				component.SetRegistered(true);
			}
			this.RemovedFromStorage();
		}
	}

	// Token: 0x06002235 RID: 8757 RVA: 0x000BE418 File Offset: 0x000BC618
	private void RemovedFromStorage()
	{
		this.storage = null;
		this.UpdateCachedCell(Grid.PosToCell(this));
		this.RefreshStorageTags(null);
		this.AddFaller(Vector2.zero);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.enabled = true;
		base.gameObject.transform.rotation = Quaternion.identity;
		this.UpdateListeners(true);
		component.GetBatchInstanceData().ClearOverrideTransformMatrix();
	}

	// Token: 0x06002236 RID: 8758 RVA: 0x000BE47D File Offset: 0x000BC67D
	public void UpdateCachedCellFromStoragePosition()
	{
		global::Debug.Assert(this.storage != null, "Only call UpdateCachedCellFromStoragePosition on pickupables in storage!");
		this.UpdateCachedCell(Grid.PosToCell(this.storage));
	}

	// Token: 0x06002237 RID: 8759 RVA: 0x000BE4A8 File Offset: 0x000BC6A8
	public void UpdateCachedCell(int cell)
	{
		if (this.cachedCell != cell && this.storedPartitionerEntry.IsValid())
		{
			GameScenePartitioner.Instance.UpdatePosition(this.storedPartitionerEntry, cell);
		}
		this.cachedCell = cell;
		this.GetOffsets(this.cachedCell);
		if (this.KPrefabID.HasTag(GameTags.PickupableStorage))
		{
			base.GetComponent<Storage>().UpdateStoredItemCachedCells();
		}
	}

	// Token: 0x06002238 RID: 8760 RVA: 0x000BE50D File Offset: 0x000BC70D
	public override int GetCell()
	{
		return this.cachedCell;
	}

	// Token: 0x06002239 RID: 8761 RVA: 0x000BE518 File Offset: 0x000BC718
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		if (this.useGunforPickup && worker.UsesMultiTool())
		{
			Workable.AnimInfo anim = base.GetAnim(worker);
			anim.smi = new MultitoolController.Instance(this, worker, "pickup", Assets.GetPrefab(EffectConfigs.OreAbsorbId));
			return anim;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x0600223A RID: 8762 RVA: 0x000BE570 File Offset: 0x000BC770
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = worker.GetComponent<Storage>();
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		float amount = pickupableStartWorkInfo.amount;
		if (!(this != null))
		{
			pickupableStartWorkInfo.setResultCb(null);
			return;
		}
		Pickupable pickupable = this.Take(amount);
		if (pickupable != null)
		{
			component.Store(pickupable.gameObject, false, false, true, false);
			worker.SetWorkCompleteData(pickupable);
			pickupableStartWorkInfo.setResultCb(pickupable.gameObject);
			return;
		}
		pickupableStartWorkInfo.setResultCb(null);
	}

	// Token: 0x0600223B RID: 8763 RVA: 0x000BE5FB File Offset: 0x000BC7FB
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x0600223C RID: 8764 RVA: 0x000BE5FE File Offset: 0x000BC7FE
	public override Vector3 GetTargetPoint()
	{
		return base.transform.GetPosition();
	}

	// Token: 0x0600223D RID: 8765 RVA: 0x000BE60B File Offset: 0x000BC80B
	public bool IsReachable()
	{
		return this.isReachable;
	}

	// Token: 0x0600223E RID: 8766 RVA: 0x000BE614 File Offset: 0x000BC814
	private void OnReachableChanged(object data)
	{
		this.isReachable = (bool)data;
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.isReachable)
		{
			component.RemoveStatusItem(Db.Get().MiscStatusItems.PickupableUnreachable, false);
			return;
		}
		component.AddStatusItem(Db.Get().MiscStatusItems.PickupableUnreachable, this);
	}

	// Token: 0x0600223F RID: 8767 RVA: 0x000BE66B File Offset: 0x000BC86B
	private void AddFaller(Vector2 initial_velocity)
	{
		if (!this.handleFallerComponents)
		{
			return;
		}
		if (!GameComps.Fallers.Has(base.gameObject))
		{
			GameComps.Fallers.Add(base.gameObject, initial_velocity);
		}
	}

	// Token: 0x06002240 RID: 8768 RVA: 0x000BE69A File Offset: 0x000BC89A
	private void RemoveFaller()
	{
		if (!this.handleFallerComponents)
		{
			return;
		}
		if (GameComps.Fallers.Has(base.gameObject))
		{
			GameComps.Fallers.Remove(base.gameObject);
		}
	}

	// Token: 0x06002241 RID: 8769 RVA: 0x000BE6C8 File Offset: 0x000BC8C8
	private void OnOreSizeChanged(object data)
	{
		Vector3 v = Vector3.zero;
		HandleVector<int>.Handle handle = GameComps.Gravities.GetHandle(base.gameObject);
		if (handle.IsValid())
		{
			v = GameComps.Gravities.GetData(handle).velocity;
		}
		this.RemoveFaller();
		if (!this.KPrefabID.HasTag(GameTags.Stored))
		{
			this.AddFaller(v);
		}
	}

	// Token: 0x06002242 RID: 8770 RVA: 0x000BE730 File Offset: 0x000BC930
	private void OnLanded(object data)
	{
		if (CameraController.Instance == null)
		{
			return;
		}
		Vector3 position = base.transform.GetPosition();
		Vector2I vector2I = Grid.PosToXY(position);
		if (vector2I.x < 0 || Grid.WidthInCells <= vector2I.x || vector2I.y < 0 || Grid.HeightInCells <= vector2I.y)
		{
			this.DeleteObject();
			return;
		}
		Vector2 vector = (Vector2)data;
		if (vector.sqrMagnitude <= 0.2f || SpeedControlScreen.Instance.IsPaused)
		{
			return;
		}
		Element element = this.primaryElement.Element;
		if (element.substance != null)
		{
			string text = element.substance.GetOreBumpSound();
			if (text == null)
			{
				if (element.HasTag(GameTags.RefinedMetal))
				{
					text = "RefinedMetal";
				}
				else if (element.HasTag(GameTags.Metal))
				{
					text = "RawMetal";
				}
				else
				{
					text = "Rock";
				}
			}
			if (element.tag.ToString() == "Creature" && !base.gameObject.HasTag(GameTags.Seed))
			{
				text = "Bodyfall_rock";
			}
			else
			{
				text = "Ore_bump_" + text;
			}
			string text2 = GlobalAssets.GetSound(text, true);
			text2 = ((text2 != null) ? text2 : GlobalAssets.GetSound("Ore_bump_rock", false));
			if (CameraController.Instance.IsAudibleSound(base.transform.GetPosition(), text2))
			{
				int num = Grid.PosToCell(position);
				bool isLiquid = Grid.Element[num].IsLiquid;
				float value = 0f;
				if (isLiquid)
				{
					value = SoundUtil.GetLiquidDepth(num);
				}
				FMOD.Studio.EventInstance instance = KFMOD.BeginOneShot(text2, CameraController.Instance.GetVerticallyScaledPosition(base.transform.GetPosition(), false), 1f);
				instance.setParameterByName("velocity", vector.magnitude, false);
				instance.setParameterByName("liquidDepth", value, false);
				KFMOD.EndOneShot(instance);
			}
		}
	}

	// Token: 0x06002243 RID: 8771 RVA: 0x000BE90C File Offset: 0x000BCB0C
	private void UpdateEntombedVisualizer()
	{
		if (this.IsEntombed)
		{
			if (this.entombedCell == -1)
			{
				int cell = Grid.PosToCell(this);
				if (EntombedItemManager.CanEntomb(this))
				{
					SaveGame.Instance.entombedItemManager.Add(this);
				}
				if (Grid.Objects[cell, 1] == null)
				{
					KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
					if (component != null && Game.Instance.GetComponent<EntombedItemVisualizer>().AddItem(cell))
					{
						this.entombedCell = cell;
						component.enabled = false;
						this.RemoveFaller();
						return;
					}
				}
			}
		}
		else
		{
			this.ReleaseEntombedVisualizerAndAddFaller(true);
		}
	}

	// Token: 0x06002244 RID: 8772 RVA: 0x000BE99C File Offset: 0x000BCB9C
	private void ReleaseEntombedVisualizerAndAddFaller(bool add_faller_if_necessary)
	{
		if (this.entombedCell != -1)
		{
			Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.entombedCell);
			this.entombedCell = -1;
			base.GetComponent<KBatchedAnimController>().enabled = true;
			if (add_faller_if_necessary)
			{
				this.AddFaller(Vector2.zero);
			}
		}
	}

	// Token: 0x0400131A RID: 4890
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x0400131B RID: 4891
	public const float WorkTime = 1.5f;

	// Token: 0x0400131C RID: 4892
	[SerializeField]
	private int _sortOrder;

	// Token: 0x0400131E RID: 4894
	[MyCmpReq]
	[NonSerialized]
	public KPrefabID KPrefabID;

	// Token: 0x0400131F RID: 4895
	[MyCmpAdd]
	[NonSerialized]
	public Clearable Clearable;

	// Token: 0x04001320 RID: 4896
	[MyCmpAdd]
	[NonSerialized]
	public Prioritizable prioritizable;

	// Token: 0x04001321 RID: 4897
	[SerializeField]
	public List<ChoreType> allowedChoreTypes;

	// Token: 0x04001322 RID: 4898
	public bool absorbable;

	// Token: 0x04001324 RID: 4900
	public Func<Pickupable, bool> CanAbsorb = (Pickupable other) => false;

	// Token: 0x04001325 RID: 4901
	public Func<Pickupable, float, Pickupable> OnTake;

	// Token: 0x04001326 RID: 4902
	public Action<Pickupable, bool, Pickupable.Reservation> OnReservationsChanged;

	// Token: 0x04001327 RID: 4903
	public ObjectLayerListItem objectLayerListItem;

	// Token: 0x04001328 RID: 4904
	public Workable targetWorkable;

	// Token: 0x04001329 RID: 4905
	public KAnimFile carryAnimOverride;

	// Token: 0x0400132A RID: 4906
	private KBatchedAnimController lastCarrier;

	// Token: 0x0400132B RID: 4907
	public bool useGunforPickup = true;

	// Token: 0x0400132D RID: 4909
	private static CellOffset[] displacementOffsets = new CellOffset[]
	{
		new CellOffset(0, 1),
		new CellOffset(0, -1),
		new CellOffset(1, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 1),
		new CellOffset(1, -1),
		new CellOffset(-1, 1),
		new CellOffset(-1, -1)
	};

	// Token: 0x0400132E RID: 4910
	private bool isReachable;

	// Token: 0x0400132F RID: 4911
	private bool isEntombed;

	// Token: 0x04001330 RID: 4912
	private bool cleaningUp;

	// Token: 0x04001332 RID: 4914
	public bool trackOnPickup = true;

	// Token: 0x04001334 RID: 4916
	private int nextTicketNumber;

	// Token: 0x04001335 RID: 4917
	[Serialize]
	public bool deleteOffGrid = true;

	// Token: 0x04001336 RID: 4918
	private List<Pickupable.Reservation> reservations = new List<Pickupable.Reservation>();

	// Token: 0x04001337 RID: 4919
	private HandleVector<int>.Handle solidPartitionerEntry;

	// Token: 0x04001338 RID: 4920
	private HandleVector<int>.Handle worldPartitionerEntry;

	// Token: 0x04001339 RID: 4921
	private HandleVector<int>.Handle storedPartitionerEntry;

	// Token: 0x0400133A RID: 4922
	private FetchableMonitor.Instance fetchable_monitor;

	// Token: 0x0400133B RID: 4923
	public bool handleFallerComponents = true;

	// Token: 0x0400133C RID: 4924
	private LoggerFSSF log;

	// Token: 0x0400133E RID: 4926
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x0400133F RID: 4927
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnLandedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnLanded(data);
	});

	// Token: 0x04001340 RID: 4928
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnOreSizeChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnOreSizeChanged(data);
	});

	// Token: 0x04001341 RID: 4929
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x04001342 RID: 4930
	private static readonly EventSystem.IntraObjectHandler<Pickupable> RefreshStorageTagsDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.RefreshStorageTags(data);
	});

	// Token: 0x04001343 RID: 4931
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnWorkableEntombOffset = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.SetWorkableOffset(data);
	});

	// Token: 0x04001344 RID: 4932
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnTagsChanged(data);
	});

	// Token: 0x04001345 RID: 4933
	private int entombedCell = -1;

	// Token: 0x02001391 RID: 5009
	public struct Reservation
	{
		// Token: 0x06008786 RID: 34694 RVA: 0x0032BF1D File Offset: 0x0032A11D
		public Reservation(GameObject reserver, float amount, int ticket)
		{
			this.reserver = reserver;
			this.amount = amount;
			this.ticket = ticket;
		}

		// Token: 0x06008787 RID: 34695 RVA: 0x0032BF34 File Offset: 0x0032A134
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.reserver.name,
				", ",
				this.amount.ToString(),
				", ",
				this.ticket.ToString()
			});
		}

		// Token: 0x04006704 RID: 26372
		public GameObject reserver;

		// Token: 0x04006705 RID: 26373
		public float amount;

		// Token: 0x04006706 RID: 26374
		public int ticket;
	}

	// Token: 0x02001392 RID: 5010
	public class PickupableStartWorkInfo : WorkerBase.StartWorkInfo
	{
		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06008788 RID: 34696 RVA: 0x0032BF86 File Offset: 0x0032A186
		// (set) Token: 0x06008789 RID: 34697 RVA: 0x0032BF8E File Offset: 0x0032A18E
		public float amount { get; private set; }

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x0600878A RID: 34698 RVA: 0x0032BF97 File Offset: 0x0032A197
		// (set) Token: 0x0600878B RID: 34699 RVA: 0x0032BF9F File Offset: 0x0032A19F
		public Pickupable originalPickupable { get; private set; }

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x0600878C RID: 34700 RVA: 0x0032BFA8 File Offset: 0x0032A1A8
		// (set) Token: 0x0600878D RID: 34701 RVA: 0x0032BFB0 File Offset: 0x0032A1B0
		public Action<GameObject> setResultCb { get; private set; }

		// Token: 0x0600878E RID: 34702 RVA: 0x0032BFB9 File Offset: 0x0032A1B9
		public PickupableStartWorkInfo(Pickupable pickupable, float amount, Action<GameObject> set_result_cb) : base(pickupable.targetWorkable)
		{
			this.originalPickupable = pickupable;
			this.amount = amount;
			this.setResultCb = set_result_cb;
		}
	}
}
