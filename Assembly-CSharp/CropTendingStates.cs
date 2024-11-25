using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020000C9 RID: 201
public class CropTendingStates : GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>
{
	// Token: 0x060003A5 RID: 933 RVA: 0x0001E170 File Offset: 0x0001C370
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.findCrop;
		this.root.Exit(delegate(CropTendingStates.Instance smi)
		{
			this.UnreserveCrop(smi);
			if (!smi.tendedSucceeded)
			{
				this.RestoreSymbolsVisibility(smi);
			}
		});
		this.findCrop.Enter(delegate(CropTendingStates.Instance smi)
		{
			this.FindCrop(smi);
			if (smi.sm.targetCrop.Get(smi) == null)
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			this.ReserverCrop(smi);
			smi.GoTo(this.moveToCrop);
		});
		GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State state = this.moveToCrop;
		string name = CREATURES.STATUSITEMS.DIVERGENT_WILL_TEND.NAME;
		string tooltip = CREATURES.STATUSITEMS.DIVERGENT_WILL_TEND.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).MoveTo((CropTendingStates.Instance smi) => smi.moveCell, this.tendCrop, this.behaviourcomplete, false).ParamTransition<GameObject>(this.targetCrop, this.behaviourcomplete, (CropTendingStates.Instance smi, GameObject p) => this.targetCrop.Get(smi) == null);
		GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State state2 = this.tendCrop.DefaultState(this.tendCrop.pre);
		string name2 = CREATURES.STATUSITEMS.DIVERGENT_TENDING.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.DIVERGENT_TENDING.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main).ParamTransition<GameObject>(this.targetCrop, this.behaviourcomplete, (CropTendingStates.Instance smi, GameObject p) => this.targetCrop.Get(smi) == null).Enter(delegate(CropTendingStates.Instance smi)
		{
			smi.animSet = this.GetCropTendingAnimSet(smi);
			this.StoreSymbolsVisibility(smi);
		});
		this.tendCrop.pre.Face(this.targetCrop, 0f).PlayAnim((CropTendingStates.Instance smi) => smi.animSet.crop_tending_pre, KAnim.PlayMode.Once).OnAnimQueueComplete(this.tendCrop.tend);
		this.tendCrop.tend.Enter(delegate(CropTendingStates.Instance smi)
		{
			this.SetSymbolsVisibility(smi, false);
		}).QueueAnim((CropTendingStates.Instance smi) => smi.animSet.crop_tending, false, null).OnAnimQueueComplete(this.tendCrop.pst);
		this.tendCrop.pst.QueueAnim((CropTendingStates.Instance smi) => smi.animSet.crop_tending_pst, false, null).OnAnimQueueComplete(this.behaviourcomplete).Exit(delegate(CropTendingStates.Instance smi)
		{
			GameObject gameObject = smi.sm.targetCrop.Get(smi);
			if (gameObject != null)
			{
				if (smi.effect != null)
				{
					gameObject.GetComponent<Effects>().Add(smi.effect, true);
				}
				smi.tendedSucceeded = true;
				CropTendingStates.CropTendingEventData data = new CropTendingStates.CropTendingEventData
				{
					source = smi.gameObject,
					cropId = smi.sm.targetCrop.Get(smi).PrefabID()
				};
				smi.sm.targetCrop.Get(smi).Trigger(90606262, data);
				smi.Trigger(90606262, data);
			}
		});
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToTendCrops, false);
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x0001E3EC File Offset: 0x0001C5EC
	private CropTendingStates.AnimSet GetCropTendingAnimSet(CropTendingStates.Instance smi)
	{
		CropTendingStates.AnimSet result;
		if (smi.def.animSetOverrides.TryGetValue(this.targetCrop.Get(smi).PrefabID(), out result))
		{
			return result;
		}
		return CropTendingStates.defaultAnimSet;
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x0001E428 File Offset: 0x0001C628
	private void FindCrop(CropTendingStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		Crop crop = null;
		int moveCell = Grid.InvalidCell;
		int num = 100;
		int num2 = -1;
		foreach (Crop crop2 in Components.Crops.GetWorldItems(smi.gameObject.GetMyWorldId(), false))
		{
			if (Vector2.SqrMagnitude(crop2.transform.position - smi.transform.position) <= 625f)
			{
				if (smi.effect != null)
				{
					Effects component2 = crop2.GetComponent<Effects>();
					if (component2 != null)
					{
						bool flag = false;
						foreach (string effect_id in smi.def.ignoreEffectGroup)
						{
							if (component2.HasEffect(effect_id))
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							continue;
						}
					}
				}
				Growing component3 = crop2.GetComponent<Growing>();
				if (!(component3 != null) || !component3.IsGrown())
				{
					KPrefabID component4 = crop2.GetComponent<KPrefabID>();
					if (!component4.HasTag(GameTags.Creatures.ReservedByCreature))
					{
						int num3;
						smi.def.interests.TryGetValue(crop2.PrefabID(), out num3);
						if (num3 >= num2)
						{
							bool flag2 = num3 > num2;
							int cell = Grid.PosToCell(crop2);
							int[] array = new int[]
							{
								Grid.CellLeft(cell),
								Grid.CellRight(cell)
							};
							if (component4.HasTag(GameTags.PlantedOnFloorVessel))
							{
								array = new int[]
								{
									Grid.CellLeft(cell),
									Grid.CellRight(cell),
									Grid.CellDownLeft(cell),
									Grid.CellDownRight(cell)
								};
							}
							int num4 = 100;
							int num5 = Grid.InvalidCell;
							for (int j = 0; j < array.Length; j++)
							{
								if (Grid.IsValidCell(array[j]))
								{
									int navigationCost = component.GetNavigationCost(array[j]);
									if (navigationCost != -1 && navigationCost < num4)
									{
										num4 = navigationCost;
										num5 = array[j];
									}
								}
							}
							if (num4 != -1 && num5 != Grid.InvalidCell && (flag2 || num4 < num))
							{
								moveCell = num5;
								num = num4;
								num2 = num3;
								crop = crop2;
							}
						}
					}
				}
			}
		}
		GameObject value = (crop != null) ? crop.gameObject : null;
		smi.sm.targetCrop.Set(value, smi, false);
		smi.moveCell = moveCell;
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x0001E6A4 File Offset: 0x0001C8A4
	private void ReserverCrop(CropTendingStates.Instance smi)
	{
		GameObject gameObject = smi.sm.targetCrop.Get(smi);
		if (gameObject != null)
		{
			DebugUtil.Assert(!gameObject.HasTag(GameTags.Creatures.ReservedByCreature));
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x0001E6EC File Offset: 0x0001C8EC
	private void UnreserveCrop(CropTendingStates.Instance smi)
	{
		GameObject gameObject = smi.sm.targetCrop.Get(smi);
		if (gameObject != null)
		{
			gameObject.RemoveTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x060003AA RID: 938 RVA: 0x0001E720 File Offset: 0x0001C920
	private void SetSymbolsVisibility(CropTendingStates.Instance smi, bool isVisible)
	{
		if (this.targetCrop.Get(smi) != null)
		{
			string[] hide_symbols_after_pre = smi.animSet.hide_symbols_after_pre;
			if (hide_symbols_after_pre != null)
			{
				KAnimControllerBase component = this.targetCrop.Get(smi).GetComponent<KAnimControllerBase>();
				if (component != null)
				{
					foreach (string str in hide_symbols_after_pre)
					{
						component.SetSymbolVisiblity(str, isVisible);
					}
				}
			}
		}
	}

	// Token: 0x060003AB RID: 939 RVA: 0x0001E790 File Offset: 0x0001C990
	private void StoreSymbolsVisibility(CropTendingStates.Instance smi)
	{
		if (this.targetCrop.Get(smi) != null)
		{
			string[] hide_symbols_after_pre = smi.animSet.hide_symbols_after_pre;
			if (hide_symbols_after_pre != null)
			{
				KAnimControllerBase component = this.targetCrop.Get(smi).GetComponent<KAnimControllerBase>();
				if (component != null)
				{
					smi.symbolStates = new bool[hide_symbols_after_pre.Length];
					for (int i = 0; i < hide_symbols_after_pre.Length; i++)
					{
						smi.symbolStates[i] = component.GetSymbolVisiblity(hide_symbols_after_pre[i]);
					}
				}
			}
		}
	}

	// Token: 0x060003AC RID: 940 RVA: 0x0001E810 File Offset: 0x0001CA10
	private void RestoreSymbolsVisibility(CropTendingStates.Instance smi)
	{
		if (this.targetCrop.Get(smi) != null && smi.symbolStates != null)
		{
			string[] hide_symbols_after_pre = smi.animSet.hide_symbols_after_pre;
			if (hide_symbols_after_pre != null)
			{
				KAnimControllerBase component = this.targetCrop.Get(smi).GetComponent<KAnimControllerBase>();
				if (component != null)
				{
					for (int i = 0; i < hide_symbols_after_pre.Length; i++)
					{
						component.SetSymbolVisiblity(hide_symbols_after_pre[i], smi.symbolStates[i]);
					}
				}
			}
		}
	}

	// Token: 0x04000284 RID: 644
	private const int MAX_NAVIGATE_DISTANCE = 100;

	// Token: 0x04000285 RID: 645
	private const int MAX_SQR_EUCLIDEAN_DISTANCE = 625;

	// Token: 0x04000286 RID: 646
	private static CropTendingStates.AnimSet defaultAnimSet = new CropTendingStates.AnimSet
	{
		crop_tending_pre = "crop_tending_pre",
		crop_tending = "crop_tending_loop",
		crop_tending_pst = "crop_tending_pst"
	};

	// Token: 0x04000287 RID: 647
	public StateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.TargetParameter targetCrop;

	// Token: 0x04000288 RID: 648
	private GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State findCrop;

	// Token: 0x04000289 RID: 649
	private GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State moveToCrop;

	// Token: 0x0400028A RID: 650
	private CropTendingStates.TendingStates tendCrop;

	// Token: 0x0400028B RID: 651
	private GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State behaviourcomplete;

	// Token: 0x0200101C RID: 4124
	public class AnimSet
	{
		// Token: 0x04005C38 RID: 23608
		public string crop_tending_pre;

		// Token: 0x04005C39 RID: 23609
		public string crop_tending;

		// Token: 0x04005C3A RID: 23610
		public string crop_tending_pst;

		// Token: 0x04005C3B RID: 23611
		public string[] hide_symbols_after_pre;
	}

	// Token: 0x0200101D RID: 4125
	public class CropTendingEventData
	{
		// Token: 0x04005C3C RID: 23612
		public GameObject source;

		// Token: 0x04005C3D RID: 23613
		public Tag cropId;
	}

	// Token: 0x0200101E RID: 4126
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005C3E RID: 23614
		public string effectId;

		// Token: 0x04005C3F RID: 23615
		public string[] ignoreEffectGroup;

		// Token: 0x04005C40 RID: 23616
		public Dictionary<Tag, int> interests = new Dictionary<Tag, int>();

		// Token: 0x04005C41 RID: 23617
		public Dictionary<Tag, CropTendingStates.AnimSet> animSetOverrides = new Dictionary<Tag, CropTendingStates.AnimSet>();
	}

	// Token: 0x0200101F RID: 4127
	public new class Instance : GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.GameInstance
	{
		// Token: 0x06007B45 RID: 31557 RVA: 0x0030347C File Offset: 0x0030167C
		public Instance(Chore<CropTendingStates.Instance> chore, CropTendingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToTendCrops);
			this.effect = Db.Get().effects.TryGet(base.smi.def.effectId);
		}

		// Token: 0x04005C42 RID: 23618
		public Effect effect;

		// Token: 0x04005C43 RID: 23619
		public int moveCell;

		// Token: 0x04005C44 RID: 23620
		public CropTendingStates.AnimSet animSet;

		// Token: 0x04005C45 RID: 23621
		public bool tendedSucceeded;

		// Token: 0x04005C46 RID: 23622
		public bool[] symbolStates;
	}

	// Token: 0x02001020 RID: 4128
	public class TendingStates : GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State
	{
		// Token: 0x04005C47 RID: 23623
		public GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State pre;

		// Token: 0x04005C48 RID: 23624
		public GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State tend;

		// Token: 0x04005C49 RID: 23625
		public GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State pst;
	}
}
