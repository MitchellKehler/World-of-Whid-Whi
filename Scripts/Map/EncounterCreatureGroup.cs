using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterCreatureGroup
{
    public string Name;
    public Rarity Rarity;
    public List<EncounterCreature> Creatures;

    public EncounterCreatureGroup(string MyName, Rarity MyRarity, List<EncounterCreature> NewCreatures)
    {
        Name = MyName;
        Creatures = NewCreatures;
        Rarity = MyRarity;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum Rarity
{
    VeryCommon,
    Common,
    UnCommon,
    Rare,
    VeryRare,
    ImpossiblyRare
}

public class EncounterCreature
{
    public string CreatureName;
    public int Min;
    public int Max;
    public Dictionary<PowerUpStat, HighLow> StatRangeList;

    public EncounterCreature (string name, int min, int max, Dictionary<PowerUpStat, HighLow> statRangeList)
    {
        CreatureName = name;
        Min = min;
        Max = max;
        StatRangeList = statRangeList;
    }
}

public class HighLow
{
    public int High;
    public int Low;

    public HighLow(int high, int low)
    {
        High = high;
        Low = low;
    }
}
