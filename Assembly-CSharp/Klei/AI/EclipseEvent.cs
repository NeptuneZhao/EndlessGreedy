using System;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02000F60 RID: 3936
	public class EclipseEvent : GameplayEvent<EclipseEvent.StatesInstance>
	{
		// Token: 0x060078F7 RID: 30967 RVA: 0x002FE0A7 File Offset: 0x002FC2A7
		public EclipseEvent() : base("EclipseEvent", 0, 0)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.ECLIPSE.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.ECLIPSE.DESCRIPTION;
		}

		// Token: 0x060078F8 RID: 30968 RVA: 0x002FE0D6 File Offset: 0x002FC2D6
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new EclipseEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x04005A57 RID: 23127
		public const string ID = "EclipseEvent";

		// Token: 0x04005A58 RID: 23128
		public const float duration = 30f;

		// Token: 0x0200234D RID: 9037
		public class StatesInstance : GameplayEventStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, EclipseEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600B631 RID: 46641 RVA: 0x003C9EAF File Offset: 0x003C80AF
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, EclipseEvent eclipseEvent) : base(master, eventInstance, eclipseEvent)
			{
			}
		}

		// Token: 0x0200234E RID: 9038
		public class States : GameplayEventStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, EclipseEvent>
		{
			// Token: 0x0600B632 RID: 46642 RVA: 0x003C9EBC File Offset: 0x003C80BC
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.planning;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.planning.GoTo(this.eclipse);
				this.eclipse.ToggleNotification((EclipseEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null)).Enter(delegate(EclipseEvent.StatesInstance smi)
				{
					TimeOfDay.Instance.SetEclipse(true);
				}).Exit(delegate(EclipseEvent.StatesInstance smi)
				{
					TimeOfDay.Instance.SetEclipse(false);
				}).ScheduleGoTo(30f, this.finished);
				this.finished.ReturnSuccess();
			}

			// Token: 0x0600B633 RID: 46643 RVA: 0x003C9F68 File Offset: 0x003C8168
			public override EventInfoData GenerateEventPopupData(EclipseEvent.StatesInstance smi)
			{
				return new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName)
				{
					location = GAMEPLAY_EVENTS.LOCATIONS.SUN,
					whenDescription = GAMEPLAY_EVENTS.TIMES.NOW
				};
			}

			// Token: 0x04009E52 RID: 40530
			public GameStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, object>.State planning;

			// Token: 0x04009E53 RID: 40531
			public GameStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, object>.State eclipse;

			// Token: 0x04009E54 RID: 40532
			public GameStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, object>.State finished;
		}
	}
}
