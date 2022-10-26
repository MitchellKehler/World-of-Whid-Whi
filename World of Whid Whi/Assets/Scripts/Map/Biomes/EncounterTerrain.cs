using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using System;
using Random = UnityEngine.Random;

public class EncounterTerrain : NetworkBehaviour
{
    private GameManager GM;
    public BiomeList MyBiome;
    public float EncounterChance = 5;
    public List<EncounterCreatureGroup> TerrainCreatures_ImpossiblyRare;
    public List<EncounterCreatureGroup> TerrainCreatures_VeryRare;
    public List<EncounterCreatureGroup> TerrainCreatures_Rare;
    public List<EncounterCreatureGroup> TerrainCreatures_UnCommon;
    public List<EncounterCreatureGroup> TerrainCreatures_Common;
    public List<EncounterCreatureGroup> TerrainCreatures_VeryCommon;
    public Dictionary<PowerUpStat, HighLow> StatRangeList;

    // Start is called before the first frame update
    protected void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // each piece of grass has it's own EncounterTerrain script.
    //      The Zone can be set indiviually on the actual sprite in the map
    //      The Biome could be set on the sprite, in the prefab, or by the Map script
    //      The Terrain will be set on the prefab
    // When a EncounterTerrain is triggered we then have all the info needed right there to get the list of encounterable creatures from the DataBase.
    // First determine the rarity based on random chance + player skills
    // Then grab the list of possibilities from the database based on Biome, Terrain, Zone (May Be Used Differently), Time of Day / Night, and Rarity
    // Then determine the actual creature encountered based on random chance and player's skills.
    // If player LVL and Skills don't automatically avoid the battle then start the battle
    public void GotCollission(Collider2D collision)
    {
        Debug.Log("Got a collision1");
        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log("Got a collision2");
            GotEncounter(collision.GetComponent<NetworkObject>().OwnerClientId, this);
        }

    }

    // Should only be the server getting here
    public void GotEncounter(ulong clientId, EncounterTerrain encounterTerrain)
    {
        float p = Random.Range(0.0f, 100.0f);
        //Debug.Log("Client " + clientId + " got Encountered!");
        //Debug.Log("In Battle " + NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject.GetComponent<Player>().inBattle);
        if (!NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject.GetComponent<Player>().inBattle && p < encounterTerrain.EncounterChance)
        {
            NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject.GetComponent<Player>().inBattle = true;
            //Debug.Log("NetworkManager.Singleton.ConnectedClientsList.Count = " + NetworkManager.Singleton.ConnectedClientsList.Count);
            //NetworkManager.Singleton.ConnectedClientsList[(Convert.ToInt32(clientId)) - 2].PlayerObject.gameObject.GetComponent<Player_Movement_Android>().IsAllowedToMove = false;

            List<EncounterCreatureGroup> tempCreatureList;
            // TODO: Get Creature Name from DB (Maybe)

            // p is always between 0 and 5 at this point
            if (p < 0.0001f && encounterTerrain.TerrainCreatures_ImpossiblyRare.Count != 0) // find Impossibly Rare
            {
                tempCreatureList = encounterTerrain.TerrainCreatures_ImpossiblyRare;
            }
            else if (p < 0.001f && encounterTerrain.TerrainCreatures_VeryRare.Count != 0) // Very Rare
            {
                tempCreatureList = encounterTerrain.TerrainCreatures_VeryRare;
            }
            else if (p < 0.01f && encounterTerrain.TerrainCreatures_Rare.Count != 0) // Rare
            {
                tempCreatureList = encounterTerrain.TerrainCreatures_Rare;
            }
            else if (p < 0.1f && encounterTerrain.TerrainCreatures_UnCommon.Count != 0) // UnCommon
            {
                tempCreatureList = encounterTerrain.TerrainCreatures_UnCommon;
            }
            else if (p < 1.0f && encounterTerrain.TerrainCreatures_Common.Count != 0) // Common
            {
                tempCreatureList = encounterTerrain.TerrainCreatures_Common;
            }
            else // VeryCommon
            {
                tempCreatureList = encounterTerrain.TerrainCreatures_VeryCommon;
            }
            int creatureGroupIndex = Random.Range(0, tempCreatureList.Count);

            //TODO Add PowerUp increases.

            List<InitializedCreatureData> encounteredCreatures = new List<InitializedCreatureData>();
            for (int i = 0; i < tempCreatureList[creatureGroupIndex].Creatures.Count; i++)
            {
                encounteredCreatures.AddRange(GM.InitializeNewCreatures(tempCreatureList[creatureGroupIndex].Creatures[i]));
            }

            InitializedCreatureData[] enemies = encounteredCreatures.ToArray();

            Debug.Log("Sending StartClientEncounter");
            GM.StartClientEncounter(clientId, enemies, tempCreatureList[creatureGroupIndex].Name);
        }

    }


}

/*
 * 6:  0 - .001
 * 5: 0 - .01
 * 4: 0 - .1
 * 3: 0 - 1
 * 2: 0 - 3
 * 1: 0 - 5
 * 
 *     VeryCommon,
    Common,
    SemiRare,
    Rare,
    VeryRare,
    ImpossiblyRare
 */
