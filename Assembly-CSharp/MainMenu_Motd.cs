using System;
using System.Linq;
using UnityEngine;

// Token: 0x02000CA0 RID: 3232
[Serializable]
public class MainMenu_Motd
{
	// Token: 0x06006386 RID: 25478 RVA: 0x002516DC File Offset: 0x0024F8DC
	public void Setup()
	{
		this.CleanUp();
		this.boxA.gameObject.SetActive(false);
		this.boxB.gameObject.SetActive(false);
		this.boxC.gameObject.SetActive(false);
		this.motdDataFetchRequest = new MotdDataFetchRequest();
		this.motdDataFetchRequest.Fetch(MotdDataFetchRequest.BuildUrl());
		this.motdDataFetchRequest.OnComplete(delegate(MotdData motdData)
		{
			this.RecieveMotdData(motdData);
		});
	}

	// Token: 0x06006387 RID: 25479 RVA: 0x00251754 File Offset: 0x0024F954
	public void CleanUp()
	{
		if (this.motdDataFetchRequest != null)
		{
			this.motdDataFetchRequest.Dispose();
			this.motdDataFetchRequest = null;
		}
	}

	// Token: 0x06006388 RID: 25480 RVA: 0x00251770 File Offset: 0x0024F970
	private void RecieveMotdData(MotdData motdData)
	{
		MainMenu_Motd.<>c__DisplayClass6_0 CS$<>8__locals1 = new MainMenu_Motd.<>c__DisplayClass6_0();
		CS$<>8__locals1.<>4__this = this;
		if (motdData == null || motdData.boxesLive == null || motdData.boxesLive.Count == 0)
		{
			global::Debug.LogWarning("MOTD Error: failed to get valid motd data, hiding ui.");
			this.boxA.gameObject.SetActive(false);
			this.boxB.gameObject.SetActive(false);
			this.boxC.gameObject.SetActive(false);
			return;
		}
		CS$<>8__locals1.boxes = motdData.boxesLive.StableSort((MotdData_Box a, MotdData_Box b) => CS$<>8__locals1.<>4__this.CalcScore(a).CompareTo(CS$<>8__locals1.<>4__this.CalcScore(b))).ToList<MotdData_Box>();
		MotdData_Box motdData_Box = CS$<>8__locals1.<RecieveMotdData>g__ConsumeBox|1("PatchNotes");
		MotdData_Box motdData_Box2 = CS$<>8__locals1.<RecieveMotdData>g__ConsumeBox|1("News");
		MotdData_Box motdData_Box3 = CS$<>8__locals1.<RecieveMotdData>g__ConsumeBox|1("Skins");
		if (motdData_Box != null)
		{
			this.boxA.Config(new MotdBox.PageData[]
			{
				this.ConvertToPageData(motdData_Box)
			});
			this.boxA.gameObject.SetActive(true);
		}
		if (motdData_Box2 != null)
		{
			this.boxB.Config(new MotdBox.PageData[]
			{
				this.ConvertToPageData(motdData_Box2)
			});
			this.boxB.gameObject.SetActive(true);
		}
		if (motdData_Box3 != null)
		{
			this.boxC.Config(new MotdBox.PageData[]
			{
				this.ConvertToPageData(motdData_Box3)
			});
			this.boxC.gameObject.SetActive(true);
		}
	}

	// Token: 0x06006389 RID: 25481 RVA: 0x002518B3 File Offset: 0x0024FAB3
	private int CalcScore(MotdData_Box box)
	{
		return 0;
	}

	// Token: 0x0600638A RID: 25482 RVA: 0x002518B6 File Offset: 0x0024FAB6
	private MotdBox.PageData ConvertToPageData(MotdData_Box box)
	{
		return new MotdBox.PageData
		{
			Texture = box.resolvedImage,
			HeaderText = box.title,
			ImageText = box.text,
			URL = box.href
		};
	}

	// Token: 0x04004394 RID: 17300
	[SerializeField]
	private MotdBox boxA;

	// Token: 0x04004395 RID: 17301
	[SerializeField]
	private MotdBox boxB;

	// Token: 0x04004396 RID: 17302
	[SerializeField]
	private MotdBox boxC;

	// Token: 0x04004397 RID: 17303
	private MotdDataFetchRequest motdDataFetchRequest;
}
