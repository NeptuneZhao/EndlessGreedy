using System;
using UnityEngine;

// Token: 0x0200094F RID: 2383
public class LogicPortVisualizer : ILogicUIElement, IUniformGridObject
{
	// Token: 0x06004592 RID: 17810 RVA: 0x0018C8A4 File Offset: 0x0018AAA4
	public LogicPortVisualizer(int cell, LogicPortSpriteType sprite_type)
	{
		this.cell = cell;
		this.spriteType = sprite_type;
	}

	// Token: 0x06004593 RID: 17811 RVA: 0x0018C8BA File Offset: 0x0018AABA
	public int GetLogicUICell()
	{
		return this.cell;
	}

	// Token: 0x06004594 RID: 17812 RVA: 0x0018C8C2 File Offset: 0x0018AAC2
	public Vector2 PosMin()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06004595 RID: 17813 RVA: 0x0018C8D4 File Offset: 0x0018AAD4
	public Vector2 PosMax()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06004596 RID: 17814 RVA: 0x0018C8E6 File Offset: 0x0018AAE6
	public LogicPortSpriteType GetLogicPortSpriteType()
	{
		return this.spriteType;
	}

	// Token: 0x04002D54 RID: 11604
	private int cell;

	// Token: 0x04002D55 RID: 11605
	private LogicPortSpriteType spriteType;
}
