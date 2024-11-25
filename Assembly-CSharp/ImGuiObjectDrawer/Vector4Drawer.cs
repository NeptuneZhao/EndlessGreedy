using System;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E38 RID: 3640
	public sealed class Vector4Drawer : InlineDrawer
	{
		// Token: 0x060073F4 RID: 29684 RVA: 0x002C508B File Offset: 0x002C328B
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is Vector4;
		}

		// Token: 0x060073F5 RID: 29685 RVA: 0x002C509C File Offset: 0x002C329C
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Vector4 vector = (Vector4)member.value;
			ImGuiEx.SimpleField(member.name, string.Format("( {0}, {1}, {2}, {3} )", new object[]
			{
				vector.x,
				vector.y,
				vector.z,
				vector.w
			}));
		}
	}
}
