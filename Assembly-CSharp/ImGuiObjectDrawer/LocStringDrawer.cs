using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E32 RID: 3634
	public sealed class LocStringDrawer : InlineDrawer
	{
		// Token: 0x060073E2 RID: 29666 RVA: 0x002C4E65 File Offset: 0x002C3065
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.CanAssignToType<LocString>();
		}

		// Token: 0x060073E3 RID: 29667 RVA: 0x002C4E6D File Offset: 0x002C306D
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, string.Format("{0}({1})", member.value, ((LocString)member.value).text));
		}
	}
}
