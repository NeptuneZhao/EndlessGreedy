using System;

// Token: 0x020004ED RID: 1261
public class MouthFlapSoundEvent : SoundEvent
{
	// Token: 0x06001C0C RID: 7180 RVA: 0x000938C8 File Offset: 0x00091AC8
	public MouthFlapSoundEvent(string file_name, string sound_name, int frame, bool is_looping) : base(file_name, sound_name, frame, false, is_looping, (float)SoundEvent.IGNORE_INTERVAL, true)
	{
	}

	// Token: 0x06001C0D RID: 7181 RVA: 0x000938DD File Offset: 0x00091ADD
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		behaviour.controller.GetSMI<SpeechMonitor.Instance>().PlaySpeech(base.name, null);
	}
}
