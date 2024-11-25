using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02000E66 RID: 3686
	public class GameplaySeasons : ResourceSet<GameplaySeason>
	{
		// Token: 0x06007493 RID: 29843 RVA: 0x002D6131 File Offset: 0x002D4331
		public GameplaySeasons(ResourceSet parent) : base("GameplaySeasons", parent)
		{
			this.VanillaSeasons();
			this.Expansion1Seasons();
			this.DLCSeasons();
			this.UnusedSeasons();
		}

		// Token: 0x06007494 RID: 29844 RVA: 0x002D6158 File Offset: 0x002D4358
		private void VanillaSeasons()
		{
			this.MeteorShowers = base.Add(new MeteorShowerSeason("MeteorShowers", GameplaySeason.Type.World, "", 14f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, -1f).AddEvent(Db.Get().GameplayEvents.MeteorShowerIronEvent).AddEvent(Db.Get().GameplayEvents.MeteorShowerGoldEvent).AddEvent(Db.Get().GameplayEvents.MeteorShowerCopperEvent));
		}

		// Token: 0x06007495 RID: 29845 RVA: 0x002D61DC File Offset: 0x002D43DC
		private void Expansion1Seasons()
		{
			this.RegolithMoonMeteorShowers = base.Add(new MeteorShowerSeason("RegolithMoonMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.MeteorShowerDustEvent).AddEvent(Db.Get().GameplayEvents.ClusterIronShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower));
			this.TemporalTearMeteorShowers = base.Add(new MeteorShowerSeason("TemporalTearMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 1f, false, 0f, false, -1, 0f, float.PositiveInfinity, 1, false, -1f).AddEvent(Db.Get().GameplayEvents.MeteorShowerFullereneEvent));
			this.GassyMooteorShowers = base.Add(new MeteorShowerSeason("GassyMooteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, false, 6000f).AddEvent(Db.Get().GameplayEvents.GassyMooteorEvent));
			this.SpacedOutStyleStartMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleStartMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.SpacedOutStyleRocketMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleRocketMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower));
			this.SpacedOutStyleWarpMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleWarpMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower));
			this.ClassicStyleStartMeteorShowers = base.Add(new MeteorShowerSeason("ClassicStyleStartMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower));
			this.ClassicStyleWarpMeteorShowers = base.Add(new MeteorShowerSeason("ClassicStyleWarpMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower).AddEvent(Db.Get().GameplayEvents.ClusterIronShower));
			this.TundraMoonletMeteorShowers = base.Add(new MeteorShowerSeason("TundraMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.MarshyMoonletMeteorShowers = base.Add(new MeteorShowerSeason("MarshyMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.NiobiumMoonletMeteorShowers = base.Add(new MeteorShowerSeason("NiobiumMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.WaterMoonletMeteorShowers = base.Add(new MeteorShowerSeason("WaterMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.MiniMetallicSwampyMeteorShowers = base.Add(new MeteorShowerSeason("MiniMetallicSwampyMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower));
			this.MiniForestFrozenMeteorShowers = base.Add(new MeteorShowerSeason("MiniForestFrozenMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower));
			this.MiniBadlandsMeteorShowers = base.Add(new MeteorShowerSeason("MiniBadlandsMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterIceShower));
			this.MiniFlippedMeteorShowers = base.Add(new MeteorShowerSeason("MiniFlippedMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.MiniRadioactiveOceanMeteorShowers = base.Add(new MeteorShowerSeason("MiniRadioactiveOceanMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterUraniumShower));
		}

		// Token: 0x06007496 RID: 29846 RVA: 0x002D6740 File Offset: 0x002D4940
		private void DLCSeasons()
		{
			this.CeresMeteorShowers = base.Add(new MeteorShowerSeason("CeresMeteorShowers", GameplaySeason.Type.World, "DLC2_ID", 20f, false, -1f, true, -1, 10f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterIceAndTreesShower));
			this.MiniCeresStartShowers = base.Add(new MeteorShowerSeason("MiniCeresStartShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower).AddEvent(Db.Get().GameplayEvents.ClusterSnowShower));
		}

		// Token: 0x06007497 RID: 29847 RVA: 0x002D67FD File Offset: 0x002D49FD
		private void UnusedSeasons()
		{
		}

		// Token: 0x040053AF RID: 21423
		public GameplaySeason NaturalRandomEvents;

		// Token: 0x040053B0 RID: 21424
		public GameplaySeason DupeRandomEvents;

		// Token: 0x040053B1 RID: 21425
		public GameplaySeason PrickleCropSeason;

		// Token: 0x040053B2 RID: 21426
		public GameplaySeason BonusEvents;

		// Token: 0x040053B3 RID: 21427
		public GameplaySeason MeteorShowers;

		// Token: 0x040053B4 RID: 21428
		public GameplaySeason TemporalTearMeteorShowers;

		// Token: 0x040053B5 RID: 21429
		public GameplaySeason SpacedOutStyleStartMeteorShowers;

		// Token: 0x040053B6 RID: 21430
		public GameplaySeason SpacedOutStyleRocketMeteorShowers;

		// Token: 0x040053B7 RID: 21431
		public GameplaySeason SpacedOutStyleWarpMeteorShowers;

		// Token: 0x040053B8 RID: 21432
		public GameplaySeason ClassicStyleStartMeteorShowers;

		// Token: 0x040053B9 RID: 21433
		public GameplaySeason ClassicStyleWarpMeteorShowers;

		// Token: 0x040053BA RID: 21434
		public GameplaySeason TundraMoonletMeteorShowers;

		// Token: 0x040053BB RID: 21435
		public GameplaySeason MarshyMoonletMeteorShowers;

		// Token: 0x040053BC RID: 21436
		public GameplaySeason NiobiumMoonletMeteorShowers;

		// Token: 0x040053BD RID: 21437
		public GameplaySeason WaterMoonletMeteorShowers;

		// Token: 0x040053BE RID: 21438
		public GameplaySeason GassyMooteorShowers;

		// Token: 0x040053BF RID: 21439
		public GameplaySeason RegolithMoonMeteorShowers;

		// Token: 0x040053C0 RID: 21440
		public GameplaySeason MiniMetallicSwampyMeteorShowers;

		// Token: 0x040053C1 RID: 21441
		public GameplaySeason MiniForestFrozenMeteorShowers;

		// Token: 0x040053C2 RID: 21442
		public GameplaySeason MiniBadlandsMeteorShowers;

		// Token: 0x040053C3 RID: 21443
		public GameplaySeason MiniFlippedMeteorShowers;

		// Token: 0x040053C4 RID: 21444
		public GameplaySeason MiniRadioactiveOceanMeteorShowers;

		// Token: 0x040053C5 RID: 21445
		public GameplaySeason MiniCeresStartShowers;

		// Token: 0x040053C6 RID: 21446
		public GameplaySeason CeresMeteorShowers;
	}
}
