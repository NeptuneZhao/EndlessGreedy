using System;
using System.Collections.Generic;

// Token: 0x020005FC RID: 1532
public class AccessorySlot : Resource
{
	// Token: 0x170001BA RID: 442
	// (get) Token: 0x06002597 RID: 9623 RVA: 0x000D1C26 File Offset: 0x000CFE26
	// (set) Token: 0x06002598 RID: 9624 RVA: 0x000D1C2E File Offset: 0x000CFE2E
	public KAnimHashedString targetSymbolId { get; private set; }

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x06002599 RID: 9625 RVA: 0x000D1C37 File Offset: 0x000CFE37
	// (set) Token: 0x0600259A RID: 9626 RVA: 0x000D1C3F File Offset: 0x000CFE3F
	public List<Accessory> accessories { get; private set; }

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x0600259B RID: 9627 RVA: 0x000D1C48 File Offset: 0x000CFE48
	public KAnimFile AnimFile
	{
		get
		{
			return this.file;
		}
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x0600259C RID: 9628 RVA: 0x000D1C50 File Offset: 0x000CFE50
	// (set) Token: 0x0600259D RID: 9629 RVA: 0x000D1C58 File Offset: 0x000CFE58
	public KAnimFile defaultAnimFile { get; private set; }

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x0600259E RID: 9630 RVA: 0x000D1C61 File Offset: 0x000CFE61
	// (set) Token: 0x0600259F RID: 9631 RVA: 0x000D1C69 File Offset: 0x000CFE69
	public int overrideLayer { get; private set; }

	// Token: 0x060025A0 RID: 9632 RVA: 0x000D1C74 File Offset: 0x000CFE74
	public AccessorySlot(string id, ResourceSet parent, KAnimFile swap_build, int overrideLayer = 0) : base(id, parent, null)
	{
		if (swap_build == null)
		{
			Debug.LogErrorFormat("AccessorySlot {0} missing swap_build", new object[]
			{
				id
			});
		}
		this.targetSymbolId = new KAnimHashedString("snapTo_" + id.ToLower());
		this.accessories = new List<Accessory>();
		this.file = swap_build;
		this.overrideLayer = overrideLayer;
		this.defaultAnimFile = swap_build;
	}

	// Token: 0x060025A1 RID: 9633 RVA: 0x000D1CE4 File Offset: 0x000CFEE4
	public AccessorySlot(string id, ResourceSet parent, KAnimHashedString target_symbol_id, KAnimFile swap_build, KAnimFile defaultAnimFile = null, int overrideLayer = 0) : base(id, parent, null)
	{
		if (swap_build == null)
		{
			Debug.LogErrorFormat("AccessorySlot {0} missing swap_build", new object[]
			{
				id
			});
		}
		this.targetSymbolId = target_symbol_id;
		this.accessories = new List<Accessory>();
		this.file = swap_build;
		this.defaultAnimFile = ((defaultAnimFile != null) ? defaultAnimFile : swap_build);
		this.overrideLayer = overrideLayer;
	}

	// Token: 0x060025A2 RID: 9634 RVA: 0x000D1D50 File Offset: 0x000CFF50
	public void AddAccessories(KAnimFile default_build, ResourceSet parent)
	{
		KAnim.Build build = default_build.GetData().build;
		default_build.GetData().build.GetSymbol(this.targetSymbolId);
		string value = this.Id.ToLower();
		for (int i = 0; i < build.symbols.Length; i++)
		{
			string text = HashCache.Get().Get(build.symbols[i].hash);
			if (text.StartsWith(value))
			{
				Accessory accessory = new Accessory(text, parent, this, this.file.batchTag, build.symbols[i], default_build, null);
				this.accessories.Add(accessory);
				HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
			}
		}
	}

	// Token: 0x060025A3 RID: 9635 RVA: 0x000D1E09 File Offset: 0x000D0009
	public Accessory Lookup(string id)
	{
		return this.Lookup(new HashedString(id));
	}

	// Token: 0x060025A4 RID: 9636 RVA: 0x000D1E18 File Offset: 0x000D0018
	public Accessory Lookup(HashedString full_id)
	{
		if (!full_id.IsValid)
		{
			return null;
		}
		return this.accessories.Find((Accessory a) => a.IdHash == full_id);
	}

	// Token: 0x0400156D RID: 5485
	private KAnimFile file;
}
