﻿using System;
using System.Collections;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020007C6 RID: 1990
public static class GeothermalVictorySequence
{
	// Token: 0x060036F3 RID: 14067 RVA: 0x0012B220 File Offset: 0x00129420
	public static void Start(KMonoBehaviour controller)
	{
		controller.StartCoroutine(GeothermalVictorySequence.Sequence());
	}

	// Token: 0x060036F4 RID: 14068 RVA: 0x0012B22E File Offset: 0x0012942E
	private static IEnumerator Sequence()
	{
		if (GeothermalVictorySequence.VictoryVent == null)
		{
			DebugUtil.DevLogError("No vent set, something went wrong");
			yield break;
		}
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		CameraController.Instance.SetWorldInteractive(false);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryMessageSnapshot, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Start(Db.Get().ColonyAchievements.ActivateGeothermalPlant.victoryNISSnapshot);
		MusicManager.instance.PlaySong("Music_Victory_02_NIS", false);
		Vector3.up * 5f;
		CameraController.Instance.FadeOut(1f, 1f, null);
		yield return SequenceUtil.WaitForSecondsRealtime(1f);
		Vector3 ventTargetPositon = GeothermalVictorySequence.VictoryVent.transform.position + Vector3.up * 3f;
		CameraController.Instance.SetTargetPos(ventTargetPositon, 10f, false);
		CameraController.Instance.SetOverrideZoomSpeed(10f);
		yield return SequenceUtil.WaitForSecondsRealtime(1f);
		CameraController.Instance.FadeIn(0f, 1f, null);
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(false);
		}
		SpeedControlScreen.Instance.SetSpeed(0);
		CameraController.Instance.SetOverrideZoomSpeed(0.05f);
		CameraController.Instance.SetTargetPos(ventTargetPositon, 20f, false);
		GeothermalVictorySequence.VictoryVent.SpawnKeepsake();
		yield return SequenceUtil.WaitForSecondsRealtime(4f);
		foreach (GeothermalVent.ElementInfo info in GeothermalControllerConfig.GetClearingEntombedVentReward())
		{
			GeothermalVictorySequence.VictoryVent.addMaterial(info);
		}
		yield return SequenceUtil.WaitForSecondsRealtime(5f);
		CameraController.Instance.FadeOut(1f, 1f, null);
		ventTargetPositon = default(Vector3);
		yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
		MusicManager.instance.StopSong("Music_Victory_02_NIS", true, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(Db.Get().ColonyAchievements.ActivateGeothermalPlant.victoryNISSnapshot, STOP_MODE.ALLOWFADEOUT);
		yield return SequenceUtil.WaitForSecondsRealtime(2f);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		VideoScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.VideoScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<VideoScreen>();
		component.PlayVideo(Assets.GetVideo(Db.Get().ColonyAchievements.ActivateGeothermalPlant.shortVideoName), true, AudioMixerSnapshots.Get().VictoryCinematicSnapshot, false);
		component.QueueVictoryVideoLoop(true, Db.Get().ColonyAchievements.ActivateGeothermalPlant.messageBody, Db.Get().ColonyAchievements.ActivateGeothermalPlant.Id, Db.Get().ColonyAchievements.ActivateGeothermalPlant.loopVideoName);
		component.OnStop = (System.Action)Delegate.Combine(component.OnStop, new System.Action(delegate()
		{
			StoryMessageScreen.HideInterface(false);
			CameraController.Instance.FadeIn(0f, 1f, null);
			CameraController.Instance.SetWorldInteractive(true);
			CameraController.Instance.SetOverrideZoomSpeed(1f);
			HoverTextScreen.Instance.Show(true);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryCinematicSnapshot, STOP_MODE.ALLOWFADEOUT);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot, STOP_MODE.ALLOWFADEOUT);
			RootMenu.Instance.canTogglePauseScreen = true;
			Game.Instance.unlocks.Unlock("notes_earthquake", true);
		}));
		GeothermalVictorySequence.VictoryVent = null;
		yield break;
	}

	// Token: 0x0400207D RID: 8317
	public static GeothermalVent VictoryVent;
}
