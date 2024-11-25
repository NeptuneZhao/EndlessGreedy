using System;
using UnityEngine;

// Token: 0x0200054A RID: 1354
public class CreaturePoopLoot : GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>
{
	// Token: 0x06001F1E RID: 7966 RVA: 0x000AE48C File Offset: 0x000AC68C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EventTransition(GameHashes.Poop, this.roll, null);
		this.roll.Enter(new StateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.State.Callback(CreaturePoopLoot.RollForLoot)).GoTo(this.idle);
	}

	// Token: 0x06001F1F RID: 7967 RVA: 0x000AE4E4 File Offset: 0x000AC6E4
	public static void RollForLoot(CreaturePoopLoot.Instance smi)
	{
		for (int i = 0; i < smi.def.Loot.Length; i++)
		{
			float value = UnityEngine.Random.value;
			CreaturePoopLoot.LootData lootData = smi.def.Loot[i];
			if (lootData.probability > 0f && value <= lootData.probability)
			{
				Tag tag = lootData.tag;
				Vector3 position = smi.transform.position;
				position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				Util.KInstantiate(Assets.GetPrefab(tag), position).SetActive(true);
			}
		}
	}

	// Token: 0x04001189 RID: 4489
	public GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.State idle;

	// Token: 0x0400118A RID: 4490
	public GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.State roll;

	// Token: 0x02001325 RID: 4901
	public struct LootData
	{
		// Token: 0x040065BB RID: 26043
		public Tag tag;

		// Token: 0x040065BC RID: 26044
		public float probability;
	}

	// Token: 0x02001326 RID: 4902
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040065BD RID: 26045
		public CreaturePoopLoot.LootData[] Loot;
	}

	// Token: 0x02001327 RID: 4903
	public new class Instance : GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.GameInstance
	{
		// Token: 0x06008617 RID: 34327 RVA: 0x003285A1 File Offset: 0x003267A1
		public Instance(IStateMachineTarget master, CreaturePoopLoot.Def def) : base(master, def)
		{
		}
	}
}
