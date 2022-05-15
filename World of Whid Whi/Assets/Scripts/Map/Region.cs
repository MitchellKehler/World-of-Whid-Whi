using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    public List<Zone> Zones;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public enum RegionList
    {
        BrightPatch
    }
}

public class Zone
{
    public List<EncounterCreatureGroup> IncludedCreatures;
    public List<EncounterCreatureGroup> ExcludedCreatures;
    public List<EncounterCreatureGroup> ModifiedRatesCreatures;
}