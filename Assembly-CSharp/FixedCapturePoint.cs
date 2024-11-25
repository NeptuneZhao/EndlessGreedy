using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000567 RID: 1383
public class FixedCapturePoint : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>
{
	// Token: 0x0600200A RID: 8202 RVA: 0x000B4818 File Offset: 0x000B2A18
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.operational;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.unoperational.TagTransition(GameTags.Operational, this.operational, false);
		this.operational.DefaultState(this.operational.manual).TagTransition(GameTags.Operational, this.unoperational, true);
		this.operational.manual.ParamTransition<bool>(this.automated, this.operational.automated, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.IsTrue);
		this.operational.automated.ParamTransition<bool>(this.automated, this.operational.manual, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.IsFalse).ToggleChore((FixedCapturePoint.Instance smi) => smi.CreateChore(), this.unoperational, this.unoperational).Update("FindFixedCapturable", delegate(FixedCapturePoint.Instance smi, float dt)
		{
			smi.FindFixedCapturable();
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x0400121C RID: 4636
	public static readonly Operational.Flag enabledFlag = new Operational.Flag("enabled", Operational.Flag.Type.Requirement);

	// Token: 0x0400121D RID: 4637
	private StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.BoolParameter automated;

	// Token: 0x0400121E RID: 4638
	public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State unoperational;

	// Token: 0x0400121F RID: 4639
	public FixedCapturePoint.OperationalState operational;

	// Token: 0x02001368 RID: 4968
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400668D RID: 26253
		public Func<FixedCapturePoint.Instance, FixedCapturableMonitor.Instance, bool> isAmountStoredOverCapacity;

		// Token: 0x0400668E RID: 26254
		public Func<FixedCapturePoint.Instance, int> getTargetCapturePoint = delegate(FixedCapturePoint.Instance smi)
		{
			int num = Grid.PosToCell(smi);
			Navigator navigator = smi.targetCapturable.Navigator;
			if (Grid.IsValidCell(num - 1) && navigator.CanReach(num - 1))
			{
				return num - 1;
			}
			if (Grid.IsValidCell(num + 1) && navigator.CanReach(num + 1))
			{
				return num + 1;
			}
			return num;
		};

		// Token: 0x0400668F RID: 26255
		public bool allowBabies;
	}

	// Token: 0x02001369 RID: 4969
	public class OperationalState : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State
	{
		// Token: 0x04006690 RID: 26256
		public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State manual;

		// Token: 0x04006691 RID: 26257
		public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State automated;
	}

	// Token: 0x0200136A RID: 4970
	[SerializationConfig(MemberSerialization.OptIn)]
	public new class Instance : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.GameInstance
	{
		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06008707 RID: 34567 RVA: 0x0032A8F8 File Offset: 0x00328AF8
		// (set) Token: 0x06008708 RID: 34568 RVA: 0x0032A900 File Offset: 0x00328B00
		public FixedCapturableMonitor.Instance targetCapturable { get; private set; }

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06008709 RID: 34569 RVA: 0x0032A909 File Offset: 0x00328B09
		// (set) Token: 0x0600870A RID: 34570 RVA: 0x0032A911 File Offset: 0x00328B11
		public bool shouldCreatureGoGetCaptured { get; private set; }

		// Token: 0x0600870B RID: 34571 RVA: 0x0032A91C File Offset: 0x00328B1C
		public Instance(IStateMachineTarget master, FixedCapturePoint.Def def) : base(master, def)
		{
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			this.captureCell = Grid.PosToCell(base.transform.GetPosition());
			this.critterCapactiy = base.GetComponent<BaggableCritterCapacityTracker>();
			this.operationComp = base.GetComponent<Operational>();
			this.logicPorts = base.GetComponent<LogicPorts>();
			if (this.logicPorts != null)
			{
				base.Subscribe(-801688580, new Action<object>(this.OnLogicEvent));
				this.operationComp.SetFlag(FixedCapturePoint.enabledFlag, !this.logicPorts.IsPortConnected("CritterPickUpInput") || this.logicPorts.GetInputValue("CritterPickUpInput") > 0);
				return;
			}
			this.operationComp.SetFlag(FixedCapturePoint.enabledFlag, true);
		}

		// Token: 0x0600870C RID: 34572 RVA: 0x0032A9FC File Offset: 0x00328BFC
		private void OnLogicEvent(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID == "CritterPickUpInput" && this.logicPorts.IsPortConnected("CritterPickUpInput"))
			{
				this.operationComp.SetFlag(FixedCapturePoint.enabledFlag, logicValueChanged.newValue > 0);
			}
		}

		// Token: 0x0600870D RID: 34573 RVA: 0x0032AA57 File Offset: 0x00328C57
		public override void StartSM()
		{
			base.StartSM();
			if (base.GetComponent<FixedCapturePoint.AutoWrangleCapture>() == null)
			{
				base.sm.automated.Set(true, this, false);
			}
		}

		// Token: 0x0600870E RID: 34574 RVA: 0x0032AA84 File Offset: 0x00328C84
		private void OnCopySettings(object data)
		{
			GameObject gameObject = (GameObject)data;
			if (gameObject == null)
			{
				return;
			}
			FixedCapturePoint.Instance smi = gameObject.GetSMI<FixedCapturePoint.Instance>();
			if (smi == null)
			{
				return;
			}
			base.sm.automated.Set(base.sm.automated.Get(smi), this, false);
		}

		// Token: 0x0600870F RID: 34575 RVA: 0x0032AAD1 File Offset: 0x00328CD1
		public bool GetAutomated()
		{
			return base.sm.automated.Get(this);
		}

		// Token: 0x06008710 RID: 34576 RVA: 0x0032AAE4 File Offset: 0x00328CE4
		public void SetAutomated(bool automate)
		{
			base.sm.automated.Set(automate, this, false);
		}

		// Token: 0x06008711 RID: 34577 RVA: 0x0032AAFA File Offset: 0x00328CFA
		public Chore CreateChore()
		{
			this.FindFixedCapturable();
			return new FixedCaptureChore(base.GetComponent<KPrefabID>());
		}

		// Token: 0x06008712 RID: 34578 RVA: 0x0032AB10 File Offset: 0x00328D10
		public bool IsCreatureAvailableForFixedCapture()
		{
			if (!this.targetCapturable.IsNullOrStopped())
			{
				CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(this.captureCell);
				return FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(this.targetCapturable, this, cavityForCell, this.captureCell);
			}
			return false;
		}

		// Token: 0x06008713 RID: 34579 RVA: 0x0032AB55 File Offset: 0x00328D55
		public void SetRancherIsAvailableForCapturing()
		{
			this.shouldCreatureGoGetCaptured = true;
		}

		// Token: 0x06008714 RID: 34580 RVA: 0x0032AB5E File Offset: 0x00328D5E
		public void ClearRancherIsAvailableForCapturing()
		{
			this.shouldCreatureGoGetCaptured = false;
		}

		// Token: 0x06008715 RID: 34581 RVA: 0x0032AB68 File Offset: 0x00328D68
		private static bool CanCapturableBeCapturedAtCapturePoint(FixedCapturableMonitor.Instance capturable, FixedCapturePoint.Instance capture_point, CavityInfo capture_cavity_info, int capture_cell)
		{
			if (!capturable.IsRunning())
			{
				return false;
			}
			if (capturable.targetCapturePoint != capture_point && !capturable.targetCapturePoint.IsNullOrStopped())
			{
				return false;
			}
			int cell = Grid.PosToCell(capturable.transform.GetPosition());
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			return cavityForCell != null && cavityForCell == capture_cavity_info && !capturable.HasTag(GameTags.Creatures.Bagged) && (!capturable.isBaby || capture_point.def.allowBabies) && capturable.ChoreConsumer.IsChoreEqualOrAboveCurrentChorePriority<FixedCaptureStates>() && capturable.Navigator.GetNavigationCost(capture_cell) != -1 && capture_point.def.isAmountStoredOverCapacity(capture_point, capturable);
		}

		// Token: 0x06008716 RID: 34582 RVA: 0x0032AC1C File Offset: 0x00328E1C
		public void FindFixedCapturable()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(num);
			if (cavityForCell == null)
			{
				this.ResetCapturePoint();
				return;
			}
			if (!this.targetCapturable.IsNullOrStopped() && !FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(this.targetCapturable, this, cavityForCell, num))
			{
				this.ResetCapturePoint();
			}
			if (this.targetCapturable.IsNullOrStopped())
			{
				foreach (object obj in Components.FixedCapturableMonitors)
				{
					FixedCapturableMonitor.Instance instance = (FixedCapturableMonitor.Instance)obj;
					if (FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(instance, this, cavityForCell, num))
					{
						this.targetCapturable = instance;
						if (!this.targetCapturable.IsNullOrStopped())
						{
							this.targetCapturable.targetCapturePoint = this;
							break;
						}
						break;
					}
				}
			}
		}

		// Token: 0x06008717 RID: 34583 RVA: 0x0032ACFC File Offset: 0x00328EFC
		public void ResetCapturePoint()
		{
			base.Trigger(643180843, null);
			if (!this.targetCapturable.IsNullOrStopped())
			{
				this.targetCapturable.targetCapturePoint = null;
				this.targetCapturable.Trigger(1034952693, null);
				this.targetCapturable = null;
			}
		}

		// Token: 0x04006694 RID: 26260
		public BaggableCritterCapacityTracker critterCapactiy;

		// Token: 0x04006695 RID: 26261
		private int captureCell;

		// Token: 0x04006696 RID: 26262
		private Operational operationComp;

		// Token: 0x04006697 RID: 26263
		private LogicPorts logicPorts;
	}

	// Token: 0x0200136B RID: 4971
	public class AutoWrangleCapture : KMonoBehaviour, ICheckboxControl
	{
		// Token: 0x06008718 RID: 34584 RVA: 0x0032AD3B File Offset: 0x00328F3B
		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.fcp = this.GetSMI<FixedCapturePoint.Instance>();
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06008719 RID: 34585 RVA: 0x0032AD4F File Offset: 0x00328F4F
		string ICheckboxControl.CheckboxTitleKey
		{
			get
			{
				return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.TITLE.key.String;
			}
		}

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x0600871A RID: 34586 RVA: 0x0032AD60 File Offset: 0x00328F60
		string ICheckboxControl.CheckboxLabel
		{
			get
			{
				return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE;
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x0600871B RID: 34587 RVA: 0x0032AD6C File Offset: 0x00328F6C
		string ICheckboxControl.CheckboxTooltip
		{
			get
			{
				return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE_TOOLTIP;
			}
		}

		// Token: 0x0600871C RID: 34588 RVA: 0x0032AD78 File Offset: 0x00328F78
		bool ICheckboxControl.GetCheckboxValue()
		{
			return this.fcp.GetAutomated();
		}

		// Token: 0x0600871D RID: 34589 RVA: 0x0032AD85 File Offset: 0x00328F85
		void ICheckboxControl.SetCheckboxValue(bool value)
		{
			this.fcp.SetAutomated(value);
		}

		// Token: 0x04006698 RID: 26264
		private FixedCapturePoint.Instance fcp;
	}
}
