using System;
using UnityEngine;

// Token: 0x02000C9F RID: 3231
[AddComponentMenu("KMonoBehaviour/scripts/MainMenuIntroShort")]
public class MainMenuIntroShort : KMonoBehaviour
{
	// Token: 0x06006383 RID: 25475 RVA: 0x002515F4 File Offset: 0x0024F7F4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		string @string = KPlayerPrefs.GetString("PlayShortOnLaunch", "");
		if (!string.IsNullOrEmpty(MainMenu.Instance.IntroShortName) && @string != MainMenu.Instance.IntroShortName)
		{
			VideoScreen component = KScreenManager.AddChild(FrontEndManager.Instance.gameObject, ScreenPrefabs.Instance.VideoScreen.gameObject).GetComponent<VideoScreen>();
			component.PlayVideo(Assets.GetVideo(MainMenu.Instance.IntroShortName), false, AudioMixerSnapshots.Get().MainMenuVideoPlayingSnapshot, false);
			component.OnStop = (System.Action)Delegate.Combine(component.OnStop, new System.Action(delegate()
			{
				KPlayerPrefs.SetString("PlayShortOnLaunch", MainMenu.Instance.IntroShortName);
				base.gameObject.SetActive(false);
			}));
			return;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x04004393 RID: 17299
	[SerializeField]
	private bool alwaysPlay;
}
