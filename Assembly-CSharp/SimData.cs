using System;

// Token: 0x02000A99 RID: 2713
public class SimData
{
	// Token: 0x040034DD RID: 13533
	public unsafe Sim.EmittedMassInfo* emittedMassEntries;

	// Token: 0x040034DE RID: 13534
	public unsafe Sim.ElementChunkInfo* elementChunks;

	// Token: 0x040034DF RID: 13535
	public unsafe Sim.BuildingTemperatureInfo* buildingTemperatures;

	// Token: 0x040034E0 RID: 13536
	public unsafe Sim.DiseaseEmittedInfo* diseaseEmittedInfos;

	// Token: 0x040034E1 RID: 13537
	public unsafe Sim.DiseaseConsumedInfo* diseaseConsumedInfos;
}
