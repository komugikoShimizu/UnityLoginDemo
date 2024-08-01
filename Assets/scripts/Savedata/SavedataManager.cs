using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SavedataManager : MonoBehaviour
{
    private const string url = "http://127.0.0.1/add_user.php";

    private const string selectUrl = "http://127.0.0.1/get_user_select.php";

    [SerializeField]
    private string username = "user";

    [SerializeField]
    private string text = "text";

    [SerializeField]
    private TextAsset savefile;

    public static UserData USER {get; private set;}

    // Start is called before the first frame update
    void Start()
    {
        if (savefile.text == "")
        { 
            Debug.Log("NULL");
            MySQLDatabaseManager.instance.Post(url, username, 1, text, Method);
        }
        else
        {
            Debug.Log("NOT NULL");
            StreamReader reader = new StreamReader("Assets/Json/" + savefile.name + ".json");
            int id = int.Parse(reader.ReadLine());
            MySQLDatabaseManager.instance.GetSelectUser(selectUrl, id, Mehtod);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Method(int id)
    {
        UserData user = new UserData();
        user.id = id;
        user.name = username;
        user.state = 1;
        user.text = text;
        string str = JsonUtility.ToJson(user);
        USER = user;
        Debug.Log(str);
        StreamWriter writer = new StreamWriter("Assets/Json/" + savefile.name + ".json");
        writer.WriteLine(id);
        writer.Close();
    }

    private void Mehtod(UserData user)
    {
        Debug.Log(user.id);
        USER = user;
    }
}
