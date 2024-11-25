using System;
using UnityEngine;

// Token: 0x0200007C RID: 124
public static class EntityTemplateExtensions
{
	// Token: 0x0600026F RID: 623 RVA: 0x0001142C File Offset: 0x0000F62C
	public static DefType AddOrGetDef<DefType>(this GameObject go) where DefType : StateMachine.BaseDef
	{
		StateMachineController stateMachineController = go.AddOrGet<StateMachineController>();
		DefType defType = stateMachineController.GetDef<DefType>();
		if (defType == null)
		{
			defType = Activator.CreateInstance<DefType>();
			stateMachineController.AddDef(defType);
			defType.Configure(go);
		}
		return defType;
	}

	// Token: 0x06000270 RID: 624 RVA: 0x00011470 File Offset: 0x0000F670
	public static ComponentType AddOrGet<ComponentType>(this GameObject go) where ComponentType : Component
	{
		ComponentType componentType = go.GetComponent<ComponentType>();
		if (componentType == null)
		{
			componentType = go.AddComponent<ComponentType>();
		}
		return componentType;
	}
}
