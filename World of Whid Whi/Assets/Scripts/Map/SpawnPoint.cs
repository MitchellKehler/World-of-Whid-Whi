using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

/// <summary>
/// Needs to handle
/// 
/// respawning a player if they die (all their creatures die util we add player fighting)
/// showing a healing button if a player or any of their creatures are hurt
/// healing all a players creatures when the healing button is clicked
/// 
/// NOTE: in the future we may want the player to need to be close enough to the healing button for it to be interactable
///   - eventually we may want interactions with the spawn point like healing creatures and reviving to cost the same sole points that are used for capturing new creatures and then provide other ways to heal like paying currency to a doctor or inn.
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    // Eventually we will want to save the players spawn point in the database and allow it to be changed so we will need an ID
    // We need to connect other aspects of the character to the player as well like their name. Should do that at the same time.
    public string ID;

    public Vector3 SpawnPosition;
    
    public GameObject HealButton;

    public GameManager GM;

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (NetworkManager.Singleton.IsClient)
        {
            // should check with server if healing is needed.
            HealButton.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Got Heal Request");
        GM.HealAllCreatures_ServerRpc(NetworkManager.Singleton.LocalClientId);
        // later we will probably be checking for this in some other way
        HealButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
