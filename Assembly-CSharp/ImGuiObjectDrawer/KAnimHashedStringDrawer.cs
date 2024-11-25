using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E35 RID: 3637
	public sealed class KAnimHashedStringDrawer : InlineDrawer
	{
		// Token: 0x060073EB RID: 29675 RVA: 0x002C4F4A File Offset: 0x002C314A
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is KAnimHashedString;
		}

		// Token: 0x060073EC RID: 29676 RVA: 0x002C4F5C File Offset: 0x002C315C
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			KAnimHashedString kanimHashedString = (KAnimHashedString)member.value;
			string str = kanimHashedString.ToString();
			string str2 = "0x" + kanimHashedString.HashValue.ToString("X");
			ImGuiEx.SimpleField(member.name, str + " (" + str2 + ")");
		}
	}
}
