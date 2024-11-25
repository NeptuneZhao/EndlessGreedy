using System;
using UnityEngine;

// Token: 0x02000C3A RID: 3130
public class SimpleInfoPanel
{
	// Token: 0x0600600A RID: 24586 RVA: 0x0023ADEF File Offset: 0x00238FEF
	public SimpleInfoPanel(SimpleInfoScreen simpleInfoRoot)
	{
		this.simpleInfoRoot = simpleInfoRoot;
	}

	// Token: 0x0600600B RID: 24587 RVA: 0x0023ADFE File Offset: 0x00238FFE
	public virtual void Refresh(CollapsibleDetailContentPanel panel, GameObject selectedTarget)
	{
	}

	// Token: 0x040040D1 RID: 16593
	protected SimpleInfoScreen simpleInfoRoot;
}
