using System;
using KSerialization;
using STRINGS;

// Token: 0x02000CD8 RID: 3288
public class WorldDetectedMessage : Message
{
	// Token: 0x060065A5 RID: 26021 RVA: 0x0025E923 File Offset: 0x0025CB23
	public WorldDetectedMessage()
	{
	}

	// Token: 0x060065A6 RID: 26022 RVA: 0x0025E92B File Offset: 0x0025CB2B
	public WorldDetectedMessage(WorldContainer world)
	{
		this.worldID = world.id;
	}

	// Token: 0x060065A7 RID: 26023 RVA: 0x0025E93F File Offset: 0x0025CB3F
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x060065A8 RID: 26024 RVA: 0x0025E948 File Offset: 0x0025CB48
	public override string GetMessageBody()
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(this.worldID);
		return string.Format(MISC.NOTIFICATIONS.WORLDDETECTED.MESSAGEBODY, world.GetProperName());
	}

	// Token: 0x060065A9 RID: 26025 RVA: 0x0025E97B File Offset: 0x0025CB7B
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.WORLDDETECTED.NAME;
	}

	// Token: 0x060065AA RID: 26026 RVA: 0x0025E988 File Offset: 0x0025CB88
	public override string GetTooltip()
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(this.worldID);
		return string.Format(MISC.NOTIFICATIONS.WORLDDETECTED.TOOLTIP, world.GetProperName());
	}

	// Token: 0x060065AB RID: 26027 RVA: 0x0025E9BB File Offset: 0x0025CBBB
	public override bool IsValid()
	{
		return this.worldID != 255;
	}

	// Token: 0x040044A7 RID: 17575
	[Serialize]
	private int worldID;
}
