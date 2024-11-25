using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000BDF RID: 3039
public class ExpandRevealUIContent : MonoBehaviour
{
	// Token: 0x06005C78 RID: 23672 RVA: 0x0021D398 File Offset: 0x0021B598
	private void OnDisable()
	{
		if (this.BGChildFitter)
		{
			this.BGChildFitter.WidthScale = (this.BGChildFitter.HeightScale = 0f);
		}
		if (this.MaskChildFitter)
		{
			if (this.MaskChildFitter.fitWidth)
			{
				this.MaskChildFitter.WidthScale = 0f;
			}
			if (this.MaskChildFitter.fitHeight)
			{
				this.MaskChildFitter.HeightScale = 0f;
			}
		}
		if (this.BGRectStretcher)
		{
			this.BGRectStretcher.XStretchFactor = (this.BGRectStretcher.YStretchFactor = 0f);
			this.BGRectStretcher.UpdateStretching();
		}
		if (this.MaskRectStretcher)
		{
			this.MaskRectStretcher.XStretchFactor = (this.MaskRectStretcher.YStretchFactor = 0f);
			this.MaskRectStretcher.UpdateStretching();
		}
	}

	// Token: 0x06005C79 RID: 23673 RVA: 0x0021D484 File Offset: 0x0021B684
	public void Expand(Action<object> completeCallback)
	{
		if (this.MaskChildFitter && this.MaskRectStretcher)
		{
			global::Debug.LogWarning("ExpandRevealUIContent has references to both a MaskChildFitter and a MaskRectStretcher. It should have only one or the other. ChildFitter to match child size, RectStretcher to match parent size.");
		}
		if (this.BGChildFitter && this.BGRectStretcher)
		{
			global::Debug.LogWarning("ExpandRevealUIContent has references to both a BGChildFitter and a BGRectStretcher . It should have only one or the other.  ChildFitter to match child size, RectStretcher to match parent size.");
		}
		if (this.activeRoutine != null)
		{
			base.StopCoroutine(this.activeRoutine);
		}
		this.CollapsedImmediate();
		this.activeRoutineCompleteCallback = completeCallback;
		this.activeRoutine = base.StartCoroutine(this.ExpandRoutine(null));
	}

	// Token: 0x06005C7A RID: 23674 RVA: 0x0021D510 File Offset: 0x0021B710
	public void Collapse(Action<object> completeCallback)
	{
		if (this.activeRoutine != null)
		{
			if (this.activeRoutineCompleteCallback != null)
			{
				this.activeRoutineCompleteCallback(null);
			}
			base.StopCoroutine(this.activeRoutine);
		}
		this.activeRoutineCompleteCallback = completeCallback;
		if (base.gameObject.activeInHierarchy)
		{
			this.activeRoutine = base.StartCoroutine(this.CollapseRoutine(completeCallback));
			return;
		}
		this.activeRoutine = null;
		if (completeCallback != null)
		{
			completeCallback(null);
		}
	}

	// Token: 0x06005C7B RID: 23675 RVA: 0x0021D57E File Offset: 0x0021B77E
	private IEnumerator ExpandRoutine(Action<object> completeCallback)
	{
		this.Collapsing = false;
		this.Expanding = true;
		float num = 0f;
		foreach (Keyframe keyframe in this.expandAnimation.keys)
		{
			if (keyframe.time > num)
			{
				num = keyframe.time;
			}
		}
		float duration = num / this.speedScale;
		for (float remaining = duration; remaining >= 0f; remaining -= Time.unscaledDeltaTime * this.speedScale)
		{
			this.SetStretch(this.expandAnimation.Evaluate(duration - remaining));
			yield return null;
		}
		this.SetStretch(this.expandAnimation.Evaluate(duration));
		if (completeCallback != null)
		{
			completeCallback(null);
		}
		this.activeRoutine = null;
		this.Expanding = false;
		yield break;
	}

	// Token: 0x06005C7C RID: 23676 RVA: 0x0021D594 File Offset: 0x0021B794
	private void SetStretch(float value)
	{
		if (this.BGRectStretcher)
		{
			if (this.BGRectStretcher.StretchX)
			{
				this.BGRectStretcher.XStretchFactor = value;
			}
			if (this.BGRectStretcher.StretchY)
			{
				this.BGRectStretcher.YStretchFactor = value;
			}
		}
		if (this.MaskRectStretcher)
		{
			if (this.MaskRectStretcher.StretchX)
			{
				this.MaskRectStretcher.XStretchFactor = value;
			}
			if (this.MaskRectStretcher.StretchY)
			{
				this.MaskRectStretcher.YStretchFactor = value;
			}
		}
		if (this.BGChildFitter)
		{
			if (this.BGChildFitter.fitWidth)
			{
				this.BGChildFitter.WidthScale = value;
			}
			if (this.BGChildFitter.fitHeight)
			{
				this.BGChildFitter.HeightScale = value;
			}
		}
		if (this.MaskChildFitter)
		{
			if (this.MaskChildFitter.fitWidth)
			{
				this.MaskChildFitter.WidthScale = value;
			}
			if (this.MaskChildFitter.fitHeight)
			{
				this.MaskChildFitter.HeightScale = value;
			}
		}
	}

	// Token: 0x06005C7D RID: 23677 RVA: 0x0021D69D File Offset: 0x0021B89D
	private IEnumerator CollapseRoutine(Action<object> completeCallback)
	{
		this.Expanding = false;
		this.Collapsing = true;
		float num = 0f;
		foreach (Keyframe keyframe in this.collapseAnimation.keys)
		{
			if (keyframe.time > num)
			{
				num = keyframe.time;
			}
		}
		float duration = num;
		for (float remaining = duration; remaining >= 0f; remaining -= Time.unscaledDeltaTime)
		{
			this.SetStretch(this.collapseAnimation.Evaluate(duration - remaining));
			yield return null;
		}
		this.SetStretch(this.collapseAnimation.Evaluate(duration));
		if (completeCallback != null)
		{
			completeCallback(null);
		}
		this.activeRoutine = null;
		this.Collapsing = false;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06005C7E RID: 23678 RVA: 0x0021D6B4 File Offset: 0x0021B8B4
	public void CollapsedImmediate()
	{
		float time = (float)this.collapseAnimation.length;
		this.SetStretch(this.collapseAnimation.Evaluate(time));
	}

	// Token: 0x04003DAA RID: 15786
	private Coroutine activeRoutine;

	// Token: 0x04003DAB RID: 15787
	private Action<object> activeRoutineCompleteCallback;

	// Token: 0x04003DAC RID: 15788
	public AnimationCurve expandAnimation;

	// Token: 0x04003DAD RID: 15789
	public AnimationCurve collapseAnimation;

	// Token: 0x04003DAE RID: 15790
	public KRectStretcher MaskRectStretcher;

	// Token: 0x04003DAF RID: 15791
	public KRectStretcher BGRectStretcher;

	// Token: 0x04003DB0 RID: 15792
	public KChildFitter MaskChildFitter;

	// Token: 0x04003DB1 RID: 15793
	public KChildFitter BGChildFitter;

	// Token: 0x04003DB2 RID: 15794
	public float speedScale = 1f;

	// Token: 0x04003DB3 RID: 15795
	public bool Collapsing;

	// Token: 0x04003DB4 RID: 15796
	public bool Expanding;
}
