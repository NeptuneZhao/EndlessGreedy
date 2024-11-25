using System;

// Token: 0x02000C25 RID: 3109
public class CustomGameSettingWidget : KMonoBehaviour
{
	// Token: 0x14000022 RID: 34
	// (add) Token: 0x06005F4E RID: 24398 RVA: 0x002366E8 File Offset: 0x002348E8
	// (remove) Token: 0x06005F4F RID: 24399 RVA: 0x00236720 File Offset: 0x00234920
	public event Action<CustomGameSettingWidget> onSettingChanged;

	// Token: 0x14000023 RID: 35
	// (add) Token: 0x06005F50 RID: 24400 RVA: 0x00236758 File Offset: 0x00234958
	// (remove) Token: 0x06005F51 RID: 24401 RVA: 0x00236790 File Offset: 0x00234990
	public event System.Action onRefresh;

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x06005F52 RID: 24402 RVA: 0x002367C8 File Offset: 0x002349C8
	// (remove) Token: 0x06005F53 RID: 24403 RVA: 0x00236800 File Offset: 0x00234A00
	public event System.Action onDestroy;

	// Token: 0x06005F54 RID: 24404 RVA: 0x00236835 File Offset: 0x00234A35
	public virtual void Refresh()
	{
		if (this.onRefresh != null)
		{
			this.onRefresh();
		}
	}

	// Token: 0x06005F55 RID: 24405 RVA: 0x0023684A File Offset: 0x00234A4A
	public void Notify()
	{
		if (this.onSettingChanged != null)
		{
			this.onSettingChanged(this);
		}
	}

	// Token: 0x06005F56 RID: 24406 RVA: 0x00236860 File Offset: 0x00234A60
	protected override void OnForcedCleanUp()
	{
		base.OnForcedCleanUp();
		if (this.onDestroy != null)
		{
			this.onDestroy();
		}
	}
}
