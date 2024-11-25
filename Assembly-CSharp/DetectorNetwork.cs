using System;
using STRINGS;

// Token: 0x020006B5 RID: 1717
public class DetectorNetwork : GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>
{
	// Token: 0x06002B3E RID: 11070 RVA: 0x000F2DD4 File Offset: 0x000F0FD4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (DetectorNetwork.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.operational.InitializeStates(this).EventTransition(GameHashes.OperationalChanged, this.inoperational, (DetectorNetwork.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
	}

	// Token: 0x040018D6 RID: 6358
	public StateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.FloatParameter networkQuality;

	// Token: 0x040018D7 RID: 6359
	public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State inoperational;

	// Token: 0x040018D8 RID: 6360
	public DetectorNetwork.NetworkStates operational;

	// Token: 0x020014A9 RID: 5289
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020014AA RID: 5290
	public class NetworkStates : GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State
	{
		// Token: 0x06008BB9 RID: 35769 RVA: 0x00337D0C File Offset: 0x00335F0C
		public DetectorNetwork.NetworkStates InitializeStates(DetectorNetwork parent)
		{
			base.DefaultState(this.poor);
			GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State state = this.poor;
			string name = BUILDING.STATUSITEMS.NETWORKQUALITY.NAME;
			string tooltip = BUILDING.STATUSITEMS.NETWORKQUALITY.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Exclamation;
			NotificationType notification_type = NotificationType.BadMinor;
			bool allow_multiples = false;
			Func<string, DetectorNetwork.Instance, string> resolve_string_callback = new Func<string, DetectorNetwork.Instance, string>(this.StringCallback);
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, resolve_string_callback, null, null).ParamTransition<float>(parent.networkQuality, this.good, (DetectorNetwork.Instance smi, float p) => (double)p >= 0.8);
			GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State state2 = this.good;
			string name2 = BUILDING.STATUSITEMS.NETWORKQUALITY.NAME;
			string tooltip2 = BUILDING.STATUSITEMS.NETWORKQUALITY.TOOLTIP;
			string icon2 = "";
			StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
			NotificationType notification_type2 = NotificationType.Neutral;
			bool allow_multiples2 = false;
			resolve_string_callback = new Func<string, DetectorNetwork.Instance, string>(this.StringCallback);
			state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, resolve_string_callback, null, null).ParamTransition<float>(parent.networkQuality, this.poor, (DetectorNetwork.Instance smi, float p) => (double)p < 0.8);
			return this;
		}

		// Token: 0x06008BBA RID: 35770 RVA: 0x00337E14 File Offset: 0x00336014
		private string StringCallback(string str, DetectorNetwork.Instance smi)
		{
			MathUtil.MinMax detectTimeRangeForWorld = Game.Instance.spaceScannerNetworkManager.GetDetectTimeRangeForWorld(smi.GetMyWorldId());
			float num = Game.Instance.spaceScannerNetworkManager.GetQualityForWorld(smi.GetMyWorldId());
			num = num.Remap(new ValueTuple<float, float>(0f, 1f), new ValueTuple<float, float>(0f, 0.5f));
			return str.Replace("{TotalQuality}", GameUtil.GetFormattedPercent(smi.GetNetworkQuality01() * 100f, GameUtil.TimeSlice.None)).Replace("{WorstTime}", GameUtil.GetFormattedTime(detectTimeRangeForWorld.min, "F0")).Replace("{BestTime}", GameUtil.GetFormattedTime(detectTimeRangeForWorld.max, "F0")).Replace("{Coverage}", GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None));
		}

		// Token: 0x04006A95 RID: 27285
		public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State poor;

		// Token: 0x04006A96 RID: 27286
		public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State good;
	}

	// Token: 0x020014AB RID: 5291
	public new class Instance : GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.GameInstance
	{
		// Token: 0x06008BBC RID: 35772 RVA: 0x00337EE4 File Offset: 0x003360E4
		public Instance(IStateMachineTarget master, DetectorNetwork.Def def) : base(master, def)
		{
		}

		// Token: 0x06008BBD RID: 35773 RVA: 0x00337EEE File Offset: 0x003360EE
		public override void StartSM()
		{
			this.worldId = base.master.gameObject.GetMyWorldId();
			Components.DetectorNetworks.Add(this.worldId, this);
			base.StartSM();
		}

		// Token: 0x06008BBE RID: 35774 RVA: 0x00337F1D File Offset: 0x0033611D
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Components.DetectorNetworks.Remove(this.worldId, this);
		}

		// Token: 0x06008BBF RID: 35775 RVA: 0x00337F37 File Offset: 0x00336137
		public void Internal_SetNetworkQuality(float quality01)
		{
			base.sm.networkQuality.Set(quality01, base.smi, false);
		}

		// Token: 0x06008BC0 RID: 35776 RVA: 0x00337F52 File Offset: 0x00336152
		public float GetNetworkQuality01()
		{
			return base.sm.networkQuality.Get(base.smi);
		}

		// Token: 0x04006A97 RID: 27287
		[NonSerialized]
		private int worldId;
	}
}
