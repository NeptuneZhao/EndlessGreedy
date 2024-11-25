using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004CF RID: 1231
public static class StateMachineControllerExtensions
{
	// Token: 0x06001A8A RID: 6794 RVA: 0x0008BE78 File Offset: 0x0008A078
	public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this StateMachine.Instance smi) where StateMachineInstanceType : StateMachine.Instance
	{
		return smi.gameObject.GetSMI<StateMachineInstanceType>();
	}

	// Token: 0x06001A8B RID: 6795 RVA: 0x0008BE85 File Offset: 0x0008A085
	public static DefType GetDef<DefType>(this Component cmp) where DefType : StateMachine.BaseDef
	{
		return cmp.gameObject.GetDef<DefType>();
	}

	// Token: 0x06001A8C RID: 6796 RVA: 0x0008BE94 File Offset: 0x0008A094
	public static DefType GetDef<DefType>(this GameObject go) where DefType : StateMachine.BaseDef
	{
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component == null)
		{
			return default(DefType);
		}
		return component.GetDef<DefType>();
	}

	// Token: 0x06001A8D RID: 6797 RVA: 0x0008BEC1 File Offset: 0x0008A0C1
	public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this Component cmp) where StateMachineInstanceType : class
	{
		return cmp.gameObject.GetSMI<StateMachineInstanceType>();
	}

	// Token: 0x06001A8E RID: 6798 RVA: 0x0008BED0 File Offset: 0x0008A0D0
	public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this GameObject go) where StateMachineInstanceType : class
	{
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component != null)
		{
			return component.GetSMI<StateMachineInstanceType>();
		}
		return default(StateMachineInstanceType);
	}

	// Token: 0x06001A8F RID: 6799 RVA: 0x0008BEFD File Offset: 0x0008A0FD
	public static List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>(this Component cmp) where StateMachineInstanceType : class
	{
		return cmp.gameObject.GetAllSMI<StateMachineInstanceType>();
	}

	// Token: 0x06001A90 RID: 6800 RVA: 0x0008BF0C File Offset: 0x0008A10C
	public static List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>(this GameObject go) where StateMachineInstanceType : class
	{
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component != null)
		{
			return component.GetAllSMI<StateMachineInstanceType>();
		}
		return new List<StateMachineInstanceType>();
	}
}
