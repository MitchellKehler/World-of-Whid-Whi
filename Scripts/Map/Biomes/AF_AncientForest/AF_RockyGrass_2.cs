using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AF_RockyGrass_2 : EncounterTerrain
{
    public AF_RockyGrass_2()
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

        CreatureList.Add(new EncounterCreature("GiantRat", 1, 3, StatRangeList));
        TerrainCreatures_VeryCommon.Add(new EncounterCreatureGroup("Small Group of Giant Rats", Rarity.Common, CreatureList));

        customStatRangeList[PowerUpStat.XP] = new HighLow(0, 70);
        CreatureList = new List<EncounterCreature>();
        CreatureList.Add(new EncounterCreature("Wasp", 4, 8, customStatRangeList));
        TerrainCreatures_VeryCommon.Add(new EncounterCreatureGroup("Small Group of Giant Rats", Rarity.Common, CreatureList));

        customStatRangeList[PowerUpStat.XP] = new HighLow(0, 50);
        CreatureList = new List<EncounterCreature>();
        CreatureList.Add(new EncounterCreature("Wasp", 6, 12, customStatRangeList));
        TerrainCreatures_VeryCommon.Add(new EncounterCreatureGroup("Small Group of Giant Rats", Rarity.Common, CreatureList));

        //CreatureList = new List<EncounterCreature>();
        //CreatureList.Add(new EncounterCreature("Wasp", 1, 1, StatRangeList));
        //TerrainCreatures_Common.Add(new EncounterCreatureGroup("Loan Cocka Doodle Devil", Rarity.Common, CreatureList));
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
