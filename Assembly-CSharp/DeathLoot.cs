using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000830 RID: 2096
public class DeathLoot : GameStateMachine<DeathLoot, DeathLoot.Instance, IStateMachineTarget, DeathLoot.Def>
{
	// Token: 0x06003A43 RID: 14915 RVA: 0x0013E470 File Offset: 0x0013C670
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	// Token: 0x04002305 RID: 8965
	private StateMachine<DeathLoot, DeathLoot.Instance, IStateMachineTarget, DeathLoot.Def>.BoolParameter WasLoopDropped;

	// Token: 0x02001751 RID: 5969
	public class Loot
	{
		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x0600954C RID: 38220 RVA: 0x0035F486 File Offset: 0x0035D686
		// (set) Token: 0x0600954B RID: 38219 RVA: 0x0035F47D File Offset: 0x0035D67D
		public Tag Id { get; private set; } = Tag.Invalid;

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x0600954E RID: 38222 RVA: 0x0035F497 File Offset: 0x0035D697
		// (set) Token: 0x0600954D RID: 38221 RVA: 0x0035F48E File Offset: 0x0035D68E
		public bool IsElement { get; private set; }

		// Token: 0x0600954F RID: 38223 RVA: 0x0035F49F File Offset: 0x0035D69F
		public Loot(Tag tag)
		{
			this.Id = tag;
			this.IsElement = false;
			this.Quantity = 1f;
		}

		// Token: 0x06009550 RID: 38224 RVA: 0x0035F4CB File Offset: 0x0035D6CB
		public Loot(SimHashes element, float quantity)
		{
			this.Id = element.CreateTag();
			this.IsElement = true;
			this.Quantity = quantity;
		}

		// Token: 0x04007271 RID: 29297
		public float Quantity;
	}

	// Token: 0x02001752 RID: 5970
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007272 RID: 29298
		public DeathLoot.Loot[] loot;

		// Token: 0x04007273 RID: 29299
		public CellOffset lootSpawnOffset;
	}

	// Token: 0x02001753 RID: 5971
	public new class Instance : GameStateMachine<DeathLoot, DeathLoot.Instance, IStateMachineTarget, DeathLoot.Def>.GameInstance
	{
		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06009552 RID: 38226 RVA: 0x0035F500 File Offset: 0x0035D700
		public bool WasLoopDropped
		{
			get
			{
				return base.sm.WasLoopDropped.Get(base.smi);
			}
		}

		// Token: 0x06009553 RID: 38227 RVA: 0x0035F518 File Offset: 0x0035D718
		public Instance(IStateMachineTarget master, DeathLoot.Def def) : base(master, def)
		{
			base.Subscribe(1623392196, new Action<object>(this.OnDeath));
		}

		// Token: 0x06009554 RID: 38228 RVA: 0x0035F539 File Offset: 0x0035D739
		private void OnDeath(object obj)
		{
			if (!this.WasLoopDropped)
			{
				base.sm.WasLoopDropped.Set(true, this, false);
				this.CreateLoot();
			}
		}

		// Token: 0x06009555 RID: 38229 RVA: 0x0035F560 File Offset: 0x0035D760
		public GameObject[] CreateLoot()
		{
			if (base.def.loot == null)
			{
				return null;
			}
			GameObject[] array = new GameObject[base.def.loot.Length];
			for (int i = 0; i < base.def.loot.Length; i++)
			{
				DeathLoot.Loot loot = base.def.loot[i];
				if (!(loot.Id == Tag.Invalid))
				{
					GameObject gameObject = Scenario.SpawnPrefab(this.GetLootSpawnCell(), 0, 0, loot.Id.ToString(), Grid.SceneLayer.Ore);
					gameObject.SetActive(true);
					Edible component = gameObject.GetComponent<Edible>();
					if (component)
					{
						ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.BUTCHERED, "{0}", gameObject.GetProperName()), UI.ENDOFDAYREPORT.NOTES.BUTCHERED_CONTEXT);
					}
					if (loot.IsElement)
					{
						PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
						if (component2 != null)
						{
							component2.Mass = loot.Quantity;
						}
					}
					array[i] = gameObject;
				}
			}
			return array;
		}

		// Token: 0x06009556 RID: 38230 RVA: 0x0035F670 File Offset: 0x0035D870
		public int GetLootSpawnCell()
		{
			int num = Grid.PosToCell(base.gameObject);
			int num2 = Grid.OffsetCell(num, base.def.lootSpawnOffset);
			if (Grid.IsWorldValidCell(num2) && Grid.IsValidCellInWorld(num2, base.gameObject.GetMyWorldId()))
			{
				return num2;
			}
			return num;
		}

		// Token: 0x06009557 RID: 38231 RVA: 0x0035F6B9 File Offset: 0x0035D8B9
		protected override void OnCleanUp()
		{
			base.Unsubscribe(1623392196, new Action<object>(this.OnDeath));
		}
	}
}
