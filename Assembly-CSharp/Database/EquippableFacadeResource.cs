using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02000E62 RID: 3682
	public class EquippableFacadeResource : PermitResource
	{
		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06007480 RID: 29824 RVA: 0x002D4730 File Offset: 0x002D2930
		// (set) Token: 0x06007481 RID: 29825 RVA: 0x002D4738 File Offset: 0x002D2938
		public string BuildOverride { get; private set; }

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06007482 RID: 29826 RVA: 0x002D4741 File Offset: 0x002D2941
		// (set) Token: 0x06007483 RID: 29827 RVA: 0x002D4749 File Offset: 0x002D2949
		public string DefID { get; private set; }

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06007484 RID: 29828 RVA: 0x002D4752 File Offset: 0x002D2952
		// (set) Token: 0x06007485 RID: 29829 RVA: 0x002D475A File Offset: 0x002D295A
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x06007486 RID: 29830 RVA: 0x002D4764 File Offset: 0x002D2964
		[Obsolete("Please use constructor with dlcIds parameter")]
		public EquippableFacadeResource(string id, string name, string desc, PermitRarity rarity, string buildOverride, string defID, string animFile) : this(id, name, desc, rarity, buildOverride, defID, animFile, DlcManager.AVAILABLE_ALL_VERSIONS)
		{
		}

		// Token: 0x06007487 RID: 29831 RVA: 0x002D4787 File Offset: 0x002D2987
		public EquippableFacadeResource(string id, string name, string desc, PermitRarity rarity, string buildOverride, string defID, string animFile, string[] dlcIds) : base(id, name, desc, PermitCategory.Equipment, rarity, dlcIds)
		{
			this.DefID = defID;
			this.BuildOverride = buildOverride;
			this.AnimFile = Assets.GetAnim(animFile);
		}

		// Token: 0x06007488 RID: 29832 RVA: 0x002D47BC File Offset: 0x002D29BC
		public global::Tuple<Sprite, Color> GetUISprite()
		{
			if (this.AnimFile == null)
			{
				global::Debug.LogError("Facade AnimFile is null: " + this.DefID);
			}
			Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			return new global::Tuple<Sprite, Color>(uispriteFromMultiObjectAnim, (uispriteFromMultiObjectAnim != null) ? Color.white : Color.clear);
		}

		// Token: 0x06007489 RID: 29833 RVA: 0x002D481C File Offset: 0x002D2A1C
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = this.GetUISprite().first;
			GameObject gameObject = Assets.TryGetPrefab(this.DefID);
			if (gameObject == null || !gameObject)
			{
				result.SetFacadeForPrefabID(this.DefID);
			}
			else
			{
				result.SetFacadeForPrefabName(gameObject.GetProperName());
			}
			return result;
		}
	}
}
