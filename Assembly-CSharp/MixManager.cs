using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x0200050F RID: 1295
public class MixManager : MonoBehaviour
{
	// Token: 0x06001CC4 RID: 7364 RVA: 0x00097F4D File Offset: 0x0009614D
	private void Update()
	{
		if (AudioMixer.instance != null && AudioMixer.instance.persistentSnapshotsActive)
		{
			AudioMixer.instance.UpdatePersistentSnapshotParameters();
		}
	}

	// Token: 0x06001CC5 RID: 7365 RVA: 0x00097F6C File Offset: 0x0009616C
	private void OnApplicationFocus(bool hasFocus)
	{
		if (AudioMixer.instance == null || AudioMixerSnapshots.Get() == null)
		{
			return;
		}
		if (!hasFocus && KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().GameNotFocusedSnapshot);
			return;
		}
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().GameNotFocusedSnapshot, STOP_MODE.ALLOWFADEOUT);
	}
}
