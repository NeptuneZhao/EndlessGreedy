using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200073C RID: 1852
public class ObjectDispenser : Switch, IUserControlledCapacity
{
	// Token: 0x17000322 RID: 802
	// (get) Token: 0x0600313B RID: 12603 RVA: 0x0010FBAD File Offset: 0x0010DDAD
	// (set) Token: 0x0600313C RID: 12604 RVA: 0x0010FBC5 File Offset: 0x0010DDC5
	public virtual float UserMaxCapacity
	{
		get
		{
			return Mathf.Min(this.userMaxCapacity, base.GetComponent<Storage>().capacityKg);
		}
		set
		{
			this.userMaxCapacity = value;
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x17000323 RID: 803
	// (get) Token: 0x0600313D RID: 12605 RVA: 0x0010FBD9 File Offset: 0x0010DDD9
	public float AmountStored
	{
		get
		{
			return base.GetComponent<Storage>().MassStored();
		}
	}

	// Token: 0x17000324 RID: 804
	// (get) Token: 0x0600313E RID: 12606 RVA: 0x0010FBE6 File Offset: 0x0010DDE6
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000325 RID: 805
	// (get) Token: 0x0600313F RID: 12607 RVA: 0x0010FBED File Offset: 0x0010DDED
	public float MaxCapacity
	{
		get
		{
			return base.GetComponent<Storage>().capacityKg;
		}
	}

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x06003140 RID: 12608 RVA: 0x0010FBFA File Offset: 0x0010DDFA
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x06003141 RID: 12609 RVA: 0x0010FBFD File Offset: 0x0010DDFD
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x06003142 RID: 12610 RVA: 0x0010FC05 File Offset: 0x0010DE05
	protected override void OnPrefabInit()
	{
		this.Initialize();
	}

	// Token: 0x06003143 RID: 12611 RVA: 0x0010FC10 File Offset: 0x0010DE10
	protected void Initialize()
	{
		base.OnPrefabInit();
		this.log = new LoggerFS("ObjectDispenser", 35);
		this.filteredStorage = new FilteredStorage(this, null, this, false, Db.Get().ChoreTypes.StorageFetch);
		base.Subscribe<ObjectDispenser>(-905833192, ObjectDispenser.OnCopySettingsDelegate);
	}

	// Token: 0x06003144 RID: 12612 RVA: 0x0010FC64 File Offset: 0x0010DE64
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new ObjectDispenser.Instance(this, base.IsSwitchedOn);
		this.smi.StartSM();
		if (ObjectDispenser.infoStatusItem == null)
		{
			ObjectDispenser.infoStatusItem = new StatusItem("ObjectDispenserAutomationInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			ObjectDispenser.infoStatusItem.resolveStringCallback = new Func<string, object, string>(ObjectDispenser.ResolveInfoStatusItemString);
		}
		this.filteredStorage.FilterChanged();
		base.GetComponent<KSelectable>().ToggleStatusItem(ObjectDispenser.infoStatusItem, true, this.smi);
	}

	// Token: 0x06003145 RID: 12613 RVA: 0x0010FCFC File Offset: 0x0010DEFC
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
		base.OnCleanUp();
	}

	// Token: 0x06003146 RID: 12614 RVA: 0x0010FD10 File Offset: 0x0010DF10
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		ObjectDispenser component = gameObject.GetComponent<ObjectDispenser>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x06003147 RID: 12615 RVA: 0x0010FD4C File Offset: 0x0010DF4C
	public void DropHeldItems()
	{
		while (this.storage.Count > 0)
		{
			GameObject gameObject = this.storage.Drop(this.storage.items[0], true);
			if (this.rotatable != null)
			{
				gameObject.transform.SetPosition(base.transform.GetPosition() + this.rotatable.GetRotatedCellOffset(this.dropOffset).ToVector3());
			}
			else
			{
				gameObject.transform.SetPosition(base.transform.GetPosition() + this.dropOffset.ToVector3());
			}
		}
		this.smi.GetMaster().GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x06003148 RID: 12616 RVA: 0x0010FE1B File Offset: 0x0010E01B
	protected override void Toggle()
	{
		base.Toggle();
	}

	// Token: 0x06003149 RID: 12617 RVA: 0x0010FE23 File Offset: 0x0010E023
	protected override void OnRefreshUserMenu(object data)
	{
		if (!this.smi.IsAutomated())
		{
			base.OnRefreshUserMenu(data);
		}
	}

	// Token: 0x0600314A RID: 12618 RVA: 0x0010FE3C File Offset: 0x0010E03C
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		ObjectDispenser.Instance instance = (ObjectDispenser.Instance)data;
		string format = instance.IsAutomated() ? BUILDING.STATUSITEMS.OBJECTDISPENSER.AUTOMATION_CONTROL : BUILDING.STATUSITEMS.OBJECTDISPENSER.MANUAL_CONTROL;
		string arg = instance.IsOpened ? BUILDING.STATUSITEMS.OBJECTDISPENSER.OPENED : BUILDING.STATUSITEMS.OBJECTDISPENSER.CLOSED;
		return string.Format(format, arg);
	}

	// Token: 0x04001CEF RID: 7407
	public static readonly HashedString PORT_ID = "ObjectDispenser";

	// Token: 0x04001CF0 RID: 7408
	private LoggerFS log;

	// Token: 0x04001CF1 RID: 7409
	public CellOffset dropOffset;

	// Token: 0x04001CF2 RID: 7410
	[MyCmpReq]
	private Building building;

	// Token: 0x04001CF3 RID: 7411
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04001CF4 RID: 7412
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04001CF5 RID: 7413
	private ObjectDispenser.Instance smi;

	// Token: 0x04001CF6 RID: 7414
	private static StatusItem infoStatusItem;

	// Token: 0x04001CF7 RID: 7415
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x04001CF8 RID: 7416
	protected FilteredStorage filteredStorage;

	// Token: 0x04001CF9 RID: 7417
	private static readonly EventSystem.IntraObjectHandler<ObjectDispenser> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ObjectDispenser>(delegate(ObjectDispenser component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0200159E RID: 5534
	public class States : GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser>
	{
		// Token: 0x06008F42 RID: 36674 RVA: 0x00346E38 File Offset: 0x00345038
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.idle.PlayAnim("on").EventHandler(GameHashes.OnStorageChange, delegate(ObjectDispenser.Instance smi)
			{
				smi.UpdateState();
			}).ParamTransition<bool>(this.should_open, this.drop_item, (ObjectDispenser.Instance smi, bool p) => p && !smi.master.GetComponent<Storage>().IsEmpty());
			this.load_item.PlayAnim("working_load").OnAnimQueueComplete(this.load_item_pst);
			this.load_item_pst.ParamTransition<bool>(this.should_open, this.idle, (ObjectDispenser.Instance smi, bool p) => !p).ParamTransition<bool>(this.should_open, this.drop_item, (ObjectDispenser.Instance smi, bool p) => p);
			this.drop_item.PlayAnim("working_dispense").OnAnimQueueComplete(this.idle).Exit(delegate(ObjectDispenser.Instance smi)
			{
				smi.master.DropHeldItems();
			});
		}

		// Token: 0x04006D6D RID: 28013
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State load_item;

		// Token: 0x04006D6E RID: 28014
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State load_item_pst;

		// Token: 0x04006D6F RID: 28015
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State drop_item;

		// Token: 0x04006D70 RID: 28016
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State idle;

		// Token: 0x04006D71 RID: 28017
		public StateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.BoolParameter should_open;
	}

	// Token: 0x0200159F RID: 5535
	public class Instance : GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.GameInstance
	{
		// Token: 0x06008F44 RID: 36676 RVA: 0x00346F8C File Offset: 0x0034518C
		public Instance(ObjectDispenser master, bool manual_start_state) : base(master)
		{
			this.manual_on = manual_start_state;
			this.operational = base.GetComponent<Operational>();
			this.logic = base.GetComponent<LogicPorts>();
			base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
			base.Subscribe(-801688580, new Action<object>(this.OnLogicValueChanged));
			base.smi.sm.should_open.Set(true, base.smi, false);
		}

		// Token: 0x06008F45 RID: 36677 RVA: 0x00347012 File Offset: 0x00345212
		public void UpdateState()
		{
			base.smi.GoTo(base.sm.load_item);
		}

		// Token: 0x06008F46 RID: 36678 RVA: 0x0034702A File Offset: 0x0034522A
		public bool IsAutomated()
		{
			return this.logic.IsPortConnected(ObjectDispenser.PORT_ID);
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06008F47 RID: 36679 RVA: 0x0034703C File Offset: 0x0034523C
		public bool IsOpened
		{
			get
			{
				if (!this.IsAutomated())
				{
					return this.manual_on;
				}
				return this.logic_on;
			}
		}

		// Token: 0x06008F48 RID: 36680 RVA: 0x00347053 File Offset: 0x00345253
		public void SetSwitchState(bool on)
		{
			this.manual_on = on;
			this.UpdateShouldOpen();
		}

		// Token: 0x06008F49 RID: 36681 RVA: 0x00347062 File Offset: 0x00345262
		public void SetActive(bool active)
		{
			this.operational.SetActive(active, false);
		}

		// Token: 0x06008F4A RID: 36682 RVA: 0x00347071 File Offset: 0x00345271
		private void OnOperationalChanged(object data)
		{
			this.UpdateShouldOpen();
		}

		// Token: 0x06008F4B RID: 36683 RVA: 0x0034707C File Offset: 0x0034527C
		private void OnLogicValueChanged(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID != ObjectDispenser.PORT_ID)
			{
				return;
			}
			this.logic_on = LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue);
			this.UpdateShouldOpen();
		}

		// Token: 0x06008F4C RID: 36684 RVA: 0x003470BC File Offset: 0x003452BC
		private void UpdateShouldOpen()
		{
			this.SetActive(this.operational.IsOperational);
			if (!this.operational.IsOperational)
			{
				return;
			}
			if (this.IsAutomated())
			{
				base.smi.sm.should_open.Set(this.logic_on, base.smi, false);
				return;
			}
			base.smi.sm.should_open.Set(this.manual_on, base.smi, false);
		}

		// Token: 0x04006D72 RID: 28018
		private Operational operational;

		// Token: 0x04006D73 RID: 28019
		public LogicPorts logic;

		// Token: 0x04006D74 RID: 28020
		public bool logic_on = true;

		// Token: 0x04006D75 RID: 28021
		private bool manual_on;
	}
}
