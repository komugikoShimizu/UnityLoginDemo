using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class JsonHelper
{
    // Jsonファイルをオブジェクトに変換する関数
    public static T[] FromJson<T>(string json)
    {
        // Jsonファイルを配列として扱えるようにする
        // Itemsなのはラッパークラスの変数名がItemsであるため
        string newJson = "{\"Items\":" + json + "}";

        // ラッパークラスにJsonの内容を記述する
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);

        // ラッパークラスの変数を返却する
        return wrapper.Items;
    }

    // ジェネリックを配列化するためのクラス
    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}