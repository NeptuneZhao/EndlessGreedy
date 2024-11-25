using System;
using Klei;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C2F RID: 3119
public class DemoTimer : MonoBehaviour
{
	// Token: 0x06005FBA RID: 24506 RVA: 0x00238E5D File Offset: 0x0023705D
	public static void DestroyInstance()
	{
		DemoTimer.Instance = null;
	}

	// Token: 0x06005FBB RID: 24507 RVA: 0x00238E68 File Offset: 0x00237068
	private void Start()
	{
		DemoTimer.Instance = this;
		if (GenericGameSettings.instance != null)
		{
			if (GenericGameSettings.instance.demoMode)
			{
				this.duration = (float)GenericGameSettings.instance.demoTime;
				this.labelText.gameObject.SetActive(GenericGameSettings.instance.showDemoTimer);
				this.clockImage.gameObject.SetActive(GenericGameSettings.instance.showDemoTimer);
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
		this.duration = (float)GenericGameSettings.instance.demoTime;
		this.fadeOutScreen = Util.KInstantiateUI(this.Prefab_FadeOutScreen, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false);
		Image component = this.fadeOutScreen.GetComponent<Image>();
		component.raycastTarget = false;
		this.fadeOutColor = component.color;
		this.fadeOutColor.a = 0f;
		this.fadeOutScreen.GetComponent<Image>().color = this.fadeOutColor;
	}

	// Token: 0x06005FBC RID: 24508 RVA: 0x00238F68 File Offset: 0x00237168
	private void Update()
	{
		if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.BackQuote))
		{
			this.CountdownActive = !this.CountdownActive;
			this.UpdateLabel();
		}
		if (this.demoOver || !this.CountdownActive)
		{
			return;
		}
		if (this.beginTime == -1f)
		{
			this.beginTime = Time.unscaledTime;
		}
		this.elapsed = Mathf.Clamp(0f, Time.unscaledTime - this.beginTime, this.duration);
		if (this.elapsed + 5f >= this.duration)
		{
			float f = (this.duration - this.elapsed) / 5f;
			this.fadeOutColor.a = Mathf.Min(1f, 1f - Mathf.Sqrt(f));
			this.fadeOutScreen.GetComponent<Image>().color = this.fadeOutColor;
		}
		if (this.elapsed >= this.duration)
		{
			this.EndDemo();
		}
		this.UpdateLabel();
	}

	// Token: 0x06005FBD RID: 24509 RVA: 0x00239070 File Offset: 0x00237270
	private void UpdateLabel()
	{
		int num = Mathf.RoundToInt(this.duration - this.elapsed);
		int num2 = Mathf.FloorToInt((float)(num / 60));
		int num3 = num % 60;
		this.labelText.text = string.Concat(new string[]
		{
			UI.DEMOOVERSCREEN.TIMEREMAINING,
			" ",
			num2.ToString("00"),
			":",
			num3.ToString("00")
		});
		if (!this.CountdownActive)
		{
			this.labelText.text = UI.DEMOOVERSCREEN.TIMERINACTIVE;
		}
	}

	// Token: 0x06005FBE RID: 24510 RVA: 0x0023910C File Offset: 0x0023730C
	public void EndDemo()
	{
		if (this.demoOver)
		{
			return;
		}
		this.demoOver = true;
		Util.KInstantiateUI(this.Prefab_DemoOverScreen, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false).GetComponent<DemoOverScreen>().Show(true);
	}

	// Token: 0x0400407E RID: 16510
	public static DemoTimer Instance;

	// Token: 0x0400407F RID: 16511
	public LocText labelText;

	// Token: 0x04004080 RID: 16512
	public Image clockImage;

	// Token: 0x04004081 RID: 16513
	public GameObject Prefab_DemoOverScreen;

	// Token: 0x04004082 RID: 16514
	public GameObject Prefab_FadeOutScreen;

	// Token: 0x04004083 RID: 16515
	private float duration;

	// Token: 0x04004084 RID: 16516
	private float elapsed;

	// Token: 0x04004085 RID: 16517
	private bool demoOver;

	// Token: 0x04004086 RID: 16518
	private float beginTime = -1f;

	// Token: 0x04004087 RID: 16519
	public bool CountdownActive;

	// Token: 0x04004088 RID: 16520
	private GameObject fadeOutScreen;

	// Token: 0x04004089 RID: 16521
	private Color fadeOutColor;
}
