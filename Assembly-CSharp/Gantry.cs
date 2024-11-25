using System;
using STRINGS;

// Token: 0x020006D9 RID: 1753
public class Gantry : Switch
{
	// Token: 0x06002C5E RID: 11358 RVA: 0x000F93A8 File Offset: 0x000F75A8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (Gantry.infoStatusItem == null)
		{
			Gantry.infoStatusItem = new StatusItem("GantryAutomationInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			Gantry.infoStatusItem.resolveStringCallback = new Func<string, object, string>(Gantry.ResolveInfoStatusItemString);
		}
		base.GetComponent<KAnimControllerBase>().PlaySpeedMultiplier = 0.5f;
		this.smi = new Gantry.Instance(this, base.IsSwitchedOn);
		this.smi.StartSM();
		base.GetComponent<KSelectable>().ToggleStatusItem(Gantry.infoStatusItem, true, this.smi);
	}

	// Token: 0x06002C5F RID: 11359 RVA: 0x000F9445 File Offset: 0x000F7645
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("cleanup");
		}
		base.OnCleanUp();
	}

	// Token: 0x06002C60 RID: 11360 RVA: 0x000F9465 File Offset: 0x000F7665
	public void SetWalkable(bool active)
	{
		this.fakeFloorAdder.SetFloor(active);
	}

	// Token: 0x06002C61 RID: 11361 RVA: 0x000F9473 File Offset: 0x000F7673
	protected override void Toggle()
	{
		base.Toggle();
		this.smi.SetSwitchState(this.switchedOn);
	}

	// Token: 0x06002C62 RID: 11362 RVA: 0x000F948C File Offset: 0x000F768C
	protected override void OnRefreshUserMenu(object data)
	{
		if (!this.smi.IsAutomated())
		{
			base.OnRefreshUserMenu(data);
		}
	}

	// Token: 0x06002C63 RID: 11363 RVA: 0x000F94A2 File Offset: 0x000F76A2
	protected override void UpdateSwitchStatus()
	{
	}

	// Token: 0x06002C64 RID: 11364 RVA: 0x000F94A4 File Offset: 0x000F76A4
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		Gantry.Instance instance = (Gantry.Instance)data;
		string format = instance.IsAutomated() ? BUILDING.STATUSITEMS.GANTRY.AUTOMATION_CONTROL : BUILDING.STATUSITEMS.GANTRY.MANUAL_CONTROL;
		string arg = instance.IsExtended() ? BUILDING.STATUSITEMS.GANTRY.EXTENDED : BUILDING.STATUSITEMS.GANTRY.RETRACTED;
		return string.Format(format, arg);
	}

	// Token: 0x0400199C RID: 6556
	public static readonly HashedString PORT_ID = "Gantry";

	// Token: 0x0400199D RID: 6557
	[MyCmpReq]
	private Building building;

	// Token: 0x0400199E RID: 6558
	[MyCmpReq]
	private FakeFloorAdder fakeFloorAdder;

	// Token: 0x0400199F RID: 6559
	private Gantry.Instance smi;

	// Token: 0x040019A0 RID: 6560
	private static StatusItem infoStatusItem;

	// Token: 0x020014E3 RID: 5347
	public class States : GameStateMachine<Gantry.States, Gantry.Instance, Gantry>
	{
		// Token: 0x06008C7A RID: 35962 RVA: 0x0033A8B0 File Offset: 0x00338AB0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.extended;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.retracted_pre.Enter(delegate(Gantry.Instance smi)
			{
				smi.SetActive(true);
			}).Exit(delegate(Gantry.Instance smi)
			{
				smi.SetActive(false);
			}).PlayAnim("off_pre").OnAnimQueueComplete(this.retracted);
			this.retracted.PlayAnim("off").ParamTransition<bool>(this.should_extend, this.extended_pre, GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.IsTrue);
			this.extended_pre.Enter(delegate(Gantry.Instance smi)
			{
				smi.SetActive(true);
			}).Exit(delegate(Gantry.Instance smi)
			{
				smi.SetActive(false);
			}).PlayAnim("on_pre").OnAnimQueueComplete(this.extended);
			this.extended.Enter(delegate(Gantry.Instance smi)
			{
				smi.master.SetWalkable(true);
			}).Exit(delegate(Gantry.Instance smi)
			{
				smi.master.SetWalkable(false);
			}).PlayAnim("on").ParamTransition<bool>(this.should_extend, this.retracted_pre, GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.IsFalse).ToggleTag(GameTags.GantryExtended);
		}

		// Token: 0x04006B46 RID: 27462
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State retracted_pre;

		// Token: 0x04006B47 RID: 27463
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State retracted;

		// Token: 0x04006B48 RID: 27464
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State extended_pre;

		// Token: 0x04006B49 RID: 27465
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State extended;

		// Token: 0x04006B4A RID: 27466
		public StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.BoolParameter should_extend;
	}

	// Token: 0x020014E4 RID: 5348
	public class Instance : GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.GameInstance
	{
		// Token: 0x06008C7C RID: 35964 RVA: 0x0033AA3C File Offset: 0x00338C3C
		public Instance(Gantry master, bool manual_start_state) : base(master)
		{
			this.manual_on = manual_start_state;
			this.operational = base.GetComponent<Operational>();
			this.logic = base.GetComponent<LogicPorts>();
			base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
			base.Subscribe(-801688580, new Action<object>(this.OnLogicValueChanged));
			base.smi.sm.should_extend.Set(true, base.smi, false);
		}

		// Token: 0x06008C7D RID: 35965 RVA: 0x0033AAC2 File Offset: 0x00338CC2
		public bool IsAutomated()
		{
			return this.logic.IsPortConnected(Gantry.PORT_ID);
		}

		// Token: 0x06008C7E RID: 35966 RVA: 0x0033AAD4 File Offset: 0x00338CD4
		public bool IsExtended()
		{
			if (!this.IsAutomated())
			{
				return this.manual_on;
			}
			return this.logic_on;
		}

		// Token: 0x06008C7F RID: 35967 RVA: 0x0033AAEB File Offset: 0x00338CEB
		public void SetSwitchState(bool on)
		{
			this.manual_on = on;
			this.UpdateShouldExtend();
		}

		// Token: 0x06008C80 RID: 35968 RVA: 0x0033AAFA File Offset: 0x00338CFA
		public void SetActive(bool active)
		{
			this.operational.SetActive(this.operational.IsOperational && active, false);
		}

		// Token: 0x06008C81 RID: 35969 RVA: 0x0033AB15 File Offset: 0x00338D15
		private void OnOperationalChanged(object data)
		{
			this.UpdateShouldExtend();
		}

		// Token: 0x06008C82 RID: 35970 RVA: 0x0033AB20 File Offset: 0x00338D20
		private void OnLogicValueChanged(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID != Gantry.PORT_ID)
			{
				return;
			}
			this.logic_on = LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue);
			this.UpdateShouldExtend();
		}

		// Token: 0x06008C83 RID: 35971 RVA: 0x0033AB60 File Offset: 0x00338D60
		private void UpdateShouldExtend()
		{
			if (!this.operational.IsOperational)
			{
				return;
			}
			if (this.IsAutomated())
			{
				base.smi.sm.should_extend.Set(this.logic_on, base.smi, false);
				return;
			}
			base.smi.sm.should_extend.Set(this.manual_on, base.smi, false);
		}

		// Token: 0x04006B4B RID: 27467
		private Operational operational;

		// Token: 0x04006B4C RID: 27468
		public LogicPorts logic;

		// Token: 0x04006B4D RID: 27469
		public bool logic_on = true;

		// Token: 0x04006B4E RID: 27470
		private bool manual_on;
	}
}
