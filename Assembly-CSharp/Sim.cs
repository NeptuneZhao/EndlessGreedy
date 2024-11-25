using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000BBC RID: 3004
public static class Sim
{
	// Token: 0x06005B54 RID: 23380 RVA: 0x002147E0 File Offset: 0x002129E0
	public static bool IsRadiationEnabled()
	{
		return DlcManager.FeatureRadiationEnabled();
	}

	// Token: 0x06005B55 RID: 23381 RVA: 0x002147E7 File Offset: 0x002129E7
	public static bool IsValidHandle(int h)
	{
		return h != -1 && h != -2;
	}

	// Token: 0x06005B56 RID: 23382 RVA: 0x002147F7 File Offset: 0x002129F7
	public static int GetHandleIndex(int h)
	{
		return h & 16777215;
	}

	// Token: 0x06005B57 RID: 23383
	[DllImport("SimDLL")]
	public static extern void SIM_Initialize(Sim.GAME_MessageHandler callback);

	// Token: 0x06005B58 RID: 23384
	[DllImport("SimDLL")]
	public static extern void SIM_Shutdown();

	// Token: 0x06005B59 RID: 23385
	[DllImport("SimDLL")]
	public unsafe static extern IntPtr SIM_HandleMessage(int sim_msg_id, int msg_length, byte* msg);

	// Token: 0x06005B5A RID: 23386
	[DllImport("SimDLL")]
	private unsafe static extern byte* SIM_BeginSave(int* size, int x, int y);

	// Token: 0x06005B5B RID: 23387
	[DllImport("SimDLL")]
	private static extern void SIM_EndSave();

	// Token: 0x06005B5C RID: 23388
	[DllImport("SimDLL")]
	public static extern void SIM_DebugCrash();

	// Token: 0x06005B5D RID: 23389 RVA: 0x00214800 File Offset: 0x00212A00
	public unsafe static IntPtr HandleMessage(SimMessageHashes sim_msg_id, int msg_length, byte[] msg)
	{
		IntPtr result;
		fixed (byte[] array = msg)
		{
			byte* msg2;
			if (msg == null || array.Length == 0)
			{
				msg2 = null;
			}
			else
			{
				msg2 = &array[0];
			}
			result = Sim.SIM_HandleMessage((int)sim_msg_id, msg_length, msg2);
		}
		return result;
	}

	// Token: 0x06005B5E RID: 23390 RVA: 0x00214830 File Offset: 0x00212A30
	public unsafe static void Save(BinaryWriter writer, int x, int y)
	{
		int num;
		void* value = (void*)Sim.SIM_BeginSave(&num, x, y);
		byte[] array = new byte[num];
		Marshal.Copy((IntPtr)value, array, 0, num);
		Sim.SIM_EndSave();
		writer.Write(num);
		writer.Write(array);
	}

	// Token: 0x06005B5F RID: 23391 RVA: 0x00214870 File Offset: 0x00212A70
	public unsafe static int LoadWorld(IReader reader)
	{
		int num = reader.ReadInt32();
		byte[] array;
		byte* msg;
		if ((array = reader.ReadBytes(num)) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		IntPtr value = Sim.SIM_HandleMessage(-672538170, num, msg);
		array = null;
		if (value == IntPtr.Zero)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06005B60 RID: 23392 RVA: 0x002148C0 File Offset: 0x00212AC0
	public static void AllocateCells(int width, int height, bool headless = false)
	{
		using (MemoryStream memoryStream = new MemoryStream(8))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				binaryWriter.Write(width);
				binaryWriter.Write(height);
				bool value = Sim.IsRadiationEnabled();
				binaryWriter.Write(value);
				binaryWriter.Write(headless);
				binaryWriter.Flush();
				Sim.HandleMessage(SimMessageHashes.AllocateCells, (int)memoryStream.Length, memoryStream.GetBuffer());
			}
		}
	}

	// Token: 0x06005B61 RID: 23393 RVA: 0x00214950 File Offset: 0x00212B50
	public unsafe static int Load(IReader reader)
	{
		int num = reader.ReadInt32();
		byte[] array;
		byte* msg;
		if ((array = reader.ReadBytes(num)) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		IntPtr value = Sim.SIM_HandleMessage(-672538170, num, msg);
		array = null;
		if (value == IntPtr.Zero)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06005B62 RID: 23394 RVA: 0x002149A0 File Offset: 0x00212BA0
	public unsafe static void Start()
	{
		Sim.GameDataUpdate* ptr = (Sim.GameDataUpdate*)((void*)Sim.SIM_HandleMessage(-931446686, 0, null));
		Grid.elementIdx = ptr->elementIdx;
		Grid.temperature = ptr->temperature;
		Grid.radiation = ptr->radiation;
		Grid.mass = ptr->mass;
		Grid.properties = ptr->properties;
		Grid.strengthInfo = ptr->strengthInfo;
		Grid.insulation = ptr->insulation;
		Grid.diseaseIdx = ptr->diseaseIdx;
		Grid.diseaseCount = ptr->diseaseCount;
		Grid.AccumulatedFlowValues = ptr->accumulatedFlow;
		PropertyTextures.externalFlowTex = ptr->propertyTextureFlow;
		PropertyTextures.externalLiquidTex = ptr->propertyTextureLiquid;
		PropertyTextures.externalExposedToSunlight = ptr->propertyTextureExposedToSunlight;
		Grid.InitializeCells();
	}

	// Token: 0x06005B63 RID: 23395 RVA: 0x00214A54 File Offset: 0x00212C54
	public static void Shutdown()
	{
		Sim.SIM_Shutdown();
		Grid.mass = null;
	}

	// Token: 0x06005B64 RID: 23396
	[DllImport("SimDLL")]
	public unsafe static extern char* SYSINFO_Acquire();

	// Token: 0x06005B65 RID: 23397
	[DllImport("SimDLL")]
	public static extern void SYSINFO_Release();

	// Token: 0x06005B66 RID: 23398 RVA: 0x00214A64 File Offset: 0x00212C64
	public unsafe static int DLL_MessageHandler(int message_id, IntPtr data)
	{
		if (message_id == 0)
		{
			Sim.DLLExceptionHandlerMessage* ptr = (Sim.DLLExceptionHandlerMessage*)((void*)data);
			string stack_trace = Marshal.PtrToStringAnsi(ptr->callstack);
			string dmp_filename = Marshal.PtrToStringAnsi(ptr->dmpFilename);
			KCrashReporter.ReportSimDLLCrash("SimDLL Crash Dump", stack_trace, dmp_filename);
			return 0;
		}
		if (message_id == 1)
		{
			Sim.DLLReportMessageMessage* ptr2 = (Sim.DLLReportMessageMessage*)((void*)data);
			string msg = "SimMessage: " + Marshal.PtrToStringAnsi(ptr2->message);
			string stack_trace2;
			if (ptr2->callstack != IntPtr.Zero)
			{
				stack_trace2 = Marshal.PtrToStringAnsi(ptr2->callstack);
			}
			else
			{
				string str = Marshal.PtrToStringAnsi(ptr2->file);
				int line = ptr2->line;
				stack_trace2 = str + ":" + line.ToString();
			}
			KCrashReporter.ReportSimDLLCrash(msg, stack_trace2, null);
			return 0;
		}
		return -1;
	}

	// Token: 0x04003C39 RID: 15417
	public const int InvalidHandle = -1;

	// Token: 0x04003C3A RID: 15418
	public const int QueuedRegisterHandle = -2;

	// Token: 0x04003C3B RID: 15419
	public const byte InvalidDiseaseIdx = 255;

	// Token: 0x04003C3C RID: 15420
	public const ushort InvalidElementIdx = 65535;

	// Token: 0x04003C3D RID: 15421
	public const byte SpaceZoneID = 255;

	// Token: 0x04003C3E RID: 15422
	public const byte SolidZoneID = 0;

	// Token: 0x04003C3F RID: 15423
	public const int ChunkEdgeSize = 32;

	// Token: 0x04003C40 RID: 15424
	public const float StateTransitionEnergy = 3f;

	// Token: 0x04003C41 RID: 15425
	public const float ZeroDegreesCentigrade = 273.15f;

	// Token: 0x04003C42 RID: 15426
	public const float StandardTemperature = 293.15f;

	// Token: 0x04003C43 RID: 15427
	public const float StandardMeltingPointOffset = 10f;

	// Token: 0x04003C44 RID: 15428
	public const float StandardPressure = 101.3f;

	// Token: 0x04003C45 RID: 15429
	public const float Epsilon = 0.0001f;

	// Token: 0x04003C46 RID: 15430
	public const float MaxTemperature = 10000f;

	// Token: 0x04003C47 RID: 15431
	public const float MinTemperature = 0f;

	// Token: 0x04003C48 RID: 15432
	public const float MaxRadiation = 9000000f;

	// Token: 0x04003C49 RID: 15433
	public const float MinRadiation = 0f;

	// Token: 0x04003C4A RID: 15434
	public const float MaxMass = 10000f;

	// Token: 0x04003C4B RID: 15435
	public const float MinMass = 1.0001f;

	// Token: 0x04003C4C RID: 15436
	private const int PressureUpdateInterval = 1;

	// Token: 0x04003C4D RID: 15437
	private const int TemperatureUpdateInterval = 1;

	// Token: 0x04003C4E RID: 15438
	private const int LiquidUpdateInterval = 1;

	// Token: 0x04003C4F RID: 15439
	private const int LifeUpdateInterval = 1;

	// Token: 0x04003C50 RID: 15440
	public const byte ClearSkyGridValue = 253;

	// Token: 0x04003C51 RID: 15441
	public const int PACKING_ALIGNMENT = 4;

	// Token: 0x02001C55 RID: 7253
	// (Invoke) Token: 0x0600A69F RID: 42655
	public delegate int GAME_MessageHandler(int message_id, IntPtr data);

	// Token: 0x02001C56 RID: 7254
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DLLExceptionHandlerMessage
	{
		// Token: 0x040082E7 RID: 33511
		public IntPtr callstack;

		// Token: 0x040082E8 RID: 33512
		public IntPtr dmpFilename;
	}

	// Token: 0x02001C57 RID: 7255
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DLLReportMessageMessage
	{
		// Token: 0x040082E9 RID: 33513
		public IntPtr callstack;

		// Token: 0x040082EA RID: 33514
		public IntPtr message;

		// Token: 0x040082EB RID: 33515
		public IntPtr file;

		// Token: 0x040082EC RID: 33516
		public int line;
	}

	// Token: 0x02001C58 RID: 7256
	private enum GameHandledMessages
	{
		// Token: 0x040082EE RID: 33518
		ExceptionHandler,
		// Token: 0x040082EF RID: 33519
		ReportMessage
	}

	// Token: 0x02001C59 RID: 7257
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PhysicsData
	{
		// Token: 0x0600A6A2 RID: 42658 RVA: 0x00397D94 File Offset: 0x00395F94
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.temperature);
			writer.Write(this.mass);
			writer.Write(this.pressure);
		}

		// Token: 0x040082F0 RID: 33520
		public float temperature;

		// Token: 0x040082F1 RID: 33521
		public float mass;

		// Token: 0x040082F2 RID: 33522
		public float pressure;
	}

	// Token: 0x02001C5A RID: 7258
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Cell
	{
		// Token: 0x0600A6A3 RID: 42659 RVA: 0x00397DC0 File Offset: 0x00395FC0
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.elementIdx);
			writer.Write(0);
			writer.Write(this.insulation);
			writer.Write(0);
			writer.Write(this.pad0);
			writer.Write(this.pad1);
			writer.Write(this.pad2);
			writer.Write(this.temperature);
			writer.Write(this.mass);
		}

		// Token: 0x0600A6A4 RID: 42660 RVA: 0x00397E31 File Offset: 0x00396031
		public void SetValues(global::Element elem, List<global::Element> elements)
		{
			this.SetValues(elem, elem.defaultValues, elements);
		}

		// Token: 0x0600A6A5 RID: 42661 RVA: 0x00397E44 File Offset: 0x00396044
		public void SetValues(global::Element elem, Sim.PhysicsData pd, List<global::Element> elements)
		{
			this.elementIdx = (ushort)elements.IndexOf(elem);
			this.temperature = pd.temperature;
			this.mass = pd.mass;
			this.insulation = byte.MaxValue;
			DebugUtil.Assert(this.temperature > 0f || this.mass == 0f, "A non-zero mass cannot have a <= 0 temperature");
		}

		// Token: 0x0600A6A6 RID: 42662 RVA: 0x00397EAC File Offset: 0x003960AC
		public void SetValues(ushort new_elem_idx, float new_temperature, float new_mass)
		{
			this.elementIdx = new_elem_idx;
			this.temperature = new_temperature;
			this.mass = new_mass;
			this.insulation = byte.MaxValue;
			DebugUtil.Assert(this.temperature > 0f || this.mass == 0f, "A non-zero mass cannot have a <= 0 temperature");
		}

		// Token: 0x040082F3 RID: 33523
		public ushort elementIdx;

		// Token: 0x040082F4 RID: 33524
		public byte properties;

		// Token: 0x040082F5 RID: 33525
		public byte insulation;

		// Token: 0x040082F6 RID: 33526
		public byte strengthInfo;

		// Token: 0x040082F7 RID: 33527
		public byte pad0;

		// Token: 0x040082F8 RID: 33528
		public byte pad1;

		// Token: 0x040082F9 RID: 33529
		public byte pad2;

		// Token: 0x040082FA RID: 33530
		public float temperature;

		// Token: 0x040082FB RID: 33531
		public float mass;

		// Token: 0x02002649 RID: 9801
		public enum Properties
		{
			// Token: 0x0400AA3A RID: 43578
			GasImpermeable = 1,
			// Token: 0x0400AA3B RID: 43579
			LiquidImpermeable,
			// Token: 0x0400AA3C RID: 43580
			SolidImpermeable = 4,
			// Token: 0x0400AA3D RID: 43581
			Unbreakable = 8,
			// Token: 0x0400AA3E RID: 43582
			Transparent = 16,
			// Token: 0x0400AA3F RID: 43583
			Opaque = 32,
			// Token: 0x0400AA40 RID: 43584
			NotifyOnMelt = 64,
			// Token: 0x0400AA41 RID: 43585
			ConstructedTile = 128
		}
	}

	// Token: 0x02001C5B RID: 7259
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Element
	{
		// Token: 0x0600A6A7 RID: 42663 RVA: 0x00397F00 File Offset: 0x00396100
		public Element(global::Element e, List<global::Element> elements)
		{
			this.id = e.id;
			this.state = (byte)e.state;
			if (e.HasTag(GameTags.Unstable))
			{
				this.state |= 8;
			}
			int num = elements.FindIndex((global::Element ele) => ele.id == e.lowTempTransitionTarget);
			int num2 = elements.FindIndex((global::Element ele) => ele.id == e.highTempTransitionTarget);
			this.lowTempTransitionIdx = (ushort)((num >= 0) ? num : 65535);
			this.highTempTransitionIdx = (ushort)((num2 >= 0) ? num2 : 65535);
			this.elementsTableIdx = (ushort)elements.IndexOf(e);
			this.specificHeatCapacity = e.specificHeatCapacity;
			this.thermalConductivity = e.thermalConductivity;
			this.solidSurfaceAreaMultiplier = e.solidSurfaceAreaMultiplier;
			this.liquidSurfaceAreaMultiplier = e.liquidSurfaceAreaMultiplier;
			this.gasSurfaceAreaMultiplier = e.gasSurfaceAreaMultiplier;
			this.molarMass = e.molarMass;
			this.strength = e.strength;
			this.flow = e.flow;
			this.viscosity = e.viscosity;
			this.minHorizontalFlow = e.minHorizontalFlow;
			this.minVerticalFlow = e.minVerticalFlow;
			this.maxMass = e.maxMass;
			this.lowTemp = e.lowTemp;
			this.highTemp = e.highTemp;
			this.highTempTransitionOreID = e.highTempTransitionOreID;
			this.highTempTransitionOreMassConversion = e.highTempTransitionOreMassConversion;
			this.lowTempTransitionOreID = e.lowTempTransitionOreID;
			this.lowTempTransitionOreMassConversion = e.lowTempTransitionOreMassConversion;
			this.sublimateIndex = (ushort)elements.FindIndex((global::Element ele) => ele.id == e.sublimateId);
			this.convertIndex = (ushort)elements.FindIndex((global::Element ele) => ele.id == e.convertId);
			this.pack0 = 0;
			if (e.substance == null)
			{
				this.colour = 0U;
			}
			else
			{
				Color32 color = e.substance.colour;
				this.colour = (uint)((int)color.a << 24 | (int)color.b << 16 | (int)color.g << 8 | (int)color.r);
			}
			this.sublimateFX = e.sublimateFX;
			this.sublimateRate = e.sublimateRate;
			this.sublimateEfficiency = e.sublimateEfficiency;
			this.sublimateProbability = e.sublimateProbability;
			this.offGasProbability = e.offGasPercentage;
			this.lightAbsorptionFactor = e.lightAbsorptionFactor;
			this.radiationAbsorptionFactor = e.radiationAbsorptionFactor;
			this.radiationPer1000Mass = e.radiationPer1000Mass;
			this.defaultValues = e.defaultValues;
		}

		// Token: 0x0600A6A8 RID: 42664 RVA: 0x00398210 File Offset: 0x00396410
		public void Write(BinaryWriter writer)
		{
			writer.Write((int)this.id);
			writer.Write(this.lowTempTransitionIdx);
			writer.Write(this.highTempTransitionIdx);
			writer.Write(this.elementsTableIdx);
			writer.Write(this.state);
			writer.Write(this.pack0);
			writer.Write(this.specificHeatCapacity);
			writer.Write(this.thermalConductivity);
			writer.Write(this.molarMass);
			writer.Write(this.solidSurfaceAreaMultiplier);
			writer.Write(this.liquidSurfaceAreaMultiplier);
			writer.Write(this.gasSurfaceAreaMultiplier);
			writer.Write(this.flow);
			writer.Write(this.viscosity);
			writer.Write(this.minHorizontalFlow);
			writer.Write(this.minVerticalFlow);
			writer.Write(this.maxMass);
			writer.Write(this.lowTemp);
			writer.Write(this.highTemp);
			writer.Write(this.strength);
			writer.Write((int)this.lowTempTransitionOreID);
			writer.Write(this.lowTempTransitionOreMassConversion);
			writer.Write((int)this.highTempTransitionOreID);
			writer.Write(this.highTempTransitionOreMassConversion);
			writer.Write(this.sublimateIndex);
			writer.Write(this.convertIndex);
			writer.Write(this.colour);
			writer.Write((int)this.sublimateFX);
			writer.Write(this.sublimateRate);
			writer.Write(this.sublimateEfficiency);
			writer.Write(this.sublimateProbability);
			writer.Write(this.offGasProbability);
			writer.Write(this.lightAbsorptionFactor);
			writer.Write(this.radiationAbsorptionFactor);
			writer.Write(this.radiationPer1000Mass);
			this.defaultValues.Write(writer);
		}

		// Token: 0x040082FC RID: 33532
		public SimHashes id;

		// Token: 0x040082FD RID: 33533
		public ushort lowTempTransitionIdx;

		// Token: 0x040082FE RID: 33534
		public ushort highTempTransitionIdx;

		// Token: 0x040082FF RID: 33535
		public ushort elementsTableIdx;

		// Token: 0x04008300 RID: 33536
		public byte state;

		// Token: 0x04008301 RID: 33537
		public byte pack0;

		// Token: 0x04008302 RID: 33538
		public float specificHeatCapacity;

		// Token: 0x04008303 RID: 33539
		public float thermalConductivity;

		// Token: 0x04008304 RID: 33540
		public float molarMass;

		// Token: 0x04008305 RID: 33541
		public float solidSurfaceAreaMultiplier;

		// Token: 0x04008306 RID: 33542
		public float liquidSurfaceAreaMultiplier;

		// Token: 0x04008307 RID: 33543
		public float gasSurfaceAreaMultiplier;

		// Token: 0x04008308 RID: 33544
		public float flow;

		// Token: 0x04008309 RID: 33545
		public float viscosity;

		// Token: 0x0400830A RID: 33546
		public float minHorizontalFlow;

		// Token: 0x0400830B RID: 33547
		public float minVerticalFlow;

		// Token: 0x0400830C RID: 33548
		public float maxMass;

		// Token: 0x0400830D RID: 33549
		public float lowTemp;

		// Token: 0x0400830E RID: 33550
		public float highTemp;

		// Token: 0x0400830F RID: 33551
		public float strength;

		// Token: 0x04008310 RID: 33552
		public SimHashes lowTempTransitionOreID;

		// Token: 0x04008311 RID: 33553
		public float lowTempTransitionOreMassConversion;

		// Token: 0x04008312 RID: 33554
		public SimHashes highTempTransitionOreID;

		// Token: 0x04008313 RID: 33555
		public float highTempTransitionOreMassConversion;

		// Token: 0x04008314 RID: 33556
		public ushort sublimateIndex;

		// Token: 0x04008315 RID: 33557
		public ushort convertIndex;

		// Token: 0x04008316 RID: 33558
		public uint colour;

		// Token: 0x04008317 RID: 33559
		public SpawnFXHashes sublimateFX;

		// Token: 0x04008318 RID: 33560
		public float sublimateRate;

		// Token: 0x04008319 RID: 33561
		public float sublimateEfficiency;

		// Token: 0x0400831A RID: 33562
		public float sublimateProbability;

		// Token: 0x0400831B RID: 33563
		public float offGasProbability;

		// Token: 0x0400831C RID: 33564
		public float lightAbsorptionFactor;

		// Token: 0x0400831D RID: 33565
		public float radiationAbsorptionFactor;

		// Token: 0x0400831E RID: 33566
		public float radiationPer1000Mass;

		// Token: 0x0400831F RID: 33567
		public Sim.PhysicsData defaultValues;
	}

	// Token: 0x02001C5C RID: 7260
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseCell
	{
		// Token: 0x0600A6A9 RID: 42665 RVA: 0x003983E4 File Offset: 0x003965E4
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.diseaseIdx);
			writer.Write(this.reservedInfestationTickCount);
			writer.Write(this.pad1);
			writer.Write(this.pad2);
			writer.Write(this.elementCount);
			writer.Write(this.reservedAccumulatedError);
		}

		// Token: 0x04008320 RID: 33568
		public byte diseaseIdx;

		// Token: 0x04008321 RID: 33569
		private byte reservedInfestationTickCount;

		// Token: 0x04008322 RID: 33570
		private byte pad1;

		// Token: 0x04008323 RID: 33571
		private byte pad2;

		// Token: 0x04008324 RID: 33572
		public int elementCount;

		// Token: 0x04008325 RID: 33573
		private float reservedAccumulatedError;

		// Token: 0x04008326 RID: 33574
		public static readonly Sim.DiseaseCell Invalid = new Sim.DiseaseCell
		{
			diseaseIdx = byte.MaxValue,
			elementCount = 0
		};
	}

	// Token: 0x02001C5D RID: 7261
	// (Invoke) Token: 0x0600A6AC RID: 42668
	public delegate void GAME_Callback();

	// Token: 0x02001C5E RID: 7262
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SolidInfo
	{
		// Token: 0x04008327 RID: 33575
		public int cellIdx;

		// Token: 0x04008328 RID: 33576
		public int isSolid;
	}

	// Token: 0x02001C5F RID: 7263
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct LiquidChangeInfo
	{
		// Token: 0x04008329 RID: 33577
		public int cellIdx;
	}

	// Token: 0x02001C60 RID: 7264
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SolidSubstanceChangeInfo
	{
		// Token: 0x0400832A RID: 33578
		public int cellIdx;
	}

	// Token: 0x02001C61 RID: 7265
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SubstanceChangeInfo
	{
		// Token: 0x0400832B RID: 33579
		public int cellIdx;

		// Token: 0x0400832C RID: 33580
		public ushort oldElemIdx;

		// Token: 0x0400832D RID: 33581
		public ushort newElemIdx;
	}

	// Token: 0x02001C62 RID: 7266
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CallbackInfo
	{
		// Token: 0x0400832E RID: 33582
		public int callbackIdx;
	}

	// Token: 0x02001C63 RID: 7267
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GameDataUpdate
	{
		// Token: 0x0400832F RID: 33583
		public int numFramesProcessed;

		// Token: 0x04008330 RID: 33584
		public unsafe ushort* elementIdx;

		// Token: 0x04008331 RID: 33585
		public unsafe float* temperature;

		// Token: 0x04008332 RID: 33586
		public unsafe float* mass;

		// Token: 0x04008333 RID: 33587
		public unsafe byte* properties;

		// Token: 0x04008334 RID: 33588
		public unsafe byte* insulation;

		// Token: 0x04008335 RID: 33589
		public unsafe byte* strengthInfo;

		// Token: 0x04008336 RID: 33590
		public unsafe float* radiation;

		// Token: 0x04008337 RID: 33591
		public unsafe byte* diseaseIdx;

		// Token: 0x04008338 RID: 33592
		public unsafe int* diseaseCount;

		// Token: 0x04008339 RID: 33593
		public int numSolidInfo;

		// Token: 0x0400833A RID: 33594
		public unsafe Sim.SolidInfo* solidInfo;

		// Token: 0x0400833B RID: 33595
		public int numLiquidChangeInfo;

		// Token: 0x0400833C RID: 33596
		public unsafe Sim.LiquidChangeInfo* liquidChangeInfo;

		// Token: 0x0400833D RID: 33597
		public int numSolidSubstanceChangeInfo;

		// Token: 0x0400833E RID: 33598
		public unsafe Sim.SolidSubstanceChangeInfo* solidSubstanceChangeInfo;

		// Token: 0x0400833F RID: 33599
		public int numSubstanceChangeInfo;

		// Token: 0x04008340 RID: 33600
		public unsafe Sim.SubstanceChangeInfo* substanceChangeInfo;

		// Token: 0x04008341 RID: 33601
		public int numCallbackInfo;

		// Token: 0x04008342 RID: 33602
		public unsafe Sim.CallbackInfo* callbackInfo;

		// Token: 0x04008343 RID: 33603
		public int numSpawnFallingLiquidInfo;

		// Token: 0x04008344 RID: 33604
		public unsafe Sim.SpawnFallingLiquidInfo* spawnFallingLiquidInfo;

		// Token: 0x04008345 RID: 33605
		public int numDigInfo;

		// Token: 0x04008346 RID: 33606
		public unsafe Sim.SpawnOreInfo* digInfo;

		// Token: 0x04008347 RID: 33607
		public int numSpawnOreInfo;

		// Token: 0x04008348 RID: 33608
		public unsafe Sim.SpawnOreInfo* spawnOreInfo;

		// Token: 0x04008349 RID: 33609
		public int numSpawnFXInfo;

		// Token: 0x0400834A RID: 33610
		public unsafe Sim.SpawnFXInfo* spawnFXInfo;

		// Token: 0x0400834B RID: 33611
		public int numUnstableCellInfo;

		// Token: 0x0400834C RID: 33612
		public unsafe Sim.UnstableCellInfo* unstableCellInfo;

		// Token: 0x0400834D RID: 33613
		public int numWorldDamageInfo;

		// Token: 0x0400834E RID: 33614
		public unsafe Sim.WorldDamageInfo* worldDamageInfo;

		// Token: 0x0400834F RID: 33615
		public int numBuildingTemperatures;

		// Token: 0x04008350 RID: 33616
		public unsafe Sim.BuildingTemperatureInfo* buildingTemperatures;

		// Token: 0x04008351 RID: 33617
		public int numMassConsumedCallbacks;

		// Token: 0x04008352 RID: 33618
		public unsafe Sim.MassConsumedCallback* massConsumedCallbacks;

		// Token: 0x04008353 RID: 33619
		public int numMassEmittedCallbacks;

		// Token: 0x04008354 RID: 33620
		public unsafe Sim.MassEmittedCallback* massEmittedCallbacks;

		// Token: 0x04008355 RID: 33621
		public int numDiseaseConsumptionCallbacks;

		// Token: 0x04008356 RID: 33622
		public unsafe Sim.DiseaseConsumptionCallback* diseaseConsumptionCallbacks;

		// Token: 0x04008357 RID: 33623
		public int numComponentStateChangedMessages;

		// Token: 0x04008358 RID: 33624
		public unsafe Sim.ComponentStateChangedMessage* componentStateChangedMessages;

		// Token: 0x04008359 RID: 33625
		public int numRemovedMassEntries;

		// Token: 0x0400835A RID: 33626
		public unsafe Sim.ConsumedMassInfo* removedMassEntries;

		// Token: 0x0400835B RID: 33627
		public int numEmittedMassEntries;

		// Token: 0x0400835C RID: 33628
		public unsafe Sim.EmittedMassInfo* emittedMassEntries;

		// Token: 0x0400835D RID: 33629
		public int numElementChunkInfos;

		// Token: 0x0400835E RID: 33630
		public unsafe Sim.ElementChunkInfo* elementChunkInfos;

		// Token: 0x0400835F RID: 33631
		public int numElementChunkMeltedInfos;

		// Token: 0x04008360 RID: 33632
		public unsafe Sim.MeltedInfo* elementChunkMeltedInfos;

		// Token: 0x04008361 RID: 33633
		public int numBuildingOverheatInfos;

		// Token: 0x04008362 RID: 33634
		public unsafe Sim.MeltedInfo* buildingOverheatInfos;

		// Token: 0x04008363 RID: 33635
		public int numBuildingNoLongerOverheatedInfos;

		// Token: 0x04008364 RID: 33636
		public unsafe Sim.MeltedInfo* buildingNoLongerOverheatedInfos;

		// Token: 0x04008365 RID: 33637
		public int numBuildingMeltedInfos;

		// Token: 0x04008366 RID: 33638
		public unsafe Sim.MeltedInfo* buildingMeltedInfos;

		// Token: 0x04008367 RID: 33639
		public int numCellMeltedInfos;

		// Token: 0x04008368 RID: 33640
		public unsafe Sim.CellMeltedInfo* cellMeltedInfos;

		// Token: 0x04008369 RID: 33641
		public int numDiseaseEmittedInfos;

		// Token: 0x0400836A RID: 33642
		public unsafe Sim.DiseaseEmittedInfo* diseaseEmittedInfos;

		// Token: 0x0400836B RID: 33643
		public int numDiseaseConsumedInfos;

		// Token: 0x0400836C RID: 33644
		public unsafe Sim.DiseaseConsumedInfo* diseaseConsumedInfos;

		// Token: 0x0400836D RID: 33645
		public int numRadiationConsumedCallbacks;

		// Token: 0x0400836E RID: 33646
		public unsafe Sim.ConsumedRadiationCallback* radiationConsumedCallbacks;

		// Token: 0x0400836F RID: 33647
		public unsafe float* accumulatedFlow;

		// Token: 0x04008370 RID: 33648
		public IntPtr propertyTextureFlow;

		// Token: 0x04008371 RID: 33649
		public IntPtr propertyTextureLiquid;

		// Token: 0x04008372 RID: 33650
		public IntPtr propertyTextureExposedToSunlight;
	}

	// Token: 0x02001C64 RID: 7268
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SpawnFallingLiquidInfo
	{
		// Token: 0x04008373 RID: 33651
		public int cellIdx;

		// Token: 0x04008374 RID: 33652
		public ushort elemIdx;

		// Token: 0x04008375 RID: 33653
		public byte diseaseIdx;

		// Token: 0x04008376 RID: 33654
		public byte pad0;

		// Token: 0x04008377 RID: 33655
		public float mass;

		// Token: 0x04008378 RID: 33656
		public float temperature;

		// Token: 0x04008379 RID: 33657
		public int diseaseCount;
	}

	// Token: 0x02001C65 RID: 7269
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SpawnOreInfo
	{
		// Token: 0x0400837A RID: 33658
		public int cellIdx;

		// Token: 0x0400837B RID: 33659
		public ushort elemIdx;

		// Token: 0x0400837C RID: 33660
		public byte diseaseIdx;

		// Token: 0x0400837D RID: 33661
		private byte pad0;

		// Token: 0x0400837E RID: 33662
		public float mass;

		// Token: 0x0400837F RID: 33663
		public float temperature;

		// Token: 0x04008380 RID: 33664
		public int diseaseCount;
	}

	// Token: 0x02001C66 RID: 7270
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SpawnFXInfo
	{
		// Token: 0x04008381 RID: 33665
		public int cellIdx;

		// Token: 0x04008382 RID: 33666
		public int fxHash;

		// Token: 0x04008383 RID: 33667
		public float rotation;
	}

	// Token: 0x02001C67 RID: 7271
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct UnstableCellInfo
	{
		// Token: 0x04008384 RID: 33668
		public int cellIdx;

		// Token: 0x04008385 RID: 33669
		public ushort elemIdx;

		// Token: 0x04008386 RID: 33670
		public byte fallingInfo;

		// Token: 0x04008387 RID: 33671
		public byte diseaseIdx;

		// Token: 0x04008388 RID: 33672
		public float mass;

		// Token: 0x04008389 RID: 33673
		public float temperature;

		// Token: 0x0400838A RID: 33674
		public int diseaseCount;

		// Token: 0x0200264B RID: 9803
		public enum FallingInfo
		{
			// Token: 0x0400AA44 RID: 43588
			StartedFalling,
			// Token: 0x0400AA45 RID: 43589
			StoppedFalling
		}
	}

	// Token: 0x02001C68 RID: 7272
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct NewGameFrame
	{
		// Token: 0x0400838B RID: 33675
		public float elapsedSeconds;

		// Token: 0x0400838C RID: 33676
		public int minX;

		// Token: 0x0400838D RID: 33677
		public int minY;

		// Token: 0x0400838E RID: 33678
		public int maxX;

		// Token: 0x0400838F RID: 33679
		public int maxY;

		// Token: 0x04008390 RID: 33680
		public float currentSunlightIntensity;

		// Token: 0x04008391 RID: 33681
		public float currentCosmicRadiationIntensity;
	}

	// Token: 0x02001C69 RID: 7273
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct WorldDamageInfo
	{
		// Token: 0x04008392 RID: 33682
		public int gameCell;

		// Token: 0x04008393 RID: 33683
		public int damageSourceOffset;
	}

	// Token: 0x02001C6A RID: 7274
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PipeTemperatureChange
	{
		// Token: 0x04008394 RID: 33684
		public int cellIdx;

		// Token: 0x04008395 RID: 33685
		public float temperature;
	}

	// Token: 0x02001C6B RID: 7275
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MassConsumedCallback
	{
		// Token: 0x04008396 RID: 33686
		public int callbackIdx;

		// Token: 0x04008397 RID: 33687
		public ushort elemIdx;

		// Token: 0x04008398 RID: 33688
		public byte diseaseIdx;

		// Token: 0x04008399 RID: 33689
		private byte pad0;

		// Token: 0x0400839A RID: 33690
		public float mass;

		// Token: 0x0400839B RID: 33691
		public float temperature;

		// Token: 0x0400839C RID: 33692
		public int diseaseCount;
	}

	// Token: 0x02001C6C RID: 7276
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MassEmittedCallback
	{
		// Token: 0x0400839D RID: 33693
		public int callbackIdx;

		// Token: 0x0400839E RID: 33694
		public ushort elemIdx;

		// Token: 0x0400839F RID: 33695
		public byte suceeded;

		// Token: 0x040083A0 RID: 33696
		public byte diseaseIdx;

		// Token: 0x040083A1 RID: 33697
		public float mass;

		// Token: 0x040083A2 RID: 33698
		public float temperature;

		// Token: 0x040083A3 RID: 33699
		public int diseaseCount;
	}

	// Token: 0x02001C6D RID: 7277
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseConsumptionCallback
	{
		// Token: 0x040083A4 RID: 33700
		public int callbackIdx;

		// Token: 0x040083A5 RID: 33701
		public byte diseaseIdx;

		// Token: 0x040083A6 RID: 33702
		private byte pad0;

		// Token: 0x040083A7 RID: 33703
		private byte pad1;

		// Token: 0x040083A8 RID: 33704
		private byte pad2;

		// Token: 0x040083A9 RID: 33705
		public int diseaseCount;
	}

	// Token: 0x02001C6E RID: 7278
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ComponentStateChangedMessage
	{
		// Token: 0x040083AA RID: 33706
		public int callbackIdx;

		// Token: 0x040083AB RID: 33707
		public int simHandle;
	}

	// Token: 0x02001C6F RID: 7279
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DebugProperties
	{
		// Token: 0x040083AC RID: 33708
		public float buildingTemperatureScale;

		// Token: 0x040083AD RID: 33709
		public float buildingToBuildingTemperatureScale;

		// Token: 0x040083AE RID: 33710
		public float biomeTemperatureLerpRate;

		// Token: 0x040083AF RID: 33711
		public byte isDebugEditing;

		// Token: 0x040083B0 RID: 33712
		public byte pad0;

		// Token: 0x040083B1 RID: 33713
		public byte pad1;

		// Token: 0x040083B2 RID: 33714
		public byte pad2;
	}

	// Token: 0x02001C70 RID: 7280
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct EmittedMassInfo
	{
		// Token: 0x040083B3 RID: 33715
		public ushort elemIdx;

		// Token: 0x040083B4 RID: 33716
		public byte diseaseIdx;

		// Token: 0x040083B5 RID: 33717
		public byte pad0;

		// Token: 0x040083B6 RID: 33718
		public float mass;

		// Token: 0x040083B7 RID: 33719
		public float temperature;

		// Token: 0x040083B8 RID: 33720
		public int diseaseCount;
	}

	// Token: 0x02001C71 RID: 7281
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ConsumedMassInfo
	{
		// Token: 0x040083B9 RID: 33721
		public int simHandle;

		// Token: 0x040083BA RID: 33722
		public ushort removedElemIdx;

		// Token: 0x040083BB RID: 33723
		public byte diseaseIdx;

		// Token: 0x040083BC RID: 33724
		private byte pad0;

		// Token: 0x040083BD RID: 33725
		public float mass;

		// Token: 0x040083BE RID: 33726
		public float temperature;

		// Token: 0x040083BF RID: 33727
		public int diseaseCount;
	}

	// Token: 0x02001C72 RID: 7282
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ConsumedDiseaseInfo
	{
		// Token: 0x040083C0 RID: 33728
		public int simHandle;

		// Token: 0x040083C1 RID: 33729
		public byte diseaseIdx;

		// Token: 0x040083C2 RID: 33730
		private byte pad0;

		// Token: 0x040083C3 RID: 33731
		private byte pad1;

		// Token: 0x040083C4 RID: 33732
		private byte pad2;

		// Token: 0x040083C5 RID: 33733
		public int diseaseCount;
	}

	// Token: 0x02001C73 RID: 7283
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ElementChunkInfo
	{
		// Token: 0x040083C6 RID: 33734
		public float temperature;

		// Token: 0x040083C7 RID: 33735
		public float deltaKJ;
	}

	// Token: 0x02001C74 RID: 7284
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MeltedInfo
	{
		// Token: 0x040083C8 RID: 33736
		public int handle;
	}

	// Token: 0x02001C75 RID: 7285
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CellMeltedInfo
	{
		// Token: 0x040083C9 RID: 33737
		public int gameCell;
	}

	// Token: 0x02001C76 RID: 7286
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct BuildingTemperatureInfo
	{
		// Token: 0x040083CA RID: 33738
		public int handle;

		// Token: 0x040083CB RID: 33739
		public float temperature;
	}

	// Token: 0x02001C77 RID: 7287
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct BuildingConductivityData
	{
		// Token: 0x040083CC RID: 33740
		public float temperature;

		// Token: 0x040083CD RID: 33741
		public float heatCapacity;

		// Token: 0x040083CE RID: 33742
		public float thermalConductivity;
	}

	// Token: 0x02001C78 RID: 7288
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseEmittedInfo
	{
		// Token: 0x040083CF RID: 33743
		public byte diseaseIdx;

		// Token: 0x040083D0 RID: 33744
		private byte pad0;

		// Token: 0x040083D1 RID: 33745
		private byte pad1;

		// Token: 0x040083D2 RID: 33746
		private byte pad2;

		// Token: 0x040083D3 RID: 33747
		public int count;
	}

	// Token: 0x02001C79 RID: 7289
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseConsumedInfo
	{
		// Token: 0x040083D4 RID: 33748
		public byte diseaseIdx;

		// Token: 0x040083D5 RID: 33749
		private byte pad0;

		// Token: 0x040083D6 RID: 33750
		private byte pad1;

		// Token: 0x040083D7 RID: 33751
		private byte pad2;

		// Token: 0x040083D8 RID: 33752
		public int count;
	}

	// Token: 0x02001C7A RID: 7290
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ConsumedRadiationCallback
	{
		// Token: 0x040083D9 RID: 33753
		public int callbackIdx;

		// Token: 0x040083DA RID: 33754
		public int gameCell;

		// Token: 0x040083DB RID: 33755
		public float radiation;
	}
}
