using System;
using System.Collections;
using FMOD.Studio;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x02000BA1 RID: 2977
public class MinionSelectScreen : CharacterSelectionController
{
	// Token: 0x06005A31 RID: 23089 RVA: 0x0020B228 File Offset: 0x00209428
	protected override void OnPrefabInit()
	{
		base.IsStarterMinion = true;
		base.OnPrefabInit();
		if (MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
		{
			MusicManager.instance.SetSongParameter("Music_FrontEnd", "songSection", 2f, true);
		}
		GameObject parent = GameObject.Find("ScreenSpaceOverlayCanvas");
		GameObject gameObject = global::Util.KInstantiateUI(this.wattsonMessagePrefab.gameObject, parent, false);
		gameObject.name = "WattsonMessage";
		gameObject.SetActive(false);
		Game.Instance.Subscribe(-1992507039, new Action<object>(this.OnBaseAlreadyCreated));
		this.backButton.onClick += delegate()
		{
			LoadScreen.ForceStopGame();
			App.LoadScene("frontend");
		};
		this.InitializeContainers();
		base.StartCoroutine(this.SetDefaultMinionsRoutine());
	}

	// Token: 0x06005A32 RID: 23090 RVA: 0x0020B2F4 File Offset: 0x002094F4
	private IEnumerator SetDefaultMinionsRoutine()
	{
		yield return SequenceUtil.WaitForNextFrame;
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		if (SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id).clusterTags.Contains("CeresCluster"))
		{
			((CharacterContainer)this.containers[2]).SetMinion(new MinionStartingStats(Db.Get().Personalities.Get("FREYJA"), null, null, false));
			((CharacterContainer)this.containers[1]).GenerateCharacter(true, null);
			((CharacterContainer)this.containers[0]).GenerateCharacter(true, null);
		}
		yield break;
	}

	// Token: 0x06005A33 RID: 23091 RVA: 0x0020B304 File Offset: 0x00209504
	public void SetProceedButtonActive(bool state, string tooltip = null)
	{
		if (state)
		{
			base.EnableProceedButton();
		}
		else
		{
			base.DisableProceedButton();
		}
		ToolTip component = this.proceedButton.GetComponent<ToolTip>();
		if (component != null)
		{
			if (tooltip != null)
			{
				component.toolTip = tooltip;
				return;
			}
			component.ClearMultiStringTooltip();
		}
	}

	// Token: 0x06005A34 RID: 23092 RVA: 0x0020B348 File Offset: 0x00209548
	protected override void OnSpawn()
	{
		this.OnDeliverableAdded();
		base.EnableProceedButton();
		this.proceedButton.GetComponentInChildren<LocText>().text = UI.IMMIGRANTSCREEN.EMBARK;
		this.containers.ForEach(delegate(ITelepadDeliverableContainer container)
		{
			CharacterContainer characterContainer = container as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.DisableSelectButton();
			}
		});
	}

	// Token: 0x06005A35 RID: 23093 RVA: 0x0020B3A8 File Offset: 0x002095A8
	protected override void OnProceed()
	{
		global::Util.KInstantiateUI(this.newBasePrefab.gameObject, GameScreenManager.Instance.ssOverlayCanvas, false);
		MusicManager.instance.StopSong("Music_FrontEnd", true, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().NewBaseSetupSnapshot);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWorldGenerationSnapshot, STOP_MODE.ALLOWFADEOUT);
		this.selectedDeliverables.Clear();
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = (CharacterContainer)telepadDeliverableContainer;
			this.selectedDeliverables.Add(characterContainer.Stats);
		}
		NewBaseScreen.Instance.Init(SaveLoader.Instance.Cluster, this.selectedDeliverables.ToArray());
		if (this.OnProceedEvent != null)
		{
			this.OnProceedEvent();
		}
		Game.Instance.Trigger(-838649377, null);
		BuildWatermark.Instance.gameObject.SetActive(false);
		this.Deactivate();
	}

	// Token: 0x06005A36 RID: 23094 RVA: 0x0020B4C8 File Offset: 0x002096C8
	private void OnBaseAlreadyCreated(object data)
	{
		Game.Instance.StopFE();
		Game.Instance.StartBE();
		Game.Instance.SetGameStarted();
		this.Deactivate();
	}

	// Token: 0x06005A37 RID: 23095 RVA: 0x0020B4EE File Offset: 0x002096EE
	private void ReshuffleAll()
	{
		if (this.OnReshuffleEvent != null)
		{
			this.OnReshuffleEvent(base.IsStarterMinion);
		}
	}

	// Token: 0x06005A38 RID: 23096 RVA: 0x0020B50C File Offset: 0x0020970C
	public override void OnPressBack()
	{
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = telepadDeliverableContainer as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.ForceStopEditingTitle();
			}
		}
	}

	// Token: 0x04003B4C RID: 15180
	[SerializeField]
	private NewBaseScreen newBasePrefab;

	// Token: 0x04003B4D RID: 15181
	[SerializeField]
	private WattsonMessage wattsonMessagePrefab;

	// Token: 0x04003B4E RID: 15182
	public const string WattsonGameObjName = "WattsonMessage";

	// Token: 0x04003B4F RID: 15183
	public KButton backButton;
}
