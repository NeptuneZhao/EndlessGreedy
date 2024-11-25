using System;

// Token: 0x02000483 RID: 1155
public struct NavOffset
{
	// Token: 0x060018E9 RID: 6377 RVA: 0x00085DBC File Offset: 0x00083FBC
	public NavOffset(NavType nav_type, int x, int y)
	{
		this.navType = nav_type;
		this.offset.x = x;
		this.offset.y = y;
	}

	// Token: 0x04000DEA RID: 3562
	public NavType navType;

	// Token: 0x04000DEB RID: 3563
	public CellOffset offset;
}
