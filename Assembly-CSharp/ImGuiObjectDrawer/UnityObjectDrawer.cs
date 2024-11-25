using System;
using ImGuiNET;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E3D RID: 3645
	public class UnityObjectDrawer : PlainCSharpObjectDrawer
	{
		// Token: 0x0600740B RID: 29707 RVA: 0x002C5418 File Offset: 0x002C3618
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is UnityEngine.Object;
		}

		// Token: 0x0600740C RID: 29708 RVA: 0x002C5428 File Offset: 0x002C3628
		protected override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			UnityEngine.Object @object = (UnityEngine.Object)member.value;
			ImGuiTreeNodeFlags imGuiTreeNodeFlags = ImGuiTreeNodeFlags.None;
			if (context.default_open && depth <= 0)
			{
				imGuiTreeNodeFlags |= ImGuiTreeNodeFlags.DefaultOpen;
			}
			bool flag = ImGui.TreeNodeEx(member.name, imGuiTreeNodeFlags);
			DrawerUtil.Tooltip(member.type);
			if (flag)
			{
				base.DrawContents(context, member, depth);
				ImGui.TreePop();
			}
		}
	}
}
