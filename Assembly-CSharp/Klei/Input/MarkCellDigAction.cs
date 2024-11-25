using System;
using Klei.Actions;
using UnityEngine;

namespace Klei.Input
{
	// Token: 0x02000F87 RID: 3975
	[Action("Mark Cell")]
	public class MarkCellDigAction : DigAction
	{
		// Token: 0x060079D4 RID: 31188 RVA: 0x00301564 File Offset: 0x002FF764
		public override void Dig(int cell, int distFromOrigin)
		{
			GameObject gameObject = DigTool.PlaceDig(cell, distFromOrigin);
			if (gameObject != null)
			{
				Prioritizable component = gameObject.GetComponent<Prioritizable>();
				if (component != null)
				{
					component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
				}
			}
		}

		// Token: 0x060079D5 RID: 31189 RVA: 0x003015A7 File Offset: 0x002FF7A7
		protected override void EntityDig(IDigActionEntity digActionEntity)
		{
			if (digActionEntity == null)
			{
				return;
			}
			digActionEntity.MarkForDig(true);
		}
	}
}
