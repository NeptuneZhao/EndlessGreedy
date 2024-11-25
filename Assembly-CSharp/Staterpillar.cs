using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007EC RID: 2028
public class Staterpillar : KMonoBehaviour
{
	// Token: 0x06003815 RID: 14357 RVA: 0x00132980 File Offset: 0x00130B80
	protected override void OnPrefabInit()
	{
		this.dummyElement = new List<Tag>
		{
			SimHashes.Unobtanium.CreateTag()
		};
		this.connectorDef = Assets.GetBuildingDef(this.connectorDefId);
	}

	// Token: 0x06003816 RID: 14358 RVA: 0x001329AE File Offset: 0x00130BAE
	public void SpawnConnectorBuilding(int targetCell)
	{
		if (this.conduitLayer == ObjectLayer.Wire)
		{
			this.SpawnGenerator(targetCell);
			return;
		}
		this.SpawnConduitConnector(targetCell);
	}

	// Token: 0x06003817 RID: 14359 RVA: 0x001329CC File Offset: 0x00130BCC
	public void DestroyOrphanedConnectorBuilding()
	{
		KPrefabID building = this.GetConnectorBuilding();
		if (building != null)
		{
			this.connectorRef.Set(null);
			this.cachedGenerator = null;
			this.cachedConduitDispenser = null;
			GameScheduler.Instance.ScheduleNextFrame("Destroy Staterpillar Connector building", delegate(object o)
			{
				if (building != null)
				{
					Util.KDestroyGameObject(building.gameObject);
				}
			}, null, null);
		}
	}

	// Token: 0x06003818 RID: 14360 RVA: 0x00132A31 File Offset: 0x00130C31
	public void EnableConnector()
	{
		if (this.conduitLayer == ObjectLayer.Wire)
		{
			this.EnableGenerator();
			return;
		}
		this.EnableConduitConnector();
	}

	// Token: 0x06003819 RID: 14361 RVA: 0x00132A4A File Offset: 0x00130C4A
	public bool IsConnectorBuildingSpawned()
	{
		return this.GetConnectorBuilding() != null;
	}

	// Token: 0x0600381A RID: 14362 RVA: 0x00132A58 File Offset: 0x00130C58
	public bool IsConnected()
	{
		if (this.conduitLayer == ObjectLayer.Wire)
		{
			return this.GetGenerator().CircuitID != ushort.MaxValue;
		}
		return this.GetConduitDispenser().IsConnected;
	}

	// Token: 0x0600381B RID: 14363 RVA: 0x00132A85 File Offset: 0x00130C85
	public KPrefabID GetConnectorBuilding()
	{
		return this.connectorRef.Get();
	}

	// Token: 0x0600381C RID: 14364 RVA: 0x00132A94 File Offset: 0x00130C94
	private void SpawnConduitConnector(int targetCell)
	{
		if (this.GetConduitDispenser() == null)
		{
			GameObject gameObject = this.connectorDef.Build(targetCell, Orientation.R180, null, this.dummyElement, base.gameObject.GetComponent<PrimaryElement>().Temperature, true, -1f);
			this.connectorRef = new Ref<KPrefabID>(gameObject.GetComponent<KPrefabID>());
			gameObject.SetActive(true);
			gameObject.GetComponent<BuildingCellVisualizer>().enabled = false;
		}
	}

	// Token: 0x0600381D RID: 14365 RVA: 0x00132AFE File Offset: 0x00130CFE
	private void EnableConduitConnector()
	{
		ConduitDispenser conduitDispenser = this.GetConduitDispenser();
		conduitDispenser.GetComponent<BuildingCellVisualizer>().enabled = true;
		conduitDispenser.storage = base.GetComponent<Storage>();
		conduitDispenser.SetOnState(true);
	}

	// Token: 0x0600381E RID: 14366 RVA: 0x00132B24 File Offset: 0x00130D24
	public ConduitDispenser GetConduitDispenser()
	{
		if (this.cachedConduitDispenser == null)
		{
			KPrefabID kprefabID = this.connectorRef.Get();
			if (kprefabID != null)
			{
				this.cachedConduitDispenser = kprefabID.GetComponent<ConduitDispenser>();
			}
		}
		return this.cachedConduitDispenser;
	}

	// Token: 0x0600381F RID: 14367 RVA: 0x00132B68 File Offset: 0x00130D68
	private void DestroyOrphanedConduitDispenserBuilding()
	{
		ConduitDispenser dispenser = this.GetConduitDispenser();
		if (dispenser != null)
		{
			this.connectorRef.Set(null);
			GameScheduler.Instance.ScheduleNextFrame("Destroy Staterpillar Dispenser", delegate(object o)
			{
				if (dispenser != null)
				{
					Util.KDestroyGameObject(dispenser.gameObject);
				}
			}, null, null);
		}
	}

	// Token: 0x06003820 RID: 14368 RVA: 0x00132BC0 File Offset: 0x00130DC0
	private void SpawnGenerator(int targetCell)
	{
		StaterpillarGenerator generator = this.GetGenerator();
		GameObject gameObject = null;
		if (generator != null)
		{
			gameObject = generator.gameObject;
		}
		if (!gameObject)
		{
			gameObject = this.connectorDef.Build(targetCell, Orientation.R180, null, this.dummyElement, base.gameObject.GetComponent<PrimaryElement>().Temperature, true, -1f);
			StaterpillarGenerator component = gameObject.GetComponent<StaterpillarGenerator>();
			component.parent = new Ref<Staterpillar>(this);
			this.connectorRef = new Ref<KPrefabID>(component.GetComponent<KPrefabID>());
			gameObject.SetActive(true);
			gameObject.GetComponent<BuildingCellVisualizer>().enabled = false;
			component.enabled = false;
		}
		Attributes attributes = gameObject.gameObject.GetAttributes();
		bool flag = base.gameObject.GetSMI<WildnessMonitor.Instance>().wildness.value > 0f;
		if (flag)
		{
			attributes.Add(this.wildMod);
		}
		bool flag2 = base.gameObject.GetComponent<Effects>().HasEffect("Unhappy");
		CreatureCalorieMonitor.Instance smi = base.gameObject.GetSMI<CreatureCalorieMonitor.Instance>();
		if (smi.IsHungry() || flag2)
		{
			float calories0to = smi.GetCalories0to1();
			float num = 1f;
			if (calories0to <= 0f)
			{
				num = (flag ? 0.1f : 0.025f);
			}
			else if (calories0to <= 0.3f)
			{
				num = 0.5f;
			}
			else if (calories0to <= 0.5f)
			{
				num = 0.75f;
			}
			if (num < 1f)
			{
				float num2;
				if (flag)
				{
					num2 = Mathf.Lerp(0f, 25f, 1f - num);
				}
				else
				{
					num2 = (1f - num) * 100f;
				}
				AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.GeneratorOutput.Id, -num2, BUILDINGS.PREFABS.STATERPILLARGENERATOR.MODIFIERS.HUNGRY, false, false, true);
				attributes.Add(modifier);
			}
		}
	}

	// Token: 0x06003821 RID: 14369 RVA: 0x00132D7E File Offset: 0x00130F7E
	private void EnableGenerator()
	{
		StaterpillarGenerator generator = this.GetGenerator();
		generator.enabled = true;
		generator.GetComponent<BuildingCellVisualizer>().enabled = true;
	}

	// Token: 0x06003822 RID: 14370 RVA: 0x00132D98 File Offset: 0x00130F98
	public StaterpillarGenerator GetGenerator()
	{
		if (this.cachedGenerator == null)
		{
			KPrefabID kprefabID = this.connectorRef.Get();
			if (kprefabID != null)
			{
				this.cachedGenerator = kprefabID.GetComponent<StaterpillarGenerator>();
			}
		}
		return this.cachedGenerator;
	}

	// Token: 0x040021B4 RID: 8628
	public ObjectLayer conduitLayer;

	// Token: 0x040021B5 RID: 8629
	public string connectorDefId;

	// Token: 0x040021B6 RID: 8630
	private IList<Tag> dummyElement;

	// Token: 0x040021B7 RID: 8631
	private BuildingDef connectorDef;

	// Token: 0x040021B8 RID: 8632
	[Serialize]
	private Ref<KPrefabID> connectorRef = new Ref<KPrefabID>();

	// Token: 0x040021B9 RID: 8633
	private AttributeModifier wildMod = new AttributeModifier(Db.Get().Attributes.GeneratorOutput.Id, -75f, BUILDINGS.PREFABS.STATERPILLARGENERATOR.MODIFIERS.WILD, false, false, true);

	// Token: 0x040021BA RID: 8634
	private ConduitDispenser cachedConduitDispenser;

	// Token: 0x040021BB RID: 8635
	private StaterpillarGenerator cachedGenerator;
}
