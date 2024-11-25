using System;

// Token: 0x02000976 RID: 2422
public class CringeMonitor : GameStateMachine<CringeMonitor, CringeMonitor.Instance>
{
	// Token: 0x060046ED RID: 18157 RVA: 0x001958F0 File Offset: 0x00193AF0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.Cringe, new GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.TriggerCringe));
		this.cringe.ToggleReactable((CringeMonitor.Instance smi) => smi.GetReactable()).ToggleStatusItem((CringeMonitor.Instance smi) => smi.GetStatusItem(), null).ScheduleGoTo(3f, this.idle);
	}

	// Token: 0x060046EE RID: 18158 RVA: 0x00195982 File Offset: 0x00193B82
	private void TriggerCringe(CringeMonitor.Instance smi, object data)
	{
		if (smi.GetComponent<KPrefabID>().HasTag(GameTags.Suit))
		{
			return;
		}
		smi.SetCringeSourceData(data);
		smi.GoTo(this.cringe);
	}

	// Token: 0x04002E36 RID: 11830
	public GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04002E37 RID: 11831
	public GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.State cringe;

	// Token: 0x02001913 RID: 6419
	public new class Instance : GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009B18 RID: 39704 RVA: 0x0036EBFD File Offset: 0x0036CDFD
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06009B19 RID: 39705 RVA: 0x0036EC08 File Offset: 0x0036CE08
		public void SetCringeSourceData(object data)
		{
			string name = (string)data;
			this.statusItem = new StatusItem("CringeSource", name, null, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
		}

		// Token: 0x06009B1A RID: 39706 RVA: 0x0036EC44 File Offset: 0x0036CE44
		public Reactable GetReactable()
		{
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "Cringe", Db.Get().ChoreTypes.EmoteHighPriority, 0f, 0f, float.PositiveInfinity, 0f);
			selfEmoteReactable.SetEmote(Db.Get().Emotes.Minion.Cringe);
			selfEmoteReactable.preventChoreInterruption = true;
			return selfEmoteReactable;
		}

		// Token: 0x06009B1B RID: 39707 RVA: 0x0036ECB0 File Offset: 0x0036CEB0
		public StatusItem GetStatusItem()
		{
			return this.statusItem;
		}

		// Token: 0x04007849 RID: 30793
		private StatusItem statusItem;
	}
}
