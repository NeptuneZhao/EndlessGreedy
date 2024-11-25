using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000C65 RID: 3173
public class IncrementorToggle : MultiToggle
{
	// Token: 0x06006151 RID: 24913 RVA: 0x00244108 File Offset: 0x00242308
	protected override void Update()
	{
		if (this.clickHeldDown)
		{
			this.totalHeldTime += Time.unscaledDeltaTime;
			if (this.timeToNextIncrement <= 0f)
			{
				this.PlayClickSound();
				this.onClick();
				this.timeToNextIncrement = Mathf.Lerp(this.timeBetweenIncrementsMax, this.timeBetweenIncrementsMin, this.totalHeldTime / 2.5f);
				return;
			}
			this.timeToNextIncrement -= Time.unscaledDeltaTime;
		}
	}

	// Token: 0x06006152 RID: 24914 RVA: 0x00244184 File Offset: 0x00242384
	private void PlayClickSound()
	{
		if (this.play_sound_on_click)
		{
			if (this.states[this.state].on_click_override_sound_path == "")
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click", false));
				return;
			}
			KFMOD.PlayUISound(GlobalAssets.GetSound(this.states[this.state].on_click_override_sound_path, false));
		}
	}

	// Token: 0x06006153 RID: 24915 RVA: 0x002441ED File Offset: 0x002423ED
	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		this.timeToNextIncrement = this.timeBetweenIncrementsMax;
	}

	// Token: 0x06006154 RID: 24916 RVA: 0x00244204 File Offset: 0x00242404
	public override void OnPointerDown(PointerEventData eventData)
	{
		if (!this.clickHeldDown)
		{
			this.clickHeldDown = true;
			this.PlayClickSound();
			if (this.onClick != null)
			{
				this.onClick();
			}
		}
		if (this.states.Length - 1 < this.state)
		{
			global::Debug.LogWarning("Multi toggle has too few / no states");
		}
		base.RefreshHoverColor();
	}

	// Token: 0x06006155 RID: 24917 RVA: 0x0024425B File Offset: 0x0024245B
	public override void OnPointerClick(PointerEventData eventData)
	{
		base.RefreshHoverColor();
	}

	// Token: 0x040041F7 RID: 16887
	private float timeBetweenIncrementsMin = 0.033f;

	// Token: 0x040041F8 RID: 16888
	private float timeBetweenIncrementsMax = 0.25f;

	// Token: 0x040041F9 RID: 16889
	private const float incrementAccelerationScale = 2.5f;

	// Token: 0x040041FA RID: 16890
	private float timeToNextIncrement;
}
