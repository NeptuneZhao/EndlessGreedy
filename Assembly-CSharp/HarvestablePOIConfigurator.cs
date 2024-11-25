using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AC7 RID: 2759
[AddComponentMenu("KMonoBehaviour/scripts/HarvestablePOIConfigurator")]
public class HarvestablePOIConfigurator : KMonoBehaviour
{
	// Token: 0x060051FD RID: 20989 RVA: 0x001D69AC File Offset: 0x001D4BAC
	public static HarvestablePOIConfigurator.HarvestablePOIType FindType(HashedString typeId)
	{
		HarvestablePOIConfigurator.HarvestablePOIType harvestablePOIType = null;
		if (typeId != HashedString.Invalid)
		{
			harvestablePOIType = HarvestablePOIConfigurator._poiTypes.Find((HarvestablePOIConfigurator.HarvestablePOIType t) => t.id == typeId);
		}
		if (harvestablePOIType == null)
		{
			global::Debug.LogError(string.Format("Tried finding a harvestable poi with id {0} but it doesn't exist!", typeId.ToString()));
		}
		return harvestablePOIType;
	}

	// Token: 0x060051FE RID: 20990 RVA: 0x001D6A15 File Offset: 0x001D4C15
	public HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration MakeConfiguration()
	{
		return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
	}

	// Token: 0x060051FF RID: 20991 RVA: 0x001D6A30 File Offset: 0x001D4C30
	private HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration CreateRandomInstance(HashedString typeId, float min, float max)
	{
		int globalWorldSeed = SaveLoader.Instance.clusterDetailSave.globalWorldSeed;
		ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
		Vector3 position = ClusterGrid.Instance.GetPosition(component);
		KRandom randomSource = new KRandom(globalWorldSeed + (int)position.x + (int)position.y);
		return new HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration
		{
			typeId = typeId,
			capacityRoll = this.Roll(randomSource, min, max),
			rechargeRoll = this.Roll(randomSource, min, max)
		};
	}

	// Token: 0x06005200 RID: 20992 RVA: 0x001D6A9F File Offset: 0x001D4C9F
	private float Roll(KRandom randomSource, float min, float max)
	{
		return (float)(randomSource.NextDouble() * (double)(max - min)) + min;
	}

	// Token: 0x0400361F RID: 13855
	private static List<HarvestablePOIConfigurator.HarvestablePOIType> _poiTypes;

	// Token: 0x04003620 RID: 13856
	public HashedString presetType;

	// Token: 0x04003621 RID: 13857
	public float presetMin;

	// Token: 0x04003622 RID: 13858
	public float presetMax = 1f;

	// Token: 0x02001B0B RID: 6923
	public class HarvestablePOIType
	{
		// Token: 0x0600A219 RID: 41497 RVA: 0x00384BB8 File Offset: 0x00382DB8
		public HarvestablePOIType(string id, Dictionary<SimHashes, float> harvestableElements, float poiCapacityMin = 54000f, float poiCapacityMax = 81000f, float poiRechargeMin = 30000f, float poiRechargeMax = 60000f, bool canProvideArtifacts = true, List<string> orbitalObject = null, int maxNumOrbitingObjects = 20, string dlcID = "EXPANSION1_ID")
		{
			this.id = id;
			this.idHash = id;
			this.harvestableElements = harvestableElements;
			this.poiCapacityMin = poiCapacityMin;
			this.poiCapacityMax = poiCapacityMax;
			this.poiRechargeMin = poiRechargeMin;
			this.poiRechargeMax = poiRechargeMax;
			this.canProvideArtifacts = canProvideArtifacts;
			this.orbitalObject = orbitalObject;
			this.maxNumOrbitingObjects = maxNumOrbitingObjects;
			this.dlcID = dlcID;
			if (HarvestablePOIConfigurator._poiTypes == null)
			{
				HarvestablePOIConfigurator._poiTypes = new List<HarvestablePOIConfigurator.HarvestablePOIType>();
			}
			HarvestablePOIConfigurator._poiTypes.Add(this);
		}

		// Token: 0x04007E8E RID: 32398
		public string id;

		// Token: 0x04007E8F RID: 32399
		public HashedString idHash;

		// Token: 0x04007E90 RID: 32400
		public Dictionary<SimHashes, float> harvestableElements;

		// Token: 0x04007E91 RID: 32401
		public float poiCapacityMin;

		// Token: 0x04007E92 RID: 32402
		public float poiCapacityMax;

		// Token: 0x04007E93 RID: 32403
		public float poiRechargeMin;

		// Token: 0x04007E94 RID: 32404
		public float poiRechargeMax;

		// Token: 0x04007E95 RID: 32405
		public bool canProvideArtifacts;

		// Token: 0x04007E96 RID: 32406
		public string dlcID;

		// Token: 0x04007E97 RID: 32407
		public List<string> orbitalObject;

		// Token: 0x04007E98 RID: 32408
		public int maxNumOrbitingObjects;
	}

	// Token: 0x02001B0C RID: 6924
	[Serializable]
	public class HarvestablePOIInstanceConfiguration
	{
		// Token: 0x0600A21A RID: 41498 RVA: 0x00384C40 File Offset: 0x00382E40
		private void Init()
		{
			if (this.didInit)
			{
				return;
			}
			this.didInit = true;
			this.poiTotalCapacity = MathUtil.ReRange(this.capacityRoll, 0f, 1f, this.poiType.poiCapacityMin, this.poiType.poiCapacityMax);
			this.poiRecharge = MathUtil.ReRange(this.rechargeRoll, 0f, 1f, this.poiType.poiRechargeMin, this.poiType.poiRechargeMax);
		}

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x0600A21B RID: 41499 RVA: 0x00384CBF File Offset: 0x00382EBF
		public HarvestablePOIConfigurator.HarvestablePOIType poiType
		{
			get
			{
				return HarvestablePOIConfigurator.FindType(this.typeId);
			}
		}

		// Token: 0x0600A21C RID: 41500 RVA: 0x00384CCC File Offset: 0x00382ECC
		public Dictionary<SimHashes, float> GetElementsWithWeights()
		{
			this.Init();
			return this.poiType.harvestableElements;
		}

		// Token: 0x0600A21D RID: 41501 RVA: 0x00384CDF File Offset: 0x00382EDF
		public bool CanProvideArtifacts()
		{
			this.Init();
			return this.poiType.canProvideArtifacts;
		}

		// Token: 0x0600A21E RID: 41502 RVA: 0x00384CF2 File Offset: 0x00382EF2
		public float GetMaxCapacity()
		{
			this.Init();
			return this.poiTotalCapacity;
		}

		// Token: 0x0600A21F RID: 41503 RVA: 0x00384D00 File Offset: 0x00382F00
		public float GetRechargeTime()
		{
			this.Init();
			return this.poiRecharge;
		}

		// Token: 0x04007E99 RID: 32409
		public HashedString typeId;

		// Token: 0x04007E9A RID: 32410
		private bool didInit;

		// Token: 0x04007E9B RID: 32411
		public float capacityRoll;

		// Token: 0x04007E9C RID: 32412
		public float rechargeRoll;

		// Token: 0x04007E9D RID: 32413
		private float poiTotalCapacity;

		// Token: 0x04007E9E RID: 32414
		private float poiRecharge;
	}
}
