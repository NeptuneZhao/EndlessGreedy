using System;
using System.Collections;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E3B RID: 3643
	public sealed class IDictionaryDrawer : CollectionDrawer
	{
		// Token: 0x06007403 RID: 29699 RVA: 0x002C52AE File Offset: 0x002C34AE
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.CanAssignToType<IDictionary>();
		}

		// Token: 0x06007404 RID: 29700 RVA: 0x002C52B6 File Offset: 0x002C34B6
		public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			return ((IDictionary)member.value).Count == 0;
		}

		// Token: 0x06007405 RID: 29701 RVA: 0x002C52CC File Offset: 0x002C34CC
		protected override void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member)
		{
			IDictionary dictionary = (IDictionary)member.value;
			int num = 0;
			using (IDictionaryEnumerator enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DictionaryEntry kvp = (DictionaryEntry)enumerator.Current;
					visit(context, new CollectionDrawer.Element(num, delegate()
					{
						DrawerUtil.Tooltip(string.Format("{0} -> {1}", kvp.Key.GetType(), kvp.Value.GetType()));
					}, () => new
					{
						key = kvp.Key,
						value = kvp.Value
					}));
					num++;
				}
			}
		}
	}
}
