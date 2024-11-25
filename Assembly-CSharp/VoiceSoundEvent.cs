using System;
using FMOD.Studio;
using Klei.AI;
using UnityEngine;

// Token: 0x020004FB RID: 1275
public class VoiceSoundEvent : SoundEvent
{
	// Token: 0x06001C6B RID: 7275 RVA: 0x000956B5 File Offset: 0x000938B5
	public VoiceSoundEvent(string file_name, string sound_name, int frame, bool is_looping) : base(file_name, sound_name, frame, false, is_looping, (float)SoundEvent.IGNORE_INTERVAL, true)
	{
		base.noiseValues = SoundEventVolumeCache.instance.GetVolume("VoiceSoundEvent", sound_name);
	}

	// Token: 0x06001C6C RID: 7276 RVA: 0x000956EB File Offset: 0x000938EB
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		VoiceSoundEvent.PlayVoice(base.name, behaviour.controller, this.intervalBetweenSpeaking, base.looping, false);
	}

	// Token: 0x06001C6D RID: 7277 RVA: 0x0009570C File Offset: 0x0009390C
	public static EventInstance PlayVoice(string name, KBatchedAnimController controller, float interval_between_speaking, bool looping, bool objectIsSelectedAndVisible = false)
	{
		EventInstance eventInstance = default(EventInstance);
		MinionIdentity component = controller.GetComponent<MinionIdentity>();
		if (component == null || (name.Contains("state") && Time.time - component.timeLastSpoke < interval_between_speaking))
		{
			return eventInstance;
		}
		bool flag = component.model == BionicMinionConfig.MODEL;
		if (name.Contains(":"))
		{
			float num = float.Parse(name.Split(':', StringSplitOptions.None)[1]);
			if ((float)UnityEngine.Random.Range(0, 100) > num)
			{
				return eventInstance;
			}
		}
		WorkerBase component2 = controller.GetComponent<WorkerBase>();
		string assetName = VoiceSoundEvent.GetAssetName(name, component2);
		StaminaMonitor.Instance smi = component2.GetSMI<StaminaMonitor.Instance>();
		if (!name.Contains("sleep_") && smi != null && smi.IsSleeping())
		{
			return eventInstance;
		}
		Vector3 vector = component2.transform.GetPosition();
		vector.z = 0f;
		if (SoundEvent.ObjectIsSelectedAndVisible(controller.gameObject))
		{
			vector = SoundEvent.AudioHighlightListenerPosition(vector);
		}
		string sound = GlobalAssets.GetSound(assetName, true);
		if (!SoundEvent.ShouldPlaySound(controller, sound, looping, false))
		{
			return eventInstance;
		}
		if (sound != null)
		{
			if (looping)
			{
				LoopingSounds component3 = controller.GetComponent<LoopingSounds>();
				if (component3 == null)
				{
					global::Debug.Log(controller.name + " is missing LoopingSounds component. ");
				}
				else if (!component3.StartSound(sound))
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", sound, controller.name)
					});
				}
				else
				{
					component3.UpdateFirstParameter(sound, "isBionic", (float)(flag ? 1 : 0));
				}
			}
			else
			{
				eventInstance = SoundEvent.BeginOneShot(sound, vector, 1f, false);
				eventInstance.setParameterByName("isBionic", (float)(flag ? 1 : 0), false);
				if (sound.Contains("sleep_") && controller.GetComponent<Traits>().HasTrait("Snorer"))
				{
					eventInstance.setParameterByName("snoring", 1f, false);
				}
				SoundEvent.EndOneShot(eventInstance);
				component.timeLastSpoke = Time.time;
			}
		}
		else if (AudioDebug.Get().debugVoiceSounds)
		{
			global::Debug.LogWarning("Missing voice sound: " + assetName);
		}
		return eventInstance;
	}

	// Token: 0x06001C6E RID: 7278 RVA: 0x0009591C File Offset: 0x00093B1C
	private static string GetAssetName(string name, Component cmp)
	{
		string b = "F01";
		if (cmp != null)
		{
			MinionIdentity component = cmp.GetComponent<MinionIdentity>();
			if (component != null)
			{
				b = component.GetVoiceId();
			}
		}
		string d = name;
		if (name.Contains(":"))
		{
			d = name.Split(':', StringSplitOptions.None)[0];
		}
		return StringFormatter.Combine("DupVoc_", b, "_", d);
	}

	// Token: 0x06001C6F RID: 7279 RVA: 0x0009597C File Offset: 0x00093B7C
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				string sound = GlobalAssets.GetSound(VoiceSoundEvent.GetAssetName(base.name, component), true);
				component.StopSound(sound);
			}
		}
	}

	// Token: 0x04000FFF RID: 4095
	public static float locomotionSoundProb = 50f;

	// Token: 0x04001000 RID: 4096
	public float timeLastSpoke;

	// Token: 0x04001001 RID: 4097
	public float intervalBetweenSpeaking = 10f;
}
