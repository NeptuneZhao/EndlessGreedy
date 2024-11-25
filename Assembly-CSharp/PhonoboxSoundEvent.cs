using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x020004EF RID: 1263
public class PhonoboxSoundEvent : SoundEvent
{
	// Token: 0x06001C11 RID: 7185 RVA: 0x000939DC File Offset: 0x00091BDC
	public PhonoboxSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, true, true, min_interval, false)
	{
	}

	// Token: 0x06001C12 RID: 7186 RVA: 0x000939EC File Offset: 0x00091BEC
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		Vector3 position = behaviour.position;
		position.z = 0f;
		AudioDebug audioDebug = AudioDebug.Get();
		if (audioDebug != null && audioDebug.debugSoundEvents)
		{
			string[] array = new string[7];
			array[0] = behaviour.name;
			array[1] = ", ";
			array[2] = base.sound;
			array[3] = ", ";
			array[4] = base.frame.ToString();
			array[5] = ", ";
			int num = 6;
			Vector3 vector = position;
			array[num] = vector.ToString();
			global::Debug.Log(string.Concat(array));
		}
		try
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component == null)
			{
				global::Debug.Log(behaviour.name + " is missing LoopingSounds component. ");
			}
			else if (!component.IsSoundPlaying(base.sound))
			{
				if (component.StartSound(base.sound, behaviour, base.noiseValues, base.ignorePause, true))
				{
					EventDescription eventDescription = RuntimeManager.GetEventDescription(base.sound);
					PARAMETER_DESCRIPTION parameter_DESCRIPTION;
					eventDescription.getParameterDescriptionByName("jukeboxSong", out parameter_DESCRIPTION);
					int num2 = (int)parameter_DESCRIPTION.maximum;
					PARAMETER_DESCRIPTION parameter_DESCRIPTION2;
					eventDescription.getParameterDescriptionByName("jukeboxPitch", out parameter_DESCRIPTION2);
					int num3 = (int)parameter_DESCRIPTION2.maximum;
					this.song = UnityEngine.Random.Range(0, num2 + 1);
					this.pitch = UnityEngine.Random.Range(0, num3 + 1);
					component.UpdateFirstParameter(base.sound, "jukeboxSong", (float)this.song);
					component.UpdateSecondParameter(base.sound, "jukeboxPitch", (float)this.pitch);
				}
				else
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", base.sound, behaviour.name)
					});
				}
			}
		}
		catch (Exception ex)
		{
			string text = string.Format(("Error trying to trigger sound [{0}] in behaviour [{1}] [{2}]\n{3}" + base.sound != null) ? base.sound.ToString() : "null", behaviour.GetType().ToString(), ex.Message, ex.StackTrace);
			global::Debug.LogError(text);
			throw new ArgumentException(text, ex);
		}
	}

	// Token: 0x04000FDF RID: 4063
	private const string SOUND_PARAM_SONG = "jukeboxSong";

	// Token: 0x04000FE0 RID: 4064
	private const string SOUND_PARAM_PITCH = "jukeboxPitch";

	// Token: 0x04000FE1 RID: 4065
	private int song;

	// Token: 0x04000FE2 RID: 4066
	private int pitch;
}
