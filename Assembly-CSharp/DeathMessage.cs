using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000CC3 RID: 3267
public class DeathMessage : TargetMessage
{
	// Token: 0x0600650E RID: 25870 RVA: 0x0025D755 File Offset: 0x0025B955
	public DeathMessage()
	{
	}

	// Token: 0x0600650F RID: 25871 RVA: 0x0025D768 File Offset: 0x0025B968
	public DeathMessage(GameObject go, Death death) : base(go.GetComponent<KPrefabID>())
	{
		this.death.Set(death);
	}

	// Token: 0x06006510 RID: 25872 RVA: 0x0025D78D File Offset: 0x0025B98D
	public override string GetSound()
	{
		return "";
	}

	// Token: 0x06006511 RID: 25873 RVA: 0x0025D794 File Offset: 0x0025B994
	public override bool PlayNotificationSound()
	{
		return false;
	}

	// Token: 0x06006512 RID: 25874 RVA: 0x0025D797 File Offset: 0x0025B997
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.DUPLICANTDIED.NAME;
	}

	// Token: 0x06006513 RID: 25875 RVA: 0x0025D7A3 File Offset: 0x0025B9A3
	public override string GetTooltip()
	{
		return this.GetMessageBody();
	}

	// Token: 0x06006514 RID: 25876 RVA: 0x0025D7AB File Offset: 0x0025B9AB
	public override string GetMessageBody()
	{
		return this.death.Get().description.Replace("{Target}", base.GetTarget().GetName());
	}

	// Token: 0x04004471 RID: 17521
	[Serialize]
	private ResourceRef<Death> death = new ResourceRef<Death>();
}
