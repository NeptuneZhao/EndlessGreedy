using System;

// Token: 0x02000418 RID: 1048
public readonly struct Result<TSuccess, TError>
{
	// Token: 0x06001643 RID: 5699 RVA: 0x00078306 File Offset: 0x00076506
	private Result(TSuccess successValue, TError errorValue)
	{
		this.successValue = successValue;
		this.errorValue = errorValue;
	}

	// Token: 0x06001644 RID: 5700 RVA: 0x00078320 File Offset: 0x00076520
	public bool IsOk()
	{
		return this.successValue.IsSome();
	}

	// Token: 0x06001645 RID: 5701 RVA: 0x0007832D File Offset: 0x0007652D
	public bool IsErr()
	{
		return this.errorValue.IsSome() || this.successValue.IsNone();
	}

	// Token: 0x06001646 RID: 5702 RVA: 0x00078349 File Offset: 0x00076549
	public TSuccess Unwrap()
	{
		if (this.successValue.IsSome())
		{
			return this.successValue.Unwrap();
		}
		if (this.errorValue.IsSome())
		{
			throw new Exception("Tried to unwrap result that is an Err()");
		}
		throw new Exception("Tried to unwrap result that isn't initialized with an Err() or Ok() value");
	}

	// Token: 0x06001647 RID: 5703 RVA: 0x00078386 File Offset: 0x00076586
	public Option<TSuccess> Ok()
	{
		return this.successValue;
	}

	// Token: 0x06001648 RID: 5704 RVA: 0x0007838E File Offset: 0x0007658E
	public Option<TError> Err()
	{
		return this.errorValue;
	}

	// Token: 0x06001649 RID: 5705 RVA: 0x00078398 File Offset: 0x00076598
	public static implicit operator Result<TSuccess, TError>(Result.Internal.Value_Ok<TSuccess> value)
	{
		return new Result<TSuccess, TError>(value.value, default(TError));
	}

	// Token: 0x0600164A RID: 5706 RVA: 0x000783BC File Offset: 0x000765BC
	public static implicit operator Result<TSuccess, TError>(Result.Internal.Value_Err<TError> value)
	{
		return new Result<TSuccess, TError>(default(TSuccess), value.value);
	}

	// Token: 0x04000C84 RID: 3204
	private readonly Option<TSuccess> successValue;

	// Token: 0x04000C85 RID: 3205
	private readonly Option<TError> errorValue;
}
