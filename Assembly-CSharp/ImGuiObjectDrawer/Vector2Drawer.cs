using System;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E36 RID: 3638
	public sealed class Vector2Drawer : InlineDrawer
	{
		// Token: 0x060073EE RID: 29678 RVA: 0x002C4FC6 File Offset: 0x002C31C6
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is Vector2;
		}

		// Token: 0x060073EF RID: 29679 RVA: 0x002C4FD8 File Offset: 0x002C31D8
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Vector2 vector = (Vector2)member.value;
			ImGuiEx.SimpleField(member.name, string.Format("( {0}, {1} )", vector.x, vector.y));
		}
	}
}
