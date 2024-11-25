using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Database;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using STRINGS;

// Token: 0x02000BBD RID: 3005
public static class SimMessages
{
	// Token: 0x06005B67 RID: 23399 RVA: 0x00214B1C File Offset: 0x00212D1C
	public unsafe static void AddElementConsumer(int gameCell, ElementConsumer.Configuration configuration, SimHashes element, byte radius, int cb_handle)
	{
		Debug.Assert(Grid.IsValidCell(gameCell));
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		ushort elementIndex = ElementLoader.GetElementIndex(element);
		SimMessages.AddElementConsumerMessage* ptr = stackalloc SimMessages.AddElementConsumerMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddElementConsumerMessage))];
		ptr->cellIdx = gameCell;
		ptr->configuration = (byte)configuration;
		ptr->elementIdx = elementIndex;
		ptr->radius = radius;
		ptr->callbackIdx = cb_handle;
		Sim.SIM_HandleMessage(2024405073, sizeof(SimMessages.AddElementConsumerMessage), (byte*)ptr);
	}

	// Token: 0x06005B68 RID: 23400 RVA: 0x00214B88 File Offset: 0x00212D88
	public unsafe static void SetElementConsumerData(int sim_handle, int cell, float consumptionRate)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			return;
		}
		SimMessages.SetElementConsumerDataMessage* ptr = stackalloc SimMessages.SetElementConsumerDataMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetElementConsumerDataMessage))];
		ptr->handle = sim_handle;
		ptr->cell = cell;
		ptr->consumptionRate = consumptionRate;
		Sim.SIM_HandleMessage(1575539738, sizeof(SimMessages.SetElementConsumerDataMessage), (byte*)ptr);
	}

	// Token: 0x06005B69 RID: 23401 RVA: 0x00214BD4 File Offset: 0x00212DD4
	public unsafe static void RemoveElementConsumer(int cb_handle, int sim_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveElementConsumerMessage* ptr = stackalloc SimMessages.RemoveElementConsumerMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveElementConsumerMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(894417742, sizeof(SimMessages.RemoveElementConsumerMessage), (byte*)ptr);
	}

	// Token: 0x06005B6A RID: 23402 RVA: 0x00214C24 File Offset: 0x00212E24
	public unsafe static void AddElementEmitter(float max_pressure, int on_registered, int on_blocked = -1, int on_unblocked = -1)
	{
		SimMessages.AddElementEmitterMessage* ptr = stackalloc SimMessages.AddElementEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddElementEmitterMessage))];
		ptr->maxPressure = max_pressure;
		ptr->callbackIdx = on_registered;
		ptr->onBlockedCB = on_blocked;
		ptr->onUnblockedCB = on_unblocked;
		Sim.SIM_HandleMessage(-505471181, sizeof(SimMessages.AddElementEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06005B6B RID: 23403 RVA: 0x00214C6C File Offset: 0x00212E6C
	public unsafe static void ModifyElementEmitter(int sim_handle, int game_cell, int max_depth, SimHashes element, float emit_interval, float emit_mass, float emit_temperature, float max_pressure, byte disease_idx, int disease_count)
	{
		Debug.Assert(Grid.IsValidCell(game_cell));
		if (!Grid.IsValidCell(game_cell))
		{
			return;
		}
		ushort elementIndex = ElementLoader.GetElementIndex(element);
		SimMessages.ModifyElementEmitterMessage* ptr = stackalloc SimMessages.ModifyElementEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyElementEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->cellIdx = game_cell;
		ptr->emitInterval = emit_interval;
		ptr->emitMass = emit_mass;
		ptr->emitTemperature = emit_temperature;
		ptr->maxPressure = max_pressure;
		ptr->elementIdx = elementIndex;
		ptr->maxDepth = (byte)max_depth;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_count;
		Sim.SIM_HandleMessage(403589164, sizeof(SimMessages.ModifyElementEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06005B6C RID: 23404 RVA: 0x00214D00 File Offset: 0x00212F00
	public unsafe static void RemoveElementEmitter(int cb_handle, int sim_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveElementEmitterMessage* ptr = stackalloc SimMessages.RemoveElementEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveElementEmitterMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(-1524118282, sizeof(SimMessages.RemoveElementEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06005B6D RID: 23405 RVA: 0x00214D50 File Offset: 0x00212F50
	public unsafe static void AddRadiationEmitter(int on_registered, int game_cell, short emitRadiusX, short emitRadiusY, float emitRads, float emitRate, float emitSpeed, float emitDirection, float emitAngle, RadiationEmitter.RadiationEmitterType emitType)
	{
		SimMessages.AddRadiationEmitterMessage* ptr = stackalloc SimMessages.AddRadiationEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddRadiationEmitterMessage))];
		ptr->callbackIdx = on_registered;
		ptr->cell = game_cell;
		ptr->emitRadiusX = emitRadiusX;
		ptr->emitRadiusY = emitRadiusY;
		ptr->emitRads = emitRads;
		ptr->emitRate = emitRate;
		ptr->emitSpeed = emitSpeed;
		ptr->emitDirection = emitDirection;
		ptr->emitAngle = emitAngle;
		ptr->emitType = (int)emitType;
		Sim.SIM_HandleMessage(-1505895314, sizeof(SimMessages.AddRadiationEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06005B6E RID: 23406 RVA: 0x00214DC8 File Offset: 0x00212FC8
	public unsafe static void ModifyRadiationEmitter(int sim_handle, int game_cell, short emitRadiusX, short emitRadiusY, float emitRads, float emitRate, float emitSpeed, float emitDirection, float emitAngle, RadiationEmitter.RadiationEmitterType emitType)
	{
		if (!Grid.IsValidCell(game_cell))
		{
			return;
		}
		SimMessages.ModifyRadiationEmitterMessage* ptr = stackalloc SimMessages.ModifyRadiationEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyRadiationEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->cell = game_cell;
		ptr->callbackIdx = -1;
		ptr->emitRadiusX = emitRadiusX;
		ptr->emitRadiusY = emitRadiusY;
		ptr->emitRads = emitRads;
		ptr->emitRate = emitRate;
		ptr->emitSpeed = emitSpeed;
		ptr->emitDirection = emitDirection;
		ptr->emitAngle = emitAngle;
		ptr->emitType = (int)emitType;
		Sim.SIM_HandleMessage(-503965465, sizeof(SimMessages.ModifyRadiationEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06005B6F RID: 23407 RVA: 0x00214E50 File Offset: 0x00213050
	public unsafe static void RemoveRadiationEmitter(int cb_handle, int sim_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveRadiationEmitterMessage* ptr = stackalloc SimMessages.RemoveRadiationEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveRadiationEmitterMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(-704259919, sizeof(SimMessages.RemoveRadiationEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06005B70 RID: 23408 RVA: 0x00214EA0 File Offset: 0x002130A0
	public unsafe static void AddElementChunk(int gameCell, SimHashes element, float mass, float temperature, float surface_area, float thickness, float ground_transfer_scale, int cb_handle)
	{
		Debug.Assert(Grid.IsValidCell(gameCell));
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		if (mass * temperature > 0f)
		{
			ushort elementIndex = ElementLoader.GetElementIndex(element);
			SimMessages.AddElementChunkMessage* ptr = stackalloc SimMessages.AddElementChunkMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddElementChunkMessage))];
			ptr->gameCell = gameCell;
			ptr->callbackIdx = cb_handle;
			ptr->mass = mass;
			ptr->temperature = temperature;
			ptr->surfaceArea = surface_area;
			ptr->thickness = thickness;
			ptr->groundTransferScale = ground_transfer_scale;
			ptr->elementIdx = elementIndex;
			Sim.SIM_HandleMessage(1445724082, sizeof(SimMessages.AddElementChunkMessage), (byte*)ptr);
		}
	}

	// Token: 0x06005B71 RID: 23409 RVA: 0x00214F2C File Offset: 0x0021312C
	public unsafe static void RemoveElementChunk(int sim_handle, int cb_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveElementChunkMessage* ptr = stackalloc SimMessages.RemoveElementChunkMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveElementChunkMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(-912908555, sizeof(SimMessages.RemoveElementChunkMessage), (byte*)ptr);
	}

	// Token: 0x06005B72 RID: 23410 RVA: 0x00214F7C File Offset: 0x0021317C
	public unsafe static void SetElementChunkData(int sim_handle, float temperature, float heat_capacity)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			return;
		}
		SimMessages.SetElementChunkDataMessage* ptr = stackalloc SimMessages.SetElementChunkDataMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetElementChunkDataMessage))];
		ptr->handle = sim_handle;
		ptr->temperature = temperature;
		ptr->heatCapacity = heat_capacity;
		Sim.SIM_HandleMessage(-435115907, sizeof(SimMessages.SetElementChunkDataMessage), (byte*)ptr);
	}

	// Token: 0x06005B73 RID: 23411 RVA: 0x00214FC8 File Offset: 0x002131C8
	public unsafe static void MoveElementChunk(int sim_handle, int cell)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.MoveElementChunkMessage* ptr = stackalloc SimMessages.MoveElementChunkMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.MoveElementChunkMessage))];
		ptr->handle = sim_handle;
		ptr->gameCell = cell;
		Sim.SIM_HandleMessage(-374911358, sizeof(SimMessages.MoveElementChunkMessage), (byte*)ptr);
	}

	// Token: 0x06005B74 RID: 23412 RVA: 0x00215018 File Offset: 0x00213218
	public unsafe static void ModifyElementChunkEnergy(int sim_handle, float delta_kj)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.ModifyElementChunkEnergyMessage* ptr = stackalloc SimMessages.ModifyElementChunkEnergyMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyElementChunkEnergyMessage))];
		ptr->handle = sim_handle;
		ptr->deltaKJ = delta_kj;
		Sim.SIM_HandleMessage(1020555667, sizeof(SimMessages.ModifyElementChunkEnergyMessage), (byte*)ptr);
	}

	// Token: 0x06005B75 RID: 23413 RVA: 0x00215068 File Offset: 0x00213268
	public unsafe static void ModifyElementChunkTemperatureAdjuster(int sim_handle, float temperature, float heat_capacity, float thermal_conductivity)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.ModifyElementChunkAdjusterMessage* ptr = stackalloc SimMessages.ModifyElementChunkAdjusterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyElementChunkAdjusterMessage))];
		ptr->handle = sim_handle;
		ptr->temperature = temperature;
		ptr->heatCapacity = heat_capacity;
		ptr->thermalConductivity = thermal_conductivity;
		Sim.SIM_HandleMessage(-1387601379, sizeof(SimMessages.ModifyElementChunkAdjusterMessage), (byte*)ptr);
	}

	// Token: 0x06005B76 RID: 23414 RVA: 0x002150C4 File Offset: 0x002132C4
	public unsafe static void AddBuildingHeatExchange(Extents extents, float mass, float temperature, float thermal_conductivity, float operating_kw, ushort elem_idx, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(Grid.XYToCell(extents.x, extents.y)))
		{
			return;
		}
		int num = Grid.XYToCell(extents.x + extents.width, extents.y + extents.height);
		if (!Grid.IsValidCell(num))
		{
			Debug.LogErrorFormat("Invalid Cell [{0}] Extents [{1},{2}] [{3},{4}]", new object[]
			{
				num,
				extents.x,
				extents.y,
				extents.width,
				extents.height
			});
		}
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		SimMessages.AddBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.AddBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddBuildingHeatExchangeMessage))];
		ptr->callbackIdx = callbackIdx;
		ptr->elemIdx = elem_idx;
		ptr->mass = mass;
		ptr->temperature = temperature;
		ptr->thermalConductivity = thermal_conductivity;
		ptr->overheatTemperature = float.MaxValue;
		ptr->operatingKilowatts = operating_kw;
		ptr->minX = extents.x;
		ptr->minY = extents.y;
		ptr->maxX = extents.x + extents.width;
		ptr->maxY = extents.y + extents.height;
		Sim.SIM_HandleMessage(1739021608, sizeof(SimMessages.AddBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06005B77 RID: 23415 RVA: 0x00215200 File Offset: 0x00213400
	public unsafe static void ModifyBuildingHeatExchange(int sim_handle, Extents extents, float mass, float temperature, float thermal_conductivity, float overheat_temperature, float operating_kw, ushort element_idx)
	{
		int cell = Grid.XYToCell(extents.x, extents.y);
		Debug.Assert(Grid.IsValidCell(cell));
		if (!Grid.IsValidCell(cell))
		{
			return;
		}
		int cell2 = Grid.XYToCell(extents.x + extents.width, extents.y + extents.height);
		Debug.Assert(Grid.IsValidCell(cell2));
		if (!Grid.IsValidCell(cell2))
		{
			return;
		}
		SimMessages.ModifyBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.ModifyBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyBuildingHeatExchangeMessage))];
		ptr->callbackIdx = sim_handle;
		ptr->elemIdx = element_idx;
		ptr->mass = mass;
		ptr->temperature = temperature;
		ptr->thermalConductivity = thermal_conductivity;
		ptr->overheatTemperature = overheat_temperature;
		ptr->operatingKilowatts = operating_kw;
		ptr->minX = extents.x;
		ptr->minY = extents.y;
		ptr->maxX = extents.x + extents.width;
		ptr->maxY = extents.y + extents.height;
		Sim.SIM_HandleMessage(1818001569, sizeof(SimMessages.ModifyBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06005B78 RID: 23416 RVA: 0x002152F4 File Offset: 0x002134F4
	public unsafe static void RemoveBuildingHeatExchange(int sim_handle, int callbackIdx = -1)
	{
		SimMessages.RemoveBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RemoveBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveBuildingHeatExchangeMessage))];
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		ptr->handle = sim_handle;
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(-456116629, sizeof(SimMessages.RemoveBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06005B79 RID: 23417 RVA: 0x00215338 File Offset: 0x00213538
	public unsafe static void ModifyBuildingEnergy(int sim_handle, float delta_kj, float min_temperature, float max_temperature)
	{
		SimMessages.ModifyBuildingEnergyMessage* ptr = stackalloc SimMessages.ModifyBuildingEnergyMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyBuildingEnergyMessage))];
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		ptr->handle = sim_handle;
		ptr->deltaKJ = delta_kj;
		ptr->minTemperature = min_temperature;
		ptr->maxTemperature = max_temperature;
		Sim.SIM_HandleMessage(-1348791658, sizeof(SimMessages.ModifyBuildingEnergyMessage), (byte*)ptr);
	}

	// Token: 0x06005B7A RID: 23418 RVA: 0x0021538C File Offset: 0x0021358C
	public unsafe static void RegisterBuildingToBuildingHeatExchange(int structureTemperatureHandler, int callbackIdx = -1)
	{
		SimMessages.RegisterBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RegisterBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RegisterBuildingToBuildingHeatExchangeMessage))];
		ptr->structureTemperatureHandler = structureTemperatureHandler;
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(-1338718217, sizeof(SimMessages.RegisterBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06005B7B RID: 23419 RVA: 0x002153C8 File Offset: 0x002135C8
	public unsafe static void AddBuildingToBuildingHeatExchange(int selfHandler, int buildingInContact, int cellsInContact)
	{
		SimMessages.AddBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.AddBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddBuildingToBuildingHeatExchangeMessage))];
		ptr->selfHandler = selfHandler;
		ptr->buildingInContactHandle = buildingInContact;
		ptr->cellsInContact = cellsInContact;
		Sim.SIM_HandleMessage(-1586724321, sizeof(SimMessages.AddBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06005B7C RID: 23420 RVA: 0x00215408 File Offset: 0x00213608
	public unsafe static void RemoveBuildingInContactFromBuildingToBuildingHeatExchange(int selfHandler, int buildingToRemove)
	{
		SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage))];
		ptr->selfHandler = selfHandler;
		ptr->buildingNoLongerInContactHandler = buildingToRemove;
		Sim.SIM_HandleMessage(-1993857213, sizeof(SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06005B7D RID: 23421 RVA: 0x00215444 File Offset: 0x00213644
	public unsafe static void RemoveBuildingToBuildingHeatExchange(int selfHandler, int callback = -1)
	{
		SimMessages.RemoveBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RemoveBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveBuildingToBuildingHeatExchangeMessage))];
		ptr->callbackIdx = callback;
		ptr->selfHandler = selfHandler;
		Sim.SIM_HandleMessage(697100730, sizeof(SimMessages.RemoveBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06005B7E RID: 23422 RVA: 0x00215480 File Offset: 0x00213680
	public unsafe static void AddDiseaseEmitter(int callbackIdx)
	{
		SimMessages.AddDiseaseEmitterMessage* ptr = stackalloc SimMessages.AddDiseaseEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddDiseaseEmitterMessage))];
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(1486783027, sizeof(SimMessages.AddDiseaseEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06005B7F RID: 23423 RVA: 0x002154B4 File Offset: 0x002136B4
	public unsafe static void ModifyDiseaseEmitter(int sim_handle, int cell, byte range, byte disease_idx, float emit_interval, int emit_count)
	{
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.ModifyDiseaseEmitterMessage* ptr = stackalloc SimMessages.ModifyDiseaseEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyDiseaseEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->gameCell = cell;
		ptr->maxDepth = range;
		ptr->diseaseIdx = disease_idx;
		ptr->emitInterval = emit_interval;
		ptr->emitCount = emit_count;
		Sim.SIM_HandleMessage(-1899123924, sizeof(SimMessages.ModifyDiseaseEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06005B80 RID: 23424 RVA: 0x00215518 File Offset: 0x00213718
	public unsafe static void RemoveDiseaseEmitter(int cb_handle, int sim_handle)
	{
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveDiseaseEmitterMessage* ptr = stackalloc SimMessages.RemoveDiseaseEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveDiseaseEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->callbackIdx = cb_handle;
		Sim.SIM_HandleMessage(468135926, sizeof(SimMessages.RemoveDiseaseEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06005B81 RID: 23425 RVA: 0x0021555C File Offset: 0x0021375C
	public unsafe static void SetSavedOptionValue(SimMessages.SimSavedOptions option, int zero_or_one)
	{
		SimMessages.SetSavedOptionsMessage* ptr = stackalloc SimMessages.SetSavedOptionsMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetSavedOptionsMessage))];
		if (zero_or_one == 0)
		{
			SimMessages.SetSavedOptionsMessage* ptr2 = ptr;
			ptr2->clearBits = (ptr2->clearBits | (byte)option);
			ptr->setBits = 0;
		}
		else
		{
			ptr->clearBits = 0;
			SimMessages.SetSavedOptionsMessage* ptr3 = ptr;
			ptr3->setBits = (ptr3->setBits | (byte)option);
		}
		Sim.SIM_HandleMessage(1154135737, sizeof(SimMessages.SetSavedOptionsMessage), (byte*)ptr);
	}

	// Token: 0x06005B82 RID: 23426 RVA: 0x002155B4 File Offset: 0x002137B4
	private static void WriteKleiString(this BinaryWriter writer, string str)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		writer.Write(bytes.Length);
		if (bytes.Length != 0)
		{
			writer.Write(bytes);
		}
	}

	// Token: 0x06005B83 RID: 23427 RVA: 0x002155E4 File Offset: 0x002137E4
	public unsafe static void CreateSimElementsTable(List<Element> elements)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(Sim.Element)) * elements.Count);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		Debug.Assert(elements.Count < 65535, "SimDLL internals assume there are fewer than 65535 elements");
		binaryWriter.Write(elements.Count);
		for (int i = 0; i < elements.Count; i++)
		{
			Sim.Element element = new Sim.Element(elements[i], elements);
			element.Write(binaryWriter);
		}
		for (int j = 0; j < elements.Count; j++)
		{
			binaryWriter.WriteKleiString(UI.StripLinkFormatting(elements[j].name));
		}
		byte[] buffer = memoryStream.GetBuffer();
		byte[] array;
		byte* msg;
		if ((array = buffer) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		Sim.SIM_HandleMessage(1108437482, buffer.Length, msg);
		array = null;
	}

	// Token: 0x06005B84 RID: 23428 RVA: 0x002156D4 File Offset: 0x002138D4
	public unsafe static void CreateDiseaseTable(Diseases diseases)
	{
		MemoryStream memoryStream = new MemoryStream(1024);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(diseases.Count);
		List<Element> elements = ElementLoader.elements;
		binaryWriter.Write(elements.Count);
		for (int i = 0; i < diseases.Count; i++)
		{
			Disease disease = diseases[i];
			binaryWriter.WriteKleiString(UI.StripLinkFormatting(disease.Name));
			binaryWriter.Write(disease.id.GetHashCode());
			binaryWriter.Write(disease.strength);
			disease.temperatureRange.Write(binaryWriter);
			disease.temperatureHalfLives.Write(binaryWriter);
			disease.pressureRange.Write(binaryWriter);
			disease.pressureHalfLives.Write(binaryWriter);
			binaryWriter.Write(disease.radiationKillRate);
			for (int j = 0; j < elements.Count; j++)
			{
				ElemGrowthInfo elemGrowthInfo = disease.elemGrowthInfo[j];
				elemGrowthInfo.Write(binaryWriter);
			}
		}
		byte[] array;
		byte* msg;
		if ((array = memoryStream.GetBuffer()) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		Sim.SIM_HandleMessage(825301935, (int)memoryStream.Length, msg);
		array = null;
	}

	// Token: 0x06005B85 RID: 23429 RVA: 0x00215810 File Offset: 0x00213A10
	public unsafe static void DefineWorldOffsets(List<SimMessages.WorldOffsetData> worldOffsets)
	{
		MemoryStream memoryStream = new MemoryStream(1024);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(worldOffsets.Count);
		foreach (SimMessages.WorldOffsetData worldOffsetData in worldOffsets)
		{
			binaryWriter.Write(worldOffsetData.worldOffsetX);
			binaryWriter.Write(worldOffsetData.worldOffsetY);
			binaryWriter.Write(worldOffsetData.worldSizeX);
			binaryWriter.Write(worldOffsetData.worldSizeY);
		}
		byte[] array;
		byte* msg;
		if ((array = memoryStream.GetBuffer()) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		Sim.SIM_HandleMessage(-895846551, (int)memoryStream.Length, msg);
		array = null;
	}

	// Token: 0x06005B86 RID: 23430 RVA: 0x002158E0 File Offset: 0x00213AE0
	public static void SimDataInitializeFromCells(int width, int height, Sim.Cell[] cells, float[] bgTemp, Sim.DiseaseCell[] dc, bool headless)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(Sim.Cell)) * width * height + Marshal.SizeOf(typeof(float)) * width * height + Marshal.SizeOf(typeof(Sim.DiseaseCell)) * width * height);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(width);
		binaryWriter.Write(height);
		bool value = Sim.IsRadiationEnabled();
		binaryWriter.Write(value);
		binaryWriter.Write(headless);
		int num = width * height;
		for (int i = 0; i < num; i++)
		{
			cells[i].Write(binaryWriter);
		}
		for (int j = 0; j < num; j++)
		{
			binaryWriter.Write(bgTemp[j]);
		}
		for (int k = 0; k < num; k++)
		{
			dc[k].Write(binaryWriter);
		}
		byte[] buffer = memoryStream.GetBuffer();
		Sim.HandleMessage(SimMessageHashes.SimData_InitializeFromCells, buffer.Length, buffer);
	}

	// Token: 0x06005B87 RID: 23431 RVA: 0x002159EC File Offset: 0x00213BEC
	public static void SimDataResizeGridAndInitializeVacuumCells(Vector2I grid_size, int width, int height, int x_offset, int y_offset)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(int)));
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(grid_size.x);
		binaryWriter.Write(grid_size.y);
		binaryWriter.Write(width);
		binaryWriter.Write(height);
		binaryWriter.Write(x_offset);
		binaryWriter.Write(y_offset);
		byte[] buffer = memoryStream.GetBuffer();
		Sim.HandleMessage(SimMessageHashes.SimData_ResizeAndInitializeVacuumCells, buffer.Length, buffer);
	}

	// Token: 0x06005B88 RID: 23432 RVA: 0x00215A6C File Offset: 0x00213C6C
	public static void SimDataFreeCells(int width, int height, int x_offset, int y_offset)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(int)));
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(width);
		binaryWriter.Write(height);
		binaryWriter.Write(x_offset);
		binaryWriter.Write(y_offset);
		byte[] buffer = memoryStream.GetBuffer();
		Sim.HandleMessage(SimMessageHashes.SimData_FreeCells, buffer.Length, buffer);
	}

	// Token: 0x06005B89 RID: 23433 RVA: 0x00215AD4 File Offset: 0x00213CD4
	public unsafe static void Dig(int gameCell, int callbackIdx = -1, bool skipEvent = false)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.DigMessage* ptr = stackalloc SimMessages.DigMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.DigMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->skipEvent = skipEvent;
		Sim.SIM_HandleMessage(833038498, sizeof(SimMessages.DigMessage), (byte*)ptr);
	}

	// Token: 0x06005B8A RID: 23434 RVA: 0x00215B20 File Offset: 0x00213D20
	public unsafe static void SetInsulation(int gameCell, float value)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.SetCellFloatValueMessage* ptr = stackalloc SimMessages.SetCellFloatValueMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetCellFloatValueMessage))];
		ptr->cellIdx = gameCell;
		ptr->value = value;
		Sim.SIM_HandleMessage(-898773121, sizeof(SimMessages.SetCellFloatValueMessage), (byte*)ptr);
	}

	// Token: 0x06005B8B RID: 23435 RVA: 0x00215B64 File Offset: 0x00213D64
	public unsafe static void SetStrength(int gameCell, int weight, float strengthMultiplier)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.SetCellFloatValueMessage* ptr = stackalloc SimMessages.SetCellFloatValueMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetCellFloatValueMessage))];
		ptr->cellIdx = gameCell;
		int num = (int)(strengthMultiplier * 4f) & 127;
		int num2 = (weight & 1) << 7 | num;
		ptr->value = (float)((byte)num2);
		Sim.SIM_HandleMessage(1593243982, sizeof(SimMessages.SetCellFloatValueMessage), (byte*)ptr);
	}

	// Token: 0x06005B8C RID: 23436 RVA: 0x00215BBC File Offset: 0x00213DBC
	public unsafe static void SetCellProperties(int gameCell, byte properties)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.CellPropertiesMessage* ptr = stackalloc SimMessages.CellPropertiesMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellPropertiesMessage))];
		ptr->cellIdx = gameCell;
		ptr->properties = properties;
		ptr->set = 1;
		Sim.SIM_HandleMessage(-469311643, sizeof(SimMessages.CellPropertiesMessage), (byte*)ptr);
	}

	// Token: 0x06005B8D RID: 23437 RVA: 0x00215C08 File Offset: 0x00213E08
	public unsafe static void ClearCellProperties(int gameCell, byte properties)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.CellPropertiesMessage* ptr = stackalloc SimMessages.CellPropertiesMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellPropertiesMessage))];
		ptr->cellIdx = gameCell;
		ptr->properties = properties;
		ptr->set = 0;
		Sim.SIM_HandleMessage(-469311643, sizeof(SimMessages.CellPropertiesMessage), (byte*)ptr);
	}

	// Token: 0x06005B8E RID: 23438 RVA: 0x00215C54 File Offset: 0x00213E54
	public unsafe static void ModifyCell(int gameCell, ushort elementIdx, float temperature, float mass, byte disease_idx, int disease_count, SimMessages.ReplaceType replace_type = SimMessages.ReplaceType.None, bool do_vertical_solid_displacement = false, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		Element element = ElementLoader.elements[(int)elementIdx];
		if (element.maxMass == 0f && mass > element.maxMass)
		{
			Debug.LogWarningFormat("Invalid cell modification (mass greater than element maximum): Cell={0}, EIdx={1}, T={2}, M={3}, {4} max mass = {5}", new object[]
			{
				gameCell,
				elementIdx,
				temperature,
				mass,
				element.id,
				element.maxMass
			});
			mass = element.maxMass;
		}
		if (temperature < 0f || temperature > 10000f)
		{
			Debug.LogWarningFormat("Invalid cell modification (temp out of bounds): Cell={0}, EIdx={1}, T={2}, M={3}, {4} default temp = {5}", new object[]
			{
				gameCell,
				elementIdx,
				temperature,
				mass,
				element.id,
				element.defaultValues.temperature
			});
			temperature = element.defaultValues.temperature;
		}
		if (temperature == 0f && mass > 0f)
		{
			Debug.LogWarningFormat("Invalid cell modification (zero temp with non-zero mass): Cell={0}, EIdx={1}, T={2}, M={3}, {4} default temp = {5}", new object[]
			{
				gameCell,
				elementIdx,
				temperature,
				mass,
				element.id,
				element.defaultValues.temperature
			});
			temperature = element.defaultValues.temperature;
		}
		SimMessages.ModifyCellMessage* ptr = stackalloc SimMessages.ModifyCellMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyCellMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->temperature = temperature;
		ptr->mass = mass;
		ptr->elementIdx = elementIdx;
		ptr->replaceType = (byte)replace_type;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_count;
		ptr->addSubType = (do_vertical_solid_displacement ? 0 : 1);
		Sim.SIM_HandleMessage(-1252920804, sizeof(SimMessages.ModifyCellMessage), (byte*)ptr);
	}

	// Token: 0x06005B8F RID: 23439 RVA: 0x00215E34 File Offset: 0x00214034
	public unsafe static void ModifyDiseaseOnCell(int gameCell, byte disease_idx, int disease_delta)
	{
		SimMessages.CellDiseaseModification* ptr = stackalloc SimMessages.CellDiseaseModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellDiseaseModification))];
		ptr->cellIdx = gameCell;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_delta;
		Sim.SIM_HandleMessage(-1853671274, sizeof(SimMessages.CellDiseaseModification), (byte*)ptr);
	}

	// Token: 0x06005B90 RID: 23440 RVA: 0x00215E74 File Offset: 0x00214074
	public unsafe static void ModifyRadiationOnCell(int gameCell, float radiationDelta, int callbackIdx = -1)
	{
		SimMessages.CellRadiationModification* ptr = stackalloc SimMessages.CellRadiationModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellRadiationModification))];
		ptr->cellIdx = gameCell;
		ptr->radiationDelta = radiationDelta;
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(-1914877797, sizeof(SimMessages.CellRadiationModification), (byte*)ptr);
	}

	// Token: 0x06005B91 RID: 23441 RVA: 0x00215EB4 File Offset: 0x002140B4
	public unsafe static void ModifyRadiationParams(RadiationParams type, float value)
	{
		SimMessages.RadiationParamsModification* ptr = stackalloc SimMessages.RadiationParamsModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RadiationParamsModification))];
		ptr->RadiationParamsType = (int)type;
		ptr->value = value;
		Sim.SIM_HandleMessage(377112707, sizeof(SimMessages.RadiationParamsModification), (byte*)ptr);
	}

	// Token: 0x06005B92 RID: 23442 RVA: 0x00215EED File Offset: 0x002140ED
	public static ushort GetElementIndex(SimHashes element)
	{
		return ElementLoader.GetElementIndex(element);
	}

	// Token: 0x06005B93 RID: 23443 RVA: 0x00215EF8 File Offset: 0x002140F8
	public unsafe static void ConsumeMass(int gameCell, SimHashes element, float mass, byte radius, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		ushort elementIndex = ElementLoader.GetElementIndex(element);
		SimMessages.MassConsumptionMessage* ptr = stackalloc SimMessages.MassConsumptionMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.MassConsumptionMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->mass = mass;
		ptr->elementIdx = elementIndex;
		ptr->radius = radius;
		Sim.SIM_HandleMessage(1727657959, sizeof(SimMessages.MassConsumptionMessage), (byte*)ptr);
	}

	// Token: 0x06005B94 RID: 23444 RVA: 0x00215F58 File Offset: 0x00214158
	public unsafe static void EmitMass(int gameCell, ushort element_idx, float mass, float temperature, byte disease_idx, int disease_count, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.MassEmissionMessage* ptr = stackalloc SimMessages.MassEmissionMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.MassEmissionMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->mass = mass;
		ptr->temperature = temperature;
		ptr->elementIdx = element_idx;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_count;
		Sim.SIM_HandleMessage(797274363, sizeof(SimMessages.MassEmissionMessage), (byte*)ptr);
	}

	// Token: 0x06005B95 RID: 23445 RVA: 0x00215FC0 File Offset: 0x002141C0
	public unsafe static void ConsumeDisease(int game_cell, float percent_to_consume, int max_to_consume, int callback_idx)
	{
		if (!Grid.IsValidCell(game_cell))
		{
			return;
		}
		SimMessages.ConsumeDiseaseMessage* ptr = stackalloc SimMessages.ConsumeDiseaseMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ConsumeDiseaseMessage))];
		ptr->callbackIdx = callback_idx;
		ptr->gameCell = game_cell;
		ptr->percentToConsume = percent_to_consume;
		ptr->maxToConsume = max_to_consume;
		Sim.SIM_HandleMessage(-1019841536, sizeof(SimMessages.ConsumeDiseaseMessage), (byte*)ptr);
	}

	// Token: 0x06005B96 RID: 23446 RVA: 0x00216010 File Offset: 0x00214210
	public static void AddRemoveSubstance(int gameCell, SimHashes new_element, CellAddRemoveSubstanceEvent ev, float mass, float temperature, byte disease_idx, int disease_count, bool do_vertical_solid_displacement = true, int callbackIdx = -1)
	{
		ushort elementIndex = SimMessages.GetElementIndex(new_element);
		SimMessages.AddRemoveSubstance(gameCell, elementIndex, ev, mass, temperature, disease_idx, disease_count, do_vertical_solid_displacement, callbackIdx);
	}

	// Token: 0x06005B97 RID: 23447 RVA: 0x00216038 File Offset: 0x00214238
	public static void AddRemoveSubstance(int gameCell, ushort elementIdx, CellAddRemoveSubstanceEvent ev, float mass, float temperature, byte disease_idx, int disease_count, bool do_vertical_solid_displacement = true, int callbackIdx = -1)
	{
		if (elementIdx == 65535)
		{
			return;
		}
		Element element = ElementLoader.elements[(int)elementIdx];
		float temperature2 = (temperature != -1f) ? temperature : element.defaultValues.temperature;
		SimMessages.ModifyCell(gameCell, elementIdx, temperature2, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, do_vertical_solid_displacement, callbackIdx);
	}

	// Token: 0x06005B98 RID: 23448 RVA: 0x00216088 File Offset: 0x00214288
	public static void ReplaceElement(int gameCell, SimHashes new_element, CellElementEvent ev, float mass, float temperature = -1f, byte diseaseIdx = 255, int diseaseCount = 0, int callbackIdx = -1)
	{
		ushort elementIndex = SimMessages.GetElementIndex(new_element);
		if (elementIndex != 65535)
		{
			Element element = ElementLoader.elements[(int)elementIndex];
			float temperature2 = (temperature != -1f) ? temperature : element.defaultValues.temperature;
			SimMessages.ModifyCell(gameCell, elementIndex, temperature2, mass, diseaseIdx, diseaseCount, SimMessages.ReplaceType.Replace, false, callbackIdx);
		}
	}

	// Token: 0x06005B99 RID: 23449 RVA: 0x002160DC File Offset: 0x002142DC
	public static void ReplaceAndDisplaceElement(int gameCell, SimHashes new_element, CellElementEvent ev, float mass, float temperature = -1f, byte disease_idx = 255, int disease_count = 0, int callbackIdx = -1)
	{
		ushort elementIndex = SimMessages.GetElementIndex(new_element);
		if (elementIndex != 65535)
		{
			Element element = ElementLoader.elements[(int)elementIndex];
			float temperature2 = (temperature != -1f) ? temperature : element.defaultValues.temperature;
			SimMessages.ModifyCell(gameCell, elementIndex, temperature2, mass, disease_idx, disease_count, SimMessages.ReplaceType.ReplaceAndDisplace, false, callbackIdx);
		}
	}

	// Token: 0x06005B9A RID: 23450 RVA: 0x00216130 File Offset: 0x00214330
	public unsafe static void ModifyEnergy(int gameCell, float kilojoules, float max_temperature, SimMessages.EnergySourceID id)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		if (max_temperature <= 0f)
		{
			Debug.LogError("invalid max temperature for cell energy modification");
			return;
		}
		SimMessages.ModifyCellEnergyMessage* ptr = stackalloc SimMessages.ModifyCellEnergyMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyCellEnergyMessage))];
		ptr->cellIdx = gameCell;
		ptr->kilojoules = kilojoules;
		ptr->maxTemperature = max_temperature;
		ptr->id = (int)id;
		Sim.SIM_HandleMessage(818320644, sizeof(SimMessages.ModifyCellEnergyMessage), (byte*)ptr);
	}

	// Token: 0x06005B9B RID: 23451 RVA: 0x00216194 File Offset: 0x00214394
	public static void ModifyMass(int gameCell, float mass, byte disease_idx, int disease_count, CellModifyMassEvent ev, float temperature = -1f, SimHashes element = SimHashes.Vacuum)
	{
		if (element != SimHashes.Vacuum)
		{
			ushort elementIndex = SimMessages.GetElementIndex(element);
			if (elementIndex != 65535)
			{
				if (temperature == -1f)
				{
					temperature = ElementLoader.elements[(int)elementIndex].defaultValues.temperature;
				}
				SimMessages.ModifyCell(gameCell, elementIndex, temperature, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, false, -1);
				return;
			}
		}
		else
		{
			SimMessages.ModifyCell(gameCell, 0, temperature, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, false, -1);
		}
	}

	// Token: 0x06005B9C RID: 23452 RVA: 0x002161FC File Offset: 0x002143FC
	public unsafe static void CreateElementInteractions(SimMessages.ElementInteraction[] interactions)
	{
		fixed (SimMessages.ElementInteraction[] array = interactions)
		{
			SimMessages.ElementInteraction* interactions2;
			if (interactions == null || array.Length == 0)
			{
				interactions2 = null;
			}
			else
			{
				interactions2 = &array[0];
			}
			SimMessages.CreateElementInteractionsMsg* ptr = stackalloc SimMessages.CreateElementInteractionsMsg[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CreateElementInteractionsMsg))];
			ptr->numInteractions = interactions.Length;
			ptr->interactions = interactions2;
			Sim.SIM_HandleMessage(-930289787, sizeof(SimMessages.CreateElementInteractionsMsg), (byte*)ptr);
		}
	}

	// Token: 0x06005B9D RID: 23453 RVA: 0x00216254 File Offset: 0x00214454
	public unsafe static void NewGameFrame(float elapsed_seconds, List<Game.SimActiveRegion> activeRegions)
	{
		Debug.Assert(activeRegions.Count > 0, "NewGameFrame cannot be called with zero activeRegions");
		Sim.NewGameFrame* ptr = stackalloc Sim.NewGameFrame[checked(unchecked((UIntPtr)activeRegions.Count) * (UIntPtr)sizeof(Sim.NewGameFrame))];
		Sim.NewGameFrame* ptr2 = ptr;
		foreach (Game.SimActiveRegion simActiveRegion in activeRegions)
		{
			Pair<Vector2I, Vector2I> region = simActiveRegion.region;
			region.first = new Vector2I(MathUtil.Clamp(0, Grid.WidthInCells - 1, simActiveRegion.region.first.x), MathUtil.Clamp(0, Grid.HeightInCells - 1, simActiveRegion.region.first.y));
			region.second = new Vector2I(MathUtil.Clamp(0, Grid.WidthInCells, simActiveRegion.region.second.x), MathUtil.Clamp(0, Grid.HeightInCells - 1, simActiveRegion.region.second.y));
			ptr2->elapsedSeconds = elapsed_seconds;
			ptr2->minX = region.first.x;
			ptr2->minY = region.first.y;
			ptr2->maxX = region.second.x;
			ptr2->maxY = region.second.y;
			ptr2->currentSunlightIntensity = simActiveRegion.currentSunlightIntensity;
			ptr2->currentCosmicRadiationIntensity = simActiveRegion.currentCosmicRadiationIntensity;
			ptr2++;
		}
		Sim.SIM_HandleMessage(-775326397, sizeof(Sim.NewGameFrame) * activeRegions.Count, (byte*)ptr);
	}

	// Token: 0x06005B9E RID: 23454 RVA: 0x002163F0 File Offset: 0x002145F0
	public unsafe static void SetDebugProperties(Sim.DebugProperties properties)
	{
		Sim.DebugProperties* ptr = stackalloc Sim.DebugProperties[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(Sim.DebugProperties))];
		*ptr = properties;
		ptr->buildingTemperatureScale = properties.buildingTemperatureScale;
		ptr->buildingToBuildingTemperatureScale = properties.buildingToBuildingTemperatureScale;
		Sim.SIM_HandleMessage(-1683118492, sizeof(Sim.DebugProperties), (byte*)ptr);
	}

	// Token: 0x06005B9F RID: 23455 RVA: 0x0021643C File Offset: 0x0021463C
	public unsafe static void ModifyCellWorldZone(int cell, byte zone_id)
	{
		SimMessages.CellWorldZoneModification* ptr = stackalloc SimMessages.CellWorldZoneModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellWorldZoneModification))];
		ptr->cell = cell;
		ptr->zoneID = zone_id;
		Sim.SIM_HandleMessage(-449718014, sizeof(SimMessages.CellWorldZoneModification), (byte*)ptr);
	}

	// Token: 0x04003C52 RID: 15442
	public const int InvalidCallback = -1;

	// Token: 0x04003C53 RID: 15443
	public const float STATE_TRANSITION_TEMPERATURE_BUFER = 3f;

	// Token: 0x02001C7B RID: 7291
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddElementConsumerMessage
	{
		// Token: 0x040083DC RID: 33756
		public int cellIdx;

		// Token: 0x040083DD RID: 33757
		public int callbackIdx;

		// Token: 0x040083DE RID: 33758
		public byte radius;

		// Token: 0x040083DF RID: 33759
		public byte configuration;

		// Token: 0x040083E0 RID: 33760
		public ushort elementIdx;
	}

	// Token: 0x02001C7C RID: 7292
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetElementConsumerDataMessage
	{
		// Token: 0x040083E1 RID: 33761
		public int handle;

		// Token: 0x040083E2 RID: 33762
		public int cell;

		// Token: 0x040083E3 RID: 33763
		public float consumptionRate;
	}

	// Token: 0x02001C7D RID: 7293
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveElementConsumerMessage
	{
		// Token: 0x040083E4 RID: 33764
		public int handle;

		// Token: 0x040083E5 RID: 33765
		public int callbackIdx;
	}

	// Token: 0x02001C7E RID: 7294
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddElementEmitterMessage
	{
		// Token: 0x040083E6 RID: 33766
		public float maxPressure;

		// Token: 0x040083E7 RID: 33767
		public int callbackIdx;

		// Token: 0x040083E8 RID: 33768
		public int onBlockedCB;

		// Token: 0x040083E9 RID: 33769
		public int onUnblockedCB;
	}

	// Token: 0x02001C7F RID: 7295
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyElementEmitterMessage
	{
		// Token: 0x040083EA RID: 33770
		public int handle;

		// Token: 0x040083EB RID: 33771
		public int cellIdx;

		// Token: 0x040083EC RID: 33772
		public float emitInterval;

		// Token: 0x040083ED RID: 33773
		public float emitMass;

		// Token: 0x040083EE RID: 33774
		public float emitTemperature;

		// Token: 0x040083EF RID: 33775
		public float maxPressure;

		// Token: 0x040083F0 RID: 33776
		public int diseaseCount;

		// Token: 0x040083F1 RID: 33777
		public ushort elementIdx;

		// Token: 0x040083F2 RID: 33778
		public byte maxDepth;

		// Token: 0x040083F3 RID: 33779
		public byte diseaseIdx;
	}

	// Token: 0x02001C80 RID: 7296
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveElementEmitterMessage
	{
		// Token: 0x040083F4 RID: 33780
		public int handle;

		// Token: 0x040083F5 RID: 33781
		public int callbackIdx;
	}

	// Token: 0x02001C81 RID: 7297
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddRadiationEmitterMessage
	{
		// Token: 0x040083F6 RID: 33782
		public int callbackIdx;

		// Token: 0x040083F7 RID: 33783
		public int cell;

		// Token: 0x040083F8 RID: 33784
		public short emitRadiusX;

		// Token: 0x040083F9 RID: 33785
		public short emitRadiusY;

		// Token: 0x040083FA RID: 33786
		public float emitRads;

		// Token: 0x040083FB RID: 33787
		public float emitRate;

		// Token: 0x040083FC RID: 33788
		public float emitSpeed;

		// Token: 0x040083FD RID: 33789
		public float emitDirection;

		// Token: 0x040083FE RID: 33790
		public float emitAngle;

		// Token: 0x040083FF RID: 33791
		public int emitType;
	}

	// Token: 0x02001C82 RID: 7298
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyRadiationEmitterMessage
	{
		// Token: 0x04008400 RID: 33792
		public int handle;

		// Token: 0x04008401 RID: 33793
		public int cell;

		// Token: 0x04008402 RID: 33794
		public int callbackIdx;

		// Token: 0x04008403 RID: 33795
		public short emitRadiusX;

		// Token: 0x04008404 RID: 33796
		public short emitRadiusY;

		// Token: 0x04008405 RID: 33797
		public float emitRads;

		// Token: 0x04008406 RID: 33798
		public float emitRate;

		// Token: 0x04008407 RID: 33799
		public float emitSpeed;

		// Token: 0x04008408 RID: 33800
		public float emitDirection;

		// Token: 0x04008409 RID: 33801
		public float emitAngle;

		// Token: 0x0400840A RID: 33802
		public int emitType;
	}

	// Token: 0x02001C83 RID: 7299
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveRadiationEmitterMessage
	{
		// Token: 0x0400840B RID: 33803
		public int handle;

		// Token: 0x0400840C RID: 33804
		public int callbackIdx;
	}

	// Token: 0x02001C84 RID: 7300
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddElementChunkMessage
	{
		// Token: 0x0400840D RID: 33805
		public int gameCell;

		// Token: 0x0400840E RID: 33806
		public int callbackIdx;

		// Token: 0x0400840F RID: 33807
		public float mass;

		// Token: 0x04008410 RID: 33808
		public float temperature;

		// Token: 0x04008411 RID: 33809
		public float surfaceArea;

		// Token: 0x04008412 RID: 33810
		public float thickness;

		// Token: 0x04008413 RID: 33811
		public float groundTransferScale;

		// Token: 0x04008414 RID: 33812
		public ushort elementIdx;

		// Token: 0x04008415 RID: 33813
		public byte pad0;

		// Token: 0x04008416 RID: 33814
		public byte pad1;
	}

	// Token: 0x02001C85 RID: 7301
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveElementChunkMessage
	{
		// Token: 0x04008417 RID: 33815
		public int handle;

		// Token: 0x04008418 RID: 33816
		public int callbackIdx;
	}

	// Token: 0x02001C86 RID: 7302
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetElementChunkDataMessage
	{
		// Token: 0x04008419 RID: 33817
		public int handle;

		// Token: 0x0400841A RID: 33818
		public float temperature;

		// Token: 0x0400841B RID: 33819
		public float heatCapacity;
	}

	// Token: 0x02001C87 RID: 7303
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct MoveElementChunkMessage
	{
		// Token: 0x0400841C RID: 33820
		public int handle;

		// Token: 0x0400841D RID: 33821
		public int gameCell;
	}

	// Token: 0x02001C88 RID: 7304
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyElementChunkEnergyMessage
	{
		// Token: 0x0400841E RID: 33822
		public int handle;

		// Token: 0x0400841F RID: 33823
		public float deltaKJ;
	}

	// Token: 0x02001C89 RID: 7305
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyElementChunkAdjusterMessage
	{
		// Token: 0x04008420 RID: 33824
		public int handle;

		// Token: 0x04008421 RID: 33825
		public float temperature;

		// Token: 0x04008422 RID: 33826
		public float heatCapacity;

		// Token: 0x04008423 RID: 33827
		public float thermalConductivity;
	}

	// Token: 0x02001C8A RID: 7306
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AddBuildingHeatExchangeMessage
	{
		// Token: 0x04008424 RID: 33828
		public int callbackIdx;

		// Token: 0x04008425 RID: 33829
		public ushort elemIdx;

		// Token: 0x04008426 RID: 33830
		public byte pad0;

		// Token: 0x04008427 RID: 33831
		public byte pad1;

		// Token: 0x04008428 RID: 33832
		public float mass;

		// Token: 0x04008429 RID: 33833
		public float temperature;

		// Token: 0x0400842A RID: 33834
		public float thermalConductivity;

		// Token: 0x0400842B RID: 33835
		public float overheatTemperature;

		// Token: 0x0400842C RID: 33836
		public float operatingKilowatts;

		// Token: 0x0400842D RID: 33837
		public int minX;

		// Token: 0x0400842E RID: 33838
		public int minY;

		// Token: 0x0400842F RID: 33839
		public int maxX;

		// Token: 0x04008430 RID: 33840
		public int maxY;
	}

	// Token: 0x02001C8B RID: 7307
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ModifyBuildingHeatExchangeMessage
	{
		// Token: 0x04008431 RID: 33841
		public int callbackIdx;

		// Token: 0x04008432 RID: 33842
		public ushort elemIdx;

		// Token: 0x04008433 RID: 33843
		public byte pad0;

		// Token: 0x04008434 RID: 33844
		public byte pad1;

		// Token: 0x04008435 RID: 33845
		public float mass;

		// Token: 0x04008436 RID: 33846
		public float temperature;

		// Token: 0x04008437 RID: 33847
		public float thermalConductivity;

		// Token: 0x04008438 RID: 33848
		public float overheatTemperature;

		// Token: 0x04008439 RID: 33849
		public float operatingKilowatts;

		// Token: 0x0400843A RID: 33850
		public int minX;

		// Token: 0x0400843B RID: 33851
		public int minY;

		// Token: 0x0400843C RID: 33852
		public int maxX;

		// Token: 0x0400843D RID: 33853
		public int maxY;
	}

	// Token: 0x02001C8C RID: 7308
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ModifyBuildingEnergyMessage
	{
		// Token: 0x0400843E RID: 33854
		public int handle;

		// Token: 0x0400843F RID: 33855
		public float deltaKJ;

		// Token: 0x04008440 RID: 33856
		public float minTemperature;

		// Token: 0x04008441 RID: 33857
		public float maxTemperature;
	}

	// Token: 0x02001C8D RID: 7309
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveBuildingHeatExchangeMessage
	{
		// Token: 0x04008442 RID: 33858
		public int handle;

		// Token: 0x04008443 RID: 33859
		public int callbackIdx;
	}

	// Token: 0x02001C8E RID: 7310
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RegisterBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x04008444 RID: 33860
		public int callbackIdx;

		// Token: 0x04008445 RID: 33861
		public int structureTemperatureHandler;
	}

	// Token: 0x02001C8F RID: 7311
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AddBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x04008446 RID: 33862
		public int selfHandler;

		// Token: 0x04008447 RID: 33863
		public int buildingInContactHandle;

		// Token: 0x04008448 RID: 33864
		public int cellsInContact;
	}

	// Token: 0x02001C90 RID: 7312
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x04008449 RID: 33865
		public int selfHandler;

		// Token: 0x0400844A RID: 33866
		public int buildingNoLongerInContactHandler;
	}

	// Token: 0x02001C91 RID: 7313
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x0400844B RID: 33867
		public int callbackIdx;

		// Token: 0x0400844C RID: 33868
		public int selfHandler;
	}

	// Token: 0x02001C92 RID: 7314
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AddDiseaseEmitterMessage
	{
		// Token: 0x0400844D RID: 33869
		public int callbackIdx;
	}

	// Token: 0x02001C93 RID: 7315
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ModifyDiseaseEmitterMessage
	{
		// Token: 0x0400844E RID: 33870
		public int handle;

		// Token: 0x0400844F RID: 33871
		public int gameCell;

		// Token: 0x04008450 RID: 33872
		public byte diseaseIdx;

		// Token: 0x04008451 RID: 33873
		public byte maxDepth;

		// Token: 0x04008452 RID: 33874
		private byte pad0;

		// Token: 0x04008453 RID: 33875
		private byte pad1;

		// Token: 0x04008454 RID: 33876
		public float emitInterval;

		// Token: 0x04008455 RID: 33877
		public int emitCount;
	}

	// Token: 0x02001C94 RID: 7316
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveDiseaseEmitterMessage
	{
		// Token: 0x04008456 RID: 33878
		public int handle;

		// Token: 0x04008457 RID: 33879
		public int callbackIdx;
	}

	// Token: 0x02001C95 RID: 7317
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetSavedOptionsMessage
	{
		// Token: 0x04008458 RID: 33880
		public byte clearBits;

		// Token: 0x04008459 RID: 33881
		public byte setBits;
	}

	// Token: 0x02001C96 RID: 7318
	public enum SimSavedOptions : byte
	{
		// Token: 0x0400845B RID: 33883
		ENABLE_DIAGONAL_FALLING_SAND = 1
	}

	// Token: 0x02001C97 RID: 7319
	public struct WorldOffsetData
	{
		// Token: 0x0400845C RID: 33884
		public int worldOffsetX;

		// Token: 0x0400845D RID: 33885
		public int worldOffsetY;

		// Token: 0x0400845E RID: 33886
		public int worldSizeX;

		// Token: 0x0400845F RID: 33887
		public int worldSizeY;
	}

	// Token: 0x02001C98 RID: 7320
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct DigMessage
	{
		// Token: 0x04008460 RID: 33888
		public int cellIdx;

		// Token: 0x04008461 RID: 33889
		public int callbackIdx;

		// Token: 0x04008462 RID: 33890
		public bool skipEvent;
	}

	// Token: 0x02001C99 RID: 7321
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetCellFloatValueMessage
	{
		// Token: 0x04008463 RID: 33891
		public int cellIdx;

		// Token: 0x04008464 RID: 33892
		public float value;
	}

	// Token: 0x02001C9A RID: 7322
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CellPropertiesMessage
	{
		// Token: 0x04008465 RID: 33893
		public int cellIdx;

		// Token: 0x04008466 RID: 33894
		public byte properties;

		// Token: 0x04008467 RID: 33895
		public byte set;

		// Token: 0x04008468 RID: 33896
		public byte pad0;

		// Token: 0x04008469 RID: 33897
		public byte pad1;
	}

	// Token: 0x02001C9B RID: 7323
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetInsulationValueMessage
	{
		// Token: 0x0400846A RID: 33898
		public int cellIdx;

		// Token: 0x0400846B RID: 33899
		public float value;
	}

	// Token: 0x02001C9C RID: 7324
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyCellMessage
	{
		// Token: 0x0400846C RID: 33900
		public int cellIdx;

		// Token: 0x0400846D RID: 33901
		public int callbackIdx;

		// Token: 0x0400846E RID: 33902
		public float temperature;

		// Token: 0x0400846F RID: 33903
		public float mass;

		// Token: 0x04008470 RID: 33904
		public int diseaseCount;

		// Token: 0x04008471 RID: 33905
		public ushort elementIdx;

		// Token: 0x04008472 RID: 33906
		public byte replaceType;

		// Token: 0x04008473 RID: 33907
		public byte diseaseIdx;

		// Token: 0x04008474 RID: 33908
		public byte addSubType;
	}

	// Token: 0x02001C9D RID: 7325
	public enum ReplaceType
	{
		// Token: 0x04008476 RID: 33910
		None,
		// Token: 0x04008477 RID: 33911
		Replace,
		// Token: 0x04008478 RID: 33912
		ReplaceAndDisplace
	}

	// Token: 0x02001C9E RID: 7326
	private enum AddSolidMassSubType
	{
		// Token: 0x0400847A RID: 33914
		DoVerticalDisplacement,
		// Token: 0x0400847B RID: 33915
		OnlyIfSameElement
	}

	// Token: 0x02001C9F RID: 7327
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CellDiseaseModification
	{
		// Token: 0x0400847C RID: 33916
		public int cellIdx;

		// Token: 0x0400847D RID: 33917
		public byte diseaseIdx;

		// Token: 0x0400847E RID: 33918
		public byte pad0;

		// Token: 0x0400847F RID: 33919
		public byte pad1;

		// Token: 0x04008480 RID: 33920
		public byte pad2;

		// Token: 0x04008481 RID: 33921
		public int diseaseCount;
	}

	// Token: 0x02001CA0 RID: 7328
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RadiationParamsModification
	{
		// Token: 0x04008482 RID: 33922
		public int RadiationParamsType;

		// Token: 0x04008483 RID: 33923
		public float value;
	}

	// Token: 0x02001CA1 RID: 7329
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CellRadiationModification
	{
		// Token: 0x04008484 RID: 33924
		public int cellIdx;

		// Token: 0x04008485 RID: 33925
		public float radiationDelta;

		// Token: 0x04008486 RID: 33926
		public int callbackIdx;
	}

	// Token: 0x02001CA2 RID: 7330
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct MassConsumptionMessage
	{
		// Token: 0x04008487 RID: 33927
		public int cellIdx;

		// Token: 0x04008488 RID: 33928
		public int callbackIdx;

		// Token: 0x04008489 RID: 33929
		public float mass;

		// Token: 0x0400848A RID: 33930
		public ushort elementIdx;

		// Token: 0x0400848B RID: 33931
		public byte radius;
	}

	// Token: 0x02001CA3 RID: 7331
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct MassEmissionMessage
	{
		// Token: 0x0400848C RID: 33932
		public int cellIdx;

		// Token: 0x0400848D RID: 33933
		public int callbackIdx;

		// Token: 0x0400848E RID: 33934
		public float mass;

		// Token: 0x0400848F RID: 33935
		public float temperature;

		// Token: 0x04008490 RID: 33936
		public int diseaseCount;

		// Token: 0x04008491 RID: 33937
		public ushort elementIdx;

		// Token: 0x04008492 RID: 33938
		public byte diseaseIdx;
	}

	// Token: 0x02001CA4 RID: 7332
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ConsumeDiseaseMessage
	{
		// Token: 0x04008493 RID: 33939
		public int gameCell;

		// Token: 0x04008494 RID: 33940
		public int callbackIdx;

		// Token: 0x04008495 RID: 33941
		public float percentToConsume;

		// Token: 0x04008496 RID: 33942
		public int maxToConsume;
	}

	// Token: 0x02001CA5 RID: 7333
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyCellEnergyMessage
	{
		// Token: 0x04008497 RID: 33943
		public int cellIdx;

		// Token: 0x04008498 RID: 33944
		public float kilojoules;

		// Token: 0x04008499 RID: 33945
		public float maxTemperature;

		// Token: 0x0400849A RID: 33946
		public int id;
	}

	// Token: 0x02001CA6 RID: 7334
	public enum EnergySourceID
	{
		// Token: 0x0400849C RID: 33948
		DebugHeat = 1000,
		// Token: 0x0400849D RID: 33949
		DebugCool,
		// Token: 0x0400849E RID: 33950
		FierySkin,
		// Token: 0x0400849F RID: 33951
		Overheatable,
		// Token: 0x040084A0 RID: 33952
		LiquidCooledFan,
		// Token: 0x040084A1 RID: 33953
		ConduitTemperatureManager,
		// Token: 0x040084A2 RID: 33954
		Excavator,
		// Token: 0x040084A3 RID: 33955
		HeatBulb,
		// Token: 0x040084A4 RID: 33956
		WarmBlooded,
		// Token: 0x040084A5 RID: 33957
		StructureTemperature,
		// Token: 0x040084A6 RID: 33958
		Burner,
		// Token: 0x040084A7 RID: 33959
		VacuumRadiator
	}

	// Token: 0x02001CA7 RID: 7335
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct VisibleCells
	{
		// Token: 0x040084A8 RID: 33960
		public Vector2I min;

		// Token: 0x040084A9 RID: 33961
		public Vector2I max;
	}

	// Token: 0x02001CA8 RID: 7336
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct WakeCellMessage
	{
		// Token: 0x040084AA RID: 33962
		public int gameCell;
	}

	// Token: 0x02001CA9 RID: 7337
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ElementInteraction
	{
		// Token: 0x040084AB RID: 33963
		public uint interactionType;

		// Token: 0x040084AC RID: 33964
		public ushort elemIdx1;

		// Token: 0x040084AD RID: 33965
		public ushort elemIdx2;

		// Token: 0x040084AE RID: 33966
		public ushort elemResultIdx;

		// Token: 0x040084AF RID: 33967
		public byte pad0;

		// Token: 0x040084B0 RID: 33968
		public byte pad1;

		// Token: 0x040084B1 RID: 33969
		public float minMass;

		// Token: 0x040084B2 RID: 33970
		public float interactionProbability;

		// Token: 0x040084B3 RID: 33971
		public float elem1MassDestructionPercent;

		// Token: 0x040084B4 RID: 33972
		public float elem2MassRequiredMultiplier;

		// Token: 0x040084B5 RID: 33973
		public float elemResultMassCreationMultiplier;
	}

	// Token: 0x02001CAA RID: 7338
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CreateElementInteractionsMsg
	{
		// Token: 0x040084B6 RID: 33974
		public int numInteractions;

		// Token: 0x040084B7 RID: 33975
		public unsafe SimMessages.ElementInteraction* interactions;
	}

	// Token: 0x02001CAB RID: 7339
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PipeChange
	{
		// Token: 0x040084B8 RID: 33976
		public int cell;

		// Token: 0x040084B9 RID: 33977
		public byte layer;

		// Token: 0x040084BA RID: 33978
		public byte pad0;

		// Token: 0x040084BB RID: 33979
		public byte pad1;

		// Token: 0x040084BC RID: 33980
		public byte pad2;

		// Token: 0x040084BD RID: 33981
		public float mass;

		// Token: 0x040084BE RID: 33982
		public float temperature;

		// Token: 0x040084BF RID: 33983
		public int elementHash;
	}

	// Token: 0x02001CAC RID: 7340
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CellWorldZoneModification
	{
		// Token: 0x040084C0 RID: 33984
		public int cell;

		// Token: 0x040084C1 RID: 33985
		public byte zoneID;
	}
}
