using System;
using System.Collections;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000DC8 RID: 3528
public class SpeedControlScreen : KScreen
{
	// Token: 0x170007D4 RID: 2004
	// (get) Token: 0x06006FEC RID: 28652 RVA: 0x002A38B9 File Offset: 0x002A1AB9
	// (set) Token: 0x06006FED RID: 28653 RVA: 0x002A38C0 File Offset: 0x002A1AC0
	public static SpeedControlScreen Instance { get; private set; }

	// Token: 0x06006FEE RID: 28654 RVA: 0x002A38C8 File Offset: 0x002A1AC8
	public static void DestroyInstance()
	{
		SpeedControlScreen.Instance = null;
	}

	// Token: 0x170007D5 RID: 2005
	// (get) Token: 0x06006FEF RID: 28655 RVA: 0x002A38D0 File Offset: 0x002A1AD0
	public bool IsPaused
	{
		get
		{
			return this.pauseCount > 0;
		}
	}

	// Token: 0x06006FF0 RID: 28656 RVA: 0x002A38DC File Offset: 0x002A1ADC
	protected override void OnPrefabInit()
	{
		SpeedControlScreen.Instance = this;
		this.pauseButton = this.pauseButtonWidget.GetComponent<KToggle>();
		this.slowButton = this.speedButtonWidget_slow.GetComponent<KToggle>();
		this.mediumButton = this.speedButtonWidget_medium.GetComponent<KToggle>();
		this.fastButton = this.speedButtonWidget_fast.GetComponent<KToggle>();
		KToggle[] array = new KToggle[]
		{
			this.pauseButton,
			this.slowButton,
			this.mediumButton,
			this.fastButton
		};
		for (int i = 0; i < array.Length; i++)
		{
			array[i].soundPlayer.Enabled = false;
		}
		this.slowButton.onClick += delegate()
		{
			this.PlaySpeedChangeSound(1f);
			this.SetSpeed(0);
		};
		this.mediumButton.onClick += delegate()
		{
			this.PlaySpeedChangeSound(2f);
			this.SetSpeed(1);
		};
		this.fastButton.onClick += delegate()
		{
			this.PlaySpeedChangeSound(3f);
			this.SetSpeed(2);
		};
		this.pauseButton.onClick += delegate()
		{
			this.TogglePause(true);
		};
		this.speedButtonWidget_slow.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_SLOW, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.speedButtonWidget_medium.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_MEDIUM, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.speedButtonWidget_fast.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_FAST, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.playButtonWidget.GetComponent<KButton>().onClick += delegate()
		{
			this.TogglePause(true);
		};
		KInputManager.InputChange.AddListener(new UnityAction(this.ResetToolTip));
	}

	// Token: 0x06006FF1 RID: 28657 RVA: 0x002A3A7D File Offset: 0x002A1C7D
	protected override void OnSpawn()
	{
		if (SaveGame.Instance != null)
		{
			this.speed = SaveGame.Instance.GetSpeed();
			this.SetSpeed(this.speed);
		}
		base.OnSpawn();
		this.OnChanged();
	}

	// Token: 0x06006FF2 RID: 28658 RVA: 0x002A3AB4 File Offset: 0x002A1CB4
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.ResetToolTip));
		base.OnForcedCleanUp();
	}

	// Token: 0x06006FF3 RID: 28659 RVA: 0x002A3AD2 File Offset: 0x002A1CD2
	public int GetSpeed()
	{
		return this.speed;
	}

	// Token: 0x06006FF4 RID: 28660 RVA: 0x002A3ADC File Offset: 0x002A1CDC
	public void SetSpeed(int Speed)
	{
		this.speed = Speed % 3;
		switch (this.speed)
		{
		case 0:
			this.slowButton.Select();
			this.slowButton.isOn = true;
			this.mediumButton.isOn = false;
			this.fastButton.isOn = false;
			break;
		case 1:
			this.mediumButton.Select();
			this.slowButton.isOn = false;
			this.mediumButton.isOn = true;
			this.fastButton.isOn = false;
			break;
		case 2:
			this.fastButton.Select();
			this.slowButton.isOn = false;
			this.mediumButton.isOn = false;
			this.fastButton.isOn = true;
			break;
		}
		this.OnSpeedChange();
	}

	// Token: 0x06006FF5 RID: 28661 RVA: 0x002A3BA7 File Offset: 0x002A1DA7
	public void ToggleRidiculousSpeed()
	{
		if (this.ultraSpeed == 3f)
		{
			this.ultraSpeed = 10f;
		}
		else
		{
			this.ultraSpeed = 3f;
		}
		this.speed = 2;
		this.OnChanged();
	}

	// Token: 0x06006FF6 RID: 28662 RVA: 0x002A3BDB File Offset: 0x002A1DDB
	public void TogglePause(bool playsound = true)
	{
		if (this.IsPaused)
		{
			this.Unpause(playsound);
			return;
		}
		this.Pause(playsound, false);
	}

	// Token: 0x06006FF7 RID: 28663 RVA: 0x002A3BF8 File Offset: 0x002A1DF8
	public void ResetToolTip()
	{
		this.speedButtonWidget_slow.GetComponent<ToolTip>().ClearMultiStringTooltip();
		this.speedButtonWidget_medium.GetComponent<ToolTip>().ClearMultiStringTooltip();
		this.speedButtonWidget_fast.GetComponent<ToolTip>().ClearMultiStringTooltip();
		this.speedButtonWidget_slow.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_SLOW, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.speedButtonWidget_medium.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_MEDIUM, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.speedButtonWidget_fast.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_FAST, global::Action.CycleSpeed), this.TooltipTextStyle);
		if (this.pauseButton.isOn)
		{
			this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
			this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.UNPAUSE, global::Action.TogglePause), this.TooltipTextStyle);
			return;
		}
		this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
		this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.PAUSE, global::Action.TogglePause), this.TooltipTextStyle);
	}

	// Token: 0x06006FF8 RID: 28664 RVA: 0x002A3D28 File Offset: 0x002A1F28
	public void Pause(bool playSound = true, bool isCrashed = false)
	{
		this.pauseCount++;
		if (this.pauseCount == 1)
		{
			if (playSound)
			{
				if (isCrashed)
				{
					KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Crash_Screen", false));
				}
				else
				{
					KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Speed_Pause", false));
				}
				if (SoundListenerController.Instance != null)
				{
					SoundListenerController.Instance.SetLoopingVolume(0f);
				}
			}
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().SpeedPausedMigrated);
			MusicManager.instance.SetDynamicMusicPaused();
			this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
			this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.UNPAUSE, global::Action.TogglePause), this.TooltipTextStyle);
			this.pauseButton.isOn = true;
			this.OnPause();
		}
	}

	// Token: 0x06006FF9 RID: 28665 RVA: 0x002A3DFC File Offset: 0x002A1FFC
	public void Unpause(bool playSound = true)
	{
		this.pauseCount = Mathf.Max(0, this.pauseCount - 1);
		if (this.pauseCount == 0)
		{
			if (playSound)
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Speed_Unpause", false));
				if (SoundListenerController.Instance != null)
				{
					SoundListenerController.Instance.SetLoopingVolume(1f);
				}
			}
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().SpeedPausedMigrated, STOP_MODE.ALLOWFADEOUT);
			MusicManager.instance.SetDynamicMusicUnpaused();
			this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
			this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.PAUSE, global::Action.TogglePause), this.TooltipTextStyle);
			this.pauseButton.isOn = false;
			this.SetSpeed(this.speed);
			this.OnPlay();
		}
	}

	// Token: 0x06006FFA RID: 28666 RVA: 0x002A3ECC File Offset: 0x002A20CC
	private void OnPause()
	{
		this.OnChanged();
	}

	// Token: 0x06006FFB RID: 28667 RVA: 0x002A3ED4 File Offset: 0x002A20D4
	private void OnPlay()
	{
		this.OnChanged();
	}

	// Token: 0x06006FFC RID: 28668 RVA: 0x002A3EDC File Offset: 0x002A20DC
	public void OnSpeedChange()
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.OnChanged();
	}

	// Token: 0x06006FFD RID: 28669 RVA: 0x002A3EEC File Offset: 0x002A20EC
	private void OnChanged()
	{
		if (this.IsPaused)
		{
			Time.timeScale = 0f;
			return;
		}
		if (this.speed == 0)
		{
			Time.timeScale = this.normalSpeed;
			return;
		}
		if (this.speed == 1)
		{
			Time.timeScale = this.fastSpeed;
			return;
		}
		if (this.speed == 2)
		{
			Time.timeScale = this.ultraSpeed;
		}
	}

	// Token: 0x06006FFE RID: 28670 RVA: 0x002A3F4C File Offset: 0x002A214C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.TogglePause))
		{
			this.TogglePause(true);
			return;
		}
		if (e.TryConsume(global::Action.CycleSpeed))
		{
			this.PlaySpeedChangeSound((float)((this.speed + 1) % 3 + 1));
			this.SetSpeed(this.speed + 1);
			this.OnSpeedChange();
			return;
		}
		if (e.TryConsume(global::Action.SpeedUp))
		{
			this.speed++;
			this.speed = Math.Min(this.speed, 2);
			this.SetSpeed(this.speed);
			return;
		}
		if (e.TryConsume(global::Action.SlowDown))
		{
			this.speed--;
			this.speed = Math.Max(this.speed, 0);
			this.SetSpeed(this.speed);
		}
	}

	// Token: 0x06006FFF RID: 28671 RVA: 0x002A400C File Offset: 0x002A220C
	private void PlaySpeedChangeSound(float speed)
	{
		string sound = GlobalAssets.GetSound("Speed_Change", false);
		if (sound != null)
		{
			EventInstance instance = SoundEvent.BeginOneShot(sound, Vector3.zero, 1f, false);
			instance.setParameterByName("Speed", speed, false);
			SoundEvent.EndOneShot(instance);
		}
	}

	// Token: 0x06007000 RID: 28672 RVA: 0x002A4054 File Offset: 0x002A2254
	public void DebugStepFrame()
	{
		DebugUtil.LogArgs(new object[]
		{
			string.Format("Stepping one frame {0} ({1})", GameClock.Instance.GetTime(), GameClock.Instance.GetTime() / 600f)
		});
		this.stepTime = Time.time;
		this.Unpause(false);
		base.StartCoroutine(this.DebugStepFrameDelay());
	}

	// Token: 0x06007001 RID: 28673 RVA: 0x002A40BC File Offset: 0x002A22BC
	private IEnumerator DebugStepFrameDelay()
	{
		yield return null;
		DebugUtil.LogArgs(new object[]
		{
			"Stepped one frame",
			Time.time - this.stepTime,
			"seconds"
		});
		this.Pause(false, false);
		yield break;
	}

	// Token: 0x04004CB6 RID: 19638
	public GameObject playButtonWidget;

	// Token: 0x04004CB7 RID: 19639
	public GameObject pauseButtonWidget;

	// Token: 0x04004CB8 RID: 19640
	public Image playIcon;

	// Token: 0x04004CB9 RID: 19641
	public Image pauseIcon;

	// Token: 0x04004CBA RID: 19642
	[SerializeField]
	private TextStyleSetting TooltipTextStyle;

	// Token: 0x04004CBB RID: 19643
	public GameObject speedButtonWidget_slow;

	// Token: 0x04004CBC RID: 19644
	public GameObject speedButtonWidget_medium;

	// Token: 0x04004CBD RID: 19645
	public GameObject speedButtonWidget_fast;

	// Token: 0x04004CBE RID: 19646
	public GameObject mainMenuWidget;

	// Token: 0x04004CBF RID: 19647
	public float normalSpeed;

	// Token: 0x04004CC0 RID: 19648
	public float fastSpeed;

	// Token: 0x04004CC1 RID: 19649
	public float ultraSpeed;

	// Token: 0x04004CC2 RID: 19650
	private KToggle pauseButton;

	// Token: 0x04004CC3 RID: 19651
	private KToggle slowButton;

	// Token: 0x04004CC4 RID: 19652
	private KToggle mediumButton;

	// Token: 0x04004CC5 RID: 19653
	private KToggle fastButton;

	// Token: 0x04004CC6 RID: 19654
	private int speed;

	// Token: 0x04004CC7 RID: 19655
	private int pauseCount;

	// Token: 0x04004CC9 RID: 19657
	private float stepTime;
}
