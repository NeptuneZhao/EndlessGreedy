using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E31 RID: 3633
	public sealed class FallbackDrawer : SimpleDrawer
	{
		// Token: 0x060073DF RID: 29663 RVA: 0x002C4E57 File Offset: 0x002C3057
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return true;
		}

		// Token: 0x060073E0 RID: 29664 RVA: 0x002C4E5A File Offset: 0x002C305A
		public override bool CanDrawAtDepth(int depth)
		{
			return true;
		}
	}
}
