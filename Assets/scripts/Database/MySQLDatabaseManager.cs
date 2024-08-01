using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class MySQLDatabaseManager : MonoBehaviour
{
    // XAMPP内のPHPファイルへのURL
    private string testUrl = "http://127.0.0.1//get_user.php";

    // インスタンスを一つに維持するための静的変数
    public static MySQLDatabaseManager instance { get; private set;}

    void Awake()
    {
        if (instance == null) // インスタンスが存在していなければ
        {
            instance = this; // 自身をインスタンスに設定

            // 値取得用コルーチンを実行
            Get(testUrl, DebugLog);
        }
        else // インスタンスが存在していれば
        {
            Destroy(this.gameObject); // オブジェクトを削除してアクセスできなくする
        }
    }

    // デバッグ用コールバック関数
    private void DebugLog()
    {
        Debug.Log("アクセス完了");
    }

    // Getにアクセスしてデータベース内すべての情報を取得
    public void Get(string url, Action endAction)
    {
        StartCoroutine(GetDataFromServer(url, endAction));
    }

    // idをもとに抜き出すクエリを実行して、一致する情報を取得
    public void GetSelectUser(string url, int id, Action<UserData> act)
    {
        StartCoroutine(GetSelectUserServer(url, id, act));
    }

    // 指定された情報をもとに登録
    public void Post(string url, string name, int state, string text, Action<int> act)
    {
        StartCoroutine(PostDataToServer(url, name, state, text, act));
    }

    // PHPアクセスを行って値を取得する
    IEnumerator GetDataFromServer(string url, Action endAction)
    {
        // WebRequestを使用してPHPにアクセス
        UnityWebRequest www = UnityWebRequest.Get(url);

        // 値が送信されるまで処理を停止
        yield return www.SendWebRequest();

        // 返却データの内容が成功ではないなら
        if (www.result != UnityWebRequest.Result.Success)
        {
            // 返却データから例外文を表示
            Debug.LogError("Error: " + www.error);
        }
        else    // 返却データ内容が成功なら
        {
            // 取得されたJson内容を出力する
            Debug.Log("Received: " + www.downloadHandler.text);

            // 取得したJsonをもとにオブジェクトに変換する関数を実行
            ProcessJsonData(www.downloadHandler.text);

            endAction();
        }
    }

    // Jsonをもとにオブジェクトに変換する関数を実行
    void ProcessJsonData(string jsonData)
    {
        // Json変換クラスを通じてJson内容を配列オブジェクトに変換
        var dataArray = JsonHelper.FromJson<UserData>(jsonData);

        // Jsonを変換した配列オブジェクトをforeachで実行
        foreach (var data in dataArray)
        {
            // 各ステータスをログに出力
            Debug.Log("ID: " + data.id + ", name: " + data.name + ", state: " + data.state + ", text: " + data.text);
        }
    }

    // データベース登録コルーチン   
    IEnumerator PostDataToServer(string url, string name, int state, string text, Action<int> act)
    {
        // PHPに渡す情報を作成
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("state", state);
        form.AddField("text", text);

        UnityWebRequest www = UnityWebRequest.Post(url, form);  // URLにPostする
        yield return www.SendWebRequest();  // 値の返却があるまで処理を停止

        if (www.result != UnityWebRequest.Result.Success) // 返却されたデータに成功以外のデータが格納されている場合
        {
            Debug.LogError("Error: " + www.error); // ログにエラー文を出力                                                              
        }
        else
        {
            string jsonResponse = www.downloadHandler.text; // レスポンス内容を取得
            Debug.Log(jsonResponse); // レスポンス内容を出力

            act(int.Parse(jsonResponse)); // 登録されていたコールバックに返却されたデータを使用して実行
            
            Debug.Log("Received: " + www.downloadHandler.text);
        }
    }

    // 指定したユーザーを取り出すコルーチン
    IEnumerator GetSelectUserServer(string url, int id, Action<UserData> act)
    {
        // 送信する内容を作成
        WWWForm form = new WWWForm();
        form.AddField("id", id);

        url += "/?id=" + id;    // URLパラメータを追加

        UnityWebRequest www = UnityWebRequest.Get(url);  // URLにリクエストを送信
        yield return www.SendWebRequest();  // 値の返却があるまで処理を停止
        
        // 返却データの内容が成功ではないなら
        if (www.result != UnityWebRequest.Result.Success)
        {
            // 返却データから例外文を表示
            Debug.LogError("Error: " + www.error);
        }
        else    // 返却データ内容が成功なら
        {
            // 取得されたJson内容を出力する
            Debug.Log("Received: " + www.downloadHandler.text);

            string[] userDataColumn = www.downloadHandler.text.Split(":"); // 取得したデータをSplitで配列化する

            // 取得した情報からユーザーデータを初期化する
            UserData user = new UserData();
            user.id = int.Parse(userDataColumn[0]);
            user.name = userDataColumn[1];
            user.state = int.Parse(userDataColumn[2]);
            user.text = userDataColumn[2];

            act(user); // コールバックに作成したユーザー情報を渡して実行
        }
    }
}