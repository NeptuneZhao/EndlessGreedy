using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E5C RID: 3676
	public class Deaths : ResourceSet<Death>
	{
		// Token: 0x06007471 RID: 29809 RVA: 0x002D2394 File Offset: 0x002D0594
		public Deaths(ResourceSet parent) : base("Deaths", parent)
		{
			this.Generic = new Death("Generic", this, DUPLICANTS.DEATHS.GENERIC.NAME, DUPLICANTS.DEATHS.GENERIC.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Frozen = new Death("Frozen", this, DUPLICANTS.DEATHS.FROZEN.NAME, DUPLICANTS.DEATHS.FROZEN.DESCRIPTION, "death_freeze_trans", "death_freeze_solid");
			this.Suffocation = new Death("Suffocation", this, DUPLICANTS.DEATHS.SUFFOCATION.NAME, DUPLICANTS.DEATHS.SUFFOCATION.DESCRIPTION, "death_suffocation", "dead_on_back");
			this.Starvation = new Death("Starvation", this, DUPLICANTS.DEATHS.STARVATION.NAME, DUPLICANTS.DEATHS.STARVATION.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Overheating = new Death("Overheating", this, DUPLICANTS.DEATHS.OVERHEATING.NAME, DUPLICANTS.DEATHS.OVERHEATING.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Drowned = new Death("Drowned", this, DUPLICANTS.DEATHS.DROWNED.NAME, DUPLICANTS.DEATHS.DROWNED.DESCRIPTION, "death_suffocation", "dead_on_back");
			this.Explosion = new Death("Explosion", this, DUPLICANTS.DEATHS.EXPLOSION.NAME, DUPLICANTS.DEATHS.EXPLOSION.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Slain = new Death("Combat", this, DUPLICANTS.DEATHS.COMBAT.NAME, DUPLICANTS.DEATHS.COMBAT.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.FatalDisease = new Death("FatalDisease", this, DUPLICANTS.DEATHS.FATALDISEASE.NAME, DUPLICANTS.DEATHS.FATALDISEASE.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Radiation = new Death("Radiation", this, DUPLICANTS.DEATHS.RADIATION.NAME, DUPLICANTS.DEATHS.RADIATION.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.HitByHighEnergyParticle = new Death("HitByHighEnergyParticle", this, DUPLICANTS.DEATHS.HITBYHIGHENERGYPARTICLE.NAME, DUPLICANTS.DEATHS.HITBYHIGHENERGYPARTICLE.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.DeadBattery = new Death("DeadBattery", this, DUPLICANTS.DEATHS.HITBYHIGHENERGYPARTICLE.NAME, DUPLICANTS.DEATHS.HITBYHIGHENERGYPARTICLE.DESCRIPTION, "dead_on_back", "dead_on_back");
		}

		// Token: 0x0400529A RID: 21146
		public Death Generic;

		// Token: 0x0400529B RID: 21147
		public Death Frozen;

		// Token: 0x0400529C RID: 21148
		public Death Suffocation;

		// Token: 0x0400529D RID: 21149
		public Death Starvation;

		// Token: 0x0400529E RID: 21150
		public Death Slain;

		// Token: 0x0400529F RID: 21151
		public Death Overheating;

		// Token: 0x040052A0 RID: 21152
		public Death Drowned;

		// Token: 0x040052A1 RID: 21153
		public Death Explosion;

		// Token: 0x040052A2 RID: 21154
		public Death FatalDisease;

		// Token: 0x040052A3 RID: 21155
		public Death Radiation;

		// Token: 0x040052A4 RID: 21156
		public Death HitByHighEnergyParticle;

		// Token: 0x040052A5 RID: 21157
		public Death DeadBattery;

		// Token: 0x040052A6 RID: 21158
		public Death DeadCyborgChargeExpired;
	}
}
