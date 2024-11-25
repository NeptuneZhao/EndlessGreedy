using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000928 RID: 2344
public class StampToolPreview
{
	// Token: 0x06004425 RID: 17445 RVA: 0x00182C47 File Offset: 0x00180E47
	public StampToolPreview(InterfaceTool tool, params IStampToolPreviewPlugin[] plugins)
	{
		this.context = new StampToolPreviewContext();
		this.context.previewParent = new GameObject("StampToolPreview::Preview").transform;
		this.context.tool = tool;
		this.plugins = plugins;
	}

	// Token: 0x06004426 RID: 17446 RVA: 0x00182C87 File Offset: 0x00180E87
	public IEnumerator Setup(TemplateContainer stampTemplate)
	{
		this.Cleanup();
		this.context.stampTemplate = stampTemplate;
		if (this.plugins != null)
		{
			IStampToolPreviewPlugin[] array = this.plugins;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Setup(this.context);
			}
		}
		yield return null;
		if (this.context.frameAfterSetupFn != null)
		{
			this.context.frameAfterSetupFn();
		}
		yield break;
	}

	// Token: 0x06004427 RID: 17447 RVA: 0x00182CA0 File Offset: 0x00180EA0
	public void Refresh(int originCell)
	{
		if (this.context.stampTemplate == null)
		{
			return;
		}
		if (originCell == this.prevOriginCell)
		{
			return;
		}
		this.prevOriginCell = originCell;
		if (!Grid.IsValidCell(originCell))
		{
			return;
		}
		if (this.context.refreshFn != null)
		{
			this.context.refreshFn(originCell);
		}
		this.context.previewParent.transform.SetPosition(Grid.CellToPosCBC(originCell, this.context.tool.visualizerLayer));
		this.context.previewParent.gameObject.SetActive(true);
	}

	// Token: 0x06004428 RID: 17448 RVA: 0x00182D35 File Offset: 0x00180F35
	public void OnErrorChange(string error)
	{
		if (this.context.onErrorChangeFn != null)
		{
			this.context.onErrorChangeFn(error);
		}
	}

	// Token: 0x06004429 RID: 17449 RVA: 0x00182D55 File Offset: 0x00180F55
	public void OnPlace()
	{
		if (this.context.onPlaceFn != null)
		{
			this.context.onPlaceFn();
		}
	}

	// Token: 0x0600442A RID: 17450 RVA: 0x00182D74 File Offset: 0x00180F74
	public void Cleanup()
	{
		if (this.context.cleanupFn != null)
		{
			this.context.cleanupFn();
		}
		this.prevOriginCell = Grid.InvalidCell;
		this.context.stampTemplate = null;
		this.context.frameAfterSetupFn = null;
		this.context.refreshFn = null;
		this.context.onPlaceFn = null;
		this.context.cleanupFn = null;
	}

	// Token: 0x04002CA3 RID: 11427
	private IStampToolPreviewPlugin[] plugins;

	// Token: 0x04002CA4 RID: 11428
	private StampToolPreviewContext context;

	// Token: 0x04002CA5 RID: 11429
	private int prevOriginCell;
}
