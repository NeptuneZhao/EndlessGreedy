using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
	// Token: 0x02000E52 RID: 3666
	public class BuildingFacadeResource : PermitResource
	{
		// Token: 0x06007449 RID: 29769 RVA: 0x002C8394 File Offset: 0x002C6594
		[Obsolete("Please use constructor with dlcIds parameter")]
		public BuildingFacadeResource(string Id, string Name, string Description, PermitRarity Rarity, string PrefabID, string AnimFile, Dictionary<string, string> workables = null) : this(Id, Name, Description, Rarity, PrefabID, AnimFile, DlcManager.AVAILABLE_ALL_VERSIONS, workables)
		{
		}

		// Token: 0x0600744A RID: 29770 RVA: 0x002C83B7 File Offset: 0x002C65B7
		public BuildingFacadeResource(string Id, string Name, string Description, PermitRarity Rarity, string PrefabID, string AnimFile, string[] dlcIds, Dictionary<string, string> workables = null) : base(Id, Name, Description, PermitCategory.Building, Rarity, dlcIds)
		{
			this.Id = Id;
			this.PrefabID = PrefabID;
			this.AnimFile = AnimFile;
			this.InteractFile = workables;
		}

		// Token: 0x0600744B RID: 29771 RVA: 0x002C83E8 File Offset: 0x002C65E8
		public void Init()
		{
			GameObject gameObject = Assets.TryGetPrefab(this.PrefabID);
			if (gameObject == null)
			{
				return;
			}
			gameObject.AddOrGet<BuildingFacade>();
			BuildingDef def = gameObject.GetComponent<Building>().Def;
			if (def != null)
			{
				def.AddFacade(this.Id);
			}
		}

		// Token: 0x0600744C RID: 29772 RVA: 0x002C8438 File Offset: 0x002C6638
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(this.AnimFile), "ui", false, "");
			result.SetFacadeForPrefabID(this.PrefabID);
			return result;
		}

		// Token: 0x04005064 RID: 20580
		public string PrefabID;

		// Token: 0x04005065 RID: 20581
		public string AnimFile;

		// Token: 0x04005066 RID: 20582
		public Dictionary<string, string> InteractFile;
	}
}
