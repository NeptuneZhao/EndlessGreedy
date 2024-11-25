using System;
using UnityEngine;

// Token: 0x02000861 RID: 2145
public class DreamBubble : KMonoBehaviour
{
	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x06003BC1 RID: 15297 RVA: 0x00149092 File Offset: 0x00147292
	// (set) Token: 0x06003BC0 RID: 15296 RVA: 0x00149089 File Offset: 0x00147289
	public bool IsVisible { get; private set; }

	// Token: 0x06003BC2 RID: 15298 RVA: 0x0014909A File Offset: 0x0014729A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.dreamBackgroundComponent.SetSymbolVisiblity(this.snapToPivotSymbol, false);
		this.SetVisibility(false);
	}

	// Token: 0x06003BC3 RID: 15299 RVA: 0x001490C0 File Offset: 0x001472C0
	public void Tick(float dt)
	{
		if (this._currentDream != null && this._currentDream.Icons.Length != 0)
		{
			float num = this._timePassedSinceDreamStarted / this._currentDream.secondPerImage;
			int num2 = Mathf.FloorToInt(num);
			float num3 = num - (float)num2;
			int num4 = (int)Mathf.Repeat((float)Mathf.FloorToInt(num), (float)this._currentDream.Icons.Length);
			if (this.dreamContentComponent.sprite != this._currentDream.Icons[num4])
			{
				this.dreamContentComponent.sprite = this._currentDream.Icons[num4];
			}
			this.dreamContentComponent.rectTransform.localScale = Vector3.one * num3;
			this._color.a = (Mathf.Sin(num3 * 6.2831855f - 1.5707964f) + 1f) * 0.5f;
			this.dreamContentComponent.color = this._color;
			this._timePassedSinceDreamStarted += dt;
		}
	}

	// Token: 0x06003BC4 RID: 15300 RVA: 0x001491BC File Offset: 0x001473BC
	public void SetDream(Dream dream)
	{
		this._currentDream = dream;
		this.dreamBackgroundComponent.Stop();
		this.dreamBackgroundComponent.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim(dream.BackgroundAnim)
		};
		this.dreamContentComponent.color = this._color;
		this.dreamContentComponent.enabled = (dream != null && dream.Icons != null && dream.Icons.Length != 0);
		this._timePassedSinceDreamStarted = 0f;
		this._color.a = 0f;
	}

	// Token: 0x06003BC5 RID: 15301 RVA: 0x00149250 File Offset: 0x00147450
	public void SetVisibility(bool visible)
	{
		this.IsVisible = visible;
		this.dreamBackgroundComponent.SetVisiblity(visible);
		this.dreamContentComponent.gameObject.SetActive(visible);
		if (visible)
		{
			if (this._currentDream != null)
			{
				this.dreamBackgroundComponent.Play("dream_loop", KAnim.PlayMode.Loop, 1f, 0f);
			}
			this.dreamBubbleBorderKanim.Play("dream_bubble_loop", KAnim.PlayMode.Loop, 1f, 0f);
			this.maskKanim.Play("dream_bubble_mask", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		this.dreamBackgroundComponent.Stop();
		this.maskKanim.Stop();
		this.dreamBubbleBorderKanim.Stop();
	}

	// Token: 0x06003BC6 RID: 15302 RVA: 0x0014930E File Offset: 0x0014750E
	public void StopDreaming()
	{
		this._currentDream = null;
		this.SetVisibility(false);
	}

	// Token: 0x04002418 RID: 9240
	public KBatchedAnimController dreamBackgroundComponent;

	// Token: 0x04002419 RID: 9241
	public KBatchedAnimController maskKanim;

	// Token: 0x0400241A RID: 9242
	public KBatchedAnimController dreamBubbleBorderKanim;

	// Token: 0x0400241B RID: 9243
	public KImage dreamContentComponent;

	// Token: 0x0400241C RID: 9244
	private const string dreamBackgroundAnimationName = "dream_loop";

	// Token: 0x0400241D RID: 9245
	private const string dreamMaskAnimationName = "dream_bubble_mask";

	// Token: 0x0400241E RID: 9246
	private const string dreamBubbleBorderAnimationName = "dream_bubble_loop";

	// Token: 0x0400241F RID: 9247
	private HashedString snapToPivotSymbol = new HashedString("snapto_pivot");

	// Token: 0x04002421 RID: 9249
	private Dream _currentDream;

	// Token: 0x04002422 RID: 9250
	private float _timePassedSinceDreamStarted;

	// Token: 0x04002423 RID: 9251
	private Color _color = Color.white;

	// Token: 0x04002424 RID: 9252
	private const float PI_2 = 6.2831855f;

	// Token: 0x04002425 RID: 9253
	private const float HALF_PI = 1.5707964f;
}
