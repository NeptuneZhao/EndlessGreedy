using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F6F RID: 3951
	public class ModifierInstance<ModifierType> : IStateMachineTarget
	{
		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06007935 RID: 31029 RVA: 0x002FEDF8 File Offset: 0x002FCFF8
		// (set) Token: 0x06007936 RID: 31030 RVA: 0x002FEE00 File Offset: 0x002FD000
		public GameObject gameObject { get; private set; }

		// Token: 0x06007937 RID: 31031 RVA: 0x002FEE09 File Offset: 0x002FD009
		public ModifierInstance(GameObject game_object, ModifierType modifier)
		{
			this.gameObject = game_object;
			this.modifier = modifier;
		}

		// Token: 0x06007938 RID: 31032 RVA: 0x002FEE1F File Offset: 0x002FD01F
		public ComponentType GetComponent<ComponentType>()
		{
			return this.gameObject.GetComponent<ComponentType>();
		}

		// Token: 0x06007939 RID: 31033 RVA: 0x002FEE2C File Offset: 0x002FD02C
		public int Subscribe(int hash, Action<object> handler)
		{
			return this.gameObject.GetComponent<KMonoBehaviour>().Subscribe(hash, handler);
		}

		// Token: 0x0600793A RID: 31034 RVA: 0x002FEE40 File Offset: 0x002FD040
		public void Unsubscribe(int hash, Action<object> handler)
		{
			this.gameObject.GetComponent<KMonoBehaviour>().Unsubscribe(hash, handler);
		}

		// Token: 0x0600793B RID: 31035 RVA: 0x002FEE54 File Offset: 0x002FD054
		public void Unsubscribe(int id)
		{
			this.gameObject.GetComponent<KMonoBehaviour>().Unsubscribe(id);
		}

		// Token: 0x0600793C RID: 31036 RVA: 0x002FEE67 File Offset: 0x002FD067
		public void Trigger(int hash, object data = null)
		{
			this.gameObject.GetComponent<KPrefabID>().Trigger(hash, data);
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x0600793D RID: 31037 RVA: 0x002FEE7B File Offset: 0x002FD07B
		public Transform transform
		{
			get
			{
				return this.gameObject.transform;
			}
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x0600793E RID: 31038 RVA: 0x002FEE88 File Offset: 0x002FD088
		public bool isNull
		{
			get
			{
				return this.gameObject == null;
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x0600793F RID: 31039 RVA: 0x002FEE96 File Offset: 0x002FD096
		public string name
		{
			get
			{
				return this.gameObject.name;
			}
		}

		// Token: 0x06007940 RID: 31040 RVA: 0x002FEEA3 File Offset: 0x002FD0A3
		public virtual void OnCleanUp()
		{
		}

		// Token: 0x04005A91 RID: 23185
		public ModifierType modifier;
	}
}
