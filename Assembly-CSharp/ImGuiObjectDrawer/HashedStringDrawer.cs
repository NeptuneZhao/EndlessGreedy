using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E34 RID: 3636
	public sealed class HashedStringDrawer : InlineDrawer
	{
		// Token: 0x060073E8 RID: 29672 RVA: 0x002C4ECF File Offset: 0x002C30CF
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is HashedString;
		}

		// Token: 0x060073E9 RID: 29673 RVA: 0x002C4EE0 File Offset: 0x002C30E0
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			HashedString hashedString = (HashedString)member.value;
			string str = hashedString.ToString();
			string str2 = "0x" + hashedString.HashValue.ToString("X");
			ImGuiEx.SimpleField(member.name, str + " (" + str2 + ")");
		}
	}
}
