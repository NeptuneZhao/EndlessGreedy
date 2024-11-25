using System;

namespace Klei
{
	// Token: 0x02000F27 RID: 3879
	public struct CallbackInfo
	{
		// Token: 0x06007779 RID: 30585 RVA: 0x002F5CD5 File Offset: 0x002F3ED5
		public CallbackInfo(HandleVector<Game.CallbackInfo>.Handle h)
		{
			this.handle = h;
		}

		// Token: 0x0600777A RID: 30586 RVA: 0x002F5CE0 File Offset: 0x002F3EE0
		public void Release()
		{
			if (this.handle.IsValid())
			{
				Game.CallbackInfo item = Game.Instance.callbackManager.GetItem(this.handle);
				System.Action cb = item.cb;
				if (!item.manuallyRelease)
				{
					Game.Instance.callbackManager.Release(this.handle);
				}
				cb();
			}
		}

		// Token: 0x04005941 RID: 22849
		private HandleVector<Game.CallbackInfo>.Handle handle;
	}
}
