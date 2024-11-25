using System;
using ImGuiNET;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E3E RID: 3646
	public class PlainCSharpObjectDrawer : MemberDrawer
	{
		// Token: 0x0600740E RID: 29710 RVA: 0x002C5483 File Offset: 0x002C3683
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return true;
		}

		// Token: 0x0600740F RID: 29711 RVA: 0x002C5486 File Offset: 0x002C3686
		public override MemberDrawType GetDrawType(in MemberDrawContext context, in MemberDetails member)
		{
			return MemberDrawType.Custom;
		}

		// Token: 0x06007410 RID: 29712 RVA: 0x002C5489 File Offset: 0x002C3689
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06007411 RID: 29713 RVA: 0x002C5490 File Offset: 0x002C3690
		protected override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			ImGuiTreeNodeFlags imGuiTreeNodeFlags = ImGuiTreeNodeFlags.None;
			if (context.default_open && depth <= 0)
			{
				imGuiTreeNodeFlags |= ImGuiTreeNodeFlags.DefaultOpen;
			}
			bool flag = ImGui.TreeNodeEx(member.name, imGuiTreeNodeFlags);
			DrawerUtil.Tooltip(member.type);
			if (flag)
			{
				this.DrawContents(context, member, depth);
				ImGui.TreePop();
			}
		}

		// Token: 0x06007412 RID: 29714 RVA: 0x002C54D7 File Offset: 0x002C36D7
		protected virtual void DrawContents(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			DrawerUtil.DrawObjectContents(member.value, context, depth + 1);
		}
	}
}
