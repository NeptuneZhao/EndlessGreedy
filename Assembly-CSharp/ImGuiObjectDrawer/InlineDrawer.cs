using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E2E RID: 3630
	public abstract class InlineDrawer : MemberDrawer
	{
		// Token: 0x060073D4 RID: 29652 RVA: 0x002C4DE0 File Offset: 0x002C2FE0
		public sealed override MemberDrawType GetDrawType(in MemberDrawContext context, in MemberDetails member)
		{
			return MemberDrawType.Inline;
		}

		// Token: 0x060073D5 RID: 29653 RVA: 0x002C4DE3 File Offset: 0x002C2FE3
		protected sealed override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			this.DrawInline(context, member);
		}
	}
}
