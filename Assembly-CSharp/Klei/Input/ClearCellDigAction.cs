using System;
using Klei.Actions;

namespace Klei.Input
{
	// Token: 0x02000F89 RID: 3977
	[Action("Clear Cell")]
	public class ClearCellDigAction : DigAction
	{
		// Token: 0x060079DA RID: 31194 RVA: 0x003015F4 File Offset: 0x002FF7F4
		public override void Dig(int cell, int distFromOrigin)
		{
			if (Grid.Solid[cell] && !Grid.Foundation[cell])
			{
				SimMessages.Dig(cell, -1, true);
			}
		}

		// Token: 0x060079DB RID: 31195 RVA: 0x00301618 File Offset: 0x002FF818
		protected override void EntityDig(IDigActionEntity digActionEntity)
		{
			if (digActionEntity == null)
			{
				return;
			}
			digActionEntity.Dig();
		}
	}
}
