using System;
using UnityEngine;

// Token: 0x0200094B RID: 2379
internal class LogicEventSender : ILogicEventSender, ILogicNetworkConnection, ILogicUIElement, IUniformGridObject
{
	// Token: 0x0600454D RID: 17741 RVA: 0x0018AF32 File Offset: 0x00189132
	public LogicEventSender(HashedString id, int cell, Action<int, int> on_value_changed, Action<int, bool> on_connection_changed, LogicPortSpriteType sprite_type)
	{
		this.id = id;
		this.cell = cell;
		this.onValueChanged = on_value_changed;
		this.onConnectionChanged = on_connection_changed;
		this.spriteType = sprite_type;
	}

	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x0600454E RID: 17742 RVA: 0x0018AF67 File Offset: 0x00189167
	public HashedString ID
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x0600454F RID: 17743 RVA: 0x0018AF6F File Offset: 0x0018916F
	public int GetLogicCell()
	{
		return this.cell;
	}

	// Token: 0x06004550 RID: 17744 RVA: 0x0018AF77 File Offset: 0x00189177
	public int GetLogicValue()
	{
		return this.logicValue;
	}

	// Token: 0x06004551 RID: 17745 RVA: 0x0018AF7F File Offset: 0x0018917F
	public int GetLogicUICell()
	{
		return this.GetLogicCell();
	}

	// Token: 0x06004552 RID: 17746 RVA: 0x0018AF87 File Offset: 0x00189187
	public LogicPortSpriteType GetLogicPortSpriteType()
	{
		return this.spriteType;
	}

	// Token: 0x06004553 RID: 17747 RVA: 0x0018AF8F File Offset: 0x0018918F
	public Vector2 PosMin()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06004554 RID: 17748 RVA: 0x0018AFA1 File Offset: 0x001891A1
	public Vector2 PosMax()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06004555 RID: 17749 RVA: 0x0018AFB4 File Offset: 0x001891B4
	public void SetValue(int value)
	{
		int arg = this.logicValue;
		this.logicValue = value;
		this.onValueChanged(value, arg);
	}

	// Token: 0x06004556 RID: 17750 RVA: 0x0018AFDC File Offset: 0x001891DC
	public void LogicTick()
	{
	}

	// Token: 0x06004557 RID: 17751 RVA: 0x0018AFDE File Offset: 0x001891DE
	public void OnLogicNetworkConnectionChanged(bool connected)
	{
		if (this.onConnectionChanged != null)
		{
			this.onConnectionChanged(this.cell, connected);
		}
	}

	// Token: 0x04002D30 RID: 11568
	private HashedString id;

	// Token: 0x04002D31 RID: 11569
	private int cell;

	// Token: 0x04002D32 RID: 11570
	private int logicValue = -16;

	// Token: 0x04002D33 RID: 11571
	private Action<int, int> onValueChanged;

	// Token: 0x04002D34 RID: 11572
	private Action<int, bool> onConnectionChanged;

	// Token: 0x04002D35 RID: 11573
	private LogicPortSpriteType spriteType;
}
