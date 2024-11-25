using System;
using KSerialization;

// Token: 0x020005CC RID: 1484
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class SubstanceSource : KMonoBehaviour
{
	// Token: 0x0600242C RID: 9260 RVA: 0x000C9EE6 File Offset: 0x000C80E6
	protected override void OnPrefabInit()
	{
		this.pickupable.SetWorkTime(SubstanceSource.MaxPickupTime);
	}

	// Token: 0x0600242D RID: 9261 RVA: 0x000C9EF8 File Offset: 0x000C80F8
	protected override void OnSpawn()
	{
		this.pickupable.SetWorkTime(10f);
	}

	// Token: 0x0600242E RID: 9262
	protected abstract CellOffset[] GetOffsetGroup();

	// Token: 0x0600242F RID: 9263
	protected abstract IChunkManager GetChunkManager();

	// Token: 0x06002430 RID: 9264 RVA: 0x000C9F0A File Offset: 0x000C810A
	public SimHashes GetElementID()
	{
		return this.primaryElement.ElementID;
	}

	// Token: 0x06002431 RID: 9265 RVA: 0x000C9F18 File Offset: 0x000C8118
	public Tag GetElementTag()
	{
		Tag result = Tag.Invalid;
		if (base.gameObject != null && this.primaryElement != null && this.primaryElement.Element != null)
		{
			result = this.primaryElement.Element.tag;
		}
		return result;
	}

	// Token: 0x06002432 RID: 9266 RVA: 0x000C9F68 File Offset: 0x000C8168
	public Tag GetMaterialCategoryTag()
	{
		Tag result = Tag.Invalid;
		if (base.gameObject != null && this.primaryElement != null && this.primaryElement.Element != null)
		{
			result = this.primaryElement.Element.GetMaterialCategoryTag();
		}
		return result;
	}

	// Token: 0x040014A0 RID: 5280
	private bool enableRefresh;

	// Token: 0x040014A1 RID: 5281
	private static readonly float MaxPickupTime = 8f;

	// Token: 0x040014A2 RID: 5282
	[MyCmpReq]
	public Pickupable pickupable;

	// Token: 0x040014A3 RID: 5283
	[MyCmpReq]
	private PrimaryElement primaryElement;
}
