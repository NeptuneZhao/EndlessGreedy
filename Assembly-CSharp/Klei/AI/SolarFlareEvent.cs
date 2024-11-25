using System;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02000F6A RID: 3946
	public class SolarFlareEvent : GameplayEvent<SolarFlareEvent.StatesInstance>
	{
		// Token: 0x06007918 RID: 31000 RVA: 0x002FE827 File Offset: 0x002FCA27
		public SolarFlareEvent() : base("SolarFlareEvent", 0, 0)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.SOLAR_FLARE.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.SOLAR_FLARE.DESCRIPTION;
		}

		// Token: 0x06007919 RID: 31001 RVA: 0x002FE856 File Offset: 0x002FCA56
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new SolarFlareEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x04005A84 RID: 23172
		public const string ID = "SolarFlareEvent";

		// Token: 0x04005A85 RID: 23173
		public const float DURATION = 7f;

		// Token: 0x0200235E RID: 9054
		public class StatesInstance : GameplayEventStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, SolarFlareEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600B686 RID: 46726 RVA: 0x003CC0F8 File Offset: 0x003CA2F8
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, SolarFlareEvent solarFlareEvent) : base(master, eventInstance, solarFlareEvent)
			{
			}
		}

		// Token: 0x0200235F RID: 9055
		public class States : GameplayEventStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, SolarFlareEvent>
		{
			// Token: 0x0600B687 RID: 46727 RVA: 0x003CC103 File Offset: 0x003CA303
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.idle;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.idle.DoNothing();
				this.start.ScheduleGoTo(7f, this.finished);
				this.finished.ReturnSuccess();
			}

			// Token: 0x0600B688 RID: 46728 RVA: 0x003CC144 File Offset: 0x003CA344
			public override EventInfoData GenerateEventPopupData(SolarFlareEvent.StatesInstance smi)
			{
				return new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName)
				{
					location = GAMEPLAY_EVENTS.LOCATIONS.SUN,
					whenDescription = GAMEPLAY_EVENTS.TIMES.NOW
				};
			}

			// Token: 0x04009E89 RID: 40585
			public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State idle;

			// Token: 0x04009E8A RID: 40586
			public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State start;

			// Token: 0x04009E8B RID: 40587
			public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State finished;
		}
	}
}
