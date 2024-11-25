using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020009D7 RID: 2519
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/OrbitalMechanics")]
public class OrbitalMechanics : KMonoBehaviour
{
	// Token: 0x06004924 RID: 18724 RVA: 0x001A2D67 File Offset: 0x001A0F67
	protected override void OnPrefabInit()
	{
		base.Subscribe<OrbitalMechanics>(-1298331547, this.OnClusterLocationChangedDelegate);
	}

	// Token: 0x06004925 RID: 18725 RVA: 0x001A2D7C File Offset: 0x001A0F7C
	private void OnClusterLocationChanged(object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		this.UpdateLocation(clusterLocationChangedEvent.newLocation);
	}

	// Token: 0x06004926 RID: 18726 RVA: 0x001A2D9C File Offset: 0x001A0F9C
	protected override void OnCleanUp()
	{
		if (this.orbitingObjects != null)
		{
			foreach (Ref<OrbitalObject> @ref in this.orbitingObjects)
			{
				if (!@ref.Get().IsNullOrDestroyed())
				{
					Util.KDestroyGameObject(@ref.Get());
				}
			}
		}
	}

	// Token: 0x06004927 RID: 18727 RVA: 0x001A2E08 File Offset: 0x001A1008
	[ContextMenu("Rebuild")]
	private void Rebuild()
	{
		List<string> list = new List<string>();
		if (this.orbitingObjects != null)
		{
			foreach (Ref<OrbitalObject> @ref in this.orbitingObjects)
			{
				if (!@ref.Get().IsNullOrDestroyed())
				{
					list.Add(@ref.Get().orbitalDBId);
					Util.KDestroyGameObject(@ref.Get());
				}
			}
			this.orbitingObjects = new List<Ref<OrbitalObject>>();
		}
		if (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				this.CreateOrbitalObject(list[i]);
			}
		}
	}

	// Token: 0x06004928 RID: 18728 RVA: 0x001A2EC0 File Offset: 0x001A10C0
	private void UpdateLocation(AxialI location)
	{
		if (this.orbitingObjects.Count > 0)
		{
			foreach (Ref<OrbitalObject> @ref in this.orbitingObjects)
			{
				if (!@ref.Get().IsNullOrDestroyed())
				{
					Util.KDestroyGameObject(@ref.Get());
				}
			}
			this.orbitingObjects = new List<Ref<OrbitalObject>>();
		}
		ClusterGridEntity visibleEntityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(location, EntityLayer.POI);
		if (visibleEntityOfLayerAtCell != null)
		{
			ArtifactPOIClusterGridEntity component = visibleEntityOfLayerAtCell.GetComponent<ArtifactPOIClusterGridEntity>();
			if (component != null)
			{
				ArtifactPOIStates.Instance smi = component.GetSMI<ArtifactPOIStates.Instance>();
				if (smi != null && smi.configuration.poiType.orbitalObject != null)
				{
					foreach (string orbit_db_name in smi.configuration.poiType.orbitalObject)
					{
						this.CreateOrbitalObject(orbit_db_name);
					}
				}
			}
			HarvestablePOIClusterGridEntity component2 = visibleEntityOfLayerAtCell.GetComponent<HarvestablePOIClusterGridEntity>();
			if (component2 != null)
			{
				HarvestablePOIStates.Instance smi2 = component2.GetSMI<HarvestablePOIStates.Instance>();
				if (smi2 != null && smi2.configuration.poiType.orbitalObject != null)
				{
					List<string> orbitalObject = smi2.configuration.poiType.orbitalObject;
					KRandom krandom = new KRandom();
					float num = smi2.poiCapacity / smi2.configuration.GetMaxCapacity() * (float)smi2.configuration.poiType.maxNumOrbitingObjects;
					int num2 = 0;
					while ((float)num2 < num)
					{
						int index = krandom.Next(orbitalObject.Count);
						this.CreateOrbitalObject(orbitalObject[index]);
						num2++;
					}
					return;
				}
			}
		}
		else
		{
			Clustercraft component3 = base.GetComponent<Clustercraft>();
			if (component3 != null)
			{
				if (component3.GetOrbitAsteroid() != null || component3.Status == Clustercraft.CraftStatus.Launching)
				{
					this.CreateOrbitalObject(Db.Get().OrbitalTypeCategories.orbit.Id);
					return;
				}
				if (component3.Status == Clustercraft.CraftStatus.Landing)
				{
					this.CreateOrbitalObject(Db.Get().OrbitalTypeCategories.landed.Id);
				}
			}
		}
	}

	// Token: 0x06004929 RID: 18729 RVA: 0x001A30F0 File Offset: 0x001A12F0
	public void CreateOrbitalObject(string orbit_db_name)
	{
		WorldContainer component = base.GetComponent<WorldContainer>();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(OrbitalBGConfig.ID), base.gameObject, null);
		OrbitalObject component2 = gameObject.GetComponent<OrbitalObject>();
		component2.Init(orbit_db_name, component, this.orbitingObjects);
		gameObject.SetActive(true);
		this.orbitingObjects.Add(new Ref<OrbitalObject>(component2));
	}

	// Token: 0x04002FDB RID: 12251
	[Serialize]
	private List<Ref<OrbitalObject>> orbitingObjects = new List<Ref<OrbitalObject>>();

	// Token: 0x04002FDC RID: 12252
	private EventSystem.IntraObjectHandler<OrbitalMechanics> OnClusterLocationChangedDelegate = new EventSystem.IntraObjectHandler<OrbitalMechanics>(delegate(OrbitalMechanics cmp, object data)
	{
		cmp.OnClusterLocationChanged(data);
	});
}
