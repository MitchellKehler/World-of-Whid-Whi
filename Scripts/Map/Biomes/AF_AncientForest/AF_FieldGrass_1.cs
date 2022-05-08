using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AF_FieldGrass_1 : EncounterTerrain
{
    public AF_FieldGrass_1()
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

        CreatureList = new List<EncounterCreature>();
        CreatureList.Add(new EncounterCreature("MooBeast", 1, 2, StatRangeList));
        TerrainCreatures_VeryCommon.Add(new EncounterCreatureGroup("A Few Cows", Rarity.Common, CreatureList));

        CreatureList = new List<EncounterCreature>();
        CreatureList.Add(new EncounterCreature("MooBeast", 3, 3, StatRangeList));
        customStatRangeList = new Dictionary<PowerUpStat, HighLow>(StatRangeList);
        customStatRangeList[PowerUpStat.XP] = new HighLow(900, 1200);
        CreatureList.Add(new EncounterCreature("GreaterMooBeast", 1, 1, customStatRangeList));
        TerrainCreatures_Common.Add(new EncounterCreatureGroup("Small Group of Cattle", Rarity.Common, CreatureList));

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
