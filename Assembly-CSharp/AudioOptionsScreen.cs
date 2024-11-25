using System;
using System.Collections.Generic;
using FMODUnity;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000BE5 RID: 3045
public class AudioOptionsScreen : KModalScreen
{
	// Token: 0x06005CA1 RID: 23713 RVA: 0x0021E9D4 File Offset: 0x0021CBD4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.closeButton.onClick += delegate()
		{
			this.OnClose(base.gameObject);
		};
		this.doneButton.onClick += delegate()
		{
			this.OnClose(base.gameObject);
		};
		this.sliderPool = new UIPool<SliderContainer>(this.sliderPrefab);
		foreach (KeyValuePair<string, AudioMixer.UserVolumeBus> keyValuePair in AudioMixer.instance.userVolumeSettings)
		{
			SliderContainer newSlider = this.sliderPool.GetFreeElement(this.sliderGroup, true);
			this.sliderBusMap.Add(newSlider.slider, keyValuePair.Key);
			newSlider.slider.value = keyValuePair.Value.busLevel;
			newSlider.nameLabel.text = keyValuePair.Value.labelString;
			newSlider.UpdateSliderLabel(keyValuePair.Value.busLevel);
			newSlider.slider.ClearReleaseHandleEvent();
			newSlider.slider.onValueChanged.AddListener(delegate(float value)
			{
				this.OnReleaseHandle(newSlider.slider);
			});
			if (keyValuePair.Key == "Master")
			{
				newSlider.transform.SetSiblingIndex(2);
				newSlider.slider.onValueChanged.AddListener(new UnityAction<float>(this.CheckMasterValue));
				this.CheckMasterValue(keyValuePair.Value.busLevel);
			}
		}
		HierarchyReferences component = this.alwaysPlayMusicButton.GetComponent<HierarchyReferences>();
		GameObject gameObject = component.GetReference("Button").gameObject;
		gameObject.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUSIC_EVERY_CYCLE_TOOLTIP);
		component.GetReference("CheckMark").gameObject.SetActive(MusicManager.instance.alwaysPlayMusic);
		gameObject.GetComponent<KButton>().onClick += delegate()
		{
			this.ToggleAlwaysPlayMusic();
		};
		component.GetReference<LocText>("Label").SetText(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUSIC_EVERY_CYCLE);
		if (!KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayAutomation))
		{
			KPlayerPrefs.SetInt(AudioOptionsScreen.AlwaysPlayAutomation, 1);
		}
		HierarchyReferences component2 = this.alwaysPlayAutomationButton.GetComponent<HierarchyReferences>();
		GameObject gameObject2 = component2.GetReference("Button").gameObject;
		gameObject2.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUTOMATION_SOUNDS_ALWAYS_TOOLTIP);
		gameObject2.GetComponent<KButton>().onClick += delegate()
		{
			this.ToggleAlwaysPlayAutomation();
		};
		component2.GetReference<LocText>("Label").SetText(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUTOMATION_SOUNDS_ALWAYS);
		component2.GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) == 1);
		if (!KPlayerPrefs.HasKey(AudioOptionsScreen.MuteOnFocusLost))
		{
			KPlayerPrefs.SetInt(AudioOptionsScreen.MuteOnFocusLost, 0);
		}
		HierarchyReferences component3 = this.muteOnFocusLostToggle.GetComponent<HierarchyReferences>();
		GameObject gameObject3 = component3.GetReference("Button").gameObject;
		gameObject3.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUTE_ON_FOCUS_LOST_TOOLTIP);
		gameObject3.GetComponent<KButton>().onClick += delegate()
		{
			this.ToggleMuteOnFocusLost();
		};
		component3.GetReference<LocText>("Label").SetText(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUTE_ON_FOCUS_LOST);
		component3.GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1);
	}

	// Token: 0x06005CA2 RID: 23714 RVA: 0x0021ED4C File Offset: 0x0021CF4C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06005CA3 RID: 23715 RVA: 0x0021ED6E File Offset: 0x0021CF6E
	private void CheckMasterValue(float value)
	{
		this.jambell.enabled = (value == 0f);
	}

	// Token: 0x06005CA4 RID: 23716 RVA: 0x0021ED83 File Offset: 0x0021CF83
	private void OnReleaseHandle(KSlider slider)
	{
		AudioMixer.instance.SetUserVolume(this.sliderBusMap[slider], slider.value);
	}

	// Token: 0x06005CA5 RID: 23717 RVA: 0x0021EDA4 File Offset: 0x0021CFA4
	private void ToggleAlwaysPlayMusic()
	{
		MusicManager.instance.alwaysPlayMusic = !MusicManager.instance.alwaysPlayMusic;
		this.alwaysPlayMusicButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(MusicManager.instance.alwaysPlayMusic);
		KPlayerPrefs.SetInt(AudioOptionsScreen.AlwaysPlayMusicKey, MusicManager.instance.alwaysPlayMusic ? 1 : 0);
	}

	// Token: 0x06005CA6 RID: 23718 RVA: 0x0021EE0C File Offset: 0x0021D00C
	private void ToggleAlwaysPlayAutomation()
	{
		KPlayerPrefs.SetInt(AudioOptionsScreen.AlwaysPlayAutomation, (KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) == 1) ? 0 : 1);
		this.alwaysPlayAutomationButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) == 1);
	}

	// Token: 0x06005CA7 RID: 23719 RVA: 0x0021EE64 File Offset: 0x0021D064
	private void ToggleMuteOnFocusLost()
	{
		KPlayerPrefs.SetInt(AudioOptionsScreen.MuteOnFocusLost, (KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1) ? 0 : 1);
		this.muteOnFocusLostToggle.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1);
	}

	// Token: 0x06005CA8 RID: 23720 RVA: 0x0021EEBC File Offset: 0x0021D0BC
	private void BuildAudioDeviceList()
	{
		this.audioDevices.Clear();
		this.audioDeviceOptions.Clear();
		int num;
		RuntimeManager.CoreSystem.getNumDrivers(out num);
		for (int i = 0; i < num; i++)
		{
			KFMOD.AudioDevice audioDevice = default(KFMOD.AudioDevice);
			string name;
			RuntimeManager.CoreSystem.getDriverInfo(i, out name, 64, out audioDevice.guid, out audioDevice.systemRate, out audioDevice.speakerMode, out audioDevice.speakerModeChannels);
			audioDevice.name = name;
			audioDevice.fmod_id = i;
			this.audioDevices.Add(audioDevice);
			this.audioDeviceOptions.Add(new Dropdown.OptionData(audioDevice.name));
		}
	}

	// Token: 0x06005CA9 RID: 23721 RVA: 0x0021EF68 File Offset: 0x0021D168
	private void OnAudioDeviceChanged(int idx)
	{
		RuntimeManager.CoreSystem.setDriver(idx);
		for (int i = 0; i < this.audioDevices.Count; i++)
		{
			if (idx == this.audioDevices[i].fmod_id)
			{
				KFMOD.currentDevice = this.audioDevices[i];
				KPlayerPrefs.SetString("AudioDeviceGuid", KFMOD.currentDevice.guid.ToString());
				return;
			}
		}
	}

	// Token: 0x06005CAA RID: 23722 RVA: 0x0021EFDF File Offset: 0x0021D1DF
	private void OnClose(GameObject go)
	{
		this.alwaysPlayMusicMetric[AudioOptionsScreen.AlwaysPlayMusicKey] = MusicManager.instance.alwaysPlayMusic;
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.alwaysPlayMusicMetric, "AudioOptionsScreen");
		UnityEngine.Object.Destroy(go);
	}

	// Token: 0x04003DE0 RID: 15840
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003DE1 RID: 15841
	[SerializeField]
	private KButton doneButton;

	// Token: 0x04003DE2 RID: 15842
	[SerializeField]
	private SliderContainer sliderPrefab;

	// Token: 0x04003DE3 RID: 15843
	[SerializeField]
	private GameObject sliderGroup;

	// Token: 0x04003DE4 RID: 15844
	[SerializeField]
	private Image jambell;

	// Token: 0x04003DE5 RID: 15845
	[SerializeField]
	private GameObject alwaysPlayMusicButton;

	// Token: 0x04003DE6 RID: 15846
	[SerializeField]
	private GameObject alwaysPlayAutomationButton;

	// Token: 0x04003DE7 RID: 15847
	[SerializeField]
	private GameObject muteOnFocusLostToggle;

	// Token: 0x04003DE8 RID: 15848
	[SerializeField]
	private Dropdown deviceDropdown;

	// Token: 0x04003DE9 RID: 15849
	private UIPool<SliderContainer> sliderPool;

	// Token: 0x04003DEA RID: 15850
	private Dictionary<KSlider, string> sliderBusMap = new Dictionary<KSlider, string>();

	// Token: 0x04003DEB RID: 15851
	public static readonly string AlwaysPlayMusicKey = "AlwaysPlayMusic";

	// Token: 0x04003DEC RID: 15852
	public static readonly string AlwaysPlayAutomation = "AlwaysPlayAutomation";

	// Token: 0x04003DED RID: 15853
	public static readonly string MuteOnFocusLost = "MuteOnFocusLost";

	// Token: 0x04003DEE RID: 15854
	private Dictionary<string, object> alwaysPlayMusicMetric = new Dictionary<string, object>
	{
		{
			AudioOptionsScreen.AlwaysPlayMusicKey,
			null
		}
	};

	// Token: 0x04003DEF RID: 15855
	private List<KFMOD.AudioDevice> audioDevices = new List<KFMOD.AudioDevice>();

	// Token: 0x04003DF0 RID: 15856
	private List<Dropdown.OptionData> audioDeviceOptions = new List<Dropdown.OptionData>();
}
