using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020004F1 RID: 1265
[Serializable]
public class RemoteSoundEvent : SoundEvent
{
	// Token: 0x06001C15 RID: 7189 RVA: 0x00093C89 File Offset: 0x00091E89
	public RemoteSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, true, false, min_interval, false)
	{
	}

	// Token: 0x06001C16 RID: 7190 RVA: 0x00093C9C File Offset: 0x00091E9C
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		Vector3 vector = behaviour.position;
		vector.z = 0f;
		if (SoundEvent.ObjectIsSelectedAndVisible(behaviour.controller.gameObject))
		{
			vector = SoundEvent.AudioHighlightListenerPosition(vector);
		}
		Workable workable = behaviour.GetComponent<WorkerBase>().GetWorkable();
		if (workable != null)
		{
			Toggleable component = workable.GetComponent<Toggleable>();
			if (component != null)
			{
				IToggleHandler toggleHandlerForWorker = component.GetToggleHandlerForWorker(behaviour.GetComponent<WorkerBase>());
				float value = 1f;
				if (toggleHandlerForWorker != null && toggleHandlerForWorker.IsHandlerOn())
				{
					value = 0f;
				}
				if (base.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, base.sound, base.soundHash, base.looping, this.isDynamic))
				{
					EventInstance instance = SoundEvent.BeginOneShot(base.sound, vector, SoundEvent.GetVolume(base.objectIsSelectedAndVisible), false);
					instance.setParameterByName("State", value, false);
					SoundEvent.EndOneShot(instance);
				}
			}
		}
	}

	// Token: 0x04000FE3 RID: 4067
	private const string STATE_PARAMETER = "State";
}
