using System;
using System.Collections.Generic;
using Klei.Actions;
using UnityEngine;

namespace Klei.Input
{
	// Token: 0x02000F85 RID: 3973
	[CreateAssetMenu(fileName = "InterfaceToolConfig", menuName = "Klei/Interface Tools/Config")]
	public class InterfaceToolConfig : ScriptableObject
	{
		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x060079CB RID: 31179 RVA: 0x00301493 File Offset: 0x002FF693
		public DigAction DigAction
		{
			get
			{
				return ActionFactory<DigToolActionFactory, DigAction, DigToolActionFactory.Actions>.GetOrCreateAction(this.digAction);
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x060079CC RID: 31180 RVA: 0x003014A0 File Offset: 0x002FF6A0
		public int Priority
		{
			get
			{
				return this.priority;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x060079CD RID: 31181 RVA: 0x003014A8 File Offset: 0x002FF6A8
		public global::Action InputAction
		{
			get
			{
				return (global::Action)Enum.Parse(typeof(global::Action), this.inputAction);
			}
		}

		// Token: 0x04005AE6 RID: 23270
		[SerializeField]
		private DigToolActionFactory.Actions digAction;

		// Token: 0x04005AE7 RID: 23271
		public static InterfaceToolConfig.Comparer ConfigComparer = new InterfaceToolConfig.Comparer();

		// Token: 0x04005AE8 RID: 23272
		[SerializeField]
		[Tooltip("Defines which config will take priority should multiple configs be activated\n0 is the lower bound for this value.")]
		private int priority;

		// Token: 0x04005AE9 RID: 23273
		[SerializeField]
		[Tooltip("This will serve as a key for activating different configs. Currently, these Actionsare how we indicate that different input modes are desired.\nAssigning Action.Invalid to this field will indicate that this is the \"default\" config")]
		private string inputAction = global::Action.Invalid.ToString();

		// Token: 0x02002371 RID: 9073
		public class Comparer : IComparer<InterfaceToolConfig>
		{
			// Token: 0x0600B6BC RID: 46780 RVA: 0x003CCC9B File Offset: 0x003CAE9B
			public int Compare(InterfaceToolConfig lhs, InterfaceToolConfig rhs)
			{
				if (lhs.Priority == rhs.Priority)
				{
					return 0;
				}
				if (lhs.Priority <= rhs.Priority)
				{
					return -1;
				}
				return 1;
			}
		}
	}
}
