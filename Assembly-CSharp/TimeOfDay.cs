using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FMOD.Studio;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x02000A47 RID: 2631
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/TimeOfDay")]
public class TimeOfDay : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x17000568 RID: 1384
	// (get) Token: 0x06004C29 RID: 19497 RVA: 0x001B3178 File Offset: 0x001B1378
	public static bool IsMilestoneApproaching
	{
		get
		{
			if (TimeOfDay.Instance != null && GameClock.Instance != null)
			{
				int currentTimeRegion = (int)TimeOfDay.Instance.GetCurrentTimeRegion();
				int cycle = GameClock.Instance.GetCycle();
				return currentTimeRegion == 2 && TimeOfDay.MILESTONE_CYCLES != null && TimeOfDay.MILESTONE_CYCLES.Contains(cycle + 1);
			}
			return false;
		}
	}

	// Token: 0x17000569 RID: 1385
	// (get) Token: 0x06004C2A RID: 19498 RVA: 0x001B31D0 File Offset: 0x001B13D0
	public static bool IsMilestoneDay
	{
		get
		{
			if (TimeOfDay.Instance != null && GameClock.Instance != null)
			{
				int currentTimeRegion = (int)TimeOfDay.Instance.GetCurrentTimeRegion();
				int cycle = GameClock.Instance.GetCycle();
				return currentTimeRegion == 1 && TimeOfDay.MILESTONE_CYCLES != null && TimeOfDay.MILESTONE_CYCLES.Contains(cycle);
			}
			return false;
		}
	}

	// Token: 0x1700056A RID: 1386
	// (get) Token: 0x06004C2C RID: 19500 RVA: 0x001B322E File Offset: 0x001B142E
	// (set) Token: 0x06004C2B RID: 19499 RVA: 0x001B3225 File Offset: 0x001B1425
	public TimeOfDay.TimeRegion timeRegion { get; private set; }

	// Token: 0x06004C2D RID: 19501 RVA: 0x001B3236 File Offset: 0x001B1436
	public static void DestroyInstance()
	{
		TimeOfDay.Instance = null;
	}

	// Token: 0x06004C2E RID: 19502 RVA: 0x001B323E File Offset: 0x001B143E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		TimeOfDay.Instance = this;
	}

	// Token: 0x06004C2F RID: 19503 RVA: 0x001B324C File Offset: 0x001B144C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		TimeOfDay.Instance = null;
	}

	// Token: 0x06004C30 RID: 19504 RVA: 0x001B325C File Offset: 0x001B145C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.timeRegion = this.GetCurrentTimeRegion();
		string clusterId = SaveLoader.Instance.GameInfo.clusterId;
		ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(clusterId);
		if (clusterData != null && !string.IsNullOrWhiteSpace(clusterData.clusterAudio.stingerDay))
		{
			this.stingerDay = clusterData.clusterAudio.stingerDay;
		}
		else
		{
			this.stingerDay = "Stinger_Day";
		}
		if (clusterData != null && !string.IsNullOrWhiteSpace(clusterData.clusterAudio.stingerNight))
		{
			this.stingerNight = clusterData.clusterAudio.stingerNight;
		}
		else
		{
			this.stingerNight = "Stinger_Loop_Night";
		}
		if (!MusicManager.instance.SongIsPlaying(this.stingerNight) && this.GetCurrentTimeRegion() == TimeOfDay.TimeRegion.Night)
		{
			MusicManager.instance.PlaySong(this.stingerNight, false);
			MusicManager.instance.SetSongParameter(this.stingerNight, "Music_PlayStinger", 0f, true);
		}
		this.UpdateSunlightIntensity();
	}

	// Token: 0x06004C31 RID: 19505 RVA: 0x001B334B File Offset: 0x001B154B
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.UpdateVisuals();
	}

	// Token: 0x06004C32 RID: 19506 RVA: 0x001B3353 File Offset: 0x001B1553
	public TimeOfDay.TimeRegion GetCurrentTimeRegion()
	{
		if (GameClock.Instance.IsNighttime())
		{
			return TimeOfDay.TimeRegion.Night;
		}
		return TimeOfDay.TimeRegion.Day;
	}

	// Token: 0x06004C33 RID: 19507 RVA: 0x001B3364 File Offset: 0x001B1564
	private void Update()
	{
		this.UpdateVisuals();
		TimeOfDay.TimeRegion currentTimeRegion = this.GetCurrentTimeRegion();
		int cycle = GameClock.Instance.GetCycle();
		if (currentTimeRegion != this.timeRegion)
		{
			if (TimeOfDay.IsMilestoneApproaching)
			{
				Game.Instance.Trigger(-720092972, cycle);
			}
			if (TimeOfDay.IsMilestoneDay)
			{
				Game.Instance.Trigger(2070437606, cycle);
			}
			this.TriggerSoundChange(currentTimeRegion, TimeOfDay.IsMilestoneDay);
			this.timeRegion = currentTimeRegion;
			base.Trigger(1791086652, null);
		}
	}

	// Token: 0x06004C34 RID: 19508 RVA: 0x001B33EC File Offset: 0x001B15EC
	private void UpdateVisuals()
	{
		float num = 0.875f;
		float num2 = 0.2f;
		float num3 = 1f;
		float b = 0f;
		if (GameClock.Instance.GetCurrentCycleAsPercentage() >= num)
		{
			b = num3;
		}
		this.scale = Mathf.Lerp(this.scale, b, Time.deltaTime * num2);
		float y = this.UpdateSunlightIntensity();
		Shader.SetGlobalVector("_TimeOfDay", new Vector4(this.scale, y, 0f, 0f));
	}

	// Token: 0x06004C35 RID: 19509 RVA: 0x001B3462 File Offset: 0x001B1662
	public void Sim4000ms(float dt)
	{
		this.UpdateSunlightIntensity();
	}

	// Token: 0x06004C36 RID: 19510 RVA: 0x001B346B File Offset: 0x001B166B
	public void SetEclipse(bool eclipse)
	{
		this.isEclipse = eclipse;
	}

	// Token: 0x06004C37 RID: 19511 RVA: 0x001B3474 File Offset: 0x001B1674
	private float UpdateSunlightIntensity()
	{
		float daytimeDurationInPercentage = GameClock.Instance.GetDaytimeDurationInPercentage();
		float num = GameClock.Instance.GetCurrentCycleAsPercentage() / daytimeDurationInPercentage;
		if (num >= 1f || this.isEclipse)
		{
			num = 0f;
		}
		float num2 = Mathf.Sin(num * 3.1415927f);
		Game.Instance.currentFallbackSunlightIntensity = num2 * 80000f;
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			worldContainer.currentSunlightIntensity = num2 * (float)worldContainer.sunlight;
			worldContainer.currentCosmicIntensity = (float)worldContainer.cosmicRadiation;
		}
		return num2;
	}

	// Token: 0x06004C38 RID: 19512 RVA: 0x001B352C File Offset: 0x001B172C
	private void TriggerSoundChange(TimeOfDay.TimeRegion new_region, bool milestoneReached)
	{
		if (new_region == TimeOfDay.TimeRegion.Day)
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().NightStartedMigrated, STOP_MODE.ALLOWFADEOUT);
			if (MusicManager.instance.SongIsPlaying(this.stingerNight))
			{
				MusicManager.instance.StopSong(this.stingerNight, true, STOP_MODE.ALLOWFADEOUT);
			}
			if (milestoneReached)
			{
				MusicManager.instance.PlaySong("Stinger_Day_Celebrate", false);
			}
			else
			{
				MusicManager.instance.PlaySong(this.stingerDay, false);
			}
			MusicManager.instance.PlayDynamicMusic();
			return;
		}
		if (new_region != TimeOfDay.TimeRegion.Night)
		{
			return;
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().NightStartedMigrated);
		MusicManager.instance.PlaySong(this.stingerNight, false);
	}

	// Token: 0x06004C39 RID: 19513 RVA: 0x001B35D3 File Offset: 0x001B17D3
	public void SetScale(float new_scale)
	{
		this.scale = new_scale;
	}

	// Token: 0x040032A5 RID: 12965
	private const string MILESTONE_CYCLE_REACHED_AUDIO_NAME = "Stinger_Day_Celebrate";

	// Token: 0x040032A6 RID: 12966
	public static List<int> MILESTONE_CYCLES = new List<int>(2)
	{
		99,
		999
	};

	// Token: 0x040032A7 RID: 12967
	[Serialize]
	private float scale;

	// Token: 0x040032A9 RID: 12969
	private EventInstance nightLPEvent;

	// Token: 0x040032AA RID: 12970
	public static TimeOfDay Instance;

	// Token: 0x040032AB RID: 12971
	public string stingerDay;

	// Token: 0x040032AC RID: 12972
	public string stingerNight;

	// Token: 0x040032AD RID: 12973
	private bool isEclipse;

	// Token: 0x02001A4E RID: 6734
	public enum TimeRegion
	{
		// Token: 0x04007BF9 RID: 31737
		Invalid,
		// Token: 0x04007BFA RID: 31738
		Day,
		// Token: 0x04007BFB RID: 31739
		Night
	}
}
