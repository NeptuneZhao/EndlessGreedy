using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F74 RID: 3956
	public class EmoteStep
	{
		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06007961 RID: 31073 RVA: 0x002FF67F File Offset: 0x002FD87F
		public int Id
		{
			get
			{
				return this.anim.HashValue;
			}
		}

		// Token: 0x06007962 RID: 31074 RVA: 0x002FF68C File Offset: 0x002FD88C
		public HandleVector<EmoteStep.Callbacks>.Handle RegisterCallbacks(Action<GameObject> startedCb, Action<GameObject> finishedCb)
		{
			if (startedCb == null && finishedCb == null)
			{
				return HandleVector<EmoteStep.Callbacks>.InvalidHandle;
			}
			EmoteStep.Callbacks item = new EmoteStep.Callbacks
			{
				StartedCb = startedCb,
				FinishedCb = finishedCb
			};
			return this.callbacks.Add(item);
		}

		// Token: 0x06007963 RID: 31075 RVA: 0x002FF6CB File Offset: 0x002FD8CB
		public void UnregisterCallbacks(HandleVector<EmoteStep.Callbacks>.Handle callbackHandle)
		{
			this.callbacks.Release(callbackHandle);
		}

		// Token: 0x06007964 RID: 31076 RVA: 0x002FF6DA File Offset: 0x002FD8DA
		public void UnregisterAllCallbacks()
		{
			this.callbacks = new HandleVector<EmoteStep.Callbacks>(64);
		}

		// Token: 0x06007965 RID: 31077 RVA: 0x002FF6EC File Offset: 0x002FD8EC
		public void OnStepStarted(HandleVector<EmoteStep.Callbacks>.Handle callbackHandle, GameObject parameter)
		{
			if (callbackHandle == HandleVector<EmoteStep.Callbacks>.Handle.InvalidHandle)
			{
				return;
			}
			EmoteStep.Callbacks item = this.callbacks.GetItem(callbackHandle);
			if (item.StartedCb != null)
			{
				item.StartedCb(parameter);
			}
		}

		// Token: 0x06007966 RID: 31078 RVA: 0x002FF728 File Offset: 0x002FD928
		public void OnStepFinished(HandleVector<EmoteStep.Callbacks>.Handle callbackHandle, GameObject parameter)
		{
			if (callbackHandle == HandleVector<EmoteStep.Callbacks>.Handle.InvalidHandle)
			{
				return;
			}
			EmoteStep.Callbacks item = this.callbacks.GetItem(callbackHandle);
			if (item.FinishedCb != null)
			{
				item.FinishedCb(parameter);
			}
		}

		// Token: 0x04005A9C RID: 23196
		public HashedString anim = HashedString.Invalid;

		// Token: 0x04005A9D RID: 23197
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;

		// Token: 0x04005A9E RID: 23198
		public float timeout = -1f;

		// Token: 0x04005A9F RID: 23199
		private HandleVector<EmoteStep.Callbacks> callbacks = new HandleVector<EmoteStep.Callbacks>(64);

		// Token: 0x02002361 RID: 9057
		public struct Callbacks
		{
			// Token: 0x04009E8C RID: 40588
			public Action<GameObject> StartedCb;

			// Token: 0x04009E8D RID: 40589
			public Action<GameObject> FinishedCb;
		}
	}
}
