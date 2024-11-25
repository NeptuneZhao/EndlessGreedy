using System;
using STRINGS;

// Token: 0x02000727 RID: 1831
public class MassiveHeatSink : StateMachineComponent<MassiveHeatSink.StatesInstance>
{
	// Token: 0x0600309B RID: 12443 RVA: 0x0010C258 File Offset: 0x0010A458
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04001C81 RID: 7297
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001C82 RID: 7298
	[MyCmpReq]
	private ElementConverter elementConverter;

	// Token: 0x02001570 RID: 5488
	public class States : GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink>
	{
		// Token: 0x06008E69 RID: 36457 RVA: 0x003434E0 File Offset: 0x003416E0
		private string AwaitingFuelResolveString(string str, object obj)
		{
			ElementConverter elementConverter = ((MassiveHeatSink.StatesInstance)obj).master.elementConverter;
			string arg = elementConverter.consumedElements[0].Tag.ProperName();
			string formattedMass = GameUtil.GetFormattedMass(elementConverter.consumedElements[0].MassConsumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
			str = string.Format(str, arg, formattedMass);
			return str;
		}

		// Token: 0x06008E6A RID: 36458 RVA: 0x00343540 File Offset: 0x00341740
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.idle, (MassiveHeatSink.StatesInstance smi) => smi.master.operational.IsOperational);
			GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State state = this.idle.EventTransition(GameHashes.OperationalChanged, this.disabled, (MassiveHeatSink.StatesInstance smi) => !smi.master.operational.IsOperational);
			string name = BUILDING.STATUSITEMS.AWAITINGFUEL.NAME;
			string tooltip = BUILDING.STATUSITEMS.AWAITINGFUEL.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Exclamation;
			NotificationType notification_type = NotificationType.BadMinor;
			bool allow_multiples = false;
			Func<string, MassiveHeatSink.StatesInstance, string> resolve_string_callback = new Func<string, MassiveHeatSink.StatesInstance, string>(this.AwaitingFuelResolveString);
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, resolve_string_callback, null, null).EventTransition(GameHashes.OnStorageChange, this.active, (MassiveHeatSink.StatesInstance smi) => smi.master.elementConverter.HasEnoughMassToStartConverting(false));
			this.active.EventTransition(GameHashes.OperationalChanged, this.disabled, (MassiveHeatSink.StatesInstance smi) => !smi.master.operational.IsOperational).EventTransition(GameHashes.OnStorageChange, this.idle, (MassiveHeatSink.StatesInstance smi) => !smi.master.elementConverter.HasEnoughMassToStartConverting(false)).Enter(delegate(MassiveHeatSink.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(MassiveHeatSink.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
		}

		// Token: 0x04006CB6 RID: 27830
		public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State disabled;

		// Token: 0x04006CB7 RID: 27831
		public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State idle;

		// Token: 0x04006CB8 RID: 27832
		public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State active;
	}

	// Token: 0x02001571 RID: 5489
	public class StatesInstance : GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.GameInstance
	{
		// Token: 0x06008E6C RID: 36460 RVA: 0x003436E4 File Offset: 0x003418E4
		public StatesInstance(MassiveHeatSink master) : base(master)
		{
		}
	}
}
