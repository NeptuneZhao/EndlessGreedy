using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

// Token: 0x02000B7A RID: 2938
public class AudioMixerSnapshots : ScriptableObject
{
	// Token: 0x06005837 RID: 22583 RVA: 0x001FDE54 File Offset: 0x001FC054
	[ContextMenu("Reload")]
	public void ReloadSnapshots()
	{
		this.snapshotMap.Clear();
		EventReference[] array = this.snapshots;
		for (int i = 0; i < array.Length; i++)
		{
			string eventReferencePath = KFMOD.GetEventReferencePath(array[i]);
			if (!eventReferencePath.IsNullOrWhiteSpace())
			{
				this.snapshotMap.Add(eventReferencePath);
			}
		}
	}

	// Token: 0x06005838 RID: 22584 RVA: 0x001FDEA2 File Offset: 0x001FC0A2
	public static AudioMixerSnapshots Get()
	{
		if (AudioMixerSnapshots.instance == null)
		{
			AudioMixerSnapshots.instance = Resources.Load<AudioMixerSnapshots>("AudioMixerSnapshots");
		}
		return AudioMixerSnapshots.instance;
	}

	// Token: 0x040039AB RID: 14763
	public EventReference TechFilterOnMigrated;

	// Token: 0x040039AC RID: 14764
	public EventReference TechFilterLogicOn;

	// Token: 0x040039AD RID: 14765
	public EventReference NightStartedMigrated;

	// Token: 0x040039AE RID: 14766
	public EventReference MenuOpenMigrated;

	// Token: 0x040039AF RID: 14767
	public EventReference MenuOpenHalfEffect;

	// Token: 0x040039B0 RID: 14768
	public EventReference SpeedPausedMigrated;

	// Token: 0x040039B1 RID: 14769
	public EventReference DuplicantCountAttenuatorMigrated;

	// Token: 0x040039B2 RID: 14770
	public EventReference NewBaseSetupSnapshot;

	// Token: 0x040039B3 RID: 14771
	public EventReference FrontEndSnapshot;

	// Token: 0x040039B4 RID: 14772
	public EventReference FrontEndWelcomeScreenSnapshot;

	// Token: 0x040039B5 RID: 14773
	public EventReference FrontEndWorldGenerationSnapshot;

	// Token: 0x040039B6 RID: 14774
	public EventReference IntroNIS;

	// Token: 0x040039B7 RID: 14775
	public EventReference PulseSnapshot;

	// Token: 0x040039B8 RID: 14776
	public EventReference ESCPauseSnapshot;

	// Token: 0x040039B9 RID: 14777
	public EventReference MENUNewDuplicantSnapshot;

	// Token: 0x040039BA RID: 14778
	public EventReference UserVolumeSettingsSnapshot;

	// Token: 0x040039BB RID: 14779
	public EventReference DuplicantCountMovingSnapshot;

	// Token: 0x040039BC RID: 14780
	public EventReference DuplicantCountSleepingSnapshot;

	// Token: 0x040039BD RID: 14781
	public EventReference PortalLPDimmedSnapshot;

	// Token: 0x040039BE RID: 14782
	public EventReference DynamicMusicPlayingSnapshot;

	// Token: 0x040039BF RID: 14783
	public EventReference FabricatorSideScreenOpenSnapshot;

	// Token: 0x040039C0 RID: 14784
	public EventReference SpaceVisibleSnapshot;

	// Token: 0x040039C1 RID: 14785
	public EventReference MENUStarmapSnapshot;

	// Token: 0x040039C2 RID: 14786
	public EventReference MENUStarmapNotPausedSnapshot;

	// Token: 0x040039C3 RID: 14787
	public EventReference GameNotFocusedSnapshot;

	// Token: 0x040039C4 RID: 14788
	public EventReference FacilityVisibleSnapshot;

	// Token: 0x040039C5 RID: 14789
	public EventReference TutorialVideoPlayingSnapshot;

	// Token: 0x040039C6 RID: 14790
	public EventReference VictoryMessageSnapshot;

	// Token: 0x040039C7 RID: 14791
	public EventReference VictoryNISGenericSnapshot;

	// Token: 0x040039C8 RID: 14792
	public EventReference VictoryNISRocketSnapshot;

	// Token: 0x040039C9 RID: 14793
	public EventReference VictoryCinematicSnapshot;

	// Token: 0x040039CA RID: 14794
	public EventReference VictoryFadeToBlackSnapshot;

	// Token: 0x040039CB RID: 14795
	public EventReference MuteDynamicMusicSnapshot;

	// Token: 0x040039CC RID: 14796
	public EventReference ActiveBaseChangeSnapshot;

	// Token: 0x040039CD RID: 14797
	public EventReference EventPopupSnapshot;

	// Token: 0x040039CE RID: 14798
	public EventReference SmallRocketInteriorReverbSnapshot;

	// Token: 0x040039CF RID: 14799
	public EventReference MediumRocketInteriorReverbSnapshot;

	// Token: 0x040039D0 RID: 14800
	public EventReference MainMenuVideoPlayingSnapshot;

	// Token: 0x040039D1 RID: 14801
	public EventReference TechFilterRadiationOn;

	// Token: 0x040039D2 RID: 14802
	public EventReference FrontEndSupplyClosetSnapshot;

	// Token: 0x040039D3 RID: 14803
	public EventReference FrontEndItemDropScreenSnapshot;

	// Token: 0x040039D4 RID: 14804
	[SerializeField]
	private EventReference[] snapshots;

	// Token: 0x040039D5 RID: 14805
	[NonSerialized]
	public List<string> snapshotMap = new List<string>();

	// Token: 0x040039D6 RID: 14806
	private static AudioMixerSnapshots instance;
}
