using System;
using UnityEngine.UI;

// Token: 0x02000CF2 RID: 3314
public class NonDrawingGraphic : Graphic
{
	// Token: 0x060066C6 RID: 26310 RVA: 0x0026650D File Offset: 0x0026470D
	public override void SetMaterialDirty()
	{
	}

	// Token: 0x060066C7 RID: 26311 RVA: 0x0026650F File Offset: 0x0026470F
	public override void SetVerticesDirty()
	{
	}

	// Token: 0x060066C8 RID: 26312 RVA: 0x00266511 File Offset: 0x00264711
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
	}
}
