using System;

// Token: 0x02000411 RID: 1041
public static class Option
{
	// Token: 0x0600160A RID: 5642 RVA: 0x00077C16 File Offset: 0x00075E16
	public static Option<T> Some<T>(T value)
	{
		return new Option<T>(value);
	}

	// Token: 0x0600160B RID: 5643 RVA: 0x00077C20 File Offset: 0x00075E20
	public static Option<T> Maybe<T>(T value)
	{
		if (value.IsNullOrDestroyed())
		{
			return default(Option<T>);
		}
		return new Option<T>(value);
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x0600160C RID: 5644 RVA: 0x00077C4C File Offset: 0x00075E4C
	public static Option.Internal.Value_None None
	{
		get
		{
			return default(Option.Internal.Value_None);
		}
	}

	// Token: 0x0600160D RID: 5645 RVA: 0x00077C64 File Offset: 0x00075E64
	public static bool AllHaveValues(params Option.Internal.Value_HasValue[] options)
	{
		if (options == null || options.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < options.Length; i++)
		{
			if (!options[i].HasValue)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0200116B RID: 4459
	public static class Internal
	{
		// Token: 0x02002396 RID: 9110
		public readonly struct Value_None
		{
		}

		// Token: 0x02002397 RID: 9111
		public readonly struct Value_HasValue
		{
			// Token: 0x0600B6FB RID: 46843 RVA: 0x003CCFC1 File Offset: 0x003CB1C1
			public Value_HasValue(bool hasValue)
			{
				this.HasValue = hasValue;
			}

			// Token: 0x04009F04 RID: 40708
			public readonly bool HasValue;
		}
	}
}
