using System;
using UnityEngine;

// Token: 0x020004FA RID: 1274
public class UIAnimationVoiceSoundEvent : SoundEvent
{
	// Token: 0x06001C67 RID: 7271 RVA: 0x0009550A File Offset: 0x0009370A
	public UIAnimationVoiceSoundEvent(string file_name, string sound_name, int frame, bool looping) : base(file_name, sound_name, frame, false, looping, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
		this.actualSoundName = sound_name;
	}

	// Token: 0x06001C68 RID: 7272 RVA: 0x00095526 File Offset: 0x00093726
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		this.PlaySound(behaviour);
	}

	// Token: 0x06001C69 RID: 7273 RVA: 0x00095530 File Offset: 0x00093730
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		string soundPath = MinionVoice.ByObject(behaviour.controller).UnwrapOr(MinionVoice.Random(), string.Format("Couldn't find MinionVoice on UI {0}, falling back to random voice", behaviour.controller)).GetSoundPath(this.actualSoundName);
		if (this.actualSoundName.Contains(":"))
		{
			float num = float.Parse(this.actualSoundName.Split(':', StringSplitOptions.None)[1]);
			if ((float)UnityEngine.Random.Range(0, 100) > num)
			{
				return;
			}
		}
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component == null)
			{
				global::Debug.Log(behaviour.name + " (UI Object) is missing LoopingSounds component.");
			}
			else if (!component.StartSound(soundPath, false, false, false))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", soundPath, behaviour.name)
				});
			}
			this.lastPlayedLoopingSoundPath = soundPath;
			return;
		}
		try
		{
			if (SoundListenerController.Instance == null)
			{
				KFMOD.PlayUISound(soundPath);
			}
			else
			{
				KFMOD.PlayOneShot(soundPath, SoundListenerController.Instance.transform.GetPosition(), 1f);
			}
		}
		catch
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"AUDIOERROR: Missing [" + soundPath + "]"
			});
		}
	}

	// Token: 0x06001C6A RID: 7274 RVA: 0x00095674 File Offset: 0x00093874
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null && this.lastPlayedLoopingSoundPath != null)
			{
				component.StopSound(this.lastPlayedLoopingSoundPath);
			}
		}
		this.lastPlayedLoopingSoundPath = null;
	}

	// Token: 0x04000FFD RID: 4093
	private string actualSoundName;

	// Token: 0x04000FFE RID: 4094
	private string lastPlayedLoopingSoundPath;
}
