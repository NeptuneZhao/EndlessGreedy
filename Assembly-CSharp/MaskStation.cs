using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000725 RID: 1829
public class MaskStation : StateMachineComponent<MaskStation.SMInstance>, IBasicBuilding
{
	// Token: 0x17000311 RID: 785
	// (get) Token: 0x06003074 RID: 12404 RVA: 0x0010BA1A File Offset: 0x00109C1A
	// (set) Token: 0x06003075 RID: 12405 RVA: 0x0010BA27 File Offset: 0x00109C27
	private bool isRotated
	{
		get
		{
			return (this.gridFlags & Grid.SuitMarker.Flags.Rotated) > (Grid.SuitMarker.Flags)0;
		}
		set
		{
			this.UpdateGridFlag(Grid.SuitMarker.Flags.Rotated, value);
		}
	}

	// Token: 0x17000312 RID: 786
	// (get) Token: 0x06003076 RID: 12406 RVA: 0x0010BA31 File Offset: 0x00109C31
	// (set) Token: 0x06003077 RID: 12407 RVA: 0x0010BA3E File Offset: 0x00109C3E
	private bool isOperational
	{
		get
		{
			return (this.gridFlags & Grid.SuitMarker.Flags.Operational) > (Grid.SuitMarker.Flags)0;
		}
		set
		{
			this.UpdateGridFlag(Grid.SuitMarker.Flags.Operational, value);
		}
	}

	// Token: 0x06003078 RID: 12408 RVA: 0x0010BA48 File Offset: 0x00109C48
	public void UpdateOperational()
	{
		bool flag = this.GetTotalOxygenAmount() >= this.oxygenConsumedPerMask * (float)this.maxUses;
		this.shouldPump = this.IsPumpable();
		if (this.operational.IsOperational && this.shouldPump && !flag)
		{
			this.operational.SetActive(true, false);
		}
		else
		{
			this.operational.SetActive(false, false);
		}
		this.noElementStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.InvalidMaskStationConsumptionState, this.noElementStatusGuid, !this.shouldPump, null);
	}

	// Token: 0x06003079 RID: 12409 RVA: 0x0010BAE0 File Offset: 0x00109CE0
	private bool IsPumpable()
	{
		ElementConsumer[] components = base.GetComponents<ElementConsumer>();
		int num = Grid.PosToCell(base.transform.GetPosition());
		bool result = false;
		foreach (ElementConsumer elementConsumer in components)
		{
			for (int j = 0; j < (int)elementConsumer.consumptionRadius; j++)
			{
				for (int k = 0; k < (int)elementConsumer.consumptionRadius; k++)
				{
					int num2 = num + k + Grid.WidthInCells * j;
					bool flag = Grid.Element[num2].IsState(Element.State.Gas);
					bool flag2 = Grid.Element[num2].id == elementConsumer.elementToConsume;
					if (flag && flag2)
					{
						result = true;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x0600307A RID: 12410 RVA: 0x0010BB84 File Offset: 0x00109D84
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(this.choreTypeID);
		this.filteredStorage = new FilteredStorage(this, null, null, false, fetch_chore_type);
	}

	// Token: 0x0600307B RID: 12411 RVA: 0x0010BBC0 File Offset: 0x00109DC0
	private List<GameObject> GetPossibleMaterials()
	{
		List<GameObject> result = new List<GameObject>();
		this.materialStorage.Find(this.materialTag, result);
		return result;
	}

	// Token: 0x0600307C RID: 12412 RVA: 0x0010BBE7 File Offset: 0x00109DE7
	private float GetTotalMaterialAmount()
	{
		return this.materialStorage.GetMassAvailable(this.materialTag);
	}

	// Token: 0x0600307D RID: 12413 RVA: 0x0010BBFA File Offset: 0x00109DFA
	private float GetTotalOxygenAmount()
	{
		return this.oxygenStorage.GetMassAvailable(this.oxygenTag);
	}

	// Token: 0x0600307E RID: 12414 RVA: 0x0010BC10 File Offset: 0x00109E10
	private void RefreshMeters()
	{
		float num = this.GetTotalMaterialAmount();
		num = Mathf.Clamp01(num / ((float)this.maxUses * this.materialConsumedPerMask));
		float num2 = this.GetTotalOxygenAmount();
		num2 = Mathf.Clamp01(num2 / ((float)this.maxUses * this.oxygenConsumedPerMask));
		this.materialsMeter.SetPositionPercent(num);
		this.oxygenMeter.SetPositionPercent(num2);
	}

	// Token: 0x0600307F RID: 12415 RVA: 0x0010BC70 File Offset: 0x00109E70
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.CreateNewReactable();
		this.cell = Grid.PosToCell(this);
		Grid.RegisterSuitMarker(this.cell);
		this.isOperational = base.GetComponent<Operational>().IsOperational;
		base.Subscribe<MaskStation>(-592767678, MaskStation.OnOperationalChangedDelegate);
		this.isRotated = base.GetComponent<Rotatable>().IsRotated;
		base.Subscribe<MaskStation>(-1643076535, MaskStation.OnRotatedDelegate);
		this.materialsMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_resources_target", "meter_resources", this.materialsMeterOffset, Grid.SceneLayer.BuildingBack, new string[]
		{
			"meter_resources_target"
		});
		this.oxygenMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_oxygen_target", "meter_oxygen", this.oxygenMeterOffset, Grid.SceneLayer.BuildingFront, new string[]
		{
			"meter_oxygen_target"
		});
		if (this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
		base.Subscribe<MaskStation>(-1697596308, MaskStation.OnStorageChangeDelegate);
		this.RefreshMeters();
	}

	// Token: 0x06003080 RID: 12416 RVA: 0x0010BD7C File Offset: 0x00109F7C
	private void Update()
	{
		float a = this.GetTotalMaterialAmount() / this.materialConsumedPerMask;
		float b = this.GetTotalOxygenAmount() / this.oxygenConsumedPerMask;
		int fullLockerCount = (int)Mathf.Min(a, b);
		int emptyLockerCount = 0;
		Grid.UpdateSuitMarker(this.cell, fullLockerCount, emptyLockerCount, this.gridFlags, this.PathFlag);
	}

	// Token: 0x06003081 RID: 12417 RVA: 0x0010BDC8 File Offset: 0x00109FC8
	protected override void OnCleanUp()
	{
		if (this.filteredStorage != null)
		{
			this.filteredStorage.CleanUp();
		}
		if (base.isSpawned)
		{
			Grid.UnregisterSuitMarker(this.cell);
		}
		if (this.reactable != null)
		{
			this.reactable.Cleanup();
		}
		base.OnCleanUp();
	}

	// Token: 0x06003082 RID: 12418 RVA: 0x0010BE14 File Offset: 0x0010A014
	private void OnOperationalChanged(bool isOperational)
	{
		this.isOperational = isOperational;
	}

	// Token: 0x06003083 RID: 12419 RVA: 0x0010BE1D File Offset: 0x0010A01D
	private void OnStorageChange(object data)
	{
		this.RefreshMeters();
	}

	// Token: 0x06003084 RID: 12420 RVA: 0x0010BE25 File Offset: 0x0010A025
	private void UpdateGridFlag(Grid.SuitMarker.Flags flag, bool state)
	{
		if (state)
		{
			this.gridFlags |= flag;
			return;
		}
		this.gridFlags &= ~flag;
	}

	// Token: 0x06003085 RID: 12421 RVA: 0x0010BE49 File Offset: 0x0010A049
	private void CreateNewReactable()
	{
		this.reactable = new MaskStation.OxygenMaskReactable(this);
	}

	// Token: 0x04001C64 RID: 7268
	private static readonly EventSystem.IntraObjectHandler<MaskStation> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<MaskStation>(delegate(MaskStation component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04001C65 RID: 7269
	private static readonly EventSystem.IntraObjectHandler<MaskStation> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<MaskStation>(delegate(MaskStation component, object data)
	{
		component.OnOperationalChanged((bool)data);
	});

	// Token: 0x04001C66 RID: 7270
	private static readonly EventSystem.IntraObjectHandler<MaskStation> OnRotatedDelegate = new EventSystem.IntraObjectHandler<MaskStation>(delegate(MaskStation component, object data)
	{
		component.isRotated = ((Rotatable)data).IsRotated;
	});

	// Token: 0x04001C67 RID: 7271
	public float materialConsumedPerMask = 1f;

	// Token: 0x04001C68 RID: 7272
	public float oxygenConsumedPerMask = 1f;

	// Token: 0x04001C69 RID: 7273
	public Tag materialTag = GameTags.Metal;

	// Token: 0x04001C6A RID: 7274
	public Tag oxygenTag = GameTags.Breathable;

	// Token: 0x04001C6B RID: 7275
	public int maxUses = 10;

	// Token: 0x04001C6C RID: 7276
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001C6D RID: 7277
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04001C6E RID: 7278
	public Storage materialStorage;

	// Token: 0x04001C6F RID: 7279
	public Storage oxygenStorage;

	// Token: 0x04001C70 RID: 7280
	private bool shouldPump;

	// Token: 0x04001C71 RID: 7281
	private MaskStation.OxygenMaskReactable reactable;

	// Token: 0x04001C72 RID: 7282
	private MeterController materialsMeter;

	// Token: 0x04001C73 RID: 7283
	private MeterController oxygenMeter;

	// Token: 0x04001C74 RID: 7284
	public Meter.Offset materialsMeterOffset = Meter.Offset.Behind;

	// Token: 0x04001C75 RID: 7285
	public Meter.Offset oxygenMeterOffset;

	// Token: 0x04001C76 RID: 7286
	public string choreTypeID;

	// Token: 0x04001C77 RID: 7287
	protected FilteredStorage filteredStorage;

	// Token: 0x04001C78 RID: 7288
	public KAnimFile interactAnim = Assets.GetAnim("anim_equip_clothing_kanim");

	// Token: 0x04001C79 RID: 7289
	private int cell;

	// Token: 0x04001C7A RID: 7290
	public PathFinder.PotentialPath.Flags PathFlag;

	// Token: 0x04001C7B RID: 7291
	private Guid noElementStatusGuid;

	// Token: 0x04001C7C RID: 7292
	private Grid.SuitMarker.Flags gridFlags;

	// Token: 0x0200156B RID: 5483
	private class OxygenMaskReactable : Reactable
	{
		// Token: 0x06008E50 RID: 36432 RVA: 0x00342B64 File Offset: 0x00340D64
		public OxygenMaskReactable(MaskStation mask_station) : base(mask_station.gameObject, "OxygenMask", Db.Get().ChoreTypes.SuitMarker, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.maskStation = mask_station;
		}

		// Token: 0x06008E51 RID: 36433 RVA: 0x00342BB8 File Offset: 0x00340DB8
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.maskStation == null)
			{
				base.Cleanup();
				return false;
			}
			bool flag = !new_reactor.GetComponent<MinionIdentity>().GetEquipment().IsSlotOccupied(Db.Get().AssignableSlots.Suit);
			int x = transition.navGridTransition.x;
			if (x == 0)
			{
				return false;
			}
			if (!flag)
			{
				return (x >= 0 || !this.maskStation.isRotated) && (x <= 0 || this.maskStation.isRotated);
			}
			return this.maskStation.smi.IsReady() && (x <= 0 || !this.maskStation.isRotated) && (x >= 0 || this.maskStation.isRotated);
		}

		// Token: 0x06008E52 RID: 36434 RVA: 0x00342C88 File Offset: 0x00340E88
		protected override void InternalBegin()
		{
			this.startTime = Time.time;
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(this.maskStation.interactAnim, 1f);
			component.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_loop", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			this.maskStation.CreateNewReactable();
		}

		// Token: 0x06008E53 RID: 36435 RVA: 0x00342D1C File Offset: 0x00340F1C
		public override void Update(float dt)
		{
			Facing facing = this.reactor ? this.reactor.GetComponent<Facing>() : null;
			if (facing && this.maskStation)
			{
				facing.SetFacing(this.maskStation.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH);
			}
			if (Time.time - this.startTime > 2.8f)
			{
				this.Run();
				base.Cleanup();
			}
		}

		// Token: 0x06008E54 RID: 36436 RVA: 0x00342D94 File Offset: 0x00340F94
		private void Run()
		{
			GameObject reactor = this.reactor;
			Equipment equipment = reactor.GetComponent<MinionIdentity>().GetEquipment();
			bool flag = !equipment.IsSlotOccupied(Db.Get().AssignableSlots.Suit);
			Navigator component = reactor.GetComponent<Navigator>();
			bool flag2 = component != null && (component.flags & this.maskStation.PathFlag) > PathFinder.PotentialPath.Flags.None;
			if (flag)
			{
				if (!this.maskStation.smi.IsReady())
				{
					return;
				}
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("Oxygen_Mask".ToTag()), null, null);
				gameObject.SetActive(true);
				SimHashes elementID = this.maskStation.GetPossibleMaterials()[0].GetComponent<PrimaryElement>().ElementID;
				gameObject.GetComponent<PrimaryElement>().SetElement(elementID, false);
				SuitTank component2 = gameObject.GetComponent<SuitTank>();
				this.maskStation.materialStorage.ConsumeIgnoringDisease(this.maskStation.materialTag, this.maskStation.materialConsumedPerMask);
				this.maskStation.oxygenStorage.Transfer(component2.storage, component2.elementTag, this.maskStation.oxygenConsumedPerMask, false, true);
				Equippable component3 = gameObject.GetComponent<Equippable>();
				component3.Assign(equipment.GetComponent<IAssignableIdentity>());
				component3.isEquipped = true;
			}
			if (!flag)
			{
				Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
				assignable.Unassign();
				if (!flag2)
				{
					Notification notification = new Notification(MISC.NOTIFICATIONS.SUIT_DROPPED.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.SUIT_DROPPED.TOOLTIP, null, true, 0f, null, null, null, true, false, false);
					assignable.GetComponent<Notifier>().Add(notification, "");
				}
			}
		}

		// Token: 0x06008E55 RID: 36437 RVA: 0x00342F3B File Offset: 0x0034113B
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.maskStation.interactAnim);
			}
		}

		// Token: 0x06008E56 RID: 36438 RVA: 0x00342F66 File Offset: 0x00341166
		protected override void InternalCleanup()
		{
		}

		// Token: 0x04006CAF RID: 27823
		private MaskStation maskStation;

		// Token: 0x04006CB0 RID: 27824
		private float startTime;
	}

	// Token: 0x0200156C RID: 5484
	public class SMInstance : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.GameInstance
	{
		// Token: 0x06008E57 RID: 36439 RVA: 0x00342F68 File Offset: 0x00341168
		public SMInstance(MaskStation master) : base(master)
		{
		}

		// Token: 0x06008E58 RID: 36440 RVA: 0x00342F71 File Offset: 0x00341171
		private bool HasSufficientMaterials()
		{
			return base.master.GetTotalMaterialAmount() >= base.master.materialConsumedPerMask;
		}

		// Token: 0x06008E59 RID: 36441 RVA: 0x00342F8E File Offset: 0x0034118E
		private bool HasSufficientOxygen()
		{
			return base.master.GetTotalOxygenAmount() >= base.master.oxygenConsumedPerMask;
		}

		// Token: 0x06008E5A RID: 36442 RVA: 0x00342FAB File Offset: 0x003411AB
		public bool OxygenIsFull()
		{
			return base.master.GetTotalOxygenAmount() >= base.master.oxygenConsumedPerMask * (float)base.master.maxUses;
		}

		// Token: 0x06008E5B RID: 36443 RVA: 0x00342FD5 File Offset: 0x003411D5
		public bool IsReady()
		{
			return this.HasSufficientMaterials() && this.HasSufficientOxygen();
		}
	}

	// Token: 0x0200156D RID: 5485
	public class States : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation>
	{
		// Token: 0x06008E5C RID: 36444 RVA: 0x00342FEC File Offset: 0x003411EC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notOperational;
			this.notOperational.PlayAnim("off").TagTransition(GameTags.Operational, this.charging, false);
			this.charging.TagTransition(GameTags.Operational, this.notOperational, true).EventTransition(GameHashes.OnStorageChange, this.notCharging, (MaskStation.SMInstance smi) => smi.OxygenIsFull() || !smi.master.shouldPump).Update(delegate(MaskStation.SMInstance smi, float dt)
			{
				smi.master.UpdateOperational();
			}, UpdateRate.SIM_1000ms, false).Enter(delegate(MaskStation.SMInstance smi)
			{
				if (smi.OxygenIsFull() || !smi.master.shouldPump)
				{
					smi.GoTo(this.notCharging);
					return;
				}
				if (smi.IsReady())
				{
					smi.GoTo(this.charging.openChargingPre);
					return;
				}
				smi.GoTo(this.charging.closedChargingPre);
			});
			this.charging.opening.QueueAnim("opening_charging", false, null).OnAnimQueueComplete(this.charging.open);
			this.charging.open.PlayAnim("open_charging_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStorageChange, this.charging.closing, (MaskStation.SMInstance smi) => !smi.IsReady());
			this.charging.closing.QueueAnim("closing_charging", false, null).OnAnimQueueComplete(this.charging.closed);
			this.charging.closed.PlayAnim("closed_charging_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStorageChange, this.charging.opening, (MaskStation.SMInstance smi) => smi.IsReady());
			this.charging.openChargingPre.PlayAnim("open_charging_pre").OnAnimQueueComplete(this.charging.open);
			this.charging.closedChargingPre.PlayAnim("closed_charging_pre").OnAnimQueueComplete(this.charging.closed);
			this.notCharging.TagTransition(GameTags.Operational, this.notOperational, true).EventTransition(GameHashes.OnStorageChange, this.charging, (MaskStation.SMInstance smi) => !smi.OxygenIsFull() && smi.master.shouldPump).Update(delegate(MaskStation.SMInstance smi, float dt)
			{
				smi.master.UpdateOperational();
			}, UpdateRate.SIM_1000ms, false).Enter(delegate(MaskStation.SMInstance smi)
			{
				if (!smi.OxygenIsFull() && smi.master.shouldPump)
				{
					smi.GoTo(this.charging);
					return;
				}
				if (smi.IsReady())
				{
					smi.GoTo(this.notCharging.openChargingPst);
					return;
				}
				smi.GoTo(this.notCharging.closedChargingPst);
			});
			this.notCharging.opening.PlayAnim("opening_not_charging").OnAnimQueueComplete(this.notCharging.open);
			this.notCharging.open.PlayAnim("open_not_charging_loop").EventTransition(GameHashes.OnStorageChange, this.notCharging.closing, (MaskStation.SMInstance smi) => !smi.IsReady());
			this.notCharging.closing.PlayAnim("closing_not_charging").OnAnimQueueComplete(this.notCharging.closed);
			this.notCharging.closed.PlayAnim("closed_not_charging_loop").EventTransition(GameHashes.OnStorageChange, this.notCharging.opening, (MaskStation.SMInstance smi) => smi.IsReady());
			this.notCharging.openChargingPst.PlayAnim("open_charging_pst").OnAnimQueueComplete(this.notCharging.open);
			this.notCharging.closedChargingPst.PlayAnim("closed_charging_pst").OnAnimQueueComplete(this.notCharging.closed);
		}

		// Token: 0x04006CB1 RID: 27825
		public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State notOperational;

		// Token: 0x04006CB2 RID: 27826
		public MaskStation.States.ChargingStates charging;

		// Token: 0x04006CB3 RID: 27827
		public MaskStation.States.NotChargingStates notCharging;

		// Token: 0x02002505 RID: 9477
		public class ChargingStates : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State
		{
			// Token: 0x0400A4B2 RID: 42162
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State opening;

			// Token: 0x0400A4B3 RID: 42163
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State open;

			// Token: 0x0400A4B4 RID: 42164
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closing;

			// Token: 0x0400A4B5 RID: 42165
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closed;

			// Token: 0x0400A4B6 RID: 42166
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State openChargingPre;

			// Token: 0x0400A4B7 RID: 42167
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closedChargingPre;
		}

		// Token: 0x02002506 RID: 9478
		public class NotChargingStates : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State
		{
			// Token: 0x0400A4B8 RID: 42168
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State opening;

			// Token: 0x0400A4B9 RID: 42169
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State open;

			// Token: 0x0400A4BA RID: 42170
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closing;

			// Token: 0x0400A4BB RID: 42171
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closed;

			// Token: 0x0400A4BC RID: 42172
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State openChargingPst;

			// Token: 0x0400A4BD RID: 42173
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closedChargingPst;
		}
	}
}
