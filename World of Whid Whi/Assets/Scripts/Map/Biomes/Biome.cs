using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome : MonoBehaviour
{
    public BiomeList MyBiome;
    public List<EncounterCreatureGroup> BiomeDefaultCreatures;
    public List<EncounterTerrain> EncounterTerrains;

    public Biome()
    {
        BiomeDefaultCreatures = new List<EncounterCreatureGroup>();
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

public enum BiomeList
{
    FairyWood,
    ShadowWood,
    DarkWood,
    FairyForest,
    DarkForest,
    Woods,
    Forest,
    Plains
}

