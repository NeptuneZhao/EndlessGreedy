using System;

// Token: 0x020008C3 RID: 2243
public static class GameSoundEvents
{
	// Token: 0x04002903 RID: 10499
	public static GameSoundEvents.Event BatteryFull = new GameSoundEvents.Event("game_triggered.battery_full");

	// Token: 0x04002904 RID: 10500
	public static GameSoundEvents.Event BatteryWarning = new GameSoundEvents.Event("game_triggered.battery_warning");

	// Token: 0x04002905 RID: 10501
	public static GameSoundEvents.Event BatteryDischarged = new GameSoundEvents.Event("game_triggered.battery_drained");

	// Token: 0x020017DC RID: 6108
	public class Event
	{
		// Token: 0x060096EB RID: 38635 RVA: 0x003636F7 File Offset: 0x003618F7
		public Event(string name)
		{
			this.Name = name;
		}

		// Token: 0x040073DB RID: 29659
		public HashedString Name;
	}
}
