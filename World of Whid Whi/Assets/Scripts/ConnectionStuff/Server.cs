using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using UnityEngine;
using MLAPI.Spawning;
using UnityEngine.UI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using System.Net;
using System;
using System.Net.Sockets;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using UnityEngine.TextCore.Text;

public class Server : MonoBehaviour
{
    public bool isServer;

    public static readonly Color CharacterButtonSelected = new Color(.25f, .17f, .08f, .375f);
    public static readonly Color CharacterButtonNotSelected = new Color(.44f, .32f, .18f, .375f);
    public const string MSSQL_DataSource = @"DESKTOP-4STICPG\MSSQLSERVER01";
    public const string MSSQL_UserID = "WWW_Server2";
    public const string MSSQL_Password = "FishSticksAreSmelly";
    public const string MSSQL_InitialCatalog = "WWWDB";

    public const float TIME_BETWEEN_NETWORK_CHECKS = 1f;

    public Text AccountMenuTitle;
    public InputField Email;
    public InputField Password;
    public InputField CreateAccount_Email;
    public InputField CreateAccount_Password;
    public Text ErrorText;
    public Text NewCharacterName;
    public Connection Connection;
    public GameManager GM;
    public GameObject AccountSelectPanel;
    public GameObject LoginCanvas;
    public GameObject InGameCanvas;

    public int AccountID;
    public Button CharacterSelectButton;
    public List<CharacterData> Characters;
    public List<Button> CharacterSelectButtons;
    public int CurrentCharacterIndex;
    public Dictionary<ulong, CharacterData> activeCharacters;

    float Timer = 0.0f;
    float TimeSinceSuccessfulPing = 0;

    // Data base
    SqlConnectionStringBuilder builder;

    // Start is called before the first frame update
    void Start()
    {
        //AccountSelectPanel.SetActive(true);
        //CharacterSelectButtons = new List<Button>();
        //CharacterSelectButtons.Add(Instantiate(CharacterSelectButton, AccountSelectPanel.transform)); //875, 480, 0
        //CharacterSelectButtons[0].transform.position = new Vector3(CharacterSelectButtons[0].transform.position.x, CharacterSelectButtons[0].transform.position.y - 0, 0);
        //CharacterSelectButtons[0].onClick.AddListener(delegate { SelectCharacterToLoad(0); });

        //button = Instantiate(CharacterSelectButton, AccountSelectPanel.transform); //875, 480, 0
        //button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y - 80, 0);
        //button = Instantiate(CharacterSelectButton, AccountSelectPanel.transform); //875, 480, 0
        //button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y - 160, 0);
    }

    void Update()
    {
    }

    public void ReConnect()
    {
        Connection.ConnectToServer();
    }

    public void Connect()
    {

        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        Debug.Log("GetLocalIPAddress() " + GetLocalIPAddress());

        NetworkManager.Singleton.OnClientConnectedCallback += clientId => ClientConnectedCallback(clientId);
        NetworkManager.Singleton.OnClientDisconnectCallback += clientId => ClientDisconnectedCallback(clientId);

        if (isServer) //GetLocalIPAddress() == "10.0.0.38"
        {
            // This is the Server
            NetworkManager.Singleton.StartServer();
            activeCharacters = new Dictionary<ulong, CharacterData>();

            // Build connection string
            builder = new SqlConnectionStringBuilder();
            builder.DataSource = MSSQL_DataSource;
            //builder.TrustServerCertificate = true;
            builder.UserID = MSSQL_UserID;
            builder.Password = MSSQL_Password;
            builder.InitialCatalog = MSSQL_InitialCatalog;
        }
        else
        {
            // This is a Client
            CurrentCharacterIndex = -1;
            ErrorText.text = "Not Connected to the Server!";
            Connection.ConnectToServer();
            //Client.SetActive(true);
            //Client.GetComponent<User>().Connection.ConnectToServer();
        }

    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, MLAPI.NetworkManager.ConnectionApprovedDelegate callback)
    {
        //Your logic here
        Debug.Log("Creating Client.");
        ErrorText.text = "Creating Client";

        if ("Please" == System.Text.Encoding.ASCII.GetString(connectionData))
        {
            bool approve = true;
            bool createPlayerObject = false;

            // The prefab hash. Use null to use the default player prefab
            // If using this hash, replace "MyPrefabHashGenerator" with the name of a prefab added to the NetworkPrefabs field of your NetworkManager object in the scene
            ulong? prefabHash = NetworkSpawnManager.GetPrefabHashFromGenerator("MyPrefabHashGenerator");

            //If approve is true, the connection gets added. If it's false. The client gets disconnected
            callback(createPlayerObject, prefabHash, approve, new Vector3(0, 0, 0), new Quaternion());

            //this.gameObject.GetComponent<User>().SetCameraFollowClientRpc(0);
        }
    }

    public void ConnectionApprovalCallback()
    {
        Debug.Log("Client Connection Approved");
        //ErrorText.text = "Connection Approved";
    }

    /// <summary>
    /// Called on the client when the client connects to the server
    /// </summary>
    /// <param name="clientId"></param>
    public void ClientConnectedCallback(ulong clientId)
    {
        //ErrorText.text = "Connected";
        //Debug.Log("Client " + clientId + " has connected!");

        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            //LoginCanvas.SetActive(false);
            //InGameCanvas.SetActive(true);
            //GM.SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
            CancelInvoke("ReConnect");
            ErrorText.text = "";
        }
        else if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log("Client " + clientId + " has connected!");
        }
    }

    public void ClientDisconnectedCallback(ulong clientId)
    {
        //ErrorText.text = "Not Connected to the Server!";
        //Debug.Log("Lost client " + clientId);

        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
//            GM.RemoveScene(Characters[CurrentCharacterIndex].Location); character is already destroyed at this point. we need to keep track of the current location on something other then the player.
            AccountSelectPanel.SetActive(false);
            ErrorText.text = "Not Connected to the Server!";
            InvokeRepeating("ReConnect", .1f, 1f);
        }
        else if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log("Lost client " + clientId);
            Debug.Log("About to call Handle_Disconnect");
            Handle_Disconnect(clientId);
            activeCharacters.Remove(clientId);
        }
    }

    public void SignIn()
    {
        Debug.Log("In SignIn()");
        GM.LogToServerRpc(NetworkManager.Singleton.LocalClientId, "Signing in");
        GM.DB_SignIn_ServerRpc(NetworkManager.Singleton.LocalClientId, Email.text, Password.text);
        Email.text = "";
        Password.text = "";
    }

    /// <summary>
    /// Called by server when client attempts to sign in.
    /// 
    /// >= 0: Successful (response == account ID)
    /// -1: Incorrect email or password
    /// -99: SQL error
    /// </summary>
    /// <param name="clientId"></param>
    public void Handle_Signin_Request(ulong clientId, string email, string password)
    {
        Debug.Log("Got sign in request: " + email + ", " + password);
        int AccountID = -1;
        List<CharacterData> characters = new List<CharacterData>();

        try
        {
            // connect to the databases
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                // if open then the connection is established
                connection.Open();
                Debug.Log("connection established");

                // sql command
                string sql = "SELECT ID " +
                    "FROM UserAccount " +
                    "WHERE email = '" + email + "' and password = '" + password + "'";
                // execute sql command
                Debug.Log(sql);
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // read
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // each line in the output
                        while (reader.Read())
                        {
                            AccountID = (int)reader["ID"];
                        }
                    }
                }

                if (AccountID >= 0)
                {
                    // sql command
                    sql = "SELECT ID, name, lvl, xp, location, position_X, position_Y  " +
                        "FROM Character " +
                        "WHERE account = '" + AccountID + "'";
                    // execute sql command
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // read
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            activeCharacters.Add(clientId, new CharacterData(AccountID));
                            // each line in the output
                            while (reader.Read())
                            {
                                Debug.Log((int)reader["ID"]);
                                Debug.Log((int)reader["lvl"]);
                                Debug.Log((int)reader["xp"]);
                                Debug.Log((double)reader["Position_X"]);
                                Debug.Log((double)reader["Position_Y"]);
                                Debug.Log((string)reader["name"]);
                                Debug.Log((string)reader["location"]);
                                double x = (double)reader["Position_X"];
                                double y = (double)reader["Position_Y"];
                                characters.Add(new CharacterData((int)reader["ID"], AccountID, (string)reader["name"], (int)reader["lvl"], (int)reader["xp"], (string)reader["location"], (float)x, (float)y));
                            }
                        }
                    }

                }

                // next get their creature data info and add icons of their current squad so we can get a bit of info about the character before hitting play
                // eventually we will probably want a button to show a more full set of character details before jumping into game but we can add that later.
                connection.Close();
            }
        }
        catch (SqlException e)
        {
            AccountID = -99;
            Debug.Log(e.ToString());
        }

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };
        GM.SignIn_Response_ClientRpc(AccountID, characters.ToArray(), clientRpcParams);

        Debug.Log("AccountID is " + AccountID);
    }

    /// <summary>
    /// Called By Client with the result of the sign in request made.
    /// </summary>
    /// <param name="SignInStatus"></param>
    public void Handle_Signin_Response(int AccountId, CharacterData[] characters)
    {
        if (AccountId == -1)
        {
            ErrorText.text = "Incorrect email or password, please try again.";
        }
        else if (AccountId == -99)
        {
            ErrorText.text = "Unknonw Issue: Please request support.";
        }
        else
        {
            // Sign in successful
            AccountSelectPanel.SetActive(true);

            AccountMenuTitle.text = "Welcome " + Email.text;

            ErrorText.text = "";
            Characters = characters.ToList();
            AccountID = AccountId;
            RecreateCharacterButtons();
            Debug.Log("AccountID is " + AccountID);
        }
    }

    /// <summary>
    /// Called by server when client disconnects. Later we will need to handle doing something like this also when they just sign out.
    /// </summary>
    /// <param name="clientId"></param>
    public void Handle_Disconnect(ulong clientId)
    {
        Debug.Log("Handle_Disconnect");
        Debug.Log("ID = " + activeCharacters[clientId].ID);
        Debug.Log("x = " + activeCharacters[clientId].Position_X);

        try
        {
            // connect to the databases
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                // if open then the connection is established
                connection.Open();
                Debug.Log("connection established");

                // sql command
                string sql = "UPDATE Character " 
                    + "SET lvl = " + activeCharacters[clientId].Lvl + ", xp = " + activeCharacters[clientId].XP + ", location = '" + activeCharacters[clientId].Location + "', position_X = " 
                    + activeCharacters[clientId].Position_X + ", position_Y = " + activeCharacters[clientId].Position_Y
                    + " WHERE ID = " + activeCharacters[clientId].ID;
                // execute sql command
                Debug.Log(sql);
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
                Debug.Log("Character Updated");
                connection.Close();
            }
        }
        catch (SqlException e)
        {
            AccountID = -99;
            Debug.Log(e.ToString());
        }

        //ClientRpcParams clientRpcParams = new ClientRpcParams
        //{
        //    Send = new ClientRpcSendParams
        //    {
        //        TargetClientIds = new ulong[] { clientId }
        //    }
        //};
        //GM.SignIn_Response_ClientRpc(AccountID, characters.ToArray(), clientRpcParams);

        //Debug.Log("AccountID is " + AccountID);
    }

    public void CreateAccount()
    {
        GM.LogToServerRpc(NetworkManager.Singleton.LocalClientId, "Creating an Account");
        if (!CreateAccount_Email.text.Contains('@'))
        {
            ErrorText.text = "Please use a valid email address.";
        } else if (CreateAccount_Password.text.Length < 5)
        {
            ErrorText.text = "Password must be at least 6 characters.";
        } else
        {
            GM.DB_CreateAccount_ServerRpc(NetworkManager.Singleton.LocalClientId, CreateAccount_Email.text, CreateAccount_Password.text);
        }
    }

    /// <summary>
    /// Called by the client to delete a character
    /// Calls a server function in GameManager which calls Handle_DeleteCharacter_Request()
    /// </summary>
    /// <param name="name"></param>
    public void ConfirmDeleteCharacter()
    {
        // if <confirmation prompt> is yes then do the work
        // turn confirmation into prefab then create prefab here and make it's parent the canvas.
        // As part of creating it we can give it a class with a method that calls DB_DeleteCharacter_ServerRpc
        // and destroys itself when yes is clicked or just destroys itself when no is clicked
        // Instantate YesNoPanel prefab and set text
        // Add yes button method and no button method
        GM.DB_DeleteCharacter_ServerRpc(NetworkManager.Singleton.LocalClientId, Characters[CurrentCharacterIndex].ID);
    }

    /// <summary>
    /// Called on the client by the server whenn a character has been successfully deleted
    /// </summary>
    public void Handle_DeleteCharacter_Response(bool success)
    {
        if (success)
        {
            Characters.RemoveAt(CurrentCharacterIndex);
            RecreateCharacterButtons();
        } else
        {
            // !FIX should create a pop up letting the user know it failed.
        }
    }

    public void RecreateCharacterButtons()
    {
        if (CharacterSelectButtons != null)
        {
            foreach (Button button in CharacterSelectButtons)
            {
                Destroy(button.gameObject);
            }
            CharacterSelectButtons.Clear();
        } else
        {
            CharacterSelectButtons = new List<Button>();
        }

        for (int i = 0; i < Characters.Count; i++)
        {
            CharacterSelectButtons.Add(Instantiate(CharacterSelectButton, AccountSelectPanel.transform));
            CharacterSelectButtons[i].transform.position = new Vector3(CharacterSelectButtons[i].transform.position.x, CharacterSelectButtons[i].transform.position.y - (i * 170), 0);
            int index = i;
            CharacterSelectButtons[i].onClick.AddListener(delegate { SelectCharacterToLoad(index); }); // not sure why I need the minus 1 hear but it keeps starting at 1.
            foreach (Text text in CharacterSelectButtons[i].GetComponentsInChildren<Text>())
            {
                if (text.transform.name.Equals("Name"))
                {
                    text.text = Characters[i].Name;
                }
                else if (text.transform.name.Equals("Lvl"))
                {
                    text.text = Characters[i].Lvl.ToString();
                }
                else if (text.transform.name.Equals("Details"))
                {
                    text.text = Characters[i].Location;
                }
            }
        }

    }

    /// <summary>
    /// Called on the Server to delete a character
    /// </summary>
    public void Handle_DeleteCharacter_Request(ulong clientId, int id)
    {
        Debug.Log("Got delete character request: " + clientId + ", " + id);
        bool success = false;

        try
        {
            // connect to the databases
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                // if open then the connection is established
                connection.Open();
                Debug.Log("connection established");

                // sql command
                string sql = "DELETE Character WHERE account = " + activeCharacters[clientId].Account + " AND ID = " + id;
                Debug.Log(sql);
                // execute sql command
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // read
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //// each line in the output
                        //while (reader.Read())
                        //{
                        //    AccountID = (int)reader["ID"];
                        //}
                    }
                }
                connection.Close();
            }
            success = true;
        }
        catch (SqlException e)
        {
            AccountID = -99;
            Debug.Log(e.ToString());
        }

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };
        GM.DeleteCharacter_Response_ClientRpc(success, clientRpcParams);
    }

    /// <summary>
    /// Called by the client to create a new character
    /// Calls a server function in GameManager which calls Handle_CreateCharacter_Request()
    /// 
    /// We will be sending more details for creating a character as new features are added. For now it is just their name.
    /// </summary>
    public void CreateCharacter()
    {
        // send server your new character name
        GM.DB_CreateCharacter_ServerRpc(NetworkManager.Singleton.LocalClientId, NewCharacterName.text);
    }

    public void Handle_CreateCharacter_Response(CharacterData character)
    {
        if (character != null)
        {
            Characters.Add(character);
            RecreateCharacterButtons();
        } else
        {
            // notifiy client user that creation failed.
        }
    }

    /// <summary>
    /// Called on the Server to create a character
    /// </summary>
    public CharacterData Handle_CreateCharacter_Request(ulong clientId, string name)
    {
        Debug.Log("Got create character request: " + clientId + ", " + name);
        CharacterData newCharacterData = null;

        try
        {
            // connect to the databases
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                // if open then the connection is established
                connection.Open();
                Debug.Log("connection established");

                // sql command
                string sql = "INSERT INTO Character (account, name, lvl, xp, location, position_X, position_Y) VALUES ('" 
                    + activeCharacters[clientId].Account + "', '" + name + "', '1', '0', 'Starting_Area', 100, -87)";
                Debug.Log(sql);
                // execute sql command
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();

                sql = "SELECT * FROM Character WHERE account = '" + activeCharacters[clientId].Account + "' AND name = '" + name + "'";
                Debug.Log(sql);

                using (command = new SqlCommand(sql, connection))
                {
                    // read
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // each line in the output
                        while (reader.Read())
                        {
                            double x = (double)reader["Position_X"];
                            double y = (double)reader["Position_Y"];
                            Debug.Log(x + ", " + y);
                            newCharacterData = new CharacterData((int)reader["ID"], AccountID, (string)reader["name"], (int)reader["lvl"], (int)reader["xp"], (string)reader["location"], (float)x, (float)y);
                        }
                    }
                }
                connection.Close();
            }
        }
        catch (SqlException e)
        {
            AccountID = -99;
            Debug.Log(e.ToString());
        }

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };
        GM.CreateCharacter_Response_ClientRpc(newCharacterData, clientRpcParams);

        Debug.Log("Created " + newCharacterData.Name);
        return newCharacterData;
    }

    /// <summary>
    /// Called by server when client attempts to create an account.
    /// 
    /// >= 0: Successful (response == account ID)
    /// -1: Unknown Error
    /// -2: Email already in use
    /// -99: SQL error
    /// /// </summary>
    /// <param name="clientId"></param>
    public void Handle_CreateAccount_Request(ulong clientId, string email, string password)
    {
        Debug.Log("Got create account request: " + email + ", " + password);
        int AccountID = -1;
        List<CharacterData> characters = new List<CharacterData>();

        try
        {
            // connect to the databases
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                // if open then the connection is established
                connection.Open();
                Debug.Log("connection established");

                // sql command
                string sql = "SELECT ID " +
                    "FROM UserAccount " +
                    "WHERE email = '" + email + "'";
                Debug.Log(sql);
                // execute sql command
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // read
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // each line in the output
                        while (reader.Read())
                        {
                            AccountID = (int)reader["ID"];
                        }
                    }
                }

                if (AccountID <= 0)
                {
                    // sql command
                    sql = "INSERT INTO UserAccount (email, password) VALUES ('" + email + "', '" + password + "')";
                    // execute sql command
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();

                    // sql command
                    sql = "SELECT ID " +
                        "FROM UserAccount " +
                        "WHERE email = '" + email + "' and password = '" + password + "'";
                    Debug.Log(sql);
                    // execute sql command
                    using (command = new SqlCommand(sql, connection))
                    {
                        // read
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // each line in the output
                            while (reader.Read())
                            {
                                AccountID = (int)reader["ID"];
                            }
                        }
                    }
                }
                else // Email already in use.
                {
                    AccountID = -2;
                }
                connection.Close();
            }
        }
        catch (SqlException e)
        {
            AccountID = -99;
            Debug.Log(e.ToString());
        }
        GM.CreateAccount_Response_ClientRpc(AccountID);

    }

    /// <summary>
    /// Handles create account responses on the client side
    /// 
    /// Response Codes
    /// 0: Successful (response == account ID)
    /// -2: Email already in use
    /// -99: SQL error

    /// </summary>
    /// <param name="responseCode"></param>
    public void Handle_CreateAccount_Response(int responseCode)
    {
        Debug.Log("responseCode: " + responseCode);
        if (responseCode == -2) // email already exists
        {
            ErrorText.text = "Email already exists, please use a different email.";
        }
        else if (responseCode == -1 || responseCode == -99) // SQL Error
        {
            ErrorText.text = "Unknonw Issue: Please request support.";
        }
        else // responseCode == 0 (success)
        {
            Email.text = CreateAccount_Email.text;
            Password.text = CreateAccount_Password.text;
            CreateAccount_Email.text = "";
            CreateAccount_Password.text = "";
            CreateAccount_Email.transform.parent.gameObject.SetActive(false);
            SignIn();
        }

    }


    /// <summary>
    /// This is called on the client side when the sign out button is clicked
    /// </summary>
    public void Sign_Out() // need to add to this. For one it should notify the server. (maybe when loading a character it should also make sure the character bellongs to the client's account?
    {
        GM.SignOut_ServerRpc(NetworkManager.Singleton.LocalClientId);
        // later we will also want a way to just switch characters rather then fully signing out.
        AccountSelectPanel.SetActive(false);
        CurrentCharacterIndex = -1;
        Characters.Clear();
        RecreateCharacterButtons();
    }

    /// <summary>
    /// Called by server when a client signs out.
    /// NOTE: the connection / client still exists.
    /// </summary>
    /// <param name="clientId"></param>
    public void Handle_SignOut_Request(ulong clientId)
    {
        Debug.Log(clientId + " has signed out.");
        activeCharacters.Remove(clientId);
    }

    public void SelectCharacterToLoad(int characterIndex)
    {
        Debug.Log("In SelectCharacterToLoad");
        GM.LogToServerRpc(NetworkManager.Singleton.LocalClientId, "Selected character " + characterIndex);
        if (CurrentCharacterIndex >= 0)
        {
            CharacterSelectButtons[CurrentCharacterIndex].GetComponent<Image>().color = CharacterButtonNotSelected;
        }
        CurrentCharacterIndex = characterIndex;
        CharacterSelectButtons[characterIndex].GetComponent<Image>().color = CharacterButtonSelected;
        GM.LogToServerRpc(NetworkManager.Singleton.LocalClientId, "Selected character " + characterIndex);
    }

    /// <summary>
    /// Called by server to get the character data requested by the user when they click the Play button.
    /// </summary>
    /// <returns></returns>
    public CharacterData DB_GetCharacterData(ulong clientId, int characterId)
    {
        Debug.Log("In DB_GetCharacterData: " + clientId + ", " + characterId);
        int accountId = activeCharacters[clientId].Account;
        Debug.Log("Got Play Request (AccountId, CharacterId): " + accountId);
        CharacterData character = null;

        try
        {
            // connect to the databases
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                // if open then the connection is established
                connection.Open();
                Debug.Log("connection established");

                // sql command
                string sql = "SELECT ID, name, lvl, xp, location, position_X, position_Y  " +
                    "FROM Character " +
                    "WHERE account = '" + accountId + "' and ID = '" + characterId + "'";
                Debug.Log(sql);
                // execute sql command
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // read
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // each line in the output
                        while (reader.Read())
                        {
                            Debug.Log((int)reader["ID"]);
                            Debug.Log((int)reader["lvl"]);
                            Debug.Log((int)reader["xp"]);
                            Debug.Log((double)reader["Position_X"]);
                            Debug.Log((double)reader["Position_Y"]);
                            Debug.Log((string)reader["name"]);
                            Debug.Log((string)reader["location"]);
                            double x = (double)reader["Position_X"];
                            double y = (double)reader["Position_Y"];
                            character = new CharacterData((int)reader["ID"], AccountID, (string)reader["name"], (int)reader["lvl"], (int)reader["xp"], (string)reader["location"], (float)x, (float)y);
                            Debug.Log("New Character = " + character);
                        }
                    }
                }
                connection.Close();

            }
        }
        catch (SqlException e)
        {
            AccountID = -99;
            Debug.Log(e.ToString());
        }

        Debug.Log("Returning Character = " + character);
        return character;
    }

    /// <summary>
    /// 
    /// 
    /// Also need to add code to create creatures on create account and to save creature data on sign out.
    /// </summary>
    /// <param name="characterId"></param>
    /// <returns></returns>
    public List<InitializedCreatureData> DB_GetPlayerCreatures(int characterId)
    {
        Debug.Log("In DB_GetPlayerCreatures");
        List<InitializedCreatureData> creatures = new List<InitializedCreatureData>();
        try
        {
            // connect to the databases
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                // if open then the connection is established
                connection.Open();
                Debug.Log("connection established");

                // sql command
                string sql = "SELECT ID, character, name, nick_name, current_hp, size, strength, agility, mind, will, xp  " +
                    "FROM Creature " +
                    "WHERE character = '" + characterId + "'";
                Debug.Log(sql);
                // execute sql command
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // read
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // each line in the output
                        while (reader.Read())
                        {
                            Debug.Log((int)reader["ID"]);
                            Debug.Log((int)reader["character"]);
                            Debug.Log((string)reader["name"]);
                            Debug.Log((string)reader["nick_name"]);
                            Debug.Log((int)reader["current_hp"]);
                            Debug.Log((string)reader["size"]);
                            Debug.Log((int)reader["strength"]);
                            Debug.Log((int)reader["agility"]);
                            Debug.Log((int)reader["mind"]);
                            Debug.Log((int)reader["will"]);
                            Debug.Log((int)reader["xp"]);
                            creatures.Add(new InitializedCreatureData((int)reader["ID"], (string)reader["name"], (string)reader["nick_name"], (int)reader["current_hp"], (string)reader["size"], (int)reader["strength"], 
                                (int)reader["agility"], (int)reader["mind"], (int)reader["will"], (int)reader["xp"]));
                        }
                    }
                }
                connection.Close();
            }
        }
        catch (SqlException e)
        {
            AccountID = -99;
            Debug.Log(e.ToString());
        }
        return creatures;
    }

    // instead of sending the character fields we should just send the index (the sql index not a local one). Then the server just loads in that index for that account so no changing you data.
    // then sign out just clears the account for that connection. 
    // move the get account data from sql code to it's own method and we can reuse it for when the client needs it and the server needs it.
    public void EnterWorld()
    {
        GM.SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, Characters[CurrentCharacterIndex].ID);
        LoginCanvas.SetActive(false);
        InGameCanvas.SetActive(true);

    }

}
