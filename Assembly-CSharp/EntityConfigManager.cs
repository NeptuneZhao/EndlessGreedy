using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000886 RID: 2182
[AddComponentMenu("KMonoBehaviour/scripts/EntityConfigManager")]
public class EntityConfigManager : KMonoBehaviour
{
	// Token: 0x06003D39 RID: 15673 RVA: 0x00152A05 File Offset: 0x00150C05
	public static void DestroyInstance()
	{
		EntityConfigManager.Instance = null;
	}

	// Token: 0x06003D3A RID: 15674 RVA: 0x00152A0D File Offset: 0x00150C0D
	protected override void OnPrefabInit()
	{
		EntityConfigManager.Instance = this;
	}

	// Token: 0x06003D3B RID: 15675 RVA: 0x00152A18 File Offset: 0x00150C18
	private static int GetSortOrder(Type type)
	{
		foreach (Attribute attribute in type.GetCustomAttributes(true))
		{
			if (attribute.GetType() == typeof(EntityConfigOrder))
			{
				return (attribute as EntityConfigOrder).sortOrder;
			}
		}
		return 0;
	}

	// Token: 0x06003D3C RID: 15676 RVA: 0x00152A68 File Offset: 0x00150C68
	public void LoadGeneratedEntities(List<Type> types)
	{
		Type typeFromHandle = typeof(IEntityConfig);
		Type typeFromHandle2 = typeof(IMultiEntityConfig);
		List<EntityConfigManager.ConfigEntry> list = new List<EntityConfigManager.ConfigEntry>();
		foreach (Type type in types)
		{
			if ((typeFromHandle.IsAssignableFrom(type) || typeFromHandle2.IsAssignableFrom(type)) && !type.IsAbstract && !type.IsInterface)
			{
				int sortOrder = EntityConfigManager.GetSortOrder(type);
				EntityConfigManager.ConfigEntry item = new EntityConfigManager.ConfigEntry
				{
					type = type,
					sortOrder = sortOrder
				};
				list.Add(item);
			}
		}
		list.Sort((EntityConfigManager.ConfigEntry x, EntityConfigManager.ConfigEntry y) => x.sortOrder.CompareTo(y.sortOrder));
		foreach (EntityConfigManager.ConfigEntry configEntry in list)
		{
			object obj = Activator.CreateInstance(configEntry.type);
			if (obj is IEntityConfig && DlcManager.IsDlcListValidForCurrentContent((obj as IEntityConfig).GetDlcIds()))
			{
				this.RegisterEntity(obj as IEntityConfig);
			}
			if (obj is IMultiEntityConfig)
			{
				this.RegisterEntities(obj as IMultiEntityConfig);
			}
		}
	}

	// Token: 0x06003D3D RID: 15677 RVA: 0x00152BC8 File Offset: 0x00150DC8
	public void RegisterEntity(IEntityConfig config)
	{
		KPrefabID component = config.CreatePrefab().GetComponent<KPrefabID>();
		component.requiredDlcIds = config.GetDlcIds();
		component.prefabInitFn += config.OnPrefabInit;
		component.prefabSpawnFn += config.OnSpawn;
		Assets.AddPrefab(component);
	}

	// Token: 0x06003D3E RID: 15678 RVA: 0x00152C18 File Offset: 0x00150E18
	public void RegisterEntities(IMultiEntityConfig config)
	{
		foreach (GameObject gameObject in config.CreatePrefabs())
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			component.prefabInitFn += config.OnPrefabInit;
			component.prefabSpawnFn += config.OnSpawn;
			Assets.AddPrefab(component);
		}
	}

	// Token: 0x0400255D RID: 9565
	public static EntityConfigManager Instance;

	// Token: 0x0200178A RID: 6026
	private struct ConfigEntry
	{
		// Token: 0x0400730A RID: 29450
		public Type type;

		// Token: 0x0400730B RID: 29451
		public int sortOrder;
	}
}
