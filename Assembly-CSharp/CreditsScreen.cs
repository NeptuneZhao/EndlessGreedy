using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;

// Token: 0x02000B8E RID: 2958
public class CreditsScreen : KModalScreen
{
	// Token: 0x06005952 RID: 22866 RVA: 0x00205030 File Offset: 0x00203230
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (TextAsset csv in this.creditsFiles)
		{
			this.AddCredits(csv);
		}
		this.CloseButton.onClick += this.Close;
	}

	// Token: 0x06005953 RID: 22867 RVA: 0x0020507A File Offset: 0x0020327A
	public void Close()
	{
		this.Deactivate();
	}

	// Token: 0x06005954 RID: 22868 RVA: 0x00205084 File Offset: 0x00203284
	private void AddCredits(TextAsset csv)
	{
		string[,] array = CSVReader.SplitCsvGrid(csv.text, csv.name);
		List<string> list = new List<string>();
		for (int i = 1; i < array.GetLength(1); i++)
		{
			string text = string.Format("{0} {1}", array[0, i], array[1, i]);
			if (!(text == " "))
			{
				list.Add(text);
			}
		}
		list.Shuffle<string>();
		string text2 = array[0, 0];
		GameObject gameObject = Util.KInstantiateUI(this.teamHeaderPrefab, this.entryContainer.gameObject, true);
		gameObject.GetComponent<LocText>().text = text2;
		this.teamContainers.Add(text2, gameObject);
		foreach (string text3 in list)
		{
			Util.KInstantiateUI(this.entryPrefab, this.teamContainers[text2], true).GetComponent<LocText>().text = text3;
		}
	}

	// Token: 0x04003AB4 RID: 15028
	public GameObject entryPrefab;

	// Token: 0x04003AB5 RID: 15029
	public GameObject teamHeaderPrefab;

	// Token: 0x04003AB6 RID: 15030
	private Dictionary<string, GameObject> teamContainers = new Dictionary<string, GameObject>();

	// Token: 0x04003AB7 RID: 15031
	public Transform entryContainer;

	// Token: 0x04003AB8 RID: 15032
	public KButton CloseButton;

	// Token: 0x04003AB9 RID: 15033
	public TextAsset[] creditsFiles;
}
