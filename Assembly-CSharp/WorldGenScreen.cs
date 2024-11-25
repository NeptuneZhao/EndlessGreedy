using System;
using System.IO;
using ProcGenGame;
using UnityEngine;

// Token: 0x02000DFE RID: 3582
public class WorldGenScreen : NewGameFlowScreen
{
	// Token: 0x0600719E RID: 29086 RVA: 0x002AFD92 File Offset: 0x002ADF92
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		WorldGenScreen.Instance = this;
	}

	// Token: 0x0600719F RID: 29087 RVA: 0x002AFDA0 File Offset: 0x002ADFA0
	protected override void OnForcedCleanUp()
	{
		WorldGenScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x060071A0 RID: 29088 RVA: 0x002AFDB0 File Offset: 0x002ADFB0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (MainMenu.Instance != null)
		{
			MainMenu.Instance.StopAmbience();
		}
		this.TriggerLoadingMusic();
		UnityEngine.Object.FindObjectOfType<FrontEndBackground>().gameObject.SetActive(false);
		SaveLoader.SetActiveSaveFilePath(null);
		try
		{
			if (File.Exists(WorldGen.WORLDGEN_SAVE_FILENAME))
			{
				File.Delete(WorldGen.WORLDGEN_SAVE_FILENAME);
			}
		}
		catch (Exception ex)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				ex.ToString()
			});
		}
		this.offlineWorldGen.Generate();
	}

	// Token: 0x060071A1 RID: 29089 RVA: 0x002AFE40 File Offset: 0x002AE040
	private void TriggerLoadingMusic()
	{
		if (AudioDebug.Get().musicEnabled && !MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
		{
			MainMenu.Instance.StopMainMenuMusic();
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWorldGenerationSnapshot);
			MusicManager.instance.PlaySong("Music_FrontEnd", false);
			MusicManager.instance.SetSongParameter("Music_FrontEnd", "songSection", 1f, true);
		}
	}

	// Token: 0x060071A2 RID: 29090 RVA: 0x002AFEB3 File Offset: 0x002AE0B3
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed)
		{
			e.TryConsume(global::Action.Escape);
		}
		if (!e.Consumed)
		{
			e.TryConsume(global::Action.MouseRight);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04004E6D RID: 20077
	[MyCmpReq]
	private OfflineWorldGen offlineWorldGen;

	// Token: 0x04004E6E RID: 20078
	public static WorldGenScreen Instance;
}
