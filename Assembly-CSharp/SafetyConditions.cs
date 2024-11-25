using System;
using System.Collections.Generic;

// Token: 0x0200049E RID: 1182
public class SafetyConditions
{
	// Token: 0x06001981 RID: 6529 RVA: 0x00088494 File Offset: 0x00086694
	public SafetyConditions()
	{
		int num = 1;
		this.IsNearby = new SafetyChecker.Condition("IsNearby", num *= 2, (int cell, int cost, SafetyChecker.Context context) => cost > 5);
		this.IsNotLedge = new SafetyChecker.Condition("IsNotLedge", num *= 2, delegate(int cell, int cost, SafetyChecker.Context context)
		{
			int i = Grid.CellBelow(Grid.CellLeft(cell));
			if (Grid.Solid[i])
			{
				return false;
			}
			int i2 = Grid.CellBelow(Grid.CellRight(cell));
			return Grid.Solid[i2];
		});
		this.IsNotLiquid = new SafetyChecker.Condition("IsNotLiquid", num *= 2, (int cell, int cost, SafetyChecker.Context context) => !Grid.Element[cell].IsLiquid);
		this.IsNotLadder = new SafetyChecker.Condition("IsNotLadder", num *= 2, (int cell, int cost, SafetyChecker.Context context) => !context.navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) && !context.navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole));
		this.IsNotDoor = new SafetyChecker.Condition("IsNotDoor", num *= 2, delegate(int cell, int cost, SafetyChecker.Context context)
		{
			int num2 = Grid.CellAbove(cell);
			return !Grid.HasDoor[cell] && Grid.IsValidCell(num2) && !Grid.HasDoor[num2];
		});
		this.IsCorrectTemperature = new SafetyChecker.Condition("IsCorrectTemperature", num *= 2, (int cell, int cost, SafetyChecker.Context context) => Grid.Temperature[cell] > 285.15f && Grid.Temperature[cell] < 303.15f);
		this.IsWarming = new SafetyChecker.Condition("IsWarming", num *= 2, (int cell, int cost, SafetyChecker.Context context) => WarmthProvider.IsWarmCell(cell));
		this.IsCooling = new SafetyChecker.Condition("IsCooling", num *= 2, (int cell, int cost, SafetyChecker.Context context) => false);
		this.HasSomeOxygen = new SafetyChecker.Condition("HasSomeOxygen", num *= 2, (int cell, int cost, SafetyChecker.Context context) => context.oxygenBreather == null || context.oxygenBreather.IsBreathableElementAtCell(cell, null));
		this.IsClear = new SafetyChecker.Condition("IsClear", num * 2, (int cell, int cost, SafetyChecker.Context context) => context.minionBrain.IsCellClear(cell));
		this.WarmUpChecker = new SafetyChecker(new List<SafetyChecker.Condition>
		{
			this.IsWarming
		}.ToArray());
		this.CoolDownChecker = new SafetyChecker(new List<SafetyChecker.Condition>
		{
			this.IsCooling
		}.ToArray());
		List<SafetyChecker.Condition> list = new List<SafetyChecker.Condition>();
		list.Add(this.HasSomeOxygen);
		list.Add(this.IsNotDoor);
		this.RecoverBreathChecker = new SafetyChecker(list.ToArray());
		List<SafetyChecker.Condition> list2 = new List<SafetyChecker.Condition>(list);
		list2.Add(this.IsNotLiquid);
		list2.Add(this.IsCorrectTemperature);
		this.SafeCellChecker = new SafetyChecker(list2.ToArray());
		this.IdleCellChecker = new SafetyChecker(new List<SafetyChecker.Condition>(list2)
		{
			this.IsClear,
			this.IsNotLadder
		}.ToArray());
		this.VomitCellChecker = new SafetyChecker(new List<SafetyChecker.Condition>
		{
			this.IsNotLiquid,
			this.IsNotLedge,
			this.IsNearby
		}.ToArray());
	}

	// Token: 0x04000E6C RID: 3692
	public SafetyChecker.Condition IsNotLiquid;

	// Token: 0x04000E6D RID: 3693
	public SafetyChecker.Condition IsNotLadder;

	// Token: 0x04000E6E RID: 3694
	public SafetyChecker.Condition IsCorrectTemperature;

	// Token: 0x04000E6F RID: 3695
	public SafetyChecker.Condition IsWarming;

	// Token: 0x04000E70 RID: 3696
	public SafetyChecker.Condition IsCooling;

	// Token: 0x04000E71 RID: 3697
	public SafetyChecker.Condition HasSomeOxygen;

	// Token: 0x04000E72 RID: 3698
	public SafetyChecker.Condition IsClear;

	// Token: 0x04000E73 RID: 3699
	public SafetyChecker.Condition IsNotFoundation;

	// Token: 0x04000E74 RID: 3700
	public SafetyChecker.Condition IsNotDoor;

	// Token: 0x04000E75 RID: 3701
	public SafetyChecker.Condition IsNotLedge;

	// Token: 0x04000E76 RID: 3702
	public SafetyChecker.Condition IsNearby;

	// Token: 0x04000E77 RID: 3703
	public SafetyChecker WarmUpChecker;

	// Token: 0x04000E78 RID: 3704
	public SafetyChecker CoolDownChecker;

	// Token: 0x04000E79 RID: 3705
	public SafetyChecker RecoverBreathChecker;

	// Token: 0x04000E7A RID: 3706
	public SafetyChecker VomitCellChecker;

	// Token: 0x04000E7B RID: 3707
	public SafetyChecker SafeCellChecker;

	// Token: 0x04000E7C RID: 3708
	public SafetyChecker IdleCellChecker;
}
