using System;

// Token: 0x020004DC RID: 1244
public class CreatureVariationSoundEvent : SoundEvent
{
	// Token: 0x06001ADE RID: 6878 RVA: 0x0008D69F File Offset: 0x0008B89F
	public CreatureVariationSoundEvent(string file_name, string sound_name, int frame, bool do_load, bool is_looping, float min_interval, bool is_dynamic) : base(file_name, sound_name, frame, do_load, is_looping, min_interval, is_dynamic)
	{
	}

	// Token: 0x06001ADF RID: 6879 RVA: 0x0008D6B4 File Offset: 0x0008B8B4
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		string sound = base.sound;
		CreatureBrain component = behaviour.GetComponent<CreatureBrain>();
		if (component != null && !string.IsNullOrEmpty(component.symbolPrefix))
		{
			string sound2 = GlobalAssets.GetSound(StringFormatter.Combine(component.symbolPrefix, base.name), false);
			if (!string.IsNullOrEmpty(sound2))
			{
				sound = sound2;
			}
		}
		base.PlaySound(behaviour, sound);
	}
}
