using System;
using System.Collections.Generic;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000E2D RID: 3629
	public class PrimaryMemberDrawerProvider : IMemberDrawerProvider
	{
		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x060073D1 RID: 29649 RVA: 0x002C4D3C File Offset: 0x002C2F3C
		public int Priority
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x060073D2 RID: 29650 RVA: 0x002C4D40 File Offset: 0x002C2F40
		public void AppendDrawersTo(List<MemberDrawer> drawers)
		{
			drawers.AddRange(new MemberDrawer[]
			{
				new NullDrawer(),
				new SimpleDrawer(),
				new LocStringDrawer(),
				new EnumDrawer(),
				new HashedStringDrawer(),
				new KAnimHashedStringDrawer(),
				new Vector2Drawer(),
				new Vector3Drawer(),
				new Vector4Drawer(),
				new UnityObjectDrawer(),
				new ArrayDrawer(),
				new IDictionaryDrawer(),
				new IEnumerableDrawer(),
				new PlainCSharpObjectDrawer(),
				new FallbackDrawer()
			});
		}
	}
}
