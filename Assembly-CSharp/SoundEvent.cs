using System;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x020004F4 RID: 1268
[DebuggerDisplay("{Name}")]
public class SoundEvent : AnimEvent
{
	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x06001C1F RID: 7199 RVA: 0x0009418B File Offset: 0x0009238B
	// (set) Token: 0x06001C20 RID: 7200 RVA: 0x00094193 File Offset: 0x00092393
	public string sound { get; private set; }

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x06001C21 RID: 7201 RVA: 0x0009419C File Offset: 0x0009239C
	// (set) Token: 0x06001C22 RID: 7202 RVA: 0x000941A4 File Offset: 0x000923A4
	public HashedString soundHash { get; private set; }

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x06001C23 RID: 7203 RVA: 0x000941AD File Offset: 0x000923AD
	// (set) Token: 0x06001C24 RID: 7204 RVA: 0x000941B5 File Offset: 0x000923B5
	public bool looping { get; private set; }

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x06001C25 RID: 7205 RVA: 0x000941BE File Offset: 0x000923BE
	// (set) Token: 0x06001C26 RID: 7206 RVA: 0x000941C6 File Offset: 0x000923C6
	public bool ignorePause { get; set; }

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x06001C27 RID: 7207 RVA: 0x000941CF File Offset: 0x000923CF
	// (set) Token: 0x06001C28 RID: 7208 RVA: 0x000941D7 File Offset: 0x000923D7
	public bool shouldCameraScalePosition { get; set; }

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x06001C29 RID: 7209 RVA: 0x000941E0 File Offset: 0x000923E0
	// (set) Token: 0x06001C2A RID: 7210 RVA: 0x000941E8 File Offset: 0x000923E8
	public float minInterval { get; private set; }

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06001C2B RID: 7211 RVA: 0x000941F1 File Offset: 0x000923F1
	// (set) Token: 0x06001C2C RID: 7212 RVA: 0x000941F9 File Offset: 0x000923F9
	public bool objectIsSelectedAndVisible { get; set; }

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06001C2D RID: 7213 RVA: 0x00094202 File Offset: 0x00092402
	// (set) Token: 0x06001C2E RID: 7214 RVA: 0x0009420A File Offset: 0x0009240A
	public EffectorValues noiseValues { get; set; }

	// Token: 0x06001C2F RID: 7215 RVA: 0x00094213 File Offset: 0x00092413
	public SoundEvent()
	{
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x0009421C File Offset: 0x0009241C
	public SoundEvent(string file_name, string sound_name, int frame, bool do_load, bool is_looping, float min_interval, bool is_dynamic) : base(file_name, sound_name, frame)
	{
		this.shouldCameraScalePosition = true;
		if (do_load)
		{
			this.sound = GlobalAssets.GetSound(sound_name, false);
			this.soundHash = new HashedString(this.sound);
			string.IsNullOrEmpty(this.sound);
		}
		this.minInterval = min_interval;
		this.looping = is_looping;
		this.isDynamic = is_dynamic;
		this.noiseValues = SoundEventVolumeCache.instance.GetVolume(file_name, sound_name);
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x00094291 File Offset: 0x00092491
	public static bool ObjectIsSelectedAndVisible(GameObject go)
	{
		return false;
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x00094294 File Offset: 0x00092494
	public static Vector3 AudioHighlightListenerPosition(Vector3 sound_pos)
	{
		Vector3 position = SoundListenerController.Instance.transform.position;
		float x = 1f * sound_pos.x + 0f * position.x;
		float y = 1f * sound_pos.y + 0f * position.y;
		float z = 0f * position.z;
		return new Vector3(x, y, z);
	}

	// Token: 0x06001C33 RID: 7219 RVA: 0x000942FC File Offset: 0x000924FC
	public static float GetVolume(bool objectIsSelectedAndVisible)
	{
		float result = 1f;
		if (objectIsSelectedAndVisible)
		{
			result = 1f;
		}
		return result;
	}

	// Token: 0x06001C34 RID: 7220 RVA: 0x00094319 File Offset: 0x00092519
	public static bool ShouldPlaySound(KBatchedAnimController controller, string sound, bool is_looping, bool is_dynamic)
	{
		return SoundEvent.ShouldPlaySound(controller, sound, sound, is_looping, is_dynamic);
	}

	// Token: 0x06001C35 RID: 7221 RVA: 0x0009432C File Offset: 0x0009252C
	public static bool ShouldPlaySound(KBatchedAnimController controller, string sound, HashedString soundHash, bool is_looping, bool is_dynamic)
	{
		CameraController instance = CameraController.Instance;
		if (instance == null)
		{
			return true;
		}
		Vector3 position = controller.transform.GetPosition();
		Vector3 offset = controller.Offset;
		position.x += offset.x;
		position.y += offset.y;
		if (!SoundCuller.IsAudibleWorld(position))
		{
			return false;
		}
		SpeedControlScreen instance2 = SpeedControlScreen.Instance;
		if (is_dynamic)
		{
			return (!(instance2 != null) || !instance2.IsPaused) && instance.IsAudibleSound(position);
		}
		if (sound == null || SoundEvent.IsLowPrioritySound(sound))
		{
			return false;
		}
		if (!instance.IsAudibleSound(position, soundHash))
		{
			if (!is_looping && !GlobalAssets.IsHighPriority(sound))
			{
				return false;
			}
		}
		else if (instance2 != null && instance2.IsPaused)
		{
			return false;
		}
		return true;
	}

	// Token: 0x06001C36 RID: 7222 RVA: 0x000943F8 File Offset: 0x000925F8
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		GameObject gameObject = behaviour.controller.gameObject;
		this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
		if (this.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, this.sound, this.soundHash, this.looping, this.isDynamic))
		{
			this.PlaySound(behaviour);
		}
	}

	// Token: 0x06001C37 RID: 7223 RVA: 0x00094454 File Offset: 0x00092654
	protected void PlaySound(AnimEventManager.EventPlayerData behaviour, string sound)
	{
		Vector3 vector = behaviour.controller.transform.GetPosition();
		vector.z = 0f;
		if (SoundEvent.ObjectIsSelectedAndVisible(behaviour.controller.gameObject))
		{
			vector = SoundEvent.AudioHighlightListenerPosition(vector);
		}
		KBatchedAnimController controller = behaviour.controller;
		if (controller != null)
		{
			Vector3 offset = controller.Offset;
			vector.x += offset.x;
			vector.y += offset.y;
		}
		AudioDebug audioDebug = AudioDebug.Get();
		if (audioDebug != null && audioDebug.debugSoundEvents)
		{
			string[] array = new string[7];
			array[0] = behaviour.name;
			array[1] = ", ";
			array[2] = sound;
			array[3] = ", ";
			array[4] = base.frame.ToString();
			array[5] = ", ";
			int num = 6;
			Vector3 vector2 = vector;
			array[num] = vector2.ToString();
			global::Debug.Log(string.Concat(array));
		}
		try
		{
			if (this.looping)
			{
				LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
				if (component == null)
				{
					global::Debug.Log(behaviour.name + " is missing LoopingSounds component. ");
				}
				else if (!component.StartSound(sound, behaviour, this.noiseValues, this.ignorePause, this.shouldCameraScalePosition))
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", sound, behaviour.name)
					});
				}
			}
			else if (!SoundEvent.PlayOneShot(sound, behaviour, this.noiseValues, SoundEvent.GetVolume(this.objectIsSelectedAndVisible), this.objectIsSelectedAndVisible))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", sound, behaviour.name)
				});
			}
		}
		catch (Exception ex)
		{
			string text = string.Format(("Error trying to trigger sound [{0}] in behaviour [{1}] [{2}]\n{3}" + sound != null) ? sound.ToString() : "null", behaviour.GetType().ToString(), ex.Message, ex.StackTrace);
			global::Debug.LogError(text);
			throw new ArgumentException(text, ex);
		}
	}

	// Token: 0x06001C38 RID: 7224 RVA: 0x00094654 File Offset: 0x00092854
	public virtual void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		this.PlaySound(behaviour, this.sound);
	}

	// Token: 0x06001C39 RID: 7225 RVA: 0x00094664 File Offset: 0x00092864
	public static Vector3 GetCameraScaledPosition(Vector3 pos, bool objectIsSelectedAndVisible = false)
	{
		Vector3 result = Vector3.zero;
		if (CameraController.Instance != null)
		{
			result = CameraController.Instance.GetVerticallyScaledPosition(pos, objectIsSelectedAndVisible);
		}
		return result;
	}

	// Token: 0x06001C3A RID: 7226 RVA: 0x00094692 File Offset: 0x00092892
	public static FMOD.Studio.EventInstance BeginOneShot(EventReference event_ref, Vector3 pos, float volume = 1f, bool objectIsSelectedAndVisible = false)
	{
		return KFMOD.BeginOneShot(event_ref, SoundEvent.GetCameraScaledPosition(pos, objectIsSelectedAndVisible), volume);
	}

	// Token: 0x06001C3B RID: 7227 RVA: 0x000946A2 File Offset: 0x000928A2
	public static FMOD.Studio.EventInstance BeginOneShot(string ev, Vector3 pos, float volume = 1f, bool objectIsSelectedAndVisible = false)
	{
		return SoundEvent.BeginOneShot(RuntimeManager.PathToEventReference(ev), pos, volume, false);
	}

	// Token: 0x06001C3C RID: 7228 RVA: 0x000946B2 File Offset: 0x000928B2
	public static bool EndOneShot(FMOD.Studio.EventInstance instance)
	{
		return KFMOD.EndOneShot(instance);
	}

	// Token: 0x06001C3D RID: 7229 RVA: 0x000946BC File Offset: 0x000928BC
	public static bool PlayOneShot(EventReference event_ref, Vector3 sound_pos, float volume = 1f)
	{
		bool result = false;
		if (!event_ref.IsNull)
		{
			FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(event_ref, sound_pos, volume, false);
			if (instance.isValid())
			{
				result = SoundEvent.EndOneShot(instance);
			}
		}
		return result;
	}

	// Token: 0x06001C3E RID: 7230 RVA: 0x000946EF File Offset: 0x000928EF
	public static bool PlayOneShot(string sound, Vector3 sound_pos, float volume = 1f)
	{
		return SoundEvent.PlayOneShot(RuntimeManager.PathToEventReference(sound), sound_pos, volume);
	}

	// Token: 0x06001C3F RID: 7231 RVA: 0x00094700 File Offset: 0x00092900
	public static bool PlayOneShot(string sound, AnimEventManager.EventPlayerData behaviour, EffectorValues noiseValues, float volume = 1f, bool objectIsSelectedAndVisible = false)
	{
		bool result = false;
		if (!string.IsNullOrEmpty(sound))
		{
			Vector3 vector = behaviour.controller.transform.GetPosition();
			vector.z = 0f;
			if (objectIsSelectedAndVisible)
			{
				vector = SoundEvent.AudioHighlightListenerPosition(vector);
			}
			FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(sound, vector, volume, false);
			if (instance.isValid())
			{
				result = SoundEvent.EndOneShot(instance);
			}
		}
		return result;
	}

	// Token: 0x06001C40 RID: 7232 RVA: 0x0009475C File Offset: 0x0009295C
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (this.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(this.sound);
			}
		}
	}

	// Token: 0x06001C41 RID: 7233 RVA: 0x0009478E File Offset: 0x0009298E
	protected static bool IsLowPrioritySound(string sound)
	{
		return sound != null && Camera.main != null && Camera.main.orthographicSize > AudioMixer.LOW_PRIORITY_CUTOFF_DISTANCE && !AudioMixer.instance.activeNIS && GlobalAssets.IsLowPriority(sound);
	}

	// Token: 0x06001C42 RID: 7234 RVA: 0x000947C8 File Offset: 0x000929C8
	protected void PrintSoundDebug(string anim_name, string sound, string sound_name, Vector3 sound_pos)
	{
		if (sound != null)
		{
			string[] array = new string[7];
			array[0] = anim_name;
			array[1] = ", ";
			array[2] = sound_name;
			array[3] = ", ";
			array[4] = base.frame.ToString();
			array[5] = ", ";
			int num = 6;
			Vector3 vector = sound_pos;
			array[num] = vector.ToString();
			global::Debug.Log(string.Concat(array));
			return;
		}
		global::Debug.Log("Missing sound: " + anim_name + ", " + sound_name);
	}

	// Token: 0x04000FE8 RID: 4072
	public static int IGNORE_INTERVAL = -1;

	// Token: 0x04000FF1 RID: 4081
	protected bool isDynamic;
}
