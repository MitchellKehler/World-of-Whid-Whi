using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AF_ForestGrass_2 : EncounterTerrain
{
    public AF_ForestGrass_2()
    {
        MyBiome = BiomeList.FairyWood;
        TerrainCreatures_ImpossiblyRare = new List<EncounterCreatureGroup>();
        TerrainCreatures_VeryRare = new List<EncounterCreatureGroup>();
        TerrainCreatures_Rare = new List<EncounterCreatureGroup>();
        TerrainCreatures_UnCommon = new List<EncounterCreatureGroup>();
        TerrainCreatures_Common = new List<EncounterCreatureGroup>();
        TerrainCreatures_VeryCommon = new List<EncounterCreatureGroup>();

        StatRangeList = new Dictionary<PowerUpStat, HighLow>();
        Dictionary<PowerUpStat, HighLow> customStatRangeList = new Dictionary<PowerUpStat, HighLow>(StatRangeList);

        StatRangeList.Add(PowerUpStat.XP, new HighLow(10, 100));


        List<EncounterCreature> CreatureList = new List<EncounterCreature>();
        customStatRangeList = new Dictionary<PowerUpStat, HighLow>(StatRangeList);
        customStatRangeList[PowerUpStat.XP] = new HighLow(200, 300);
        CreatureList.Add(new EncounterCreature("Wasp", 12, 16, customStatRangeList));
        TerrainCreatures_VeryCommon.Add(new EncounterCreatureGroup("Wasp Swarm", Rarity.UnCommon, CreatureList));

        CreatureList = new List<EncounterCreature>();
        customStatRangeList = new Dictionary<PowerUpStat, HighLow>(StatRangeList);
        customStatRangeList[PowerUpStat.XP] = new HighLow(400, 500);
        CreatureList.Add(new EncounterCreature("GreyWolf", 2, 5, customStatRangeList));
        TerrainCreatures_VeryCommon.Add(new EncounterCreatureGroup("Several Wolves", Rarity.Common, CreatureList));


        CreatureList = new List<EncounterCreature>();
        customStatRangeList = new Dictionary<PowerUpStat, HighLow>(StatRangeList);
        customStatRangeList[PowerUpStat.XP] = new HighLow(400, 600);
        CreatureList.Add(new EncounterCreature("GreyWolf", 3, 6, customStatRangeList));
        customStatRangeList = new Dictionary<PowerUpStat, HighLow>(StatRangeList);
        customStatRangeList[PowerUpStat.XP] = new HighLow(600, 700);
        CreatureList.Add(new EncounterCreature("WhiteWolf", 1, 1, customStatRangeList));
        TerrainCreatures_VeryCommon.Add(new EncounterCreatureGroup("Small Wolf Pack", Rarity.UnCommon, CreatureList));

        CreatureList = new List<EncounterCreature>();
        customStatRangeList = new Dictionary<PowerUpStat, HighLow>(StatRangeList);
        customStatRangeList[PowerUpStat.XP] = new HighLow(1000, 1400);
        CreatureList.Add(new EncounterCreature("Bear", 1, 1, customStatRangeList));
        TerrainCreatures_VeryCommon.Add(new EncounterCreatureGroup("Bear", Rarity.Common, CreatureList));

    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
