using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E3A RID: 3642
	public sealed class ArrayDrawer : CollectionDrawer
	{
		// Token: 0x060073FF RID: 29695 RVA: 0x002C51D4 File Offset: 0x002C33D4
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.type.IsArray;
		}

		// Token: 0x06007400 RID: 29696 RVA: 0x002C51E1 File Offset: 0x002C33E1
		public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			return ((Array)member.value).Length == 0;
		}

		// Token: 0x06007401 RID: 29697 RVA: 0x002C51F8 File Offset: 0x002C33F8
		protected override void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member)
		{
			ArrayDrawer.<>c__DisplayClass2_0 CS$<>8__locals1 = new ArrayDrawer.<>c__DisplayClass2_0();
			CS$<>8__locals1.array = (Array)member.value;
			int i;
			int i2;
			for (i = 0; i < CS$<>8__locals1.array.Length; i = i2)
			{
				int j = i;
				System.Action draw_tooltip;
				if ((draw_tooltip = CS$<>8__locals1.<>9__0) == null)
				{
					draw_tooltip = (CS$<>8__locals1.<>9__0 = delegate()
					{
						DrawerUtil.Tooltip(CS$<>8__locals1.array.GetType().GetElementType());
					});
				}
				visit(context, new CollectionDrawer.Element(j, draw_tooltip, () => new
				{
					value = CS$<>8__locals1.array.GetValue(i)
				}));
				i2 = i + 1;
			}
		}
	}
}
