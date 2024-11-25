using System;

namespace Klei.Actions
{
	// Token: 0x02000F8B RID: 3979
	[AttributeUsage(AttributeTargets.Class)]
	public class ActionAttribute : Attribute
	{
		// Token: 0x060079E2 RID: 31202 RVA: 0x003016B6 File Offset: 0x002FF8B6
		public ActionAttribute(string actionName)
		{
			this.ActionName = actionName;
		}

		// Token: 0x04005AED RID: 23277
		public readonly string ActionName;
	}
}
