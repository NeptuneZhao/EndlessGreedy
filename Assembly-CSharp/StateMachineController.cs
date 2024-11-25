using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;
using UnityEngine;

// Token: 0x020004CE RID: 1230
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/StateMachineController")]
public class StateMachineController : KMonoBehaviour, ISaveLoadableDetails, IStateMachineControllerHack
{
	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06001A72 RID: 6770 RVA: 0x0008B90A File Offset: 0x00089B0A
	public StateMachineController.CmpDef cmpdef
	{
		get
		{
			return this.defHandle.Get<StateMachineController.CmpDef>();
		}
	}

	// Token: 0x06001A73 RID: 6771 RVA: 0x0008B917 File Offset: 0x00089B17
	public IEnumerator<StateMachine.Instance> GetEnumerator()
	{
		return this.stateMachines.GetEnumerator();
	}

	// Token: 0x06001A74 RID: 6772 RVA: 0x0008B929 File Offset: 0x00089B29
	public void AddStateMachineInstance(StateMachine.Instance state_machine)
	{
		if (!this.stateMachines.Contains(state_machine))
		{
			this.stateMachines.Add(state_machine);
			MyAttributes.OnAwake(state_machine, this);
		}
	}

	// Token: 0x06001A75 RID: 6773 RVA: 0x0008B94C File Offset: 0x00089B4C
	public void RemoveStateMachineInstance(StateMachine.Instance state_machine)
	{
		if (!state_machine.GetStateMachine().saveHistory && !state_machine.GetStateMachine().debugSettings.saveHistory)
		{
			this.stateMachines.Remove(state_machine);
		}
	}

	// Token: 0x06001A76 RID: 6774 RVA: 0x0008B97A File Offset: 0x00089B7A
	public bool HasStateMachineInstance(StateMachine.Instance state_machine)
	{
		return this.stateMachines.Contains(state_machine);
	}

	// Token: 0x06001A77 RID: 6775 RVA: 0x0008B988 File Offset: 0x00089B88
	public void AddDef(StateMachine.BaseDef def)
	{
		this.cmpdef.defs.Add(def);
	}

	// Token: 0x06001A78 RID: 6776 RVA: 0x0008B99B File Offset: 0x00089B9B
	public LoggerFSSSS GetLog()
	{
		return this.log;
	}

	// Token: 0x06001A79 RID: 6777 RVA: 0x0008B9A3 File Offset: 0x00089BA3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.log.SetName(base.name);
		base.Subscribe<StateMachineController>(1969584890, StateMachineController.OnTargetDestroyedDelegate);
		base.Subscribe<StateMachineController>(1502190696, StateMachineController.OnTargetDestroyedDelegate);
	}

	// Token: 0x06001A7A RID: 6778 RVA: 0x0008B9E0 File Offset: 0x00089BE0
	private void OnTargetDestroyed(object data)
	{
		while (this.stateMachines.Count > 0)
		{
			StateMachine.Instance instance = this.stateMachines[0];
			instance.StopSM("StateMachineController.OnCleanUp");
			this.stateMachines.Remove(instance);
		}
	}

	// Token: 0x06001A7B RID: 6779 RVA: 0x0008BA24 File Offset: 0x00089C24
	protected override void OnLoadLevel()
	{
		while (this.stateMachines.Count > 0)
		{
			StateMachine.Instance instance = this.stateMachines[0];
			instance.FreeResources();
			this.stateMachines.Remove(instance);
		}
	}

	// Token: 0x06001A7C RID: 6780 RVA: 0x0008BA64 File Offset: 0x00089C64
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		while (this.stateMachines.Count > 0)
		{
			StateMachine.Instance instance = this.stateMachines[0];
			instance.StopSM("StateMachineController.OnCleanUp");
			this.stateMachines.Remove(instance);
		}
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x0008BAAC File Offset: 0x00089CAC
	public void CreateSMIS()
	{
		if (!this.defHandle.IsValid())
		{
			return;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			baseDef.CreateSMI(this);
		}
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x0008BB14 File Offset: 0x00089D14
	public void StartSMIS()
	{
		if (!this.defHandle.IsValid())
		{
			return;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			if (!baseDef.preventStartSMIOnSpawn)
			{
				StateMachine.Instance smi = this.GetSMI(Singleton<StateMachineManager>.Instance.CreateStateMachine(baseDef.GetStateMachineType()).GetStateMachineInstanceType());
				if (smi != null && !smi.IsRunning())
				{
					smi.StartSM();
				}
			}
		}
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x0008BBA8 File Offset: 0x00089DA8
	public void Serialize(BinaryWriter writer)
	{
		this.serializer.Serialize(this.stateMachines, writer);
	}

	// Token: 0x06001A80 RID: 6784 RVA: 0x0008BBBC File Offset: 0x00089DBC
	public void Deserialize(IReader reader)
	{
		this.serializer.Deserialize(reader);
	}

	// Token: 0x06001A81 RID: 6785 RVA: 0x0008BBCA File Offset: 0x00089DCA
	public bool Restore(StateMachine.Instance smi)
	{
		return this.serializer.Restore(smi);
	}

	// Token: 0x06001A82 RID: 6786 RVA: 0x0008BBD8 File Offset: 0x00089DD8
	public DefType GetDef<DefType>() where DefType : StateMachine.BaseDef
	{
		if (!this.defHandle.IsValid())
		{
			return default(DefType);
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			DefType defType = baseDef as DefType;
			if (defType != null)
			{
				return defType;
			}
		}
		return default(DefType);
	}

	// Token: 0x06001A83 RID: 6787 RVA: 0x0008BC64 File Offset: 0x00089E64
	public List<DefType> GetDefs<DefType>() where DefType : StateMachine.BaseDef
	{
		List<DefType> list = new List<DefType>();
		if (!this.defHandle.IsValid())
		{
			return list;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			DefType defType = baseDef as DefType;
			if (defType != null)
			{
				list.Add(defType);
			}
		}
		return list;
	}

	// Token: 0x06001A84 RID: 6788 RVA: 0x0008BCE4 File Offset: 0x00089EE4
	public StateMachine.Instance GetSMI(Type type)
	{
		for (int i = 0; i < this.stateMachines.Count; i++)
		{
			StateMachine.Instance instance = this.stateMachines[i];
			if (type.IsAssignableFrom(instance.GetType()))
			{
				return instance;
			}
		}
		return null;
	}

	// Token: 0x06001A85 RID: 6789 RVA: 0x0008BD25 File Offset: 0x00089F25
	public StateMachineInstanceType GetSMI<StateMachineInstanceType>() where StateMachineInstanceType : class
	{
		return this.GetSMI(typeof(StateMachineInstanceType)) as StateMachineInstanceType;
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x0008BD44 File Offset: 0x00089F44
	public List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>() where StateMachineInstanceType : class
	{
		List<StateMachineInstanceType> list = new List<StateMachineInstanceType>();
		foreach (StateMachine.Instance instance in this.stateMachines)
		{
			StateMachineInstanceType stateMachineInstanceType = instance as StateMachineInstanceType;
			if (stateMachineInstanceType != null)
			{
				list.Add(stateMachineInstanceType);
			}
		}
		return list;
	}

	// Token: 0x06001A87 RID: 6791 RVA: 0x0008BDB0 File Offset: 0x00089FB0
	public List<IGameObjectEffectDescriptor> GetDescriptors()
	{
		List<IGameObjectEffectDescriptor> list = new List<IGameObjectEffectDescriptor>();
		if (!this.defHandle.IsValid())
		{
			return list;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			if (baseDef is IGameObjectEffectDescriptor)
			{
				list.Add(baseDef as IGameObjectEffectDescriptor);
			}
		}
		return list;
	}

	// Token: 0x04000F0D RID: 3853
	public DefHandle defHandle;

	// Token: 0x04000F0E RID: 3854
	private List<StateMachine.Instance> stateMachines = new List<StateMachine.Instance>();

	// Token: 0x04000F0F RID: 3855
	private LoggerFSSSS log = new LoggerFSSSS("StateMachineController", 35);

	// Token: 0x04000F10 RID: 3856
	private StateMachineSerializer serializer = new StateMachineSerializer();

	// Token: 0x04000F11 RID: 3857
	private static readonly EventSystem.IntraObjectHandler<StateMachineController> OnTargetDestroyedDelegate = new EventSystem.IntraObjectHandler<StateMachineController>(delegate(StateMachineController component, object data)
	{
		component.OnTargetDestroyed(data);
	});

	// Token: 0x020012A8 RID: 4776
	public class CmpDef
	{
		// Token: 0x040063F0 RID: 25584
		public List<StateMachine.BaseDef> defs = new List<StateMachine.BaseDef>();
	}
}
