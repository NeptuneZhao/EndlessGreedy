using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000688 RID: 1672
[AddComponentMenu("KMonoBehaviour/Workable/Bottler")]
public class Bottler : Workable, IUserControlledCapacity
{
	// Token: 0x17000225 RID: 549
	// (get) Token: 0x0600299B RID: 10651 RVA: 0x000EAD77 File Offset: 0x000E8F77
	// (set) Token: 0x0600299C RID: 10652 RVA: 0x000EADA3 File Offset: 0x000E8FA3
	public float UserMaxCapacity
	{
		get
		{
			if (this.consumer != null)
			{
				return Mathf.Min(this.userMaxCapacity, this.storage.capacityKg);
			}
			return 0f;
		}
		set
		{
			this.userMaxCapacity = value;
			this.SetConsumerCapacity(value);
		}
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x0600299D RID: 10653 RVA: 0x000EADB3 File Offset: 0x000E8FB3
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x0600299E RID: 10654 RVA: 0x000EADC0 File Offset: 0x000E8FC0
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x0600299F RID: 10655 RVA: 0x000EADC7 File Offset: 0x000E8FC7
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x060029A0 RID: 10656 RVA: 0x000EADD4 File Offset: 0x000E8FD4
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x060029A1 RID: 10657 RVA: 0x000EADD7 File Offset: 0x000E8FD7
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x060029A2 RID: 10658 RVA: 0x000EADDF File Offset: 0x000E8FDF
	private Tag SourceTag
	{
		get
		{
			if (this.smi.master.consumer.conduitType != ConduitType.Gas)
			{
				return GameTags.LiquidSource;
			}
			return GameTags.GasSource;
		}
	}

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x060029A3 RID: 10659 RVA: 0x000EAE04 File Offset: 0x000E9004
	private Tag ElementTag
	{
		get
		{
			if (this.smi.master.consumer.conduitType != ConduitType.Gas)
			{
				return GameTags.Liquid;
			}
			return GameTags.Gas;
		}
	}

	// Token: 0x060029A4 RID: 10660 RVA: 0x000EAE2C File Offset: 0x000E902C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_bottler_kanim")
		};
		this.workAnims = new HashedString[]
		{
			"pick_up"
		};
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.synchronizeAnims = true;
		base.SetOffsets(new CellOffset[]
		{
			this.workCellOffset
		});
		base.SetWorkTime(this.overrideAnims[0].GetData().GetAnim("pick_up").totalTime);
		this.resetProgressOnStop = true;
		this.showProgressBar = false;
	}

	// Token: 0x060029A5 RID: 10661 RVA: 0x000EAED8 File Offset: 0x000E90D8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new Bottler.Controller.Instance(this);
		this.smi.StartSM();
		base.Subscribe<Bottler>(-905833192, Bottler.OnCopySettingsDelegate);
		this.UpdateStoredItemState();
		this.SetConsumerCapacity(this.userMaxCapacity);
	}

	// Token: 0x060029A6 RID: 10662 RVA: 0x000EAF28 File Offset: 0x000E9128
	protected override void OnForcedCleanUp()
	{
		if (base.worker != null)
		{
			ChoreDriver component = base.worker.GetComponent<ChoreDriver>();
			if (component != null)
			{
				component.StopChore();
			}
			else
			{
				base.worker.StopWork();
			}
		}
		if (this.workerMeter != null)
		{
			this.CleanupBottleProxyObject();
		}
		base.OnForcedCleanUp();
	}

	// Token: 0x060029A7 RID: 10663 RVA: 0x000EAF7F File Offset: 0x000E917F
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.CreateBottleProxyObject(worker);
	}

	// Token: 0x060029A8 RID: 10664 RVA: 0x000EAF90 File Offset: 0x000E9190
	private void CreateBottleProxyObject(WorkerBase worker)
	{
		if (this.workerMeter != null)
		{
			this.CleanupBottleProxyObject();
			KCrashReporter.ReportDevNotification("CreateBottleProxyObject called before cleanup", Environment.StackTrace, "", false, null);
		}
		PrimaryElement firstPrimaryElement = this.smi.master.GetFirstPrimaryElement();
		if (firstPrimaryElement == null)
		{
			KCrashReporter.ReportDevNotification("CreateBottleProxyObject on a null element", Environment.StackTrace, "", false, null);
			return;
		}
		this.workerMeter = new MeterController(worker.GetComponent<KBatchedAnimController>(), "snapto_chest", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"snapto_chest"
		});
		this.workerMeter.meterController.SwapAnims(firstPrimaryElement.Element.substance.anims);
		this.workerMeter.meterController.Play("empty", KAnim.PlayMode.Paused, 1f, 0f);
		Color32 colour = firstPrimaryElement.Element.substance.colour;
		colour.a = byte.MaxValue;
		this.workerMeter.SetSymbolTint(new KAnimHashedString("meter_fill"), colour);
		this.workerMeter.SetSymbolTint(new KAnimHashedString("water1"), colour);
		this.workerMeter.SetSymbolTint(new KAnimHashedString("substance_tinter"), colour);
	}

	// Token: 0x060029A9 RID: 10665 RVA: 0x000EB0C4 File Offset: 0x000E92C4
	private void CleanupBottleProxyObject()
	{
		if (this.workerMeter != null && !this.workerMeter.gameObject.IsNullOrDestroyed())
		{
			this.workerMeter.Unlink();
			this.workerMeter.gameObject.DeleteObject();
		}
		else
		{
			string str = "Bottler finished work but could not clean up the proxy bottle object. workerMeter=";
			MeterController meterController = this.workerMeter;
			DebugUtil.DevLogError(str + ((meterController != null) ? meterController.ToString() : null));
			KCrashReporter.ReportDevNotification("Bottle emptier could not clean up proxy object", Environment.StackTrace, "", false, null);
		}
		this.workerMeter = null;
	}

	// Token: 0x060029AA RID: 10666 RVA: 0x000EB146 File Offset: 0x000E9346
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.CleanupBottleProxyObject();
	}

	// Token: 0x060029AB RID: 10667 RVA: 0x000EB155 File Offset: 0x000E9355
	protected override void OnAbortWork(WorkerBase worker)
	{
		base.OnAbortWork(worker);
		this.GetAnimController().Play("ready", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060029AC RID: 10668 RVA: 0x000EB180 File Offset: 0x000E9380
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = worker.GetComponent<Storage>();
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		if (pickupableStartWorkInfo.amount > 0f)
		{
			this.storage.TransferMass(component, pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID(), pickupableStartWorkInfo.amount, false, false, false);
		}
		GameObject gameObject = component.FindFirst(pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID());
		if (gameObject != null)
		{
			Pickupable component2 = gameObject.GetComponent<Pickupable>();
			component2.targetWorkable = component2;
			component2.RemoveTag(this.SourceTag);
			pickupableStartWorkInfo.setResultCb(gameObject);
		}
		else
		{
			pickupableStartWorkInfo.setResultCb(null);
		}
		base.OnCompleteWork(worker);
	}

	// Token: 0x060029AD RID: 10669 RVA: 0x000EB22C File Offset: 0x000E942C
	private void OnReservationsChanged(Pickupable _ignore, bool _ignore2, Pickupable.Reservation _ignore3)
	{
		bool forceUnfetchable = false;
		using (List<GameObject>.Enumerator enumerator = this.storage.items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetComponent<Pickupable>().ReservedAmount > 0f)
				{
					forceUnfetchable = true;
					break;
				}
			}
		}
		foreach (GameObject go in this.storage.items)
		{
			FetchableMonitor.Instance instance = go.GetSMI<FetchableMonitor.Instance>();
			if (instance != null)
			{
				instance.SetForceUnfetchable(forceUnfetchable);
			}
		}
	}

	// Token: 0x060029AE RID: 10670 RVA: 0x000EB2E4 File Offset: 0x000E94E4
	private void SetConsumerCapacity(float value)
	{
		if (this.consumer != null)
		{
			this.consumer.capacityKG = value;
			float num = this.storage.MassStored() - this.userMaxCapacity;
			if (num > 0f)
			{
				this.storage.DropSome(this.storage.FindFirstWithMass(this.smi.master.ElementTag, 0f).ElementID.CreateTag(), num, false, false, new Vector3(0.8f, 0f, 0f), true, false);
			}
		}
	}

	// Token: 0x060029AF RID: 10671 RVA: 0x000EB375 File Offset: 0x000E9575
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("OnCleanUp");
		}
		base.OnCleanUp();
	}

	// Token: 0x060029B0 RID: 10672 RVA: 0x000EB398 File Offset: 0x000E9598
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

	// Token: 0x060029B1 RID: 10673 RVA: 0x000EB3E4 File Offset: 0x000E95E4
	private void UpdateStoredItemState()
	{
		this.storage.allowItemRemoval = (this.smi != null && this.smi.GetCurrentState() == this.smi.sm.ready);
		foreach (GameObject gameObject in this.storage.items)
		{
			if (gameObject != null)
			{
				gameObject.Trigger(-778359855, this.storage);
			}
		}
	}

	// Token: 0x060029B2 RID: 10674 RVA: 0x000EB484 File Offset: 0x000E9684
	private void OnCopySettings(object data)
	{
		Bottler component = ((GameObject)data).GetComponent<Bottler>();
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x040017FF RID: 6143
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001800 RID: 6144
	public Storage storage;

	// Token: 0x04001801 RID: 6145
	public ConduitConsumer consumer;

	// Token: 0x04001802 RID: 6146
	public CellOffset workCellOffset = new CellOffset(0, 0);

	// Token: 0x04001803 RID: 6147
	[Serialize]
	public float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x04001804 RID: 6148
	private Bottler.Controller.Instance smi;

	// Token: 0x04001805 RID: 6149
	private int storageHandle;

	// Token: 0x04001806 RID: 6150
	private MeterController workerMeter;

	// Token: 0x04001807 RID: 6151
	private static readonly EventSystem.IntraObjectHandler<Bottler> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Bottler>(delegate(Bottler component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02001472 RID: 5234
	private class Controller : GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler>
	{
		// Token: 0x06008AAF RID: 35503 RVA: 0x00334710 File Offset: 0x00332910
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.PlayAnim("off").EventHandlerTransition(GameHashes.OnStorageChange, this.filling, (Bottler.Controller.Instance smi, object o) => Bottler.Controller.IsFull(smi)).EnterTransition(this.ready, new StateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.Transition.ConditionCallback(Bottler.Controller.IsFull));
			this.filling.PlayAnim("working").Enter(delegate(Bottler.Controller.Instance smi)
			{
				smi.UpdateMeter();
			}).OnAnimQueueComplete(this.ready);
			this.ready.EventTransition(GameHashes.OnStorageChange, this.empty, GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.Not(new StateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.Transition.ConditionCallback(Bottler.Controller.IsFull))).PlayAnim("ready").Enter(delegate(Bottler.Controller.Instance smi)
			{
				smi.master.storage.allowItemRemoval = true;
				smi.UpdateMeter();
				foreach (GameObject gameObject in smi.master.storage.items)
				{
					Pickupable component = gameObject.GetComponent<Pickupable>();
					component.targetWorkable = smi.master;
					component.SetOffsets(new CellOffset[]
					{
						smi.master.workCellOffset
					});
					Pickupable pickupable = component;
					pickupable.OnReservationsChanged = (Action<Pickupable, bool, Pickupable.Reservation>)Delegate.Combine(pickupable.OnReservationsChanged, new Action<Pickupable, bool, Pickupable.Reservation>(smi.master.OnReservationsChanged));
					component.KPrefabID.AddTag(smi.master.SourceTag, false);
					gameObject.Trigger(-778359855, smi.master.storage);
				}
			}).Exit(delegate(Bottler.Controller.Instance smi)
			{
				smi.master.storage.allowItemRemoval = false;
				foreach (GameObject gameObject in smi.master.storage.items)
				{
					Pickupable component = gameObject.GetComponent<Pickupable>();
					component.targetWorkable = component;
					component.SetOffsetTable(OffsetGroups.InvertedStandardTable);
					component.OnReservationsChanged = (Action<Pickupable, bool, Pickupable.Reservation>)Delegate.Remove(component.OnReservationsChanged, new Action<Pickupable, bool, Pickupable.Reservation>(smi.master.OnReservationsChanged));
					component.KPrefabID.RemoveTag(smi.master.SourceTag);
					FetchableMonitor.Instance smi2 = component.GetSMI<FetchableMonitor.Instance>();
					if (smi2 != null)
					{
						smi2.SetForceUnfetchable(false);
					}
					gameObject.Trigger(-778359855, smi.master.storage);
				}
			});
		}

		// Token: 0x06008AB0 RID: 35504 RVA: 0x00334836 File Offset: 0x00332A36
		public static bool IsFull(Bottler.Controller.Instance smi)
		{
			return smi.master.storage.MassStored() >= smi.master.userMaxCapacity;
		}

		// Token: 0x040069C9 RID: 27081
		public GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.State empty;

		// Token: 0x040069CA RID: 27082
		public GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.State filling;

		// Token: 0x040069CB RID: 27083
		public GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.State ready;

		// Token: 0x020024BF RID: 9407
		public new class Instance : GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.GameInstance
		{
			// Token: 0x17000C17 RID: 3095
			// (get) Token: 0x0600BB0F RID: 47887 RVA: 0x003D48E7 File Offset: 0x003D2AE7
			// (set) Token: 0x0600BB10 RID: 47888 RVA: 0x003D48EF File Offset: 0x003D2AEF
			public MeterController meter { get; private set; }

			// Token: 0x0600BB11 RID: 47889 RVA: 0x003D48F8 File Offset: 0x003D2AF8
			public Instance(Bottler master) : base(master)
			{
				this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "bottle", "off", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, new string[]
				{
					"bottle",
					"substance_tinter"
				});
			}

			// Token: 0x0600BB12 RID: 47890 RVA: 0x003D4940 File Offset: 0x003D2B40
			public void UpdateMeter()
			{
				PrimaryElement firstPrimaryElement = base.smi.master.GetFirstPrimaryElement();
				if (firstPrimaryElement == null)
				{
					return;
				}
				this.meter.meterController.SwapAnims(firstPrimaryElement.Element.substance.anims);
				this.meter.meterController.Play(OreSizeVisualizerComponents.GetAnimForMass(firstPrimaryElement.Mass), KAnim.PlayMode.Paused, 1f, 0f);
				Color32 colour = firstPrimaryElement.Element.substance.colour;
				colour.a = byte.MaxValue;
				this.meter.SetSymbolTint(new KAnimHashedString("meter_fill"), colour);
				this.meter.SetSymbolTint(new KAnimHashedString("water1"), colour);
				this.meter.SetSymbolTint(new KAnimHashedString("substance_tinter"), colour);
			}
		}
	}
}
