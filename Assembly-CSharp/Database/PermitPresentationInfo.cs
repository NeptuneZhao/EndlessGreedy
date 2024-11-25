using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000E6E RID: 3694
	public struct PermitPresentationInfo
	{
		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x060074B0 RID: 29872 RVA: 0x002D8036 File Offset: 0x002D6236
		// (set) Token: 0x060074B1 RID: 29873 RVA: 0x002D803E File Offset: 0x002D623E
		public string facadeFor { readonly get; private set; }

		// Token: 0x060074B2 RID: 29874 RVA: 0x002D8047 File Offset: 0x002D6247
		public static Sprite GetUnknownSprite()
		{
			return Assets.GetSprite("unknown");
		}

		// Token: 0x060074B3 RID: 29875 RVA: 0x002D8058 File Offset: 0x002D6258
		public void SetFacadeForPrefabName(string prefabName)
		{
			this.facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_FACADE_FOR.Replace("{ConfigProperName}", prefabName);
		}

		// Token: 0x060074B4 RID: 29876 RVA: 0x002D8070 File Offset: 0x002D6270
		public void SetFacadeForPrefabID(string prefabId)
		{
			if (Assets.TryGetPrefab(prefabId) == null)
			{
				this.facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_DLC_REQUIRED;
				return;
			}
			this.facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_FACADE_FOR.Replace("{ConfigProperName}", Assets.GetPrefab(prefabId).GetProperName());
		}

		// Token: 0x060074B5 RID: 29877 RVA: 0x002D80C6 File Offset: 0x002D62C6
		public void SetFacadeForText(string text)
		{
			this.facadeFor = text;
		}

		// Token: 0x04005432 RID: 21554
		public Sprite sprite;
	}
}
