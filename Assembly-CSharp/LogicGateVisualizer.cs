using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200070A RID: 1802
[SkipSaveFileSerialization]
public class LogicGateVisualizer : LogicGateBase
{
	// Token: 0x06002EB1 RID: 11953 RVA: 0x001063C2 File Offset: 0x001045C2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
	}

	// Token: 0x06002EB2 RID: 11954 RVA: 0x001063D0 File Offset: 0x001045D0
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.Unregister();
	}

	// Token: 0x06002EB3 RID: 11955 RVA: 0x001063E0 File Offset: 0x001045E0
	private void Register()
	{
		this.Unregister();
		this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellOne, false));
		if (base.RequiresFourOutputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellTwo, false));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellThree, false));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellFour, false));
		}
		this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellOne, true));
		if (base.RequiresTwoInputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellTwo, true));
		}
		else if (base.RequiresFourInputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellTwo, true));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellThree, true));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellFour, true));
		}
		if (base.RequiresControlInputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.ControlCellOne, true));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.ControlCellTwo, true));
		}
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		foreach (LogicGateVisualizer.IOVisualizer elem in this.visChildren)
		{
			logicCircuitManager.AddVisElem(elem);
		}
	}

	// Token: 0x06002EB4 RID: 11956 RVA: 0x00106564 File Offset: 0x00104764
	private void Unregister()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		foreach (LogicGateVisualizer.IOVisualizer elem in this.visChildren)
		{
			logicCircuitManager.RemoveVisElem(elem);
		}
		this.visChildren.Clear();
	}

	// Token: 0x04001B90 RID: 7056
	private List<LogicGateVisualizer.IOVisualizer> visChildren = new List<LogicGateVisualizer.IOVisualizer>();

	// Token: 0x02001548 RID: 5448
	private class IOVisualizer : ILogicUIElement, IUniformGridObject
	{
		// Token: 0x06008DC8 RID: 36296 RVA: 0x00340A17 File Offset: 0x0033EC17
		public IOVisualizer(int cell, bool input)
		{
			this.cell = cell;
			this.input = input;
		}

		// Token: 0x06008DC9 RID: 36297 RVA: 0x00340A2D File Offset: 0x0033EC2D
		public int GetLogicUICell()
		{
			return this.cell;
		}

		// Token: 0x06008DCA RID: 36298 RVA: 0x00340A35 File Offset: 0x0033EC35
		public LogicPortSpriteType GetLogicPortSpriteType()
		{
			if (!this.input)
			{
				return LogicPortSpriteType.Output;
			}
			return LogicPortSpriteType.Input;
		}

		// Token: 0x06008DCB RID: 36299 RVA: 0x00340A42 File Offset: 0x0033EC42
		public Vector2 PosMin()
		{
			return Grid.CellToPos2D(this.cell);
		}

		// Token: 0x06008DCC RID: 36300 RVA: 0x00340A54 File Offset: 0x0033EC54
		public Vector2 PosMax()
		{
			return this.PosMin();
		}

		// Token: 0x04006C76 RID: 27766
		private int cell;

		// Token: 0x04006C77 RID: 27767
		private bool input;
	}
}
