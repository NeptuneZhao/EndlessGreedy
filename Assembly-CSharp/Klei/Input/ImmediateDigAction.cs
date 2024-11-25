using System;
using Klei.Actions;

namespace Klei.Input
{
	// Token: 0x02000F88 RID: 3976
	[Action("Immediate")]
	public class ImmediateDigAction : DigAction
	{
		// Token: 0x060079D7 RID: 31191 RVA: 0x003015BC File Offset: 0x002FF7BC
		public override void Dig(int cell, int distFromOrigin)
		{
			if (Grid.Solid[cell] && !Grid.Foundation[cell])
			{
				SimMessages.Dig(cell, -1, false);
			}
		}

		// Token: 0x060079D8 RID: 31192 RVA: 0x003015E0 File Offset: 0x002FF7E0
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
