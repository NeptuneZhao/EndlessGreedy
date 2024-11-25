using System;

namespace EventSystem2Syntax
{
	// Token: 0x02000E22 RID: 3618
	internal class OldExample : KMonoBehaviour2
	{
		// Token: 0x0600735C RID: 29532 RVA: 0x002C40D8 File Offset: 0x002C22D8
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			base.Subscribe(0, new Action<object>(this.OnObjectDestroyed));
			bool flag = false;
			base.Trigger(0, flag);
		}

		// Token: 0x0600735D RID: 29533 RVA: 0x002C410D File Offset: 0x002C230D
		private void OnObjectDestroyed(object data)
		{
			Debug.Log((bool)data);
		}
	}
}
