using System;
using UnityEngine;

// Token: 0x0200083C RID: 2108
[AddComponentMenu("KMonoBehaviour/scripts/DeserializeWarnings")]
public class DeserializeWarnings : KMonoBehaviour
{
	// Token: 0x06003AF5 RID: 15093 RVA: 0x00143D07 File Offset: 0x00141F07
	public static void DestroyInstance()
	{
		DeserializeWarnings.Instance = null;
	}

	// Token: 0x06003AF6 RID: 15094 RVA: 0x00143D0F File Offset: 0x00141F0F
	protected override void OnPrefabInit()
	{
		DeserializeWarnings.Instance = this;
	}

	// Token: 0x040023CA RID: 9162
	public DeserializeWarnings.Warning BuildingTemeperatureIsZeroKelvin;

	// Token: 0x040023CB RID: 9163
	public DeserializeWarnings.Warning PipeContentsTemperatureIsNan;

	// Token: 0x040023CC RID: 9164
	public DeserializeWarnings.Warning PrimaryElementTemperatureIsNan;

	// Token: 0x040023CD RID: 9165
	public DeserializeWarnings.Warning PrimaryElementHasNoElement;

	// Token: 0x040023CE RID: 9166
	public static DeserializeWarnings Instance;

	// Token: 0x02001759 RID: 5977
	public struct Warning
	{
		// Token: 0x06009568 RID: 38248 RVA: 0x0035F7CC File Offset: 0x0035D9CC
		public void Warn(string message, GameObject obj = null)
		{
			if (!this.isSet)
			{
				global::Debug.LogWarning(message, obj);
				this.isSet = true;
			}
		}

		// Token: 0x04007285 RID: 29317
		private bool isSet;
	}
}
