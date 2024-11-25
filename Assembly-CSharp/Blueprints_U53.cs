using System;
using Database;

// Token: 0x02000522 RID: 1314
public class Blueprints_U53 : BlueprintProvider
{
	// Token: 0x06001D6E RID: 7534 RVA: 0x000A3150 File Offset: 0x000A1350
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001D6F RID: 7535 RVA: 0x000A3157 File Offset: 0x000A1357
	public override void SetupBlueprints()
	{
		base.AddBuilding("LuxuryBed", PermitRarity.Loyalty, "permit_elegantbed_hatch", "elegantbed_hatch_kanim");
		base.AddBuilding("LuxuryBed", PermitRarity.Loyalty, "permit_elegantbed_pipsqueak", "elegantbed_pipsqueak_kanim");
	}
}
