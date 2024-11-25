using System;
using FMODUnity;
using UnityEngine;

// Token: 0x02000AAF RID: 2735
[AddComponentMenu("KMonoBehaviour/scripts/Sounds")]
public class Sounds : KMonoBehaviour
{
	// Token: 0x170005D1 RID: 1489
	// (get) Token: 0x060050A3 RID: 20643 RVA: 0x001CFAD8 File Offset: 0x001CDCD8
	// (set) Token: 0x060050A4 RID: 20644 RVA: 0x001CFADF File Offset: 0x001CDCDF
	public static Sounds Instance { get; private set; }

	// Token: 0x060050A5 RID: 20645 RVA: 0x001CFAE7 File Offset: 0x001CDCE7
	public static void DestroyInstance()
	{
		Sounds.Instance = null;
	}

	// Token: 0x060050A6 RID: 20646 RVA: 0x001CFAEF File Offset: 0x001CDCEF
	protected override void OnPrefabInit()
	{
		Sounds.Instance = this;
	}

	// Token: 0x0400358A RID: 13706
	public FMODAsset BlowUp_Generic;

	// Token: 0x0400358B RID: 13707
	public FMODAsset Build_Generic;

	// Token: 0x0400358C RID: 13708
	public FMODAsset InUse_Fabricator;

	// Token: 0x0400358D RID: 13709
	public FMODAsset InUse_OxygenGenerator;

	// Token: 0x0400358E RID: 13710
	public FMODAsset Place_OreOnSite;

	// Token: 0x0400358F RID: 13711
	public FMODAsset Footstep_rock;

	// Token: 0x04003590 RID: 13712
	public FMODAsset Ice_crack;

	// Token: 0x04003591 RID: 13713
	public FMODAsset BuildingPowerOn;

	// Token: 0x04003592 RID: 13714
	public FMODAsset ElectricGridOverload;

	// Token: 0x04003593 RID: 13715
	public FMODAsset IngameMusic;

	// Token: 0x04003594 RID: 13716
	public FMODAsset[] OreSplashSounds;

	// Token: 0x04003596 RID: 13718
	public EventReference BlowUp_GenericMigrated;

	// Token: 0x04003597 RID: 13719
	public EventReference Build_GenericMigrated;

	// Token: 0x04003598 RID: 13720
	public EventReference InUse_FabricatorMigrated;

	// Token: 0x04003599 RID: 13721
	public EventReference InUse_OxygenGeneratorMigrated;

	// Token: 0x0400359A RID: 13722
	public EventReference Place_OreOnSiteMigrated;

	// Token: 0x0400359B RID: 13723
	public EventReference Footstep_rockMigrated;

	// Token: 0x0400359C RID: 13724
	public EventReference Ice_crackMigrated;

	// Token: 0x0400359D RID: 13725
	public EventReference BuildingPowerOnMigrated;

	// Token: 0x0400359E RID: 13726
	public EventReference ElectricGridOverloadMigrated;

	// Token: 0x0400359F RID: 13727
	public EventReference IngameMusicMigrated;

	// Token: 0x040035A0 RID: 13728
	public EventReference[] OreSplashSoundsMigrated;
}
