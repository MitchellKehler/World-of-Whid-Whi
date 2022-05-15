using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Target_Script : MonoBehaviour
{
    int TargetNumber = -1;
    public Color Color;
    bool ClickUI;

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
        BattleCreatureClient battleCreatureClient = GetComponent<BattleCreatureClient>();
        if (GetComponent<BattleCreatureClient>() == null)
        {
            battleCreatureClient = transform.parent.transform.parent.GetComponent<BattleCreatureClient>();
        }
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().TargetCreatureClicked(NetworkManager.Singleton.LocalClientId, battleCreatureClient.ID);


        //if (!GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Ability_Pick_Panel.activeSelf)
        //{
        //    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().TargetCreatureClicked(NetworkManager.Singleton.LocalClientId, TargetNumber);
        //}
    }

    //public int GetTargetNumber()
    //{
    //    return TargetNumber;
    //}

    //public void SetCreatureNumber(int newTargetNumber)
    //{
    //    TargetNumber = newTargetNumber;
    //}

}
