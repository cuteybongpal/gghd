using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class DataManager
{
    public static GameSettings settingData;
    public void Init()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));
        using (StringReader stringReader = new StringReader(CommandManager.Instance.ExecuteCommand<TextAsset>(new Load(), "GameSetting.data").ToString()))
        {
            settingData = (GameSettings)serializer.Deserialize(stringReader);
        }
    }
}

[XmlRoot("setting")]
public class GameSettings
{
    [XmlElement("gravityScale")]
    public float GravityScale { get; set; }

    [XmlElement("PlayerHp")]
    public int PlayerHp { get; set; }

    [XmlElement("PlayerSpeed")]
    public float PlayerSpeed { get; set; }

    [XmlElement("ThrowingPower")]
    public float ThrowingPower { get; set; }
}
