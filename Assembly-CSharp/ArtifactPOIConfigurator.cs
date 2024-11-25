using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AB2 RID: 2738
[AddComponentMenu("KMonoBehaviour/scripts/ArtifactPOIConfigurator")]
public class ArtifactPOIConfigurator : KMonoBehaviour
{
	// Token: 0x060050B1 RID: 20657 RVA: 0x001CFCFC File Offset: 0x001CDEFC
	public static ArtifactPOIConfigurator.ArtifactPOIType FindType(HashedString typeId)
	{
		ArtifactPOIConfigurator.ArtifactPOIType artifactPOIType = null;
		if (typeId != HashedString.Invalid)
		{
			artifactPOIType = ArtifactPOIConfigurator._poiTypes.Find((ArtifactPOIConfigurator.ArtifactPOIType t) => t.id == typeId);
		}
		if (artifactPOIType == null)
		{
			global::Debug.LogError(string.Format("Tried finding a harvestable poi with id {0} but it doesn't exist!", typeId.ToString()));
		}
		return artifactPOIType;
	}

	// Token: 0x060050B2 RID: 20658 RVA: 0x001CFD65 File Offset: 0x001CDF65
	public ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration MakeConfiguration()
	{
		return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
	}

	// Token: 0x060050B3 RID: 20659 RVA: 0x001CFD80 File Offset: 0x001CDF80
	private ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration CreateRandomInstance(HashedString typeId, float min, float max)
	{
		int globalWorldSeed = SaveLoader.Instance.clusterDetailSave.globalWorldSeed;
		ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
		Vector3 position = ClusterGrid.Instance.GetPosition(component);
		KRandom randomSource = new KRandom(globalWorldSeed + (int)position.x + (int)position.y);
		return new ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration
		{
			typeId = typeId,
			rechargeRoll = this.Roll(randomSource, min, max)
		};
	}

	// Token: 0x060050B4 RID: 20660 RVA: 0x001CFDE0 File Offset: 0x001CDFE0
	private float Roll(KRandom randomSource, float min, float max)
	{
		return (float)(randomSource.NextDouble() * (double)(max - min)) + min;
	}

	// Token: 0x040035A6 RID: 13734
	private static List<ArtifactPOIConfigurator.ArtifactPOIType> _poiTypes;

	// Token: 0x040035A7 RID: 13735
	public static ArtifactPOIConfigurator.ArtifactPOIType defaultArtifactPoiType = new ArtifactPOIConfigurator.ArtifactPOIType("HarvestablePOIArtifacts", null, false, 30000f, 60000f, "EXPANSION1_ID");

	// Token: 0x040035A8 RID: 13736
	public HashedString presetType;

	// Token: 0x040035A9 RID: 13737
	public float presetMin;

	// Token: 0x040035AA RID: 13738
	public float presetMax = 1f;

	// Token: 0x02001AED RID: 6893
	public class ArtifactPOIType
	{
		// Token: 0x0600A196 RID: 41366 RVA: 0x00383980 File Offset: 0x00381B80
		public ArtifactPOIType(string id, string harvestableArtifactID = null, bool destroyOnHarvest = false, float poiRechargeTimeMin = 30000f, float poiRechargeTimeMax = 60000f, string dlcID = "EXPANSION1_ID")
		{
			this.id = id;
			this.idHash = id;
			this.harvestableArtifactID = harvestableArtifactID;
			this.destroyOnHarvest = destroyOnHarvest;
			this.poiRechargeTimeMin = poiRechargeTimeMin;
			this.poiRechargeTimeMax = poiRechargeTimeMax;
			this.dlcID = dlcID;
			if (ArtifactPOIConfigurator._poiTypes == null)
			{
				ArtifactPOIConfigurator._poiTypes = new List<ArtifactPOIConfigurator.ArtifactPOIType>();
			}
			ArtifactPOIConfigurator._poiTypes.Add(this);
		}

		// Token: 0x04007E20 RID: 32288
		public string id;

		// Token: 0x04007E21 RID: 32289
		public HashedString idHash;

		// Token: 0x04007E22 RID: 32290
		public string harvestableArtifactID;

		// Token: 0x04007E23 RID: 32291
		public bool destroyOnHarvest;

		// Token: 0x04007E24 RID: 32292
		public float poiRechargeTimeMin;

		// Token: 0x04007E25 RID: 32293
		public float poiRechargeTimeMax;

		// Token: 0x04007E26 RID: 32294
		public string dlcID;

		// Token: 0x04007E27 RID: 32295
		public List<string> orbitalObject = new List<string>
		{
			Db.Get().OrbitalTypeCategories.gravitas.Id
		};
	}

	// Token: 0x02001AEE RID: 6894
	[Serializable]
	public class ArtifactPOIInstanceConfiguration
	{
		// Token: 0x0600A197 RID: 41367 RVA: 0x00383A10 File Offset: 0x00381C10
		private void Init()
		{
			if (this.didInit)
			{
				return;
			}
			this.didInit = true;
			this.poiRechargeTime = MathUtil.ReRange(this.rechargeRoll, 0f, 1f, this.poiType.poiRechargeTimeMin, this.poiType.poiRechargeTimeMax);
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x0600A198 RID: 41368 RVA: 0x00383A5E File Offset: 0x00381C5E
		public ArtifactPOIConfigurator.ArtifactPOIType poiType
		{
			get
			{
				return ArtifactPOIConfigurator.FindType(this.typeId);
			}
		}

		// Token: 0x0600A199 RID: 41369 RVA: 0x00383A6B File Offset: 0x00381C6B
		public bool DestroyOnHarvest()
		{
			this.Init();
			return this.poiType.destroyOnHarvest;
		}

		// Token: 0x0600A19A RID: 41370 RVA: 0x00383A7E File Offset: 0x00381C7E
		public string GetArtifactID()
		{
			this.Init();
			return this.poiType.harvestableArtifactID;
		}

		// Token: 0x0600A19B RID: 41371 RVA: 0x00383A91 File Offset: 0x00381C91
		public float GetRechargeTime()
		{
			this.Init();
			return this.poiRechargeTime;
		}

		// Token: 0x04007E28 RID: 32296
		public HashedString typeId;

		// Token: 0x04007E29 RID: 32297
		private bool didInit;

		// Token: 0x04007E2A RID: 32298
		public float rechargeRoll;

		// Token: 0x04007E2B RID: 32299
		private float poiRechargeTime;
	}
}
