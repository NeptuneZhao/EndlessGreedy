using System;
using KSerialization;

// Token: 0x02000A49 RID: 2633
public class RepairableEquipment : KMonoBehaviour
{
	// Token: 0x1700056E RID: 1390
	// (get) Token: 0x06004C4E RID: 19534 RVA: 0x001B3FCE File Offset: 0x001B21CE
	// (set) Token: 0x06004C4F RID: 19535 RVA: 0x001B3FDB File Offset: 0x001B21DB
	public EquipmentDef def
	{
		get
		{
			return this.defHandle.Get<EquipmentDef>();
		}
		set
		{
			this.defHandle.Set<EquipmentDef>(value);
		}
	}

	// Token: 0x06004C50 RID: 19536 RVA: 0x001B3FEC File Offset: 0x001B21EC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.def.AdditionalTags != null)
		{
			foreach (Tag tag in this.def.AdditionalTags)
			{
				base.GetComponent<KPrefabID>().AddTag(tag, false);
			}
		}
	}

	// Token: 0x06004C51 RID: 19537 RVA: 0x001B403C File Offset: 0x001B223C
	protected override void OnSpawn()
	{
		if (!this.facadeID.IsNullOrWhiteSpace())
		{
			KAnim.Build.Symbol symbol = Db.GetEquippableFacades().Get(this.facadeID).AnimFile.GetData().build.GetSymbol("object");
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			component.TryRemoveSymbolOverride("object", 0);
			component.AddSymbolOverride("object", symbol, 0);
		}
	}

	// Token: 0x040032BE RID: 12990
	public DefHandle defHandle;

	// Token: 0x040032BF RID: 12991
	[Serialize]
	public string facadeID;
}
