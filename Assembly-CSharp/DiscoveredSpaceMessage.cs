using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000CC4 RID: 3268
public class DiscoveredSpaceMessage : Message
{
	// Token: 0x06006515 RID: 25877 RVA: 0x0025D7D2 File Offset: 0x0025B9D2
	public DiscoveredSpaceMessage()
	{
	}

	// Token: 0x06006516 RID: 25878 RVA: 0x0025D7DA File Offset: 0x0025B9DA
	public DiscoveredSpaceMessage(Vector3 pos)
	{
		this.cameraFocusPos = pos;
		this.cameraFocusPos.z = -40f;
	}

	// Token: 0x06006517 RID: 25879 RVA: 0x0025D7F9 File Offset: 0x0025B9F9
	public override string GetSound()
	{
		return "Discover_Space";
	}

	// Token: 0x06006518 RID: 25880 RVA: 0x0025D800 File Offset: 0x0025BA00
	public override string GetMessageBody()
	{
		return MISC.NOTIFICATIONS.DISCOVERED_SPACE.TOOLTIP;
	}

	// Token: 0x06006519 RID: 25881 RVA: 0x0025D80C File Offset: 0x0025BA0C
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.DISCOVERED_SPACE.NAME;
	}

	// Token: 0x0600651A RID: 25882 RVA: 0x0025D818 File Offset: 0x0025BA18
	public override string GetTooltip()
	{
		return null;
	}

	// Token: 0x0600651B RID: 25883 RVA: 0x0025D81B File Offset: 0x0025BA1B
	public override bool IsValid()
	{
		return true;
	}

	// Token: 0x0600651C RID: 25884 RVA: 0x0025D81E File Offset: 0x0025BA1E
	public override void OnClick()
	{
		this.OnDiscoveredSpaceClicked();
	}

	// Token: 0x0600651D RID: 25885 RVA: 0x0025D826 File Offset: 0x0025BA26
	private void OnDiscoveredSpaceClicked()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound(this.GetSound(), false));
		MusicManager.instance.PlaySong("Stinger_Surface", false);
		CameraController.Instance.SetTargetPos(this.cameraFocusPos, 8f, true);
	}

	// Token: 0x04004472 RID: 17522
	[Serialize]
	private Vector3 cameraFocusPos;

	// Token: 0x04004473 RID: 17523
	private const string MUSIC_STINGER = "Stinger_Surface";
}
