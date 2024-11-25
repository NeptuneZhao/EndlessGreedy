using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000409 RID: 1033
public static class SequenceUtil
{
	// Token: 0x17000051 RID: 81
	// (get) Token: 0x060015C1 RID: 5569 RVA: 0x00077048 File Offset: 0x00075248
	public static YieldInstruction WaitForNextFrame
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x060015C2 RID: 5570 RVA: 0x0007704B File Offset: 0x0007524B
	public static YieldInstruction WaitForEndOfFrame
	{
		get
		{
			if (SequenceUtil.waitForEndOfFrame == null)
			{
				SequenceUtil.waitForEndOfFrame = new WaitForEndOfFrame();
			}
			return SequenceUtil.waitForEndOfFrame;
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x060015C3 RID: 5571 RVA: 0x00077063 File Offset: 0x00075263
	public static YieldInstruction WaitForFixedUpdate
	{
		get
		{
			if (SequenceUtil.waitForFixedUpdate == null)
			{
				SequenceUtil.waitForFixedUpdate = new WaitForFixedUpdate();
			}
			return SequenceUtil.waitForFixedUpdate;
		}
	}

	// Token: 0x060015C4 RID: 5572 RVA: 0x0007707C File Offset: 0x0007527C
	public static YieldInstruction WaitForSeconds(float duration)
	{
		WaitForSeconds result;
		if (!SequenceUtil.scaledTimeCache.TryGetValue(duration, out result))
		{
			result = (SequenceUtil.scaledTimeCache[duration] = new WaitForSeconds(duration));
		}
		return result;
	}

	// Token: 0x060015C5 RID: 5573 RVA: 0x000770B0 File Offset: 0x000752B0
	public static WaitForSecondsRealtime WaitForSecondsRealtime(float duration)
	{
		WaitForSecondsRealtime result;
		if (!SequenceUtil.reailTimeWaitCache.TryGetValue(duration, out result))
		{
			result = (SequenceUtil.reailTimeWaitCache[duration] = new WaitForSecondsRealtime(duration));
		}
		return result;
	}

	// Token: 0x04000C4A RID: 3146
	private static WaitForEndOfFrame waitForEndOfFrame = null;

	// Token: 0x04000C4B RID: 3147
	private static WaitForFixedUpdate waitForFixedUpdate = null;

	// Token: 0x04000C4C RID: 3148
	private static Dictionary<float, WaitForSeconds> scaledTimeCache = new Dictionary<float, WaitForSeconds>();

	// Token: 0x04000C4D RID: 3149
	private static Dictionary<float, WaitForSecondsRealtime> reailTimeWaitCache = new Dictionary<float, WaitForSecondsRealtime>();
}
