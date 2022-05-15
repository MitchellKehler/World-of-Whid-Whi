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

    public Text Email;
    public Text Password;
    public Text CreateAccount_Email;
    public Text CreateAccount_Password;
    public Text ErrorText;
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
        GM.LogToServerRpc(NetworkManager.Singleton.LocalClientId, "Signing in");
        GM.DB_SignIn_ServerRpc(NetworkManager.Singleton.LocalClientId, Email.text, Password.text);
    }

    public void CreateAccount()
    {
        GM.LogToServerRpc(NetworkManager.Singleton.LocalClientId, "Creating an Account");
        GM.DB_CreateAccount_ServerRpc(NetworkManager.Singleton.LocalClientId, CreateAccount_Email.text, CreateAccount_Password.text);
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

            Email.text = "";
            Password.text = "";
            CreateAccount_Email.text = "";
            CreateAccount_Password.text = "";
            ErrorText.text = "";
            AccountID = characters[0].Account;

            if (CharacterSelectButtons != null)
            {
                foreach (Button button in CharacterSelectButtons)
                {
                    Destroy(button);
                }
                CharacterSelectButtons.Clear();
            }

            Characters = characters.ToList();
            CharacterSelectButtons = new List<Button>();
            for (int i = 0; i < characters.Length; i++)
            {
                CharacterSelectButtons.Add(Instantiate(CharacterSelectButton, AccountSelectPanel.transform));
                CharacterSelectButtons[i].transform.position = new Vector3(CharacterSelectButtons[i].transform.position.x, CharacterSelectButtons[i].transform.position.y - (i * 170), 0);
                int index = i;
                CharacterSelectButtons[i].onClick.AddListener(delegate { SelectCharacterToLoad(index); }); // not sure why I need the minus 1 hear but it keeps starting at 1.
                foreach (Text text in CharacterSelectButtons[i].GetComponentsInChildren<Text>())
                {
                    if (text.transform.name.Equals("Name"))
                    {
                        text.text = characters[i].Name;
                    }
                    else if (text.transform.name.Equals("Lvl"))
                    {
                        text.text = characters[i].Lvl.ToString();
                    }
                    else if (text.transform.name.Equals("Details"))
                    {
                        text.text = characters[i].Location;
                    }
                }
            }
        }
    }


    public void Sign_Out() // need to add to this. For one it should notify the server. (maybe when loading a character it should also make sure the character bellongs to the client's account?
    {
        // also needs to remove scene and remove character from activeCharacters
        // later we will also want a way to just switch characters rather then fully signing out.
        AccountSelectPanel.SetActive(false);
        CurrentCharacterIndex = -1;
    }

    public void SelectCharacterToLoad(int characterIndex)
    {
        Debug.Log("In SelectCharacterToLoad");
        ErrorText.text = "In SelectCharacterToLoad";
        GM.LogToServerRpc(NetworkManager.Singleton.LocalClientId, "Selected character " + characterIndex);
        if (CurrentCharacterIndex >= 0)
        {
            CharacterSelectButtons[CurrentCharacterIndex].GetComponent<Image>().color = CharacterButtonNotSelected;
        }
        CurrentCharacterIndex = characterIndex;
        CharacterSelectButtons[characterIndex].GetComponent<Image>().color = CharacterButtonSelected;
        GM.LogToServerRpc(NetworkManager.Singleton.LocalClientId, "Selected character " + characterIndex);
    }

    public void EnterWorld()
    {
        GM.SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, Characters[CurrentCharacterIndex]);
        LoginCanvas.SetActive(false);
        InGameCanvas.SetActive(true);

    }

}
