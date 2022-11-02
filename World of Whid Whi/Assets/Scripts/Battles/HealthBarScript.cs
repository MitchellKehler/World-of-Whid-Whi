using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.FantasyMonsters.Scripts;

public class HealthBarScript : MonoBehaviour
{
    const float FAST_INCREMENT = 0.03f;
    const float SLOW_INCREMENT = 0.005f;

    float Timer = 0.0f;
    float Time_Between_Updates = 0.01f;

    public float CurrentHP;
    float PreviousHP;
    public float CurrentInitiative;
    float PreviousInitiative;
    GameObject CurrentHPSprite;
    GameObject PreviousHPSprite;
    GameObject CurrentInitiativeSprite;
    GameObject PreviousInitiativeSprite;

    private void Awake()
    {
        //Debug.Log("Getting Sprites");
        SpriteRenderer[] MySprites = this.gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in MySprites)
        {
            //Debug.Log("Got a sprite = " + sprite.transform.name);
            if (sprite.transform.name.Equals("CurrentHealthSprite"))
            {
                CurrentHPSprite = sprite.transform.parent.gameObject;
            }
            else if (sprite.transform.name.Equals("PreviousHealthSprite"))
            {
                PreviousHPSprite = sprite.transform.parent.gameObject;
            }
            else if (sprite.transform.name.Equals("CurrentInitiativeSprite"))
            {
                CurrentInitiativeSprite = sprite.transform.parent.gameObject;
            }
            else if (sprite.transform.name.Equals("PreviousInitiativeSprite"))
            {
                PreviousInitiativeSprite = sprite.transform.parent.gameObject;
            }
        }
        CurrentHP = 1;
        PreviousHP = 1;
        CurrentInitiative = 1;
        PreviousInitiative = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= Time_Between_Updates) // a second has elapsed
        {
            // Server and Client
            Timer -= Time_Between_Updates;

            if (CurrentHPSprite.transform.localScale.x < CurrentHP)
            {
                if (CurrentHP - CurrentHPSprite.transform.localScale.x < SLOW_INCREMENT)
                {
                    CurrentHPSprite.transform.localScale = new Vector3(CurrentHP, 1, 1);
                }
                else
                {
                    CurrentHPSprite.transform.localScale = new Vector3(CurrentHPSprite.transform.localScale.x + SLOW_INCREMENT, 1, 1);
                }
            }
            else if (CurrentHPSprite.transform.localScale.x > CurrentHP)
            {
                if (CurrentHPSprite.transform.localScale.x - CurrentHP < FAST_INCREMENT)
                {
                    CurrentHPSprite.transform.localScale = new Vector3(CurrentHP, 1, 1);
                }
                else
                {
                    CurrentHPSprite.transform.localScale = new Vector3(CurrentHPSprite.transform.localScale.x - FAST_INCREMENT, 1, 1);
                }
            }

            if (PreviousHPSprite.transform.localScale.x < CurrentHP)
            {
                if (CurrentHP - PreviousHPSprite.transform.localScale.x < FAST_INCREMENT)
                {
                    PreviousHPSprite.transform.localScale = new Vector3(CurrentHP, 1, 1);
                }
                else
                {
                    PreviousHPSprite.transform.localScale = new Vector3(PreviousHPSprite.transform.localScale.x + FAST_INCREMENT, 1, 1);
                }
            }
            else if (PreviousHPSprite.transform.localScale.x > CurrentHP)
            {
                if (PreviousHPSprite.transform.localScale.x - CurrentHP < SLOW_INCREMENT)
                {
                    PreviousHPSprite.transform.localScale = new Vector3(CurrentHP, 1, 1);
                }
                else
                {
                    PreviousHPSprite.transform.localScale = new Vector3(PreviousHPSprite.transform.localScale.x - SLOW_INCREMENT, 1, 1);
                }
            }

            // need to do the same for initiative
            if (CurrentInitiativeSprite.transform.localScale.x < CurrentInitiative)
            {
                if (CurrentInitiative - CurrentInitiativeSprite.transform.localScale.x < SLOW_INCREMENT)
                {
                    CurrentInitiativeSprite.transform.localScale = new Vector3(CurrentInitiative, 1, 1);
                }
                else
                {
                    CurrentInitiativeSprite.transform.localScale = new Vector3(CurrentInitiativeSprite.transform.localScale.x + SLOW_INCREMENT, 1, 1);
                }
            }
            else if (CurrentInitiativeSprite.transform.localScale.x > CurrentInitiative)
            {
                if (CurrentInitiativeSprite.transform.localScale.x - CurrentInitiative < FAST_INCREMENT)
                {
                    CurrentInitiativeSprite.transform.localScale = new Vector3(CurrentInitiative, 1, 1);
                }
                else
                {
                    CurrentInitiativeSprite.transform.localScale = new Vector3(CurrentInitiativeSprite.transform.localScale.x - FAST_INCREMENT, 1, 1);
                }
            }

            if (PreviousInitiativeSprite.transform.localScale.x < CurrentInitiative)
            {
                if (CurrentInitiative - PreviousInitiativeSprite.transform.localScale.x < FAST_INCREMENT)
                {
                    PreviousInitiativeSprite.transform.localScale = new Vector3(CurrentInitiative, 1, 1);
                }
                else
                {
                    PreviousInitiativeSprite.transform.localScale = new Vector3(PreviousInitiativeSprite.transform.localScale.x + FAST_INCREMENT, 1, 1);
                }
            }
            else if (PreviousInitiativeSprite.transform.localScale.x > CurrentInitiative)
            {
                if (PreviousInitiativeSprite.transform.localScale.x - CurrentInitiative < SLOW_INCREMENT)
                {
                    PreviousInitiativeSprite.transform.localScale = new Vector3(CurrentInitiative, 1, 1);
                }
                else
                {
                    PreviousInitiativeSprite.transform.localScale = new Vector3(PreviousInitiativeSprite.transform.localScale.x - SLOW_INCREMENT, 1, 1);
                }
            }
        }
        if (CurrentHP == 0)
        {
            transform.parent.gameObject.GetComponentInChildren<Monster>().Die();
            Destroy(this.gameObject);
        }
    }

    public void SetHealthPercent(float health)
    {
        //health = health / 100;
        if (health >= 0 && health <= 1)
        {
            CurrentHP = health;
        } else if (health <= 0)
        {
            Debug.Log("Health is less then 0.");
            CurrentHP = 0;
        }
        else
        {
            Debug.Log("Error: Tried to set invalid health");
        }
    }

    public void SetInitiativePercent(float initiative)
    {
        //initiative = initiative / 100;
        if (initiative >= 0 && initiative <= 1)
        {
            CurrentInitiative = initiative;
        }
        else
        {
            Debug.Log("Error: Tried to set invalid health");
        }
    }

}
