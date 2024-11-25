using System;

// Token: 0x02000CFA RID: 3322
public class NotificationHighlightTarget : KMonoBehaviour
{
	// Token: 0x0600670F RID: 26383 RVA: 0x00267D35 File Offset: 0x00265F35
	protected void OnEnable()
	{
		this.controller = base.GetComponentInParent<NotificationHighlightController>();
		if (this.controller != null)
		{
			this.controller.AddTarget(this);
		}
	}

	// Token: 0x06006710 RID: 26384 RVA: 0x00267D5D File Offset: 0x00265F5D
	protected override void OnDisable()
	{
		if (this.controller != null)
		{
			this.controller.RemoveTarget(this);
		}
	}

	// Token: 0x06006711 RID: 26385 RVA: 0x00267D79 File Offset: 0x00265F79
	public void View()
	{
		base.GetComponentInParent<NotificationHighlightController>().TargetViewed(this);
	}

	// Token: 0x0400458A RID: 17802
	public string targetKey;

	// Token: 0x0400458B RID: 17803
	private NotificationHighlightController controller;
}
