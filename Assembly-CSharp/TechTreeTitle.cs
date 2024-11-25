using System;
using UnityEngine;

// Token: 0x02000A55 RID: 2645
public class TechTreeTitle : Resource
{
	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x06004CBF RID: 19647 RVA: 0x001B68E1 File Offset: 0x001B4AE1
	public Vector2 center
	{
		get
		{
			return this.node.center;
		}
	}

	// Token: 0x1700057F RID: 1407
	// (get) Token: 0x06004CC0 RID: 19648 RVA: 0x001B68EE File Offset: 0x001B4AEE
	public float width
	{
		get
		{
			return this.node.width;
		}
	}

	// Token: 0x17000580 RID: 1408
	// (get) Token: 0x06004CC1 RID: 19649 RVA: 0x001B68FB File Offset: 0x001B4AFB
	public float height
	{
		get
		{
			return this.node.height;
		}
	}

	// Token: 0x06004CC2 RID: 19650 RVA: 0x001B6908 File Offset: 0x001B4B08
	public TechTreeTitle(string id, ResourceSet parent, string name, ResourceTreeNode node) : base(id, parent, name)
	{
		this.node = node;
	}

	// Token: 0x04003309 RID: 13065
	public string desc;

	// Token: 0x0400330A RID: 13066
	private ResourceTreeNode node;
}
