using System;

// Token: 0x0200099A RID: 2458
public class RoomMonitor : GameStateMachine<RoomMonitor, RoomMonitor.Instance>
{
	// Token: 0x060047A6 RID: 18342 RVA: 0x0019A2F9 File Offset: 0x001984F9
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.PathAdvanced, new StateMachine<RoomMonitor, RoomMonitor.Instance, IStateMachineTarget, object>.State.Callback(RoomMonitor.UpdateRoomType));
	}

	// Token: 0x060047A7 RID: 18343 RVA: 0x0019A320 File Offset: 0x00198520
	private static void UpdateRoomType(RoomMonitor.Instance smi)
	{
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(smi.master.gameObject);
		if (roomOfGameObject != smi.currentRoom)
		{
			smi.currentRoom = roomOfGameObject;
			if (roomOfGameObject != null)
			{
				roomOfGameObject.cavity.OnEnter(smi.master.gameObject);
			}
		}
	}

	// Token: 0x02001974 RID: 6516
	public new class Instance : GameStateMachine<RoomMonitor, RoomMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009CA5 RID: 40101 RVA: 0x00372CCC File Offset: 0x00370ECC
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0400797D RID: 31101
		public Room currentRoom;
	}
}
