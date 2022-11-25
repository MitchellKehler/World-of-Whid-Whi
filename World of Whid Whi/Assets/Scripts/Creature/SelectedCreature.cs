using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedCreature : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseUp()
    {
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Got click in SelectedCreature: " + gameObject.name);
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "parent: " + transform.parent.transform.parent.name);
        BattleCreatureClient battleCreatureClient = GetComponent<BattleCreatureClient>();
        if (battleCreatureClient == null)
        {
            battleCreatureClient = transform.parent.GetComponent<BattleCreatureClient>();
            if (battleCreatureClient == null)
            {
                battleCreatureClient = transform.parent.transform.parent.GetComponent<BattleCreatureClient>();
            }
        }
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "ID: " + battleCreatureClient.ID);
        // Should not use Find if possible
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EncounterCreatureClicked(battleCreatureClient.ID);
        //if (!GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Ability_Pick_Panel.activeSelf)
        //{
        //    //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EncounterCreatureClicked(CreatureNumber);
        //}
    }


    //public int GetCreatureNumber()
    //{
    //    return CreatureNumber;
    //}

    //public void SetCreatureNumber(int newCreatureNumber)
    //{
    //    CreatureNumber = newCreatureNumber;
    //}
}
