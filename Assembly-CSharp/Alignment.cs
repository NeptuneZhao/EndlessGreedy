using System;
using UnityEngine;

// Token: 0x02000C94 RID: 3220
public readonly struct Alignment
{
	// Token: 0x060062E3 RID: 25315 RVA: 0x0024DE14 File Offset: 0x0024C014
	public Alignment(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	// Token: 0x060062E4 RID: 25316 RVA: 0x0024DE24 File Offset: 0x0024C024
	public static Alignment Custom(float x, float y)
	{
		return new Alignment(x, y);
	}

	// Token: 0x060062E5 RID: 25317 RVA: 0x0024DE2D File Offset: 0x0024C02D
	public static Alignment TopLeft()
	{
		return new Alignment(0f, 1f);
	}

	// Token: 0x060062E6 RID: 25318 RVA: 0x0024DE3E File Offset: 0x0024C03E
	public static Alignment Top()
	{
		return new Alignment(0.5f, 1f);
	}

	// Token: 0x060062E7 RID: 25319 RVA: 0x0024DE4F File Offset: 0x0024C04F
	public static Alignment TopRight()
	{
		return new Alignment(1f, 1f);
	}

	// Token: 0x060062E8 RID: 25320 RVA: 0x0024DE60 File Offset: 0x0024C060
	public static Alignment Left()
	{
		return new Alignment(0f, 0.5f);
	}

	// Token: 0x060062E9 RID: 25321 RVA: 0x0024DE71 File Offset: 0x0024C071
	public static Alignment Center()
	{
		return new Alignment(0.5f, 0.5f);
	}

	// Token: 0x060062EA RID: 25322 RVA: 0x0024DE82 File Offset: 0x0024C082
	public static Alignment Right()
	{
		return new Alignment(1f, 0.5f);
	}

	// Token: 0x060062EB RID: 25323 RVA: 0x0024DE93 File Offset: 0x0024C093
	public static Alignment BottomLeft()
	{
		return new Alignment(0f, 0f);
	}

	// Token: 0x060062EC RID: 25324 RVA: 0x0024DEA4 File Offset: 0x0024C0A4
	public static Alignment Bottom()
	{
		return new Alignment(0.5f, 0f);
	}

	// Token: 0x060062ED RID: 25325 RVA: 0x0024DEB5 File Offset: 0x0024C0B5
	public static Alignment BottomRight()
	{
		return new Alignment(1f, 0f);
	}

	// Token: 0x060062EE RID: 25326 RVA: 0x0024DEC6 File Offset: 0x0024C0C6
	public static implicit operator Vector2(Alignment a)
	{
		return new Vector2(a.x, a.y);
	}

	// Token: 0x060062EF RID: 25327 RVA: 0x0024DED9 File Offset: 0x0024C0D9
	public static implicit operator Alignment(Vector2 v)
	{
		return new Alignment(v.x, v.y);
	}

	// Token: 0x0400431D RID: 17181
	public readonly float x;

	// Token: 0x0400431E RID: 17182
	public readonly float y;
}
