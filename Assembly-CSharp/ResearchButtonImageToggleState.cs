using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BAE RID: 2990
public class ResearchButtonImageToggleState : ImageToggleState
{
	// Token: 0x06005A86 RID: 23174 RVA: 0x0020D5F0 File Offset: 0x0020B7F0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Research.Instance.Subscribe(-1914338957, new Action<object>(this.UpdateActiveResearch));
		Research.Instance.Subscribe(-125623018, new Action<object>(this.RefreshProgressBar));
		this.toggle = base.GetComponent<KToggle>();
	}

	// Token: 0x06005A87 RID: 23175 RVA: 0x0020D647 File Offset: 0x0020B847
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateActiveResearch(null);
		this.RestartCoroutine();
	}

	// Token: 0x06005A88 RID: 23176 RVA: 0x0020D65C File Offset: 0x0020B85C
	protected override void OnCleanUp()
	{
		this.AbortCoroutine();
		Research.Instance.Unsubscribe(-1914338957, new Action<object>(this.UpdateActiveResearch));
		Research.Instance.Unsubscribe(-125623018, new Action<object>(this.RefreshProgressBar));
		base.OnCleanUp();
	}

	// Token: 0x06005A89 RID: 23177 RVA: 0x0020D6AB File Offset: 0x0020B8AB
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.RestartCoroutine();
	}

	// Token: 0x06005A8A RID: 23178 RVA: 0x0020D6B9 File Offset: 0x0020B8B9
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.AbortCoroutine();
	}

	// Token: 0x06005A8B RID: 23179 RVA: 0x0020D6C7 File Offset: 0x0020B8C7
	private void AbortCoroutine()
	{
		if (this.scrollIconCoroutine != null)
		{
			base.StopCoroutine(this.scrollIconCoroutine);
		}
		this.scrollIconCoroutine = null;
	}

	// Token: 0x06005A8C RID: 23180 RVA: 0x0020D6E4 File Offset: 0x0020B8E4
	private void RestartCoroutine()
	{
		this.AbortCoroutine();
		if (base.gameObject.activeInHierarchy)
		{
			this.scrollIconCoroutine = base.StartCoroutine(this.ScrollIcon());
		}
	}

	// Token: 0x06005A8D RID: 23181 RVA: 0x0020D70C File Offset: 0x0020B90C
	private void UpdateActiveResearch(object o)
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch == null)
		{
			this.currentResearchIcons = null;
		}
		else
		{
			this.currentResearchIcons = new Sprite[activeResearch.tech.unlockedItems.Count];
			for (int i = 0; i < activeResearch.tech.unlockedItems.Count; i++)
			{
				TechItem techItem = activeResearch.tech.unlockedItems[i];
				this.currentResearchIcons[i] = techItem.UISprite();
			}
		}
		this.ResetCoroutineTimers();
		this.RefreshProgressBar(o);
	}

	// Token: 0x06005A8E RID: 23182 RVA: 0x0020D794 File Offset: 0x0020B994
	public void RefreshProgressBar(object o)
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch == null)
		{
			this.progressBar.fillAmount = 0f;
			return;
		}
		this.progressBar.fillAmount = activeResearch.GetTotalPercentageComplete();
	}

	// Token: 0x06005A8F RID: 23183 RVA: 0x0020D7D1 File Offset: 0x0020B9D1
	public void SetProgressBarVisibility(bool viisble)
	{
		this.progressBar.enabled = viisble;
	}

	// Token: 0x06005A90 RID: 23184 RVA: 0x0020D7DF File Offset: 0x0020B9DF
	public override void SetActive()
	{
		base.SetActive();
		this.SetProgressBarVisibility(false);
	}

	// Token: 0x06005A91 RID: 23185 RVA: 0x0020D7EE File Offset: 0x0020B9EE
	public override void SetDisabledActive()
	{
		base.SetDisabledActive();
		this.SetProgressBarVisibility(false);
	}

	// Token: 0x06005A92 RID: 23186 RVA: 0x0020D7FD File Offset: 0x0020B9FD
	public override void SetDisabled()
	{
		base.SetDisabled();
		this.SetProgressBarVisibility(false);
	}

	// Token: 0x06005A93 RID: 23187 RVA: 0x0020D80C File Offset: 0x0020BA0C
	public override void SetInactive()
	{
		base.SetInactive();
		this.SetProgressBarVisibility(true);
		this.RefreshProgressBar(null);
	}

	// Token: 0x06005A94 RID: 23188 RVA: 0x0020D822 File Offset: 0x0020BA22
	private void ResetCoroutineTimers()
	{
		this.mainIconScreenTime = 0f;
		this.itemScreenTime = 0f;
		this.item_idx = -1;
	}

	// Token: 0x170006BF RID: 1727
	// (get) Token: 0x06005A95 RID: 23189 RVA: 0x0020D841 File Offset: 0x0020BA41
	private bool ReadyToDisplayIcons
	{
		get
		{
			return this.progressBar.enabled && this.currentResearchIcons != null && this.item_idx >= 0 && this.item_idx < this.currentResearchIcons.Length;
		}
	}

	// Token: 0x06005A96 RID: 23190 RVA: 0x0020D873 File Offset: 0x0020BA73
	private IEnumerator ScrollIcon()
	{
		while (Application.isPlaying)
		{
			if (this.mainIconScreenTime < this.researchLogoDuration)
			{
				this.toggle.fgImage.Opacity(1f);
				if (this.toggle.fgImage.overrideSprite != null)
				{
					this.toggle.fgImage.overrideSprite = null;
				}
				this.item_idx = 0;
				this.itemScreenTime = 0f;
				this.mainIconScreenTime += Time.unscaledDeltaTime;
				if (this.progressBar.enabled && this.mainIconScreenTime >= this.researchLogoDuration && this.ReadyToDisplayIcons)
				{
					yield return this.toggle.fgImage.FadeAway(this.fadingDuration, () => this.progressBar.enabled && this.mainIconScreenTime >= this.researchLogoDuration && this.ReadyToDisplayIcons);
				}
				yield return null;
			}
			else if (this.ReadyToDisplayIcons)
			{
				if (this.toggle.fgImage.overrideSprite != this.currentResearchIcons[this.item_idx])
				{
					this.toggle.fgImage.overrideSprite = this.currentResearchIcons[this.item_idx];
				}
				yield return this.toggle.fgImage.FadeToVisible(this.fadingDuration, () => this.ReadyToDisplayIcons);
				while (this.itemScreenTime < this.durationPerResearchItemIcon && this.ReadyToDisplayIcons)
				{
					this.itemScreenTime += Time.unscaledDeltaTime;
					yield return null;
				}
				yield return this.toggle.fgImage.FadeAway(this.fadingDuration, () => this.ReadyToDisplayIcons);
				if (this.ReadyToDisplayIcons)
				{
					this.itemScreenTime = 0f;
					this.item_idx++;
				}
				yield return null;
			}
			else
			{
				this.mainIconScreenTime = 0f;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x04003BA1 RID: 15265
	public Image progressBar;

	// Token: 0x04003BA2 RID: 15266
	private KToggle toggle;

	// Token: 0x04003BA3 RID: 15267
	[Header("Scroll Options")]
	public float researchLogoDuration = 5f;

	// Token: 0x04003BA4 RID: 15268
	public float durationPerResearchItemIcon = 0.6f;

	// Token: 0x04003BA5 RID: 15269
	public float fadingDuration = 0.2f;

	// Token: 0x04003BA6 RID: 15270
	private Coroutine scrollIconCoroutine;

	// Token: 0x04003BA7 RID: 15271
	private Sprite[] currentResearchIcons;

	// Token: 0x04003BA8 RID: 15272
	private float mainIconScreenTime;

	// Token: 0x04003BA9 RID: 15273
	private float itemScreenTime;

	// Token: 0x04003BAA RID: 15274
	private int item_idx = -1;
}
