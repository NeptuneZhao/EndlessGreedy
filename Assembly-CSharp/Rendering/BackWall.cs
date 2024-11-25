using System;
using UnityEngine;

namespace rendering
{
	// Token: 0x02000E1F RID: 3615
	public class BackWall : MonoBehaviour
	{
		// Token: 0x06007333 RID: 29491 RVA: 0x002C2BA7 File Offset: 0x002C0DA7
		private void Awake()
		{
			this.backwallMaterial.SetTexture("images", this.array);
		}

		// Token: 0x04004F64 RID: 20324
		[SerializeField]
		public Material backwallMaterial;

		// Token: 0x04004F65 RID: 20325
		[SerializeField]
		public Texture2DArray array;
	}
}
