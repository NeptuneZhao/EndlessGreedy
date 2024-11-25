using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Database
{
	// Token: 0x02000E7E RID: 3710
	[DebuggerDisplay("{Id}")]
	public class SpaceDestinationType : Resource
	{
		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x060074DE RID: 29918 RVA: 0x002DAD2C File Offset: 0x002D8F2C
		// (set) Token: 0x060074DF RID: 29919 RVA: 0x002DAD34 File Offset: 0x002D8F34
		public int maxiumMass { get; private set; }

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x060074E0 RID: 29920 RVA: 0x002DAD3D File Offset: 0x002D8F3D
		// (set) Token: 0x060074E1 RID: 29921 RVA: 0x002DAD45 File Offset: 0x002D8F45
		public int minimumMass { get; private set; }

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x060074E2 RID: 29922 RVA: 0x002DAD4E File Offset: 0x002D8F4E
		public float replishmentPerCycle
		{
			get
			{
				return 1000f / (float)this.cyclesToRecover;
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x060074E3 RID: 29923 RVA: 0x002DAD5D File Offset: 0x002D8F5D
		public float replishmentPerSim1000ms
		{
			get
			{
				return 1000f / ((float)this.cyclesToRecover * 600f);
			}
		}

		// Token: 0x060074E4 RID: 29924 RVA: 0x002DAD74 File Offset: 0x002D8F74
		public SpaceDestinationType(string id, ResourceSet parent, string name, string description, int iconSize, string spriteName, Dictionary<SimHashes, MathUtil.MinMax> elementTable, Dictionary<string, int> recoverableEntities = null, ArtifactDropRate artifactDropRate = null, int max = 64000000, int min = 63994000, int cycles = 6, bool visitable = true) : base(id, parent, name)
		{
			this.typeName = name;
			this.description = description;
			this.iconSize = iconSize;
			this.spriteName = spriteName;
			this.elementTable = elementTable;
			this.recoverableEntities = recoverableEntities;
			this.artifactDropTable = artifactDropRate;
			this.maxiumMass = max;
			this.minimumMass = min;
			this.cyclesToRecover = cycles;
			this.visitable = visitable;
		}

		// Token: 0x0400549E RID: 21662
		public const float MASS_TO_RECOVER = 1000f;

		// Token: 0x0400549F RID: 21663
		public string typeName;

		// Token: 0x040054A0 RID: 21664
		public string description;

		// Token: 0x040054A1 RID: 21665
		public int iconSize = 128;

		// Token: 0x040054A2 RID: 21666
		public string spriteName;

		// Token: 0x040054A3 RID: 21667
		public Dictionary<SimHashes, MathUtil.MinMax> elementTable;

		// Token: 0x040054A4 RID: 21668
		public Dictionary<string, int> recoverableEntities;

		// Token: 0x040054A5 RID: 21669
		public ArtifactDropRate artifactDropTable;

		// Token: 0x040054A6 RID: 21670
		public bool visitable;

		// Token: 0x040054A9 RID: 21673
		public int cyclesToRecover;
	}
}
