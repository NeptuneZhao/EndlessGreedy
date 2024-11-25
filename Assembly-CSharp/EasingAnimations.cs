using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BDE RID: 3038
public class EasingAnimations : MonoBehaviour
{
	// Token: 0x170006CC RID: 1740
	// (get) Token: 0x06005C6E RID: 23662 RVA: 0x0021D0FB File Offset: 0x0021B2FB
	public bool IsPlaying
	{
		get
		{
			return this.animationCoroutine != null;
		}
	}

	// Token: 0x06005C6F RID: 23663 RVA: 0x0021D106 File Offset: 0x0021B306
	private void Start()
	{
		if (this.animationMap == null || this.animationMap.Count == 0)
		{
			this.Initialize();
		}
	}

	// Token: 0x06005C70 RID: 23664 RVA: 0x0021D124 File Offset: 0x0021B324
	private void Initialize()
	{
		this.animationMap = new Dictionary<string, EasingAnimations.AnimationScales>();
		foreach (EasingAnimations.AnimationScales animationScales in this.scales)
		{
			this.animationMap.Add(animationScales.name, animationScales);
		}
	}

	// Token: 0x06005C71 RID: 23665 RVA: 0x0021D16C File Offset: 0x0021B36C
	public void PlayAnimation(string animationName, float delay = 0f)
	{
		if (this.animationMap == null || this.animationMap.Count == 0)
		{
			this.Initialize();
		}
		if (!this.animationMap.ContainsKey(animationName))
		{
			return;
		}
		if (this.animationCoroutine != null)
		{
			base.StopCoroutine(this.animationCoroutine);
		}
		this.currentAnimation = this.animationMap[animationName];
		this.currentAnimation.currentScale = this.currentAnimation.startScale;
		base.transform.localScale = Vector3.one * this.currentAnimation.currentScale;
		this.animationCoroutine = base.StartCoroutine(this.ExecuteAnimation(delay));
	}

	// Token: 0x06005C72 RID: 23666 RVA: 0x0021D212 File Offset: 0x0021B412
	private IEnumerator ExecuteAnimation(float delay)
	{
		float startTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < startTime + delay)
		{
			yield return SequenceUtil.WaitForNextFrame;
		}
		startTime = Time.realtimeSinceStartup;
		bool keepAnimating = true;
		while (keepAnimating)
		{
			float num = Time.realtimeSinceStartup - startTime;
			this.currentAnimation.currentScale = this.GetEasing(num * this.currentAnimation.easingMultiplier);
			if (this.currentAnimation.endScale > this.currentAnimation.startScale)
			{
				keepAnimating = (this.currentAnimation.currentScale < this.currentAnimation.endScale - 0.025f);
			}
			else
			{
				keepAnimating = (this.currentAnimation.currentScale > this.currentAnimation.endScale + 0.025f);
			}
			if (!keepAnimating)
			{
				this.currentAnimation.currentScale = this.currentAnimation.endScale;
			}
			base.transform.localScale = Vector3.one * this.currentAnimation.currentScale;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.animationCoroutine = null;
		if (this.OnAnimationDone != null)
		{
			this.OnAnimationDone(this.currentAnimation.name);
		}
		yield break;
	}

	// Token: 0x06005C73 RID: 23667 RVA: 0x0021D228 File Offset: 0x0021B428
	private float GetEasing(float t)
	{
		EasingAnimations.AnimationScales.AnimationType type = this.currentAnimation.type;
		if (type == EasingAnimations.AnimationScales.AnimationType.EaseOutBack)
		{
			return this.EaseOutBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
		}
		if (type == EasingAnimations.AnimationScales.AnimationType.EaseInBack)
		{
			return this.EaseInBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
		}
		return this.EaseInOutBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
	}

	// Token: 0x06005C74 RID: 23668 RVA: 0x0021D2A4 File Offset: 0x0021B4A4
	public float EaseInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end * 0.5f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end * 0.5f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	// Token: 0x06005C75 RID: 23669 RVA: 0x0021D320 File Offset: 0x0021B520
	public float EaseInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	// Token: 0x06005C76 RID: 23670 RVA: 0x0021D354 File Offset: 0x0021B554
	public float EaseOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value -= 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	// Token: 0x04003DA5 RID: 15781
	public EasingAnimations.AnimationScales[] scales;

	// Token: 0x04003DA6 RID: 15782
	private EasingAnimations.AnimationScales currentAnimation;

	// Token: 0x04003DA7 RID: 15783
	private Coroutine animationCoroutine;

	// Token: 0x04003DA8 RID: 15784
	private Dictionary<string, EasingAnimations.AnimationScales> animationMap;

	// Token: 0x04003DA9 RID: 15785
	public Action<string> OnAnimationDone;

	// Token: 0x02001CC3 RID: 7363
	[Serializable]
	public struct AnimationScales
	{
		// Token: 0x040084F7 RID: 34039
		public string name;

		// Token: 0x040084F8 RID: 34040
		public float startScale;

		// Token: 0x040084F9 RID: 34041
		public float endScale;

		// Token: 0x040084FA RID: 34042
		public EasingAnimations.AnimationScales.AnimationType type;

		// Token: 0x040084FB RID: 34043
		public float easingMultiplier;

		// Token: 0x040084FC RID: 34044
		[HideInInspector]
		public float currentScale;

		// Token: 0x0200264C RID: 9804
		public enum AnimationType
		{
			// Token: 0x0400AA47 RID: 43591
			EaseInOutBack,
			// Token: 0x0400AA48 RID: 43592
			EaseOutBack,
			// Token: 0x0400AA49 RID: 43593
			EaseInBack
		}
	}
}
