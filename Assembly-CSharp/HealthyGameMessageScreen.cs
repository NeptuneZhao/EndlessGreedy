using System;
using UnityEngine;

// Token: 0x02000C5D RID: 3165
[AddComponentMenu("KMonoBehaviour/scripts/HealthyGameMessageScreen")]
public class HealthyGameMessageScreen : KMonoBehaviour
{
	// Token: 0x0600613A RID: 24890 RVA: 0x00243CC3 File Offset: 0x00241EC3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.confirmButton.onClick += delegate()
		{
			this.PlayIntroShort();
		};
		this.confirmButton.gameObject.SetActive(false);
	}

	// Token: 0x0600613B RID: 24891 RVA: 0x00243CF4 File Offset: 0x00241EF4
	private void PlayIntroShort()
	{
		string @string = KPlayerPrefs.GetString("PlayShortOnLaunch", "");
		if (!string.IsNullOrEmpty(MainMenu.Instance.IntroShortName) && @string != MainMenu.Instance.IntroShortName)
		{
			VideoScreen component = KScreenManager.AddChild(FrontEndManager.Instance.gameObject, ScreenPrefabs.Instance.VideoScreen.gameObject).GetComponent<VideoScreen>();
			component.PlayVideo(Assets.GetVideo(MainMenu.Instance.IntroShortName), false, AudioMixerSnapshots.Get().MainMenuVideoPlayingSnapshot, false);
			component.OnStop = (System.Action)Delegate.Combine(component.OnStop, new System.Action(delegate()
			{
				KPlayerPrefs.SetString("PlayShortOnLaunch", MainMenu.Instance.IntroShortName);
				if (base.gameObject != null)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}));
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600613C RID: 24892 RVA: 0x00243DA5 File Offset: 0x00241FA5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600613D RID: 24893 RVA: 0x00243DB8 File Offset: 0x00241FB8
	private void Update()
	{
		if (!DistributionPlatform.Inst.IsDLCStatusReady())
		{
			return;
		}
		if (this.isFirstUpdate)
		{
			this.isFirstUpdate = false;
			this.spawnTime = Time.unscaledTime;
			return;
		}
		float num = Mathf.Min(Time.unscaledDeltaTime, 0.033333335f);
		float num2 = Time.unscaledTime - this.spawnTime;
		if (num2 < this.totalTime - this.fadeTime)
		{
			this.canvasGroup.alpha = this.canvasGroup.alpha + num * (1f / this.fadeTime);
			return;
		}
		if (num2 >= this.totalTime + 0.75f)
		{
			this.canvasGroup.alpha = 1f;
			this.confirmButton.gameObject.SetActive(true);
			return;
		}
		if (num2 >= this.totalTime - this.fadeTime)
		{
			this.canvasGroup.alpha = this.canvasGroup.alpha - num * (1f / this.fadeTime);
		}
	}

	// Token: 0x040041EC RID: 16876
	public KButton confirmButton;

	// Token: 0x040041ED RID: 16877
	public CanvasGroup canvasGroup;

	// Token: 0x040041EE RID: 16878
	private float spawnTime;

	// Token: 0x040041EF RID: 16879
	private float totalTime = 10f;

	// Token: 0x040041F0 RID: 16880
	private float fadeTime = 1.5f;

	// Token: 0x040041F1 RID: 16881
	private bool isFirstUpdate = true;
}
