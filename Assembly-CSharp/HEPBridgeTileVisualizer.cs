using System;

// Token: 0x020006EA RID: 1770
public class HEPBridgeTileVisualizer : KMonoBehaviour, IHighEnergyParticleDirection
{
	// Token: 0x06002D0C RID: 11532 RVA: 0x000FD338 File Offset: 0x000FB538
	protected override void OnSpawn()
	{
		base.Subscribe<HEPBridgeTileVisualizer>(-1643076535, HEPBridgeTileVisualizer.OnRotateDelegate);
		this.OnRotate();
	}

	// Token: 0x06002D0D RID: 11533 RVA: 0x000FD351 File Offset: 0x000FB551
	public void OnRotate()
	{
		Game.Instance.ForceOverlayUpdate(true);
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06002D0E RID: 11534 RVA: 0x000FD360 File Offset: 0x000FB560
	// (set) Token: 0x06002D0F RID: 11535 RVA: 0x000FD3AD File Offset: 0x000FB5AD
	public EightDirection Direction
	{
		get
		{
			EightDirection result = EightDirection.Right;
			Rotatable component = base.GetComponent<Rotatable>();
			if (component != null)
			{
				switch (component.Orientation)
				{
				case Orientation.Neutral:
					result = EightDirection.Left;
					break;
				case Orientation.R90:
					result = EightDirection.Up;
					break;
				case Orientation.R180:
					result = EightDirection.Right;
					break;
				case Orientation.R270:
					result = EightDirection.Down;
					break;
				}
			}
			return result;
		}
		set
		{
		}
	}

	// Token: 0x04001A02 RID: 6658
	private static readonly EventSystem.IntraObjectHandler<HEPBridgeTileVisualizer> OnRotateDelegate = new EventSystem.IntraObjectHandler<HEPBridgeTileVisualizer>(delegate(HEPBridgeTileVisualizer component, object data)
	{
		component.OnRotate();
	});
}
