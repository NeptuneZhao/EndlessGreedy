using System;
using System.Collections.Generic;

namespace Klei.Actions
{
	// Token: 0x02000F8C RID: 3980
	public class ActionFactory<ActionFactoryType, ActionType, EnumType> where ActionFactoryType : ActionFactory<ActionFactoryType, ActionType, EnumType>
	{
		// Token: 0x060079E3 RID: 31203 RVA: 0x003016C8 File Offset: 0x002FF8C8
		public static ActionType GetOrCreateAction(EnumType actionType)
		{
			ActionType result;
			if (!ActionFactory<ActionFactoryType, ActionType, EnumType>.actionInstances.TryGetValue(actionType, out result))
			{
				ActionFactory<ActionFactoryType, ActionType, EnumType>.EnsureFactoryInstance();
				result = (ActionFactory<ActionFactoryType, ActionType, EnumType>.actionInstances[actionType] = ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory.CreateAction(actionType));
			}
			return result;
		}

		// Token: 0x060079E4 RID: 31204 RVA: 0x00301707 File Offset: 0x002FF907
		private static void EnsureFactoryInstance()
		{
			if (ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory != null)
			{
				return;
			}
			ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory = (Activator.CreateInstance(typeof(ActionFactoryType)) as ActionFactoryType);
		}

		// Token: 0x060079E5 RID: 31205 RVA: 0x00301734 File Offset: 0x002FF934
		protected virtual ActionType CreateAction(EnumType actionType)
		{
			throw new InvalidOperationException("Can not call InterfaceToolActionFactory<T1, T2>.CreateAction()! This function must be called from a deriving class!");
		}

		// Token: 0x04005AEE RID: 23278
		private static Dictionary<EnumType, ActionType> actionInstances = new Dictionary<EnumType, ActionType>();

		// Token: 0x04005AEF RID: 23279
		private static ActionFactoryType actionFactory = default(ActionFactoryType);
	}
}
