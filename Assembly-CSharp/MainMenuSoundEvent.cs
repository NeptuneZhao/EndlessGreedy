using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020004EC RID: 1260
public class MainMenuSoundEvent : SoundEvent
{
	// Token: 0x06001C0A RID: 7178 RVA: 0x00093869 File Offset: 0x00091A69
	public MainMenuSoundEvent(string file_name, string sound_name, int frame) : base(file_name, sound_name, frame, true, false, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
	}

	// Token: 0x06001C0B RID: 7179 RVA: 0x00093880 File Offset: 0x00091A80
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		EventInstance instance = KFMOD.BeginOneShot(base.sound, Vector3.zero, 1f);
		if (instance.isValid())
		{
			instance.setParameterByName("frame", (float)base.frame, false);
			KFMOD.EndOneShot(instance);
		}
	}
}
