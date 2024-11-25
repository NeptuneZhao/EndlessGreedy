using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BAF RID: 2991
public static class ResearchButtonImageToggleStateUtilityFunctions
{
	// Token: 0x06005A9B RID: 23195 RVA: 0x0020D8E8 File Offset: 0x0020BAE8
	public static void Opacity(this Graphic graphic, float opacity)
	{
		Color color = graphic.color;
		color.a = opacity;
		graphic.color = color;
	}

	// Token: 0x06005A9C RID: 23196 RVA: 0x0020D90C File Offset: 0x0020BB0C
	public static WaitUntil FadeAway(this Graphic graphic, float duration, Func<bool> assertCondition = null)
	{
		float timer = 0f;
		float startingOpacity = graphic.color.a;
		return new WaitUntil(delegate()
		{
			if (timer >= duration || (assertCondition != null && !assertCondition()))
			{
				graphic.Opacity(0f);
				return true;
			}
			float num = timer / duration;
			num = 1f - num;
			graphic.Opacity(startingOpacity * num);
			timer += Time.unscaledDeltaTime;
			return false;
		});
	}

	// Token: 0x06005A9D RID: 23197 RVA: 0x0020D964 File Offset: 0x0020BB64
	public static WaitUntil FadeToVisible(this Graphic graphic, float duration, Func<bool> assertCondition = null)
	{
		float timer = 0f;
		float startingOpacity = graphic.color.a;
		float remainingOpacity = 1f - graphic.color.a;
		return new WaitUntil(delegate()
		{
			if (timer >= duration || (assertCondition != null && !assertCondition()))
			{
				graphic.Opacity(1f);
				return true;
			}
			float num = timer / duration;
			graphic.Opacity(startingOpacity + remainingOpacity * num);
			timer += Time.unscaledDeltaTime;
			return false;
		});
	}
}
