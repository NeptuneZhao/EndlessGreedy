using System;

// Token: 0x02000419 RID: 1049
public static class Result
{
	// Token: 0x0600164B RID: 5707 RVA: 0x000783DD File Offset: 0x000765DD
	public static Result.Internal.Value_Ok<T> Ok<T>(T value)
	{
		return new Result.Internal.Value_Ok<T>(value);
	}

	// Token: 0x0600164C RID: 5708 RVA: 0x000783E5 File Offset: 0x000765E5
	public static Result.Internal.Value_Err<T> Err<T>(T value)
	{
		return new Result.Internal.Value_Err<T>(value);
	}

	// Token: 0x02001174 RID: 4468
	public static class Internal
	{
		// Token: 0x02002398 RID: 9112
		public readonly struct Value_Ok<T>
		{
			// Token: 0x0600B6FC RID: 46844 RVA: 0x003CCFCA File Offset: 0x003CB1CA
			public Value_Ok(T value)
			{
				this.value = value;
			}

			// Token: 0x04009F05 RID: 40709
			public readonly T value;
		}

		// Token: 0x02002399 RID: 9113
		public readonly struct Value_Err<T>
		{
			// Token: 0x0600B6FD RID: 46845 RVA: 0x003CCFD3 File Offset: 0x003CB1D3
			public Value_Err(T value)
			{
				this.value = value;
			}

			// Token: 0x04009F06 RID: 40710
			public readonly T value;
		}
	}
}
