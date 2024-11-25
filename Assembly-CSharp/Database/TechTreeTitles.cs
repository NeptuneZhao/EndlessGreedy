using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02000E89 RID: 3721
	public class TechTreeTitles : ResourceSet<TechTreeTitle>
	{
		// Token: 0x06007510 RID: 29968 RVA: 0x002DCC44 File Offset: 0x002DAE44
		public TechTreeTitles(ResourceSet parent) : base("TreeTitles", parent)
		{
		}

		// Token: 0x06007511 RID: 29969 RVA: 0x002DCC54 File Offset: 0x002DAE54
		public void Load(TextAsset tree_file)
		{
			foreach (ResourceTreeNode resourceTreeNode in new ResourceTreeLoader<ResourceTreeNode>(tree_file))
			{
				if (string.Equals(resourceTreeNode.Id.Substring(0, 1), "_"))
				{
					new TechTreeTitle(resourceTreeNode.Id, this, Strings.Get("STRINGS.RESEARCH.TREES.TITLE" + resourceTreeNode.Id.ToUpper()), resourceTreeNode);
				}
			}
		}
	}
}
