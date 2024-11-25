using System;

namespace Database
{
	// Token: 0x02000E82 RID: 3714
	public class StateMachineCategories : ResourceSet<StateMachine.Category>
	{
		// Token: 0x060074F7 RID: 29943 RVA: 0x002DC19C File Offset: 0x002DA39C
		public StateMachineCategories()
		{
			this.Ai = base.Add(new StateMachine.Category("Ai"));
			this.Monitor = base.Add(new StateMachine.Category("Monitor"));
			this.Chore = base.Add(new StateMachine.Category("Chore"));
			this.Misc = base.Add(new StateMachine.Category("Misc"));
		}

		// Token: 0x040054D0 RID: 21712
		public StateMachine.Category Ai;

		// Token: 0x040054D1 RID: 21713
		public StateMachine.Category Monitor;

		// Token: 0x040054D2 RID: 21714
		public StateMachine.Category Chore;

		// Token: 0x040054D3 RID: 21715
		public StateMachine.Category Misc;
	}
}
