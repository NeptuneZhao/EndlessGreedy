using System;
using UnityEngine;

// Token: 0x020004C4 RID: 1220
public abstract class GameplayEventStateMachine<StateMachineType, StateMachineInstanceType, MasterType, SecondMasterType> : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType> where StateMachineType : GameplayEventStateMachine<StateMachineType, StateMachineInstanceType, MasterType, SecondMasterType> where StateMachineInstanceType : GameplayEventStateMachine<StateMachineType, StateMachineInstanceType, MasterType, SecondMasterType>.GameplayEventStateMachineInstance where MasterType : IStateMachineTarget where SecondMasterType : GameplayEvent<StateMachineInstanceType>
{
	// Token: 0x06001A3F RID: 6719 RVA: 0x0008B194 File Offset: 0x00089394
	public void MonitorStart(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.TargetParameter target, StateMachineInstanceType smi)
	{
		GameObject gameObject = target.Get(smi);
		if (gameObject != null)
		{
			gameObject.Trigger(-1660384580, smi.eventInstance);
		}
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x0008B1C8 File Offset: 0x000893C8
	public void MonitorChanged(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.TargetParameter target, StateMachineInstanceType smi)
	{
		GameObject gameObject = target.Get(smi);
		if (gameObject != null)
		{
			gameObject.Trigger(-1122598290, smi.eventInstance);
		}
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x0008B1FC File Offset: 0x000893FC
	public void MonitorStop(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.TargetParameter target, StateMachineInstanceType smi)
	{
		GameObject gameObject = target.Get(smi);
		if (gameObject != null)
		{
			gameObject.Trigger(-828272459, smi.eventInstance);
		}
	}

	// Token: 0x06001A42 RID: 6722 RVA: 0x0008B230 File Offset: 0x00089430
	public virtual EventInfoData GenerateEventPopupData(StateMachineInstanceType smi)
	{
		return null;
	}

	// Token: 0x02001289 RID: 4745
	public class GameplayEventStateMachineInstance : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.GameInstance
	{
		// Token: 0x0600841B RID: 33819 RVA: 0x00322700 File Offset: 0x00320900
		public GameplayEventStateMachineInstance(MasterType master, GameplayEventInstance eventInstance, SecondMasterType gameplayEvent) : base(master)
		{
			this.gameplayEvent = gameplayEvent;
			this.eventInstance = eventInstance;
			eventInstance.GetEventPopupData = (() => base.smi.sm.GenerateEventPopupData(base.smi));
			this.serializationSuffix = gameplayEvent.Id;
		}

		// Token: 0x040063A8 RID: 25512
		public GameplayEventInstance eventInstance;

		// Token: 0x040063A9 RID: 25513
		public SecondMasterType gameplayEvent;
	}
}
