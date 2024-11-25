using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000C28 RID: 3112
public class DLCBetaMessageScreen : KModalScreen
{
	// Token: 0x06005F66 RID: 24422 RVA: 0x00236A84 File Offset: 0x00234C84
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.confirmButton.onClick += delegate()
		{
			base.gameObject.SetActive(false);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot, STOP_MODE.ALLOWFADEOUT);
		};
		this.quitButton.onClick += delegate()
		{
			App.Quit();
		};
	}

	// Token: 0x06005F67 RID: 24423 RVA: 0x00236AD8 File Offset: 0x00234CD8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!this.betaIsLive || (Application.isEditor && this.skipInEditor) || !DlcManager.GetActiveDLCIds().Contains("DLC2_ID"))
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
	}

	// Token: 0x06005F68 RID: 24424 RVA: 0x00236B34 File Offset: 0x00234D34
	private void Update()
	{
		this.logo.rectTransform().localPosition = new Vector3(0f, Mathf.Sin(Time.realtimeSinceStartup) * 7.5f);
	}

	// Token: 0x04004035 RID: 16437
	public RectTransform logo;

	// Token: 0x04004036 RID: 16438
	public KButton confirmButton;

	// Token: 0x04004037 RID: 16439
	public KButton quitButton;

	// Token: 0x04004038 RID: 16440
	public LocText bodyText;

	// Token: 0x04004039 RID: 16441
	public RectTransform messageContainer;

	// Token: 0x0400403A RID: 16442
	private bool betaIsLive;

	// Token: 0x0400403B RID: 16443
	private bool skipInEditor;
}
