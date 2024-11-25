using System;

namespace Database
{
	// Token: 0x02000E6A RID: 3690
	public class OrbitalTypeCategories : ResourceSet<OrbitalData>
	{
		// Token: 0x060074A8 RID: 29864 RVA: 0x002D7760 File Offset: 0x002D5960
		public OrbitalTypeCategories(ResourceSet parent) : base("OrbitalTypeCategories", parent)
		{
			this.backgroundEarth = new OrbitalData("backgroundEarth", this, "earth_kanim", "", OrbitalData.OrbitalType.world, 1f, 0.5f, 0.95f, 10f, 10f, 1.05f, true, 0.05f, 25f, 1f);
			this.backgroundEarth.GetRenderZ = (() => Grid.GetLayerZ(Grid.SceneLayer.Background) + 0.9f);
			this.frozenOre = new OrbitalData("frozenOre", this, "starmap_frozen_ore_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1f, true, 0.05f, 25f, 1f);
			this.heliumCloud = new OrbitalData("heliumCloud", this, "starmap_helium_cloud_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.iceCloud = new OrbitalData("iceCloud", this, "starmap_ice_cloud_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.iceRock = new OrbitalData("iceRock", this, "starmap_ice_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.purpleGas = new OrbitalData("purpleGas", this, "starmap_purple_gas_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.radioactiveGas = new OrbitalData("radioactiveGas", this, "starmap_radioactive_gas_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.rocky = new OrbitalData("rocky", this, "starmap_rocky_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.gravitas = new OrbitalData("gravitas", this, "starmap_space_junk_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.orbit = new OrbitalData("orbit", this, "starmap_orbit_kanim", "", OrbitalData.OrbitalType.inOrbit, 1f, 0.25f, 0.5f, -350f, 350f, 1.05f, false, 0.05f, 4f, 1f);
			this.landed = new OrbitalData("landed", this, "starmap_landed_surface_kanim", "", OrbitalData.OrbitalType.landed, 0f, 0.5f, 0.35f, -350f, 350f, 1.05f, false, 0.05f, 4f, 1f);
		}

		// Token: 0x04005403 RID: 21507
		public OrbitalData backgroundEarth;

		// Token: 0x04005404 RID: 21508
		public OrbitalData frozenOre;

		// Token: 0x04005405 RID: 21509
		public OrbitalData heliumCloud;

		// Token: 0x04005406 RID: 21510
		public OrbitalData iceCloud;

		// Token: 0x04005407 RID: 21511
		public OrbitalData iceRock;

		// Token: 0x04005408 RID: 21512
		public OrbitalData purpleGas;

		// Token: 0x04005409 RID: 21513
		public OrbitalData radioactiveGas;

		// Token: 0x0400540A RID: 21514
		public OrbitalData rocky;

		// Token: 0x0400540B RID: 21515
		public OrbitalData gravitas;

		// Token: 0x0400540C RID: 21516
		public OrbitalData orbit;

		// Token: 0x0400540D RID: 21517
		public OrbitalData landed;
	}
}
