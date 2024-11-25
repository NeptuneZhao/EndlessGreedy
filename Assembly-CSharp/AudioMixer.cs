using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x020004FF RID: 1279
public class AudioMixer
{
	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06001C8B RID: 7307 RVA: 0x00096375 File Offset: 0x00094575
	public static AudioMixer instance
	{
		get
		{
			return AudioMixer._instance;
		}
	}

	// Token: 0x06001C8C RID: 7308 RVA: 0x0009637C File Offset: 0x0009457C
	public static AudioMixer Create()
	{
		AudioMixer._instance = new AudioMixer();
		AudioMixerSnapshots audioMixerSnapshots = AudioMixerSnapshots.Get();
		if (audioMixerSnapshots != null)
		{
			audioMixerSnapshots.ReloadSnapshots();
		}
		return AudioMixer._instance;
	}

	// Token: 0x06001C8D RID: 7309 RVA: 0x000963AD File Offset: 0x000945AD
	public static void Destroy()
	{
		AudioMixer._instance.StopAll(FMOD.Studio.STOP_MODE.IMMEDIATE);
		AudioMixer._instance = null;
	}

	// Token: 0x06001C8E RID: 7310 RVA: 0x000963C0 File Offset: 0x000945C0
	public EventInstance Start(EventReference event_ref)
	{
		string snapshot;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out snapshot);
		return this.Start(snapshot);
	}

	// Token: 0x06001C8F RID: 7311 RVA: 0x000963EC File Offset: 0x000945EC
	public EventInstance Start(string snapshot)
	{
		EventInstance eventInstance;
		if (!this.activeSnapshots.TryGetValue(snapshot, out eventInstance))
		{
			if (RuntimeManager.IsInitialized)
			{
				eventInstance = KFMOD.CreateInstance(snapshot);
				this.activeSnapshots[snapshot] = eventInstance;
				eventInstance.start();
				eventInstance.setParameterByName("snapshotActive", 1f, false);
			}
			else
			{
				eventInstance = default(EventInstance);
			}
		}
		AudioMixer.instance.Log("Start Snapshot: " + snapshot);
		return eventInstance;
	}

	// Token: 0x06001C90 RID: 7312 RVA: 0x0009646C File Offset: 0x0009466C
	public bool Stop(EventReference event_ref, FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		string s;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out s);
		return this.Stop(s, stop_mode);
	}

	// Token: 0x06001C91 RID: 7313 RVA: 0x0009649C File Offset: 0x0009469C
	public bool Stop(HashedString snapshot, FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		bool result = false;
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshot, out eventInstance))
		{
			eventInstance.setParameterByName("snapshotActive", 0f, false);
			eventInstance.stop(stop_mode);
			eventInstance.release();
			this.activeSnapshots.Remove(snapshot);
			result = true;
			AudioMixer instance = AudioMixer.instance;
			string[] array = new string[5];
			array[0] = "Stop Snapshot: [";
			int num = 1;
			HashedString hashedString = snapshot;
			array[num] = hashedString.ToString();
			array[2] = "] with fadeout mode: [";
			array[3] = stop_mode.ToString();
			array[4] = "]";
			instance.Log(string.Concat(array));
		}
		else
		{
			AudioMixer instance2 = AudioMixer.instance;
			string str = "Tried to stop snapshot: [";
			HashedString hashedString = snapshot;
			instance2.Log(str + hashedString.ToString() + "] but it wasn't active.");
		}
		return result;
	}

	// Token: 0x06001C92 RID: 7314 RVA: 0x0009656B File Offset: 0x0009476B
	public void Reset()
	{
		this.StopAll(FMOD.Studio.STOP_MODE.IMMEDIATE);
	}

	// Token: 0x06001C93 RID: 7315 RVA: 0x00096574 File Offset: 0x00094774
	public void StopAll(FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
	{
		List<HashedString> list = new List<HashedString>();
		foreach (KeyValuePair<HashedString, EventInstance> keyValuePair in this.activeSnapshots)
		{
			if (keyValuePair.Key != AudioMixer.UserVolumeSettingsHash)
			{
				list.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			this.Stop(list[i], stop_mode);
		}
	}

	// Token: 0x06001C94 RID: 7316 RVA: 0x00096608 File Offset: 0x00094808
	public bool SnapshotIsActive(EventReference event_ref)
	{
		string s;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out s);
		return this.SnapshotIsActive(s);
	}

	// Token: 0x06001C95 RID: 7317 RVA: 0x00096637 File Offset: 0x00094837
	public bool SnapshotIsActive(HashedString snapshot_name)
	{
		return this.activeSnapshots.ContainsKey(snapshot_name);
	}

	// Token: 0x06001C96 RID: 7318 RVA: 0x0009664C File Offset: 0x0009484C
	public void SetSnapshotParameter(EventReference event_ref, string parameter_name, float parameter_value, bool shouldLog = true)
	{
		string snapshot_name;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out snapshot_name);
		this.SetSnapshotParameter(snapshot_name, parameter_name, parameter_value, shouldLog);
	}

	// Token: 0x06001C97 RID: 7319 RVA: 0x0009667C File Offset: 0x0009487C
	public void SetSnapshotParameter(string snapshot_name, string parameter_name, float parameter_value, bool shouldLog = true)
	{
		if (shouldLog)
		{
			this.Log(string.Format("Set Param {0}: {1}, {2}", snapshot_name, parameter_name, parameter_value));
		}
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshot_name, out eventInstance))
		{
			eventInstance.setParameterByName(parameter_name, parameter_value, false);
			return;
		}
		this.Log(string.Concat(new string[]
		{
			"Tried to set [",
			parameter_name,
			"] to [",
			parameter_value.ToString(),
			"] but [",
			snapshot_name,
			"] is not active."
		}));
	}

	// Token: 0x06001C98 RID: 7320 RVA: 0x0009670C File Offset: 0x0009490C
	public void StartPersistentSnapshots()
	{
		this.persistentSnapshotsActive = true;
		this.Start(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated);
		this.Start(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot);
		this.Start(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot);
		this.spaceVisibleInst = this.Start(AudioMixerSnapshots.Get().SpaceVisibleSnapshot);
		this.facilityVisibleInst = this.Start(AudioMixerSnapshots.Get().FacilityVisibleSnapshot);
		this.Start(AudioMixerSnapshots.Get().PulseSnapshot);
	}

	// Token: 0x06001C99 RID: 7321 RVA: 0x00096790 File Offset: 0x00094990
	public void StopPersistentSnapshots()
	{
		this.persistentSnapshotsActive = false;
		this.Stop(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().SpaceVisibleSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().FacilityVisibleSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().PulseSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x06001C9A RID: 7322 RVA: 0x00096810 File Offset: 0x00094A10
	private string GetSnapshotName(EventReference event_ref)
	{
		string result;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out result);
		return result;
	}

	// Token: 0x06001C9B RID: 7323 RVA: 0x00096834 File Offset: 0x00094A34
	public void UpdatePersistentSnapshotParameters()
	{
		this.SetVisibleDuplicants();
		string snapshotName = this.GetSnapshotName(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot);
		if (this.activeSnapshots.TryGetValue(snapshotName, out this.duplicantCountMovingInst))
		{
			this.duplicantCountMovingInst.setParameterByName("duplicantCount", (float)Mathf.Max(0, this.visibleDupes["moving"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION), false);
		}
		string snapshotName2 = this.GetSnapshotName(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot);
		if (this.activeSnapshots.TryGetValue(snapshotName2, out this.duplicantCountSleepingInst))
		{
			this.duplicantCountSleepingInst.setParameterByName("duplicantCount", (float)Mathf.Max(0, this.visibleDupes["sleeping"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION), false);
		}
		string snapshotName3 = this.GetSnapshotName(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated);
		if (this.activeSnapshots.TryGetValue(snapshotName3, out this.duplicantCountInst))
		{
			this.duplicantCountInst.setParameterByName("duplicantCount", (float)Mathf.Max(0, this.visibleDupes["visible"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION), false);
		}
		string snapshotName4 = this.GetSnapshotName(AudioMixerSnapshots.Get().PulseSnapshot);
		if (this.activeSnapshots.TryGetValue(snapshotName4, out this.pulseInst))
		{
			float num = AudioMixer.PULSE_SNAPSHOT_BPM / 60f;
			int speed = SpeedControlScreen.Instance.GetSpeed();
			if (speed == 1)
			{
				num /= 2f;
			}
			else if (speed == 2)
			{
				num /= 3f;
			}
			float value = Mathf.Abs(Mathf.Sin(Time.time * 3.1415927f * num));
			this.pulseInst.setParameterByName("Pulse", value, false);
		}
	}

	// Token: 0x06001C9C RID: 7324 RVA: 0x000969E3 File Offset: 0x00094BE3
	public void UpdateSpaceVisibleSnapshot(float percent)
	{
		this.spaceVisibleInst.setParameterByName("spaceVisible", percent, false);
	}

	// Token: 0x06001C9D RID: 7325 RVA: 0x000969F8 File Offset: 0x00094BF8
	public void PauseSpaceVisibleSnapshot(bool pause)
	{
		this.spaceVisibleInst.setParameterByName("spaceVisible", 0f, true);
		this.spaceVisibleInst.setPaused(pause);
	}

	// Token: 0x06001C9E RID: 7326 RVA: 0x00096A1E File Offset: 0x00094C1E
	public void UpdateFacilityVisibleSnapshot(float percent)
	{
		this.facilityVisibleInst.setParameterByName("facilityVisible", percent, false);
	}

	// Token: 0x06001C9F RID: 7327 RVA: 0x00096A34 File Offset: 0x00094C34
	private void SetVisibleDuplicants()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			Vector3 position = Components.LiveMinionIdentities[i].transform.GetPosition();
			if (CameraController.Instance.IsVisiblePos(position))
			{
				num++;
				Navigator component = Components.LiveMinionIdentities[i].GetComponent<Navigator>();
				if (component != null && component.IsMoving())
				{
					num2++;
				}
				else
				{
					StaminaMonitor.Instance smi = Components.LiveMinionIdentities[i].GetComponent<WorkerBase>().GetSMI<StaminaMonitor.Instance>();
					if (smi != null && smi.IsSleeping())
					{
						num3++;
					}
				}
			}
		}
		this.visibleDupes["visible"] = num;
		this.visibleDupes["moving"] = num2;
		this.visibleDupes["sleeping"] = num3;
	}

	// Token: 0x06001CA0 RID: 7328 RVA: 0x00096B14 File Offset: 0x00094D14
	public void StartUserVolumesSnapshot()
	{
		this.Start(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot);
		string snapshotName = this.GetSnapshotName(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot);
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshotName, out eventInstance))
		{
			EventDescription eventDescription;
			eventInstance.getDescription(out eventDescription);
			USER_PROPERTY user_PROPERTY;
			eventDescription.getUserProperty("buses", out user_PROPERTY);
			string text = user_PROPERTY.stringValue();
			char separator = '-';
			string[] array = text.Split(separator, StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				float busLevel = 1f;
				string key = "Volume_" + array[i];
				if (KPlayerPrefs.HasKey(key))
				{
					busLevel = KPlayerPrefs.GetFloat(key);
				}
				AudioMixer.UserVolumeBus userVolumeBus = new AudioMixer.UserVolumeBus();
				userVolumeBus.busLevel = busLevel;
				userVolumeBus.labelString = Strings.Get("STRINGS.UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUDIO_BUS_" + array[i].ToUpper());
				this.userVolumeSettings.Add(array[i], userVolumeBus);
				this.SetUserVolume(array[i], userVolumeBus.busLevel);
			}
		}
	}

	// Token: 0x06001CA1 RID: 7329 RVA: 0x00096C28 File Offset: 0x00094E28
	public void SetUserVolume(string bus, float value)
	{
		if (!this.userVolumeSettings.ContainsKey(bus))
		{
			global::Debug.LogError("The provided bus doesn't exist. Check yo'self fool!");
			return;
		}
		if (value > 1f)
		{
			value = 1f;
		}
		else if (value < 0f)
		{
			value = 0f;
		}
		this.userVolumeSettings[bus].busLevel = value;
		KPlayerPrefs.SetFloat("Volume_" + bus, value);
		string snapshotName = this.GetSnapshotName(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot);
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshotName, out eventInstance))
		{
			eventInstance.setParameterByName("userVolume_" + bus, this.userVolumeSettings[bus].busLevel, false);
		}
		else
		{
			this.Log(string.Concat(new string[]
			{
				"Tried to set [",
				bus,
				"] to [",
				value.ToString(),
				"] but UserVolumeSettingsSnapshot is not active."
			}));
		}
		if (bus == "Music")
		{
			this.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "userVolume_Music", value, true);
		}
	}

	// Token: 0x06001CA2 RID: 7330 RVA: 0x00096D39 File Offset: 0x00094F39
	private void Log(string s)
	{
	}

	// Token: 0x04001015 RID: 4117
	private static AudioMixer _instance = null;

	// Token: 0x04001016 RID: 4118
	private const string DUPLICANT_COUNT_ID = "duplicantCount";

	// Token: 0x04001017 RID: 4119
	private const string PULSE_ID = "Pulse";

	// Token: 0x04001018 RID: 4120
	private const string SNAPSHOT_ACTIVE_ID = "snapshotActive";

	// Token: 0x04001019 RID: 4121
	private const string SPACE_VISIBLE_ID = "spaceVisible";

	// Token: 0x0400101A RID: 4122
	private const string FACILITY_VISIBLE_ID = "facilityVisible";

	// Token: 0x0400101B RID: 4123
	private const string FOCUS_BUS_PATH = "bus:/SFX/Focus";

	// Token: 0x0400101C RID: 4124
	public Dictionary<HashedString, EventInstance> activeSnapshots = new Dictionary<HashedString, EventInstance>();

	// Token: 0x0400101D RID: 4125
	public List<HashedString> SnapshotDebugLog = new List<HashedString>();

	// Token: 0x0400101E RID: 4126
	public bool activeNIS;

	// Token: 0x0400101F RID: 4127
	public static float LOW_PRIORITY_CUTOFF_DISTANCE = 10f;

	// Token: 0x04001020 RID: 4128
	public static float PULSE_SNAPSHOT_BPM = 120f;

	// Token: 0x04001021 RID: 4129
	public static int VISIBLE_DUPLICANTS_BEFORE_ATTENUATION = 2;

	// Token: 0x04001022 RID: 4130
	private EventInstance duplicantCountInst;

	// Token: 0x04001023 RID: 4131
	private EventInstance pulseInst;

	// Token: 0x04001024 RID: 4132
	private EventInstance duplicantCountMovingInst;

	// Token: 0x04001025 RID: 4133
	private EventInstance duplicantCountSleepingInst;

	// Token: 0x04001026 RID: 4134
	private EventInstance spaceVisibleInst;

	// Token: 0x04001027 RID: 4135
	private EventInstance facilityVisibleInst;

	// Token: 0x04001028 RID: 4136
	private static readonly HashedString UserVolumeSettingsHash = new HashedString("event:/Snapshots/Mixing/Snapshot_UserVolumeSettings");

	// Token: 0x04001029 RID: 4137
	public bool persistentSnapshotsActive;

	// Token: 0x0400102A RID: 4138
	private Dictionary<string, int> visibleDupes = new Dictionary<string, int>();

	// Token: 0x0400102B RID: 4139
	public Dictionary<string, AudioMixer.UserVolumeBus> userVolumeSettings = new Dictionary<string, AudioMixer.UserVolumeBus>();

	// Token: 0x020012CE RID: 4814
	public class UserVolumeBus
	{
		// Token: 0x04006486 RID: 25734
		public string labelString;

		// Token: 0x04006487 RID: 25735
		public float busLevel;
	}
}
