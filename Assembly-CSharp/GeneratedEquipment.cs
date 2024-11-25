using System;
using System.Collections.Generic;

// Token: 0x02000083 RID: 131
public class GeneratedEquipment
{
	// Token: 0x0600028C RID: 652 RVA: 0x00012100 File Offset: 0x00010300
	public static void LoadGeneratedEquipment(List<Type> types)
	{
		Type typeFromHandle = typeof(IEquipmentConfig);
		List<Type> list = new List<Type>();
		foreach (Type type in types)
		{
			if (typeFromHandle.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
			{
				list.Add(type);
			}
		}
		foreach (Type type2 in list)
		{
			object obj = Activator.CreateInstance(type2);
			try
			{
				EquipmentConfigManager.Instance.RegisterEquipment(obj as IEquipmentConfig);
			}
			catch (Exception e)
			{
				DebugUtil.LogException(null, "Exception in RegisterEquipment for type " + type2.FullName + " from " + type2.Assembly.GetName().Name, e);
			}
		}
	}
}
