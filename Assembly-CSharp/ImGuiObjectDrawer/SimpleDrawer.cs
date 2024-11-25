using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E30 RID: 3632
	public class SimpleDrawer : InlineDrawer
	{
		// Token: 0x060073DB RID: 29659 RVA: 0x002C4E1D File Offset: 0x002C301D
		public override bool CanDrawAtDepth(int depth)
		{
			return true;
		}

		// Token: 0x060073DC RID: 29660 RVA: 0x002C4E20 File Offset: 0x002C3020
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.type.IsPrimitive || member.CanAssignToType<string>();
		}

		// Token: 0x060073DD RID: 29661 RVA: 0x002C4E37 File Offset: 0x002C3037
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, member.value.ToString());
		}
	}
}
