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

public class Server : MonoBehaviour
{
    public static readonly Color CharacterButtonSelected = new Color(.25f, .17f, .08f, .375f);
    public static readonly Color CharacterButtonNotSelected = new Color(.44f, .32f, .18f, .375f);
    public const string MSSQL_DataSource = @"DESKTOP-4STICPG\MSSQLSERVER01";
    public const string MSSQL_UserID = "WWW_Server2";
    public const string MSSQL_Password = "FishSticksAreSmelly";
    public const string MSSQL_InitialCatalog = "WWWDB";

    public Text AccountMenuTitle;
    public Text Email;
    public Text Password;
    public Text CreateAccount_Email;
    public Text CreateAccount_Password;
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

    public void Connect()
    {
        CurrentCharacterIndex = -1;

        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        Debug.Log("GetLocalIPAddress() " + GetLocalIPAddress());

        NetworkManager.Singleton.OnClientConnectedCallback += clientId => ClientConnectedCallback(clientId);
        NetworkManager.Singleton.OnClientDisconnectCallback += clientId => ClientDisconnectedCallback(clientId);

        if (GetLocalIPAddress() == "10.0.0.38")
        {
            NetworkManager.Singleton.StartServer();
            activeCharacters = new Dictionary<ulong, CharacterData>();
        }
        else
        {
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

    public void ClientConnectedCallback(ulong clientId)
    {
        //ErrorText.text = "Connected";
        //Debug.Log("Client " + clientId + " has connected!");

        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            //LoginCanvas.SetActive(false);
            //InGameCanvas.SetActive(true);
            //GM.SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
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
            GM.RemoveScene(Characters[CurrentCharacterIndex].Location);
            AccountSelectPanel.SetActive(false);
            ErrorText.text = "Not Connected to the Server!";
        }
        else if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log("Lost client " + clientId);
            Debug.Log("About to call Handle_Disconnect");
            Handle_Disconnect(clientId);
            activeCharacters.Remove(clientId);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (NetworkManager.Singleton.IsServer)
        //{
        //    Debug.Log(NetworkManager.Singleton.ConnectedClients.Count);
        //}

        //ConnectionCount.text = NetworkManager.Singleton.ConnectedClients.Count.ToString();
    }

    public void SignIn()
    {
        Email.text = "";
        Password.text = "";
        GM.LogToServerRpc(NetworkManager.Singleton.LocalClientId, "Signing in");
        GM.DB_SignIn_ServerRpc(NetworkManager.Singleton.LocalClientId, Email.text, Password.text);
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

        // Build connection string
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.DataSource = MSSQL_DataSource;
        //builder.TrustServerCertificate = true;
        builder.UserID = MSSQL_UserID;
        builder.Password = MSSQL_Password;
        builder.InitialCatalog = MSSQL_InitialCatalog;
        //builder.PersistSecurityInfo = true;
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
    public void Handle_Signin_Response(int SignInStatus, CharacterData[] characters)
    {
        if (SignInStatus == -1)
        {
            ErrorText.text = "Incorrect email or password, please try again.";
        }
        else if (SignInStatus == -2)
        {
            ErrorText.text = "Email already in use.";
        }
        else if (SignInStatus == -99)
        {
            ErrorText.text = "Unknonw Issue: Please request support.";
        }
        else
        {
            // Sign in successful
            AccountSelectPanel.SetActive(true);

            AccountMenuTitle.text = "Welcome " + Email.text;

            ErrorText.text = "";
            AccountID = characters[0].Account;

            Characters = characters.ToList();
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
        // Build connection string
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.DataSource = MSSQL_DataSource;
        //builder.TrustServerCertificate = true;
        builder.UserID = MSSQL_UserID;
        builder.Password = MSSQL_Password;
        builder.InitialCatalog = MSSQL_InitialCatalog;
        //builder.PersistSecurityInfo = true;
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
        GM.DB_CreateAccount_ServerRpc(NetworkManager.Singleton.LocalClientId, CreateAccount_Email.text, CreateAccount_Password.text);
        Email.text = CreateAccount_Email.text;
        Password.text = CreateAccount_Password.text;
        CreateAccount_Email.text = "";
        CreateAccount_Password.text = "";
        SignIn();
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

        // Build connection string
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.DataSource = MSSQL_DataSource;
        //builder.TrustServerCertificate = true;
        builder.UserID = MSSQL_UserID;
        builder.Password = MSSQL_Password;
        builder.InitialCatalog = MSSQL_InitialCatalog;
        //builder.PersistSecurityInfo = true;
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
        // Build connection string
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.DataSource = MSSQL_DataSource;
        //builder.TrustServerCertificate = true;
        builder.UserID = MSSQL_UserID;
        builder.Password = MSSQL_Password;
        builder.InitialCatalog = MSSQL_InitialCatalog;
        //builder.PersistSecurityInfo = true;
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
    /// -2: Email already in use
    /// -99: SQL error
    /// /// </summary>
    /// <param name="clientId"></param>
    public void Handle_CreateAccount_Request(ulong clientId, string email, string password)
    {
        Debug.Log("Got create account request: " + email + ", " + password);
        int AccountID = -2;
        List<CharacterData> characters = new List<CharacterData>();

        // Build connection string
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.DataSource = MSSQL_DataSource;
        //builder.TrustServerCertificate = true;
        builder.UserID = MSSQL_UserID;
        builder.Password = MSSQL_Password;
        builder.InitialCatalog = MSSQL_InitialCatalog;
        //builder.PersistSecurityInfo = true;
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

                if (AccountID <= 0) // Email not already in use.
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
            }
        }
        catch (SqlException e)
        {
            AccountID = -99;
            Debug.Log(e.ToString());
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
        int accountId = activeCharacters[clientId].Account;
        Debug.Log("Got Play Request (AccountId, CharacterId): " + characterId + ", " + accountId);
        CharacterData character = null;

        // Build connection string
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.DataSource = MSSQL_DataSource;
        //builder.TrustServerCertificate = true;
        builder.UserID = MSSQL_UserID;
        builder.Password = MSSQL_Password;
        builder.InitialCatalog = MSSQL_InitialCatalog;
        //builder.PersistSecurityInfo = true;
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
                        }
                    }
                }

            }
        }
        catch (SqlException e)
        {
            AccountID = -99;
            Debug.Log(e.ToString());
        }

        return character;
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
