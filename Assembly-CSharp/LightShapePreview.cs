using System;
using UnityEngine;

// Token: 0x0200093E RID: 2366
[AddComponentMenu("KMonoBehaviour/scripts/LightShapePreview")]
public class LightShapePreview : KMonoBehaviour
{
	// Token: 0x060044CA RID: 17610 RVA: 0x00187734 File Offset: 0x00185934
	private void Update()
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (num != this.previousCell)
		{
			this.previousCell = num;
			LightGridManager.DestroyPreview();
			LightGridManager.CreatePreview(Grid.OffsetCell(num, this.offset), this.radius, this.shape, this.lux, this.width, this.direction);
		}
	}

	// Token: 0x060044CB RID: 17611 RVA: 0x00187796 File Offset: 0x00185996
	protected override void OnCleanUp()
	{
		LightGridManager.DestroyPreview();
	}

	// Token: 0x04002CFB RID: 11515
	public float radius;

	// Token: 0x04002CFC RID: 11516
	public int lux;

	// Token: 0x04002CFD RID: 11517
	public int width;

	// Token: 0x04002CFE RID: 11518
	public DiscreteShadowCaster.Direction direction;

	// Token: 0x04002CFF RID: 11519
	public global::LightShape shape;

	// Token: 0x04002D00 RID: 11520
	public CellOffset offset;

	// Token: 0x04002D01 RID: 11521
	private int previousCell = -1;
}
