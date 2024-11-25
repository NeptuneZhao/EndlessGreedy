using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E33 RID: 3635
	public sealed class EnumDrawer : InlineDrawer
	{
		// Token: 0x060073E5 RID: 29669 RVA: 0x002C4EA2 File Offset: 0x002C30A2
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.type.IsEnum;
		}

		// Token: 0x060073E6 RID: 29670 RVA: 0x002C4EAF File Offset: 0x002C30AF
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, member.value.ToString());
		}
	}
}
