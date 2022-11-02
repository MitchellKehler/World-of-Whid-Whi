using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.FantasyMonsters.Scripts;
using MLAPI;

/// <summary>
/// To DO
/// 
/// should increase the time to move in Game Manager but have this set Go when move is complete to make sure that all moves are finished but that the next thing happens
/// imediately after.
/// </summary>
public class BattleCreatureClient : MonoBehaviour
{
    public int ID;
    public Vector3 moveToTarget;
    public Vector3 Ancher;
    public float speed;
    public ulong owner;
    public InitializedCreature InitilizedCreature;
    public bool acting;
    public bool target;

    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToTarget != transform.position)
        {
            float step = speed * Time.deltaTime;
            //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Moving to : " + Vector3.MoveTowards(transform.position, moveToTarget, step));
            transform.position = Vector3.MoveTowards(transform.position, moveToTarget, step);
            if (moveToTarget == transform.position && InitilizedCreature.CurrentHP != 0)
            {
                //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "I have arived at : " + moveToTarget);
                this.gameObject.GetComponent<Monster>().SetState(MonsterState.Idle);
            }
            if (transform.position == Ancher)
            {
                Vector3 newScale = transform.localScale;
                newScale.x *= -1;
                transform.localScale = newScale;
            }
        }
    }

    public void Move(Vector3 target)
    {
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Got new move command, My name is : " + gameObject.name);
        this.gameObject.GetComponent<Monster>().SetState(MonsterState.Run);
        moveToTarget = target;
    }

    public void SetAncher(Vector3 position)
    {
        Ancher = position;
        moveToTarget = Ancher;
    }

    private void OnMouseUp()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Got click in BattleCreatureClient: " + gameObject.name);
        if (acting)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Is Acting");
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EncounterCreatureClicked(ID);
            //if (!GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Ability_Pick_Panel.activeSelf)
            //{
            //    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EncounterCreatureClicked(ID);
            //    //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EncounterCreatureClicked(CreatureNumber);
            //}
        }
        else if (target)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Is Trigger");
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().TargetCreatureClicked(NetworkManager.Singleton.LocalClientId, ID);
            //if (!GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Ability_Pick_Panel.activeSelf)
            //{
            //    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().TargetCreatureClicked(NetworkManager.Singleton.LocalClientId, ID);
            //}
        }
    }

    public void Die()
    {
        Invoke("DestroySelf", 2);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void ReturnToAncher()
    {
        if (transform.position != Ancher)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            this.gameObject.GetComponent<Monster>().SetState(MonsterState.Run);
        }

        moveToTarget = Ancher;
    }

    public void SetId(int iD)
    {
        ID = iD;
    }

    public int GetId()
    {
        return ID;
    }

    public void SetOwner(ulong newOwner)
    {
        owner = newOwner;
    }

    public ulong GetOwner()
    {
        return owner;
    }
}
