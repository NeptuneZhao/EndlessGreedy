using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x020007D5 RID: 2005
public class GameComps : KComponents
{
	// Token: 0x0600374E RID: 14158 RVA: 0x0012DB90 File Offset: 0x0012BD90
	public GameComps()
	{
		foreach (FieldInfo fieldInfo in typeof(GameComps).GetFields())
		{
			object obj = Activator.CreateInstance(fieldInfo.FieldType);
			fieldInfo.SetValue(null, obj);
			base.Add<IComponentManager>(obj as IComponentManager);
			if (obj is IKComponentManager)
			{
				IKComponentManager inst = obj as IKComponentManager;
				GameComps.AddKComponentManager(fieldInfo.FieldType, inst);
			}
		}
	}

	// Token: 0x0600374F RID: 14159 RVA: 0x0012DC04 File Offset: 0x0012BE04
	public new void Clear()
	{
		FieldInfo[] fields = typeof(GameComps).GetFields();
		for (int i = 0; i < fields.Length; i++)
		{
			IComponentManager componentManager = fields[i].GetValue(null) as IComponentManager;
			if (componentManager != null)
			{
				componentManager.Clear();
			}
		}
	}

	// Token: 0x06003750 RID: 14160 RVA: 0x0012DC47 File Offset: 0x0012BE47
	public static void AddKComponentManager(Type kcomponent, IKComponentManager inst)
	{
		GameComps.kcomponentManagers[kcomponent] = inst;
	}

	// Token: 0x06003751 RID: 14161 RVA: 0x0012DC55 File Offset: 0x0012BE55
	public static IKComponentManager GetKComponentManager(Type kcomponent_type)
	{
		return GameComps.kcomponentManagers[kcomponent_type];
	}

	// Token: 0x04002140 RID: 8512
	public static GravityComponents Gravities;

	// Token: 0x04002141 RID: 8513
	public static FallerComponents Fallers;

	// Token: 0x04002142 RID: 8514
	public static InfraredVisualizerComponents InfraredVisualizers;

	// Token: 0x04002143 RID: 8515
	public static ElementSplitterComponents ElementSplitters;

	// Token: 0x04002144 RID: 8516
	public static OreSizeVisualizerComponents OreSizeVisualizers;

	// Token: 0x04002145 RID: 8517
	public static StructureTemperatureComponents StructureTemperatures;

	// Token: 0x04002146 RID: 8518
	public static DiseaseContainers DiseaseContainers;

	// Token: 0x04002147 RID: 8519
	public static RequiresFoundation RequiresFoundations;

	// Token: 0x04002148 RID: 8520
	public static WhiteBoard WhiteBoards;

	// Token: 0x04002149 RID: 8521
	private static Dictionary<Type, IKComponentManager> kcomponentManagers = new Dictionary<Type, IKComponentManager>();
}
