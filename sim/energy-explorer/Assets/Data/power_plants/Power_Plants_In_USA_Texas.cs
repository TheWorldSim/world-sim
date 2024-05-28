using System;
using System.Collections.Generic;

public static class Power_Plants_In_USA_Texas
{
	public static List<PowerPlantData> power_plants = new List<PowerPlantData>
	{
		new PowerPlantData_ThermalCombustion(
			id: "29186062",
			name: "Mountain Creek Generating Station",
			lon_lat: new LonLat(-96.9364983f, 32.7227155f),
			outline: new List<LonLat>() {
				new LonLat(-96.9380519f, 32.7227155f),
				new LonLat(-96.9356486f, 32.7276762f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 893f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_NuclearFission(
			id: "39899843",
			name: "South Texas Project Electric Generating Station",
			lon_lat: new LonLat(-96.0748201f, 28.7899931f),
			outline: new List<LonLat>() {
				new LonLat(-96.0748201f, 28.7817936f),
				new LonLat(-96.0210043f, 28.808917f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("1988-1-1"),
			power_mw: 2700f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_ThermalCombustion(
			id: "39979635",
			name: "Cedar Bayou Station",
			lon_lat: new LonLat(-94.9251713f, 29.7522156f),
			outline: new List<LonLat>() {
				new LonLat(-94.9278942f, 29.7470786f),
				new LonLat(-94.9195514f, 29.7528537f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 2065.5f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "43706421",
			name: "W. A. Parish Electric Generating Station",
			lon_lat: new LonLat(-95.6396148f, 29.4792634f),
			outline: new List<LonLat>() {
				new LonLat(-95.643184f, 29.46828f),
				new LonLat(-95.6219778f, 29.487194f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 3990f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal(),
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "77712979",
			name: "Brandon Station",
			lon_lat: new LonLat(-101.8867988f, 33.585011f),
			outline: new List<LonLat>() {
				new LonLat(-101.8867988f, 33.5846356f),
				new LonLat(-101.8857053f, 33.585483f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 21f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "94615598",
			name: "Fayette Power Project",
			lon_lat: new LonLat(-96.7503423f, 29.9026583f),
			outline: new List<LonLat>() {
				new LonLat(-96.7677806f, 29.9009667f),
				new LonLat(-96.7367274f, 29.9211112f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 1650f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "100301317",
			name: "Lewis Creek Power Plant",
			lon_lat: new LonLat(-95.5265313f, 30.4431892f),
			outline: new List<LonLat>() {
				new LonLat(-95.5316808f, 30.4318112f),
				new LonLat(-95.5175185f, 30.4431892f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 504f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "100396239",
			name: "Limestone Electric Generating Station",
			lon_lat: new LonLat(-96.2474443f, 31.4287724f),
			outline: new List<LonLat>() {
				new LonLat(-96.2634947f, 31.4113767f),
				new LonLat(-96.2436249f, 31.4287724f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("1985-1-1"),
			power_mw: 1690f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "101027469",
			name: "Carbon II",
			lon_lat: new LonLat(-100.6942598f, 28.4663443f),
			outline: new List<LonLat>() {
				new LonLat(-100.7005756f, 28.4643737f),
				new LonLat(-100.6942598f, 28.4696176f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 1400f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "101027472",
			name: "Carbon I",
			lon_lat: new LonLat(-100.6905818f, 28.4877539f),
			outline: new List<LonLat>() {
				new LonLat(-100.6923399f, 28.4835661f),
				new LonLat(-100.6824338f, 28.4931312f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 1200f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "121051560",
			name: "Leon Creek Power Plant",
			lon_lat: new LonLat(-98.5736584f, 29.3545901f),
			outline: new List<LonLat>() {
				new LonLat(-98.5797748f, 29.3488227f),
				new LonLat(-98.5736584f, 29.3547142f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("1948-1-1"),
			power_mw: 192f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "125729472",
			name: "Pearsall Power Plant",
			lon_lat: new LonLat(-99.0865523f, 28.9286618f),
			outline: new List<LonLat>() {
				new LonLat(-99.0978515f, 28.9204467f),
				new LonLat(-99.0865523f, 28.9339497f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 277f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "126175792",
			name: "San Miguel Power Plant",
			lon_lat: new LonLat(-98.4826255f, 28.6990095f),
			outline: new List<LonLat>() {
				new LonLat(-98.4835681f, 28.6988952f),
				new LonLat(-98.4662044f, 28.7081564f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 391f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "126452423",
			name: "T. H. Wharton Power Plant",
			lon_lat: new LonLat(-95.5371791f, 29.9385194f),
			outline: new List<LonLat>() {
				new LonLat(-95.5384719f, 29.9383994f),
				new LonLat(-95.5243139f, 29.9475175f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 772f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "127240417",
			name: "Oak Grove Power Plant",
			lon_lat: new LonLat(-96.4847727f, 31.1820467f),
			outline: new List<LonLat>() {
				new LonLat(-96.5066314f, 31.1688485f),
				new LonLat(-96.4808513f, 31.1900895f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2010-1-1"),
			power_mw: 1665f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "127241425",
			name: "Twin Oaks Power Station",
			lon_lat: new LonLat(-96.7017452f, 31.0959763f),
			outline: new List<LonLat>() {
				new LonLat(-96.7017452f, 31.0813309f),
				new LonLat(-96.679632f, 31.0999239f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 350f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "127263416",
			name: "Gibbons Creek Steam Electric Station",
			lon_lat: new LonLat(-96.0808478f, 30.6104182f),
			outline: new List<LonLat>() {
				new LonLat(-96.0882926f, 30.6104182f),
				new LonLat(-96.074153f, 30.6256712f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 454f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "127596521",
			name: "Lost Pines Power Park",
			lon_lat: new LonLat(-97.2725362f, 30.1508352f),
			outline: new List<LonLat>() {
				new LonLat(-97.2736236f, 30.1439211f),
				new LonLat(-97.2680542f, 30.1511182f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 1163f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "128372086",
			name: "Rio Nogales Power Project",
			lon_lat: new LonLat(-97.9711672f, 29.5944448f),
			outline: new List<LonLat>() {
				new LonLat(-97.9766431f, 29.5907076f),
				new LonLat(-97.9711672f, 29.5944448f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 848f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "129320296",
			name: "Baytown Energy Center",
			lon_lat: new LonLat(-94.9054354f, 29.773024f),
			outline: new List<LonLat>() {
				new LonLat(-94.9054354f, 29.7709298f),
				new LonLat(-94.9012329f, 29.7732942f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 842f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "165665537",
			name: "Oklaunion Power Plant",
			lon_lat: new LonLat(-99.1985147f, 34.0935849f),
			outline: new List<LonLat>() {
				new LonLat(-99.1985147f, 34.068665f),
				new LonLat(-99.1572878f, 34.097216f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 650f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "174016471",
			name: "Hays Energy Project",
			lon_lat: new LonLat(-97.9944565f, 29.7822796f),
			outline: new List<LonLat>() {
				new LonLat(-97.9945241f, 29.7765832f),
				new LonLat(-97.9866054f, 29.7843209f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 968f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "205129452",
			name: "Tenaska Frontier Generating Station",
			lon_lat: new LonLat(-95.9195868f, 30.5911264f),
			outline: new List<LonLat>() {
				new LonLat(-95.9205953f, 30.5911264f),
				new LonLat(-95.9157657f, 30.5951715f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 939f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "263231710",
			name: "San Jacinto Peaking Power Facility",
			lon_lat: new LonLat(-95.0124157f, 30.4175893f),
			outline: new List<LonLat>() {
				new LonLat(-95.0124157f, 30.4175823f),
				new LonLat(-95.0101894f, 30.4218151f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2009-1-1"),
			power_mw: 150f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_Hydro(
			id: "303473140",
			name: "Falcon Power Plant (United States Unit)",
			lon_lat: new LonLat(-99.1641129f, 26.5577612f),
			outline: new List<LonLat>() {
				new LonLat(-99.1643483f, 26.5570254f),
				new LonLat(-99.1637348f, 26.5577612f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 36f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_Hydro(
			id: "303498754",
			name: "Falcon Power Plant (Mexico Unit)",
			lon_lat: new LonLat(-99.1694449f, 26.5581307f),
			outline: new List<LonLat>() {
				new LonLat(-99.169581f, 26.5581307f),
				new LonLat(-99.1687663f, 26.558685f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 36f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_SolarFarm(
			id: "318453564",
			name: "Alamo 1",
			lon_lat: new LonLat(-98.4563908f, 29.2790822f),
			outline: new List<LonLat>() {
				new LonLat(-98.456427f, 29.2659723f),
				new LonLat(-98.4336296f, 29.2791395f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2013-1-1"),
			power_mw: 41f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_ThermalCombustion(
			id: "337225056",
			name: "Thermal Energy Cooporation Central Plant",
			lon_lat: new LonLat(-95.399026f, 29.7042691f),
			outline: new List<LonLat>() {
				new LonLat(-95.3994685f, 29.7031146f),
				new LonLat(-95.3960966f, 29.7042691f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 48f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_SolarFarm(
			id: "340408023",
			name: "Alamo 2",
			lon_lat: new LonLat(-98.3361062f, 29.4683679f),
			outline: new List<LonLat>() {
				new LonLat(-98.3361062f, 29.4676206f),
				new LonLat(-98.3302304f, 29.4719734f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2014-1-1"),
			power_mw: 4.4f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_SolarFarm(
			id: "340408024",
			name: "Alamo 3",
			lon_lat: new LonLat(-98.3030301f, 29.4823198f),
			outline: new List<LonLat>() {
				new LonLat(-98.3030301f, 29.4769014f),
				new LonLat(-98.296454f, 29.4823851f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2014-1-1"),
			power_mw: 5.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_ThermalCombustion(
			id: "342660063",
			name: "Guadalupe Power Partners Generating Station",
			lon_lat: new LonLat(-98.1452293f, 29.6254273f),
			outline: new List<LonLat>() {
				new LonLat(-98.1452293f, 29.6209354f),
				new LonLat(-98.1374641f, 29.6282084f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 1074f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_SolarFarm(
			id: "402581609",
			name: "Webberville Solar Project",
			lon_lat: new LonLat(-97.5106111f, 30.2457856f),
			outline: new List<LonLat>() {
				new LonLat(-97.5167732f, 30.2304388f),
				new LonLat(-97.5000679f, 30.2468482f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 30f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_ThermalCombustion(
			id: "402680483",
			name: "Quail Run Energy Center",
			lon_lat: new LonLat(-102.3182008f, 31.8432048f),
			outline: new List<LonLat>() {
				new LonLat(-102.3182008f, 31.8402186f),
				new LonLat(-102.3135127f, 31.8447535f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 550f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "405055524",
			name: "Ector County Energy Center",
			lon_lat: new LonLat(-102.5873295f, 32.0712774f),
			outline: new List<LonLat>() {
				new LonLat(-102.5873295f, 32.0694136f),
				new LonLat(-102.5846205f, 32.071791f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 350f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "407980441",
			name: "Aspen Power",
			lon_lat: new LonLat(-94.7448008f, 31.3667317f),
			outline: new List<LonLat>() {
				new LonLat(-94.7460615f, 31.3653789f),
				new LonLat(-94.7383582f, 31.3708632f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2011-1-1"),
			power_mw: 50f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Biomass()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "419366188",
			name: "Sandy Creek Energy Station",
			lon_lat: new LonLat(-96.9629788f, 31.4747372f),
			outline: new List<LonLat>() {
				new LonLat(-96.9629788f, 31.4662568f),
				new LonLat(-96.9344303f, 31.4832626f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 970f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "419984534",
			name: "Odessa Ector Power Partners",
			lon_lat: new LonLat(-102.3278042f, 31.8401562f),
			outline: new List<LonLat>() {
				new LonLat(-102.3314426f, 31.8366403f),
				new LonLat(-102.3197951f, 31.8412704f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 1152f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "420139935",
			name: "Brazos Valley Generating Facility",
			lon_lat: new LonLat(-95.6247181f, 29.4712921f),
			outline: new List<LonLat>() {
				new LonLat(-95.6248412f, 29.4712921f),
				new LonLat(-95.6220059f, 29.4735674f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 609f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "422731518",
			name: "Bosque Energy Center",
			lon_lat: new LonLat(-97.3578301f, 31.8580702f),
			outline: new List<LonLat>() {
				new LonLat(-97.3605308f, 31.8565383f),
				new LonLat(-97.3534858f, 31.8618999f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 762f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "422731519",
			name: "Channel Energy Center",
			lon_lat: new LonLat(-95.2316306f, 29.719221f),
			outline: new List<LonLat>() {
				new LonLat(-95.2337606f, 29.7178061f),
				new LonLat(-95.2316282f, 29.7198417f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 808f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "422731520",
			name: "Deer Park Energy Center",
			lon_lat: new LonLat(-95.1333662f, 29.7121167f),
			outline: new List<LonLat>() {
				new LonLat(-95.1363998f, 29.7117407f),
				new LonLat(-95.1327286f, 29.7155036f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 1204f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "422731521",
			name: "Freestone Energy Center",
			lon_lat: new LonLat(-96.1119349f, 31.8930321f),
			outline: new List<LonLat>() {
				new LonLat(-96.1144789f, 31.8881469f),
				new LonLat(-96.1095513f, 31.8930611f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 1036f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "422731522",
			name: "Magic Valley Generating Station",
			lon_lat: new LonLat(-98.1921637f, 26.3421701f),
			outline: new List<LonLat>() {
				new LonLat(-98.1931467f, 26.3346651f),
				new LonLat(-98.1885575f, 26.3421701f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2002-1-1"),
			power_mw: 712f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "422731523",
			name: "Pasadena Power Plant",
			lon_lat: new LonLat(-95.1776522f, 29.72422f),
			outline: new List<LonLat>() {
				new LonLat(-95.1776727f, 29.724014f),
				new LonLat(-95.1753844f, 29.7267965f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 781f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "422732475",
			name: "Corpus Christi Energy Center",
			lon_lat: new LonLat(-97.4277341f, 27.8134808f),
			outline: new List<LonLat>() {
				new LonLat(-97.4297568f, 27.8132166f),
				new LonLat(-97.4277289f, 27.8162421f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 500f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "422732476",
			name: "Hidalgo Energy Center",
			lon_lat: new LonLat(-98.1742986f, 26.3418693f),
			outline: new List<LonLat>() {
				new LonLat(-98.1774621f, 26.3397577f),
				new LonLat(-98.1720457f, 26.3437105f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2000-1-1"),
			power_mw: 476f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "422852282",
			name: "Sabine Power Plant",
			lon_lat: new LonLat(-93.8725407f, 30.0258321f),
			outline: new List<LonLat>() {
				new LonLat(-93.8821968f, 30.0197452f),
				new LonLat(-93.8716799f, 30.0263139f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 2051f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "449155822",
			name: "DeCordova Power Plant",
			lon_lat: new LonLat(-97.6952235f, 32.4029545f),
			outline: new List<LonLat>() {
				new LonLat(-97.7033566f, 32.401093f),
				new LonLat(-97.6952235f, 32.4071938f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 356f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "449466232",
			name: "Unknown",
			lon_lat: new LonLat(-97.9583197f, 33.1033168f),
			outline: new List<LonLat>() {
				new LonLat(-97.9596085f, 33.0999936f),
				new LonLat(-97.9557769f, 33.1040919f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 1200f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "451971706",
			name: "Laredo Power Plant",
			lon_lat: new LonLat(-99.5053564f, 27.5671292f),
			outline: new List<LonLat>() {
				new LonLat(-99.5110052f, 27.5652674f),
				new LonLat(-99.504922f, 27.5683775f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 188f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_SolarFarm(
			id: "459614011",
			name: "Alamo 5",
			lon_lat: new LonLat(-99.6985137f, 29.2266266f),
			outline: new List<LonLat>() {
				new LonLat(-99.7166079f, 29.206843f),
				new LonLat(-99.6940774f, 29.2266266f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2015-1-1"),
			power_mw: 95f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_ThermalCombustion(
			id: "459877999",
			name: "Permian Basin Power Plant",
			lon_lat: new LonLat(-102.9708892f, 31.5861845f),
			outline: new List<LonLat>() {
				new LonLat(-102.9708892f, 31.5808858f),
				new LonLat(-102.9564743f, 31.5868311f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 325f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_Hydro(
			id: "460356978",
			name: "Amistad Power Plant",
			lon_lat: new LonLat(-101.0552364f, 29.4487482f),
			outline: new List<LonLat>() {
				new LonLat(-101.0563165f, 29.4487388f),
				new LonLat(-101.0552364f, 29.4499972f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 66f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_ThermalCombustion(
			id: "460794187",
			name: "Tenaska Gateway Generating Station",
			lon_lat: new LonLat(-94.6213806f, 32.0151595f),
			outline: new List<LonLat>() {
				new LonLat(-94.6220673f, 32.0151595f),
				new LonLat(-94.6183336f, 32.0201275f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 933f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "460979363",
			name: "Trinidad Power Plant",
			lon_lat: new LonLat(-96.1071751f, 32.1245108f),
			outline: new List<LonLat>() {
				new LonLat(-96.1071751f, 32.1232092f),
				new LonLat(-96.0959474f, 32.1303797f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 244f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "462426864",
			name: "Panda Temple 2 Power Project",
			lon_lat: new LonLat(-97.31702f, 31.0542039f),
			outline: new List<LonLat>() {
				new LonLat(-97.3185391f, 31.0542039f),
				new LonLat(-97.3156023f, 31.0581005f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 770f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "462426866",
			name: "Panda Temple 1 Power Project",
			lon_lat: new LonLat(-97.31702f, 31.0542039f),
			outline: new List<LonLat>() {
				new LonLat(-97.31702f, 31.0540069f),
				new LonLat(-97.3140752f, 31.0579514f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 770f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "462642572",
			name: "Sand Hill Energy Center",
			lon_lat: new LonLat(-97.6106188f, 30.2097881f),
			outline: new List<LonLat>() {
				new LonLat(-97.6165749f, 30.2079928f),
				new LonLat(-97.6106188f, 30.2118319f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 570f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "462642757",
			name: "Bastrop Energy Center",
			lon_lat: new LonLat(-97.552272f, 30.1459743f),
			outline: new List<LonLat>() {
				new LonLat(-97.552272f, 30.1434943f),
				new LonLat(-97.5458535f, 30.1478635f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 552f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "465471410",
			name: "Alex 'Ty' Cooke Station",
			lon_lat: new LonLat(-101.7872004f, 33.521485f),
			outline: new List<LonLat>() {
				new LonLat(-101.7935045f, 33.5200553f),
				new LonLat(-101.7871779f, 33.5227288f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 158f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "465695519",
			name: "Massengale Generating Station",
			lon_lat: new LonLat(-101.8414499f, 33.6046006f),
			outline: new List<LonLat>() {
				new LonLat(-101.8429658f, 33.6027925f),
				new LonLat(-101.8398473f, 33.6046006f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 86f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "465695766",
			name: "Jones Generating Station",
			lon_lat: new LonLat(-101.7351642f, 33.5321549f),
			outline: new List<LonLat>() {
				new LonLat(-101.7456517f, 33.5202529f),
				new LonLat(-101.7350032f, 33.5321549f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 824f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "467959231",
			name: "Unknown",
			lon_lat: new LonLat(-100.9552088f, 32.748337f),
			outline: new List<LonLat>() {
				new LonLat(-100.9552138f, 32.7467307f),
				new LonLat(-100.9534251f, 32.748337f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 132f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "468019587",
			name: "Power Resources Cogeneration Plant",
			lon_lat: new LonLat(-101.4235133f, 32.2745263f),
			outline: new List<LonLat>() {
				new LonLat(-101.4235133f, 32.2719508f),
				new LonLat(-101.4214259f, 32.2748145f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 212f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "490163553",
			name: "Antelope Elk Energy Center",
			lon_lat: new LonLat(-101.8443643f, 33.8669951f),
			outline: new List<LonLat>() {
				new LonLat(-101.8444233f, 33.8616476f),
				new LonLat(-101.8315041f, 33.8669951f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 740f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_Hydro(
			id: "493980715",
			name: "Toledo Bend Power Plant",
			lon_lat: new LonLat(-93.5667668f, 31.1732859f),
			outline: new List<LonLat>() {
				new LonLat(-93.5667668f, 31.1724143f),
				new LonLat(-93.5637722f, 31.1748666f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 90f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_ThermalCombustion(
			id: "494732432",
			name: "Dolet Hills Power Station",
			lon_lat: new LonLat(-93.5739899f, 32.0363384f),
			outline: new List<LonLat>() {
				new LonLat(-93.5754275f, 32.027061f),
				new LonLat(-93.5559547f, 32.0371023f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 650f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "496987908",
			name: "Dansby Power Plant",
			lon_lat: new LonLat(-96.4636906f, 30.7217694f),
			outline: new List<LonLat>() {
				new LonLat(-96.4637268f, 30.7202603f),
				new LonLat(-96.4555694f, 30.7227993f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 199f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "504555597",
			name: "Thomas C. Ferguson Power Plant",
			lon_lat: new LonLat(-98.3739484f, 30.5593788f),
			outline: new List<LonLat>() {
				new LonLat(-98.374971f, 30.554955f),
				new LonLat(-98.3691721f, 30.5597943f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("1974-1-1"),
			power_mw: 520f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_Hydro(
			id: "504607152",
			name: "Starcke Power Plant",
			lon_lat: new LonLat(-98.2582922f, 30.5549682f),
			outline: new List<LonLat>() {
				new LonLat(-98.2582922f, 30.5547522f),
				new LonLat(-98.2556955f, 30.5575504f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("1951-1-1"),
			power_mw: 41f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_Hydro(
			id: "504607276",
			name: "Wirtz Power Plant",
			lon_lat: new LonLat(-98.3380879f, 30.5567282f),
			outline: new List<LonLat>() {
				new LonLat(-98.3386163f, 30.5559065f),
				new LonLat(-98.3379878f, 30.5567282f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("1951-1-1"),
			power_mw: 60f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_Hydro(
			id: "504634990",
			name: "Buchanan Power Plant",
			lon_lat: new LonLat(-98.4181546f, 30.7503776f),
			outline: new List<LonLat>() {
				new LonLat(-98.4181546f, 30.7503742f),
				new LonLat(-98.4173325f, 30.7516166f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("1938-1-1"),
			power_mw: 49f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_Hydro(
			id: "504635016",
			name: "Inks Power Plant",
			lon_lat: new LonLat(-98.3851054f, 30.7313192f),
			outline: new List<LonLat>() {
				new LonLat(-98.3858291f, 30.7311535f),
				new LonLat(-98.3851054f, 30.7316757f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("1938-1-1"),
			power_mw: 14f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_Hydro(
			id: "508017023",
			name: "Lake Whitney Power Plant",
			lon_lat: new LonLat(-97.3718035f, 31.8654938f),
			outline: new List<LonLat>() {
				new LonLat(-97.3722885f, 31.8643149f),
				new LonLat(-97.3710926f, 31.8654938f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("1953-1-1"),
			power_mw: 48f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_Hydro(
			id: "508684688",
			name: "Marshall Ford Power Plant",
			lon_lat: new LonLat(-97.9071694f, 30.3912307f),
			outline: new List<LonLat>() {
				new LonLat(-97.9097083f, 30.3886074f),
				new LonLat(-97.9063971f, 30.3912307f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("1941-1-1"),
			power_mw: 108f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_Hydro(
			id: "508764501",
			name: "Tom Miller Power Plant",
			lon_lat: new LonLat(-97.7852191f, 30.2938678f),
			outline: new List<LonLat>() {
				new LonLat(-97.7852384f, 30.2934051f),
				new LonLat(-97.7844787f, 30.2938678f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("1940-1-1"),
			power_mw: 17f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_ThermalCombustion(
			id: "509273328",
			name: "Decker Creek Power Station",
			lon_lat: new LonLat(-97.6196519f, 30.3038116f),
			outline: new List<LonLat>() {
				new LonLat(-97.6196519f, 30.3017815f),
				new LonLat(-97.6088288f, 30.3108058f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 958f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "510839027",
			name: "Winchester Power Park",
			lon_lat: new LonLat(-96.99489f, 30.0247344f),
			outline: new List<LonLat>() {
				new LonLat(-96.99489f, 30.0236905f),
				new LonLat(-96.9921085f, 30.0260372f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2009-1-1"),
			power_mw: 184f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "512042697",
			name: "Red Gate Power Plant",
			lon_lat: new LonLat(-98.1786266f, 26.4510913f),
			outline: new List<LonLat>() {
				new LonLat(-98.1786266f, 26.4492374f),
				new LonLat(-98.1755987f, 26.4518829f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2016-1-1"),
			power_mw: 220f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "512644989",
			name: "Frontera Generation Plant",
			lon_lat: new LonLat(-98.4006223f, 26.2066751f),
			outline: new List<LonLat>() {
				new LonLat(-98.4006223f, 26.2062696f),
				new LonLat(-98.3967693f, 26.2109466f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("1999-1-1"),
			power_mw: 524f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "513680414",
			name: "Silas Ray Power Plant",
			lon_lat: new LonLat(-97.5202864f, 25.9130788f),
			outline: new List<LonLat>() {
				new LonLat(-97.5240093f, 25.9114117f),
				new LonLat(-97.5202864f, 25.9151101f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 126f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "514740384",
			name: "Ingleside Cogeneration Plant",
			lon_lat: new LonLat(-97.2452581f, 27.884613f),
			outline: new List<LonLat>() {
				new LonLat(-97.2475795f, 27.8816459f),
				new LonLat(-97.2449403f, 27.8846414f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 498f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "514740385",
			name: "Gregory Cogeneration Plant",
			lon_lat: new LonLat(-97.2604421f, 27.8892951f),
			outline: new List<LonLat>() {
				new LonLat(-97.2604421f, 27.8884227f),
				new LonLat(-97.2572969f, 27.8905504f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 488f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "515325972",
			name: "Coleto Creek Power Station",
			lon_lat: new LonLat(-97.2211933f, 28.732094f),
			outline: new List<LonLat>() {
				new LonLat(-97.230313f, 28.7090759f),
				new LonLat(-97.1942375f, 28.7353115f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("1980-1-1"),
			power_mw: 650f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "515589164",
			name: "Port Comfort Power Plant",
			lon_lat: new LonLat(-96.5462807f, 28.6483639f),
			outline: new List<LonLat>() {
				new LonLat(-96.5462807f, 28.6458308f),
				new LonLat(-96.5436812f, 28.6484009f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 100f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "515589171",
			name: "Formosa Cogeneration Plant",
			lon_lat: new LonLat(-96.5448097f, 28.6932856f),
			outline: new List<LonLat>() {
				new LonLat(-96.5448613f, 28.6932856f),
				new LonLat(-96.5418269f, 28.6967341f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 655f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "515589291",
			name: "Formosa CFB Plant",
			lon_lat: new LonLat(-96.5429552f, 28.6514398f),
			outline: new List<LonLat>() {
				new LonLat(-96.5429552f, 28.6456117f),
				new LonLat(-96.5367204f, 28.651807f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2009-1-1"),
			power_mw: 286f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "515655227",
			name: "Sam Rayburn Power Plant",
			lon_lat: new LonLat(-97.1340517f, 28.8966594f),
			outline: new List<LonLat>() {
				new LonLat(-97.1372911f, 28.8903812f),
				new LonLat(-97.1309048f, 28.8969723f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 218f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "515655407",
			name: "Victoria Power Station",
			lon_lat: new LonLat(-97.0109114f, 28.7897144f),
			outline: new List<LonLat>() {
				new LonLat(-97.0124002f, 28.7855391f),
				new LonLat(-97.007385f, 28.7899332f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 303f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "520305123",
			name: "Sky Global Power One",
			lon_lat: new LonLat(-96.5389386f, 29.5499282f),
			outline: new List<LonLat>() {
				new LonLat(-96.5396923f, 29.5499282f),
				new LonLat(-96.5377856f, 29.5515569f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2016-1-1"),
			power_mw: 52f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "520385174",
			name: "Newgulf Cogeneration Plant",
			lon_lat: new LonLat(-95.901293f, 29.2634318f),
			outline: new List<LonLat>() {
				new LonLat(-95.9012957f, 29.2623355f),
				new LonLat(-95.8993202f, 29.2645338f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 85f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "520385241",
			name: "Colorado Bend Power Plant",
			lon_lat: new LonLat(-96.0648152f, 29.2914051f),
			outline: new List<LonLat>() {
				new LonLat(-96.0718372f, 29.2847637f),
				new LonLat(-96.0632086f, 29.2933513f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2007-1-1"),
			power_mw: 1592f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "521857228",
			name: "P. H. Robinson Peaker Plant",
			lon_lat: new LonLat(-94.9851939f, 29.4930912f),
			outline: new List<LonLat>() {
				new LonLat(-94.9854863f, 29.4910507f),
				new LonLat(-94.9829248f, 29.4930912f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2017-1-1"),
			power_mw: 324f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "583388823",
			name: "Sweeny Cogeneration",
			lon_lat: new LonLat(-95.7446694f, 29.0734611f),
			outline: new List<LonLat>() {
				new LonLat(-95.7462734f, 29.07154f),
				new LonLat(-95.7430306f, 29.0734611f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 450f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "584370755",
			name: "South Houston Green Power",
			lon_lat: new LonLat(-94.9340382f, 29.3789326f),
			outline: new List<LonLat>() {
				new LonLat(-94.9353647f, 29.3775674f),
				new LonLat(-94.9310294f, 29.3791227f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2003-1-1"),
			power_mw: 611f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "584370756",
			name: "Texas City Power",
			lon_lat: new LonLat(-94.9446309f, 29.3791685f),
			outline: new List<LonLat>() {
				new LonLat(-94.9446309f, 29.3780464f),
				new LonLat(-94.9416053f, 29.3792138f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("1987-1-1"),
			power_mw: 420f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "585023319",
			name: "Johnson Space Center Combined Heat & Power",
			lon_lat: new LonLat(-95.0895416f, 29.5622086f),
			outline: new List<LonLat>() {
				new LonLat(-95.0895416f, 29.5620667f),
				new LonLat(-95.0889877f, 29.5625822f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2017-1-1"),
			power_mw: 11.9f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "585622300",
			name: "Bayou Cogen Plant",
			lon_lat: new LonLat(-95.0476572f, 29.6254274f),
			outline: new List<LonLat>() {
				new LonLat(-95.0478396f, 29.6254274f),
				new LonLat(-95.0456308f, 29.6271224f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("1984-1-1"),
			power_mw: 300f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "585787416",
			name: "Fiendswood Energy Center",
			lon_lat: new LonLat(-95.4511006f, 29.6477731f),
			outline: new List<LonLat>() {
				new LonLat(-95.452098f, 29.6463674f),
				new LonLat(-95.4510709f, 29.6477731f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2017-1-1"),
			power_mw: 121f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "604028349",
			name: "Hardin County Peaking Power Facility",
			lon_lat: new LonLat(-94.25346f, 30.3042833f),
			outline: new List<LonLat>() {
				new LonLat(-94.25346f, 30.3027865f),
				new LonLat(-94.2516829f, 30.3045707f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2010-1-1"),
			power_mw: 150f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "683998447",
			name: "J. Robert Welsh Power Plant",
			lon_lat: new LonLat(-94.8481143f, 33.0530664f),
			outline: new List<LonLat>() {
				new LonLat(-94.8481143f, 33.0527899f),
				new LonLat(-94.8362093f, 33.0574996f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 1584f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "2741038",
			name: "Barney Davis Power Plant",
			lon_lat: new LonLat(-97.2983703f, 27.6098828f),
			outline: new List<LonLat>() {
				new LonLat(-97.3538178f, 27.5958751f),
				new LonLat(-97.2955518f, 27.6307511f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 975f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "4431114",
			name: "Braunig Power Station",
			lon_lat: new LonLat(-98.3789544f, 29.2584107f),
			outline: new List<LonLat>() {
				new LonLat(-98.3915723f, 29.2554829f),
				new LonLat(-98.3707005f, 29.2732547f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 1604f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "4572280",
			name: "Calaveras Power Station",
			lon_lat: new LonLat(-98.3302507f, 29.3056348f),
			outline: new List<LonLat>() {
				new LonLat(-98.3302997f, 29.2917997f),
				new LonLat(-98.2991481f, 29.3373033f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 3010f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal(),
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_SolarFarm(
			id: "4871408",
			name: "Alamo 4",
			lon_lat: new LonLat(-100.3859034f, 29.3119466f),
			outline: new List<LonLat>() {
				new LonLat(-100.3942965f, 29.3108398f),
				new LonLat(-100.3810295f, 29.3305649f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2014-1-1"),
			power_mw: 40f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "4920626",
			name: "Anacacho Wind Farm",
			lon_lat: new LonLat(-100.1769359f, 29.2040492f),
			outline: new List<LonLat>() {
				new LonLat(-100.2296077f, 29.1628494f),
				new LonLat(-100.1387762f, 29.2170684f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2012-1-1"),
			power_mw: 99.8f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6706822",
			name: "Javelina Wind",
			lon_lat: new LonLat(-99.0065283f, 27.3076633f),
			outline: new List<LonLat>() {
				new LonLat(-99.068437f, 27.2721514f),
				new LonLat(-98.9146445f, 27.3952028f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 249.7f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6706823",
			name: "Whitetail Wind Energy",
			lon_lat: new LonLat(-99.0057672f, 27.5065699f),
			outline: new List<LonLat>() {
				new LonLat(-99.0169923f, 27.4689561f),
				new LonLat(-98.9595488f, 27.5195299f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 91f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6706824",
			name: "Cedro Hill Wind",
			lon_lat: new LonLat(-98.8917063f, 27.5994956f),
			outline: new List<LonLat>() {
				new LonLat(-98.9776935f, 27.5326007f),
				new LonLat(-98.8536623f, 27.6033581f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 150f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6706825",
			name: "Los Vientos Wind IV",
			lon_lat: new LonLat(-98.7151958f, 26.4723203f),
			outline: new List<LonLat>() {
				new LonLat(-98.7425195f, 26.4723203f),
				new LonLat(-98.636591f, 26.6483769f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 200f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6706826",
			name: "Los Vientos Wind V",
			lon_lat: new LonLat(-98.5604359f, 26.4186442f),
			outline: new List<LonLat>() {
				new LonLat(-98.5913113f, 26.3491834f),
				new LonLat(-98.5536383f, 26.4530095f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 110f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6706827",
			name: "Los Vientos Wind III",
			lon_lat: new LonLat(-98.6051246f, 26.4159754f),
			outline: new List<LonLat>() {
				new LonLat(-98.6373036f, 26.3837134f),
				new LonLat(-98.5432339f, 26.5419416f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 200f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6708570",
			name: "Sendero WInd",
			lon_lat: new LonLat(-98.8671866f, 27.1788509f),
			outline: new List<LonLat>() {
				new LonLat(-98.9589075f, 27.1219887f),
				new LonLat(-98.8623198f, 27.235142f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 78f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_NuclearFission(
			id: "6732566",
			name: "Comanche Peak Nuclear Power Plant",
			lon_lat: new LonLat(-97.794553f, 32.3041418f),
			outline: new List<LonLat>() {
				new LonLat(-97.8184328f, 32.2907667f),
				new LonLat(-97.767851f, 32.3052479f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 2400f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6775541",
			name: "Stanton Wind Project",
			lon_lat: new LonLat(-101.8120933f, 32.1745923f),
			outline: new List<LonLat>() {
				new LonLat(-101.8647156f, 32.159463f),
				new LonLat(-101.7455237f, 32.2536441f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 120f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6785669",
			name: "Notrees Windpower",
			lon_lat: new LonLat(-102.8199222f, 32.0195776f),
			outline: new List<LonLat>() {
				new LonLat(-102.8708517f, 31.9147654f),
				new LonLat(-102.774247f, 32.0380868f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 153f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6801807",
			name: "Capricorn Ridge Wind I",
			lon_lat: new LonLat(-100.9568684f, 32.0223258f),
			outline: new List<LonLat>() {
				new LonLat(-101.0038626f, 31.9367501f),
				new LonLat(-100.8763234f, 32.0379353f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 214.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6801808",
			name: "Capricorn Ridge Wind II",
			lon_lat: new LonLat(-101.0070259f, 31.924695f),
			outline: new List<LonLat>() {
				new LonLat(-101.0441603f, 31.8723737f),
				new LonLat(-100.8999862f, 31.9534307f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 186f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6801809",
			name: "Capricorn Ridge Wind III",
			lon_lat: new LonLat(-100.9477413f, 31.8939008f),
			outline: new List<LonLat>() {
				new LonLat(-100.9477413f, 31.8350072f),
				new LonLat(-100.8597786f, 31.9095066f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 149.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6801810",
			name: "Capricorn Ridge Wind IV",
			lon_lat: new LonLat(-100.8201274f, 31.8744104f),
			outline: new List<LonLat>() {
				new LonLat(-100.8658521f, 31.8702372f),
				new LonLat(-100.7796533f, 31.9218981f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 112.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6801811",
			name: "Goat Mountain Wind Ranch 2",
			lon_lat: new LonLat(-100.782773f, 31.9518072f),
			outline: new List<LonLat>() {
				new LonLat(-100.8274364f, 31.9404004f),
				new LonLat(-100.7540963f, 31.9838238f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 69.6f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6801812",
			name: "Goat Mountain Wind Ranch 1",
			lon_lat: new LonLat(-100.7983434f, 31.9496216f),
			outline: new List<LonLat>() {
				new LonLat(-100.8323671f, 31.9267542f),
				new LonLat(-100.7841992f, 31.967804f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 80f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6891301",
			name: "Stephens Ranch Wind Project II",
			lon_lat: new LonLat(-101.6820871f, 32.9889164f),
			outline: new List<LonLat>() {
				new LonLat(-101.7353665f, 32.9639606f),
				new LonLat(-101.6073018f, 33.0285754f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 164.7f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6891302",
			name: "Stephens Ranch Wind Project I",
			lon_lat: new LonLat(-101.6767013f, 32.868235f),
			outline: new List<LonLat>() {
				new LonLat(-101.7078095f, 32.8489215f),
				new LonLat(-101.6120386f, 32.9639764f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 211.2f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6891303",
			name: "Cirrus 1 Wind Project",
			lon_lat: new LonLat(-101.6810853f, 33.0329492f),
			outline: new List<LonLat>() {
				new LonLat(-101.7354658f, 33.0102304f),
				new LonLat(-101.6725613f, 33.0333538f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 61.2f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6903041",
			name: "Camp Springs Wind 1",
			lon_lat: new LonLat(-100.8137256f, 32.7678167f),
			outline: new List<LonLat>() {
				new LonLat(-100.9141462f, 32.7406925f),
				new LonLat(-100.7878223f, 32.8049532f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2007-1-1"),
			power_mw: 130.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6903042",
			name: "Camp Springs Wind 2",
			lon_lat: new LonLat(-100.7970723f, 32.7232572f),
			outline: new List<LonLat>() {
				new LonLat(-100.8385912f, 32.6954634f),
				new LonLat(-100.7385135f, 32.7770873f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2007-1-1"),
			power_mw: 120f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6903043",
			name: "Snyder Wind Farm",
			lon_lat: new LonLat(-100.7568051f, 32.7441968f),
			outline: new List<LonLat>() {
				new LonLat(-100.7576304f, 32.727403f),
				new LonLat(-100.7225511f, 32.7497432f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2007-1-1"),
			power_mw: 63f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6903044",
			name: "Brazos Wind 1",
			lon_lat: new LonLat(-101.1150291f, 32.9428614f),
			outline: new List<LonLat>() {
				new LonLat(-101.1766857f, 32.9257217f),
				new LonLat(-101.0914081f, 32.9614494f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 180f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6903045",
			name: "Red Canyon Wind",
			lon_lat: new LonLat(-101.2542814f, 32.9781537f),
			outline: new List<LonLat>() {
				new LonLat(-101.2786585f, 32.9185625f),
				new LonLat(-101.1796443f, 32.9852163f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 84f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6903046",
			name: "Brazos Wind 2",
			lon_lat: new LonLat(-101.3267251f, 32.9439239f),
			outline: new List<LonLat>() {
				new LonLat(-101.3411378f, 32.928348f),
				new LonLat(-101.2791391f, 32.9490845f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 180f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6903047",
			name: "Bull Creek Wind",
			lon_lat: new LonLat(-101.5960575f, 32.8960047f),
			outline: new List<LonLat>() {
				new LonLat(-101.6178461f, 32.8921003f),
				new LonLat(-101.5348082f, 32.9609083f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 180f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6903835",
			name: "Sand Bluff Wind",
			lon_lat: new LonLat(-101.3080396f, 31.993992f),
			outline: new List<LonLat>() {
				new LonLat(-101.375984f, 31.9417678f),
				new LonLat(-101.2235891f, 32.0254219f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2008-1-1"),
			power_mw: 90f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6903837",
			name: "Panther Creek Wind 2",
			lon_lat: new LonLat(-101.3185687f, 31.9327351f),
			outline: new List<LonLat>() {
				new LonLat(-101.3515207f, 31.914969f),
				new LonLat(-101.2477597f, 31.9671822f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2008-1-1"),
			power_mw: 115.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6903838",
			name: "Panther Creek Wind 1",
			lon_lat: new LonLat(-101.4377504f, 32.0475388f),
			outline: new List<LonLat>() {
				new LonLat(-101.477303f, 31.9626041f),
				new LonLat(-101.3685688f, 32.137078f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2008-1-1"),
			power_mw: 142.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6903839",
			name: "Elbow Creek Wind Farm",
			lon_lat: new LonLat(-101.4292221f, 32.1321683f),
			outline: new List<LonLat>() {
				new LonLat(-101.437937f, 32.1178072f),
				new LonLat(-101.3866841f, 32.2211849f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 58.8f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6903840",
			name: "Ocotillo Wind Farm",
			lon_lat: new LonLat(-101.4169707f, 32.1440772f),
			outline: new List<LonLat>() {
				new LonLat(-101.4169707f, 32.1225448f),
				new LonLat(-101.3747224f, 32.1626548f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 58.8f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6903841",
			name: "Panther Creek Wind 3",
			lon_lat: new LonLat(-101.1323529f, 31.9392106f),
			outline: new List<LonLat>() {
				new LonLat(-101.1753635f, 31.9392106f),
				new LonLat(-101.057109f, 32.0342692f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2009-1-1"),
			power_mw: 199.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6903842",
			name: "Big Spring Wind 2",
			lon_lat: new LonLat(-101.4022451f, 32.2139392f),
			outline: new List<LonLat>() {
				new LonLat(-101.4044112f, 32.2070456f),
				new LonLat(-101.3877811f, 32.2141152f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 6.6f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6903843",
			name: "Big Spring Wind 1",
			lon_lat: new LonLat(-101.4349339f, 32.1897056f),
			outline: new List<LonLat>() {
				new LonLat(-101.4444891f, 32.1667329f),
				new LonLat(-101.358342f, 32.2155919f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 27.72f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6903844",
			name: "Mesquite Creek Wind",
			lon_lat: new LonLat(-101.7614601f, 32.7250541f),
			outline: new List<LonLat>() {
				new LonLat(-101.8216485f, 32.6621097f),
				new LonLat(-101.6754467f, 32.7632679f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 200.6f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6906783",
			name: "McAdoo Wind Energy",
			lon_lat: new LonLat(-100.9831036f, 33.7444519f),
			outline: new List<LonLat>() {
				new LonLat(-101.0373134f, 33.7319194f),
				new LonLat(-100.9269712f, 33.8124494f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 150f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6929428",
			name: "Whirlwind Energy Center",
			lon_lat: new LonLat(-101.1039667f, 34.1567572f),
			outline: new List<LonLat>() {
				new LonLat(-101.110858f, 34.0757698f),
				new LonLat(-101.0823293f, 34.1567572f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 59.8f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_ThermalCombustion(
			id: "7173583",
			name: "Nueces Bay Power Plant",
			lon_lat: new LonLat(-97.4179224f, 27.8198664f),
			outline: new List<LonLat>() {
				new LonLat(-97.4220012f, 27.8178631f),
				new LonLat(-97.4170102f, 27.8226946f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 655f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_WindFarm(
			id: "7192089",
			name: "Papalote Creek Wind Farm 1",
			lon_lat: new LonLat(-97.5283957f, 27.9167442f),
			outline: new List<LonLat>() {
				new LonLat(-97.5352155f, 27.8999848f),
				new LonLat(-97.3771607f, 27.9686735f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 179.85f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7192090",
			name: "Papalote Creek Wind Farm 2",
			lon_lat: new LonLat(-97.3669617f, 28.000793f),
			outline: new List<LonLat>() {
				new LonLat(-97.42716f, 27.9236656f),
				new LonLat(-97.2714254f, 28.0430497f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 200.1f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7200716",
			name: "South Plains Wind 1",
			lon_lat: new LonLat(-101.2910793f, 34.1372988f),
			outline: new List<LonLat>() {
				new LonLat(-101.4072511f, 34.0561814f),
				new LonLat(-101.2735591f, 34.1729264f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 200f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7200717",
			name: "South Plains Wind 2",
			lon_lat: new LonLat(-101.3910989f, 34.1727311f),
			outline: new List<LonLat>() {
				new LonLat(-101.4303771f, 34.1727311f),
				new LonLat(-101.2936971f, 34.2533521f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 300.3f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214571",
			name: "Sweetwater Wind 2",
			lon_lat: new LonLat(-100.3304034f, 32.3679472f),
			outline: new List<LonLat>() {
				new LonLat(-100.3805254f, 32.3075438f),
				new LonLat(-100.3256559f, 32.3831029f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 91.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214572",
			name: "Sweetwater Wind 3",
			lon_lat: new LonLat(-100.411136f, 32.3009236f),
			outline: new List<LonLat>() {
				new LonLat(-100.5036026f, 32.2589337f),
				new LonLat(-100.3698051f, 32.3736077f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 135f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214573",
			name: "Sweetwater Wind 1",
			lon_lat: new LonLat(-100.3771164f, 32.3525056f),
			outline: new List<LonLat>() {
				new LonLat(-100.4069551f, 32.3354121f),
				new LonLat(-100.3506835f, 32.4383186f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 37.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214574",
			name: "Sweetwater Wind 5",
			lon_lat: new LonLat(-100.4820637f, 32.2203407f),
			outline: new List<LonLat>() {
				new LonLat(-100.5155419f, 32.2194425f),
				new LonLat(-100.4354498f, 32.2440735f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 80.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214575",
			name: "Horse Hollow 2",
			lon_lat: new LonLat(-100.2006113f, 32.2672688f),
			outline: new List<LonLat>() {
				new LonLat(-100.2281971f, 32.1767808f),
				new LonLat(-99.9044538f, 32.3041758f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 299f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214576",
			name: "Horse Hollow 1",
			lon_lat: new LonLat(-99.9852338f, 32.1992246f),
			outline: new List<LonLat>() {
				new LonLat(-100.1403863f, 32.1805619f),
				new LonLat(-99.9582902f, 32.2659951f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 213f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214577",
			name: "Horse Hollow 3",
			lon_lat: new LonLat(-100.3087737f, 32.265433f),
			outline: new List<LonLat>() {
				new LonLat(-100.3270513f, 32.2522497f),
				new LonLat(-100.2115228f, 32.3055725f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 223.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214578",
			name: "Turkey Track Energy Center",
			lon_lat: new LonLat(-100.2610575f, 32.1517282f),
			outline: new List<LonLat>() {
				new LonLat(-100.2942444f, 32.148272f),
				new LonLat(-100.178735f, 32.2453336f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 169.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214579",
			name: "Buffalo Gap 3",
			lon_lat: new LonLat(-100.1665072f, 32.2947771f),
			outline: new List<LonLat>() {
				new LonLat(-100.2074737f, 32.2866f),
				new LonLat(-100.1002632f, 32.3122008f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 170.2f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214580",
			name: "Buffalo Gap 1",
			lon_lat: new LonLat(-100.0959144f, 32.3047825f),
			outline: new List<LonLat>() {
				new LonLat(-100.1669862f, 32.296908f),
				new LonLat(-100.0540107f, 32.3252877f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 120.6f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214581",
			name: "Buffalo Gap 2",
			lon_lat: new LonLat(-100.202108f, 32.3169986f),
			outline: new List<LonLat>() {
				new LonLat(-100.2547699f, 32.3106254f),
				new LonLat(-100.1691297f, 32.3780693f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 232.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214582",
			name: "Callahan Divide Wind Energy Center",
			lon_lat: new LonLat(-100.0482913f, 32.305314f),
			outline: new List<LonLat>() {
				new LonLat(-100.049978f, 32.2993059f),
				new LonLat(-99.9732429f, 32.3571316f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 114f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214583",
			name: "Trent Wind Farm",
			lon_lat: new LonLat(-100.180972f, 32.4248963f),
			outline: new List<LonLat>() {
				new LonLat(-100.2824004f, 32.4106841f),
				new LonLat(-100.1470636f, 32.4560274f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 150f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214584",
			name: "South Trent Wind Farm",
			lon_lat: new LonLat(-100.1871104f, 32.4214276f),
			outline: new List<LonLat>() {
				new LonLat(-100.2702391f, 32.4016703f),
				new LonLat(-100.1041511f, 32.4333713f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 101.2f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214585",
			name: "Sweetwater Wind 4A",
			lon_lat: new LonLat(-100.5061313f, 32.2783157f),
			outline: new List<LonLat>() {
				new LonLat(-100.5961504f, 32.268254f),
				new LonLat(-100.3010098f, 32.3583872f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 135f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214586",
			name: "Sweetwater Wind 4B",
			lon_lat: new LonLat(-100.551469f, 32.2519391f),
			outline: new List<LonLat>() {
				new LonLat(-100.551469f, 32.2487678f),
				new LonLat(-100.4571035f, 32.2865924f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 105.8f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214587",
			name: "Roscoe Wind Farm",
			lon_lat: new LonLat(-100.6573482f, 32.4508768f),
			outline: new List<LonLat>() {
				new LonLat(-100.7420767f, 32.4298074f),
				new LonLat(-100.5638883f, 32.5312383f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2008-1-1"),
			power_mw: 209f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214588",
			name: "Loraine Wind Farm",
			lon_lat: new LonLat(-100.7613882f, 32.4624508f),
			outline: new List<LonLat>() {
				new LonLat(-100.7806972f, 32.4097876f),
				new LonLat(-100.6621129f, 32.5050082f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 150f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214589",
			name: "Inadale Wind Farm",
			lon_lat: new LonLat(-100.5660029f, 32.4831092f),
			outline: new List<LonLat>() {
				new LonLat(-100.6819567f, 32.4642217f),
				new LonLat(-100.5147719f, 32.5460004f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2008-1-1"),
			power_mw: 197f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214590",
			name: "Champion Wind Farm",
			lon_lat: new LonLat(-100.6071276f, 32.3808449f),
			outline: new List<LonLat>() {
				new LonLat(-100.6645932f, 32.3772799f),
				new LonLat(-100.5588834f, 32.4103019f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2008-1-1"),
			power_mw: 126.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7214591",
			name: "Pyron Wind Farm",
			lon_lat: new LonLat(-100.6588846f, 32.5517776f),
			outline: new List<LonLat>() {
				new LonLat(-100.7446601f, 32.5452056f),
				new LonLat(-100.5938227f, 32.6362174f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2008-1-1"),
			power_mw: 249f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7244620",
			name: "Langford Wind Farm",
			lon_lat: new LonLat(-100.6066453f, 31.099929f),
			outline: new List<LonLat>() {
				new LonLat(-100.7371688f, 31.0695955f),
				new LonLat(-100.5876671f, 31.1663401f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 150f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_ThermalCombustion(
			id: "7278643",
			name: "Lieberman Power Plant",
			lon_lat: new LonLat(-93.9653576f, 32.7067804f),
			outline: new List<LonLat>() {
				new LonLat(-93.9653576f, 32.7031243f),
				new LonLat(-93.959139f, 32.706982f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 268f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "7278644",
			name: "Arsenal Hill Power Plant",
			lon_lat: new LonLat(-93.7600775f, 32.5201105f),
			outline: new List<LonLat>() {
				new LonLat(-93.7675408f, 32.5146496f),
				new LonLat(-93.7582576f, 32.5211755f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("1926-1-1"),
			power_mw: 624f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_SolarFarm(
			id: "7290017",
			name: "Alamo 7",
			lon_lat: new LonLat(-99.5901686f, 33.0137355f),
			outline: new List<LonLat>() {
				new LonLat(-99.6246243f, 33.0049296f),
				new LonLat(-99.5762426f, 33.0160498f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2016-1-1"),
			power_mw: 106f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7290991",
			name: "Desert Sky Wind Farm",
			lon_lat: new LonLat(-102.1019588f, 30.9248641f),
			outline: new List<LonLat>() {
				new LonLat(-102.1604364f, 30.9011798f),
				new LonLat(-102.0635891f, 30.9552094f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 160.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7290992",
			name: "Indian Mesa Wind Energy Center",
			lon_lat: new LonLat(-102.1888429f, 30.9340359f),
			outline: new List<LonLat>() {
				new LonLat(-102.2352647f, 30.9243715f),
				new LonLat(-102.1617337f, 30.9614922f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2001-1-1"),
			power_mw: 82.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7290993",
			name: "Southwest Mesa Wind Energy Center",
			lon_lat: new LonLat(-102.1323285f, 31.0548197f),
			outline: new List<LonLat>() {
				new LonLat(-102.1379798f, 31.0538634f),
				new LonLat(-102.0552389f, 31.1023708f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("1999-1-1"),
			power_mw: 74.2f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7308922",
			name: "Rattlesnake Den 1",
			lon_lat: new LonLat(-101.4528608f, 31.7132732f),
			outline: new List<LonLat>() {
				new LonLat(-101.5314817f, 31.6567422f),
				new LonLat(-101.3743419f, 31.7733407f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 207f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7309299",
			name: "Wake Wind Wind Energy",
			lon_lat: new LonLat(-101.1363709f, 33.8568919f),
			outline: new List<LonLat>() {
				new LonLat(-101.2132859f, 33.7634776f),
				new LonLat(-101.0430515f, 33.9227794f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 257.25f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7309565",
			name: "Briscoe Wind Farm",
			lon_lat: new LonLat(-101.3143623f, 34.3208616f),
			outline: new List<LonLat>() {
				new LonLat(-101.3403905f, 34.3208616f),
				new LonLat(-101.2668014f, 34.4219563f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 149.85f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7309566",
			name: "Longhorn Wind Farm",
			lon_lat: new LonLat(-101.2550747f, 34.3173438f),
			outline: new List<LonLat>() {
				new LonLat(-101.2937415f, 34.2745464f),
				new LonLat(-101.1892104f, 34.3468997f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 200f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7315266",
			name: "Blue Summit Wind Energy Center",
			lon_lat: new LonLat(-99.3604589f, 34.3453008f),
			outline: new List<LonLat>() {
				new LonLat(-99.4576608f, 34.2429207f),
				new LonLat(-99.3156982f, 34.3609785f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 135.4f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7318397",
			name: "Electra Wind",
			lon_lat: new LonLat(-99.1484892f, 34.0927309f),
			outline: new List<LonLat>() {
				new LonLat(-99.1667819f, 34.0651566f),
				new LonLat(-99.0003508f, 34.1571795f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 230f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7328009",
			name: "King Mountain Wind Northwest",
			lon_lat: new LonLat(-102.2442888f, 31.2342672f),
			outline: new List<LonLat>() {
				new LonLat(-102.2551304f, 31.231629f),
				new LonLat(-102.2265695f, 31.2764881f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2001-1-1"),
			power_mw: 79.3f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7328010",
			name: "King Mountain Wind Southwest",
			lon_lat: new LonLat(-102.2556674f, 31.2083575f),
			outline: new List<LonLat>() {
				new LonLat(-102.2776028f, 31.2058095f),
				new LonLat(-102.2204196f, 31.2307493f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2001-1-1"),
			power_mw: 79.3f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7328011",
			name: "King Mountain Wind Southeast",
			lon_lat: new LonLat(-102.1594392f, 31.2216442f),
			outline: new List<LonLat>() {
				new LonLat(-102.160681f, 31.1859045f),
				new LonLat(-102.1320632f, 31.2403501f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2001-1-1"),
			power_mw: 43.3f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7328012",
			name: "King Mountain Wind Northeast",
			lon_lat: new LonLat(-102.1570167f, 31.2803187f),
			outline: new List<LonLat>() {
				new LonLat(-102.2053275f, 31.2663172f),
				new LonLat(-102.1511183f, 31.30069f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2001-1-1"),
			power_mw: 79.3f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7352170",
			name: "Woodward Wind 1",
			lon_lat: new LonLat(-102.4900581f, 30.8867443f),
			outline: new List<LonLat>() {
				new LonLat(-102.4907481f, 30.8841744f),
				new LonLat(-102.4426646f, 30.965115f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2001-1-1"),
			power_mw: 82.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7352171",
			name: "Woodward Wind 2",
			lon_lat: new LonLat(-102.4121504f, 30.97746f),
			outline: new List<LonLat>() {
				new LonLat(-102.4462713f, 30.9413157f),
				new LonLat(-102.3645191f, 30.9803073f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2001-1-1"),
			power_mw: 77.2f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7353890",
			name: "Sherbino Mesa Wind 2",
			lon_lat: new LonLat(-102.4378726f, 30.7752591f),
			outline: new List<LonLat>() {
				new LonLat(-102.5623512f, 30.7533247f),
				new LonLat(-102.4378726f, 30.7978429f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2011-1-1"),
			power_mw: 145f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7353891",
			name: "Sherbino Mesa Wind 1",
			lon_lat: new LonLat(-102.3955116f, 30.8134822f),
			outline: new List<LonLat>() {
				new LonLat(-102.3960253f, 30.8016818f),
				new LonLat(-102.3234844f, 30.8366341f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2008-1-1"),
			power_mw: 150f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7376955",
			name: "Goldthwaite Wind Energy Center",
			lon_lat: new LonLat(-98.4924088f, 31.404402f),
			outline: new List<LonLat>() {
				new LonLat(-98.5570655f, 31.3281674f),
				new LonLat(-98.433301f, 31.4411346f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2014-1-1"),
			power_mw: 148.6f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7379060",
			name: "Logan's Gap Wind",
			lon_lat: new LonLat(-98.6757164f, 31.8005173f),
			outline: new List<LonLat>() {
				new LonLat(-98.7775022f, 31.7275763f),
				new LonLat(-98.6338947f, 31.8976423f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2015-1-1"),
			power_mw: 200.1f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7380412",
			name: "Cotton Plains Wind",
			lon_lat: new LonLat(-101.2317556f, 34.0251077f),
			outline: new List<LonLat>() {
				new LonLat(-101.2677321f, 33.9508895f),
				new LonLat(-101.1869413f, 34.0611262f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2017-1-1"),
			power_mw: 50.4f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7380413",
			name: "Old Settler Wind",
			lon_lat: new LonLat(-101.1555809f, 34.0340258f),
			outline: new List<LonLat>() {
				new LonLat(-101.2453115f, 33.9952609f),
				new LonLat(-101.0661775f, 34.0611262f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2017-1-1"),
			power_mw: 50.4f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7429952",
			name: "Baffin Wind",
			lon_lat: new LonLat(-97.6776503f, 27.1345792f),
			outline: new List<LonLat>() {
				new LonLat(-97.679902f, 27.1304627f),
				new LonLat(-97.4813548f, 27.2151167f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2015-1-1"),
			power_mw: 202f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7429953",
			name: "PeÃ±ascal Wind Power 1",
			lon_lat: new LonLat(-97.5407813f, 27.0727018f),
			outline: new List<LonLat>() {
				new LonLat(-97.5509804f, 27.0147242f),
				new LonLat(-97.4656525f, 27.1458082f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 201.6f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7429954",
			name: "PeÃ±ascal Wind Power 2",
			lon_lat: new LonLat(-97.6144283f, 27.09824f),
			outline: new List<LonLat>() {
				new LonLat(-97.6409029f, 27.0681147f),
				new LonLat(-97.4735989f, 27.177676f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 201.6f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7429955",
			name: "Texas Gulf Wind",
			lon_lat: new LonLat(-97.5859183f, 26.9271331f),
			outline: new List<LonLat>() {
				new LonLat(-97.6159425f, 26.9088788f),
				new LonLat(-97.5115587f, 27.0159037f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2009-1-1"),
			power_mw: 283.2f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7433170",
			name: "Magic Valley Wind 1",
			lon_lat: new LonLat(-97.7291139f, 26.4706871f),
			outline: new List<LonLat>() {
				new LonLat(-97.7503851f, 26.4162781f),
				new LonLat(-97.6010089f, 26.5005194f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2012-1-1"),
			power_mw: 201.6f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7433171",
			name: "Los Vientos Wind I",
			lon_lat: new LonLat(-97.6229994f, 26.3238985f),
			outline: new List<LonLat>() {
				new LonLat(-97.6461148f, 26.2866553f),
				new LonLat(-97.5494836f, 26.3610535f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2013-1-1"),
			power_mw: 200.1f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7433172",
			name: "Los Vientos Wind II",
			lon_lat: new LonLat(-97.68334f, 26.34038f),
			outline: new List<LonLat>() {
				new LonLat(-97.7553199f, 26.328738f),
				new LonLat(-97.6117547f, 26.3777604f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2013-1-1"),
			power_mw: 201.6f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7434565",
			name: "San Roman Wind Farm",
			lon_lat: new LonLat(-97.3618135f, 26.1133776f),
			outline: new List<LonLat>() {
				new LonLat(-97.3940107f, 26.0927513f),
				new LonLat(-97.3326874f, 26.1289591f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2017-1-1"),
			power_mw: 95.2f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7434566",
			name: "Cameron Wind Farm",
			lon_lat: new LonLat(-97.4561548f, 26.1855622f),
			outline: new List<LonLat>() {
				new LonLat(-97.5356743f, 26.1397649f),
				new LonLat(-97.4474886f, 26.2562468f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2016-1-1"),
			power_mw: 165f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7434585",
			name: "Harbor Wind",
			lon_lat: new LonLat(-97.4402386f, 27.8320756f),
			outline: new List<LonLat>() {
				new LonLat(-97.4445261f, 27.8305255f),
				new LonLat(-97.4338228f, 27.8321503f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2012-1-1"),
			power_mw: 9f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7434827",
			name: "Los Mirasoles Wind Farm",
			lon_lat: new LonLat(-98.4995931f, 26.4775958f),
			outline: new List<LonLat>() {
				new LonLat(-98.5015833f, 26.4360941f),
				new LonLat(-98.3216219f, 26.5534274f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2016-1-1"),
			power_mw: 250f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7495745",
			name: "Hackberry Wind Farm",
			lon_lat: new LonLat(-99.4909946f, 32.7694117f),
			outline: new List<LonLat>() {
				new LonLat(-99.5046771f, 32.7649325f),
				new LonLat(-99.404357f, 32.8069078f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2008-1-1"),
			power_mw: 166f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7495746",
			name: "Lone Star Mesquite Wind",
			lon_lat: new LonLat(-99.5036148f, 32.614733f),
			outline: new List<LonLat>() {
				new LonLat(-99.5683097f, 32.5626704f),
				new LonLat(-99.443642f, 32.6568184f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2007-1-1"),
			power_mw: 200f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7495747",
			name: "Lone Star Post Oak Wind",
			lon_lat: new LonLat(-99.4784501f, 32.5474284f),
			outline: new List<LonLat>() {
				new LonLat(-99.5601671f, 32.4823625f),
				new LonLat(-99.4542189f, 32.5738899f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2007-1-1"),
			power_mw: 200f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7514458",
			name: "Green Pastures Wind",
			lon_lat: new LonLat(-99.4199154f, 33.6488237f),
			outline: new List<LonLat>() {
				new LonLat(-99.4601887f, 33.6202167f),
				new LonLat(-99.3349417f, 33.67541f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2015-1-1"),
			power_mw: 150f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7514459",
			name: "Horse Creek Wind",
			lon_lat: new LonLat(-99.5117354f, 33.3056762f),
			outline: new List<LonLat>() {
				new LonLat(-99.6173608f, 33.30445f),
				new LonLat(-99.4756973f, 33.4294312f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 230f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7521647",
			name: "Mozart Wind Farm",
			lon_lat: new LonLat(-100.5424936f, 33.0136432f),
			outline: new List<LonLat>() {
				new LonLat(-100.5574107f, 32.969549f),
				new LonLat(-100.509088f, 33.0136432f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2012-1-1"),
			power_mw: 30f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7608019",
			name: "Silver Star Wind Farm",
			lon_lat: new LonLat(-98.4761729f, 32.3387632f),
			outline: new List<LonLat>() {
				new LonLat(-98.5024819f, 32.3164848f),
				new LonLat(-98.4305946f, 32.3458029f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2008-1-1"),
			power_mw: 60f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_SolarFarm(
			id: "8207862",
			name: "Lamesa Solar",
			lon_lat: new LonLat(-101.9383246f, 32.7230319f),
			outline: new List<LonLat>() {
				new LonLat(-101.9383246f, 32.7117621f),
				new LonLat(-101.9126076f, 32.7324817f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2017-4-1"),
			power_mw: 102f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "8210948",
			name: "Fluvanna Wind 1",
			lon_lat: new LonLat(-101.1150473f, 32.9310386f),
			outline: new List<LonLat>() {
				new LonLat(-101.1725593f, 32.8443006f),
				new LonLat(-101.0512f, 32.9317005f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2017-1-1"),
			power_mw: 155.4f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "8210949",
			name: "Dermott Wind",
			lon_lat: new LonLat(-101.0179836f, 32.8932186f),
			outline: new List<LonLat>() {
				new LonLat(-101.0489738f, 32.8491725f),
				new LonLat(-100.9292722f, 32.9416729f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2017-1-1"),
			power_mw: 253f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "8211848",
			name: "Chapman Ranch Wind",
			lon_lat: new LonLat(-97.5702399f, 27.6053048f),
			outline: new List<LonLat>() {
				new LonLat(-97.5836992f, 27.5626745f),
				new LonLat(-97.457327f, 27.64722f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2017-1-1"),
			power_mw: 250f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "8216745",
			name: "Bruenning's Breeze Wind Farm",
			lon_lat: new LonLat(-97.6422036f, 26.5318978f),
			outline: new List<LonLat>() {
				new LonLat(-97.7588368f, 26.4981526f),
				new LonLat(-97.6017022f, 26.539951f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2012-1-1"),
			power_mw: 228f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "8227194",
			name: "Vertigo Wind",
			lon_lat: new LonLat(-99.535481f, 33.6755105f),
			outline: new List<LonLat>() {
				new LonLat(-99.5758389f, 33.6333423f),
				new LonLat(-99.4210232f, 33.6783117f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2016-1-1"),
			power_mw: 150f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "8760035",
			name: "Rattlesnake Wind",
			lon_lat: new LonLat(-99.5484227f, 31.2435951f),
			outline: new List<LonLat>() {
				new LonLat(-99.5871511f, 31.2252843f),
				new LonLat(-99.4629353f, 31.2682301f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2018-1-1"),
			power_mw: 160f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "8774936",
			name: "Rocksprings Wind",
			lon_lat: new LonLat(-100.7223022f, 29.7882867f),
			outline: new List<LonLat>() {
				new LonLat(-100.8135188f, 29.7295298f),
				new LonLat(-100.7206285f, 29.806954f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2017-1-1"),
			power_mw: 150f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "8792825",
			name: "Flat Top Wind",
			lon_lat: new LonLat(-98.5999775f, 31.6875135f),
			outline: new List<LonLat>() {
				new LonLat(-98.6168861f, 31.5385936f),
				new LonLat(-98.5091472f, 31.727793f)
			},
			construction_start_date: default,
			generation_start_date: DateTime.Parse("2018-1-1"),
			power_mw: 200f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_ThermalCombustion(
			id: "463732504",
			name: "Blackhawk Station",
			lon_lat: new LonLat(-101.3627395f, 35.6936946f),
			outline: new List<LonLat>() {
				new LonLat(-101.3627395f, 35.6936946f),
				new LonLat(-101.3584426f, 35.6982995f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 230f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "463732741",
			name: "Harrington Generating Station",
			lon_lat: new LonLat(-101.7597956f, 35.2944573f),
			outline: new List<LonLat>() {
				new LonLat(-101.7598036f, 35.2944529f),
				new LonLat(-101.7420447f, 35.3090185f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 1018f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "463732742",
			name: "Nichols Generating Station",
			lon_lat: new LonLat(-101.7597956f, 35.2944573f),
			outline: new List<LonLat>() {
				new LonLat(-101.7598546f, 35.2797237f),
				new LonLat(-101.7421359f, 35.2944573f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 457f,
			operator_name: "undefined",
			mdollar_capex: default,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas()
			}
		),
		new PowerPlantData_WindFarm(
			id: "6803879",
			name: "Spinning Spur 3 Wind",
			lon_lat: new LonLat(-102.6749718f, 35.277428f),
			outline: new List<LonLat>() {
				new LonLat(-102.7794479f, 35.268359f),
				new LonLat(-102.6749718f, 35.3396573f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 194f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6803880",
			name: "Spinning Spur 2 Wind",
			lon_lat: new LonLat(-102.5659814f, 35.289706f),
			outline: new List<LonLat>() {
				new LonLat(-102.6730565f, 35.2797089f),
				new LonLat(-102.5316692f, 35.336974f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 161f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6803881",
			name: "Spinning Spur 1 Wind",
			lon_lat: new LonLat(-102.5105904f, 35.3345431f),
			outline: new List<LonLat>() {
				new LonLat(-102.5439336f, 35.2692069f),
				new LonLat(-102.3938915f, 35.3390942f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 161f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6804128",
			name: "Wildorado Wind 2",
			lon_lat: new LonLat(-102.2496163f, 35.2360825f),
			outline: new List<LonLat>() {
				new LonLat(-102.3166089f, 35.2265878f),
				new LonLat(-102.1573497f, 35.2822417f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 78.2f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6804129",
			name: "Wildorado Wind 1",
			lon_lat: new LonLat(-102.2909838f, 35.304904f),
			outline: new List<LonLat>() {
				new LonLat(-102.3711955f, 35.2550376f),
				new LonLat(-102.2423778f, 35.3113957f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 161f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6848588",
			name: "High Plains Wind Project",
			lon_lat: new LonLat(-101.532998f, 35.3482672f),
			outline: new List<LonLat>() {
				new LonLat(-101.5401116f, 35.3399805f),
				new LonLat(-101.5328927f, 35.3503727f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 10f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6848589",
			name: "Pantex Renewable Energy Project",
			lon_lat: new LonLat(-101.5253251f, 35.3227453f),
			outline: new List<LonLat>() {
				new LonLat(-101.5395489f, 35.3227103f),
				new LonLat(-101.5253251f, 35.3227453f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 11.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6848590",
			name: "White Deer Wind",
			lon_lat: new LonLat(-101.2395619f, 35.4834888f),
			outline: new List<LonLat>() {
				new LonLat(-101.2532984f, 35.4487249f),
				new LonLat(-101.189202f, 35.4934096f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 80f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6848591",
			name: "Majestic 1 Wind Energy Center",
			lon_lat: new LonLat(-101.5325788f, 35.3765354f),
			outline: new List<LonLat>() {
				new LonLat(-101.547057f, 35.3669367f),
				new LonLat(-101.5070301f, 35.4452663f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 79.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6848592",
			name: "Majestic 2 Wind Energy Center",
			lon_lat: new LonLat(-101.5861946f, 35.3860212f),
			outline: new List<LonLat>() {
				new LonLat(-101.6209694f, 35.3540012f),
				new LonLat(-101.5383014f, 35.4098946f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 79.6f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6851368",
			name: "Grandview Wind Farm 1",
			lon_lat: new LonLat(-101.1679097f, 35.2573671f),
			outline: new List<LonLat>() {
				new LonLat(-101.2948264f, 35.2104043f),
				new LonLat(-101.0871564f, 35.3069513f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 211f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6851369",
			name: "Panhandle Wind 2",
			lon_lat: new LonLat(-101.349346f, 35.4397763f),
			outline: new List<LonLat>() {
				new LonLat(-101.4524206f, 35.4121433f),
				new LonLat(-101.3093918f, 35.4559959f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 181.7f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6851370",
			name: "Panhandle Wind 1",
			lon_lat: new LonLat(-101.2373825f, 35.3977985f),
			outline: new List<LonLat>() {
				new LonLat(-101.3089787f, 35.3777379f),
				new LonLat(-101.1334617f, 35.456186f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 218.3f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861475",
			name: "Little Pringle Wind Farm",
			lon_lat: new LonLat(-101.5019367f, 36.5425288f),
			outline: new List<LonLat>() {
				new LonLat(-101.5078054f, 36.535594f),
				new LonLat(-101.4734156f, 36.5589334f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 20f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861476",
			name: "Novus 1 Wind Farm",
			lon_lat: new LonLat(-101.3737593f, 36.5249426f),
			outline: new List<LonLat>() {
				new LonLat(-101.3937168f, 36.5020676f),
				new LonLat(-101.340921f, 36.5427594f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 80f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861477",
			name: "Noble Great Plains Windpark",
			lon_lat: new LonLat(-101.3965611f, 36.4741355f),
			outline: new List<LonLat>() {
				new LonLat(-101.3988796f, 36.4386634f),
				new LonLat(-101.3303726f, 36.4991922f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 114f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861478",
			name: "Balko Wind",
			lon_lat: new LonLat(-100.9266607f, 36.523538f),
			outline: new List<LonLat>() {
				new LonLat(-100.9518842f, 36.5021012f),
				new LonLat(-100.7311116f, 36.5561044f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 299.7f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861479",
			name: "Palo Duro Wind Energy Center",
			lon_lat: new LonLat(-100.9967307f, 36.4234192f),
			outline: new List<LonLat>() {
				new LonLat(-101.1273221f, 36.3323744f),
				new LonLat(-100.9501354f, 36.4966268f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 249.9f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861481",
			name: "Frisco Wind Farm",
			lon_lat: new LonLat(-101.5989692f, 36.4984911f),
			outline: new List<LonLat>() {
				new LonLat(-101.6214944f, 36.4958908f),
				new LonLat(-101.5862568f, 36.4985213f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 10f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861482",
			name: "Exelon Wind 3",
			lon_lat: new LonLat(-101.3498199f, 36.4418017f),
			outline: new List<LonLat>() {
				new LonLat(-101.3498199f, 36.4418017f),
				new LonLat(-101.3299065f, 36.44308f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 10f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861483",
			name: "Exelon Wind 5",
			lon_lat: new LonLat(-101.8742325f, 36.4455168f),
			outline: new List<LonLat>() {
				new LonLat(-101.8763606f, 36.4454695f),
				new LonLat(-101.8609434f, 36.4455168f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 10f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861484",
			name: "Exelon Wind 6",
			lon_lat: new LonLat(-101.818175f, 36.4669638f),
			outline: new List<LonLat>() {
				new LonLat(-101.8223603f, 36.4668929f),
				new LonLat(-101.8059972f, 36.4671057f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 10f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861485",
			name: "Exelon Wind 4",
			lon_lat: new LonLat(-101.3869502f, 36.3728084f),
			outline: new List<LonLat>() {
				new LonLat(-101.3880126f, 36.3726921f),
				new LonLat(-101.3582128f, 36.4146826f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 79.8f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861486",
			name: "Exelon Wind 2",
			lon_lat: new LonLat(-101.4555661f, 36.488701f),
			outline: new List<LonLat>() {
				new LonLat(-101.4598349f, 36.4886812f),
				new LonLat(-101.442834f, 36.4887209f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 10f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861487",
			name: "Exelon Wind 1",
			lon_lat: new LonLat(-101.4835436f, 36.4983559f),
			outline: new List<LonLat>() {
				new LonLat(-101.49613f, 36.4983489f),
				new LonLat(-101.478571f, 36.4983754f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 10f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861488",
			name: "Exelon Wind 7",
			lon_lat: new LonLat(-101.8997717f, 36.0268792f),
			outline: new List<LonLat>() {
				new LonLat(-101.8997717f, 36.0252004f),
				new LonLat(-101.8833208f, 36.029298f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 10f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861489",
			name: "Exelon Wind 8",
			lon_lat: new LonLat(-101.8496674f, 35.9955083f),
			outline: new List<LonLat>() {
				new LonLat(-101.8636938f, 35.9954905f),
				new LonLat(-101.8475753f, 36.0075539f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 10f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861490",
			name: "Exelon Wind 9",
			lon_lat: new LonLat(-101.8054239f, 35.9614027f),
			outline: new List<LonLat>() {
				new LonLat(-101.8097134f, 35.9613308f),
				new LonLat(-101.794908f, 35.9614111f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 10f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861491",
			name: "Valero Sunray Wind Farm",
			lon_lat: new LonLat(-101.8824545f, 35.9252438f),
			outline: new List<LonLat>() {
				new LonLat(-101.9257484f, 35.9239785f),
				new LonLat(-101.8762071f, 35.9653647f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 49.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861492",
			name: "Exelon Wind 11",
			lon_lat: new LonLat(-101.986855f, 35.8168631f),
			outline: new List<LonLat>() {
				new LonLat(-101.9889816f, 35.8114874f),
				new LonLat(-101.9752618f, 35.8169416f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 10f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6861493",
			name: "Exelon Wind 10",
			lon_lat: new LonLat(-101.9987817f, 35.8082092f),
			outline: new List<LonLat>() {
				new LonLat(-101.9989082f, 35.8081778f),
				new LonLat(-101.9899517f, 35.8137031f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 10f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "6867062",
			name: "Miami Wind",
			lon_lat: new LonLat(-100.5422402f, 35.6275956f),
			outline: new List<LonLat>() {
				new LonLat(-100.6320299f, 35.6198119f),
				new LonLat(-100.3997372f, 35.7149088f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 288.6f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7204165",
			name: "Route 66 Wind Project",
			lon_lat: new LonLat(-101.465511f, 35.2136029f),
			outline: new List<LonLat>() {
				new LonLat(-101.5408078f, 35.1548161f),
				new LonLat(-101.3682283f, 35.2170032f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 150f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7309636",
			name: "Mariah del Norte Wind",
			lon_lat: new LonLat(-102.6330596f, 34.7059697f),
			outline: new List<LonLat>() {
				new LonLat(-102.8113246f, 34.685748f),
				new LonLat(-102.6054919f, 34.7468844f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 230.4f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7309710",
			name: "Jumbo Road Wind",
			lon_lat: new LonLat(-102.3040599f, 34.6911287f),
			outline: new List<LonLat>() {
				new LonLat(-102.3337466f, 34.689779f),
				new LonLat(-102.1761507f, 34.7440369f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 299.7f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7309711",
			name: "Hereford Wind",
			lon_lat: new LonLat(-102.3431128f, 34.7906778f),
			outline: new List<LonLat>() {
				new LonLat(-102.3602253f, 34.7476955f),
				new LonLat(-102.1742785f, 34.8051353f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 200f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7311044",
			name: "Colbeck's Corner Wind Farm",
			lon_lat: new LonLat(-101.1773551f, 35.3061994f),
			outline: new List<LonLat>() {
				new LonLat(-101.194202f, 35.2959023f),
				new LonLat(-101.0276127f, 35.3558762f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 200.5f,
			operator_name: "undefined",
			mdollar_capex: default
		),
		new PowerPlantData_WindFarm(
			id: "7311045",
			name: "Salt Fork Wind",
			lon_lat: new LonLat(-100.9321582f, 35.1570825f),
			outline: new List<LonLat>() {
				new LonLat(-100.9728688f, 35.143867f),
				new LonLat(-100.8754832f, 35.2078282f)
			},
			construction_start_date: default,
			generation_start_date: default,
			power_mw: 174f,
			operator_name: "undefined",
			mdollar_capex: default
		)
	};
}
