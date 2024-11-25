using System;

// Token: 0x020004F9 RID: 1273
public class UIAnimationSoundEvent : SoundEvent
{
	// Token: 0x06001C63 RID: 7267 RVA: 0x000953CC File Offset: 0x000935CC
	public UIAnimationSoundEvent(string file_name, string sound_name, int frame, bool looping) : base(file_name, sound_name, frame, true, looping, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
	}

	// Token: 0x06001C64 RID: 7268 RVA: 0x000953E1 File Offset: 0x000935E1
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		this.PlaySound(behaviour);
	}

	// Token: 0x06001C65 RID: 7269 RVA: 0x000953EC File Offset: 0x000935EC
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component == null)
			{
				Debug.Log(behaviour.name + " (UI Object) is missing LoopingSounds component.");
				return;
			}
			if (!component.StartSound(base.sound, false, false, false))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", base.sound, behaviour.name)
				});
				return;
			}
		}
		else
		{
			try
			{
				if (SoundListenerController.Instance == null)
				{
					KFMOD.PlayUISound(base.sound);
				}
				else
				{
					KFMOD.PlayOneShot(base.sound, SoundListenerController.Instance.transform.GetPosition(), 1f);
				}
			}
			catch
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"AUDIOERROR: Missing [" + base.sound + "]"
				});
			}
		}
	}

	// Token: 0x06001C66 RID: 7270 RVA: 0x000954D8 File Offset: 0x000936D8
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(base.sound);
			}
		}
	}
}
