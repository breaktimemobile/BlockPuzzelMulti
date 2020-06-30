using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

///
public class JsonRead : MonoBehaviour
{
    public static JsonRead Instance;

    private void Awake()
    {
        Instance = this;
    }

    private readonly string PlayerDataName = "/Player.dat";

    public bool CheckPlayer()
    {
        string GameInfoPath = Application.persistentDataPath + PlayerDataName;
        System.IO.File.Delete(GameInfoPath);

        return false;
    }
    #region SaveData


    /// <summary>
    /// 집 정보 저장 하기
    /// </summary>
    /// <param name="SaveData"></param>
    public void Save(State_Player SaveData)
    {
        string GameInfoPath = Application.persistentDataPath + PlayerDataName;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(GameInfoPath);

        string jsonStr = JsonUtility.ToJson(SaveData);
        string aes = AESCrypto.instance.AESEncrypt128(jsonStr);
        bf.Serialize(file, aes);
        file.Close();
    }

    #endregion SaveData

    #region LoadData


    /// <summary>
    /// 자기 정보 가져오기
    /// </summary>
    /// <returns></returns>
    public State_Player Load_Player()
    {
        string InfoPath = Application.persistentDataPath + PlayerDataName;
        State_Player playerInfoSave = new State_Player();

        if (File.Exists(InfoPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(InfoPath, FileMode.Open);
            var str = (string)bf.Deserialize(file);
            file.Close();

            if (!string.IsNullOrEmpty(str))
            {
                string aes = AESCrypto.instance.AESDecrypt128(str);

                var data = JsonUtility.FromJson<State_Player>(aes);

                playerInfoSave = data;
            }
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(InfoPath);
            State_Player data = new State_Player();

            playerInfoSave = data;

            string jsonStr = JsonUtility.ToJson(playerInfoSave);
            string aes = AESCrypto.instance.AESEncrypt128(jsonStr);

            bf.Serialize(file, aes);
            file.Close();
        }

        return playerInfoSave;
    }

    #endregion LoadData
}