using System;

// Token: 0x02000465 RID: 1125
public struct PrioritySetting : IComparable<PrioritySetting>
{
	// Token: 0x06001804 RID: 6148 RVA: 0x0008077C File Offset: 0x0007E97C
	public override int GetHashCode()
	{
		return ((int)((int)this.priority_class << 28)).GetHashCode() ^ this.priority_value.GetHashCode();
	}

	// Token: 0x06001805 RID: 6149 RVA: 0x000807A6 File Offset: 0x0007E9A6
	public static bool operator ==(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.Equals(rhs);
	}

	// Token: 0x06001806 RID: 6150 RVA: 0x000807BB File Offset: 0x0007E9BB
	public static bool operator !=(PrioritySetting lhs, PrioritySetting rhs)
	{
		return !lhs.Equals(rhs);
	}

	// Token: 0x06001807 RID: 6151 RVA: 0x000807D3 File Offset: 0x0007E9D3
	public static bool operator <=(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) <= 0;
	}

	// Token: 0x06001808 RID: 6152 RVA: 0x000807E3 File Offset: 0x0007E9E3
	public static bool operator >=(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) >= 0;
	}

	// Token: 0x06001809 RID: 6153 RVA: 0x000807F3 File Offset: 0x0007E9F3
	public static bool operator <(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) < 0;
	}

	// Token: 0x0600180A RID: 6154 RVA: 0x00080800 File Offset: 0x0007EA00
	public static bool operator >(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) > 0;
	}

	// Token: 0x0600180B RID: 6155 RVA: 0x0008080D File Offset: 0x0007EA0D
	public override bool Equals(object obj)
	{
		return obj is PrioritySetting && ((PrioritySetting)obj).priority_class == this.priority_class && ((PrioritySetting)obj).priority_value == this.priority_value;
	}

	// Token: 0x0600180C RID: 6156 RVA: 0x00080844 File Offset: 0x0007EA44
	public int CompareTo(PrioritySetting other)
	{
		if (this.priority_class > other.priority_class)
		{
			return 1;
		}
		if (this.priority_class < other.priority_class)
		{
			return -1;
		}
		if (this.priority_value > other.priority_value)
		{
			return 1;
		}
		if (this.priority_value < other.priority_value)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x0600180D RID: 6157 RVA: 0x00080892 File Offset: 0x0007EA92
	public PrioritySetting(PriorityScreen.PriorityClass priority_class, int priority_value)
	{
		this.priority_class = priority_class;
		this.priority_value = priority_value;
	}

	// Token: 0x04000D4A RID: 3402
	public PriorityScreen.PriorityClass priority_class;

	// Token: 0x04000D4B RID: 3403
	public int priority_value;
}
