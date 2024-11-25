using System;
using UnityEngine;

// Token: 0x020007A6 RID: 1958
public class CargoDropperStorage : GameStateMachine<CargoDropperStorage, CargoDropperStorage.StatesInstance, IStateMachineTarget, CargoDropperStorage.Def>
{
	// Token: 0x06003591 RID: 13713 RVA: 0x0012355D File Offset: 0x0012175D
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.JettisonCargo, delegate(CargoDropperStorage.StatesInstance smi, object data)
		{
			smi.JettisonCargo(data);
		});
	}

	// Token: 0x0200165E RID: 5726
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006F79 RID: 28537
		public Vector3 dropOffset;
	}

	// Token: 0x0200165F RID: 5727
	public class StatesInstance : GameStateMachine<CargoDropperStorage, CargoDropperStorage.StatesInstance, IStateMachineTarget, CargoDropperStorage.Def>.GameInstance
	{
		// Token: 0x060091FE RID: 37374 RVA: 0x00352AB4 File Offset: 0x00350CB4
		public StatesInstance(IStateMachineTarget master, CargoDropperStorage.Def def) : base(master, def)
		{
		}

		// Token: 0x060091FF RID: 37375 RVA: 0x00352AC0 File Offset: 0x00350CC0
		public void JettisonCargo(object data)
		{
			Vector3 position = base.master.transform.GetPosition() + base.def.dropOffset;
			Storage component = base.GetComponent<Storage>();
			if (component != null)
			{
				GameObject gameObject = component.FindFirst("ScoutRover");
				if (gameObject != null)
				{
					component.Drop(gameObject, true);
					Vector3 position2 = base.master.transform.GetPosition();
					position2.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
					gameObject.transform.SetPosition(position2);
					ChoreProvider component2 = gameObject.GetComponent<ChoreProvider>();
					if (component2 != null)
					{
						KBatchedAnimController component3 = gameObject.GetComponent<KBatchedAnimController>();
						if (component3 != null)
						{
							component3.Play("enter", KAnim.PlayMode.Once, 1f, 0f);
						}
						new EmoteChore(component2, Db.Get().ChoreTypes.EmoteHighPriority, null, new HashedString[]
						{
							"enter"
						}, KAnim.PlayMode.Once, false);
					}
					gameObject.GetMyWorld().SetRoverLanded();
				}
				component.DropAll(position, false, false, default(Vector3), true, null);
			}
		}
	}
}
