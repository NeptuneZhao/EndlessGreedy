using System;
using KSerialization;
using STRINGS;

// Token: 0x020001D7 RID: 471
public class FossilBits : FossilExcavationWorkable, ISidescreenButtonControl
{
	// Token: 0x0600099A RID: 2458 RVA: 0x0003937A File Offset: 0x0003757A
	protected override bool IsMarkedForExcavation()
	{
		return this.MarkedForDig;
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x00039382 File Offset: 0x00037582
	public void SetEntombStatusItemVisibility(bool visible)
	{
		this.entombComponent.SetShowStatusItemOnEntombed(visible);
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x00039390 File Offset: 0x00037590
	public void CreateWorkableChore()
	{
		if (this.chore == null && this.operational.IsOperational)
		{
			this.chore = new WorkChore<FossilBits>(Db.Get().ChoreTypes.ExcavateFossil, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x000393DE File Offset: 0x000375DE
	public void CancelWorkChore()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("FossilBits.CancelChore");
			this.chore = null;
		}
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x00039400 File Offset: 0x00037600
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sculpture_kanim")
		};
		base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
		base.SetWorkTime(30f);
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x00039454 File Offset: 0x00037654
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SetEntombStatusItemVisibility(this.MarkedForDig);
		base.SetShouldShowSkillPerkStatusItem(this.IsMarkedForExcavation());
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x00039474 File Offset: 0x00037674
	private void OnOperationalChanged(object state)
	{
		if ((bool)state)
		{
			if (this.MarkedForDig)
			{
				this.CreateWorkableChore();
				return;
			}
		}
		else if (this.MarkedForDig)
		{
			this.CancelWorkChore();
		}
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x0003949C File Offset: 0x0003769C
	private void DropLoot()
	{
		PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Element element = ElementLoader.GetElement(component.Element.tag);
		if (element != null)
		{
			float num = component.Mass;
			int num2 = 0;
			while ((float)num2 < component.Mass / 400f)
			{
				float num3 = num;
				if (num > 400f)
				{
					num3 = 400f;
					num -= 400f;
				}
				int disease_count = (int)((float)component.DiseaseCount * (num3 / component.Mass));
				element.substance.SpawnResource(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore), num3, component.Temperature, component.DiseaseIdx, disease_count, false, false, false);
				num2++;
			}
		}
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x00039552 File Offset: 0x00037752
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.DropLoot();
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x0003956C File Offset: 0x0003776C
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x060009A4 RID: 2468 RVA: 0x0003956F File Offset: 0x0003776F
	public string SidescreenButtonText
	{
		get
		{
			if (!this.MarkedForDig)
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_EXCAVATE_BUTTON;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON;
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x060009A5 RID: 2469 RVA: 0x0003958E File Offset: 0x0003778E
	public string SidescreenButtonTooltip
	{
		get
		{
			if (!this.MarkedForDig)
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_EXCAVATE_BUTTON_TOOLTIP;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON_TOOLTIP;
		}
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x000395AD File Offset: 0x000377AD
	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x000395B4 File Offset: 0x000377B4
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x000395B7 File Offset: 0x000377B7
	public bool SidescreenButtonInteractable()
	{
		return true;
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x000395BC File Offset: 0x000377BC
	public void OnSidescreenButtonPressed()
	{
		this.MarkedForDig = !this.MarkedForDig;
		base.SetShouldShowSkillPerkStatusItem(this.MarkedForDig);
		this.SetEntombStatusItemVisibility(this.MarkedForDig);
		if (this.MarkedForDig)
		{
			this.CreateWorkableChore();
		}
		else
		{
			this.CancelWorkChore();
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x0003960D File Offset: 0x0003780D
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x04000665 RID: 1637
	[Serialize]
	public bool MarkedForDig;

	// Token: 0x04000666 RID: 1638
	private Chore chore;

	// Token: 0x04000667 RID: 1639
	[MyCmpGet]
	private EntombVulnerable entombComponent;

	// Token: 0x04000668 RID: 1640
	[MyCmpGet]
	private Operational operational;
}
