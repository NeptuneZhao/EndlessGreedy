using System;
using KSerialization;

// Token: 0x02000895 RID: 2197
public class EquippableFacade : KMonoBehaviour
{
	// Token: 0x06003D91 RID: 15761 RVA: 0x0015484C File Offset: 0x00152A4C
	public static void AddFacadeToEquippable(Equippable equippable, string facadeID)
	{
		EquippableFacade equippableFacade = equippable.gameObject.AddOrGet<EquippableFacade>();
		equippableFacade.FacadeID = facadeID;
		equippableFacade.BuildOverride = Db.GetEquippableFacades().Get(facadeID).BuildOverride;
		equippableFacade.ApplyAnimOverride();
	}

	// Token: 0x06003D92 RID: 15762 RVA: 0x0015487B File Offset: 0x00152A7B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OverrideName();
		this.ApplyAnimOverride();
	}

	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x06003D93 RID: 15763 RVA: 0x0015488F File Offset: 0x00152A8F
	// (set) Token: 0x06003D94 RID: 15764 RVA: 0x00154897 File Offset: 0x00152A97
	public string FacadeID
	{
		get
		{
			return this._facadeID;
		}
		private set
		{
			this._facadeID = value;
			this.OverrideName();
		}
	}

	// Token: 0x06003D95 RID: 15765 RVA: 0x001548A6 File Offset: 0x00152AA6
	public void ApplyAnimOverride()
	{
		if (this.FacadeID.IsNullOrWhiteSpace())
		{
			return;
		}
		base.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[]
		{
			Db.GetEquippableFacades().Get(this.FacadeID).AnimFile
		});
	}

	// Token: 0x06003D96 RID: 15766 RVA: 0x001548DF File Offset: 0x00152ADF
	private void OverrideName()
	{
		base.GetComponent<KSelectable>().SetName(EquippableFacade.GetNameOverride(base.GetComponent<Equippable>().def.Id, this.FacadeID));
	}

	// Token: 0x06003D97 RID: 15767 RVA: 0x00154907 File Offset: 0x00152B07
	public static string GetNameOverride(string defID, string facadeID)
	{
		if (facadeID.IsNullOrWhiteSpace())
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + defID.ToUpper() + ".NAME");
		}
		return Db.GetEquippableFacades().Get(facadeID).Name;
	}

	// Token: 0x04002596 RID: 9622
	[Serialize]
	private string _facadeID;

	// Token: 0x04002597 RID: 9623
	[Serialize]
	public string BuildOverride;
}
