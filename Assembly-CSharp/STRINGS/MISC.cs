using System;

namespace STRINGS
{
	// Token: 0x02000F1D RID: 3869
	public class MISC
	{
		// Token: 0x02002288 RID: 8840
		public class TAGS
		{
			// Token: 0x04009B4D RID: 39757
			public static LocString OTHER = "Miscellaneous";

			// Token: 0x04009B4E RID: 39758
			public static LocString FILTER = UI.FormatAsLink("Filtration Medium", "FILTER");

			// Token: 0x04009B4F RID: 39759
			public static LocString FILTER_DESC = string.Concat(new string[]
			{
				"Filtration Mediums are materials which are supplied to some filtration buildings that are used in separating purified ",
				UI.FormatAsLink("gases", "ELEMENTS_GASSES"),
				" or ",
				UI.FormatAsLink("liquids", "ELEMENTS_LIQUID"),
				" from their polluted forms.\n\nExamples include filtering ",
				UI.FormatAsLink("Water", "WATER"),
				" from ",
				UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
				" using a ",
				UI.FormatAsLink("Water Sieve", "WATERPURIFIER"),
				", or a ",
				UI.FormatAsLink("Deodorizer", "AIRFILTER"),
				" purifying ",
				UI.FormatAsLink("Oxygen", "OXYGEN"),
				" from ",
				UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
				".\n\nFiltration Mediums are a consumable that will be transformed by the filtering process to generate a by-product, like when ",
				UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
				" is the result after ",
				UI.FormatAsLink("Sand", "SAND"),
				" has been used to filter polluted water. The filtering building will cease to function once the filtering material has been consumed. Once the Filtering Material has been resupplied to the filtering building it will start working again."
			});

			// Token: 0x04009B50 RID: 39760
			public static LocString ICEORE = UI.FormatAsLink("Ice", "ICEORE");

			// Token: 0x04009B51 RID: 39761
			public static LocString ICEORE_DESC = string.Concat(new string[]
			{
				"Ice is a class of materials made up mostly (if not completely) of ",
				UI.FormatAsLink("Water", "WATER"),
				" in a frozen or partially frozen form.\n\nAs a material in a frigid solid or semi-solid state, these elements are very useful as a low-cost way to cool the environment around them.\n\nWhen heated, ice will melt into its original liquified form (ie.",
				UI.FormatAsLink("Brine Ice", "BRINEICE"),
				" will liquify into ",
				UI.FormatAsLink("Brine", "BRINE"),
				"). Each ice element has a different freezing and melting point based upon its composition and state."
			});

			// Token: 0x04009B52 RID: 39762
			public static LocString PHOSPHORUS = UI.FormatAsLink("Phosphorus", "PHOSPHORUS");

			// Token: 0x04009B53 RID: 39763
			public static LocString BUILDABLERAW = UI.FormatAsLink("Raw Mineral", "BUILDABLERAW");

			// Token: 0x04009B54 RID: 39764
			public static LocString BUILDABLERAW_DESC = string.Concat(new string[]
			{
				"Raw minerals are the unrefined forms of organic solids. Almost all raw minerals can be processed in the ",
				UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER"),
				", although a handful require the use of the ",
				UI.FormatAsLink("Molecular Forge", "SUPERMATERIALREFINERY"),
				"."
			});

			// Token: 0x04009B55 RID: 39765
			public static LocString BUILDABLEPROCESSED = UI.FormatAsLink("Refined Mineral", "BUILDABLEPROCESSED");

			// Token: 0x04009B56 RID: 39766
			public static LocString BUILDABLEANY = UI.FormatAsLink("General Buildable", "BUILDABLEANY");

			// Token: 0x04009B57 RID: 39767
			public static LocString BUILDABLEANY_DESC = "";

			// Token: 0x04009B58 RID: 39768
			public static LocString DEHYDRATED = "Dehydrated";

			// Token: 0x04009B59 RID: 39769
			public static LocString PLASTIFIABLELIQUID = UI.FormatAsLink("Plastic Monomer", "PLASTIFIABLELIQUID");

			// Token: 0x04009B5A RID: 39770
			public static LocString PLASTIFIABLELIQUID_DESC = string.Concat(new string[]
			{
				"Plastic monomers are organic compounds that can be processed into ",
				UI.FormatAsLink("Plastics", "PLASTIC"),
				" that have valuable applications as advanced building materials.\n\nPlastics derived from these monomers can also be used as packaging materials for ",
				UI.FormatAsLink("Food", "FOOD"),
				" preservation."
			});

			// Token: 0x04009B5B RID: 39771
			public static LocString UNREFINEDOIL = UI.FormatAsLink("Unrefined Oil", "RAWOIL");

			// Token: 0x04009B5C RID: 39772
			public static LocString UNREFINEDOIL_DESC = "Oils in their raw, minimally processed forms. They can be refined at the " + UI.FormatAsLink("Oil Refinery", "OILREFINERY") + ".";

			// Token: 0x04009B5D RID: 39773
			public static LocString REFINEDMETAL = UI.FormatAsLink("Refined Metal", "REFINEDMETAL");

			// Token: 0x04009B5E RID: 39774
			public static LocString REFINEDMETAL_DESC = string.Concat(new string[]
			{
				"Refined metals are purified forms of metal often used in higher-tier electronics due to their tendency to be able to withstand higher temperatures when they are made into wires. Other benefits include the increased decor value for some metals which can greatly improve the well-being of a colony.\n\nMetal ore can be refined in either the ",
				UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER"),
				" or the ",
				UI.FormatAsLink("Metal Refinery", "METALREFINERY"),
				"."
			});

			// Token: 0x04009B5F RID: 39775
			public static LocString METAL = UI.FormatAsLink("Metal Ore", "METAL");

			// Token: 0x04009B60 RID: 39776
			public static LocString METAL_DESC = string.Concat(new string[]
			{
				"Metal ore is the raw form of metal, and has a wide variety of practical applications in electronics and general construction.\n\nMetal ore is typically processed into ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" using the ",
				UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER"),
				" or the ",
				UI.FormatAsLink("Metal Refinery", "METALREFINERY"),
				".\n\nSome rare metal ores can also be refined in the ",
				UI.FormatAsLink("Molecular Forge", "SUPERMATERIALREFINERY"),
				"."
			});

			// Token: 0x04009B61 RID: 39777
			public static LocString PRECIOUSMETAL = UI.FormatAsLink("Precious Metal", "PRECIOUSMETAL");

			// Token: 0x04009B62 RID: 39778
			public static LocString RAWPRECIOUSMETAL = "Precious Metal Ore";

			// Token: 0x04009B63 RID: 39779
			public static LocString PRECIOUSROCK = UI.FormatAsLink("Precious Rock", "PRECIOUSROCK");

			// Token: 0x04009B64 RID: 39780
			public static LocString PRECIOUSROCK_DESC = "Precious rocks are raw minerals. Their extreme hardness produces durable " + UI.FormatAsLink("Decor", "DECOR") + ".\n\nSome precious rocks are inherently attractive even in their natural, unfinished form.";

			// Token: 0x04009B65 RID: 39781
			public static LocString ALLOY = UI.FormatAsLink("Alloy", "ALLOY");

			// Token: 0x04009B66 RID: 39782
			public static LocString BUILDINGFIBER = UI.FormatAsLink("Fiber", "BUILDINGFIBER");

			// Token: 0x04009B67 RID: 39783
			public static LocString BUILDINGFIBER_DESC = "Fibers are organically sourced polymers which are both sturdy and sensorially pleasant, making them suitable in the construction of " + UI.FormatAsLink("Morale", "MORALE") + "-boosting buildings.";

			// Token: 0x04009B68 RID: 39784
			public static LocString BUILDINGWOOD = UI.FormatAsLink("Wood", "BUILDINGWOOD");

			// Token: 0x04009B69 RID: 39785
			public static LocString BUILDINGWOOD_DESC = string.Concat(new string[]
			{
				"Wood is a renewable building material which can also be used as a valuable source of fuel and electricity when refined at the ",
				UI.FormatAsLink("Wood Burner", "WOODGASGENERATOR"),
				" or the ",
				UI.FormatAsLink("Ethanol Distiller", "ETHANOLDISTILLERY"),
				"."
			});

			// Token: 0x04009B6A RID: 39786
			public static LocString CRUSHABLE = "Crushable";

			// Token: 0x04009B6B RID: 39787
			public static LocString CROPSEEDS = "Crop Seeds";

			// Token: 0x04009B6C RID: 39788
			public static LocString CERAMIC = UI.FormatAsLink("Ceramic", "CERAMIC");

			// Token: 0x04009B6D RID: 39789
			public static LocString POLYPROPYLENE = UI.FormatAsLink("Plastic", "POLYPROPYLENE");

			// Token: 0x04009B6E RID: 39790
			public static LocString BAGABLECREATURE = UI.FormatAsLink("Critter", "CREATURES");

			// Token: 0x04009B6F RID: 39791
			public static LocString SWIMMINGCREATURE = "Aquatic Critter";

			// Token: 0x04009B70 RID: 39792
			public static LocString LIFE = "Life";

			// Token: 0x04009B71 RID: 39793
			public static LocString LIQUIFIABLE = "Liquefiable";

			// Token: 0x04009B72 RID: 39794
			public static LocString LIQUID = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID");

			// Token: 0x04009B73 RID: 39795
			public static LocString LUBRICATINGOIL = "Gear Oil";

			// Token: 0x04009B74 RID: 39796
			public static LocString LUBRICATINGOIL_DESC = "Gear oils are lubricating fluids useful in the maintenance of complex machinery, protecting gear systems from damage and minimizing friction between moving parts to support optimal performance.";

			// Token: 0x04009B75 RID: 39797
			public static LocString SLIPPERY = "Slippery";

			// Token: 0x04009B76 RID: 39798
			public static LocString LEAD = UI.FormatAsLink("Lead", "LEAD");

			// Token: 0x04009B77 RID: 39799
			public static LocString CHARGEDPORTABLEBATTERY = UI.FormatAsLink("Power Banks", "ELECTROBANK");

			// Token: 0x04009B78 RID: 39800
			public static LocString EMPTYPORTABLEBATTERY = UI.FormatAsLink("Empty Eco Power Banks", "ELECTROBANK_EMPTY");

			// Token: 0x04009B79 RID: 39801
			public static LocString SPECIAL = "Special";

			// Token: 0x04009B7A RID: 39802
			public static LocString FARMABLE = UI.FormatAsLink("Cultivable Soil", "FARMABLE");

			// Token: 0x04009B7B RID: 39803
			public static LocString FARMABLE_DESC = "Cultivable soil is a fundamental building block of basic agricultural systems and can also be useful in the production of clean " + UI.FormatAsLink("Oxygen", "OXYGEN") + ".";

			// Token: 0x04009B7C RID: 39804
			public static LocString AGRICULTURE = UI.FormatAsLink("Agriculture", "AGRICULTURE");

			// Token: 0x04009B7D RID: 39805
			public static LocString COAL = "Coal";

			// Token: 0x04009B7E RID: 39806
			public static LocString BLEACHSTONE = "Bleach Stone";

			// Token: 0x04009B7F RID: 39807
			public static LocString ORGANICS = "Organic";

			// Token: 0x04009B80 RID: 39808
			public static LocString CONSUMABLEORE = "Consumable Ore";

			// Token: 0x04009B81 RID: 39809
			public static LocString SUBLIMATING = "Sublimators";

			// Token: 0x04009B82 RID: 39810
			public static LocString ORE = "Ore";

			// Token: 0x04009B83 RID: 39811
			public static LocString BREATHABLE = "Breathable Gas";

			// Token: 0x04009B84 RID: 39812
			public static LocString UNBREATHABLE = "Unbreathable Gas";

			// Token: 0x04009B85 RID: 39813
			public static LocString GAS = "Gas";

			// Token: 0x04009B86 RID: 39814
			public static LocString BURNS = "Flammable";

			// Token: 0x04009B87 RID: 39815
			public static LocString UNSTABLE = "Unstable";

			// Token: 0x04009B88 RID: 39816
			public static LocString TOXIC = "Toxic";

			// Token: 0x04009B89 RID: 39817
			public static LocString MIXTURE = "Mixture";

			// Token: 0x04009B8A RID: 39818
			public static LocString SOLID = UI.FormatAsLink("Solid", "ELEMENTS_SOLID");

			// Token: 0x04009B8B RID: 39819
			public static LocString FLYINGCRITTEREDIBLE = "Bait";

			// Token: 0x04009B8C RID: 39820
			public static LocString INDUSTRIALPRODUCT = "Industrial Product";

			// Token: 0x04009B8D RID: 39821
			public static LocString INDUSTRIALINGREDIENT = UI.FormatAsLink("Industrial Ingredient", "INDUSTRIALINGREDIENT");

			// Token: 0x04009B8E RID: 39822
			public static LocString MEDICALSUPPLIES = "Medical Supplies";

			// Token: 0x04009B8F RID: 39823
			public static LocString CLOTHES = UI.FormatAsLink("Clothing", "EQUIPMENT");

			// Token: 0x04009B90 RID: 39824
			public static LocString EMITSLIGHT = UI.FormatAsLink("Light Emitter", "LIGHT");

			// Token: 0x04009B91 RID: 39825
			public static LocString BED = "Beds";

			// Token: 0x04009B92 RID: 39826
			public static LocString MESSSTATION = "Dining Table";

			// Token: 0x04009B93 RID: 39827
			public static LocString TOY = "Toy";

			// Token: 0x04009B94 RID: 39828
			public static LocString SUIT = "Suits";

			// Token: 0x04009B95 RID: 39829
			public static LocString MULTITOOL = "Multitool";

			// Token: 0x04009B96 RID: 39830
			public static LocString CLINIC = "Clinic";

			// Token: 0x04009B97 RID: 39831
			public static LocString RELAXATION_POINT = "Leisure Area";

			// Token: 0x04009B98 RID: 39832
			public static LocString SOLIDMATERIAL = "Solid Material";

			// Token: 0x04009B99 RID: 39833
			public static LocString EXTRUDABLE = "Extrudable";

			// Token: 0x04009B9A RID: 39834
			public static LocString PLUMBABLE = UI.FormatAsLink("Plumbable", "PLUMBABLE");

			// Token: 0x04009B9B RID: 39835
			public static LocString PLUMBABLE_DESC = "";

			// Token: 0x04009B9C RID: 39836
			public static LocString COMPOSTABLE = UI.FormatAsLink("Compostable", "COMPOSTABLE");

			// Token: 0x04009B9D RID: 39837
			public static LocString COMPOSTABLE_DESC = string.Concat(new string[]
			{
				"Compostables are biological materials which can be put into a ",
				UI.FormatAsLink("Compost", "COMPOST"),
				" to generate clean ",
				UI.FormatAsLink("Dirt", "DIRT"),
				".\n\nComposting also generates a small amount of ",
				UI.FormatAsLink("Heat", "HEAT"),
				".\n\nOnce it starts to rot, consumable food should be composted to prevent ",
				UI.FormatAsLink("Food Poisoning", "FOODSICKNESS"),
				"."
			});

			// Token: 0x04009B9E RID: 39838
			public static LocString COMPOSTBASICPLANTFOOD = "Compost Muckroot";

			// Token: 0x04009B9F RID: 39839
			public static LocString EDIBLE = "Edible";

			// Token: 0x04009BA0 RID: 39840
			public static LocString OXIDIZER = "Oxidizer";

			// Token: 0x04009BA1 RID: 39841
			public static LocString COOKINGINGREDIENT = "Cooking Ingredient";

			// Token: 0x04009BA2 RID: 39842
			public static LocString MEDICINE = "Medicine";

			// Token: 0x04009BA3 RID: 39843
			public static LocString SEED = "Seed";

			// Token: 0x04009BA4 RID: 39844
			public static LocString ANYWATER = "Water Based";

			// Token: 0x04009BA5 RID: 39845
			public static LocString MARKEDFORCOMPOST = "Marked For Compost";

			// Token: 0x04009BA6 RID: 39846
			public static LocString MARKEDFORCOMPOSTINSTORAGE = "In Compost Storage";

			// Token: 0x04009BA7 RID: 39847
			public static LocString COMPOSTMEAT = "Compost Meat";

			// Token: 0x04009BA8 RID: 39848
			public static LocString PICKLED = "Pickled";

			// Token: 0x04009BA9 RID: 39849
			public static LocString PLASTIC = UI.FormatAsLink("Plastics", "PLASTIC");

			// Token: 0x04009BAA RID: 39850
			public static LocString PLASTIC_DESC = string.Concat(new string[]
			{
				"Plastics are synthetic ",
				UI.FormatAsLink("Solids", "ELEMENTSSOLID"),
				" that are pliable and minimize the transfer of ",
				UI.FormatAsLink("Heat", "Heat"),
				". They typically have a low melting point, although more advanced plastics have been developed to circumvent this issue."
			});

			// Token: 0x04009BAB RID: 39851
			public static LocString TOILET = "Toilets";

			// Token: 0x04009BAC RID: 39852
			public static LocString MASSAGE_TABLE = "Massage Tables";

			// Token: 0x04009BAD RID: 39853
			public static LocString POWERSTATION = "Power Station";

			// Token: 0x04009BAE RID: 39854
			public static LocString FARMSTATION = "Farm Station";

			// Token: 0x04009BAF RID: 39855
			public static LocString MACHINE_SHOP = "Machine Shop";

			// Token: 0x04009BB0 RID: 39856
			public static LocString ANTISEPTIC = "Antiseptic";

			// Token: 0x04009BB1 RID: 39857
			public static LocString OIL = "Hydrocarbon";

			// Token: 0x04009BB2 RID: 39858
			public static LocString DECORATION = "Decoration";

			// Token: 0x04009BB3 RID: 39859
			public static LocString EGG = "Critter Egg";

			// Token: 0x04009BB4 RID: 39860
			public static LocString EGGSHELL = "Egg Shell";

			// Token: 0x04009BB5 RID: 39861
			public static LocString MANUFACTUREDMATERIAL = "Manufactured Material";

			// Token: 0x04009BB6 RID: 39862
			public static LocString STEEL = "Steel";

			// Token: 0x04009BB7 RID: 39863
			public static LocString RAW = "Raw Animal Product";

			// Token: 0x04009BB8 RID: 39864
			public static LocString FOSSIL = "Fossil";

			// Token: 0x04009BB9 RID: 39865
			public static LocString ICE = "Ice";

			// Token: 0x04009BBA RID: 39866
			public static LocString ANY = "Any";

			// Token: 0x04009BBB RID: 39867
			public static LocString TRANSPARENT = "Transparent";

			// Token: 0x04009BBC RID: 39868
			public static LocString TRANSPARENT_DESC = string.Concat(new string[]
			{
				"Transparent materials allow ",
				UI.FormatAsLink("Light", "LIGHT"),
				" to pass through. Illumination boosts Duplicant productivity during working hours, but undermines sleep quality.\n\nTransparency is also important for buildings that require a clear line of sight in order to function correctly, such as the ",
				UI.FormatAsLink("Space Scanner", "COMETDETECTOR"),
				"."
			});

			// Token: 0x04009BBD RID: 39869
			public static LocString RAREMATERIALS = "Rare Resource";

			// Token: 0x04009BBE RID: 39870
			public static LocString FARMINGMATERIAL = "Fertilizer";

			// Token: 0x04009BBF RID: 39871
			public static LocString INSULATOR = UI.FormatAsLink("Insulator", "INSULATOR");

			// Token: 0x04009BC0 RID: 39872
			public static LocString INSULATOR_DESC = "Insulators have low thermal conductivity, and effectively reduce the speed at which " + UI.FormatAsLink("Heat", "Heat") + " is transferred through them.";

			// Token: 0x04009BC1 RID: 39873
			public static LocString RAILGUNPAYLOADEMPTYABLE = "Payload";

			// Token: 0x04009BC2 RID: 39874
			public static LocString NONCRUSHABLE = "Uncrushable";

			// Token: 0x04009BC3 RID: 39875
			public static LocString STORYTRAITRESOURCE = "Story Trait";

			// Token: 0x04009BC4 RID: 39876
			public static LocString GLASS = "Glass";

			// Token: 0x04009BC5 RID: 39877
			public static LocString OBSIDIAN = UI.FormatAsLink("Obsidian", "OBSIDIAN");

			// Token: 0x04009BC6 RID: 39878
			public static LocString DIAMOND = UI.FormatAsLink("Diamond", "DIAMOND");

			// Token: 0x04009BC7 RID: 39879
			public static LocString SNOW = UI.FormatAsLink("Snow", "STABLESNOW");

			// Token: 0x04009BC8 RID: 39880
			public static LocString WOODLOG = UI.FormatAsLink("Wood", "WOODLOG");

			// Token: 0x04009BC9 RID: 39881
			public static LocString COMMAND_MODULE = "Command Module";

			// Token: 0x04009BCA RID: 39882
			public static LocString HABITAT_MODULE = "Habitat Module";

			// Token: 0x04009BCB RID: 39883
			public static LocString COMBUSTIBLEGAS = UI.FormatAsLink("Combustible Gas", "COMBUSTIBLEGAS");

			// Token: 0x04009BCC RID: 39884
			public static LocString COMBUSTIBLEGAS_DESC = string.Concat(new string[]
			{
				"Combustible Gases can be burned as fuel to be used in the production of ",
				UI.FormatAsLink("Power", "POWER"),
				" and ",
				UI.FormatAsLink("Food", "FOOD"),
				"."
			});

			// Token: 0x04009BCD RID: 39885
			public static LocString COMBUSTIBLELIQUID = UI.FormatAsLink("Combustible Liquid", "COMBUSTIBLELIQUID");

			// Token: 0x04009BCE RID: 39886
			public static LocString COMBUSTIBLELIQUID_DESC = string.Concat(new string[]
			{
				"Combustible Liquids can be burned as fuels to be used in energy production, such as in a ",
				UI.FormatAsLink("Petroleum Generator", "PETROLEUMGENERATOR"),
				" or a ",
				UI.FormatAsLink("Petroleum Engine", "KEROSENEENGINE"),
				".\n\nThough these liquids have other uses, such as fertilizer for growing a ",
				UI.FormatAsLink("Nosh Bean", "BEANPLANTSEED"),
				", their primary usefulness lies in their ability to be burned for ",
				UI.FormatAsLink("Power", "POWER"),
				"."
			});

			// Token: 0x04009BCF RID: 39887
			public static LocString COMBUSTIBLESOLID = UI.FormatAsLink("Combustible Solid", "COMBUSTIBLESOLID");

			// Token: 0x04009BD0 RID: 39888
			public static LocString COMBUSTIBLESOLID_DESC = "Combustible Solids can be burned as fuel to be used in " + UI.FormatAsLink("Power", "POWER") + " production.";

			// Token: 0x04009BD1 RID: 39889
			public static LocString UNIDENTIFIEDSEED = "Seed (Unidentified Mutation)";

			// Token: 0x04009BD2 RID: 39890
			public static LocString CHARMEDARTIFACT = "Artifact of Interest";

			// Token: 0x04009BD3 RID: 39891
			public static LocString GENE_SHUFFLER = "Neural Vacillator";

			// Token: 0x04009BD4 RID: 39892
			public static LocString WARP_PORTAL = "Teleportal";

			// Token: 0x04009BD5 RID: 39893
			public static LocString BIONIC_UPGRADE = "Boosters";

			// Token: 0x04009BD6 RID: 39894
			public static LocString FARMING = "Farm Build-Delivery";

			// Token: 0x04009BD7 RID: 39895
			public static LocString RESEARCH = "Research Delivery";

			// Token: 0x04009BD8 RID: 39896
			public static LocString POWER = "Generator Delivery";

			// Token: 0x04009BD9 RID: 39897
			public static LocString BUILDING = "Build Dig-Delivery";

			// Token: 0x04009BDA RID: 39898
			public static LocString COOKING = "Cook Delivery";

			// Token: 0x04009BDB RID: 39899
			public static LocString FABRICATING = "Fabricate Delivery";

			// Token: 0x04009BDC RID: 39900
			public static LocString WIRING = "Wire Build-Delivery";

			// Token: 0x04009BDD RID: 39901
			public static LocString ART = "Art Build-Delivery";

			// Token: 0x04009BDE RID: 39902
			public static LocString DOCTORING = "Treatment Delivery";

			// Token: 0x04009BDF RID: 39903
			public static LocString CONVEYOR = "Shipping Build";

			// Token: 0x04009BE0 RID: 39904
			public static LocString COMPOST_FORMAT = "{Item}";

			// Token: 0x04009BE1 RID: 39905
			public static LocString ADVANCEDDOCTORSTATIONMEDICALSUPPLIES = "Serum Vial";

			// Token: 0x04009BE2 RID: 39906
			public static LocString DOCTORSTATIONMEDICALSUPPLIES = "Medical Pack";
		}

		// Token: 0x02002289 RID: 8841
		public class STATUSITEMS
		{
			// Token: 0x02003457 RID: 13399
			public class ATTENTIONREQUIRED
			{
				// Token: 0x0400D4BB RID: 54459
				public static LocString NAME = "Attention Required!";

				// Token: 0x0400D4BC RID: 54460
				public static LocString TOOLTIP = "Something in my colony needs to be attended to";
			}

			// Token: 0x02003458 RID: 13400
			public class SUBLIMATIONBLOCKED
			{
				// Token: 0x0400D4BD RID: 54461
				public static LocString NAME = "{SubElement} emission blocked";

				// Token: 0x0400D4BE RID: 54462
				public static LocString TOOLTIP = "This {Element} deposit is not exposed to air and cannot emit {SubElement}";
			}

			// Token: 0x02003459 RID: 13401
			public class SUBLIMATIONOVERPRESSURE
			{
				// Token: 0x0400D4BF RID: 54463
				public static LocString NAME = "Inert";

				// Token: 0x0400D4C0 RID: 54464
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Environmental ",
					UI.PRE_KEYWORD,
					"Gas Pressure",
					UI.PST_KEYWORD,
					" is too high for this {Element} deposit to emit {SubElement}"
				});
			}

			// Token: 0x0200345A RID: 13402
			public class SUBLIMATIONEMITTING
			{
				// Token: 0x0400D4C1 RID: 54465
				public static LocString NAME = BUILDING.STATUSITEMS.EMITTINGGASAVG.NAME;

				// Token: 0x0400D4C2 RID: 54466
				public static LocString TOOLTIP = BUILDING.STATUSITEMS.EMITTINGGASAVG.TOOLTIP;
			}

			// Token: 0x0200345B RID: 13403
			public class SPACE
			{
				// Token: 0x0400D4C3 RID: 54467
				public static LocString NAME = "Space exposure";

				// Token: 0x0400D4C4 RID: 54468
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This region is exposed to the vacuum of space and will result in the loss of ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" resources"
				});
			}

			// Token: 0x0200345C RID: 13404
			public class EDIBLE
			{
				// Token: 0x0400D4C5 RID: 54469
				public static LocString NAME = "Rations: {0}";

				// Token: 0x0400D4C6 RID: 54470
				public static LocString TOOLTIP = "Can provide " + UI.FormatAsLink("{0}", "KCAL") + " of energy to Duplicants";
			}

			// Token: 0x0200345D RID: 13405
			public class REHYDRATEDFOOD
			{
				// Token: 0x0400D4C7 RID: 54471
				public static LocString NAME = "Rehydrated Food";

				// Token: 0x0400D4C8 RID: 54472
				public static LocString TOOLTIP = string.Format(string.Concat(new string[]
				{
					"This food has been carefully re-moistened for consumption\n\n",
					UI.PRE_KEYWORD,
					"{1}",
					UI.PST_KEYWORD,
					": {0}"
				}), -1f, UI.FormatAsLink(DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.NAME, DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.NAME));
			}

			// Token: 0x0200345E RID: 13406
			public class MARKEDFORDISINFECTION
			{
				// Token: 0x0400D4C9 RID: 54473
				public static LocString NAME = "Disinfect Errand";

				// Token: 0x0400D4CA RID: 54474
				public static LocString TOOLTIP = "Building will be disinfected once a Duplicant is available";
			}

			// Token: 0x0200345F RID: 13407
			public class PENDINGCLEAR
			{
				// Token: 0x0400D4CB RID: 54475
				public static LocString NAME = "Sweep Errand";

				// Token: 0x0400D4CC RID: 54476
				public static LocString TOOLTIP = "Debris will be swept once a Duplicant is available";
			}

			// Token: 0x02003460 RID: 13408
			public class PENDINGCLEARNOSTORAGE
			{
				// Token: 0x0400D4CD RID: 54477
				public static LocString NAME = "Storage Unavailable";

				// Token: 0x0400D4CE RID: 54478
				public static LocString TOOLTIP = "No available " + BUILDINGS.PREFABS.STORAGELOCKER.NAME + " can accept this item\n\nMake sure the filter on your storage is correctly set and there is sufficient space remaining";
			}

			// Token: 0x02003461 RID: 13409
			public class MARKEDFORCOMPOST
			{
				// Token: 0x0400D4CF RID: 54479
				public static LocString NAME = "Compost Errand";

				// Token: 0x0400D4D0 RID: 54480
				public static LocString TOOLTIP = "Object is marked and will be moved to " + BUILDINGS.PREFABS.COMPOST.NAME + " once a Duplicant is available";
			}

			// Token: 0x02003462 RID: 13410
			public class NOCLEARLOCATIONSAVAILABLE
			{
				// Token: 0x0400D4D1 RID: 54481
				public static LocString NAME = "No Sweep Destination";

				// Token: 0x0400D4D2 RID: 54482
				public static LocString TOOLTIP = "There are no valid destinations for this object to be swept to";
			}

			// Token: 0x02003463 RID: 13411
			public class PENDINGHARVEST
			{
				// Token: 0x0400D4D3 RID: 54483
				public static LocString NAME = "Harvest Errand";

				// Token: 0x0400D4D4 RID: 54484
				public static LocString TOOLTIP = "Plant will be harvested once a Duplicant is available";
			}

			// Token: 0x02003464 RID: 13412
			public class PENDINGUPROOT
			{
				// Token: 0x0400D4D5 RID: 54485
				public static LocString NAME = "Uproot Errand";

				// Token: 0x0400D4D6 RID: 54486
				public static LocString TOOLTIP = "Plant will be uprooted once a Duplicant is available";
			}

			// Token: 0x02003465 RID: 13413
			public class WAITINGFORDIG
			{
				// Token: 0x0400D4D7 RID: 54487
				public static LocString NAME = "Dig Errand";

				// Token: 0x0400D4D8 RID: 54488
				public static LocString TOOLTIP = "Tile will be dug out once a Duplicant is available";
			}

			// Token: 0x02003466 RID: 13414
			public class WAITINGFORMOP
			{
				// Token: 0x0400D4D9 RID: 54489
				public static LocString NAME = "Mop Errand";

				// Token: 0x0400D4DA RID: 54490
				public static LocString TOOLTIP = "Spill will be mopped once a Duplicant is available";
			}

			// Token: 0x02003467 RID: 13415
			public class NOTMARKEDFORHARVEST
			{
				// Token: 0x0400D4DB RID: 54491
				public static LocString NAME = "No Harvest Pending";

				// Token: 0x0400D4DC RID: 54492
				public static LocString TOOLTIP = "Use the " + UI.FormatAsTool("Harvest Tool", global::Action.Harvest) + " to mark this plant for harvest";
			}

			// Token: 0x02003468 RID: 13416
			public class GROWINGBRANCHES
			{
				// Token: 0x0400D4DD RID: 54493
				public static LocString NAME = "Growing Branches";

				// Token: 0x0400D4DE RID: 54494
				public static LocString TOOLTIP = "This tree is working hard to grow new branches right now";
			}

			// Token: 0x02003469 RID: 13417
			public class CLUSTERMETEORREMAININGTRAVELTIME
			{
				// Token: 0x0400D4DF RID: 54495
				public static LocString NAME = "Time to collision: {time}";

				// Token: 0x0400D4E0 RID: 54496
				public static LocString TOOLTIP = "The time remaining before this meteor reaches its destination";
			}

			// Token: 0x0200346A RID: 13418
			public class ELEMENTALCATEGORY
			{
				// Token: 0x0400D4E1 RID: 54497
				public static LocString NAME = "{Category}";

				// Token: 0x0400D4E2 RID: 54498
				public static LocString TOOLTIP = "The selected object belongs to the <b>{Category}</b> resource category";
			}

			// Token: 0x0200346B RID: 13419
			public class ELEMENTALMASS
			{
				// Token: 0x0400D4E3 RID: 54499
				public static LocString NAME = "{Mass}";

				// Token: 0x0400D4E4 RID: 54500
				public static LocString TOOLTIP = "The selected object has a mass of <b>{Mass}</b>";
			}

			// Token: 0x0200346C RID: 13420
			public class ELEMENTALDISEASE
			{
				// Token: 0x0400D4E5 RID: 54501
				public static LocString NAME = "{Disease}";

				// Token: 0x0400D4E6 RID: 54502
				public static LocString TOOLTIP = "Current disease: {Disease}";
			}

			// Token: 0x0200346D RID: 13421
			public class ELEMENTALTEMPERATURE
			{
				// Token: 0x0400D4E7 RID: 54503
				public static LocString NAME = "{Temp}";

				// Token: 0x0400D4E8 RID: 54504
				public static LocString TOOLTIP = "The selected object is currently <b>{Temp}</b>";
			}

			// Token: 0x0200346E RID: 13422
			public class MARKEDFORCOMPOSTINSTORAGE
			{
				// Token: 0x0400D4E9 RID: 54505
				public static LocString NAME = "Composted";

				// Token: 0x0400D4EA RID: 54506
				public static LocString TOOLTIP = "The selected object is currently in the compost";
			}

			// Token: 0x0200346F RID: 13423
			public class BURIEDITEM
			{
				// Token: 0x0400D4EB RID: 54507
				public static LocString NAME = "Buried Object";

				// Token: 0x0400D4EC RID: 54508
				public static LocString TOOLTIP = "Something seems to be hidden here";

				// Token: 0x0400D4ED RID: 54509
				public static LocString NOTIFICATION = "Buried object discovered";

				// Token: 0x0400D4EE RID: 54510
				public static LocString NOTIFICATION_TOOLTIP = "My Duplicants have uncovered a {Uncoverable}!\n\n" + UI.CLICK(UI.ClickType.Click) + " to jump to its location.";
			}

			// Token: 0x02003470 RID: 13424
			public class GENETICANALYSISCOMPLETED
			{
				// Token: 0x0400D4EF RID: 54511
				public static LocString NAME = "Genome Sequenced";

				// Token: 0x0400D4F0 RID: 54512
				public static LocString TOOLTIP = "This Station has sequenced a new seed mutation";
			}

			// Token: 0x02003471 RID: 13425
			public class HEALTHSTATUS
			{
				// Token: 0x02003844 RID: 14404
				public class PERFECT
				{
					// Token: 0x0400DE9D RID: 56989
					public static LocString NAME = "None";

					// Token: 0x0400DE9E RID: 56990
					public static LocString TOOLTIP = "This Duplicant is in peak condition";
				}

				// Token: 0x02003845 RID: 14405
				public class ALRIGHT
				{
					// Token: 0x0400DE9F RID: 56991
					public static LocString NAME = "None";

					// Token: 0x0400DEA0 RID: 56992
					public static LocString TOOLTIP = "This Duplicant is none the worse for wear";
				}

				// Token: 0x02003846 RID: 14406
				public class SCUFFED
				{
					// Token: 0x0400DEA1 RID: 56993
					public static LocString NAME = "Minor";

					// Token: 0x0400DEA2 RID: 56994
					public static LocString TOOLTIP = "This Duplicant has a few scrapes and bruises";
				}

				// Token: 0x02003847 RID: 14407
				public class INJURED
				{
					// Token: 0x0400DEA3 RID: 56995
					public static LocString NAME = "Moderate";

					// Token: 0x0400DEA4 RID: 56996
					public static LocString TOOLTIP = "This Duplicant needs some patching up";
				}

				// Token: 0x02003848 RID: 14408
				public class CRITICAL
				{
					// Token: 0x0400DEA5 RID: 56997
					public static LocString NAME = "Severe";

					// Token: 0x0400DEA6 RID: 56998
					public static LocString TOOLTIP = "This Duplicant is in serious need of medical attention";
				}

				// Token: 0x02003849 RID: 14409
				public class INCAPACITATED
				{
					// Token: 0x0400DEA7 RID: 56999
					public static LocString NAME = "Paralyzing";

					// Token: 0x0400DEA8 RID: 57000
					public static LocString TOOLTIP = "This Duplicant will die if they do not receive medical attention";
				}

				// Token: 0x0200384A RID: 14410
				public class DEAD
				{
					// Token: 0x0400DEA9 RID: 57001
					public static LocString NAME = "Conclusive";

					// Token: 0x0400DEAA RID: 57002
					public static LocString TOOLTIP = "This Duplicant won't be getting back up";
				}
			}

			// Token: 0x02003472 RID: 13426
			public class HIT
			{
				// Token: 0x0400D4F1 RID: 54513
				public static LocString NAME = "{targetName} took {damageAmount} damage from {attackerName}'s attack!";
			}

			// Token: 0x02003473 RID: 13427
			public class OREMASS
			{
				// Token: 0x0400D4F2 RID: 54514
				public static LocString NAME = MISC.STATUSITEMS.ELEMENTALMASS.NAME;

				// Token: 0x0400D4F3 RID: 54515
				public static LocString TOOLTIP = MISC.STATUSITEMS.ELEMENTALMASS.TOOLTIP;
			}

			// Token: 0x02003474 RID: 13428
			public class ORETEMP
			{
				// Token: 0x0400D4F4 RID: 54516
				public static LocString NAME = MISC.STATUSITEMS.ELEMENTALTEMPERATURE.NAME;

				// Token: 0x0400D4F5 RID: 54517
				public static LocString TOOLTIP = MISC.STATUSITEMS.ELEMENTALTEMPERATURE.TOOLTIP;
			}

			// Token: 0x02003475 RID: 13429
			public class TREEFILTERABLETAGS
			{
				// Token: 0x0400D4F6 RID: 54518
				public static LocString NAME = "{Tags}";

				// Token: 0x0400D4F7 RID: 54519
				public static LocString TOOLTIP = "{Tags}";
			}

			// Token: 0x02003476 RID: 13430
			public class SPOUTOVERPRESSURE
			{
				// Token: 0x0400D4F8 RID: 54520
				public static LocString NAME = "Overpressure {StudiedDetails}";

				// Token: 0x0400D4F9 RID: 54521
				public static LocString TOOLTIP = "Spout cannot vent due to high environmental pressure";

				// Token: 0x0400D4FA RID: 54522
				public static LocString STUDIED = "(idle in <b>{Time}</b>)";
			}

			// Token: 0x02003477 RID: 13431
			public class SPOUTEMITTING
			{
				// Token: 0x0400D4FB RID: 54523
				public static LocString NAME = "Venting {StudiedDetails}";

				// Token: 0x0400D4FC RID: 54524
				public static LocString TOOLTIP = "This geyser is erupting";

				// Token: 0x0400D4FD RID: 54525
				public static LocString STUDIED = "(idle in <b>{Time}</b>)";
			}

			// Token: 0x02003478 RID: 13432
			public class SPOUTPRESSUREBUILDING
			{
				// Token: 0x0400D4FE RID: 54526
				public static LocString NAME = "Rising pressure {StudiedDetails}";

				// Token: 0x0400D4FF RID: 54527
				public static LocString TOOLTIP = "This geyser's internal pressure is steadily building";

				// Token: 0x0400D500 RID: 54528
				public static LocString STUDIED = "(erupts in <b>{Time}</b>)";
			}

			// Token: 0x02003479 RID: 13433
			public class SPOUTIDLE
			{
				// Token: 0x0400D501 RID: 54529
				public static LocString NAME = "Idle {StudiedDetails}";

				// Token: 0x0400D502 RID: 54530
				public static LocString TOOLTIP = "This geyser is not currently erupting";

				// Token: 0x0400D503 RID: 54531
				public static LocString STUDIED = "(erupts in <b>{Time}</b>)";
			}

			// Token: 0x0200347A RID: 13434
			public class SPOUTDORMANT
			{
				// Token: 0x0400D504 RID: 54532
				public static LocString NAME = "Dormant";

				// Token: 0x0400D505 RID: 54533
				public static LocString TOOLTIP = "This geyser's geoactivity has halted\n\nIt won't erupt again for some time";
			}

			// Token: 0x0200347B RID: 13435
			public class SPICEDFOOD
			{
				// Token: 0x0400D506 RID: 54534
				public static LocString NAME = "Seasoned";

				// Token: 0x0400D507 RID: 54535
				public static LocString TOOLTIP = "This food has been improved with spice from the " + BUILDINGS.PREFABS.SPICEGRINDER.NAME;
			}

			// Token: 0x0200347C RID: 13436
			public class PICKUPABLEUNREACHABLE
			{
				// Token: 0x0400D508 RID: 54536
				public static LocString NAME = "Unreachable";

				// Token: 0x0400D509 RID: 54537
				public static LocString TOOLTIP = "Duplicants cannot reach this object";
			}

			// Token: 0x0200347D RID: 13437
			public class PRIORITIZED
			{
				// Token: 0x0400D50A RID: 54538
				public static LocString NAME = "High Priority";

				// Token: 0x0400D50B RID: 54539
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Errand",
					UI.PST_KEYWORD,
					" has been marked as important and will be preferred over other pending ",
					UI.PRE_KEYWORD,
					"Errands",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200347E RID: 13438
			public class USING
			{
				// Token: 0x0400D50C RID: 54540
				public static LocString NAME = "Using {Target}";

				// Token: 0x0400D50D RID: 54541
				public static LocString TOOLTIP = "{Target} is currently in use";
			}

			// Token: 0x0200347F RID: 13439
			public class ORDERATTACK
			{
				// Token: 0x0400D50E RID: 54542
				public static LocString NAME = "Pending Attack";

				// Token: 0x0400D50F RID: 54543
				public static LocString TOOLTIP = "Waiting for a Duplicant to murderize this defenseless " + UI.PRE_KEYWORD + "Critter" + UI.PST_KEYWORD;
			}

			// Token: 0x02003480 RID: 13440
			public class ORDERCAPTURE
			{
				// Token: 0x0400D510 RID: 54544
				public static LocString NAME = "Pending Wrangle";

				// Token: 0x0400D511 RID: 54545
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Waiting for a Duplicant to capture this ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					"\n\nOnly Duplicants with the ",
					DUPLICANTS.ROLES.RANCHER.NAME,
					" skill can catch critters without traps"
				});
			}

			// Token: 0x02003481 RID: 13441
			public class OPERATING
			{
				// Token: 0x0400D512 RID: 54546
				public static LocString NAME = "In Use";

				// Token: 0x0400D513 RID: 54547
				public static LocString TOOLTIP = "This object is currently being used";
			}

			// Token: 0x02003482 RID: 13442
			public class CLEANING
			{
				// Token: 0x0400D514 RID: 54548
				public static LocString NAME = "Cleaning";

				// Token: 0x0400D515 RID: 54549
				public static LocString TOOLTIP = "This building is currently being cleaned";
			}

			// Token: 0x02003483 RID: 13443
			public class REGIONISBLOCKED
			{
				// Token: 0x0400D516 RID: 54550
				public static LocString NAME = "Blocked";

				// Token: 0x0400D517 RID: 54551
				public static LocString TOOLTIP = "Undug material is blocking off an essential tile";
			}

			// Token: 0x02003484 RID: 13444
			public class STUDIED
			{
				// Token: 0x0400D518 RID: 54552
				public static LocString NAME = "Analysis Complete";

				// Token: 0x0400D519 RID: 54553
				public static LocString TOOLTIP = "Information on this Natural Feature has been compiled below.";
			}

			// Token: 0x02003485 RID: 13445
			public class AWAITINGSTUDY
			{
				// Token: 0x0400D51A RID: 54554
				public static LocString NAME = "Analysis Pending";

				// Token: 0x0400D51B RID: 54555
				public static LocString TOOLTIP = "New information on this Natural Feature will be compiled once the field study is complete";
			}

			// Token: 0x02003486 RID: 13446
			public class DURABILITY
			{
				// Token: 0x0400D51C RID: 54556
				public static LocString NAME = "Durability: {durability}";

				// Token: 0x0400D51D RID: 54557
				public static LocString TOOLTIP = "Items lose durability each time they are equipped, and can no longer be put on by a Duplicant once they reach 0% durability\n\nRepair of this item can be done in the appropriate fabrication station";
			}

			// Token: 0x02003487 RID: 13447
			public class BIONICEXPLORERBOOSTER
			{
				// Token: 0x0400D51E RID: 54558
				public static LocString NAME = "Stored Geodata: {0}";

				// Token: 0x0400D51F RID: 54559
				public static LocString TOOLTIP = UI.PRE_KEYWORD + "Dowsing Boosters" + UI.PST_KEYWORD + " retain geodata gathered by Bionic Duplicants\n\nWhen dowsing is complete and this booster is installed in a Bionic Duplicant, a new geyser will be revealed";
			}

			// Token: 0x02003488 RID: 13448
			public class BIONICEXPLORERBOOSTERREADY
			{
				// Token: 0x0400D520 RID: 54560
				public static LocString NAME = "Dowsing Complete";

				// Token: 0x0400D521 RID: 54561
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Dowsing Booster",
					UI.PST_KEYWORD,
					" has sufficient geodata stored to reveal a new geyser\n\nIt must be installed in a Bionic Duplicant in order to function"
				});
			}

			// Token: 0x02003489 RID: 13449
			public class STOREDITEMDURABILITY
			{
				// Token: 0x0400D522 RID: 54562
				public static LocString NAME = "Durability: {durability}";

				// Token: 0x0400D523 RID: 54563
				public static LocString TOOLTIP = "Items lose durability each time they are equipped, and can no longer be put on by a Duplicant once they reach 0% durability\n\nRepair of this item can be done in the appropriate fabrication station";
			}

			// Token: 0x0200348A RID: 13450
			public class ARTIFACTENTOMBED
			{
				// Token: 0x0400D524 RID: 54564
				public static LocString NAME = "Entombed Artifact";

				// Token: 0x0400D525 RID: 54565
				public static LocString TOOLTIP = "This artifact is trapped in an obscuring shell limiting its decor. A skilled artist can remove it at the " + BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.NAME;
			}

			// Token: 0x0200348B RID: 13451
			public class TEAROPEN
			{
				// Token: 0x0400D526 RID: 54566
				public static LocString NAME = "Temporal Tear open";

				// Token: 0x0400D527 RID: 54567
				public static LocString TOOLTIP = "An open passage through spacetime";
			}

			// Token: 0x0200348C RID: 13452
			public class TEARCLOSED
			{
				// Token: 0x0400D528 RID: 54568
				public static LocString NAME = "Temporal Tear closed";

				// Token: 0x0400D529 RID: 54569
				public static LocString TOOLTIP = "Perhaps some technology could open the passage";
			}

			// Token: 0x0200348D RID: 13453
			public class MARKEDFORMOVE
			{
				// Token: 0x0400D52A RID: 54570
				public static LocString NAME = "Pending Move";

				// Token: 0x0400D52B RID: 54571
				public static LocString TOOLTIP = "Waiting for a Duplicant to move this object";
			}

			// Token: 0x0200348E RID: 13454
			public class MOVESTORAGEUNREACHABLE
			{
				// Token: 0x0400D52C RID: 54572
				public static LocString NAME = "Unreachable Move";

				// Token: 0x0400D52D RID: 54573
				public static LocString TOOLTIP = "Duplicants cannot reach this object to move it";
			}

			// Token: 0x0200348F RID: 13455
			public class PENDINGCARVE
			{
				// Token: 0x0400D52E RID: 54574
				public static LocString NAME = "Carve Errand";

				// Token: 0x0400D52F RID: 54575
				public static LocString TOOLTIP = "Rock will be carved once a Duplicant is available";
			}
		}

		// Token: 0x0200228A RID: 8842
		public class POPFX
		{
			// Token: 0x04009BE3 RID: 39907
			public static LocString RESOURCE_EATEN = "Resource Eaten";

			// Token: 0x04009BE4 RID: 39908
			public static LocString RESOURCE_SELECTION_CHANGED = "Changed to {0}";

			// Token: 0x04009BE5 RID: 39909
			public static LocString EXTRA_POWERBANKS_BIONIC = "Extra Power Banks";
		}

		// Token: 0x0200228B RID: 8843
		public class NOTIFICATIONS
		{
			// Token: 0x02003490 RID: 13456
			public class BASICCONTROLS
			{
				// Token: 0x0400D530 RID: 54576
				public static LocString NAME = "Tutorial: Basic Controls";

				// Token: 0x0400D531 RID: 54577
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"• I can use ",
					UI.FormatAsHotKey(global::Action.PanLeft),
					" and ",
					UI.FormatAsHotKey(global::Action.PanRight),
					" to pan my view left and right, and ",
					UI.FormatAsHotKey(global::Action.PanUp),
					" and ",
					UI.FormatAsHotKey(global::Action.PanDown),
					" to pan up and down.\n\n• ",
					UI.FormatAsHotKey(global::Action.ZoomIn),
					" lets me zoom in, and ",
					UI.FormatAsHotKey(global::Action.ZoomOut),
					" zooms out.\n\n• ",
					UI.FormatAsHotKey(global::Action.CameraHome),
					" returns my view to the Printing Pod.\n\n• I can speed or slow my perception of time using the top left corner buttons, or by pressing ",
					UI.FormatAsHotKey(global::Action.SpeedUp),
					" or ",
					UI.FormatAsHotKey(global::Action.SlowDown),
					". Pressing ",
					UI.FormatAsHotKey(global::Action.TogglePause),
					" will pause the flow of time entirely.\n\n• I'll keep records of everything I discover in my personal DATABASE ",
					UI.FormatAsHotKey(global::Action.ManageDatabase),
					" to refer back to if I forget anything important."
				});

				// Token: 0x0400D532 RID: 54578
				public static LocString MESSAGEBODYALT = string.Concat(new string[]
				{
					"• I can use ",
					UI.FormatAsHotKey(global::Action.AnalogCamera),
					" to pan my view.\n\n• ",
					UI.FormatAsHotKey(global::Action.ZoomIn),
					" lets me zoom in, and ",
					UI.FormatAsHotKey(global::Action.ZoomOut),
					" zooms out.\n\n• I can speed or slow my perception of time using the top left corner buttons, or by pressing ",
					UI.FormatAsHotKey(global::Action.CycleSpeed),
					". Pressing ",
					UI.FormatAsHotKey(global::Action.TogglePause),
					" will pause the flow of time entirely.\n\n• I'll keep records of everything I discover in my personal DATABASE ",
					UI.FormatAsHotKey(global::Action.ManageDatabase),
					" to refer back to if I forget anything important."
				});

				// Token: 0x0400D533 RID: 54579
				public static LocString TOOLTIP = "Notes on using my HUD";
			}

			// Token: 0x02003491 RID: 13457
			public class CODEXUNLOCK
			{
				// Token: 0x0400D534 RID: 54580
				public static LocString NAME = "New Log Entry";

				// Token: 0x0400D535 RID: 54581
				public static LocString MESSAGEBODY = "I've added a new log entry to my Database";

				// Token: 0x0400D536 RID: 54582
				public static LocString TOOLTIP = "I've added a new log entry to my Database";
			}

			// Token: 0x02003492 RID: 13458
			public class WELCOMEMESSAGE
			{
				// Token: 0x0400D537 RID: 54583
				public static LocString NAME = "Tutorial: Colony Management";

				// Token: 0x0400D538 RID: 54584
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"I can use the ",
					UI.FormatAsTool("Dig Tool", global::Action.Dig),
					" and the ",
					UI.FormatAsBuildMenuTab("Build Menu"),
					" in the lower left of the screen to begin planning my first construction tasks.\n\nOnce I've placed a few errands my Duplicants will automatically get to work, without me needing to direct them individually."
				});

				// Token: 0x0400D539 RID: 54585
				public static LocString TOOLTIP = "Notes on getting Duplicants to do my bidding";
			}

			// Token: 0x02003493 RID: 13459
			public class STRESSMANAGEMENTMESSAGE
			{
				// Token: 0x0400D53A RID: 54586
				public static LocString NAME = "Tutorial: Stress Management";

				// Token: 0x0400D53B RID: 54587
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"At 100% ",
					UI.FormatAsLink("Stress", "STRESS"),
					", a Duplicant will have a nervous breakdown and be unable to work.\n\nBreakdowns can manifest in different colony-threatening ways, such as the destruction of buildings or the binge eating of food.\n\nI can help my Duplicants manage stressful situations by giving them access to good ",
					UI.FormatAsLink("Food", "FOOD"),
					", fancy ",
					UI.FormatAsLink("Decor", "DECOR"),
					" and comfort items which boost their ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nI can select a Duplicant and mouse over ",
					UI.FormatAsLink("Stress", "STRESS"),
					" or ",
					UI.FormatAsLink("Morale", "MORALE"),
					" in their CONDITION TAB to view current statuses, and hopefully manage things before they become a problem.\n\nRelated ",
					UI.FormatAsLink("Video: Duplicant Morale", "VIDEOS13"),
					" "
				});

				// Token: 0x0400D53C RID: 54588
				public static LocString TOOLTIP = "Notes on keeping Duplicants happy and productive";
			}

			// Token: 0x02003494 RID: 13460
			public class TASKPRIORITIESMESSAGE
			{
				// Token: 0x0400D53D RID: 54589
				public static LocString NAME = "Tutorial: Priority";

				// Token: 0x0400D53E RID: 54590
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Duplicants always perform errands in order of highest to lowest priority. They will harvest ",
					UI.FormatAsLink("Food", "FOOD"),
					" before they build, for example, or always build new structures before they mine materials.\n\nI can open the ",
					UI.FormatAsManagementMenu("Priorities Screen", global::Action.ManagePriorities),
					" to set which Errand Types Duplicants may or may not perform, or to specialize skilled Duplicants for particular Errand Types."
				});

				// Token: 0x0400D53F RID: 54591
				public static LocString TOOLTIP = "Notes on managing Duplicants' errands";
			}

			// Token: 0x02003495 RID: 13461
			public class MOPPINGMESSAGE
			{
				// Token: 0x0400D540 RID: 54592
				public static LocString NAME = "Tutorial: Polluted Water";

				// Token: 0x0400D541 RID: 54593
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" slowly emits ",
					UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
					" which accelerates the spread of ",
					UI.FormatAsLink("Disease", "DISEASE"),
					".\n\nDuplicants will also be ",
					UI.FormatAsLink("Stressed", "STRESS"),
					" by walking through Polluted Water, so I should have my Duplicants clean up spills by ",
					UI.CLICK(UI.ClickType.clicking),
					" and dragging the ",
					UI.FormatAsTool("Mop Tool", global::Action.Mop)
				});

				// Token: 0x0400D542 RID: 54594
				public static LocString TOOLTIP = "Notes on handling polluted materials";
			}

			// Token: 0x02003496 RID: 13462
			public class LOCOMOTIONMESSAGE
			{
				// Token: 0x0400D543 RID: 54595
				public static LocString NAME = "Video: Duplicant Movement";

				// Token: 0x0400D544 RID: 54596
				public static LocString MESSAGEBODY = "Duplicants have limited jumping and climbing abilities. They can only climb two tiles high and cannot fit into spaces shorter than two tiles, or cross gaps wider than one tile. I should keep this in mind while placing errands.\n\nTo check if an errand I've placed is accessible, I can select a Duplicant and " + UI.CLICK(UI.ClickType.click) + " <b>Show Navigation</b> to view all areas within their reach.";

				// Token: 0x0400D545 RID: 54597
				public static LocString TOOLTIP = "Notes on my Duplicants' maneuverability";
			}

			// Token: 0x02003497 RID: 13463
			public class PRIORITIESMESSAGE
			{
				// Token: 0x0400D546 RID: 54598
				public static LocString NAME = "Tutorial: Errand Priorities";

				// Token: 0x0400D547 RID: 54599
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Duplicants will choose where to work based on the priority of the errands that I give them. I can open the ",
					UI.FormatAsManagementMenu("Priorities Screen", global::Action.ManagePriorities),
					" to set their ",
					UI.PRE_KEYWORD,
					"Duplicant Priorities",
					UI.PST_KEYWORD,
					", and the ",
					UI.FormatAsTool("Priority Tool", global::Action.Prioritize),
					" to fine tune ",
					UI.PRE_KEYWORD,
					"Building Priority",
					UI.PST_KEYWORD,
					". Many buildings will also let me change their Priority level when I select them."
				});

				// Token: 0x0400D548 RID: 54600
				public static LocString TOOLTIP = "Notes on my Duplicants' priorities";
			}

			// Token: 0x02003498 RID: 13464
			public class FETCHINGWATERMESSAGE
			{
				// Token: 0x0400D549 RID: 54601
				public static LocString NAME = "Tutorial: Fetching Water";

				// Token: 0x0400D54A RID: 54602
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"By building a ",
					UI.FormatAsLink("Pitcher Pump", "LIQUIDPUMPINGSTATION"),
					" from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5),
					" over a pool of liquid, my Duplicants will be able to bottle it up and manually deliver it wherever it needs to go."
				});

				// Token: 0x0400D54B RID: 54603
				public static LocString TOOLTIP = "Notes on liquid resource gathering";
			}

			// Token: 0x02003499 RID: 13465
			public class SCHEDULEMESSAGE
			{
				// Token: 0x0400D54C RID: 54604
				public static LocString NAME = "Tutorial: Scheduling";

				// Token: 0x0400D54D RID: 54605
				public static LocString MESSAGEBODY = "My Duplicants will only eat, sleep, work, or bathe during the times I allot for such activities.\n\nTo make the best use of their time, I can open the " + UI.FormatAsManagementMenu("Schedule Tab", global::Action.ManageSchedule) + " to adjust the colony's schedule and plan how they should utilize their day.";

				// Token: 0x0400D54E RID: 54606
				public static LocString TOOLTIP = "Notes on scheduling my Duplicants' time";
			}

			// Token: 0x0200349A RID: 13466
			public class THERMALCOMFORT
			{
				// Token: 0x0400D54F RID: 54607
				public static LocString NAME = "Tutorial: Duplicant Temperature";

				// Token: 0x0400D550 RID: 54608
				public static LocString TOOLTIP = "Notes on helping Duplicants keep their cool";

				// Token: 0x0400D551 RID: 54609
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Environments that are extremely ",
					UI.FormatAsLink("Hot", "HEAT"),
					" or ",
					UI.FormatAsLink("Cold", "HEAT"),
					" affect my Duplicants' internal body temperature and cause undue ",
					UI.FormatAsLink("Stress", "STRESS"),
					" or unscheduled naps.\n\nOpening the ",
					UI.FormatAsOverlay("Temperature Overlay", global::Action.Overlay3),
					" and checking the <b>Thermal Tolerance</b> box allows me to view all areas where my Duplicants will feel discomfort and be unable to regulate their internal body temperature.\n\nRelated ",
					UI.FormatAsLink("Video: Insulation", "VIDEOS17")
				});
			}

			// Token: 0x0200349B RID: 13467
			public class TUTORIAL_OVERHEATING
			{
				// Token: 0x0400D552 RID: 54610
				public static LocString NAME = "Tutorial: Building Temperature";

				// Token: 0x0400D553 RID: 54611
				public static LocString TOOLTIP = "Notes on preventing building from breaking";

				// Token: 0x0400D554 RID: 54612
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"When constructing buildings, I should always take note of their ",
					UI.FormatAsLink("Overheat Temperature", "HEAT"),
					" and plan their locations accordingly. Maintaining low ambient temperatures and good ventilation in the colony will also help keep building temperatures down.\n\nThe <b>Relative Temperature</b> slider tool in the ",
					UI.FormatAsOverlay("Temperature Overlay", global::Action.Overlay3),
					" allows me to change adjust the overlay's color-coding in order to highlight specific temperature ranges.\n\nIf I allow buildings to exceed their Overheat Temperature they will begin to take damage, and if left unattended, they will break down and be unusable until repaired."
				});
			}

			// Token: 0x0200349C RID: 13468
			public class LOTS_OF_GERMS
			{
				// Token: 0x0400D555 RID: 54613
				public static LocString NAME = "Tutorial: Germs and Disease";

				// Token: 0x0400D556 RID: 54614
				public static LocString TOOLTIP = "Notes on Duplicant disease risks";

				// Token: 0x0400D557 RID: 54615
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					UI.FormatAsLink("Germs", "DISEASE"),
					" such as ",
					UI.FormatAsLink("Food Poisoning", "FOODSICKNESS"),
					" and ",
					UI.FormatAsLink("Slimelung", "SLIMESICKNESS"),
					" can cause ",
					UI.FormatAsLink("Disease", "DISEASE"),
					" in my Duplicants. I can use the ",
					UI.FormatAsOverlay("Germ Overlay", global::Action.Overlay9),
					" to view all germ concentrations in my colony, and even detect the sources spawning them.\n\nBuilding Wash Basins from the ",
					UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8),
					" near colony toilets will tell my Duplicants they need to wash up.\n\nRelated ",
					UI.FormatAsLink("Video: Plumbing and Ventilation", "VIDEOS18")
				});
			}

			// Token: 0x0200349D RID: 13469
			public class BEING_INFECTED
			{
				// Token: 0x0400D558 RID: 54616
				public static LocString NAME = "Tutorial: Immune Systems";

				// Token: 0x0400D559 RID: 54617
				public static LocString TOOLTIP = "Notes on keeping Duplicants in peak health";

				// Token: 0x0400D55A RID: 54618
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"When Duplicants come into contact with various ",
					UI.FormatAsLink("Germs", "DISEASE"),
					", they'll need to expend points of ",
					UI.FormatAsLink("Immunity", "IMMUNE SYSTEM"),
					" to resist them and remain healthy. If repeated exposes causes their Immunity to drop to 0%, they'll be unable to resist germs and will contract the next disease they encounter.\n\nDoors with Access Permissions can be built from the BASE TAB<color=#F44A47> <b>[1]</b></color> of the ",
					UI.FormatAsLink("Build menu", "misc"),
					" to block Duplicants from entering biohazardous areas while they recover their spent immunity points."
				});
			}

			// Token: 0x0200349E RID: 13470
			public class DISEASE_COOKING
			{
				// Token: 0x0400D55B RID: 54619
				public static LocString NAME = "Tutorial: Food Safety";

				// Token: 0x0400D55C RID: 54620
				public static LocString TOOLTIP = "Notes on managing food contamination";

				// Token: 0x0400D55D RID: 54621
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"The ",
					UI.FormatAsLink("Food", "FOOD"),
					" my Duplicants cook will only ever be as clean as the ingredients used to make it. Storing food in sterile or ",
					UI.FormatAsLink("Refrigerated", "REFRIGERATOR"),
					" environments will keep food free of ",
					UI.FormatAsLink("Germs", "DISEASE"),
					", while carefully placed hygiene stations like ",
					BUILDINGS.PREFABS.WASHBASIN.NAME,
					" or ",
					BUILDINGS.PREFABS.SHOWER.NAME,
					" will prevent the cooks from infecting the food by handling it.\n\nDangerously contaminated food can be sent to compost by ",
					UI.CLICK(UI.ClickType.clicking),
					" the <b>Compost</b> button on the selected item."
				});
			}

			// Token: 0x0200349F RID: 13471
			public class SUITS
			{
				// Token: 0x0400D55E RID: 54622
				public static LocString NAME = "Tutorial: Atmo Suits";

				// Token: 0x0400D55F RID: 54623
				public static LocString TOOLTIP = "Notes on using atmo suits";

				// Token: 0x0400D560 RID: 54624
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					UI.FormatAsLink("Atmo Suits", "ATMO_SUIT"),
					" can be equipped to protect my Duplicants from environmental hazards like extreme ",
					UI.FormatAsLink("Heat", "Heat"),
					", airborne ",
					UI.FormatAsLink("Germs", "DISEASE"),
					", or unbreathable ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					". In order to utilize these suits, I'll need to hook up an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					" to an ",
					UI.FormatAsLink("Atmo Suit Checkpoint", "SUITMARKER"),
					" , then store one of the suits inside.\n\nDuplicants will equip a suit when they walk past the checkpoint in the chosen direction, and will unequip their suit when walking back the opposite way."
				});
			}

			// Token: 0x020034A0 RID: 13472
			public class RADIATION
			{
				// Token: 0x0400D561 RID: 54625
				public static LocString NAME = "Tutorial: Radiation";

				// Token: 0x0400D562 RID: 54626
				public static LocString TOOLTIP = "Notes on managing radiation";

				// Token: 0x0400D563 RID: 54627
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Objects such as ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					" and ",
					UI.FormatAsLink("Beeta Hives", "BEE"),
					" emit a ",
					UI.FormatAsLink("Radioactive", "RADIOACTIVE"),
					" energy that can be toxic to my Duplicants.\n\nI can use the ",
					UI.FormatAsOverlay("Radiation Overlay"),
					" ",
					UI.FormatAsHotKey(global::Action.Overlay15),
					" to check the scope of the Radiation field. Building thick walls around radiation emitters will dampen the field and protect my Duplicants from getting ",
					UI.FormatAsLink("Radiation Sickness", "RADIATIONSICKNESS"),
					" ."
				});
			}

			// Token: 0x020034A1 RID: 13473
			public class SPACETRAVEL
			{
				// Token: 0x0400D564 RID: 54628
				public static LocString NAME = "Tutorial: Space Travel";

				// Token: 0x0400D565 RID: 54629
				public static LocString TOOLTIP = "Notes on traveling in space";

				// Token: 0x0400D566 RID: 54630
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Building a rocket first requires constructing a ",
					UI.FormatAsLink("Rocket Platform", "LAUNCHPAD"),
					" and adding modules from the menu. All components of the Rocket Checklist will need to be complete before being capable of launching.\n\nA ",
					UI.FormatAsLink("Telescope", "CLUSTERTELESCOPE"),
					" needs to be built on the surface of a Planetoid in order to use the ",
					UI.PRE_KEYWORD,
					"Starmap Screen",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ManageStarmap),
					" to see and set course for new destinations."
				});
			}

			// Token: 0x020034A2 RID: 13474
			public class MORALE
			{
				// Token: 0x0400D567 RID: 54631
				public static LocString NAME = "Video: Duplicant Morale";

				// Token: 0x0400D568 RID: 54632
				public static LocString TOOLTIP = "Notes on Duplicant expectations";

				// Token: 0x0400D569 RID: 54633
				public static LocString MESSAGEBODY = "Food, Rooms, Decor, and Recreation all have an effect on Duplicant Morale. Good experiences improve their Morale, while poor experiences lower it. When a Duplicant's Morale is below their Expectations, they will become Stressed.\n\nDuplicants' Expectations will get higher as they are given new Skills, and the colony will have to be improved to keep up their Morale. An overview of Morale and Stress can be viewed on the Vitals screen.\n\nRelated " + UI.FormatAsLink("Tutorial: Stress Management", "MISCELLANEOUSTIPS");
			}

			// Token: 0x020034A3 RID: 13475
			public class POWER
			{
				// Token: 0x0400D56A RID: 54634
				public static LocString NAME = "Video: Power Circuits";

				// Token: 0x0400D56B RID: 54635
				public static LocString TOOLTIP = "Notes on managing electricity";

				// Token: 0x0400D56C RID: 54636
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Generators are considered \"Producers\" of Power, while the various buildings and machines in the colony are considered \"Consumers\". Each Consumer will pull a certain wattage from the power circuit it is connected to, which can be checked at any time by ",
					UI.CLICK(UI.ClickType.clicking),
					" the building and going to the Energy Tab.\n\nI can use the Power Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay2),
					" to quickly check the status of all my circuits. If the Consumers are taking more wattage than the Generators are creating, the Batteries will drain and there will be brownouts.\n\nAdditionally, if the Consumers are pulling more wattage through the Wires than the Wires can handle, they will overload and burn out. To correct both these situations, I will need to reorganize my Consumers onto separate circuits."
				});
			}

			// Token: 0x020034A4 RID: 13476
			public class BIONICBATTERY
			{
				// Token: 0x0400D56D RID: 54637
				public static LocString NAME = "Tutorial: Powering Bionics";

				// Token: 0x0400D56E RID: 54638
				public static LocString TOOLTIP = "Notes on Duplicant power bank needs";

				// Token: 0x0400D56F RID: 54639
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Bionic Duplicants require ",
					UI.FormatAsLink("Power Banks", "ELECTROBANK"),
					" to function. Bionic Duplicants who run out of ",
					UI.FormatAsLink("Power", "POWER"),
					" will become incapacitated and require another Duplicant to reboot them.\n\nBasic power banks can be made at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x020034A5 RID: 13477
			public class GUNKEDTOILET
			{
				// Token: 0x0400D570 RID: 54640
				public static LocString NAME = "Tutorial: Clogged Toilets";

				// Token: 0x0400D571 RID: 54641
				public static LocString TOOLTIP = "Notes on unclogging toilets";

				// Token: 0x0400D572 RID: 54642
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Bionic Duplicants can dump built-up ",
					UI.FormatAsLink("Liquid Gunk", "LIQUIDGUNK"),
					" into ",
					UI.FormatAsLink("Toilets", "BUILDCATEGORYREQUIREMENTCLASSTOILETTYPE"),
					" if no other options are available. This invariably clogs the plumbing, however, and must be removed before facilities can be used by other Duplicants.\n\nBuilding a ",
					UI.FormatAsLink("Gunk Extractor", "GUNKEMPTIER"),
					" from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5),
					" will ensure that Bionic Duplicants can dispose of their waste appropriately."
				});
			}

			// Token: 0x020034A6 RID: 13478
			public class SLIPPERYSURFACE
			{
				// Token: 0x0400D573 RID: 54643
				public static LocString NAME = "Tutorial: Wet Surfaces";

				// Token: 0x0400D574 RID: 54644
				public static LocString TOOLTIP = "Notes on slipping hazards";

				// Token: 0x0400D575 RID: 54645
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"My Duplicants may slip and fall on wet surfaces. I can help them avoid undue ",
					UI.FormatAsLink("Stress", "STRESS"),
					" and potential injury by using the ",
					UI.FormatAsTool("Mop Tool", global::Action.Mop),
					" to clean up spills. Building ",
					UI.FormatAsLink("Toilets", "BUILDCATEGORYREQUIREMENTCLASSTOILETTYPE"),
					" can help minimize the incidence of spills."
				});
			}

			// Token: 0x020034A7 RID: 13479
			public class BIONICOIL
			{
				// Token: 0x0400D576 RID: 54646
				public static LocString NAME = "Tutorial: Oiling Bionics";

				// Token: 0x0400D577 RID: 54647
				public static LocString TOOLTIP = "Notes on keeping Bionics working efficiently";

				// Token: 0x0400D578 RID: 54648
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Bionic Duplicants with insufficient ",
					UI.FormatAsLink("Gear Oil", "LUBRICATINGOIL"),
					" will slow down significantly to avoid grinding their gears.\n\nI can keep them running smoothly by producing ",
					UI.FormatAsLink("Phyto Oil", "PHYTOOIL"),
					" out of ",
					UI.FormatAsLink("Slime", "SLIME"),
					" and building a ",
					UI.FormatAsLink("Lubrication Station", "OILCHANGER"),
					" from the ",
					UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8),
					"."
				});
			}

			// Token: 0x020034A8 RID: 13480
			public class DIGGING
			{
				// Token: 0x0400D579 RID: 54649
				public static LocString NAME = "Video: Digging for Resources";

				// Token: 0x0400D57A RID: 54650
				public static LocString TOOLTIP = "Notes on buried riches";

				// Token: 0x0400D57B RID: 54651
				public static LocString MESSAGEBODY = "Everything a colony needs to get going is found in the ground. Instructing Duplicants to dig out areas means we can find food, mine resources to build infrastructure, and clear space for the colony to grow. I can access the Dig Tool with " + UI.FormatAsHotKey(global::Action.Dig) + ", which allows me to select the area where I want my Duplicants to dig.\n\nDuplicants will need to gain the Superhard Digging skill to mine Abyssalite and the Superduperhard Digging skill to mine Diamond and Obsidian. Without the proper skills, these materials will be undiggable.";
			}

			// Token: 0x020034A9 RID: 13481
			public class INSULATION
			{
				// Token: 0x0400D57C RID: 54652
				public static LocString NAME = "Video: Insulation";

				// Token: 0x0400D57D RID: 54653
				public static LocString TOOLTIP = "Notes on effective temperature management";

				// Token: 0x0400D57E RID: 54654
				public static LocString MESSAGEBODY = "The temperature of an environment can have positive or negative effects on the well-being of my Duplicants, as well as the plants and critters in my colony. Selecting " + UI.FormatAsHotKey(global::Action.Overlay3) + " will open the Temperature Overlay where I can check for any hot or cold spots.\n\nI can use a Utility building like an Ice-E Fan or a Space Heater to make an area colder or warmer. However, I will have limited success changing the temperature of a room unless I build the area with insulating tiles to prevent cold or warm air from escaping.";
			}

			// Token: 0x020034AA RID: 13482
			public class PLUMBING
			{
				// Token: 0x0400D57F RID: 54655
				public static LocString NAME = "Video: Plumbing and Ventilation";

				// Token: 0x0400D580 RID: 54656
				public static LocString TOOLTIP = "Notes on connecting buildings with pipes";

				// Token: 0x0400D581 RID: 54657
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"When connecting pipes for plumbing, it is useful to have the Plumbing Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay6),
					" selected. Each building which requires plumbing must have their Building Intake connected to the Output Pipe from a source such as a Liquid Pump. Liquid Pumps must be submerged in liquid and attached to a power source to function.\n\nBuildings often output contaminated water which must flow out of the building through piping from the Output Pipe. The water can then be expelled through a Liquid Vent, or filtered through a Water Sieve for reuse.\n\nVentilation applies the same principles to gases. Select the Ventilation Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay7),
					" to see how gases are being moved around the colony."
				});
			}

			// Token: 0x020034AB RID: 13483
			public class NEW_AUTOMATION_WARNING
			{
				// Token: 0x0400D582 RID: 54658
				public static LocString NAME = "New Automation Port";

				// Token: 0x0400D583 RID: 54659
				public static LocString TOOLTIP = "This building has a new automation port and is unintentionally connected to an existing " + BUILDINGS.PREFABS.LOGICWIRE.NAME;
			}

			// Token: 0x020034AC RID: 13484
			public class DTU
			{
				// Token: 0x0400D584 RID: 54660
				public static LocString NAME = "Tutorial: Duplicant Thermal Units";

				// Token: 0x0400D585 RID: 54661
				public static LocString TOOLTIP = "Notes on measuring heat energy";

				// Token: 0x0400D586 RID: 54662
				public static LocString MESSAGEBODY = "My Duplicants measure heat energy in Duplicant Thermal Units or DTU.\n\n1 DTU = 1055.06 J";
			}

			// Token: 0x020034AD RID: 13485
			public class NOMESSAGES
			{
				// Token: 0x0400D587 RID: 54663
				public static LocString NAME = "";

				// Token: 0x0400D588 RID: 54664
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020034AE RID: 13486
			public class NOALERTS
			{
				// Token: 0x0400D589 RID: 54665
				public static LocString NAME = "";

				// Token: 0x0400D58A RID: 54666
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020034AF RID: 13487
			public class NEWTRAIT
			{
				// Token: 0x0400D58B RID: 54667
				public static LocString NAME = "{0} has developed a trait";

				// Token: 0x0400D58C RID: 54668
				public static LocString TOOLTIP = "{0} has developed the trait(s):\n    • {1}";
			}

			// Token: 0x020034B0 RID: 13488
			public class RESEARCHCOMPLETE
			{
				// Token: 0x0400D58D RID: 54669
				public static LocString NAME = "Research Complete";

				// Token: 0x0400D58E RID: 54670
				public static LocString MESSAGEBODY = "Eureka! We've discovered {0} Technology.\n\nNew buildings have become available:\n  • {1}";

				// Token: 0x0400D58F RID: 54671
				public static LocString TOOLTIP = "{0} research complete!";
			}

			// Token: 0x020034B1 RID: 13489
			public class WORLDDETECTED
			{
				// Token: 0x0400D590 RID: 54672
				public static LocString NAME = "New " + UI.CLUSTERMAP.PLANETOID + " detected";

				// Token: 0x0400D591 RID: 54673
				public static LocString MESSAGEBODY = "My Duplicants' astronomical efforts have uncovered a new " + UI.CLUSTERMAP.PLANETOID + ":\n{0}";

				// Token: 0x0400D592 RID: 54674
				public static LocString TOOLTIP = "{0} discovered";
			}

			// Token: 0x020034B2 RID: 13490
			public class SKILL_POINT_EARNED
			{
				// Token: 0x0400D593 RID: 54675
				public static LocString NAME = "{Duplicant} earned a skill point!";

				// Token: 0x0400D594 RID: 54676
				public static LocString MESSAGEBODY = "These Duplicants have Skill Points that can be spent on new abilities:\n{0}";

				// Token: 0x0400D595 RID: 54677
				public static LocString LINE = "\n• <b>{0}</b>";

				// Token: 0x0400D596 RID: 54678
				public static LocString TOOLTIP = "{Duplicant} has been working hard and is ready to learn a new skill";
			}

			// Token: 0x020034B3 RID: 13491
			public class DUPLICANTABSORBED
			{
				// Token: 0x0400D597 RID: 54679
				public static LocString NAME = "Printables have been reabsorbed";

				// Token: 0x0400D598 RID: 54680
				public static LocString MESSAGEBODY = "The Printing Pod is no longer available for printing.\nCountdown to the next production has been rebooted.";

				// Token: 0x0400D599 RID: 54681
				public static LocString TOOLTIP = "Printing countdown rebooted";
			}

			// Token: 0x020034B4 RID: 13492
			public class DUPLICANTDIED
			{
				// Token: 0x0400D59A RID: 54682
				public static LocString NAME = "Duplicants have died";

				// Token: 0x0400D59B RID: 54683
				public static LocString TOOLTIP = "These Duplicants have died:";
			}

			// Token: 0x020034B5 RID: 13493
			public class FOODROT
			{
				// Token: 0x0400D59C RID: 54684
				public static LocString NAME = "Food has decayed";

				// Token: 0x0400D59D RID: 54685
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items have rotted and are no longer edible:{0}";
			}

			// Token: 0x020034B6 RID: 13494
			public class FOODSTALE
			{
				// Token: 0x0400D59E RID: 54686
				public static LocString NAME = "Food has become stale";

				// Token: 0x0400D59F RID: 54687
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items have become stale and could rot if not stored:";
			}

			// Token: 0x020034B7 RID: 13495
			public class YELLOWALERT
			{
				// Token: 0x0400D5A0 RID: 54688
				public static LocString NAME = "Yellow Alert";

				// Token: 0x0400D5A1 RID: 54689
				public static LocString TOOLTIP = "The colony has some top priority tasks to complete before resuming a normal schedule";
			}

			// Token: 0x020034B8 RID: 13496
			public class REDALERT
			{
				// Token: 0x0400D5A2 RID: 54690
				public static LocString NAME = "Red Alert";

				// Token: 0x0400D5A3 RID: 54691
				public static LocString TOOLTIP = "The colony is prioritizing work over their individual well-being";
			}

			// Token: 0x020034B9 RID: 13497
			public class REACTORMELTDOWN
			{
				// Token: 0x0400D5A4 RID: 54692
				public static LocString NAME = "Reactor Meltdown";

				// Token: 0x0400D5A5 RID: 54693
				public static LocString TOOLTIP = "A Research Reactor has overheated and is melting down! Extreme radiation is flooding the area";
			}

			// Token: 0x020034BA RID: 13498
			public class HEALING
			{
				// Token: 0x0400D5A6 RID: 54694
				public static LocString NAME = "Healing";

				// Token: 0x0400D5A7 RID: 54695
				public static LocString TOOLTIP = "This Duplicant is recovering from an injury";
			}

			// Token: 0x020034BB RID: 13499
			public class UNREACHABLEITEM
			{
				// Token: 0x0400D5A8 RID: 54696
				public static LocString NAME = "Unreachable resources";

				// Token: 0x0400D5A9 RID: 54697
				public static LocString TOOLTIP = "Duplicants cannot retrieve these resources:";
			}

			// Token: 0x020034BC RID: 13500
			public class INVALIDCONSTRUCTIONLOCATION
			{
				// Token: 0x0400D5AA RID: 54698
				public static LocString NAME = "Invalid construction location";

				// Token: 0x0400D5AB RID: 54699
				public static LocString TOOLTIP = "These buildings cannot be constructed in the planned areas:";
			}

			// Token: 0x020034BD RID: 13501
			public class MISSINGMATERIALS
			{
				// Token: 0x0400D5AC RID: 54700
				public static LocString NAME = "Missing materials";

				// Token: 0x0400D5AD RID: 54701
				public static LocString TOOLTIP = "These resources are not available:";
			}

			// Token: 0x020034BE RID: 13502
			public class BUILDINGOVERHEATED
			{
				// Token: 0x0400D5AE RID: 54702
				public static LocString NAME = "Damage: Overheated";

				// Token: 0x0400D5AF RID: 54703
				public static LocString TOOLTIP = "Extreme heat is damaging these buildings:";
			}

			// Token: 0x020034BF RID: 13503
			public class TILECOLLAPSE
			{
				// Token: 0x0400D5B0 RID: 54704
				public static LocString NAME = "Ceiling Collapse!";

				// Token: 0x0400D5B1 RID: 54705
				public static LocString TOOLTIP = "Falling material fell on these Duplicants and displaced them:";
			}

			// Token: 0x020034C0 RID: 13504
			public class NO_OXYGEN_GENERATOR
			{
				// Token: 0x0400D5B2 RID: 54706
				public static LocString NAME = "No " + UI.FormatAsLink("Oxygen Generator", "OXYGEN") + " built";

				// Token: 0x0400D5B3 RID: 54707
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"My colony is not producing any new ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					"\n\n",
					UI.FormatAsLink("Oxygen Diffusers", "MINERALDEOXIDIZER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Oxygen Tab", global::Action.Plan2)
				});
			}

			// Token: 0x020034C1 RID: 13505
			public class INSUFFICIENTOXYGENLASTCYCLE
			{
				// Token: 0x0400D5B4 RID: 54708
				public static LocString NAME = "Insufficient Oxygen generation";

				// Token: 0x0400D5B5 RID: 54709
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"My colony is consuming more ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" than it is producing, and will run out air if I do not increase production.\n\nI should check my existing oxygen production buildings to ensure they're operating correctly\n\n• ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" produced last cycle: {EmittingRate}\n• Consumed last cycle: {ConsumptionRate}"
				});
			}

			// Token: 0x020034C2 RID: 13506
			public class UNREFRIGERATEDFOOD
			{
				// Token: 0x0400D5B6 RID: 54710
				public static LocString NAME = "Unrefrigerated Food";

				// Token: 0x0400D5B7 RID: 54711
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items are stored but not refrigerated:\n";
			}

			// Token: 0x020034C3 RID: 13507
			public class FOODLOW
			{
				// Token: 0x0400D5B8 RID: 54712
				public static LocString NAME = "Food shortage";

				// Token: 0x0400D5B9 RID: 54713
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The colony's ",
					UI.FormatAsLink("Food", "FOOD"),
					" reserves are low:\n\n    • {0} are currently available\n    • {1} is being consumed per cycle\n\n",
					UI.FormatAsLink("Microbe Mushers", "MICROBEMUSHER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Food Tab", global::Action.Plan4)
				});
			}

			// Token: 0x020034C4 RID: 13508
			public class NO_MEDICAL_COTS
			{
				// Token: 0x0400D5BA RID: 54714
				public static LocString NAME = "No " + UI.FormatAsLink("Sick Bay", "DOCTORSTATION") + " built";

				// Token: 0x0400D5BB RID: 54715
				public static LocString TOOLTIP = "There is nowhere for sick Duplicants receive medical care\n\n" + UI.FormatAsLink("Sick Bays", "DOCTORSTATION") + " can be built from the " + UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8);
			}

			// Token: 0x020034C5 RID: 13509
			public class NEEDTOILET
			{
				// Token: 0x0400D5BC RID: 54716
				public static LocString NAME = "No " + UI.FormatAsLink("Outhouse", "OUTHOUSE") + " built";

				// Token: 0x0400D5BD RID: 54717
				public static LocString TOOLTIP = "My Duplicants have nowhere to relieve themselves\n\n" + UI.FormatAsLink("Outhouses", "OUTHOUSE") + " can be built from the " + UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5);
			}

			// Token: 0x020034C6 RID: 13510
			public class NEEDFOOD
			{
				// Token: 0x0400D5BE RID: 54718
				public static LocString NAME = "Colony requires a food source";

				// Token: 0x0400D5BF RID: 54719
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The colony will exhaust their supplies without a new ",
					UI.FormatAsLink("Food", "FOOD"),
					" source\n\n",
					UI.FormatAsLink("Microbe Mushers", "MICROBEMUSHER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Food Tab", global::Action.Plan4)
				});
			}

			// Token: 0x020034C7 RID: 13511
			public class HYGENE_NEEDED
			{
				// Token: 0x0400D5C0 RID: 54720
				public static LocString NAME = "No " + UI.FormatAsLink("Wash Basin", "WASHBASIN") + " built";

				// Token: 0x0400D5C1 RID: 54721
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					UI.FormatAsLink("Germs", "DISEASE"),
					" are spreading in the colony because my Duplicants have nowhere to clean up\n\n",
					UI.FormatAsLink("Wash Basins", "WASHBASIN"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8)
				});
			}

			// Token: 0x020034C8 RID: 13512
			public class NEEDSLEEP
			{
				// Token: 0x0400D5C2 RID: 54722
				public static LocString NAME = "No " + UI.FormatAsLink("Cots", "BED") + " built";

				// Token: 0x0400D5C3 RID: 54723
				public static LocString TOOLTIP = "My Duplicants would appreciate a place to sleep\n\n" + UI.FormatAsLink("Cots", "BED") + " can be built from the " + UI.FormatAsBuildMenuTab("Furniture Tab", global::Action.Plan9);
			}

			// Token: 0x020034C9 RID: 13513
			public class NEEDENERGYSOURCE
			{
				// Token: 0x0400D5C4 RID: 54724
				public static LocString NAME = "Colony requires a " + UI.FormatAsLink("Power", "POWER") + " source";

				// Token: 0x0400D5C5 RID: 54725
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					UI.FormatAsLink("Power", "POWER"),
					" is required to operate electrical buildings\n\n",
					UI.FormatAsLink("Manual Generators", "MANUALGENERATOR"),
					" and ",
					UI.FormatAsLink("Wire", "WIRE"),
					" can be built from the ",
					UI.FormatAsLink("Power Tab", "[3]")
				});
			}

			// Token: 0x020034CA RID: 13514
			public class RESOURCEMELTED
			{
				// Token: 0x0400D5C6 RID: 54726
				public static LocString NAME = "Resources melted";

				// Token: 0x0400D5C7 RID: 54727
				public static LocString TOOLTIP = "These resources have melted:";
			}

			// Token: 0x020034CB RID: 13515
			public class VENTOVERPRESSURE
			{
				// Token: 0x0400D5C8 RID: 54728
				public static LocString NAME = "Vent overpressurized";

				// Token: 0x0400D5C9 RID: 54729
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" systems have exited the ideal ",
					UI.PRE_KEYWORD,
					"Pressure",
					UI.PST_KEYWORD,
					" range:"
				});
			}

			// Token: 0x020034CC RID: 13516
			public class VENTBLOCKED
			{
				// Token: 0x0400D5CA RID: 54730
				public static LocString NAME = "Vent blocked";

				// Token: 0x0400D5CB RID: 54731
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Blocked ",
					UI.PRE_KEYWORD,
					"Pipes",
					UI.PST_KEYWORD,
					" have stopped these systems from functioning:"
				});
			}

			// Token: 0x020034CD RID: 13517
			public class OUTPUTBLOCKED
			{
				// Token: 0x0400D5CC RID: 54732
				public static LocString NAME = "Output blocked";

				// Token: 0x0400D5CD RID: 54733
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Blocked ",
					UI.PRE_KEYWORD,
					"Pipes",
					UI.PST_KEYWORD,
					" have stopped these systems from functioning:"
				});
			}

			// Token: 0x020034CE RID: 13518
			public class BROKENMACHINE
			{
				// Token: 0x0400D5CE RID: 54734
				public static LocString NAME = "Building broken";

				// Token: 0x0400D5CF RID: 54735
				public static LocString TOOLTIP = "These buildings have taken significant damage and are nonfunctional:";
			}

			// Token: 0x020034CF RID: 13519
			public class STRUCTURALDAMAGE
			{
				// Token: 0x0400D5D0 RID: 54736
				public static LocString NAME = "Structural damage";

				// Token: 0x0400D5D1 RID: 54737
				public static LocString TOOLTIP = "These buildings' structural integrity has been compromised";
			}

			// Token: 0x020034D0 RID: 13520
			public class STRUCTURALCOLLAPSE
			{
				// Token: 0x0400D5D2 RID: 54738
				public static LocString NAME = "Structural collapse";

				// Token: 0x0400D5D3 RID: 54739
				public static LocString TOOLTIP = "These buildings have collapsed:";
			}

			// Token: 0x020034D1 RID: 13521
			public class GASCLOUDWARNING
			{
				// Token: 0x0400D5D4 RID: 54740
				public static LocString NAME = "A gas cloud approaches";

				// Token: 0x0400D5D5 RID: 54741
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A toxic ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" cloud will soon envelop the colony"
				});
			}

			// Token: 0x020034D2 RID: 13522
			public class GASCLOUDARRIVING
			{
				// Token: 0x0400D5D6 RID: 54742
				public static LocString NAME = "The colony is entering a cloud of gas";

				// Token: 0x0400D5D7 RID: 54743
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020034D3 RID: 13523
			public class GASCLOUDPEAK
			{
				// Token: 0x0400D5D8 RID: 54744
				public static LocString NAME = "The gas cloud is at its densest point";

				// Token: 0x0400D5D9 RID: 54745
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020034D4 RID: 13524
			public class GASCLOUDDEPARTING
			{
				// Token: 0x0400D5DA RID: 54746
				public static LocString NAME = "The gas cloud is receding";

				// Token: 0x0400D5DB RID: 54747
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020034D5 RID: 13525
			public class GASCLOUDGONE
			{
				// Token: 0x0400D5DC RID: 54748
				public static LocString NAME = "The colony is once again in open space";

				// Token: 0x0400D5DD RID: 54749
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020034D6 RID: 13526
			public class AVAILABLE
			{
				// Token: 0x0400D5DE RID: 54750
				public static LocString NAME = "Resource available";

				// Token: 0x0400D5DF RID: 54751
				public static LocString TOOLTIP = "These resources have become available:";
			}

			// Token: 0x020034D7 RID: 13527
			public class ALLOCATED
			{
				// Token: 0x0400D5E0 RID: 54752
				public static LocString NAME = "Resource allocated";

				// Token: 0x0400D5E1 RID: 54753
				public static LocString TOOLTIP = "These resources are reserved for a planned building:";
			}

			// Token: 0x020034D8 RID: 13528
			public class INCREASEDEXPECTATIONS
			{
				// Token: 0x0400D5E2 RID: 54754
				public static LocString NAME = "Duplicants' expectations increased";

				// Token: 0x0400D5E3 RID: 54755
				public static LocString TOOLTIP = "Duplicants require better amenities over time.\nThese Duplicants have increased their expectations:";
			}

			// Token: 0x020034D9 RID: 13529
			public class NEARLYDRY
			{
				// Token: 0x0400D5E4 RID: 54756
				public static LocString NAME = "Nearly dry";

				// Token: 0x0400D5E5 RID: 54757
				public static LocString TOOLTIP = "These Duplicants will dry off soon:";
			}

			// Token: 0x020034DA RID: 13530
			public class IMMIGRANTSLEFT
			{
				// Token: 0x0400D5E6 RID: 54758
				public static LocString NAME = "Printables have been reabsorbed";

				// Token: 0x0400D5E7 RID: 54759
				public static LocString TOOLTIP = "The care packages have been disintegrated and printable Duplicants have been Oozed";
			}

			// Token: 0x020034DB RID: 13531
			public class LEVELUP
			{
				// Token: 0x0400D5E8 RID: 54760
				public static LocString NAME = "Attribute increase";

				// Token: 0x0400D5E9 RID: 54761
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants' ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" have improved:"
				});

				// Token: 0x0400D5EA RID: 54762
				public static LocString SUFFIX = " - {0} Skill Level modifier raised to +{1}";
			}

			// Token: 0x020034DC RID: 13532
			public class RESETSKILL
			{
				// Token: 0x0400D5EB RID: 54763
				public static LocString NAME = "Skills reset";

				// Token: 0x0400D5EC RID: 54764
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have had their ",
					UI.PRE_KEYWORD,
					"Skill Points",
					UI.PST_KEYWORD,
					" refunded:"
				});
			}

			// Token: 0x020034DD RID: 13533
			public class BADROCKETPATH
			{
				// Token: 0x0400D5ED RID: 54765
				public static LocString NAME = "Flight Path Obstructed";

				// Token: 0x0400D5EE RID: 54766
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A rocket's flight path has been interrupted by a new astronomical discovery.\nOpen the ",
					UI.PRE_KEYWORD,
					"Starmap Screen",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ManageStarmap),
					" to reassign rocket paths"
				});
			}

			// Token: 0x020034DE RID: 13534
			public class SCHEDULE_CHANGED
			{
				// Token: 0x0400D5EF RID: 54767
				public static LocString NAME = "{0}: {1}!";

				// Token: 0x0400D5F0 RID: 54768
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants assigned to ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" have started their <b>{1}</b> block.\n\n{2}\n\nOpen the ",
					UI.PRE_KEYWORD,
					"Schedule Screen",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ManageSchedule),
					" to change blocks or assignments"
				});
			}

			// Token: 0x020034DF RID: 13535
			public class GENESHUFFLER
			{
				// Token: 0x0400D5F1 RID: 54769
				public static LocString NAME = "Genes Shuffled";

				// Token: 0x0400D5F2 RID: 54770
				public static LocString TOOLTIP = "These Duplicants had their genetic makeup modified:";

				// Token: 0x0400D5F3 RID: 54771
				public static LocString SUFFIX = " has developed " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;
			}

			// Token: 0x020034E0 RID: 13536
			public class HEALINGTRAITGAIN
			{
				// Token: 0x0400D5F4 RID: 54772
				public static LocString NAME = "New trait";

				// Token: 0x0400D5F5 RID: 54773
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants' injuries weren't set and healed improperly.\nThey developed ",
					UI.PRE_KEYWORD,
					"Traits",
					UI.PST_KEYWORD,
					" as a result:"
				});

				// Token: 0x0400D5F6 RID: 54774
				public static LocString SUFFIX = " has developed " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;
			}

			// Token: 0x020034E1 RID: 13537
			public class COLONYLOST
			{
				// Token: 0x0400D5F7 RID: 54775
				public static LocString NAME = "Colony Lost";

				// Token: 0x0400D5F8 RID: 54776
				public static LocString TOOLTIP = "All Duplicants are dead or incapacitated";
			}

			// Token: 0x020034E2 RID: 13538
			public class FABRICATOREMPTY
			{
				// Token: 0x0400D5F9 RID: 54777
				public static LocString NAME = "Fabricator idle";

				// Token: 0x0400D5FA RID: 54778
				public static LocString TOOLTIP = "These fabricators have no recipes queued:";
			}

			// Token: 0x020034E3 RID: 13539
			public class BUILDING_MELTED
			{
				// Token: 0x0400D5FB RID: 54779
				public static LocString NAME = "Building melted";

				// Token: 0x0400D5FC RID: 54780
				public static LocString TOOLTIP = "Extreme heat has melted these buildings:";
			}

			// Token: 0x020034E4 RID: 13540
			public class SUIT_DROPPED
			{
				// Token: 0x0400D5FD RID: 54781
				public static LocString NAME = "No Docks available";

				// Token: 0x0400D5FE RID: 54782
				public static LocString TOOLTIP = "An exosuit was dropped because there were no empty docks available";
			}

			// Token: 0x020034E5 RID: 13541
			public class DEATH_SUFFOCATION
			{
				// Token: 0x0400D5FF RID: 54783
				public static LocString NAME = "Duplicants suffocated";

				// Token: 0x0400D600 RID: 54784
				public static LocString TOOLTIP = "These Duplicants died from a lack of " + ELEMENTS.OXYGEN.NAME + ":";
			}

			// Token: 0x020034E6 RID: 13542
			public class DEATH_FROZENSOLID
			{
				// Token: 0x0400D601 RID: 54785
				public static LocString NAME = "Duplicants have frozen";

				// Token: 0x0400D602 RID: 54786
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from extremely low ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020034E7 RID: 13543
			public class DEATH_OVERHEATING
			{
				// Token: 0x0400D603 RID: 54787
				public static LocString NAME = "Duplicants have overheated";

				// Token: 0x0400D604 RID: 54788
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from extreme ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020034E8 RID: 13544
			public class DEATH_STARVATION
			{
				// Token: 0x0400D605 RID: 54789
				public static LocString NAME = "Duplicants have starved";

				// Token: 0x0400D606 RID: 54790
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from a lack of ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020034E9 RID: 13545
			public class DEATH_FELL
			{
				// Token: 0x0400D607 RID: 54791
				public static LocString NAME = "Duplicants splattered";

				// Token: 0x0400D608 RID: 54792
				public static LocString TOOLTIP = "These Duplicants fell to their deaths:";
			}

			// Token: 0x020034EA RID: 13546
			public class DEATH_CRUSHED
			{
				// Token: 0x0400D609 RID: 54793
				public static LocString NAME = "Duplicants crushed";

				// Token: 0x0400D60A RID: 54794
				public static LocString TOOLTIP = "These Duplicants have been crushed:";
			}

			// Token: 0x020034EB RID: 13547
			public class DEATH_SUFFOCATEDTANKEMPTY
			{
				// Token: 0x0400D60B RID: 54795
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400D60C RID: 54796
				public static LocString TOOLTIP = "These Duplicants were unable to reach " + UI.FormatAsLink("Oxygen", "OXYGEN") + " and died:";
			}

			// Token: 0x020034EC RID: 13548
			public class DEATH_SUFFOCATEDAIRTOOHOT
			{
				// Token: 0x0400D60D RID: 54797
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400D60E RID: 54798
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have asphyxiated in ",
					UI.PRE_KEYWORD,
					"Hot",
					UI.PST_KEYWORD,
					" air:"
				});
			}

			// Token: 0x020034ED RID: 13549
			public class DEATH_SUFFOCATEDAIRTOOCOLD
			{
				// Token: 0x0400D60F RID: 54799
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400D610 RID: 54800
				public static LocString TOOLTIP = "These Duplicants have asphyxiated in " + UI.FormatAsLink("Cold", "HEAT") + " air:";
			}

			// Token: 0x020034EE RID: 13550
			public class DEATH_DROWNED
			{
				// Token: 0x0400D611 RID: 54801
				public static LocString NAME = "Duplicants have drowned";

				// Token: 0x0400D612 RID: 54802
				public static LocString TOOLTIP = "These Duplicants have drowned:";
			}

			// Token: 0x020034EF RID: 13551
			public class DEATH_ENTOUMBED
			{
				// Token: 0x0400D613 RID: 54803
				public static LocString NAME = "Duplicants have been entombed";

				// Token: 0x0400D614 RID: 54804
				public static LocString TOOLTIP = "These Duplicants are trapped and need assistance:";
			}

			// Token: 0x020034F0 RID: 13552
			public class DEATH_RAPIDDECOMPRESSION
			{
				// Token: 0x0400D615 RID: 54805
				public static LocString NAME = "Duplicants pressurized";

				// Token: 0x0400D616 RID: 54806
				public static LocString TOOLTIP = "These Duplicants died in a low pressure environment:";
			}

			// Token: 0x020034F1 RID: 13553
			public class DEATH_OVERPRESSURE
			{
				// Token: 0x0400D617 RID: 54807
				public static LocString NAME = "Duplicants pressurized";

				// Token: 0x0400D618 RID: 54808
				public static LocString TOOLTIP = "These Duplicants died in a high pressure environment:";
			}

			// Token: 0x020034F2 RID: 13554
			public class DEATH_POISONED
			{
				// Token: 0x0400D619 RID: 54809
				public static LocString NAME = "Duplicants poisoned";

				// Token: 0x0400D61A RID: 54810
				public static LocString TOOLTIP = "These Duplicants died as a result of poisoning:";
			}

			// Token: 0x020034F3 RID: 13555
			public class DEATH_DISEASE
			{
				// Token: 0x0400D61B RID: 54811
				public static LocString NAME = "Duplicants have succumbed to disease";

				// Token: 0x0400D61C RID: 54812
				public static LocString TOOLTIP = "These Duplicants died from an untreated " + UI.FormatAsLink("Disease", "DISEASE") + ":";
			}

			// Token: 0x020034F4 RID: 13556
			public class CIRCUIT_OVERLOADED
			{
				// Token: 0x0400D61D RID: 54813
				public static LocString NAME = "Circuit Overloaded";

				// Token: 0x0400D61E RID: 54814
				public static LocString TOOLTIP = "These " + BUILDINGS.PREFABS.WIRE.NAME + "s melted due to excessive current demands on their circuits";
			}

			// Token: 0x020034F5 RID: 13557
			public class LOGIC_CIRCUIT_OVERLOADED
			{
				// Token: 0x0400D61F RID: 54815
				public static LocString NAME = "Logic Circuit Overloaded";

				// Token: 0x0400D620 RID: 54816
				public static LocString TOOLTIP = "These " + BUILDINGS.PREFABS.LOGICWIRE.NAME + "s melted due to more bits of data being sent over them than they can support";
			}

			// Token: 0x020034F6 RID: 13558
			public class DISCOVERED_SPACE
			{
				// Token: 0x0400D621 RID: 54817
				public static LocString NAME = "ALERT - Surface Breach";

				// Token: 0x0400D622 RID: 54818
				public static LocString TOOLTIP = "Amazing!\n\nMy Duplicants have managed to breach the surface of our rocky prison.\n\nI should be careful; the region is extremely inhospitable and I could easily lose resources to the vacuum of space.";
			}

			// Token: 0x020034F7 RID: 13559
			public class COLONY_ACHIEVEMENT_EARNED
			{
				// Token: 0x0400D623 RID: 54819
				public static LocString NAME = "Colony Achievement earned";

				// Token: 0x0400D624 RID: 54820
				public static LocString TOOLTIP = "The colony has earned a new achievement.";
			}

			// Token: 0x020034F8 RID: 13560
			public class WARP_PORTAL_DUPE_READY
			{
				// Token: 0x0400D625 RID: 54821
				public static LocString NAME = "Duplicant warp ready";

				// Token: 0x0400D626 RID: 54822
				public static LocString TOOLTIP = "{dupe} is ready to warp from the " + BUILDINGS.PREFABS.WARPPORTAL.NAME;
			}

			// Token: 0x020034F9 RID: 13561
			public class GENETICANALYSISCOMPLETE
			{
				// Token: 0x0400D627 RID: 54823
				public static LocString NAME = "Seed Analysis Complete";

				// Token: 0x0400D628 RID: 54824
				public static LocString MESSAGEBODY = "Deeply probing the genes of the {Plant} plant have led to the discovery of a promising new cultivatable mutation:\n\n<b>{Subspecies}</b>\n\n{Info}";

				// Token: 0x0400D629 RID: 54825
				public static LocString TOOLTIP = "{Plant} Analysis complete!";
			}

			// Token: 0x020034FA RID: 13562
			public class NEWMUTANTSEED
			{
				// Token: 0x0400D62A RID: 54826
				public static LocString NAME = "New Mutant Seed Discovered";

				// Token: 0x0400D62B RID: 54827
				public static LocString TOOLTIP = "A new mutant variety of the {Plant} has been found. Analyze it at the " + BUILDINGS.PREFABS.GENETICANALYSISSTATION.NAME + " to learn more!";
			}

			// Token: 0x020034FB RID: 13563
			public class DUPLICANT_CRASH_LANDED
			{
				// Token: 0x0400D62C RID: 54828
				public static LocString NAME = "Duplicant Crash Landed!";

				// Token: 0x0400D62D RID: 54829
				public static LocString TOOLTIP = "A Duplicant has successfully crashed an Escape Pod onto the surface of a nearby Planetoid.";
			}

			// Token: 0x020034FC RID: 13564
			public class POIRESEARCHUNLOCKCOMPLETE
			{
				// Token: 0x0400D62E RID: 54830
				public static LocString NAME = "Research Discovered";

				// Token: 0x0400D62F RID: 54831
				public static LocString MESSAGEBODY = "Eureka! We've decrypted the Research Portal's final transmission. New buildings have become available:\n  {0}\n\nOne file was labeled \"Open This First.\" New Database Entry unlocked.";

				// Token: 0x0400D630 RID: 54832
				public static LocString TOOLTIP = "{0} unlocked!";

				// Token: 0x0400D631 RID: 54833
				public static LocString BUTTON_VIEW_LORE = "View entry";
			}

			// Token: 0x020034FD RID: 13565
			public class BIONICRESEARCHUNLOCK
			{
				// Token: 0x0400D632 RID: 54834
				public static LocString NAME = "Research Discovered";

				// Token: 0x0400D633 RID: 54835
				public static LocString MESSAGEBODY = "My new Bionic Duplicant came programmed with {0} technology. How crafty!";
			}

			// Token: 0x020034FE RID: 13566
			public class BIONICLIQUIDDAMAGE
			{
				// Token: 0x0400D634 RID: 54836
				public static LocString NAME = "Liquid Damage";

				// Token: 0x0400D635 RID: 54837
				public static LocString TOOLTIP = "This Duplicant stepped in liquid and damaged their bionic systems!";
			}
		}

		// Token: 0x0200228C RID: 8844
		public class TUTORIAL
		{
			// Token: 0x04009BE6 RID: 39910
			public static LocString DONT_SHOW_AGAIN = "Don't Show Again";
		}

		// Token: 0x0200228D RID: 8845
		public class PLACERS
		{
			// Token: 0x020034FF RID: 13567
			public class DIGPLACER
			{
				// Token: 0x0400D636 RID: 54838
				public static LocString NAME = "Dig";
			}

			// Token: 0x02003500 RID: 13568
			public class MOPPLACER
			{
				// Token: 0x0400D637 RID: 54839
				public static LocString NAME = "Mop";
			}

			// Token: 0x02003501 RID: 13569
			public class MOVEPICKUPABLEPLACER
			{
				// Token: 0x0400D638 RID: 54840
				public static LocString NAME = "Relocate Here";

				// Token: 0x0400D639 RID: 54841
				public static LocString PLACER_STATUS = "Next Destination";

				// Token: 0x0400D63A RID: 54842
				public static LocString PLACER_STATUS_TOOLTIP = "Click to see where this item will be relocated to";
			}
		}

		// Token: 0x0200228E RID: 8846
		public class MONUMENT_COMPLETE
		{
			// Token: 0x04009BE7 RID: 39911
			public static LocString NAME = "Great Monument";

			// Token: 0x04009BE8 RID: 39912
			public static LocString DESC = "A feat of artistic vision and expert engineering that will doubtless inspire Duplicants for thousands of cycles to come";
		}
	}
}
