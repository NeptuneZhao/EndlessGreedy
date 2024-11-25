using System;
using UnityEngine;

// Token: 0x0200066D RID: 1645
[AddComponentMenu("KMonoBehaviour/scripts/BuildingAttachPoint")]
public class BuildingAttachPoint : KMonoBehaviour
{
	// Token: 0x060028AA RID: 10410 RVA: 0x000E637E File Offset: 0x000E457E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.BuildingAttachPoints.Add(this);
		this.TryAttachEmptyHardpoints();
	}

	// Token: 0x060028AB RID: 10411 RVA: 0x000E6397 File Offset: 0x000E4597
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060028AC RID: 10412 RVA: 0x000E63A0 File Offset: 0x000E45A0
	private void TryAttachEmptyHardpoints()
	{
		for (int i = 0; i < this.points.Length; i++)
		{
			if (!(this.points[i].attachedBuilding != null))
			{
				bool flag = false;
				int num = 0;
				while (num < Components.AttachableBuildings.Count && !flag)
				{
					if (Components.AttachableBuildings[num].attachableToTag == this.points[i].attachableType && Grid.OffsetCell(Grid.PosToCell(base.gameObject), this.points[i].position) == Grid.PosToCell(Components.AttachableBuildings[num]))
					{
						this.points[i].attachedBuilding = Components.AttachableBuildings[num];
						flag = true;
					}
					num++;
				}
			}
		}
	}

	// Token: 0x060028AD RID: 10413 RVA: 0x000E6478 File Offset: 0x000E4678
	public bool AcceptsAttachment(Tag type, int cell)
	{
		int cell2 = Grid.PosToCell(base.gameObject);
		for (int i = 0; i < this.points.Length; i++)
		{
			if (Grid.OffsetCell(cell2, this.points[i].position) == cell && this.points[i].attachableType == type)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060028AE RID: 10414 RVA: 0x000E64DA File Offset: 0x000E46DA
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.BuildingAttachPoints.Remove(this);
	}

	// Token: 0x04001756 RID: 5974
	public BuildingAttachPoint.HardPoint[] points = new BuildingAttachPoint.HardPoint[0];

	// Token: 0x02001452 RID: 5202
	[Serializable]
	public struct HardPoint
	{
		// Token: 0x06008A24 RID: 35364 RVA: 0x0033297A File Offset: 0x00330B7A
		public HardPoint(CellOffset position, Tag attachableType, AttachableBuilding attachedBuilding)
		{
			this.position = position;
			this.attachableType = attachableType;
			this.attachedBuilding = attachedBuilding;
		}

		// Token: 0x04006965 RID: 26981
		public CellOffset position;

		// Token: 0x04006966 RID: 26982
		public Tag attachableType;

		// Token: 0x04006967 RID: 26983
		public AttachableBuilding attachedBuilding;
	}
}
