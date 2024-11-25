using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;

// Token: 0x02000410 RID: 1040
[DebuggerDisplay("has_value={hasValue} {value}")]
[Serializable]
public readonly struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
{
	// Token: 0x060015ED RID: 5613 RVA: 0x00077934 File Offset: 0x00075B34
	public Option(T value)
	{
		this.value = value;
		this.hasValue = true;
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x060015EE RID: 5614 RVA: 0x00077944 File Offset: 0x00075B44
	public bool HasValue
	{
		get
		{
			return this.hasValue;
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x060015EF RID: 5615 RVA: 0x0007794C File Offset: 0x00075B4C
	public T Value
	{
		get
		{
			return this.Unwrap();
		}
	}

	// Token: 0x060015F0 RID: 5616 RVA: 0x00077954 File Offset: 0x00075B54
	public T Unwrap()
	{
		if (!this.hasValue)
		{
			throw new Exception("Tried to get a value for a Option<" + typeof(T).FullName + ">, but hasValue is false");
		}
		return this.value;
	}

	// Token: 0x060015F1 RID: 5617 RVA: 0x00077988 File Offset: 0x00075B88
	public T UnwrapOr(T fallback_value, string warn_on_fallback = null)
	{
		if (!this.hasValue)
		{
			if (warn_on_fallback != null)
			{
				DebugUtil.DevAssert(false, "Failed to unwrap a Option<" + typeof(T).FullName + ">: " + warn_on_fallback, null);
			}
			return fallback_value;
		}
		return this.value;
	}

	// Token: 0x060015F2 RID: 5618 RVA: 0x000779C3 File Offset: 0x00075BC3
	public T UnwrapOrElse(Func<T> get_fallback_value_fn, string warn_on_fallback = null)
	{
		if (!this.hasValue)
		{
			if (warn_on_fallback != null)
			{
				DebugUtil.DevAssert(false, "Failed to unwrap a Option<" + typeof(T).FullName + ">: " + warn_on_fallback, null);
			}
			return get_fallback_value_fn();
		}
		return this.value;
	}

	// Token: 0x060015F3 RID: 5619 RVA: 0x00077A04 File Offset: 0x00075C04
	public T UnwrapOrDefault()
	{
		if (!this.hasValue)
		{
			return default(T);
		}
		return this.value;
	}

	// Token: 0x060015F4 RID: 5620 RVA: 0x00077A29 File Offset: 0x00075C29
	public T Expect(string msg_on_fail)
	{
		if (!this.hasValue)
		{
			throw new Exception(msg_on_fail);
		}
		return this.value;
	}

	// Token: 0x060015F5 RID: 5621 RVA: 0x00077A40 File Offset: 0x00075C40
	public bool IsSome()
	{
		return this.hasValue;
	}

	// Token: 0x060015F6 RID: 5622 RVA: 0x00077A48 File Offset: 0x00075C48
	public bool IsNone()
	{
		return !this.hasValue;
	}

	// Token: 0x060015F7 RID: 5623 RVA: 0x00077A53 File Offset: 0x00075C53
	public Option<U> AndThen<U>(Func<T, U> fn)
	{
		if (this.IsNone())
		{
			return Option.None;
		}
		return Option.Maybe<U>(fn(this.value));
	}

	// Token: 0x060015F8 RID: 5624 RVA: 0x00077A79 File Offset: 0x00075C79
	public Option<U> AndThen<U>(Func<T, Option<U>> fn)
	{
		if (this.IsNone())
		{
			return Option.None;
		}
		return fn(this.value);
	}

	// Token: 0x060015F9 RID: 5625 RVA: 0x00077A9A File Offset: 0x00075C9A
	public static implicit operator Option<T>(T value)
	{
		return Option.Maybe<T>(value);
	}

	// Token: 0x060015FA RID: 5626 RVA: 0x00077AA2 File Offset: 0x00075CA2
	public static explicit operator T(Option<T> option)
	{
		return option.Unwrap();
	}

	// Token: 0x060015FB RID: 5627 RVA: 0x00077AAC File Offset: 0x00075CAC
	public static implicit operator Option<T>(Option.Internal.Value_None value)
	{
		return default(Option<T>);
	}

	// Token: 0x060015FC RID: 5628 RVA: 0x00077AC2 File Offset: 0x00075CC2
	public static implicit operator Option.Internal.Value_HasValue(Option<T> value)
	{
		return new Option.Internal.Value_HasValue(value.hasValue);
	}

	// Token: 0x060015FD RID: 5629 RVA: 0x00077ACF File Offset: 0x00075CCF
	public void Deconstruct(out bool hasValue, out T value)
	{
		hasValue = this.hasValue;
		value = this.value;
	}

	// Token: 0x060015FE RID: 5630 RVA: 0x00077AE5 File Offset: 0x00075CE5
	public bool Equals(Option<T> other)
	{
		return EqualityComparer<bool>.Default.Equals(this.hasValue, other.hasValue) && EqualityComparer<T>.Default.Equals(this.value, other.value);
	}

	// Token: 0x060015FF RID: 5631 RVA: 0x00077B18 File Offset: 0x00075D18
	public override bool Equals(object obj)
	{
		if (obj is Option<T>)
		{
			Option<T> other = (Option<T>)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x06001600 RID: 5632 RVA: 0x00077B3D File Offset: 0x00075D3D
	public static bool operator ==(Option<T> lhs, Option<T> rhs)
	{
		return lhs.Equals(rhs);
	}

	// Token: 0x06001601 RID: 5633 RVA: 0x00077B47 File Offset: 0x00075D47
	public static bool operator !=(Option<T> lhs, Option<T> rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x06001602 RID: 5634 RVA: 0x00077B54 File Offset: 0x00075D54
	public override int GetHashCode()
	{
		return (-363764631 * -1521134295 + this.hasValue.GetHashCode()) * -1521134295 + EqualityComparer<T>.Default.GetHashCode(this.value);
	}

	// Token: 0x06001603 RID: 5635 RVA: 0x00077B92 File Offset: 0x00075D92
	public override string ToString()
	{
		if (!this.hasValue)
		{
			return "None";
		}
		return string.Format("{0}", this.value);
	}

	// Token: 0x06001604 RID: 5636 RVA: 0x00077BB7 File Offset: 0x00075DB7
	public static bool operator ==(Option<T> lhs, T rhs)
	{
		return lhs.Equals(rhs);
	}

	// Token: 0x06001605 RID: 5637 RVA: 0x00077BC1 File Offset: 0x00075DC1
	public static bool operator !=(Option<T> lhs, T rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x06001606 RID: 5638 RVA: 0x00077BCD File Offset: 0x00075DCD
	public static bool operator ==(T lhs, Option<T> rhs)
	{
		return rhs.Equals(lhs);
	}

	// Token: 0x06001607 RID: 5639 RVA: 0x00077BD7 File Offset: 0x00075DD7
	public static bool operator !=(T lhs, Option<T> rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x06001608 RID: 5640 RVA: 0x00077BE3 File Offset: 0x00075DE3
	public bool Equals(T other)
	{
		return this.HasValue && EqualityComparer<T>.Default.Equals(this.value, other);
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06001609 RID: 5641 RVA: 0x00077C00 File Offset: 0x00075E00
	public static Option<T> None
	{
		get
		{
			return default(Option<T>);
		}
	}

	// Token: 0x04000C79 RID: 3193
	[Serialize]
	private readonly bool hasValue;

	// Token: 0x04000C7A RID: 3194
	[Serialize]
	private readonly T value;
}
