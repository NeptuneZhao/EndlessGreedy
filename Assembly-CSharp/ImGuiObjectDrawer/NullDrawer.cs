using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E2F RID: 3631
	public class NullDrawer : InlineDrawer
	{
		// Token: 0x060073D7 RID: 29655 RVA: 0x002C4DF5 File Offset: 0x002C2FF5
		public override bool CanDrawAtDepth(int depth)
		{
			return true;
		}

		// Token: 0x060073D8 RID: 29656 RVA: 0x002C4DF8 File Offset: 0x002C2FF8
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value == null;
		}

		// Token: 0x060073D9 RID: 29657 RVA: 0x002C4E03 File Offset: 0x002C3003
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, "null");
		}
	}
}
