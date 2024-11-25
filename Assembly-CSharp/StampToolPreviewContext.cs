using System;
using UnityEngine;

// Token: 0x02000927 RID: 2343
public class StampToolPreviewContext
{
	// Token: 0x04002C9B RID: 11419
	public Transform previewParent;

	// Token: 0x04002C9C RID: 11420
	public InterfaceTool tool;

	// Token: 0x04002C9D RID: 11421
	public TemplateContainer stampTemplate;

	// Token: 0x04002C9E RID: 11422
	public System.Action frameAfterSetupFn;

	// Token: 0x04002C9F RID: 11423
	public Action<int> refreshFn;

	// Token: 0x04002CA0 RID: 11424
	public System.Action onPlaceFn;

	// Token: 0x04002CA1 RID: 11425
	public Action<string> onErrorChangeFn;

	// Token: 0x04002CA2 RID: 11426
	public System.Action cleanupFn;
}
