using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x020003F0 RID: 1008
public class CodeWriter
{
	// Token: 0x06001515 RID: 5397 RVA: 0x000741F3 File Offset: 0x000723F3
	public CodeWriter(string path)
	{
		this.Path = path;
	}

	// Token: 0x06001516 RID: 5398 RVA: 0x0007420D File Offset: 0x0007240D
	public void Comment(string text)
	{
		this.Lines.Add("// " + text);
	}

	// Token: 0x06001517 RID: 5399 RVA: 0x00074228 File Offset: 0x00072428
	public void BeginPartialClass(string class_name, string parent_name = null)
	{
		string text = "public partial class " + class_name;
		if (parent_name != null)
		{
			text = text + " : " + parent_name;
		}
		this.Line(text);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001518 RID: 5400 RVA: 0x00074274 File Offset: 0x00072474
	public void BeginClass(string class_name, string parent_name = null)
	{
		string text = "public class " + class_name;
		if (parent_name != null)
		{
			text = text + " : " + parent_name;
		}
		this.Line(text);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001519 RID: 5401 RVA: 0x000742BD File Offset: 0x000724BD
	public void EndClass()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x0600151A RID: 5402 RVA: 0x000742D8 File Offset: 0x000724D8
	public void BeginNameSpace(string name)
	{
		this.Line("namespace " + name);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x0600151B RID: 5403 RVA: 0x00074304 File Offset: 0x00072504
	public void EndNameSpace()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x0600151C RID: 5404 RVA: 0x0007431F File Offset: 0x0007251F
	public void BeginArrayStructureInitialization(string name)
	{
		this.Line("new " + name);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x0600151D RID: 5405 RVA: 0x0007434B File Offset: 0x0007254B
	public void EndArrayStructureInitialization(bool last_item)
	{
		this.Indent--;
		if (!last_item)
		{
			this.Line("},");
			return;
		}
		this.Line("}");
	}

	// Token: 0x0600151E RID: 5406 RVA: 0x00074375 File Offset: 0x00072575
	public void BeginArraArrayInitialization(string array_type, string array_name)
	{
		this.Line(array_name + " = new " + array_type + "[]");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x0600151F RID: 5407 RVA: 0x000743A7 File Offset: 0x000725A7
	public void EndArrayArrayInitialization(bool last_item)
	{
		this.Indent--;
		if (last_item)
		{
			this.Line("}");
			return;
		}
		this.Line("},");
	}

	// Token: 0x06001520 RID: 5408 RVA: 0x000743D1 File Offset: 0x000725D1
	public void BeginConstructor(string name)
	{
		this.Line("public " + name + "()");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001521 RID: 5409 RVA: 0x00074402 File Offset: 0x00072602
	public void EndConstructor()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x06001522 RID: 5410 RVA: 0x0007441D File Offset: 0x0007261D
	public void BeginArrayAssignment(string array_type, string array_name)
	{
		this.Line(array_name + " = new " + array_type + "[]");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001523 RID: 5411 RVA: 0x0007444F File Offset: 0x0007264F
	public void EndArrayAssignment()
	{
		this.Indent--;
		this.Line("};");
	}

	// Token: 0x06001524 RID: 5412 RVA: 0x0007446A File Offset: 0x0007266A
	public void FieldAssignment(string field_name, string value)
	{
		this.Line(field_name + " = " + value + ";");
	}

	// Token: 0x06001525 RID: 5413 RVA: 0x00074483 File Offset: 0x00072683
	public void BeginStructureDelegateFieldInitializer(string name)
	{
		this.Line(name + "=delegate()");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001526 RID: 5414 RVA: 0x000744AF File Offset: 0x000726AF
	public void EndStructureDelegateFieldInitializer()
	{
		this.Indent--;
		this.Line("},");
	}

	// Token: 0x06001527 RID: 5415 RVA: 0x000744CA File Offset: 0x000726CA
	public void BeginIf(string condition)
	{
		this.Line("if(" + condition + ")");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001528 RID: 5416 RVA: 0x000744FC File Offset: 0x000726FC
	public void BeginElseIf(string condition)
	{
		this.Indent--;
		this.Line("}");
		this.Line("else if(" + condition + ")");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001529 RID: 5417 RVA: 0x00074551 File Offset: 0x00072751
	public void EndIf()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x0600152A RID: 5418 RVA: 0x0007456C File Offset: 0x0007276C
	public void BeginFunctionDeclaration(string name, string parameter, string return_type)
	{
		this.Line(string.Concat(new string[]
		{
			"public ",
			return_type,
			" ",
			name,
			"(",
			parameter,
			")"
		}));
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x0600152B RID: 5419 RVA: 0x000745D0 File Offset: 0x000727D0
	public void BeginFunctionDeclaration(string name, string return_type)
	{
		this.Line(string.Concat(new string[]
		{
			"public ",
			return_type,
			" ",
			name,
			"()"
		}));
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x0600152C RID: 5420 RVA: 0x00074627 File Offset: 0x00072827
	public void EndFunctionDeclaration()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x0600152D RID: 5421 RVA: 0x00074644 File Offset: 0x00072844
	private void InternalNamedParameter(string name, string value, bool last_parameter)
	{
		string str = "";
		if (!last_parameter)
		{
			str = ",";
		}
		this.Line(name + ":" + value + str);
	}

	// Token: 0x0600152E RID: 5422 RVA: 0x00074673 File Offset: 0x00072873
	public void NamedParameterBool(string name, bool value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value.ToString().ToLower(), last_parameter);
	}

	// Token: 0x0600152F RID: 5423 RVA: 0x00074689 File Offset: 0x00072889
	public void NamedParameterInt(string name, int value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value.ToString(), last_parameter);
	}

	// Token: 0x06001530 RID: 5424 RVA: 0x0007469A File Offset: 0x0007289A
	public void NamedParameterFloat(string name, float value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value.ToString() + "f", last_parameter);
	}

	// Token: 0x06001531 RID: 5425 RVA: 0x000746B5 File Offset: 0x000728B5
	public void NamedParameterString(string name, string value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value, last_parameter);
	}

	// Token: 0x06001532 RID: 5426 RVA: 0x000746C0 File Offset: 0x000728C0
	public void BeginFunctionCall(string name)
	{
		this.Line(name);
		this.Line("(");
		this.Indent++;
	}

	// Token: 0x06001533 RID: 5427 RVA: 0x000746E2 File Offset: 0x000728E2
	public void EndFunctionCall()
	{
		this.Indent--;
		this.Line(");");
	}

	// Token: 0x06001534 RID: 5428 RVA: 0x00074700 File Offset: 0x00072900
	public void FunctionCall(string function_name, params string[] parameters)
	{
		string str = function_name + "(";
		for (int i = 0; i < parameters.Length; i++)
		{
			str += parameters[i];
			if (i != parameters.Length - 1)
			{
				str += ", ";
			}
		}
		this.Line(str + ");");
	}

	// Token: 0x06001535 RID: 5429 RVA: 0x00074756 File Offset: 0x00072956
	public void StructureFieldInitializer(string field, string value)
	{
		this.Line(field + " = " + value + ",");
	}

	// Token: 0x06001536 RID: 5430 RVA: 0x00074770 File Offset: 0x00072970
	public void StructureArrayFieldInitializer(string field, string field_type, params string[] values)
	{
		string text = field + " = new " + field_type + "[]{ ";
		for (int i = 0; i < values.Length; i++)
		{
			text += values[i];
			if (i < values.Length - 1)
			{
				text += ", ";
			}
		}
		text += " },";
		this.Line(text);
	}

	// Token: 0x06001537 RID: 5431 RVA: 0x000747D0 File Offset: 0x000729D0
	public void Line(string text = "")
	{
		for (int i = 0; i < this.Indent; i++)
		{
			text = "\t" + text;
		}
		this.Lines.Add(text);
	}

	// Token: 0x06001538 RID: 5432 RVA: 0x00074807 File Offset: 0x00072A07
	public void Flush()
	{
		File.WriteAllLines(this.Path, this.Lines.ToArray());
	}

	// Token: 0x04000BFE RID: 3070
	private List<string> Lines = new List<string>();

	// Token: 0x04000BFF RID: 3071
	private string Path;

	// Token: 0x04000C00 RID: 3072
	private int Indent;
}
