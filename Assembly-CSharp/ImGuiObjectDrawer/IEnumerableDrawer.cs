using System;
using System.Collections;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E3C RID: 3644
	public sealed class IEnumerableDrawer : CollectionDrawer
	{
		// Token: 0x06007407 RID: 29703 RVA: 0x002C5364 File Offset: 0x002C3564
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.CanAssignToType<IEnumerable>();
		}

		// Token: 0x06007408 RID: 29704 RVA: 0x002C536C File Offset: 0x002C356C
		public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			return !((IEnumerable)member.value).GetEnumerator().MoveNext();
		}

		// Token: 0x06007409 RID: 29705 RVA: 0x002C5388 File Offset: 0x002C3588
		protected override void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member)
		{
			IEnumerable enumerable = (IEnumerable)member.value;
			int num = 0;
			using (IEnumerator enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object el = enumerator.Current;
					visit(context, new CollectionDrawer.Element(num, delegate()
					{
						DrawerUtil.Tooltip(el.GetType());
					}, () => new
					{
						value = el
					}));
					num++;
				}
			}
		}
	}
}
