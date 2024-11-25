using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;
using ProcGen;
using UnityEngine;

// Token: 0x020009B6 RID: 2486
[AddComponentMenu("KMonoBehaviour/scripts/MusicManager")]
public class MusicManager : KMonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x17000512 RID: 1298
	// (get) Token: 0x06004830 RID: 18480 RVA: 0x0019D7A9 File Offset: 0x0019B9A9
	public Dictionary<string, MusicManager.SongInfo> SongMap
	{
		get
		{
			return this.songMap;
		}
	}

	// Token: 0x06004831 RID: 18481 RVA: 0x0019D7B4 File Offset: 0x0019B9B4
	public void PlaySong(string song_name, bool canWait = false)
	{
		this.Log("Play: " + song_name);
		if (!AudioDebug.Get().musicEnabled)
		{
			return;
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.songMap.TryGetValue(song_name, out songInfo))
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Unknown song:",
				song_name
			});
			return;
		}
		if (this.activeSongs.ContainsKey(song_name))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Trying to play duplicate song:",
				song_name
			});
			return;
		}
		if (this.activeSongs.Count == 0)
		{
			songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
			if (!songInfo.ev.isValid())
			{
				object[] array = new object[1];
				int num = 0;
				string str = "Failed to find FMOD event [";
				EventReference fmodEvent = songInfo.fmodEvent;
				array[num] = str + fmodEvent.ToString() + "]";
				DebugUtil.LogWarningArgs(array);
			}
			int num2 = (songInfo.numberOfVariations > 0) ? UnityEngine.Random.Range(1, songInfo.numberOfVariations + 1) : -1;
			if (num2 != -1)
			{
				songInfo.ev.setParameterByName("variation", (float)num2, false);
			}
			if (songInfo.dynamic)
			{
				songInfo.ev.setProperty(EVENT_PROPERTY.SCHEDULE_DELAY, 16000f);
				songInfo.ev.setProperty(EVENT_PROPERTY.SCHEDULE_LOOKAHEAD, 48000f);
				this.activeDynamicSong = songInfo;
			}
			songInfo.ev.start();
			this.activeSongs[song_name] = songInfo;
			return;
		}
		List<string> list = new List<string>(this.activeSongs.Keys);
		if (songInfo.interruptsActiveMusic)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (!this.activeSongs[list[i]].interruptsActiveMusic)
				{
					MusicManager.SongInfo songInfo2 = this.activeSongs[list[i]];
					songInfo2.ev.setParameterByName("interrupted_dimmed", 1f, false);
					this.Log("Dimming: " + Assets.GetSimpleSoundEventName(songInfo2.fmodEvent));
					songInfo.songsOnHold.Add(list[i]);
				}
			}
			songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
			if (!songInfo.ev.isValid())
			{
				object[] array2 = new object[1];
				int num3 = 0;
				string str2 = "Failed to find FMOD event [";
				EventReference fmodEvent = songInfo.fmodEvent;
				array2[num3] = str2 + fmodEvent.ToString() + "]";
				DebugUtil.LogWarningArgs(array2);
			}
			songInfo.ev.start();
			songInfo.ev.release();
			this.activeSongs[song_name] = songInfo;
			return;
		}
		int num4 = 0;
		foreach (string key in this.activeSongs.Keys)
		{
			MusicManager.SongInfo songInfo3 = this.activeSongs[key];
			if (!songInfo3.interruptsActiveMusic && songInfo3.priority > num4)
			{
				num4 = songInfo3.priority;
			}
		}
		if (songInfo.priority >= num4)
		{
			for (int j = 0; j < list.Count; j++)
			{
				MusicManager.SongInfo songInfo4 = this.activeSongs[list[j]];
				FMOD.Studio.EventInstance ev = songInfo4.ev;
				if (!songInfo4.interruptsActiveMusic)
				{
					ev.setParameterByName("interrupted_dimmed", 1f, false);
					ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
					this.activeSongs.Remove(list[j]);
					list.Remove(list[j]);
				}
			}
			songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
			if (!songInfo.ev.isValid())
			{
				object[] array3 = new object[1];
				int num5 = 0;
				string str3 = "Failed to find FMOD event [";
				EventReference fmodEvent = songInfo.fmodEvent;
				array3[num5] = str3 + fmodEvent.ToString() + "]";
				DebugUtil.LogWarningArgs(array3);
			}
			int num6 = (songInfo.numberOfVariations > 0) ? UnityEngine.Random.Range(1, songInfo.numberOfVariations + 1) : -1;
			if (num6 != -1)
			{
				songInfo.ev.setParameterByName("variation", (float)num6, false);
			}
			songInfo.ev.start();
			this.activeSongs[song_name] = songInfo;
		}
	}

	// Token: 0x06004832 RID: 18482 RVA: 0x0019DBC4 File Offset: 0x0019BDC4
	public void StopSong(string song_name, bool shouldLog = true, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		if (shouldLog)
		{
			this.Log("Stop: " + song_name);
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.songMap.TryGetValue(song_name, out songInfo))
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Unknown song:",
				song_name
			});
			return;
		}
		if (!this.activeSongs.ContainsKey(song_name))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Trying to stop a song that isn't playing:",
				song_name
			});
			return;
		}
		FMOD.Studio.EventInstance ev = songInfo.ev;
		ev.stop(stopMode);
		ev.release();
		if (songInfo.dynamic)
		{
			this.activeDynamicSong = null;
		}
		if (songInfo.songsOnHold.Count > 0)
		{
			for (int i = 0; i < songInfo.songsOnHold.Count; i++)
			{
				MusicManager.SongInfo songInfo2;
				if (this.activeSongs.TryGetValue(songInfo.songsOnHold[i], out songInfo2) && songInfo2.ev.isValid())
				{
					FMOD.Studio.EventInstance ev2 = songInfo2.ev;
					this.Log("Undimming: " + Assets.GetSimpleSoundEventName(songInfo2.fmodEvent));
					ev2.setParameterByName("interrupted_dimmed", 0f, false);
					songInfo.songsOnHold.Remove(songInfo.songsOnHold[i]);
				}
				else
				{
					songInfo.songsOnHold.Remove(songInfo.songsOnHold[i]);
				}
			}
		}
		this.activeSongs.Remove(song_name);
	}

	// Token: 0x06004833 RID: 18483 RVA: 0x0019DD28 File Offset: 0x0019BF28
	public void KillAllSongs(FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
	{
		this.Log("Kill All Songs");
		if (this.DynamicMusicIsActive())
		{
			this.StopDynamicMusic(true);
		}
		List<string> list = new List<string>(this.activeSongs.Keys);
		for (int i = 0; i < list.Count; i++)
		{
			this.StopSong(list[i], true, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06004834 RID: 18484 RVA: 0x0019DD80 File Offset: 0x0019BF80
	public void SetSongParameter(string song_name, string parameter_name, float parameter_value, bool shouldLog = true)
	{
		if (shouldLog)
		{
			this.Log(string.Format("Set Param {0}: {1}, {2}", song_name, parameter_name, parameter_value));
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.activeSongs.TryGetValue(song_name, out songInfo))
		{
			return;
		}
		FMOD.Studio.EventInstance ev = songInfo.ev;
		if (ev.isValid())
		{
			ev.setParameterByName(parameter_name, parameter_value, false);
		}
	}

	// Token: 0x06004835 RID: 18485 RVA: 0x0019DDD8 File Offset: 0x0019BFD8
	public void SetSongParameter(string song_name, string parameter_name, string parameter_lable, bool shouldLog = true)
	{
		if (shouldLog)
		{
			this.Log(string.Format("Set Param {0}: {1}, {2}", song_name, parameter_name, parameter_lable));
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.activeSongs.TryGetValue(song_name, out songInfo))
		{
			return;
		}
		FMOD.Studio.EventInstance ev = songInfo.ev;
		if (ev.isValid())
		{
			ev.setParameterByNameWithLabel(parameter_name, parameter_lable, false);
		}
	}

	// Token: 0x06004836 RID: 18486 RVA: 0x0019DE2C File Offset: 0x0019C02C
	public bool SongIsPlaying(string song_name)
	{
		MusicManager.SongInfo songInfo = null;
		return this.activeSongs.TryGetValue(song_name, out songInfo) && songInfo.musicPlaybackState != PLAYBACK_STATE.STOPPED;
	}

	// Token: 0x06004837 RID: 18487 RVA: 0x0019DE58 File Offset: 0x0019C058
	private void Update()
	{
		this.ClearFinishedSongs();
		if (this.DynamicMusicIsActive())
		{
			this.SetDynamicMusicZoomLevel();
			this.SetDynamicMusicTimeSinceLastJob();
			if (this.activeDynamicSong.useTimeOfDay)
			{
				this.SetDynamicMusicTimeOfDay();
			}
			if (GameClock.Instance != null && GameClock.Instance.GetCurrentCycleAsPercentage() >= this.duskTimePercentage / 100f)
			{
				this.StopDynamicMusic(false);
			}
		}
	}

	// Token: 0x06004838 RID: 18488 RVA: 0x0019DEC0 File Offset: 0x0019C0C0
	private void ClearFinishedSongs()
	{
		if (this.activeSongs.Count > 0)
		{
			ListPool<string, MusicManager>.PooledList pooledList = ListPool<string, MusicManager>.Allocate();
			foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
			{
				MusicManager.SongInfo value = keyValuePair.Value;
				FMOD.Studio.EventInstance ev = value.ev;
				ev.getPlaybackState(out value.musicPlaybackState);
				if (value.musicPlaybackState == PLAYBACK_STATE.STOPPED || value.musicPlaybackState == PLAYBACK_STATE.STOPPING)
				{
					pooledList.Add(keyValuePair.Key);
					foreach (string song_name in value.songsOnHold)
					{
						this.SetSongParameter(song_name, "interrupted_dimmed", 0f, true);
					}
					value.songsOnHold.Clear();
				}
			}
			foreach (string key in pooledList)
			{
				this.activeSongs.Remove(key);
			}
			pooledList.Recycle();
		}
	}

	// Token: 0x06004839 RID: 18489 RVA: 0x0019E010 File Offset: 0x0019C210
	public void OnEscapeMenu(bool paused)
	{
		foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
		{
			if (keyValuePair.Value != null)
			{
				this.StartFadeToPause(keyValuePair.Value.ev, paused, 0.25f);
			}
		}
	}

	// Token: 0x0600483A RID: 18490 RVA: 0x0019E080 File Offset: 0x0019C280
	public void OnSupplyClosetMenu(bool paused, float fadeTime)
	{
		bool flag = !paused;
		if (!PauseScreen.Instance.IsNullOrDestroyed() && PauseScreen.Instance.IsActive() && flag && MusicManager.instance.SongIsPlaying("Music_ESC_Menu"))
		{
			MusicManager.SongInfo songInfo = this.songMap["Music_ESC_Menu"];
			foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
			{
				if (keyValuePair.Value != null && keyValuePair.Value != songInfo)
				{
					this.StartFadeToPause(keyValuePair.Value.ev, paused, 0.25f);
				}
			}
			this.StartFadeToPause(songInfo.ev, false, 0.25f);
			return;
		}
		foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair2 in this.activeSongs)
		{
			if (keyValuePair2.Value != null)
			{
				this.StartFadeToPause(keyValuePair2.Value.ev, paused, fadeTime);
			}
		}
	}

	// Token: 0x0600483B RID: 18491 RVA: 0x0019E1AC File Offset: 0x0019C3AC
	public void StartFadeToPause(FMOD.Studio.EventInstance inst, bool paused, float fadeTime = 0.25f)
	{
		if (paused)
		{
			base.StartCoroutine(this.FadeToPause(inst, fadeTime));
			return;
		}
		base.StartCoroutine(this.FadeToUnpause(inst, fadeTime));
	}

	// Token: 0x0600483C RID: 18492 RVA: 0x0019E1D0 File Offset: 0x0019C3D0
	private IEnumerator FadeToPause(FMOD.Studio.EventInstance inst, float fadeTime)
	{
		float startVolume;
		float targetVolume;
		inst.getVolume(out startVolume, out targetVolume);
		targetVolume = 0f;
		float lerpTime = 0f;
		while (lerpTime < 1f)
		{
			lerpTime += Time.unscaledDeltaTime / fadeTime;
			float volume = Mathf.Lerp(startVolume, targetVolume, lerpTime);
			inst.setVolume(volume);
			yield return null;
		}
		inst.setPaused(true);
		yield break;
	}

	// Token: 0x0600483D RID: 18493 RVA: 0x0019E1E6 File Offset: 0x0019C3E6
	private IEnumerator FadeToUnpause(FMOD.Studio.EventInstance inst, float fadeTime)
	{
		float startVolume;
		float targetVolume;
		inst.getVolume(out startVolume, out targetVolume);
		targetVolume = 1f;
		float lerpTime = 0f;
		inst.setPaused(false);
		while (lerpTime < 1f)
		{
			lerpTime += Time.unscaledDeltaTime / fadeTime;
			float volume = Mathf.Lerp(startVolume, targetVolume, lerpTime);
			inst.setVolume(volume);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600483E RID: 18494 RVA: 0x0019E1FC File Offset: 0x0019C3FC
	public void WattsonStartDynamicMusic()
	{
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		if (currentClusterLayout != null && currentClusterLayout.clusterAudio != null && !string.IsNullOrWhiteSpace(currentClusterLayout.clusterAudio.musicFirst))
		{
			DebugUtil.Assert(this.fullSongPlaylist.songMap.ContainsKey(currentClusterLayout.clusterAudio.musicFirst), "Attempting to play dlc music that isn't in the fullSongPlaylist");
			this.activePlaylist = this.fullSongPlaylist;
			this.PlayDynamicMusic(currentClusterLayout.clusterAudio.musicFirst);
			return;
		}
		this.PlayDynamicMusic();
	}

	// Token: 0x0600483F RID: 18495 RVA: 0x0019E27C File Offset: 0x0019C47C
	public void PlayDynamicMusic()
	{
		if (this.DynamicMusicIsActive())
		{
			this.Log("Trying to play DynamicMusic when it is already playing.");
			return;
		}
		string nextDynamicSong = this.GetNextDynamicSong();
		this.PlayDynamicMusic(nextDynamicSong);
	}

	// Token: 0x06004840 RID: 18496 RVA: 0x0019E2AC File Offset: 0x0019C4AC
	private void PlayDynamicMusic(string song_name)
	{
		if (song_name == "NONE")
		{
			return;
		}
		this.PlaySong(song_name, false);
		MusicManager.SongInfo songInfo;
		if (this.activeSongs.TryGetValue(song_name, out songInfo))
		{
			this.activeDynamicSong = songInfo;
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot);
			if (SpeedControlScreen.Instance != null && SpeedControlScreen.Instance.IsPaused)
			{
				this.SetDynamicMusicPaused();
			}
			if (OverlayScreen.Instance != null && OverlayScreen.Instance.mode != OverlayModes.None.ID)
			{
				this.SetDynamicMusicOverlayActive();
			}
			this.SetDynamicMusicPlayHook();
			this.SetDynamicMusicKeySigniture();
			string key = "Volume_Music";
			if (KPlayerPrefs.HasKey(key))
			{
				float @float = KPlayerPrefs.GetFloat(key);
				AudioMixer.instance.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "userVolume_Music", @float, true);
			}
			AudioMixer.instance.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "intensity", songInfo.sfxAttenuationPercentage / 100f, true);
			return;
		}
		this.Log("DynamicMusic song " + song_name + " did not start.");
		string text = "";
		foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
		{
			text = text + keyValuePair.Key + ", ";
			global::Debug.Log(text);
		}
		DebugUtil.DevAssert(false, "Song failed to play: " + song_name, null);
	}

	// Token: 0x06004841 RID: 18497 RVA: 0x0019E430 File Offset: 0x0019C630
	public void StopDynamicMusic(bool stopImmediate = false)
	{
		if (this.activeDynamicSong != null)
		{
			FMOD.Studio.STOP_MODE stopMode = stopImmediate ? FMOD.Studio.STOP_MODE.IMMEDIATE : FMOD.Studio.STOP_MODE.ALLOWFADEOUT;
			this.Log("Stop DynamicMusic: " + Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent));
			this.StopSong(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), true, stopMode);
			this.activeDynamicSong = null;
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06004842 RID: 18498 RVA: 0x0019E4A4 File Offset: 0x0019C6A4
	public string GetNextDynamicSong()
	{
		string result = "";
		if (this.alwaysPlayMusic && this.nextMusicType == MusicManager.TypeOfMusic.None)
		{
			while (this.nextMusicType == MusicManager.TypeOfMusic.None)
			{
				this.CycleToNextMusicType();
			}
		}
		switch (this.nextMusicType)
		{
		case MusicManager.TypeOfMusic.DynamicSong:
			result = this.fullSongPlaylist.GetNextSong();
			this.activePlaylist = this.fullSongPlaylist;
			break;
		case MusicManager.TypeOfMusic.MiniSong:
			result = this.miniSongPlaylist.GetNextSong();
			this.activePlaylist = this.miniSongPlaylist;
			break;
		case MusicManager.TypeOfMusic.None:
			result = "NONE";
			this.activePlaylist = null;
			break;
		}
		this.CycleToNextMusicType();
		return result;
	}

	// Token: 0x06004843 RID: 18499 RVA: 0x0019E53C File Offset: 0x0019C73C
	private void CycleToNextMusicType()
	{
		int num = this.musicTypeIterator + 1;
		this.musicTypeIterator = num;
		this.musicTypeIterator = num % this.musicStyleOrder.Length;
		this.nextMusicType = this.musicStyleOrder[this.musicTypeIterator];
	}

	// Token: 0x06004844 RID: 18500 RVA: 0x0019E57C File Offset: 0x0019C77C
	public bool DynamicMusicIsActive()
	{
		return this.activeDynamicSong != null;
	}

	// Token: 0x06004845 RID: 18501 RVA: 0x0019E589 File Offset: 0x0019C789
	public void SetDynamicMusicPaused()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "Paused", 1f, true);
		}
	}

	// Token: 0x06004846 RID: 18502 RVA: 0x0019E5B4 File Offset: 0x0019C7B4
	public void SetDynamicMusicUnpaused()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "Paused", 0f, true);
		}
	}

	// Token: 0x06004847 RID: 18503 RVA: 0x0019E5E0 File Offset: 0x0019C7E0
	public void SetDynamicMusicZoomLevel()
	{
		if (CameraController.Instance != null)
		{
			float parameter_value = 100f - Camera.main.orthographicSize / 20f * 100f;
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "zoomPercentage", parameter_value, false);
		}
	}

	// Token: 0x06004848 RID: 18504 RVA: 0x0019E634 File Offset: 0x0019C834
	public void SetDynamicMusicTimeSinceLastJob()
	{
		this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "secsSinceNewJob", Time.time - Game.Instance.LastTimeWorkStarted, false);
	}

	// Token: 0x06004849 RID: 18505 RVA: 0x0019E664 File Offset: 0x0019C864
	public void SetDynamicMusicTimeOfDay()
	{
		if (this.time >= this.timeOfDayUpdateRate)
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "timeOfDay", GameClock.Instance.GetCurrentCycleAsPercentage(), false);
			this.time = 0f;
		}
		this.time += Time.deltaTime;
	}

	// Token: 0x0600484A RID: 18506 RVA: 0x0019E6C2 File Offset: 0x0019C8C2
	public void SetDynamicMusicOverlayActive()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "overlayActive", 1f, true);
		}
	}

	// Token: 0x0600484B RID: 18507 RVA: 0x0019E6ED File Offset: 0x0019C8ED
	public void SetDynamicMusicOverlayInactive()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "overlayActive", 0f, true);
		}
	}

	// Token: 0x0600484C RID: 18508 RVA: 0x0019E718 File Offset: 0x0019C918
	public void SetDynamicMusicPlayHook()
	{
		if (this.DynamicMusicIsActive())
		{
			string simpleSoundEventName = Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent);
			this.SetSongParameter(simpleSoundEventName, "playHook", this.activeDynamicSong.playHook ? 1f : 0f, true);
			this.activePlaylist.songMap[simpleSoundEventName].playHook = !this.activePlaylist.songMap[simpleSoundEventName].playHook;
		}
	}

	// Token: 0x0600484D RID: 18509 RVA: 0x0019E793 File Offset: 0x0019C993
	public bool ShouldPlayDynamicMusicLoadedGame()
	{
		return GameClock.Instance.GetCurrentCycleAsPercentage() <= this.loadGameCutoffPercentage / 100f;
	}

	// Token: 0x0600484E RID: 18510 RVA: 0x0019E7B0 File Offset: 0x0019C9B0
	public void SetDynamicMusicKeySigniture()
	{
		if (this.DynamicMusicIsActive())
		{
			string simpleSoundEventName = Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent);
			string musicKeySigniture = this.activePlaylist.songMap[simpleSoundEventName].musicKeySigniture;
			float value;
			if (!(musicKeySigniture == "Ab"))
			{
				if (!(musicKeySigniture == "Bb"))
				{
					if (!(musicKeySigniture == "C"))
					{
						if (!(musicKeySigniture == "D"))
						{
							value = 2f;
						}
						else
						{
							value = 3f;
						}
					}
					else
					{
						value = 2f;
					}
				}
				else
				{
					value = 1f;
				}
			}
			else
			{
				value = 0f;
			}
			RuntimeManager.StudioSystem.setParameterByName("MusicInKey", value, false);
		}
	}

	// Token: 0x17000513 RID: 1299
	// (get) Token: 0x0600484F RID: 18511 RVA: 0x0019E861 File Offset: 0x0019CA61
	public static MusicManager instance
	{
		get
		{
			return MusicManager._instance;
		}
	}

	// Token: 0x06004850 RID: 18512 RVA: 0x0019E868 File Offset: 0x0019CA68
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!RuntimeManager.IsInitialized)
		{
			base.enabled = false;
			return;
		}
		if (KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayMusicKey))
		{
			this.alwaysPlayMusic = (KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayMusicKey) == 1);
		}
	}

	// Token: 0x06004851 RID: 18513 RVA: 0x0019E8A2 File Offset: 0x0019CAA2
	protected override void OnPrefabInit()
	{
		MusicManager._instance = this;
		this.ConfigureSongs();
		this.nextMusicType = this.musicStyleOrder[this.musicTypeIterator];
	}

	// Token: 0x06004852 RID: 18514 RVA: 0x0019E8C3 File Offset: 0x0019CAC3
	protected override void OnCleanUp()
	{
		MusicManager._instance = null;
	}

	// Token: 0x06004853 RID: 18515 RVA: 0x0019E8CB File Offset: 0x0019CACB
	private static bool IsValidForDLCContext(string dlcid)
	{
		if (SaveLoader.Instance != null)
		{
			return SaveLoader.Instance.IsDLCActiveForCurrentSave(dlcid);
		}
		return DlcManager.IsContentSubscribed(dlcid);
	}

	// Token: 0x06004854 RID: 18516 RVA: 0x0019E8EC File Offset: 0x0019CAEC
	[ContextMenu("Reload")]
	public void ConfigureSongs()
	{
		this.songMap.Clear();
		this.fullSongPlaylist.Clear();
		this.miniSongPlaylist.Clear();
		foreach (MusicManager.DynamicSong dynamicSong in this.fullSongs)
		{
			if (MusicManager.IsValidForDLCContext(dynamicSong.requiredDlcId))
			{
				string simpleSoundEventName = Assets.GetSimpleSoundEventName(dynamicSong.fmodEvent);
				MusicManager.SongInfo songInfo = new MusicManager.SongInfo();
				songInfo.fmodEvent = dynamicSong.fmodEvent;
				songInfo.requiredDlcId = dynamicSong.requiredDlcId;
				songInfo.priority = 100;
				songInfo.interruptsActiveMusic = false;
				songInfo.dynamic = true;
				songInfo.useTimeOfDay = dynamicSong.useTimeOfDay;
				songInfo.numberOfVariations = dynamicSong.numberOfVariations;
				songInfo.musicKeySigniture = dynamicSong.musicKeySigniture;
				songInfo.sfxAttenuationPercentage = this.dynamicMusicSFXAttenuationPercentage;
				this.songMap[simpleSoundEventName] = songInfo;
				this.fullSongPlaylist.songMap[simpleSoundEventName] = songInfo;
			}
		}
		foreach (MusicManager.Minisong minisong in this.miniSongs)
		{
			if (MusicManager.IsValidForDLCContext(minisong.requiredDlcId))
			{
				string simpleSoundEventName2 = Assets.GetSimpleSoundEventName(minisong.fmodEvent);
				MusicManager.SongInfo songInfo2 = new MusicManager.SongInfo();
				songInfo2.fmodEvent = minisong.fmodEvent;
				songInfo2.requiredDlcId = minisong.requiredDlcId;
				songInfo2.priority = 100;
				songInfo2.interruptsActiveMusic = false;
				songInfo2.dynamic = true;
				songInfo2.useTimeOfDay = false;
				songInfo2.numberOfVariations = 5;
				songInfo2.musicKeySigniture = minisong.musicKeySigniture;
				songInfo2.sfxAttenuationPercentage = this.miniSongSFXAttenuationPercentage;
				this.songMap[simpleSoundEventName2] = songInfo2;
				this.miniSongPlaylist.songMap[simpleSoundEventName2] = songInfo2;
			}
		}
		foreach (MusicManager.Stinger stinger in this.stingers)
		{
			if (MusicManager.IsValidForDLCContext(stinger.requiredDlcId))
			{
				string simpleSoundEventName3 = Assets.GetSimpleSoundEventName(stinger.fmodEvent);
				MusicManager.SongInfo songInfo3 = new MusicManager.SongInfo();
				songInfo3.fmodEvent = stinger.fmodEvent;
				songInfo3.priority = 100;
				songInfo3.interruptsActiveMusic = true;
				songInfo3.dynamic = false;
				songInfo3.useTimeOfDay = false;
				songInfo3.numberOfVariations = 0;
				songInfo3.requiredDlcId = stinger.requiredDlcId;
				this.songMap[simpleSoundEventName3] = songInfo3;
			}
		}
		foreach (MusicManager.MenuSong menuSong in this.menuSongs)
		{
			if (MusicManager.IsValidForDLCContext(menuSong.requiredDlcId))
			{
				string simpleSoundEventName4 = Assets.GetSimpleSoundEventName(menuSong.fmodEvent);
				MusicManager.SongInfo songInfo4 = new MusicManager.SongInfo();
				songInfo4.fmodEvent = menuSong.fmodEvent;
				songInfo4.priority = 100;
				songInfo4.interruptsActiveMusic = true;
				songInfo4.dynamic = false;
				songInfo4.useTimeOfDay = false;
				songInfo4.numberOfVariations = 0;
				songInfo4.requiredDlcId = menuSong.requiredDlcId;
				this.songMap[simpleSoundEventName4] = songInfo4;
			}
		}
		this.fullSongPlaylist.ResetUnplayedSongs();
		this.miniSongPlaylist.ResetUnplayedSongs();
	}

	// Token: 0x06004855 RID: 18517 RVA: 0x0019EBFA File Offset: 0x0019CDFA
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x06004856 RID: 18518 RVA: 0x0019EBFC File Offset: 0x0019CDFC
	public void OnAfterDeserialize()
	{
	}

	// Token: 0x06004857 RID: 18519 RVA: 0x0019EBFE File Offset: 0x0019CDFE
	private void Log(string s)
	{
	}

	// Token: 0x04002F5A RID: 12122
	private const string VARIATION_ID = "variation";

	// Token: 0x04002F5B RID: 12123
	private const string INTERRUPTED_DIMMED_ID = "interrupted_dimmed";

	// Token: 0x04002F5C RID: 12124
	private const string MUSIC_KEY = "MusicInKey";

	// Token: 0x04002F5D RID: 12125
	private const float DYNAMIC_MUSIC_SCHEDULE_DELAY = 16000f;

	// Token: 0x04002F5E RID: 12126
	private const float DYNAMIC_MUSIC_SCHEDULE_LOOKAHEAD = 48000f;

	// Token: 0x04002F5F RID: 12127
	[Header("Song Lists")]
	[Tooltip("Play during the daytime. The mix of the song is affected by the player's input, like pausing the sim, activating an overlay, or zooming in and out.")]
	[SerializeField]
	private MusicManager.DynamicSong[] fullSongs;

	// Token: 0x04002F60 RID: 12128
	[Tooltip("Simple dynamic songs which are more ambient in nature, which play quietly during \"non-music\" days. These are affected by Pause and OverlayActive.")]
	[SerializeField]
	private MusicManager.Minisong[] miniSongs;

	// Token: 0x04002F61 RID: 12129
	[Tooltip("Triggered by in-game events, such as completing research or night-time falling. They will temporarily interrupt a dynamicSong, fading the dynamicSong back in after the stinger is complete.")]
	[SerializeField]
	private MusicManager.Stinger[] stingers;

	// Token: 0x04002F62 RID: 12130
	[Tooltip("Generally songs that don't play during gameplay, while a menu is open. For example, the ESC menu or the Starmap.")]
	[SerializeField]
	private MusicManager.MenuSong[] menuSongs;

	// Token: 0x04002F63 RID: 12131
	private Dictionary<string, MusicManager.SongInfo> songMap = new Dictionary<string, MusicManager.SongInfo>();

	// Token: 0x04002F64 RID: 12132
	public Dictionary<string, MusicManager.SongInfo> activeSongs = new Dictionary<string, MusicManager.SongInfo>();

	// Token: 0x04002F65 RID: 12133
	[Space]
	[Header("Tuning Values")]
	[Tooltip("Just before night-time (88%), dynamic music fades out. At which point of the day should the music fade?")]
	[SerializeField]
	private float duskTimePercentage = 85f;

	// Token: 0x04002F66 RID: 12134
	[Tooltip("If we load into a save and the day is almost over, we shouldn't play music because it will stop soon anyway. At what point of the day should we not play music?")]
	[SerializeField]
	private float loadGameCutoffPercentage = 50f;

	// Token: 0x04002F67 RID: 12135
	[Tooltip("When dynamic music is active, we play a snapshot which attenuates the ambience and SFX. What intensity should that snapshot be applied?")]
	[SerializeField]
	private float dynamicMusicSFXAttenuationPercentage = 65f;

	// Token: 0x04002F68 RID: 12136
	[Tooltip("When mini songs are active, we play a snapshot which attenuates the ambience and SFX. What intensity should that snapshot be applied?")]
	[SerializeField]
	private float miniSongSFXAttenuationPercentage;

	// Token: 0x04002F69 RID: 12137
	[SerializeField]
	private MusicManager.TypeOfMusic[] musicStyleOrder;

	// Token: 0x04002F6A RID: 12138
	[NonSerialized]
	public bool alwaysPlayMusic;

	// Token: 0x04002F6B RID: 12139
	private MusicManager.DynamicSongPlaylist fullSongPlaylist = new MusicManager.DynamicSongPlaylist();

	// Token: 0x04002F6C RID: 12140
	private MusicManager.DynamicSongPlaylist miniSongPlaylist = new MusicManager.DynamicSongPlaylist();

	// Token: 0x04002F6D RID: 12141
	[NonSerialized]
	public MusicManager.SongInfo activeDynamicSong;

	// Token: 0x04002F6E RID: 12142
	[NonSerialized]
	public MusicManager.DynamicSongPlaylist activePlaylist;

	// Token: 0x04002F6F RID: 12143
	private MusicManager.TypeOfMusic nextMusicType;

	// Token: 0x04002F70 RID: 12144
	private int musicTypeIterator;

	// Token: 0x04002F71 RID: 12145
	private float time;

	// Token: 0x04002F72 RID: 12146
	private float timeOfDayUpdateRate = 2f;

	// Token: 0x04002F73 RID: 12147
	private static MusicManager _instance;

	// Token: 0x04002F74 RID: 12148
	[NonSerialized]
	public List<string> MusicDebugLog = new List<string>();

	// Token: 0x020019B4 RID: 6580
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class SongInfo
	{
		// Token: 0x04007A6B RID: 31339
		public EventReference fmodEvent;

		// Token: 0x04007A6C RID: 31340
		[NonSerialized]
		public int priority;

		// Token: 0x04007A6D RID: 31341
		[NonSerialized]
		public bool interruptsActiveMusic;

		// Token: 0x04007A6E RID: 31342
		[NonSerialized]
		public bool dynamic;

		// Token: 0x04007A6F RID: 31343
		[NonSerialized]
		public string requiredDlcId = "";

		// Token: 0x04007A70 RID: 31344
		[NonSerialized]
		public bool useTimeOfDay;

		// Token: 0x04007A71 RID: 31345
		[NonSerialized]
		public int numberOfVariations;

		// Token: 0x04007A72 RID: 31346
		[NonSerialized]
		public string musicKeySigniture = "C";

		// Token: 0x04007A73 RID: 31347
		[NonSerialized]
		public FMOD.Studio.EventInstance ev;

		// Token: 0x04007A74 RID: 31348
		[NonSerialized]
		public List<string> songsOnHold = new List<string>();

		// Token: 0x04007A75 RID: 31349
		[NonSerialized]
		public PLAYBACK_STATE musicPlaybackState;

		// Token: 0x04007A76 RID: 31350
		[NonSerialized]
		public bool playHook = true;

		// Token: 0x04007A77 RID: 31351
		[NonSerialized]
		public float sfxAttenuationPercentage = 65f;
	}

	// Token: 0x020019B5 RID: 6581
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class DynamicSong
	{
		// Token: 0x04007A78 RID: 31352
		public EventReference fmodEvent;

		// Token: 0x04007A79 RID: 31353
		[Tooltip("Some songs are set up to have Morning, Daytime, Hook, and Intro sections. Toggle this ON if this song has those sections.")]
		[SerializeField]
		public bool useTimeOfDay;

		// Token: 0x04007A7A RID: 31354
		[Tooltip("Some songs have different possible start locations. Enter how many start locations this song is set up to support.")]
		[SerializeField]
		public int numberOfVariations;

		// Token: 0x04007A7B RID: 31355
		[Tooltip("Some songs have different key signitures. Enter the key this music is in.")]
		[SerializeField]
		public string musicKeySigniture = "";

		// Token: 0x04007A7C RID: 31356
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId = "";
	}

	// Token: 0x020019B6 RID: 6582
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class Stinger
	{
		// Token: 0x04007A7D RID: 31357
		public EventReference fmodEvent;

		// Token: 0x04007A7E RID: 31358
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId = "";
	}

	// Token: 0x020019B7 RID: 6583
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class MenuSong
	{
		// Token: 0x04007A7F RID: 31359
		public EventReference fmodEvent;

		// Token: 0x04007A80 RID: 31360
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId = "";
	}

	// Token: 0x020019B8 RID: 6584
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class Minisong
	{
		// Token: 0x04007A81 RID: 31361
		public EventReference fmodEvent;

		// Token: 0x04007A82 RID: 31362
		[Tooltip("Some songs have different key signitures. Enter the key this music is in.")]
		[SerializeField]
		public string musicKeySigniture = "";

		// Token: 0x04007A83 RID: 31363
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId = "";
	}

	// Token: 0x020019B9 RID: 6585
	public enum TypeOfMusic
	{
		// Token: 0x04007A85 RID: 31365
		DynamicSong,
		// Token: 0x04007A86 RID: 31366
		MiniSong,
		// Token: 0x04007A87 RID: 31367
		None
	}

	// Token: 0x020019BA RID: 6586
	public class DynamicSongPlaylist
	{
		// Token: 0x06009DDB RID: 40411 RVA: 0x0037624A File Offset: 0x0037444A
		public void Clear()
		{
			this.songMap.Clear();
			this.unplayedSongs.Clear();
			this.lastSongPlayed = "";
		}

		// Token: 0x06009DDC RID: 40412 RVA: 0x00376270 File Offset: 0x00374470
		public string GetNextSong()
		{
			string text;
			if (this.unplayedSongs.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, this.unplayedSongs.Count);
				text = this.unplayedSongs[index];
				this.unplayedSongs.RemoveAt(index);
			}
			else
			{
				this.ResetUnplayedSongs();
				bool flag = this.unplayedSongs.Count > 1;
				if (flag)
				{
					for (int i = 0; i < this.unplayedSongs.Count; i++)
					{
						if (this.unplayedSongs[i] == this.lastSongPlayed)
						{
							this.unplayedSongs.Remove(this.unplayedSongs[i]);
							break;
						}
					}
				}
				int index2 = UnityEngine.Random.Range(0, this.unplayedSongs.Count);
				text = this.unplayedSongs[index2];
				this.unplayedSongs.RemoveAt(index2);
				if (flag)
				{
					this.unplayedSongs.Add(this.lastSongPlayed);
				}
			}
			this.lastSongPlayed = text;
			global::Debug.Assert(this.songMap.ContainsKey(text), "Missing song " + text);
			return Assets.GetSimpleSoundEventName(this.songMap[text].fmodEvent);
		}

		// Token: 0x06009DDD RID: 40413 RVA: 0x0037639C File Offset: 0x0037459C
		public void ResetUnplayedSongs()
		{
			this.unplayedSongs.Clear();
			foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.songMap)
			{
				if (MusicManager.IsValidForDLCContext(keyValuePair.Value.requiredDlcId))
				{
					this.unplayedSongs.Add(keyValuePair.Key);
				}
			}
		}

		// Token: 0x04007A88 RID: 31368
		public Dictionary<string, MusicManager.SongInfo> songMap = new Dictionary<string, MusicManager.SongInfo>();

		// Token: 0x04007A89 RID: 31369
		public List<string> unplayedSongs = new List<string>();

		// Token: 0x04007A8A RID: 31370
		private string lastSongPlayed = "";
	}
}
