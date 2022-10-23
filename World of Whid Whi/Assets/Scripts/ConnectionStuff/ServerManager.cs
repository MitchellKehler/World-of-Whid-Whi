using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;

public class ServerManager : NetworkBehaviour
{
    // Cameras
    //public Camera PlayerCamera;
    //public Camera BattleCamera;

    // Game Objects
    //public GameObject Player;
    public GameManager GM;
    public Dictionary<string, BaseCreature> AllCreatures;

    //// UI Elements
    //public GameObject Detail_Panel_Close_Button;

    //// UI - Battle Canvise
    //public GameObject EmptyCreature;
    //public GameObject BattleBoard_FriendlyPos_1;
    //public GameObject BattleBoard_FriendlyPos_2;
    //public GameObject BattleBoard_FriendlyPos_3;
    //public GameObject BattleBoard_FriendlyPos_4;
    //public GameObject BattleBoard_EnemyPos_1;
    //public GameObject BattleBoard_EnemyPos_2;
    //public GameObject BattleBoard_EnemyPos_3;
    //public GameObject BattleBoard_EnemyPos_4;
    //public InitializedCreature EncounterCreature1;
    //public InitializedCreature EncounterCreature2;
    //public InitializedCreature EncounterCreature3;
    //public InitializedCreature EncounterCreature4;
    //public InitializedCreature EncounterCreature5;
    //public InitializedCreature EncounterCreature6;
    //public InitializedCreature EncounterCreature7;
    //public InitializedCreature EncounterCreature8;

    //public GameObject Battle_Action_Panel;
    //public GameObject Battle_Friendly_Icon_Panel;
    //public GameObject Battle_Enemy_Icon_Panel;
    //public GameObject Friendly_Creature_1_Icon;
    //public GameObject Friendly_Creature_1_ShortName;
    //public GameObject Friendly_Creature_2_Icon;
    //public GameObject Friendly_Creature_2_ShortName;
    //public GameObject Friendly_Creature_3_Icon;
    //public GameObject Friendly_Creature_3_ShortName;
    //public GameObject Friendly_Creature_4_Icon;
    //public GameObject Friendly_Creature_4_ShortName;
    //public GameObject Enemy_Creature_1_Icon;
    //public GameObject Enemy_Creature_1_ShortName;
    //public GameObject Enemy_Creature_2_Icon;
    //public GameObject Enemy_Creature_2_ShortName;
    //public GameObject Enemy_Creature_3_Icon;
    //public GameObject Enemy_Creature_3_ShortName;
    //public GameObject Enemy_Creature_4_Icon;
    //public GameObject Enemy_Creature_4_ShortName;

    //// UI - Creature Details Canvise - Creature Details Main Panel
    //public GameObject Creature_Details_Background_Panel;
    //public GameObject Creature_Details_Name_Text;
    //public GameObject Creature_Details_HP_Text;
    //public GameObject Creature_Details_Types_Text;
    //public GameObject Creature_Details_PowerLvl_Text;
    //public GameObject Creature_Details_Rating_Text;
    //public GameObject Creature_Details_Lvl_Text;
    //public GameObject Creature_Details_Str_Text;
    //public GameObject Creature_Details_Agi_Text;
    //public GameObject Creature_Details_Mnd_Text;
    //public GameObject Creature_Details_Wil_Text;
    //public GameObject Creature_Details_Size_Text;
    //public GameObject Creature_Details_Int_Text;
    //public Dropdown Creature_Details_Attributes_Dropdown;
    //public Dropdown Creature_Details_Abilities_Dropdown;

    //// UI - Creature Details Canvise - Creature Details General Details Panel
    //public GameObject Creature_Details_Description_Panel;
    //public Image Creature_Details_Image;
    //public Text Creature_Details_Description;

    //// UI - Creature Details Canvise - Creature Details General Details Panel
    //public GameObject Creature_Details_Attributes_Panel;
    //public Text Creature_Details_Attribute_Description;

    //// UI - Creature Details Canvise - Creature Details General Details Panel
    //public GameObject Creature_Details_Abilities_Panel;

    //// UI - Player Options Canvise
    //public GameObject SettingsPanel;
    //public GameObject MapPanel;
    //public GameObject InventoryPanel;
    //public GameObject CreaturesPanel;
    //public GameObject DiaryPanel;

    //// Other Objects
    //public List<BaseCreature> AllCreatures;
    //InitializedCreature FriendlyCreature1;
    //InitializedCreature FriendlyCreature2;
    //InitializedCreature FriendlyCreature3;
    //InitializedCreature FriendlyCreature4;
    //InitializedCreature EnemyCreature1;
    //InitializedCreature EnemyCreature2;
    //InitializedCreature EnemyCreature3;
    //InitializedCreature EnemyCreature4;
    //public List<GameObject> BattleObjects;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game Started");

        //BattleObjects = new List<GameObject>();
        AllCreatures = InitializeCreatures.GetInitializedCreatures();
        //GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    //public void StartClientEncounter(ulong clientId, InitializedCreatureData[] PassedEnemyCreatures)
    //{
    //    Debug.Log("In StartClientEncounter");
    //    Debug.Log("clientId = " + clientId);
    //    Debug.Log("PassedEnemyCreatures[1] = " + PassedEnemyCreatures[1].Name);

    //    InitializedCreatureData[] BattleStartingCreatures = new InitializedCreatureData[8];
    //    BattleStartingCreatures[5] = PassedEnemyCreatures[1];

    //    // Add players creatures to InitializedCreatureData Array to bring it from 4 to 8

    //    //EncounterCreature5 = new InitializedCreature(PassedEnemyCreatures[5]);

    //    //string Enemy_1_BaseImagePath = "BattleCreatureImages/" + EnemyCreature1.Name + "/" + EnemyCreature1.Name;
    //    //Player.GetComponent<PlayerMovement_Fluid>().IsAllowedToMove = false;
    //    //PlayerCamera.enabled = false;
    //    //BattleCamera.enabled = true;
    //    //Battle_Action_Panel.SetActive(true);
    //    //Battle_Friendly_Icon_Panel.SetActive(true);
    //    //Battle_Enemy_Icon_Panel.SetActive(true);

    //    //// Get CreatureInfo From SQL

    //    //Enemy_Creature_1_Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>(Enemy_1_BaseImagePath + "_Icon");
    //    //Enemy_Creature_1_Icon.SetActive(true);
    //    //Enemy_Creature_1_ShortName.GetComponent<Text>().text = EncounterCreature5.ShortName;
    //    //Enemy_Creature_1_ShortName.SetActive(true);
    //    //GameObject EncounterCreature_1 = Instantiate(EmptyCreature, BattleBoard_EnemyPos_1.transform.position, Quaternion.identity) as GameObject;
    //    //EncounterCreature_1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(Enemy_1_BaseImagePath + "_Enemy");
    //    //BattleObjects.Add(EncounterCreature_1);

    //    //ClientRpcParams clientRpcParams = new ClientRpcParams
    //    //{
    //    //    Send = new ClientRpcSendParams
    //    //    {
    //    //        TargetClientIds = new ulong[] { clientId }
    //    //    }
    //    //};

    //    GM.StartBattleClientRpc(BattleStartingCreatures);

    //    // Later we need to deal with multiple enemies.

    //}


//[ClientRpc]
//public void SubmitMoveRequestClientRpc(InitializedCreature creature, ClientRpcParams rpcParams = default)
//{
//    Debug.Log("Client " + OwnerClientId + " is trying to move.");
//    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EnterBattle(creature);
//}

//public void Run()
//    {
//        // TODO should check if run was successful
//        // Selecting Run should put a flag that limits all player controlled creature's actions to quick and defensive actions.
//        // Once each of the player controlled creatures, and the player, have had a turn the result of the run attempt is calculated.
//        // The run may not be successful, this will be dependant on the speed of the player's creatures VS the speed of the enemy in
//        // combination with the temperment and anger level of the enemy. Generarlly at the very least a run attempt will reduce the 
//        // anger level of the enemy making the next run attempt more likely to succeed (especially against non aggressive enemies).

//        ExitBattle();
//    }

//    public void ExitBattle()
//    {
//        // TODO should destroy prviously created creatures

//        Player.GetComponent<PlayerMovement_Fluid>().IsAllowedToMove = true;
//        PlayerCamera.enabled = true;
//        BattleCamera.enabled = false;
//        Battle_Action_Panel.SetActive(false);
//        Battle_Friendly_Icon_Panel.SetActive(false);
//        Battle_Enemy_Icon_Panel.SetActive(false);
//        CloseCreatureDetails();

//        foreach (GameObject gameObject in BattleObjects)
//        {
//            Destroy(gameObject);

//        }
//    }

//    public void OpenInfoPanel(GameObject SelectedPanel)
//    {
//        if (Player != null)
//        {
//            Player.GetComponent<Player_Movement_Android>().IsAllowedToMove = false;
//        }
//        SelectedPanel.GetComponent<RectTransform>().SetAsLastSibling();
//        SelectedPanel.SetActive(true);
//        Detail_Panel_Close_Button.GetComponent<RectTransform>().SetAsLastSibling();
//        Detail_Panel_Close_Button.SetActive(true);
//        // TODO set text and other details
//    }

//    public void CloseInfoPanels()
//    {
//        SettingsPanel.SetActive(false);
//        MapPanel.SetActive(false);
//        InventoryPanel.SetActive(false);
//        CreaturesPanel.SetActive(false);
//        DiaryPanel.SetActive(false);
//        Detail_Panel_Close_Button.SetActive(false);
//        if (Player != null)
//        {
//            Player.GetComponent<Player_Movement_Android>().IsAllowedToMove = true;
//        }
//    }

//    public void DisplayEncounterCreatureDetails(int EncounterCreatureCount)
//    {
//        switch (EncounterCreatureCount)
//        {
//            case 1:
//                OpenCreatureDetails(EncounterCreature1);
//                break;
//            case 2:
//                OpenCreatureDetails(EncounterCreature2);
//                break;
//            case 3:
//                OpenCreatureDetails(EncounterCreature3);
//                break;
//            case 4:
//                OpenCreatureDetails(EncounterCreature4);
//                break;
//            case 5:
//                OpenCreatureDetails(EncounterCreature5);
//                break;
//            case 6:
//                OpenCreatureDetails(EncounterCreature6);
//                break;
//            case 7:
//                OpenCreatureDetails(EncounterCreature7);
//                break;
//            case 8:
//                OpenCreatureDetails(EncounterCreature8);
//                break;
//        }

//    }

//    public void OpenCreatureDetails(InitializedCreature SelectedCreature) // IdentifyCreature just sets which creature was clicked and should show it's details
//    {

//        int MaxHpForBattle = (int)Mathf.Floor(SelectedCreature.MaxHP * SelectedCreature.HPMultiplier); // need to move this
//        Creature_Details_Name_Text.GetComponent<Text>().text = SelectedCreature.Name;
//        Creature_Details_HP_Text.GetComponent<Text>().text = MaxHpForBattle + " / " + MaxHpForBattle;

//        Debug.Log("OpenCreatureDetails: Point 2");
//        Creature_Details_Types_Text.GetComponent<Text>().text = "";
//        //foreach (CreatureType type in EnemyCreature1.Types)
//        //{
//        //    Battle_Creature_Types_Text.GetComponent<Text>().text += EnemyCreature1.Name;
//        //}

//        Debug.Log("OpenCreatureDetails: Point 3");
//        Creature_Details_PowerLvl_Text.GetComponent<Text>().text = SelectedCreature.PowerLevel.ToString();
//        Creature_Details_Rating_Text.GetComponent<Text>().text = SelectedCreature.Rating.ToString();
//        Creature_Details_Lvl_Text.GetComponent<Text>().text = SelectedCreature.CurrentLvl.ToString();
//        Creature_Details_Str_Text.GetComponent<Text>().text = SelectedCreature.Strength.ToString();
//        Creature_Details_Agi_Text.GetComponent<Text>().text = SelectedCreature.Agility.ToString();
//        Creature_Details_Mnd_Text.GetComponent<Text>().text = SelectedCreature.Mind.ToString();
//        Creature_Details_Wil_Text.GetComponent<Text>().text = SelectedCreature.Will.ToString();
//        Creature_Details_Size_Text.GetComponent<Text>().text = SelectedCreature.Size.ToString();
//        Creature_Details_Int_Text.GetComponent<Text>().text = SelectedCreature.Intelligence.ToString();

//        Creature_Details_Attributes_Dropdown.ClearOptions();
//        List<Dropdown.OptionData> DropDownData = new List<Dropdown.OptionData>();

//        DropDownData.Add(new Dropdown.OptionData("Attributes"));
//        foreach (Attribute attribute in SelectedCreature.CurrentAttributes)
//        {
//            DropDownData.Add(new Dropdown.OptionData(attribute.Name.ToString()));
//        }
//        Creature_Details_Attributes_Dropdown.AddOptions(DropDownData);

//        ChangeToCreatureDescriptionPanel(SelectedCreature);

//        Debug.Log("OpenCreatureDetails: Point 4");
//        OpenInfoPanel(CreaturesPanel);
//    }

//    public void CloseCreatureDetails()
//    {
//        Creature_Details_Background_Panel.SetActive(false);
//    }

    //public InitializedCreatureData InitializeNewCreature(string CreatureName)
    //{
    //    InitializedCreatureData NewCreature = new InitializedCreatureData(AllCreatures.Find(creature => creature.Name == CreatureName));

    //    return NewCreature;
    //}

    //public void ChangeToCreatureDescriptionPanel(InitializedCreature SelectedCreature)
    //{
    //    Creature_Details_Description_Panel.SetActive(true);
    //    Creature_Details_Attributes_Panel.SetActive(false);
    //    Creature_Details_Abilities_Panel.SetActive(false);

    //    Debug.Log("BattleCreatureImages/" + SelectedCreature.Name + "/" + EnemyCreature1.Name + "_Profile");
    //    Creature_Details_Image.sprite = Resources.Load<Sprite>("BattleCreatureImages/" + SelectedCreature.Name + "/" + EnemyCreature1.Name + "_Profile");
    //    Creature_Details_Description.text = SelectedCreature.Description;
    //}

    //public void OpenAttributeDescriptionPanel()
    //{
    //    Attribute SelectedAttribute = new Attribute(AttributeName.Tough);

    //    Creature_Details_Description_Panel.SetActive(false);
    //    Creature_Details_Attributes_Panel.SetActive(true);
    //    Creature_Details_Abilities_Panel.SetActive(false);

    //    Creature_Details_Attribute_Description.text = SelectedAttribute.Description;
    //}
    //public void OpenAbilityDescriptionPanel()
    //{
    //    Attribute SelectedAttribute = new Attribute(AttributeName.Tough);

    //    Creature_Details_Description_Panel.SetActive(false);
    //    Creature_Details_Attributes_Panel.SetActive(true);
    //    Creature_Details_Abilities_Panel.SetActive(false);

    //    Creature_Details_Attribute_Description.text = SelectedAttribute.Description;
    //}

}
