using System;
using ImGuiNET;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E39 RID: 3641
	public abstract class CollectionDrawer : MemberDrawer
	{
		// Token: 0x060073F7 RID: 29687
		public abstract bool IsEmpty(in MemberDrawContext context, in MemberDetails member);

		// Token: 0x060073F8 RID: 29688 RVA: 0x002C5110 File Offset: 0x002C3310
		public override MemberDrawType GetDrawType(in MemberDrawContext context, in MemberDetails member)
		{
			if (this.IsEmpty(context, member))
			{
				return MemberDrawType.Inline;
			}
			return MemberDrawType.Custom;
		}

		// Token: 0x060073F9 RID: 29689 RVA: 0x002C511F File Offset: 0x002C331F
		protected sealed override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Debug.Assert(this.IsEmpty(context, member));
			this.DrawEmpty(context, member);
		}

		// Token: 0x060073FA RID: 29690 RVA: 0x002C5136 File Offset: 0x002C3336
		protected sealed override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			Debug.Assert(!this.IsEmpty(context, member));
			this.DrawWithContents(context, member, depth);
		}

		// Token: 0x060073FB RID: 29691 RVA: 0x002C5151 File Offset: 0x002C3351
		private void DrawEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			ImGui.Text(member.name + "(empty)");
		}

		// Token: 0x060073FC RID: 29692 RVA: 0x002C5168 File Offset: 0x002C3368
		private void DrawWithContents(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			CollectionDrawer.<>c__DisplayClass5_0 CS$<>8__locals1 = new CollectionDrawer.<>c__DisplayClass5_0();
			CS$<>8__locals1.depth = depth;
			ImGuiTreeNodeFlags imGuiTreeNodeFlags = ImGuiTreeNodeFlags.None;
			if (context.default_open && CS$<>8__locals1.depth <= 0)
			{
				imGuiTreeNodeFlags |= ImGuiTreeNodeFlags.DefaultOpen;
			}
			bool flag = ImGui.TreeNodeEx(member.name, imGuiTreeNodeFlags);
			DrawerUtil.Tooltip(member.type);
			if (flag)
			{
				this.VisitElements(new CollectionDrawer.ElementVisitor(CS$<>8__locals1.<DrawWithContents>g__Visitor|0), context, member);
				ImGui.TreePop();
			}
		}

		// Token: 0x060073FD RID: 29693
		protected abstract void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member);

		// Token: 0x02001F51 RID: 8017
		// (Invoke) Token: 0x0600ADE9 RID: 44521
		protected delegate void ElementVisitor(in MemberDrawContext context, CollectionDrawer.Element element);

		// Token: 0x02001F52 RID: 8018
		protected struct Element
		{
			// Token: 0x0600ADEC RID: 44524 RVA: 0x003AAFA8 File Offset: 0x003A91A8
			public Element(string node_name, System.Action draw_tooltip, Func<object> get_object_to_inspect)
			{
				this.node_name = node_name;
				this.draw_tooltip = draw_tooltip;
				this.get_object_to_inspect = get_object_to_inspect;
			}

			// Token: 0x0600ADED RID: 44525 RVA: 0x003AAFBF File Offset: 0x003A91BF
			public Element(int index, System.Action draw_tooltip, Func<object> get_object_to_inspect)
			{
				this = new CollectionDrawer.Element(string.Format("[{0}]", index), draw_tooltip, get_object_to_inspect);
			}

			// Token: 0x04008D41 RID: 36161
			public readonly string node_name;

			// Token: 0x04008D42 RID: 36162
			public readonly System.Action draw_tooltip;

			// Token: 0x04008D43 RID: 36163
			public readonly Func<object> get_object_to_inspect;
		}
	}
}
