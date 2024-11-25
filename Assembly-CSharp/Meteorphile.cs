using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x0200095D RID: 2397
[SkipSaveFileSerialization]
public class Meteorphile : StateMachineComponent<Meteorphile.StatesInstance>
{
	// Token: 0x06004604 RID: 17924 RVA: 0x0018EC80 File Offset: 0x0018CE80
	protected override void OnSpawn()
	{
		this.attributeModifiers = new AttributeModifier[]
		{
			new AttributeModifier("Construction", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Digging", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Machinery", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Athletics", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Learning", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Cooking", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Art", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Strength", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Caring", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Botanist", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Ranching", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true)
		};
		base.smi.StartSM();
	}

	// Token: 0x06004605 RID: 17925 RVA: 0x0018EDFC File Offset: 0x0018CFFC
	public void ApplyModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Add(modifier);
		}
	}

	// Token: 0x06004606 RID: 17926 RVA: 0x0018EE38 File Offset: 0x0018D038
	public void RemoveModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Remove(modifier);
		}
	}

	// Token: 0x04002D8A RID: 11658
	[MyCmpReq]
	private KPrefabID kPrefabID;

	// Token: 0x04002D8B RID: 11659
	private AttributeModifier[] attributeModifiers;

	// Token: 0x020018C9 RID: 6345
	public class StatesInstance : GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.GameInstance
	{
		// Token: 0x060099CF RID: 39375 RVA: 0x0036B5BA File Offset: 0x003697BA
		public StatesInstance(Meteorphile master) : base(master)
		{
		}

		// Token: 0x060099D0 RID: 39376 RVA: 0x0036B5C4 File Offset: 0x003697C4
		public bool IsMeteors()
		{
			if (GameplayEventManager.Instance == null || base.master.kPrefabID.PrefabTag == GameTags.MinionSelectPreview)
			{
				return false;
			}
			int myWorldId = this.GetMyWorldId();
			List<GameplayEventInstance> list = new List<GameplayEventInstance>();
			GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(myWorldId, ref list);
			for (int i = 0; i < list.Count; i++)
			{
				MeteorShowerEvent.StatesInstance statesInstance = list[i].smi as MeteorShowerEvent.StatesInstance;
				if (statesInstance != null && statesInstance.IsInsideState(statesInstance.sm.running.bombarding))
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x020018CA RID: 6346
	public class States : GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile>
	{
		// Token: 0x060099D1 RID: 39377 RVA: 0x0036B658 File Offset: 0x00369858
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Transition(this.early, (Meteorphile.StatesInstance smi) => smi.IsMeteors(), UpdateRate.SIM_200ms);
			this.early.Enter("Meteors", delegate(Meteorphile.StatesInstance smi)
			{
				smi.master.ApplyModifiers();
			}).Exit("NotMeteors", delegate(Meteorphile.StatesInstance smi)
			{
				smi.master.RemoveModifiers();
			}).ToggleStatusItem(Db.Get().DuplicantStatusItems.Meteorphile, null).ToggleExpression(Db.Get().Expressions.Happy, null).Transition(this.idle, (Meteorphile.StatesInstance smi) => !smi.IsMeteors(), UpdateRate.SIM_200ms);
		}

		// Token: 0x04007767 RID: 30567
		public GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.State idle;

		// Token: 0x04007768 RID: 30568
		public GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.State early;
	}
}
