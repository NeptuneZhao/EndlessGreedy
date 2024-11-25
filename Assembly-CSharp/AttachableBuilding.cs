using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200065A RID: 1626
[AddComponentMenu("KMonoBehaviour/scripts/AttachableBuilding")]
public class AttachableBuilding : KMonoBehaviour
{
	// Token: 0x06002819 RID: 10265 RVA: 0x000E3950 File Offset: 0x000E1B50
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.RegisterWithAttachPoint(true);
		Components.AttachableBuildings.Add(this);
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this))
		{
			AttachableBuilding component = gameObject.GetComponent<AttachableBuilding>();
			if (component != null && component.onAttachmentNetworkChanged != null)
			{
				component.onAttachmentNetworkChanged(this);
			}
		}
	}

	// Token: 0x0600281A RID: 10266 RVA: 0x000E39D8 File Offset: 0x000E1BD8
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600281B RID: 10267 RVA: 0x000E39E0 File Offset: 0x000E1BE0
	public void RegisterWithAttachPoint(bool register)
	{
		BuildingDef buildingDef = null;
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		BuildingUnderConstruction component2 = base.GetComponent<BuildingUnderConstruction>();
		if (component != null)
		{
			buildingDef = component.Def;
		}
		else if (component2 != null)
		{
			buildingDef = component2.Def;
		}
		int num = Grid.OffsetCell(Grid.PosToCell(base.gameObject), buildingDef.attachablePosition);
		bool flag = false;
		int num2 = 0;
		while (!flag && num2 < Components.BuildingAttachPoints.Count)
		{
			for (int i = 0; i < Components.BuildingAttachPoints[num2].points.Length; i++)
			{
				if (num == Grid.OffsetCell(Grid.PosToCell(Components.BuildingAttachPoints[num2]), Components.BuildingAttachPoints[num2].points[i].position))
				{
					if (register)
					{
						Components.BuildingAttachPoints[num2].points[i].attachedBuilding = this;
					}
					else if (Components.BuildingAttachPoints[num2].points[i].attachedBuilding == this)
					{
						Components.BuildingAttachPoints[num2].points[i].attachedBuilding = null;
					}
					flag = true;
					break;
				}
			}
			num2++;
		}
	}

	// Token: 0x0600281C RID: 10268 RVA: 0x000E3B28 File Offset: 0x000E1D28
	public static void GetAttachedBelow(AttachableBuilding searchStart, ref List<GameObject> buildings)
	{
		AttachableBuilding attachableBuilding = searchStart;
		while (attachableBuilding != null)
		{
			BuildingAttachPoint attachedTo = attachableBuilding.GetAttachedTo();
			attachableBuilding = null;
			if (attachedTo != null)
			{
				buildings.Add(attachedTo.gameObject);
				attachableBuilding = attachedTo.GetComponent<AttachableBuilding>();
			}
		}
	}

	// Token: 0x0600281D RID: 10269 RVA: 0x000E3B68 File Offset: 0x000E1D68
	public static int CountAttachedBelow(AttachableBuilding searchStart)
	{
		int num = 0;
		AttachableBuilding attachableBuilding = searchStart;
		while (attachableBuilding != null)
		{
			BuildingAttachPoint attachedTo = attachableBuilding.GetAttachedTo();
			attachableBuilding = null;
			if (attachedTo != null)
			{
				num++;
				attachableBuilding = attachedTo.GetComponent<AttachableBuilding>();
			}
		}
		return num;
	}

	// Token: 0x0600281E RID: 10270 RVA: 0x000E3BA4 File Offset: 0x000E1DA4
	public static void GetAttachedAbove(AttachableBuilding searchStart, ref List<GameObject> buildings)
	{
		BuildingAttachPoint buildingAttachPoint = searchStart.GetComponent<BuildingAttachPoint>();
		while (buildingAttachPoint != null)
		{
			bool flag = false;
			foreach (BuildingAttachPoint.HardPoint hardPoint in buildingAttachPoint.points)
			{
				if (flag)
				{
					break;
				}
				if (hardPoint.attachedBuilding != null)
				{
					foreach (object obj in Components.AttachableBuildings)
					{
						AttachableBuilding attachableBuilding = (AttachableBuilding)obj;
						if (attachableBuilding == hardPoint.attachedBuilding)
						{
							buildings.Add(attachableBuilding.gameObject);
							buildingAttachPoint = attachableBuilding.GetComponent<BuildingAttachPoint>();
							flag = true;
						}
					}
				}
			}
			if (!flag)
			{
				buildingAttachPoint = null;
			}
		}
	}

	// Token: 0x0600281F RID: 10271 RVA: 0x000E3C80 File Offset: 0x000E1E80
	public static void NotifyBuildingsNetworkChanged(List<GameObject> buildings, AttachableBuilding attachable = null)
	{
		foreach (GameObject gameObject in buildings)
		{
			AttachableBuilding component = gameObject.GetComponent<AttachableBuilding>();
			if (component != null && component.onAttachmentNetworkChanged != null)
			{
				component.onAttachmentNetworkChanged(attachable);
			}
		}
	}

	// Token: 0x06002820 RID: 10272 RVA: 0x000E3CEC File Offset: 0x000E1EEC
	public static List<GameObject> GetAttachedNetwork(AttachableBuilding searchStart)
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(searchStart.gameObject);
		AttachableBuilding.GetAttachedAbove(searchStart, ref list);
		AttachableBuilding.GetAttachedBelow(searchStart, ref list);
		return list;
	}

	// Token: 0x06002821 RID: 10273 RVA: 0x000E3D1C File Offset: 0x000E1F1C
	public BuildingAttachPoint GetAttachedTo()
	{
		for (int i = 0; i < Components.BuildingAttachPoints.Count; i++)
		{
			for (int j = 0; j < Components.BuildingAttachPoints[i].points.Length; j++)
			{
				if (Components.BuildingAttachPoints[i].points[j].attachedBuilding == this && (Components.BuildingAttachPoints[i].points[j].attachedBuilding.GetComponent<Deconstructable>() == null || !Components.BuildingAttachPoints[i].points[j].attachedBuilding.GetComponent<Deconstructable>().HasBeenDestroyed))
				{
					return Components.BuildingAttachPoints[i];
				}
			}
		}
		return null;
	}

	// Token: 0x06002822 RID: 10274 RVA: 0x000E3DE6 File Offset: 0x000E1FE6
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		AttachableBuilding.NotifyBuildingsNetworkChanged(AttachableBuilding.GetAttachedNetwork(this), this);
		this.RegisterWithAttachPoint(false);
		Components.AttachableBuildings.Remove(this);
	}

	// Token: 0x0400171F RID: 5919
	public Tag attachableToTag;

	// Token: 0x04001720 RID: 5920
	public Action<object> onAttachmentNetworkChanged;
}
