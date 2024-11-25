using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x02000DEC RID: 3564
public class VideoScreen : KModalScreen
{
	// Token: 0x06007122 RID: 28962 RVA: 0x002ACE90 File Offset: 0x002AB090
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
		this.closeButton.onClick += delegate()
		{
			this.Stop();
		};
		this.proceedButton.onClick += delegate()
		{
			this.Stop();
		};
		this.videoPlayer.isLooping = false;
		this.videoPlayer.loopPointReached += delegate(VideoPlayer data)
		{
			if (this.victoryLoopQueued)
			{
				base.StartCoroutine(this.SwitchToVictoryLoop());
				return;
			}
			if (!this.videoPlayer.isLooping)
			{
				this.Stop();
			}
		};
		VideoScreen.Instance = this;
		this.Show(false);
	}

	// Token: 0x06007123 RID: 28963 RVA: 0x002ACF08 File Offset: 0x002AB108
	protected override void OnForcedCleanUp()
	{
		VideoScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06007124 RID: 28964 RVA: 0x002ACF16 File Offset: 0x002AB116
	protected override void OnShow(bool show)
	{
		base.transform.SetAsLastSibling();
		base.OnShow(show);
		this.screen = this.videoPlayer.gameObject.GetComponent<RawImage>();
	}

	// Token: 0x06007125 RID: 28965 RVA: 0x002ACF40 File Offset: 0x002AB140
	public void DisableAllMedia()
	{
		this.overlayContainer.gameObject.SetActive(false);
		this.videoPlayer.gameObject.SetActive(false);
		this.slideshow.gameObject.SetActive(false);
	}

	// Token: 0x06007126 RID: 28966 RVA: 0x002ACF78 File Offset: 0x002AB178
	public void PlaySlideShow(Sprite[] sprites)
	{
		this.Show(true);
		this.DisableAllMedia();
		this.slideshow.updateType = SlideshowUpdateType.preloadedSprites;
		this.slideshow.gameObject.SetActive(true);
		this.slideshow.SetSprites(sprites);
		this.slideshow.SetPaused(false);
	}

	// Token: 0x06007127 RID: 28967 RVA: 0x002ACFC8 File Offset: 0x002AB1C8
	public void PlaySlideShow(string[] files)
	{
		this.Show(true);
		this.DisableAllMedia();
		this.slideshow.updateType = SlideshowUpdateType.loadOnDemand;
		this.slideshow.gameObject.SetActive(true);
		this.slideshow.SetFiles(files, 0);
		this.slideshow.SetPaused(false);
	}

	// Token: 0x06007128 RID: 28968 RVA: 0x002AD018 File Offset: 0x002AB218
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.IsAction(global::Action.Escape))
		{
			if (this.slideshow.gameObject.activeSelf && e.TryConsume(global::Action.Escape))
			{
				this.Stop();
				return;
			}
			if (e.TryConsume(global::Action.Escape))
			{
				if (this.videoSkippable)
				{
					this.Stop();
				}
				return;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06007129 RID: 28969 RVA: 0x002AD070 File Offset: 0x002AB270
	public void PlayVideo(VideoClip clip, bool unskippable = false, EventReference overrideAudioSnapshot = default(EventReference), bool showProceedButton = false)
	{
		global::Debug.Assert(clip != null);
		for (int i = 0; i < this.overlayContainer.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.overlayContainer.GetChild(i).gameObject);
		}
		this.Show(true);
		this.videoPlayer.isLooping = false;
		this.activeAudioSnapshot = (overrideAudioSnapshot.IsNull ? AudioMixerSnapshots.Get().TutorialVideoPlayingSnapshot : overrideAudioSnapshot);
		AudioMixer.instance.Start(this.activeAudioSnapshot);
		this.DisableAllMedia();
		this.videoPlayer.gameObject.SetActive(true);
		this.renderTexture = new RenderTexture(Convert.ToInt32(clip.width), Convert.ToInt32(clip.height), 16);
		this.screen.texture = this.renderTexture;
		this.videoPlayer.targetTexture = this.renderTexture;
		this.videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
		this.videoPlayer.clip = clip;
		this.videoPlayer.timeReference = VideoTimeReference.ExternalTime;
		this.videoPlayer.Play();
		if (this.audioHandle.isValid())
		{
			KFMOD.EndOneShot(this.audioHandle);
			this.audioHandle.clearHandle();
		}
		this.audioHandle = KFMOD.BeginOneShot(GlobalAssets.GetSound("vid_" + clip.name, false), Vector3.zero, 1f);
		KFMOD.EndOneShot(this.audioHandle);
		this.videoSkippable = !unskippable;
		this.closeButton.gameObject.SetActive(this.videoSkippable);
		this.proceedButton.gameObject.SetActive(showProceedButton && this.videoSkippable);
	}

	// Token: 0x0600712A RID: 28970 RVA: 0x002AD218 File Offset: 0x002AB418
	public void QueueVictoryVideoLoop(bool queue, string message = "", string victoryAchievement = "", string loopVideo = "")
	{
		this.victoryLoopQueued = queue;
		this.victoryLoopMessage = message;
		this.victoryLoopClip = loopVideo;
		this.OnStop = (System.Action)Delegate.Combine(this.OnStop, new System.Action(delegate()
		{
			RetireColonyUtility.SaveColonySummaryData();
			MainMenu.ActivateRetiredColoniesScreenFromData(base.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
		}));
	}

	// Token: 0x0600712B RID: 28971 RVA: 0x002AD254 File Offset: 0x002AB454
	public void SetOverlayText(string overlayTemplate, List<string> strings)
	{
		VideoOverlay videoOverlay = null;
		foreach (VideoOverlay videoOverlay2 in this.overlayPrefabs)
		{
			if (videoOverlay2.name == overlayTemplate)
			{
				videoOverlay = videoOverlay2;
				break;
			}
		}
		DebugUtil.Assert(videoOverlay != null, "Could not find a template named ", overlayTemplate);
		global::Util.KInstantiateUI<VideoOverlay>(videoOverlay.gameObject, this.overlayContainer.gameObject, true).SetText(strings);
		this.overlayContainer.gameObject.SetActive(true);
	}

	// Token: 0x0600712C RID: 28972 RVA: 0x002AD2F4 File Offset: 0x002AB4F4
	private IEnumerator SwitchToVictoryLoop()
	{
		this.victoryLoopQueued = false;
		Color color = this.fadeOverlay.color;
		for (float i = 0f; i < 1f; i += Time.unscaledDeltaTime)
		{
			this.fadeOverlay.color = new Color(color.r, color.g, color.b, i);
			yield return SequenceUtil.WaitForNextFrame;
		}
		this.fadeOverlay.color = new Color(color.r, color.g, color.b, 1f);
		MusicManager.instance.PlaySong("Music_Victory_03_StoryAndSummary", false);
		MusicManager.instance.SetSongParameter("Music_Victory_03_StoryAndSummary", "songSection", 1f, true);
		this.closeButton.gameObject.SetActive(true);
		this.proceedButton.gameObject.SetActive(true);
		this.SetOverlayText("VictoryEnd", new List<string>
		{
			this.victoryLoopMessage
		});
		this.videoPlayer.clip = Assets.GetVideo(this.victoryLoopClip);
		this.videoPlayer.isLooping = true;
		this.videoPlayer.Play();
		this.proceedButton.gameObject.SetActive(true);
		yield return SequenceUtil.WaitForSecondsRealtime(1f);
		for (float i = 1f; i >= 0f; i -= Time.unscaledDeltaTime)
		{
			this.fadeOverlay.color = new Color(color.r, color.g, color.b, i);
			yield return SequenceUtil.WaitForNextFrame;
		}
		this.fadeOverlay.color = new Color(color.r, color.g, color.b, 0f);
		yield break;
	}

	// Token: 0x0600712D RID: 28973 RVA: 0x002AD304 File Offset: 0x002AB504
	public void Stop()
	{
		this.videoPlayer.Stop();
		this.screen.texture = null;
		this.videoPlayer.targetTexture = null;
		if (!this.activeAudioSnapshot.IsNull)
		{
			AudioMixer.instance.Stop(this.activeAudioSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			this.audioHandle.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (this.OnStop != null)
		{
			this.OnStop();
		}
		this.Show(false);
	}

	// Token: 0x0600712E RID: 28974 RVA: 0x002AD37C File Offset: 0x002AB57C
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		if (this.audioHandle.isValid())
		{
			int num;
			this.audioHandle.getTimelinePosition(out num);
			this.videoPlayer.externalReferenceTime = (double)((float)num / 1000f);
		}
	}

	// Token: 0x04004DC9 RID: 19913
	public static VideoScreen Instance;

	// Token: 0x04004DCA RID: 19914
	[SerializeField]
	private VideoPlayer videoPlayer;

	// Token: 0x04004DCB RID: 19915
	[SerializeField]
	private Slideshow slideshow;

	// Token: 0x04004DCC RID: 19916
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004DCD RID: 19917
	[SerializeField]
	private KButton proceedButton;

	// Token: 0x04004DCE RID: 19918
	[SerializeField]
	private RectTransform overlayContainer;

	// Token: 0x04004DCF RID: 19919
	[SerializeField]
	private List<VideoOverlay> overlayPrefabs;

	// Token: 0x04004DD0 RID: 19920
	private RawImage screen;

	// Token: 0x04004DD1 RID: 19921
	private RenderTexture renderTexture;

	// Token: 0x04004DD2 RID: 19922
	private EventReference activeAudioSnapshot;

	// Token: 0x04004DD3 RID: 19923
	[SerializeField]
	private Image fadeOverlay;

	// Token: 0x04004DD4 RID: 19924
	private EventInstance audioHandle;

	// Token: 0x04004DD5 RID: 19925
	private bool victoryLoopQueued;

	// Token: 0x04004DD6 RID: 19926
	private string victoryLoopMessage = "";

	// Token: 0x04004DD7 RID: 19927
	private string victoryLoopClip = "";

	// Token: 0x04004DD8 RID: 19928
	private bool videoSkippable = true;

	// Token: 0x04004DD9 RID: 19929
	public System.Action OnStop;
}
