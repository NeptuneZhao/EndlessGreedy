using System;
using System.IO;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000B84 RID: 2948
[AddComponentMenu("KMonoBehaviour/scripts/BaseNaming")]
public class BaseNaming : KMonoBehaviour
{
	// Token: 0x0600589C RID: 22684 RVA: 0x001FF264 File Offset: 0x001FD464
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.GenerateBaseName();
		this.shuffleBaseNameButton.onClick += this.GenerateBaseName;
		this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		this.inputField.onValueChanged.AddListener(new UnityAction<string>(this.OnEditing));
		this.minionSelectScreen = base.GetComponent<MinionSelectScreen>();
	}

	// Token: 0x0600589D RID: 22685 RVA: 0x001FF2D8 File Offset: 0x001FD4D8
	private bool CheckBaseName(string newName)
	{
		if (string.IsNullOrEmpty(newName))
		{
			return true;
		}
		string savePrefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
		string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
		if (this.minionSelectScreen != null)
		{
			bool flag = false;
			try
			{
				bool flag2 = Directory.Exists(Path.Combine(savePrefixAndCreateFolder, newName));
				bool flag3 = cloudSavePrefix != null && Directory.Exists(Path.Combine(cloudSavePrefix, newName));
				flag = (flag2 || flag3);
			}
			catch (Exception arg)
			{
				flag = true;
				global::Debug.Log(string.Format("Base Naming / Warning / {0}", arg));
			}
			if (flag)
			{
				this.minionSelectScreen.SetProceedButtonActive(false, string.Format(UI.IMMIGRANTSCREEN.DUPLICATE_COLONY_NAME, newName));
				return false;
			}
			this.minionSelectScreen.SetProceedButtonActive(true, null);
		}
		return true;
	}

	// Token: 0x0600589E RID: 22686 RVA: 0x001FF388 File Offset: 0x001FD588
	private void OnEditing(string newName)
	{
		Util.ScrubInputField(this.inputField, false, false);
		this.CheckBaseName(this.inputField.text);
	}

	// Token: 0x0600589F RID: 22687 RVA: 0x001FF3AC File Offset: 0x001FD5AC
	private void OnEndEdit(string newName)
	{
		if (Localization.HasDirtyWords(newName))
		{
			this.inputField.text = this.GenerateBaseNameString();
			newName = this.inputField.text;
		}
		if (string.IsNullOrEmpty(newName))
		{
			return;
		}
		if (newName.EndsWith(" "))
		{
			newName = newName.TrimEnd(' ');
		}
		if (!this.CheckBaseName(newName))
		{
			return;
		}
		this.inputField.text = newName;
		SaveGame.Instance.SetBaseName(newName);
		string path = Path.ChangeExtension(newName, ".sav");
		string savePrefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
		string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
		string path2 = savePrefixAndCreateFolder;
		if (SaveLoader.GetCloudSavesAvailable() && Game.Instance.SaveToCloudActive && cloudSavePrefix != null)
		{
			path2 = cloudSavePrefix;
		}
		SaveLoader.SetActiveSaveFilePath(Path.Combine(path2, newName, path));
	}

	// Token: 0x060058A0 RID: 22688 RVA: 0x001FF460 File Offset: 0x001FD660
	private void GenerateBaseName()
	{
		string text = this.GenerateBaseNameString();
		((LocText)this.inputField.placeholder).text = text;
		this.inputField.text = text;
		this.OnEndEdit(text);
	}

	// Token: 0x060058A1 RID: 22689 RVA: 0x001FF4A0 File Offset: 0x001FD6A0
	private string GenerateBaseNameString()
	{
		string fullString = LocString.GetStrings(typeof(NAMEGEN.COLONY.FORMATS)).GetRandom<string>();
		fullString = this.ReplaceStringWithRandom(fullString, "{noun}", LocString.GetStrings(typeof(NAMEGEN.COLONY.NOUN)));
		string[] strings = LocString.GetStrings(typeof(NAMEGEN.COLONY.ADJECTIVE));
		fullString = this.ReplaceStringWithRandom(fullString, "{adjective}", strings);
		fullString = this.ReplaceStringWithRandom(fullString, "{adjective2}", strings);
		fullString = this.ReplaceStringWithRandom(fullString, "{adjective3}", strings);
		return this.ReplaceStringWithRandom(fullString, "{adjective4}", strings);
	}

	// Token: 0x060058A2 RID: 22690 RVA: 0x001FF527 File Offset: 0x001FD727
	private string ReplaceStringWithRandom(string fullString, string replacementKey, string[] replacementValues)
	{
		if (!fullString.Contains(replacementKey))
		{
			return fullString;
		}
		return fullString.Replace(replacementKey, replacementValues.GetRandom<string>());
	}

	// Token: 0x04003A30 RID: 14896
	[SerializeField]
	private KInputTextField inputField;

	// Token: 0x04003A31 RID: 14897
	[SerializeField]
	private KButton shuffleBaseNameButton;

	// Token: 0x04003A32 RID: 14898
	private MinionSelectScreen minionSelectScreen;
}
