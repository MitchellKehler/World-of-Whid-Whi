using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AF_Reeds_2 : EncounterTerrain
{
 
    public AF_Reeds_2()
    {
        MyBiome = BiomeList.FairyWood;
        TerrainCreatures_ImpossiblyRare = new List<EncounterCreatureGroup>();
        TerrainCreatures_VeryRare = new List<EncounterCreatureGroup>();
        TerrainCreatures_Rare = new List<EncounterCreatureGroup>();
        TerrainCreatures_UnCommon = new List<EncounterCreatureGroup>();
        TerrainCreatures_Common = new List<EncounterCreatureGroup>();
        TerrainCreatures_VeryCommon = new List<EncounterCreatureGroup>();

        StatRangeList = new Dictionary<PowerUpStat, HighLow>();
        Dictionary<PowerUpStat, HighLow> customStatRangeList;

        StatRangeList.Add(PowerUpStat.XP, new HighLow(10, 100));


        List<EncounterCreature> CreatureList = new List<EncounterCreature>();
        CreatureList.Add(new EncounterCreature("Wasp", 1, 3, StatRangeList));
        TerrainCreatures_VeryCommon.Add(new EncounterCreatureGroup("Small Group of Giant Frogs", Rarity.Common, CreatureList));

        CreatureList = new List<EncounterCreature>();
        customStatRangeList = new Dictionary<PowerUpStat, HighLow>(StatRangeList);
        customStatRangeList[PowerUpStat.XP] = new HighLow(100, 160);
        CreatureList.Add(new EncounterCreature("Wasp", 3, 3, customStatRangeList));
        customStatRangeList = new Dictionary<PowerUpStat, HighLow>(StatRangeList);
        customStatRangeList[PowerUpStat.XP] = new HighLow(160, 160);
        CreatureList.Add(new EncounterCreature("Wasp", 1, 1, customStatRangeList));
        TerrainCreatures_Common.Add(new EncounterCreatureGroup("King Toad", Rarity.Common, CreatureList));
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
