using System;
using UnityEngine;

// Token: 0x0200040B RID: 1035
public class Easing
{
	// Token: 0x04000C4E RID: 3150
	public const Easing.EasingFn PARAM_DEFAULT = null;

	// Token: 0x04000C4F RID: 3151
	public static readonly Easing.EasingFn Linear = (float x) => x;

	// Token: 0x04000C50 RID: 3152
	public static readonly Easing.EasingFn SmoothStep = (float x) => Mathf.SmoothStep(0f, 1f, x);

	// Token: 0x04000C51 RID: 3153
	public static readonly Easing.EasingFn QuadIn = (float x) => x * x;

	// Token: 0x04000C52 RID: 3154
	public static readonly Easing.EasingFn QuadOut = (float x) => 1f - (1f - x) * (1f - x);

	// Token: 0x04000C53 RID: 3155
	public static readonly Easing.EasingFn QuadInOut = delegate(float x)
	{
		if ((double)x >= 0.5)
		{
			return 1f - Mathf.Pow(-2f * x + 2f, 2f) / 2f;
		}
		return 2f * x * x;
	};

	// Token: 0x04000C54 RID: 3156
	public static readonly Easing.EasingFn CubicIn = (float x) => x * x * x;

	// Token: 0x04000C55 RID: 3157
	public static readonly Easing.EasingFn CubicOut = (float x) => 1f - Mathf.Pow(1f - x, 3f);

	// Token: 0x04000C56 RID: 3158
	public static readonly Easing.EasingFn CubicInOut = delegate(float x)
	{
		if ((double)x >= 0.5)
		{
			return 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;
		}
		return 4f * x * x * x;
	};

	// Token: 0x04000C57 RID: 3159
	public static readonly Easing.EasingFn QuartIn = (float x) => x * x * x * x;

	// Token: 0x04000C58 RID: 3160
	public static readonly Easing.EasingFn QuartOut = (float x) => 1f - Mathf.Pow(1f - x, 4f);

	// Token: 0x04000C59 RID: 3161
	public static readonly Easing.EasingFn QuartInOut = delegate(float x)
	{
		if ((double)x >= 0.5)
		{
			return 1f - Mathf.Pow(-2f * x + 2f, 4f) / 2f;
		}
		return 8f * x * x * x * x;
	};

	// Token: 0x04000C5A RID: 3162
	public static readonly Easing.EasingFn QuintIn = (float x) => x * x * x * x * x;

	// Token: 0x04000C5B RID: 3163
	public static readonly Easing.EasingFn QuintOut = (float x) => 1f - Mathf.Pow(1f - x, 5f);

	// Token: 0x04000C5C RID: 3164
	public static readonly Easing.EasingFn QuintInOut = delegate(float x)
	{
		if ((double)x >= 0.5)
		{
			return 1f - Mathf.Pow(-2f * x + 2f, 5f) / 2f;
		}
		return 16f * x * x * x * x * x;
	};

	// Token: 0x04000C5D RID: 3165
	public static readonly Easing.EasingFn ExpoIn = delegate(float x)
	{
		if (x != 0f)
		{
			return Mathf.Pow(2f, 10f * x - 10f);
		}
		return 0f;
	};

	// Token: 0x04000C5E RID: 3166
	public static readonly Easing.EasingFn ExpoOut = delegate(float x)
	{
		if (x != 1f)
		{
			return 1f - Mathf.Pow(2f, -10f * x);
		}
		return 1f;
	};

	// Token: 0x04000C5F RID: 3167
	public static readonly Easing.EasingFn ExpoInOut = delegate(float x)
	{
		if (x == 0f)
		{
			return 0f;
		}
		if (x == 1f)
		{
			return 1f;
		}
		if ((double)x >= 0.5)
		{
			return (2f - Mathf.Pow(2f, -20f * x + 10f)) / 2f;
		}
		return Mathf.Pow(2f, 20f * x - 10f) / 2f;
	};

	// Token: 0x04000C60 RID: 3168
	public static readonly Easing.EasingFn SineIn = (float x) => 1f - Mathf.Cos(x * 3.1415927f / 2f);

	// Token: 0x04000C61 RID: 3169
	public static readonly Easing.EasingFn SineOut = (float x) => Mathf.Sin(x * 3.1415927f / 2f);

	// Token: 0x04000C62 RID: 3170
	public static readonly Easing.EasingFn SineInOut = (float x) => -(Mathf.Cos(3.1415927f * x) - 1f) / 2f;

	// Token: 0x04000C63 RID: 3171
	public static readonly Easing.EasingFn CircIn = (float x) => 1f - Mathf.Sqrt(1f - Mathf.Pow(x, 2f));

	// Token: 0x04000C64 RID: 3172
	public static readonly Easing.EasingFn CircOut = (float x) => Mathf.Sqrt(1f - Mathf.Pow(x - 1f, 2f));

	// Token: 0x04000C65 RID: 3173
	public static readonly Easing.EasingFn CircInOut = delegate(float x)
	{
		if ((double)x >= 0.5)
		{
			return (Mathf.Sqrt(1f - Mathf.Pow(-2f * x + 2f, 2f)) + 1f) / 2f;
		}
		return (1f - Mathf.Sqrt(1f - Mathf.Pow(2f * x, 2f))) / 2f;
	};

	// Token: 0x04000C66 RID: 3174
	public static readonly Easing.EasingFn EaseOutBack = (float x) => 1f + 2.70158f * Mathf.Pow(x - 1f, 3f) + 1.70158f * Mathf.Pow(x - 1f, 2f);

	// Token: 0x04000C67 RID: 3175
	public static readonly Easing.EasingFn ElasticIn = delegate(float x)
	{
		if (x == 0f)
		{
			return 0f;
		}
		if (x != 1f)
		{
			return -Mathf.Pow(2f, 10f * x - 10f) * Mathf.Sin((x * 10f - 10.75f) * 2.0943952f);
		}
		return 1f;
	};

	// Token: 0x04000C68 RID: 3176
	public static readonly Easing.EasingFn ElasticOut = delegate(float x)
	{
		if (x == 0f)
		{
			return 0f;
		}
		if (x != 1f)
		{
			return Mathf.Pow(2f, -10f * x) * Mathf.Sin((x * 10f - 0.75f) * 2.0943952f) + 1f;
		}
		return 1f;
	};

	// Token: 0x04000C69 RID: 3177
	public static readonly Easing.EasingFn ElasticInOut = delegate(float x)
	{
		if (x == 0f)
		{
			return 0f;
		}
		if (x == 1f)
		{
			return 1f;
		}
		if ((double)x >= 0.5)
		{
			return Mathf.Pow(2f, -20f * x + 10f) * Mathf.Sin((20f * x - 11.125f) * 1.3962635f) / 2f + 1f;
		}
		return -(Mathf.Pow(2f, 20f * x - 10f) * Mathf.Sin((20f * x - 11.125f) * 1.3962635f)) / 2f;
	};

	// Token: 0x04000C6A RID: 3178
	public static readonly Easing.EasingFn BackIn = (float x) => 2.70158f * x * x * x - 1.70158f * x * x;

	// Token: 0x04000C6B RID: 3179
	public static readonly Easing.EasingFn BackOut = (float x) => 1f + 2.70158f * Mathf.Pow(x - 1f, 3f) + 1.70158f * Mathf.Pow(x - 1f, 2f);

	// Token: 0x04000C6C RID: 3180
	public static readonly Easing.EasingFn BackInOut = delegate(float x)
	{
		if ((double)x >= 0.5)
		{
			return (Mathf.Pow(2f * x - 2f, 2f) * (3.5949094f * (x * 2f - 2f) + 2.5949094f) + 2f) / 2f;
		}
		return Mathf.Pow(2f * x, 2f) * (7.189819f * x - 2.5949094f) / 2f;
	};

	// Token: 0x04000C6D RID: 3181
	public static readonly Easing.EasingFn BounceIn = (float x) => 1f - Easing.BounceOut(1f - x);

	// Token: 0x04000C6E RID: 3182
	public static readonly Easing.EasingFn BounceOut = delegate(float x)
	{
		if (x < 0.36363637f)
		{
			return 7.5625f * x * x;
		}
		if (x < 0.72727275f)
		{
			return 7.5625f * (x -= 0.54545456f) * x + 0.75f;
		}
		if ((double)x < 0.9090909090909091)
		{
			return 7.5625f * (x -= 0.8181818f) * x + 0.9375f;
		}
		return 7.5625f * (x -= 0.95454544f) * x + 0.984375f;
	};

	// Token: 0x04000C6F RID: 3183
	public static readonly Easing.EasingFn BounceInOut = delegate(float x)
	{
		if ((double)x >= 0.5)
		{
			return (1f + Easing.BounceOut(2f * x - 1f)) / 2f;
		}
		return (1f - Easing.BounceOut(1f - 2f * x)) / 2f;
	};

	// Token: 0x02001168 RID: 4456
	// (Invoke) Token: 0x06007F87 RID: 32647
	public delegate float EasingFn(float f);
}
