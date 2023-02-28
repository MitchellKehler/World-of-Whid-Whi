using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    /*
     * 
    */
    
    public const float TIME_BETWEEN_BATTLE_UPDATES = .1f;

    GameManager GM;
    public Battle battle;
    float Timer = 0.0f;
    private float BattleSecondsTimer = 0;
    public float TimeTillBattleUpdate = 0;
    private IEnumerator SetBeginBattleDelay;

    public void SetUpAndStart(GameManager GM, ulong Player1ID, ulong Player1Character, InitializedCreatureData[] teamOneCreatures, ulong Player2ID, ulong Player2Character, InitializedCreatureData[] teamTwoCreatures)
    {
        Debug.Log("Creating new Battle Manager");
        this.GM = GM;
        battle = new Battle(GM, this, Player1ID, Player1Character, teamOneCreatures, Player2ID, Player2Character, teamTwoCreatures);
        SetBeginBattleDelay = SetNextBattleUpdateTime(60);
        StartCoroutine(SetBeginBattleDelay);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkManager.Singleton.IsServer && TimeTillBattleUpdate > 0) ///// NetworkManager.Singleton.IsServer!!!!!!!!!!!
        {
            Timer += UnityEngine.Time.deltaTime;
            if (Timer >= TIME_BETWEEN_BATTLE_UPDATES)
            {
                // Server and Client
                Timer -= TIME_BETWEEN_BATTLE_UPDATES;
                BattleSecondsTimer += TIME_BETWEEN_BATTLE_UPDATES;
            }

            ///////////// a second has passed
            if (BattleSecondsTimer >= 1 && battle != null)
            {
                //Debug.Log("Second elapsed");
                TimeTillBattleUpdate -= BattleSecondsTimer;
                BattleSecondsTimer = 0;
                if (!NetworkManager.Singleton.ConnectedClients.Keys.Contains(battle.Player1))
                {
                    EndBattle(battle.Player2);
                }
                else if (battle.Player2 != GameManager.SERVERID && !NetworkManager.Singleton.ConnectedClients.Keys.Contains(battle.Player2))
                {
                    EndBattle(battle.Player1);
                }
                else if (battle.Stage == BattleStage.BattleStarting)
                {
                    //Debug.Log("Battle Stage BattleStarting");

                    if (NetworkManager.Singleton.ConnectedClients[battle.Player1].PlayerObject.gameObject.GetComponent<Player>().Battle_Go
                        && (battle.Player2 == GameManager.SERVERID || NetworkManager.Singleton.ConnectedClients[battle.Player2].PlayerObject.gameObject.GetComponent<Player>().Battle_Go))
                    {
                        NetworkManager.Singleton.ConnectedClients[battle.Player1].PlayerObject.gameObject.GetComponent<Player>().Battle_Go = false;
                        if (battle.Player2 != GameManager.SERVERID)
                            NetworkManager.Singleton.ConnectedClients[battle.Player2].PlayerObject.gameObject.GetComponent<Player>().Battle_Go = false;
                        Debug.Log("All Players skipped to start");
                        TimeTillBattleUpdate = 0;
                        StopAllCoroutines();
                        UpdateBattle();
                    }
                    else
                    {
                        UpdateBattleTimer();
                    }
                    //Debug.Log("Done Battle Stage BattleStarting");
                }
                else
                {
                    if (battle.CurrentCreature.Owner == battle.Player1 && NetworkManager.Singleton.ConnectedClients[battle.Player1].PlayerObject.gameObject.GetComponent<Player>().Battle_Go)
                    {
                        NetworkManager.Singleton.ConnectedClients[battle.Player1].PlayerObject.gameObject.GetComponent<Player>().Battle_Go = false;
                        Debug.Log("Player 1 skipped turn");
                        StopAllCoroutines();
                        UpdateBattle();
                    }
                    else if (battle.Player2 != GameManager.SERVERID && battle.CurrentCreature.Owner == battle.Player2 && NetworkManager.Singleton.ConnectedClients[battle.Player2].PlayerObject.gameObject.GetComponent<Player>().Battle_Go)
                    {
                        NetworkManager.Singleton.ConnectedClients[battle.Player2].PlayerObject.gameObject.GetComponent<Player>().Battle_Go = false;
                        Debug.Log("Player 2 skipped turn");
                        TimeTillBattleUpdate = 0;
                        StopAllCoroutines();
                        UpdateBattle();
                    }
                    else
                    {
                        UpdateBattleTimer();
                    }
                }
            } 
        }
    }

    public void UpdateBattleTimer()
    {
        bool IsAI;
        if (battle.CurrentCreature == null || battle.CurrentCreature.Owner == GameManager.SERVERID)
        { // if CurrentCreature is null then we are still in the BattleStarting Stage of the battle and it doesn't matter if it is an AI or a Player's turn.
            IsAI = true;
        }
        else
        {
            IsAI = false;
        }
        if ((!IsAI && battle.Stage == BattleStage.ChooseAbility) || battle.Stage == BattleStage.BattleStarting)
        {
            // Also not super efficient way of doing this
            ClientRpcParams clientRpcParams;
            if (battle.Player2 == GameManager.SERVERID)
            {
                clientRpcParams = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { battle.Player1 }
                    }
                };
            }
            else
            {
                clientRpcParams = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { battle.Player1, battle.Player2 }
                    }
                };
            }
            //Debug.Log("Setting Timer Button = Done? (" + TimeTillBattleUpdate.ToString() + ")");
            GM.UpdateBattleTimerClientRpc("Done? (" + TimeTillBattleUpdate.ToString() + ")", clientRpcParams);
        }
    }

    public IEnumerator SetNextBattleUpdateTime(float time)
    {
        Debug.Log("SetNextBattleUpdateTime time = " + time);
        TimeTillBattleUpdate = time;
        UpdateBattleTimer();
        yield return new WaitForSeconds(time);
        UpdateBattle();
    }


    void UpdateBattle()
    {
        StopCoroutine(SetBeginBattleDelay);
        Debug.Log("UpdateBattle");
        ClientRpcParams clientRpcParams;
        if (battle.Player2 == GameManager.SERVERID)
        {
            clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { battle.Player1 }
                }
            };
        }
        else
        {
            clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { battle.Player1, battle.Player2 }
                }
            };
        }
        GM.SetGoButtonClientRpc(false, "", clientRpcParams);
        Debug.Log("Calling NextAction");
        battle.NextAction();
    }


    public void EndBattle(ulong winner)
    {
        foreach (InitializedCreatureData creature in GM.charactersInGame[battle.Player1CharacterID].CurrentCreatureTeam)
        {
            BattleCreature battleCreature = battle.BattleCreatures.Find(battleCreature => battleCreature.ID == creature.battleCreatureID);
            if (battleCreature != null)
                creature.CurrentHP = battleCreature.Creature.CurrentHP;
            else creature.CurrentHP = 0;
            creature.battleCreatureID = -1;
        }
        GM.ExitBattle(battle, battle.Player1, winner);
        if (battle.Player2 != GameManager.SERVERID)
        {
            foreach (InitializedCreatureData creature in GM.charactersInGame[battle.Player2CharacterID].CurrentCreatureTeam)
            {
                BattleCreature battleCreature = battle.BattleCreatures.Find(battleCreature => battleCreature.ID == creature.battleCreatureID);
                if (battleCreature != null)
                    creature.CurrentHP = battleCreature.Creature.CurrentHP;
                else creature.CurrentHP = 0;
                creature.battleCreatureID = -1;
            }
            GM.ExitBattle(battle, battle.Player2, winner);
        }
        GM.Battles.Remove(this);
        Destroy(this);
    }
}
