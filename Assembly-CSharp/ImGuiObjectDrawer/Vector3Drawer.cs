using System;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E37 RID: 3639
	public sealed class Vector3Drawer : InlineDrawer
	{
		// Token: 0x060073F1 RID: 29681 RVA: 0x002C5024 File Offset: 0x002C3224
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is Vector3;
		}

		// Token: 0x060073F2 RID: 29682 RVA: 0x002C5034 File Offset: 0x002C3234
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Vector3 vector = (Vector3)member.value;
			ImGuiEx.SimpleField(member.name, string.Format("( {0}, {1}, {2} )", vector.x, vector.y, vector.z));
		}
	}
}
