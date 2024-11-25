using System;
using UnityEngine;

// Token: 0x02000413 RID: 1043
public static class PresUtil
{
	// Token: 0x0600161F RID: 5663 RVA: 0x00077E48 File Offset: 0x00076048
	public static Promise MoveAndFade(RectTransform rect, Vector2 targetAnchoredPosition, float targetAlpha, float duration, Easing.EasingFn easing = null)
	{
		CanvasGroup canvasGroup = rect.FindOrAddComponent<CanvasGroup>();
		return rect.FindOrAddComponent<CoroutineRunner>().Run(Updater.Parallel(new Updater[]
		{
			Updater.Ease(delegate(float f)
			{
				canvasGroup.alpha = f;
			}, canvasGroup.alpha, targetAlpha, duration, easing, -1f),
			Updater.Ease(delegate(Vector2 v2)
			{
				rect.anchoredPosition = v2;
			}, rect.anchoredPosition, targetAnchoredPosition, duration, easing, -1f)
		}));
	}

	// Token: 0x06001620 RID: 5664 RVA: 0x00077EEC File Offset: 0x000760EC
	public static Promise OffsetFromAndFade(RectTransform rect, Vector2 offset, float targetAlpha, float duration, Easing.EasingFn easing = null)
	{
		Vector2 anchoredPosition = rect.anchoredPosition;
		return PresUtil.MoveAndFade(rect, offset + anchoredPosition, targetAlpha, duration, easing);
	}

	// Token: 0x06001621 RID: 5665 RVA: 0x00077F14 File Offset: 0x00076114
	public static Promise OffsetToAndFade(RectTransform rect, Vector2 offset, float targetAlpha, float duration, Easing.EasingFn easing = null)
	{
		Vector2 anchoredPosition = rect.anchoredPosition;
		rect.anchoredPosition = offset + anchoredPosition;
		return PresUtil.MoveAndFade(rect, anchoredPosition, targetAlpha, duration, easing);
	}
}
