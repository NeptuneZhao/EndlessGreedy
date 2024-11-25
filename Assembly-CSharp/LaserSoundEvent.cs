using System;

// Token: 0x020004EB RID: 1259
public class LaserSoundEvent : SoundEvent
{
	// Token: 0x06001C09 RID: 7177 RVA: 0x00093843 File Offset: 0x00091A43
	public LaserSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, true, true, min_interval, false)
	{
		base.noiseValues = SoundEventVolumeCache.instance.GetVolume("LaserSoundEvent", sound_name);
	}
}
