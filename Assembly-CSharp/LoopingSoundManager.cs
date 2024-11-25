using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000582 RID: 1410
[AddComponentMenu("KMonoBehaviour/scripts/LoopingSoundManager")]
public class LoopingSoundManager : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x060020C3 RID: 8387 RVA: 0x000B71FE File Offset: 0x000B53FE
	public static void DestroyInstance()
	{
		LoopingSoundManager.instance = null;
	}

	// Token: 0x060020C4 RID: 8388 RVA: 0x000B7206 File Offset: 0x000B5406
	protected override void OnPrefabInit()
	{
		LoopingSoundManager.instance = this;
		this.CollectParameterUpdaters();
	}

	// Token: 0x060020C5 RID: 8389 RVA: 0x000B7214 File Offset: 0x000B5414
	protected override void OnSpawn()
	{
		if (SpeedControlScreen.Instance != null && Game.Instance != null)
		{
			Game.Instance.Subscribe(-1788536802, new Action<object>(LoopingSoundManager.instance.OnPauseChanged));
		}
		Game.Instance.Subscribe(1983128072, delegate(object worlds)
		{
			this.OnActiveWorldChanged();
		});
	}

	// Token: 0x060020C6 RID: 8390 RVA: 0x000B7277 File Offset: 0x000B5477
	private void OnActiveWorldChanged()
	{
		this.StopAllSounds();
	}

	// Token: 0x060020C7 RID: 8391 RVA: 0x000B7280 File Offset: 0x000B5480
	private void CollectParameterUpdaters()
	{
		foreach (Type type in App.GetCurrentDomainTypes())
		{
			if (!type.IsAbstract)
			{
				bool flag = false;
				Type baseType = type.BaseType;
				while (baseType != null)
				{
					if (baseType == typeof(LoopingSoundParameterUpdater))
					{
						flag = true;
						break;
					}
					baseType = baseType.BaseType;
				}
				if (flag)
				{
					LoopingSoundParameterUpdater loopingSoundParameterUpdater = (LoopingSoundParameterUpdater)Activator.CreateInstance(type);
					DebugUtil.Assert(!this.parameterUpdaters.ContainsKey(loopingSoundParameterUpdater.parameter));
					this.parameterUpdaters[loopingSoundParameterUpdater.parameter] = loopingSoundParameterUpdater;
				}
			}
		}
	}

	// Token: 0x060020C8 RID: 8392 RVA: 0x000B7348 File Offset: 0x000B5548
	public void UpdateFirstParameter(HandleVector<int>.Handle handle, HashedString parameter, float value)
	{
		LoopingSoundManager.Sound data = this.sounds.GetData(handle);
		data.firstParameterValue = value;
		data.firstParameter = parameter;
		if (data.IsPlaying)
		{
			data.ev.setParameterByID(this.GetSoundDescription(data.path).GetParameterId(parameter), value, false);
		}
		this.sounds.SetData(handle, data);
	}

	// Token: 0x060020C9 RID: 8393 RVA: 0x000B73AC File Offset: 0x000B55AC
	public void UpdateSecondParameter(HandleVector<int>.Handle handle, HashedString parameter, float value)
	{
		LoopingSoundManager.Sound data = this.sounds.GetData(handle);
		data.secondParameterValue = value;
		data.secondParameter = parameter;
		if (data.IsPlaying)
		{
			data.ev.setParameterByID(this.GetSoundDescription(data.path).GetParameterId(parameter), value, false);
		}
		this.sounds.SetData(handle, data);
	}

	// Token: 0x060020CA RID: 8394 RVA: 0x000B7410 File Offset: 0x000B5610
	public void UpdateObjectSelection(HandleVector<int>.Handle handle, Vector3 sound_pos, float vol, bool objectIsSelectedAndVisible)
	{
		LoopingSoundManager.Sound data = this.sounds.GetData(handle);
		data.pos = sound_pos;
		data.vol = vol;
		data.objectIsSelectedAndVisible = objectIsSelectedAndVisible;
		ATTRIBUTES_3D attributes = sound_pos.To3DAttributes();
		if (data.IsPlaying)
		{
			data.ev.set3DAttributes(attributes);
			data.ev.setVolume(vol);
		}
		this.sounds.SetData(handle, data);
	}

	// Token: 0x060020CB RID: 8395 RVA: 0x000B747C File Offset: 0x000B567C
	public void UpdateVelocity(HandleVector<int>.Handle handle, Vector2 velocity)
	{
		LoopingSoundManager.Sound data = this.sounds.GetData(handle);
		data.velocity = velocity;
		this.sounds.SetData(handle, data);
	}

	// Token: 0x060020CC RID: 8396 RVA: 0x000B74AC File Offset: 0x000B56AC
	public void RenderEveryTick(float dt)
	{
		ListPool<LoopingSoundManager.Sound, LoopingSoundManager>.PooledList pooledList = ListPool<LoopingSoundManager.Sound, LoopingSoundManager>.Allocate();
		ListPool<int, LoopingSoundManager>.PooledList pooledList2 = ListPool<int, LoopingSoundManager>.Allocate();
		ListPool<int, LoopingSoundManager>.PooledList pooledList3 = ListPool<int, LoopingSoundManager>.Allocate();
		List<LoopingSoundManager.Sound> dataList = this.sounds.GetDataList();
		bool flag = Time.timeScale == 0f;
		SoundCuller soundCuller = CameraController.Instance.soundCuller;
		for (int i = 0; i < dataList.Count; i++)
		{
			LoopingSoundManager.Sound sound = dataList[i];
			if (sound.objectIsSelectedAndVisible)
			{
				sound.pos = SoundEvent.AudioHighlightListenerPosition(sound.transform.GetPosition());
				sound.vol = 1f;
			}
			else if (sound.transform != null)
			{
				sound.pos = sound.transform.GetPosition();
				sound.pos.z = 0f;
			}
			if (sound.animController != null)
			{
				Vector3 offset = sound.animController.Offset;
				sound.pos.x = sound.pos.x + offset.x;
				sound.pos.y = sound.pos.y + offset.y;
			}
			bool flag2 = !sound.IsCullingEnabled || (sound.ShouldCameraScalePosition && soundCuller.IsAudible(sound.pos, sound.falloffDistanceSq)) || soundCuller.IsAudibleNoCameraScaling(sound.pos, sound.falloffDistanceSq);
			bool isPlaying = sound.IsPlaying;
			if (flag2)
			{
				pooledList.Add(sound);
				if (!isPlaying)
				{
					SoundDescription soundDescription = this.GetSoundDescription(sound.path);
					sound.ev = KFMOD.CreateInstance(soundDescription.path);
					dataList[i] = sound;
					pooledList2.Add(i);
				}
			}
			else if (isPlaying)
			{
				pooledList3.Add(i);
			}
		}
		foreach (int index in pooledList2)
		{
			LoopingSoundManager.Sound sound2 = dataList[index];
			SoundDescription soundDescription2 = this.GetSoundDescription(sound2.path);
			sound2.ev.setPaused(flag && sound2.ShouldPauseOnGamePaused);
			sound2.pos.z = 0f;
			Vector3 pos = sound2.pos;
			if (sound2.objectIsSelectedAndVisible)
			{
				sound2.pos = SoundEvent.AudioHighlightListenerPosition(sound2.transform.GetPosition());
				sound2.vol = 1f;
			}
			else if (sound2.transform != null)
			{
				sound2.pos = sound2.transform.GetPosition();
			}
			sound2.ev.set3DAttributes(pos.To3DAttributes());
			sound2.ev.setVolume(sound2.vol);
			sound2.ev.start();
			sound2.flags |= LoopingSoundManager.Sound.Flags.PLAYING;
			if (sound2.firstParameter != HashedString.Invalid)
			{
				sound2.ev.setParameterByID(soundDescription2.GetParameterId(sound2.firstParameter), sound2.firstParameterValue, false);
			}
			if (sound2.secondParameter != HashedString.Invalid)
			{
				sound2.ev.setParameterByID(soundDescription2.GetParameterId(sound2.secondParameter), sound2.secondParameterValue, false);
			}
			LoopingSoundParameterUpdater.Sound sound3 = new LoopingSoundParameterUpdater.Sound
			{
				ev = sound2.ev,
				path = sound2.path,
				description = soundDescription2,
				transform = sound2.transform,
				objectIsSelectedAndVisible = false
			};
			foreach (SoundDescription.Parameter parameter in soundDescription2.parameters)
			{
				LoopingSoundParameterUpdater loopingSoundParameterUpdater = null;
				if (this.parameterUpdaters.TryGetValue(parameter.name, out loopingSoundParameterUpdater))
				{
					loopingSoundParameterUpdater.Add(sound3);
				}
			}
			dataList[index] = sound2;
		}
		pooledList2.Recycle();
		foreach (int index2 in pooledList3)
		{
			LoopingSoundManager.Sound sound4 = dataList[index2];
			SoundDescription soundDescription3 = this.GetSoundDescription(sound4.path);
			LoopingSoundParameterUpdater.Sound sound5 = new LoopingSoundParameterUpdater.Sound
			{
				ev = sound4.ev,
				path = sound4.path,
				description = soundDescription3,
				transform = sound4.transform,
				objectIsSelectedAndVisible = false
			};
			foreach (SoundDescription.Parameter parameter2 in soundDescription3.parameters)
			{
				LoopingSoundParameterUpdater loopingSoundParameterUpdater2 = null;
				if (this.parameterUpdaters.TryGetValue(parameter2.name, out loopingSoundParameterUpdater2))
				{
					loopingSoundParameterUpdater2.Remove(sound5);
				}
			}
			if (sound4.ShouldCameraScalePosition)
			{
				sound4.ev.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			else
			{
				sound4.ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			sound4.flags &= ~LoopingSoundManager.Sound.Flags.PLAYING;
			sound4.ev.release();
			dataList[index2] = sound4;
		}
		pooledList3.Recycle();
		float velocityScale = TuningData<LoopingSoundManager.Tuning>.Get().velocityScale;
		foreach (LoopingSoundManager.Sound sound6 in pooledList)
		{
			ATTRIBUTES_3D attributes = SoundEvent.GetCameraScaledPosition(sound6.pos, sound6.objectIsSelectedAndVisible).To3DAttributes();
			attributes.velocity = (sound6.velocity * velocityScale).ToFMODVector();
			EventInstance ev = sound6.ev;
			ev.set3DAttributes(attributes);
		}
		foreach (KeyValuePair<HashedString, LoopingSoundParameterUpdater> keyValuePair in this.parameterUpdaters)
		{
			keyValuePair.Value.Update(dt);
		}
		pooledList.Recycle();
	}

	// Token: 0x060020CD RID: 8397 RVA: 0x000B7AE8 File Offset: 0x000B5CE8
	public static LoopingSoundManager Get()
	{
		return LoopingSoundManager.instance;
	}

	// Token: 0x060020CE RID: 8398 RVA: 0x000B7AF0 File Offset: 0x000B5CF0
	public void StopAllSounds()
	{
		foreach (LoopingSoundManager.Sound sound in this.sounds.GetDataList())
		{
			if (sound.IsPlaying)
			{
				EventInstance ev = sound.ev;
				ev.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				ev = sound.ev;
				ev.release();
			}
		}
	}

	// Token: 0x060020CF RID: 8399 RVA: 0x000B7B6C File Offset: 0x000B5D6C
	private SoundDescription GetSoundDescription(HashedString path)
	{
		return KFMOD.GetSoundEventDescription(path);
	}

	// Token: 0x060020D0 RID: 8400 RVA: 0x000B7B74 File Offset: 0x000B5D74
	public HandleVector<int>.Handle Add(string path, Vector3 pos, Transform transform = null, bool pause_on_game_pause = true, bool enable_culling = true, bool enable_camera_scaled_position = true, float vol = 1f, bool objectIsSelectedAndVisible = false)
	{
		SoundDescription soundEventDescription = KFMOD.GetSoundEventDescription(path);
		LoopingSoundManager.Sound.Flags flags = (LoopingSoundManager.Sound.Flags)0;
		if (pause_on_game_pause)
		{
			flags |= LoopingSoundManager.Sound.Flags.PAUSE_ON_GAME_PAUSED;
		}
		if (enable_culling)
		{
			flags |= LoopingSoundManager.Sound.Flags.ENABLE_CULLING;
		}
		if (enable_camera_scaled_position)
		{
			flags |= LoopingSoundManager.Sound.Flags.ENABLE_CAMERA_SCALED_POSITION;
		}
		KBatchedAnimController animController = null;
		if (transform != null)
		{
			animController = transform.GetComponent<KBatchedAnimController>();
		}
		LoopingSoundManager.Sound initial_data = new LoopingSoundManager.Sound
		{
			transform = transform,
			animController = animController,
			falloffDistanceSq = soundEventDescription.falloffDistanceSq,
			path = path,
			pos = pos,
			flags = flags,
			firstParameter = HashedString.Invalid,
			secondParameter = HashedString.Invalid,
			vol = vol,
			objectIsSelectedAndVisible = objectIsSelectedAndVisible
		};
		return this.sounds.Allocate(initial_data);
	}

	// Token: 0x060020D1 RID: 8401 RVA: 0x000B7C34 File Offset: 0x000B5E34
	public static HandleVector<int>.Handle StartSound(EventReference event_ref, Vector3 pos, bool pause_on_game_pause = true, bool enable_culling = true)
	{
		return LoopingSoundManager.StartSound(KFMOD.GetEventReferencePath(event_ref), pos, pause_on_game_pause, enable_culling);
	}

	// Token: 0x060020D2 RID: 8402 RVA: 0x000B7C44 File Offset: 0x000B5E44
	public static HandleVector<int>.Handle StartSound(string path, Vector3 pos, bool pause_on_game_pause = true, bool enable_culling = true)
	{
		if (string.IsNullOrEmpty(path))
		{
			global::Debug.LogWarning("Missing sound");
			return HandleVector<int>.InvalidHandle;
		}
		return LoopingSoundManager.Get().Add(path, pos, null, pause_on_game_pause, enable_culling, true, 1f, false);
	}

	// Token: 0x060020D3 RID: 8403 RVA: 0x000B7C80 File Offset: 0x000B5E80
	public static void StopSound(HandleVector<int>.Handle handle)
	{
		if (LoopingSoundManager.Get() == null)
		{
			return;
		}
		LoopingSoundManager.Sound data = LoopingSoundManager.Get().sounds.GetData(handle);
		if (data.IsPlaying)
		{
			data.ev.stop(LoopingSoundManager.Get().GameIsPaused ? FMOD.Studio.STOP_MODE.IMMEDIATE : FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			data.ev.release();
			SoundDescription soundEventDescription = KFMOD.GetSoundEventDescription(data.path);
			foreach (SoundDescription.Parameter parameter in soundEventDescription.parameters)
			{
				LoopingSoundParameterUpdater loopingSoundParameterUpdater = null;
				if (LoopingSoundManager.Get().parameterUpdaters.TryGetValue(parameter.name, out loopingSoundParameterUpdater))
				{
					LoopingSoundParameterUpdater.Sound sound = new LoopingSoundParameterUpdater.Sound
					{
						ev = data.ev,
						path = data.path,
						description = soundEventDescription,
						transform = data.transform,
						objectIsSelectedAndVisible = false
					};
					loopingSoundParameterUpdater.Remove(sound);
				}
			}
		}
		LoopingSoundManager.Get().sounds.Free(handle);
	}

	// Token: 0x060020D4 RID: 8404 RVA: 0x000B7D88 File Offset: 0x000B5F88
	public static void PauseSound(HandleVector<int>.Handle handle, bool paused)
	{
		LoopingSoundManager.Sound data = LoopingSoundManager.Get().sounds.GetData(handle);
		if (data.IsPlaying)
		{
			data.ev.setPaused(paused);
		}
	}

	// Token: 0x060020D5 RID: 8405 RVA: 0x000B7DC0 File Offset: 0x000B5FC0
	private void OnPauseChanged(object data)
	{
		bool flag = (bool)data;
		this.GameIsPaused = flag;
		foreach (LoopingSoundManager.Sound sound in this.sounds.GetDataList())
		{
			if (sound.IsPlaying)
			{
				EventInstance ev = sound.ev;
				ev.setPaused(flag && sound.ShouldPauseOnGamePaused);
			}
		}
	}

	// Token: 0x04001260 RID: 4704
	private static LoopingSoundManager instance;

	// Token: 0x04001261 RID: 4705
	private bool GameIsPaused;

	// Token: 0x04001262 RID: 4706
	private Dictionary<HashedString, LoopingSoundParameterUpdater> parameterUpdaters = new Dictionary<HashedString, LoopingSoundParameterUpdater>();

	// Token: 0x04001263 RID: 4707
	private KCompactedVector<LoopingSoundManager.Sound> sounds = new KCompactedVector<LoopingSoundManager.Sound>(0);

	// Token: 0x0200137A RID: 4986
	public class Tuning : TuningData<LoopingSoundManager.Tuning>
	{
		// Token: 0x040066C4 RID: 26308
		public float velocityScale;
	}

	// Token: 0x0200137B RID: 4987
	public struct Sound
	{
		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06008751 RID: 34641 RVA: 0x0032B708 File Offset: 0x00329908
		public bool IsPlaying
		{
			get
			{
				return (this.flags & LoopingSoundManager.Sound.Flags.PLAYING) > (LoopingSoundManager.Sound.Flags)0;
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06008752 RID: 34642 RVA: 0x0032B715 File Offset: 0x00329915
		public bool ShouldPauseOnGamePaused
		{
			get
			{
				return (this.flags & LoopingSoundManager.Sound.Flags.PAUSE_ON_GAME_PAUSED) > (LoopingSoundManager.Sound.Flags)0;
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06008753 RID: 34643 RVA: 0x0032B722 File Offset: 0x00329922
		public bool IsCullingEnabled
		{
			get
			{
				return (this.flags & LoopingSoundManager.Sound.Flags.ENABLE_CULLING) > (LoopingSoundManager.Sound.Flags)0;
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06008754 RID: 34644 RVA: 0x0032B72F File Offset: 0x0032992F
		public bool ShouldCameraScalePosition
		{
			get
			{
				return (this.flags & LoopingSoundManager.Sound.Flags.ENABLE_CAMERA_SCALED_POSITION) > (LoopingSoundManager.Sound.Flags)0;
			}
		}

		// Token: 0x040066C5 RID: 26309
		public EventInstance ev;

		// Token: 0x040066C6 RID: 26310
		public Transform transform;

		// Token: 0x040066C7 RID: 26311
		public KBatchedAnimController animController;

		// Token: 0x040066C8 RID: 26312
		public float falloffDistanceSq;

		// Token: 0x040066C9 RID: 26313
		public HashedString path;

		// Token: 0x040066CA RID: 26314
		public Vector3 pos;

		// Token: 0x040066CB RID: 26315
		public Vector2 velocity;

		// Token: 0x040066CC RID: 26316
		public HashedString firstParameter;

		// Token: 0x040066CD RID: 26317
		public HashedString secondParameter;

		// Token: 0x040066CE RID: 26318
		public float firstParameterValue;

		// Token: 0x040066CF RID: 26319
		public float secondParameterValue;

		// Token: 0x040066D0 RID: 26320
		public float vol;

		// Token: 0x040066D1 RID: 26321
		public bool objectIsSelectedAndVisible;

		// Token: 0x040066D2 RID: 26322
		public LoopingSoundManager.Sound.Flags flags;

		// Token: 0x02002499 RID: 9369
		[Flags]
		public enum Flags
		{
			// Token: 0x0400A243 RID: 41539
			PLAYING = 1,
			// Token: 0x0400A244 RID: 41540
			PAUSE_ON_GAME_PAUSED = 2,
			// Token: 0x0400A245 RID: 41541
			ENABLE_CULLING = 4,
			// Token: 0x0400A246 RID: 41542
			ENABLE_CAMERA_SCALED_POSITION = 8
		}
	}
}
