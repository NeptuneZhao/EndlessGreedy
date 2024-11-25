using System;
using Klei.Input;

namespace Klei.Actions
{
	// Token: 0x02000F8D RID: 3981
	public class DigToolActionFactory : ActionFactory<DigToolActionFactory, DigAction, DigToolActionFactory.Actions>
	{
		// Token: 0x060079E8 RID: 31208 RVA: 0x0030175F File Offset: 0x002FF95F
		protected override DigAction CreateAction(DigToolActionFactory.Actions action)
		{
			if (action == DigToolActionFactory.Actions.Immediate)
			{
				return new ImmediateDigAction();
			}
			if (action == DigToolActionFactory.Actions.ClearCell)
			{
				return new ClearCellDigAction();
			}
			if (action == DigToolActionFactory.Actions.MarkCell)
			{
				return new MarkCellDigAction();
			}
			throw new InvalidOperationException("Can not create DigAction 'Count'. Please provide a valid action.");
		}

		// Token: 0x02002372 RID: 9074
		public enum Actions
		{
			// Token: 0x04009EDB RID: 40667
			MarkCell = 145163119,
			// Token: 0x04009EDC RID: 40668
			Immediate = -1044758767,
			// Token: 0x04009EDD RID: 40669
			ClearCell = -1011242513,
			// Token: 0x04009EDE RID: 40670
			Count = -1427607121
		}
	}
}
