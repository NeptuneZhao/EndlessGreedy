using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x020004C7 RID: 1223
public class MySmi : MyAttributeManager<StateMachine.Instance>
{
	// Token: 0x06001A46 RID: 6726 RVA: 0x0008B24C File Offset: 0x0008944C
	public static void Init()
	{
		MyAttributes.Register(new MySmi(new Dictionary<Type, MethodInfo>
		{
			{
				typeof(MySmiGet),
				typeof(MySmi).GetMethod("FindSmi")
			},
			{
				typeof(MySmiReq),
				typeof(MySmi).GetMethod("RequireSmi")
			}
		}));
	}

	// Token: 0x06001A47 RID: 6727 RVA: 0x0008B2B0 File Offset: 0x000894B0
	public MySmi(Dictionary<Type, MethodInfo> attributeMap) : base(attributeMap, null)
	{
	}

	// Token: 0x06001A48 RID: 6728 RVA: 0x0008B2BC File Offset: 0x000894BC
	public static StateMachine.Instance FindSmi<T>(KMonoBehaviour c, bool isStart) where T : StateMachine.Instance
	{
		StateMachineController component = c.GetComponent<StateMachineController>();
		if (component != null)
		{
			return component.GetSMI<T>();
		}
		return null;
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x0008B2E8 File Offset: 0x000894E8
	public static StateMachine.Instance RequireSmi<T>(KMonoBehaviour c, bool isStart) where T : StateMachine.Instance
	{
		if (isStart)
		{
			StateMachine.Instance instance = MySmi.FindSmi<T>(c, isStart);
			Debug.Assert(instance != null, string.Format("{0} '{1}' requires a StateMachineInstance of type {2}!", c.GetType().ToString(), c.name, typeof(T)));
			return instance;
		}
		return MySmi.FindSmi<T>(c, isStart);
	}
}
