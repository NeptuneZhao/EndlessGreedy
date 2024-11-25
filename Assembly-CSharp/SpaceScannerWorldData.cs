using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;

// Token: 0x02000B13 RID: 2835
[Serialize]
[SerializationConfig(MemberSerialization.OptIn)]
[Serializable]
public class SpaceScannerWorldData
{
	// Token: 0x06005471 RID: 21617 RVA: 0x001E2DDA File Offset: 0x001E0FDA
	[Serialize]
	public SpaceScannerWorldData(int worldId)
	{
		this.worldId = worldId;
	}

	// Token: 0x06005472 RID: 21618 RVA: 0x001E2E0A File Offset: 0x001E100A
	public WorldContainer GetWorld()
	{
		if (this.world == null)
		{
			this.world = ClusterManager.Instance.GetWorld(this.worldId);
		}
		return this.world;
	}

	// Token: 0x0400375B RID: 14171
	[NonSerialized]
	private WorldContainer world;

	// Token: 0x0400375C RID: 14172
	[Serialize]
	public int worldId;

	// Token: 0x0400375D RID: 14173
	[Serialize]
	public float networkQuality01;

	// Token: 0x0400375E RID: 14174
	[Serialize]
	public Dictionary<string, float> targetIdToRandomValue01Map = new Dictionary<string, float>();

	// Token: 0x0400375F RID: 14175
	[Serialize]
	public HashSet<string> targetIdsDetected = new HashSet<string>();

	// Token: 0x04003760 RID: 14176
	[NonSerialized]
	public SpaceScannerWorldData.Scratchpad scratchpad = new SpaceScannerWorldData.Scratchpad();

	// Token: 0x02001B5D RID: 7005
	public class Scratchpad
	{
		// Token: 0x04007F83 RID: 32643
		public List<ClusterTraveler> ballisticObjects = new List<ClusterTraveler>();

		// Token: 0x04007F84 RID: 32644
		public HashSet<MeteorShowerEvent.StatesInstance> lastDetectedMeteorShowers = new HashSet<MeteorShowerEvent.StatesInstance>();

		// Token: 0x04007F85 RID: 32645
		public HashSet<LaunchConditionManager> lastDetectedRocketsBaseGame = new HashSet<LaunchConditionManager>();

		// Token: 0x04007F86 RID: 32646
		public HashSet<Clustercraft> lastDetectedRocketsDLC1 = new HashSet<Clustercraft>();
	}
}
