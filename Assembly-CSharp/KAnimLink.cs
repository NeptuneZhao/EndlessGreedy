using System;
using UnityEngine;

// Token: 0x020004E1 RID: 1249
public class KAnimLink
{
	// Token: 0x06001B6B RID: 7019 RVA: 0x0008F93B File Offset: 0x0008DB3B
	public KAnimLink(KAnimControllerBase master, KAnimControllerBase slave)
	{
		this.slave = slave;
		this.master = master;
		this.Register();
	}

	// Token: 0x06001B6C RID: 7020 RVA: 0x0008F960 File Offset: 0x0008DB60
	private void Register()
	{
		this.master.OnOverlayColourChanged += this.OnOverlayColourChanged;
		KAnimControllerBase kanimControllerBase = this.master;
		kanimControllerBase.OnTintChanged = (Action<Color>)Delegate.Combine(kanimControllerBase.OnTintChanged, new Action<Color>(this.OnTintColourChanged));
		KAnimControllerBase kanimControllerBase2 = this.master;
		kanimControllerBase2.OnHighlightChanged = (Action<Color>)Delegate.Combine(kanimControllerBase2.OnHighlightChanged, new Action<Color>(this.OnHighlightColourChanged));
		this.master.onLayerChanged += this.slave.SetLayer;
	}

	// Token: 0x06001B6D RID: 7021 RVA: 0x0008F9F0 File Offset: 0x0008DBF0
	public void Unregister()
	{
		if (this.master != null)
		{
			this.master.OnOverlayColourChanged -= this.OnOverlayColourChanged;
			KAnimControllerBase kanimControllerBase = this.master;
			kanimControllerBase.OnTintChanged = (Action<Color>)Delegate.Remove(kanimControllerBase.OnTintChanged, new Action<Color>(this.OnTintColourChanged));
			KAnimControllerBase kanimControllerBase2 = this.master;
			kanimControllerBase2.OnHighlightChanged = (Action<Color>)Delegate.Remove(kanimControllerBase2.OnHighlightChanged, new Action<Color>(this.OnHighlightColourChanged));
			if (this.slave != null)
			{
				this.master.onLayerChanged -= this.slave.SetLayer;
			}
		}
	}

	// Token: 0x06001B6E RID: 7022 RVA: 0x0008FA9E File Offset: 0x0008DC9E
	private void OnOverlayColourChanged(Color32 c)
	{
		if (this.slave != null)
		{
			this.slave.OverlayColour = c;
		}
	}

	// Token: 0x06001B6F RID: 7023 RVA: 0x0008FABF File Offset: 0x0008DCBF
	private void OnTintColourChanged(Color c)
	{
		if (this.syncTint && this.slave != null)
		{
			this.slave.TintColour = c;
		}
	}

	// Token: 0x06001B70 RID: 7024 RVA: 0x0008FAE8 File Offset: 0x0008DCE8
	private void OnHighlightColourChanged(Color c)
	{
		if (this.slave != null)
		{
			this.slave.HighlightColour = c;
		}
	}

	// Token: 0x04000F81 RID: 3969
	public bool syncTint = true;

	// Token: 0x04000F82 RID: 3970
	private KAnimControllerBase master;

	// Token: 0x04000F83 RID: 3971
	private KAnimControllerBase slave;
}
