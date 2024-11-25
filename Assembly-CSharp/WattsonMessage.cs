using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BBA RID: 3002
public class WattsonMessage : KScreen
{
	// Token: 0x06005B41 RID: 23361 RVA: 0x00213ED3 File Offset: 0x002120D3
	public override float GetSortKey()
	{
		return 8f;
	}

	// Token: 0x06005B42 RID: 23362 RVA: 0x00213EDC File Offset: 0x002120DC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Game.Instance.Subscribe(-122303817, new Action<object>(this.OnNewBaseCreated));
		string welcomeMessage = CustomGameSettings.Instance.GetCurrentClusterLayout().welcomeMessage;
		if (welcomeMessage != null)
		{
			StringEntry stringEntry;
			this.message.SetText(Strings.TryGet(welcomeMessage, out stringEntry) ? stringEntry.String : welcomeMessage);
			return;
		}
		if (DlcManager.IsExpansion1Active())
		{
			this.message.SetText(UI.WELCOMEMESSAGEBODY_SPACEDOUT);
			return;
		}
		this.message.SetText(UI.WELCOMEMESSAGEBODY);
	}

	// Token: 0x06005B43 RID: 23363 RVA: 0x00213F6F File Offset: 0x0021216F
	private IEnumerator ExpandPanel()
	{
		this.button.isInteractable = false;
		if (CustomGameSettings.Instance.GetSettingsCoordinate().StartsWith("KF23"))
		{
			this.dialog.rectTransform().rotation = Quaternion.Euler(0f, 0f, -90f);
		}
		yield return SequenceUtil.WaitForSecondsRealtime(0.2f);
		float height = 0f;
		while (height < 299f)
		{
			height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, 300f, Time.unscaledDeltaTime * 15f);
			this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
			yield return 0;
		}
		if (CustomGameSettings.Instance.GetSettingsCoordinate().StartsWith("KF23"))
		{
			Quaternion initialOrientation = Quaternion.Euler(0f, 0f, -90f);
			yield return SequenceUtil.WaitForSecondsRealtime(1f);
			float t = 0f;
			float duration = 0.5f;
			while (t < duration)
			{
				t += Time.unscaledDeltaTime;
				this.dialog.rectTransform().rotation = Quaternion.Slerp(initialOrientation, Quaternion.identity, t / duration);
				yield return 0;
			}
			initialOrientation = default(Quaternion);
		}
		this.button.isInteractable = true;
		yield return null;
		yield break;
	}

	// Token: 0x06005B44 RID: 23364 RVA: 0x00213F7E File Offset: 0x0021217E
	private IEnumerator CollapsePanel()
	{
		float height = 300f;
		while (height > 1f)
		{
			height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, 0f, Time.unscaledDeltaTime * 15f);
			this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
			yield return 0;
		}
		this.Deactivate();
		yield return null;
		yield break;
	}

	// Token: 0x06005B45 RID: 23365 RVA: 0x00213F90 File Offset: 0x00212190
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.hideScreensWhileActive.Add(NotificationScreen.Instance);
		this.hideScreensWhileActive.Add(OverlayMenu.Instance);
		if (PlanScreen.Instance != null)
		{
			this.hideScreensWhileActive.Add(PlanScreen.Instance);
		}
		if (BuildMenu.Instance != null)
		{
			this.hideScreensWhileActive.Add(BuildMenu.Instance);
		}
		this.hideScreensWhileActive.Add(ManagementMenu.Instance);
		this.hideScreensWhileActive.Add(ToolMenu.Instance);
		this.hideScreensWhileActive.Add(ToolMenu.Instance.PriorityScreen);
		this.hideScreensWhileActive.Add(PinnedResourcesPanel.Instance);
		this.hideScreensWhileActive.Add(TopLeftControlScreen.Instance);
		this.hideScreensWhileActive.Add(global::DateTime.Instance);
		this.hideScreensWhileActive.Add(BuildWatermark.Instance);
		this.hideScreensWhileActive.Add(BuildWatermark.Instance);
		this.hideScreensWhileActive.Add(ColonyDiagnosticScreen.Instance);
		if (WorldSelector.Instance != null)
		{
			this.hideScreensWhileActive.Add(WorldSelector.Instance);
		}
		foreach (KScreen kscreen in this.hideScreensWhileActive)
		{
			kscreen.Show(false);
		}
	}

	// Token: 0x06005B46 RID: 23366 RVA: 0x002140F4 File Offset: 0x002122F4
	public void Update()
	{
		if (!this.startFade)
		{
			return;
		}
		Color color = this.bg.color;
		color.a -= 0.01f;
		if (color.a <= 0f)
		{
			color.a = 0f;
		}
		this.bg.color = color;
	}

	// Token: 0x06005B47 RID: 23367 RVA: 0x0021414C File Offset: 0x0021234C
	protected override void OnActivate()
	{
		global::Debug.Log("WattsonMessage OnActivate");
		base.OnActivate();
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().NewBaseSetupSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().IntroNIS);
		AudioMixer.instance.activeNIS = true;
		this.button.onClick += delegate()
		{
			base.StartCoroutine(this.CollapsePanel());
		};
		this.dialog.GetComponent<KScreen>().Show(false);
		this.startFade = false;
		GameObject telepad = GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id);
		if (telepad != null)
		{
			KAnimControllerBase kac = telepad.GetComponent<KAnimControllerBase>();
			kac.Play(WattsonMessage.WorkLoopAnims, KAnim.PlayMode.Loop);
			NameDisplayScreen.Instance.gameObject.SetActive(false);
			for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
			{
				int idx = i + 1;
				MinionIdentity minionIdentity = Components.LiveMinionIdentities[i];
				minionIdentity.gameObject.transform.SetPosition(new Vector3(telepad.transform.GetPosition().x + (float)idx - 1.5f, telepad.transform.GetPosition().y, minionIdentity.gameObject.transform.GetPosition().z));
				GameObject gameObject = minionIdentity.gameObject;
				ChoreProvider chore_provider = gameObject.GetComponent<ChoreProvider>();
				EmoteChore chorePre = new EmoteChore(chore_provider, Db.Get().ChoreTypes.EmoteHighPriority, "anim_interacts_portal_kanim", new HashedString[]
				{
					"portalbirth_pre_" + idx.ToString()
				}, KAnim.PlayMode.Loop, false);
				UIScheduler.Instance.Schedule("DupeBirth", (float)idx * 0.5f, delegate(object data)
				{
					chorePre.Cancel("Done looping");
					EmoteChore emoteChore = new EmoteChore(chore_provider, Db.Get().ChoreTypes.EmoteHighPriority, "anim_interacts_portal_kanim", new HashedString[]
					{
						"portalbirth_" + idx.ToString()
					}, null);
					emoteChore.onComplete = (Action<Chore>)Delegate.Combine(emoteChore.onComplete, new Action<Chore>(delegate(Chore param)
					{
						this.birthsComplete++;
						if (this.birthsComplete == Components.LiveMinionIdentities.Count - 1 && base.IsActive())
						{
							NameDisplayScreen.Instance.gameObject.SetActive(true);
							this.PauseAndShowMessage();
						}
					}));
				}, null, null);
			}
			UIScheduler.Instance.Schedule("Welcome", 6.6f, delegate(object data)
			{
				kac.Play(new HashedString[]
				{
					"working_pst",
					"idle"
				}, KAnim.PlayMode.Once);
			}, null, null);
			CameraController.Instance.DisableUserCameraControl = true;
		}
		else
		{
			global::Debug.LogWarning("Failed to spawn telepad - does the starting base template lack a 'Headquarters' ?");
			this.PauseAndShowMessage();
		}
		this.scheduleHandles.Add(UIScheduler.Instance.Schedule("GoHome", 0.1f, delegate(object data)
		{
			CameraController.Instance.OrthographicSize = TuningData<WattsonMessage.Tuning>.Get().initialOrthographicSize;
			CameraController.Instance.CameraGoHome(0.5f);
			this.startFade = true;
			MusicManager.instance.PlaySong(this.WelcomeMusic, false);
		}, null, null));
	}

	// Token: 0x170006C4 RID: 1732
	// (get) Token: 0x06005B48 RID: 23368 RVA: 0x002143C4 File Offset: 0x002125C4
	private string WelcomeMusic
	{
		get
		{
			string musicWelcome = CustomGameSettings.Instance.GetCurrentClusterLayout().clusterAudio.musicWelcome;
			if (!musicWelcome.IsNullOrWhiteSpace())
			{
				return musicWelcome;
			}
			return "Music_WattsonMessage";
		}
	}

	// Token: 0x06005B49 RID: 23369 RVA: 0x002143F8 File Offset: 0x002125F8
	protected void PauseAndShowMessage()
	{
		SpeedControlScreen.Instance.Pause(false, false);
		base.StartCoroutine(this.ExpandPanel());
		KFMOD.PlayUISound(this.dialogSound);
		this.dialog.GetComponent<KScreen>().Activate();
		this.dialog.GetComponent<KScreen>().SetShouldFadeIn(true);
		this.dialog.GetComponent<KScreen>().Show(true);
	}

	// Token: 0x06005B4A RID: 23370 RVA: 0x0021445C File Offset: 0x0021265C
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().IntroNIS, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.StartPersistentSnapshots();
		MusicManager.instance.StopSong(this.WelcomeMusic, true, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		MusicManager.instance.WattsonStartDynamicMusic();
		AudioMixer.instance.activeNIS = false;
		DemoTimer.Instance.CountdownActive = true;
		SpeedControlScreen.Instance.Unpause(false);
		CameraController.Instance.DisableUserCameraControl = false;
		foreach (SchedulerHandle schedulerHandle in this.scheduleHandles)
		{
			schedulerHandle.ClearScheduler();
		}
		UIScheduler.Instance.Schedule("fadeInUI", 0.5f, delegate(object d)
		{
			foreach (KScreen kscreen in this.hideScreensWhileActive)
			{
				if (!(kscreen == null))
				{
					kscreen.SetShouldFadeIn(true);
					kscreen.Show(true);
				}
			}
			CameraController.Instance.SetMaxOrthographicSize(20f);
			Game.Instance.StartDelayedInitialSave();
			UIScheduler.Instance.Schedule("InitialScreenshot", 1f, delegate(object data)
			{
				Game.Instance.timelapser.InitialScreenshot();
			}, null, null);
			GameScheduler.Instance.Schedule("BasicTutorial", 1.5f, delegate(object data)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Basics, true);
			}, null, null);
			GameScheduler.Instance.Schedule("WelcomeTutorial", 2f, delegate(object data)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Welcome, true);
			}, null, null);
			GameScheduler.Instance.Schedule("DiggingTutorial", 420f, delegate(object data)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Digging, true);
			}, null, null);
		}, null, null);
		Game.Instance.SetGameStarted();
		if (TopLeftControlScreen.Instance != null)
		{
			TopLeftControlScreen.Instance.RefreshName();
		}
	}

	// Token: 0x06005B4B RID: 23371 RVA: 0x00214564 File Offset: 0x00212764
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			CameraController.Instance.CameraGoHome(2f);
			this.Deactivate();
		}
		e.Consumed = true;
	}

	// Token: 0x06005B4C RID: 23372 RVA: 0x0021458B File Offset: 0x0021278B
	public override void OnKeyUp(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x06005B4D RID: 23373 RVA: 0x00214594 File Offset: 0x00212794
	private void OnNewBaseCreated(object data)
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x04003C26 RID: 15398
	private const float STARTTIME = 0.1f;

	// Token: 0x04003C27 RID: 15399
	private const float ENDTIME = 6.6f;

	// Token: 0x04003C28 RID: 15400
	private const float ALPHA_SPEED = 0.01f;

	// Token: 0x04003C29 RID: 15401
	private const float expandedHeight = 300f;

	// Token: 0x04003C2A RID: 15402
	[SerializeField]
	private GameObject dialog;

	// Token: 0x04003C2B RID: 15403
	[SerializeField]
	private RectTransform content;

	// Token: 0x04003C2C RID: 15404
	[SerializeField]
	private LocText message;

	// Token: 0x04003C2D RID: 15405
	[SerializeField]
	private Image bg;

	// Token: 0x04003C2E RID: 15406
	[SerializeField]
	private KButton button;

	// Token: 0x04003C2F RID: 15407
	[SerializeField]
	private EventReference dialogSound;

	// Token: 0x04003C30 RID: 15408
	private List<KScreen> hideScreensWhileActive = new List<KScreen>();

	// Token: 0x04003C31 RID: 15409
	private bool startFade;

	// Token: 0x04003C32 RID: 15410
	private List<SchedulerHandle> scheduleHandles = new List<SchedulerHandle>();

	// Token: 0x04003C33 RID: 15411
	private static readonly HashedString[] WorkLoopAnims = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04003C34 RID: 15412
	private int birthsComplete;

	// Token: 0x02001C4F RID: 7247
	public class Tuning : TuningData<WattsonMessage.Tuning>
	{
		// Token: 0x040082D0 RID: 33488
		public float initialOrthographicSize;
	}
}
