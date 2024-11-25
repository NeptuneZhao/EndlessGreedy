using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

// Token: 0x02000583 RID: 1411
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/LoopingSounds")]
public class LoopingSounds : KMonoBehaviour
{
	// Token: 0x060020D8 RID: 8408 RVA: 0x000B7E6C File Offset: 0x000B606C
	public bool IsSoundPlaying(string path)
	{
		using (List<LoopingSounds.LoopingSoundEvent>.Enumerator enumerator = this.loopingSounds.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.asset == path)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060020D9 RID: 8409 RVA: 0x000B7ECC File Offset: 0x000B60CC
	public bool StartSound(string asset, AnimEventManager.EventPlayerData behaviour, EffectorValues noiseValues, bool ignore_pause = false, bool enable_camera_scaled_position = true)
	{
		if (asset == null || asset == "")
		{
			global::Debug.LogWarning("Missing sound");
			return false;
		}
		if (!this.IsSoundPlaying(asset))
		{
			LoopingSounds.LoopingSoundEvent item = new LoopingSounds.LoopingSoundEvent
			{
				asset = asset
			};
			GameObject gameObject = base.gameObject;
			this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
			if (this.objectIsSelectedAndVisible)
			{
				this.sound_pos = SoundEvent.AudioHighlightListenerPosition(base.transform.GetPosition());
				this.vol = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
			}
			else
			{
				this.sound_pos = behaviour.position;
				this.sound_pos.z = 0f;
			}
			item.handle = LoopingSoundManager.Get().Add(asset, this.sound_pos, base.transform, !ignore_pause, true, enable_camera_scaled_position, this.vol, this.objectIsSelectedAndVisible);
			this.loopingSounds.Add(item);
		}
		return true;
	}

	// Token: 0x060020DA RID: 8410 RVA: 0x000B7FB4 File Offset: 0x000B61B4
	public bool StartSound(EventReference event_ref)
	{
		string eventReferencePath = KFMOD.GetEventReferencePath(event_ref);
		return this.StartSound(eventReferencePath);
	}

	// Token: 0x060020DB RID: 8411 RVA: 0x000B7FD0 File Offset: 0x000B61D0
	public bool StartSound(string asset)
	{
		if (asset.IsNullOrWhiteSpace())
		{
			global::Debug.LogWarning("Missing sound");
			return false;
		}
		if (!this.IsSoundPlaying(asset))
		{
			LoopingSounds.LoopingSoundEvent item = new LoopingSounds.LoopingSoundEvent
			{
				asset = asset
			};
			GameObject gameObject = base.gameObject;
			this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
			if (this.objectIsSelectedAndVisible)
			{
				this.sound_pos = SoundEvent.AudioHighlightListenerPosition(base.transform.GetPosition());
				this.vol = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
			}
			else
			{
				this.sound_pos = base.transform.GetPosition();
				this.sound_pos.z = 0f;
			}
			item.handle = LoopingSoundManager.Get().Add(asset, this.sound_pos, base.transform, true, true, true, this.vol, this.objectIsSelectedAndVisible);
			this.loopingSounds.Add(item);
		}
		return true;
	}

	// Token: 0x060020DC RID: 8412 RVA: 0x000B80B0 File Offset: 0x000B62B0
	public bool StartSound(string asset, bool pause_on_game_pause = true, bool enable_culling = true, bool enable_camera_scaled_position = true)
	{
		if (asset.IsNullOrWhiteSpace())
		{
			global::Debug.LogWarning("Missing sound");
			return false;
		}
		if (!this.IsSoundPlaying(asset))
		{
			LoopingSounds.LoopingSoundEvent item = new LoopingSounds.LoopingSoundEvent
			{
				asset = asset
			};
			GameObject gameObject = base.gameObject;
			this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
			if (this.objectIsSelectedAndVisible)
			{
				this.sound_pos = SoundEvent.AudioHighlightListenerPosition(base.transform.GetPosition());
				this.vol = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
			}
			else
			{
				this.sound_pos = base.transform.GetPosition();
				this.sound_pos.z = 0f;
			}
			item.handle = LoopingSoundManager.Get().Add(asset, this.sound_pos, base.transform, pause_on_game_pause, enable_culling, enable_camera_scaled_position, this.vol, this.objectIsSelectedAndVisible);
			this.loopingSounds.Add(item);
		}
		return true;
	}

	// Token: 0x060020DD RID: 8413 RVA: 0x000B8190 File Offset: 0x000B6390
	public void UpdateVelocity(string asset, Vector2 value)
	{
		foreach (LoopingSounds.LoopingSoundEvent loopingSoundEvent in this.loopingSounds)
		{
			if (loopingSoundEvent.asset == asset)
			{
				LoopingSoundManager.Get().UpdateVelocity(loopingSoundEvent.handle, value);
				break;
			}
		}
	}

	// Token: 0x060020DE RID: 8414 RVA: 0x000B8200 File Offset: 0x000B6400
	public void UpdateFirstParameter(string asset, HashedString parameter, float value)
	{
		foreach (LoopingSounds.LoopingSoundEvent loopingSoundEvent in this.loopingSounds)
		{
			if (loopingSoundEvent.asset == asset)
			{
				LoopingSoundManager.Get().UpdateFirstParameter(loopingSoundEvent.handle, parameter, value);
				break;
			}
		}
	}

	// Token: 0x060020DF RID: 8415 RVA: 0x000B8270 File Offset: 0x000B6470
	public void UpdateSecondParameter(string asset, HashedString parameter, float value)
	{
		foreach (LoopingSounds.LoopingSoundEvent loopingSoundEvent in this.loopingSounds)
		{
			if (loopingSoundEvent.asset == asset)
			{
				LoopingSoundManager.Get().UpdateSecondParameter(loopingSoundEvent.handle, parameter, value);
				break;
			}
		}
	}

	// Token: 0x060020E0 RID: 8416 RVA: 0x000B82E0 File Offset: 0x000B64E0
	private void StopSoundAtIndex(int i)
	{
		LoopingSoundManager.StopSound(this.loopingSounds[i].handle);
	}

	// Token: 0x060020E1 RID: 8417 RVA: 0x000B82F8 File Offset: 0x000B64F8
	public void StopSound(EventReference event_ref)
	{
		string eventReferencePath = KFMOD.GetEventReferencePath(event_ref);
		this.StopSound(eventReferencePath);
	}

	// Token: 0x060020E2 RID: 8418 RVA: 0x000B8314 File Offset: 0x000B6514
	public void StopSound(string asset)
	{
		for (int i = 0; i < this.loopingSounds.Count; i++)
		{
			if (this.loopingSounds[i].asset == asset)
			{
				this.StopSoundAtIndex(i);
				this.loopingSounds.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x060020E3 RID: 8419 RVA: 0x000B8364 File Offset: 0x000B6564
	public void PauseSound(string asset, bool paused)
	{
		for (int i = 0; i < this.loopingSounds.Count; i++)
		{
			if (this.loopingSounds[i].asset == asset)
			{
				LoopingSoundManager.PauseSound(this.loopingSounds[i].handle, paused);
				return;
			}
		}
	}

	// Token: 0x060020E4 RID: 8420 RVA: 0x000B83B8 File Offset: 0x000B65B8
	public void StopAllSounds()
	{
		for (int i = 0; i < this.loopingSounds.Count; i++)
		{
			this.StopSoundAtIndex(i);
		}
		this.loopingSounds.Clear();
	}

	// Token: 0x060020E5 RID: 8421 RVA: 0x000B83ED File Offset: 0x000B65ED
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.StopAllSounds();
	}

	// Token: 0x060020E6 RID: 8422 RVA: 0x000B83FC File Offset: 0x000B65FC
	public void SetParameter(EventReference event_ref, HashedString parameter, float value)
	{
		string eventReferencePath = KFMOD.GetEventReferencePath(event_ref);
		this.SetParameter(eventReferencePath, parameter, value);
	}

	// Token: 0x060020E7 RID: 8423 RVA: 0x000B841C File Offset: 0x000B661C
	public void SetParameter(string path, HashedString parameter, float value)
	{
		foreach (LoopingSounds.LoopingSoundEvent loopingSoundEvent in this.loopingSounds)
		{
			if (loopingSoundEvent.asset == path)
			{
				LoopingSoundManager.Get().UpdateFirstParameter(loopingSoundEvent.handle, parameter, value);
				break;
			}
		}
	}

	// Token: 0x060020E8 RID: 8424 RVA: 0x000B848C File Offset: 0x000B668C
	public void PlayEvent(GameSoundEvents.Event ev)
	{
		if (AudioDebug.Get().debugGameEventSounds)
		{
			string str = "GameSoundEvent: ";
			HashedString name = ev.Name;
			global::Debug.Log(str + name.ToString());
		}
		List<AnimEvent> events = GameAudioSheets.Get().GetEvents(ev.Name);
		if (events == null)
		{
			return;
		}
		Vector2 v = base.transform.GetPosition();
		for (int i = 0; i < events.Count; i++)
		{
			SoundEvent soundEvent = events[i] as SoundEvent;
			if (soundEvent == null || soundEvent.sound == null)
			{
				return;
			}
			if (CameraController.Instance.IsAudibleSound(v, soundEvent.sound))
			{
				if (AudioDebug.Get().debugGameEventSounds)
				{
					global::Debug.Log("GameSound: " + soundEvent.sound);
				}
				float num = 0f;
				if (this.lastTimePlayed.TryGetValue(soundEvent.soundHash, out num))
				{
					if (Time.time - num > soundEvent.minInterval)
					{
						SoundEvent.PlayOneShot(soundEvent.sound, v, 1f);
					}
				}
				else
				{
					SoundEvent.PlayOneShot(soundEvent.sound, v, 1f);
				}
				this.lastTimePlayed[soundEvent.soundHash] = Time.time;
			}
		}
	}

	// Token: 0x060020E9 RID: 8425 RVA: 0x000B85DC File Offset: 0x000B67DC
	public void UpdateObjectSelection(bool selected)
	{
		GameObject gameObject = base.gameObject;
		if (selected && gameObject != null && CameraController.Instance.IsVisiblePos(gameObject.transform.position))
		{
			this.objectIsSelectedAndVisible = true;
			this.sound_pos = SoundEvent.AudioHighlightListenerPosition(this.sound_pos);
			this.vol = 1f;
		}
		else
		{
			this.objectIsSelectedAndVisible = false;
			this.sound_pos = base.transform.GetPosition();
			this.sound_pos.z = 0f;
			this.vol = 1f;
		}
		for (int i = 0; i < this.loopingSounds.Count; i++)
		{
			LoopingSoundManager.Get().UpdateObjectSelection(this.loopingSounds[i].handle, this.sound_pos, this.vol, this.objectIsSelectedAndVisible);
		}
	}

	// Token: 0x04001264 RID: 4708
	private List<LoopingSounds.LoopingSoundEvent> loopingSounds = new List<LoopingSounds.LoopingSoundEvent>();

	// Token: 0x04001265 RID: 4709
	private Dictionary<HashedString, float> lastTimePlayed = new Dictionary<HashedString, float>();

	// Token: 0x04001266 RID: 4710
	[SerializeField]
	public bool updatePosition;

	// Token: 0x04001267 RID: 4711
	public float vol = 1f;

	// Token: 0x04001268 RID: 4712
	public bool objectIsSelectedAndVisible;

	// Token: 0x04001269 RID: 4713
	public Vector3 sound_pos;

	// Token: 0x0200137C RID: 4988
	private struct LoopingSoundEvent
	{
		// Token: 0x040066D3 RID: 26323
		public string asset;

		// Token: 0x040066D4 RID: 26324
		public HandleVector<int>.Handle handle;
	}
}
