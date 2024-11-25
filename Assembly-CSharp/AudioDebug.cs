using System;
using UnityEngine;

// Token: 0x020004FD RID: 1277
[AddComponentMenu("KMonoBehaviour/scripts/AudioDebug")]
public class AudioDebug : KMonoBehaviour
{
	// Token: 0x06001C75 RID: 7285 RVA: 0x00095DA4 File Offset: 0x00093FA4
	public static AudioDebug Get()
	{
		return AudioDebug.instance;
	}

	// Token: 0x06001C76 RID: 7286 RVA: 0x00095DAB File Offset: 0x00093FAB
	protected override void OnPrefabInit()
	{
		AudioDebug.instance = this;
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x00095DB3 File Offset: 0x00093FB3
	public void ToggleMusic()
	{
		if (Game.Instance != null)
		{
			Game.Instance.SetMusicEnabled(this.musicEnabled);
		}
		this.musicEnabled = !this.musicEnabled;
	}

	// Token: 0x04001005 RID: 4101
	private static AudioDebug instance;

	// Token: 0x04001006 RID: 4102
	public bool musicEnabled;

	// Token: 0x04001007 RID: 4103
	public bool debugSoundEvents;

	// Token: 0x04001008 RID: 4104
	public bool debugFloorSounds;

	// Token: 0x04001009 RID: 4105
	public bool debugGameEventSounds;

	// Token: 0x0400100A RID: 4106
	public bool debugNotificationSounds;

	// Token: 0x0400100B RID: 4107
	public bool debugVoiceSounds;
}
