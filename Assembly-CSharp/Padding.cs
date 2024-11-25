using System;

// Token: 0x02000412 RID: 1042
public readonly struct Padding
{
	// Token: 0x1700005B RID: 91
	// (get) Token: 0x0600160E RID: 5646 RVA: 0x00077C99 File Offset: 0x00075E99
	public float Width
	{
		get
		{
			return this.left + this.right;
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x0600160F RID: 5647 RVA: 0x00077CA8 File Offset: 0x00075EA8
	public float Height
	{
		get
		{
			return this.top + this.bottom;
		}
	}

	// Token: 0x06001610 RID: 5648 RVA: 0x00077CB7 File Offset: 0x00075EB7
	public Padding(float left, float right, float top, float bottom)
	{
		this.top = top;
		this.bottom = bottom;
		this.left = left;
		this.right = right;
	}

	// Token: 0x06001611 RID: 5649 RVA: 0x00077CD6 File Offset: 0x00075ED6
	public static Padding All(float padding)
	{
		return new Padding(padding, padding, padding, padding);
	}

	// Token: 0x06001612 RID: 5650 RVA: 0x00077CE1 File Offset: 0x00075EE1
	public static Padding Symmetric(float horizontal, float vertical)
	{
		return new Padding(horizontal, horizontal, vertical, vertical);
	}

	// Token: 0x06001613 RID: 5651 RVA: 0x00077CEC File Offset: 0x00075EEC
	public static Padding Only(float left = 0f, float right = 0f, float top = 0f, float bottom = 0f)
	{
		return new Padding(left, right, top, bottom);
	}

	// Token: 0x06001614 RID: 5652 RVA: 0x00077CF7 File Offset: 0x00075EF7
	public static Padding Vertical(float vertical)
	{
		return new Padding(0f, 0f, vertical, vertical);
	}

	// Token: 0x06001615 RID: 5653 RVA: 0x00077D0A File Offset: 0x00075F0A
	public static Padding Horizontal(float horizontal)
	{
		return new Padding(horizontal, horizontal, 0f, 0f);
	}

	// Token: 0x06001616 RID: 5654 RVA: 0x00077D1D File Offset: 0x00075F1D
	public static Padding Top(float amount)
	{
		return new Padding(0f, 0f, amount, 0f);
	}

	// Token: 0x06001617 RID: 5655 RVA: 0x00077D34 File Offset: 0x00075F34
	public static Padding Left(float amount)
	{
		return new Padding(amount, 0f, 0f, 0f);
	}

	// Token: 0x06001618 RID: 5656 RVA: 0x00077D4B File Offset: 0x00075F4B
	public static Padding Bottom(float amount)
	{
		return new Padding(0f, 0f, 0f, amount);
	}

	// Token: 0x06001619 RID: 5657 RVA: 0x00077D62 File Offset: 0x00075F62
	public static Padding Right(float amount)
	{
		return new Padding(0f, amount, 0f, 0f);
	}

	// Token: 0x0600161A RID: 5658 RVA: 0x00077D79 File Offset: 0x00075F79
	public static Padding operator +(Padding a, Padding b)
	{
		return new Padding(a.left + b.left, a.right + b.right, a.top + b.top, a.bottom + b.bottom);
	}

	// Token: 0x0600161B RID: 5659 RVA: 0x00077DB4 File Offset: 0x00075FB4
	public static Padding operator -(Padding a, Padding b)
	{
		return new Padding(a.left - b.left, a.right - b.right, a.top - b.top, a.bottom - b.bottom);
	}

	// Token: 0x0600161C RID: 5660 RVA: 0x00077DEF File Offset: 0x00075FEF
	public static Padding operator *(float f, Padding p)
	{
		return p * f;
	}

	// Token: 0x0600161D RID: 5661 RVA: 0x00077DF8 File Offset: 0x00075FF8
	public static Padding operator *(Padding p, float f)
	{
		return new Padding(p.left * f, p.right * f, p.top * f, p.bottom * f);
	}

	// Token: 0x0600161E RID: 5662 RVA: 0x00077E1F File Offset: 0x0007601F
	public static Padding operator /(Padding p, float f)
	{
		return new Padding(p.left / f, p.right / f, p.top / f, p.bottom / f);
	}

	// Token: 0x04000C7B RID: 3195
	public readonly float top;

	// Token: 0x04000C7C RID: 3196
	public readonly float bottom;

	// Token: 0x04000C7D RID: 3197
	public readonly float left;

	// Token: 0x04000C7E RID: 3198
	public readonly float right;
}
