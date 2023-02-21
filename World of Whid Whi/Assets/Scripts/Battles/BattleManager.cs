using MLAPI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    /*
     * use update to check for seconds counter changes and player disconnects
     * NextAction event
     * end battle method
     * player 1 and player 2
     * battle?
     * 
     * 
     * 
     * Need to add code to start battle imediatley if both players have readied up since I removed that code.
    */
    
    public const float TIME_BETWEEN_BATTLE_UPDATES = .1f;

    GameManager GM;
    float Timer = 0.0f;
    private float BattleSecondsTimer = 0;
    ulong Player1, Player2;

    public BattleManager(GameManager GM, ulong Player1, ulong Player2)
    {
        this.GM = GM;
        this.Player1 = Player1;
        this.Player2 = Player2;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkManager.Singleton.IsServer) ///// NetworkManager.Singleton.IsServer!!!!!!!!!!!
        {
            Timer += Time.deltaTime;
            if (Timer >= TIME_BETWEEN_BATTLE_UPDATES)
            {
                // Server and Client
                Timer -= TIME_BETWEEN_BATTLE_UPDATES;
                BattleSecondsTimer += TIME_BETWEEN_BATTLE_UPDATES;
            }

            if (BattleSecondsTimer >= TIME_BETWEEN_BATTLE_UPDATES)
            {
                if (!NetworkManager.Singleton.ConnectedClients.Keys.Contains(Player1))
                {
                    EndBattle(Player2);
                }
                else if (Player2 != GameManager.SERVERID && !NetworkManager.Singleton.ConnectedClients.Keys.Contains(Player2))
                {
                    EndBattle(Player1);
                }
            }
        }
    }



    public void EndBattle(ulong winner)
    {

    }
}
