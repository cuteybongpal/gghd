using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class DataManager
{
    public static GameSettings Setting;
    public static GameData GameData = new GameData();
    
    public void Init()
    {
        Setting = ReadData<GameSettings>("GameSetting.data");
        GameData.FrogData = ReadData<FrogData>("FrogData.data");
        GameData.GunData = ReadData<GunData>("GunData.data");
        GameData.SlimeData = ReadData<SlimeData>("SlimeData.data");
    }
    T ReadData<T>(string key) where T : class
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        using (StringReader stringReader = new StringReader(CommandManager.Instance.ExecuteCommand<TextAsset>(new Load(key)).ToString()))
        {
            T data = (T)serializer.Deserialize(stringReader);
            return data as T;
        }
    }
}
public class GameData
{
    public FrogData FrogData;
    public SlimeData SlimeData;
    public GunData GunData;
}
[XmlRoot("setting")]
public class GameSettings
{
    [XmlElement("gravityScale")]
    public float GravityScale { get; set; }

    [XmlElement("ThrowingPower")]
    public float ThrowingPower { get; set; }

    [XmlElement("HatThrowingPower")]
    public float HatThrowingPower { get; set; }

    [XmlElement("Volume")]
    public float Volume;
    [XmlElement("RewindCooldown")]
    public float RewindCooldown;
}
[XmlRoot("Frog")]
public class FrogData
{
    [XmlElement("Speed")]
    public float Speed;
    [XmlElement("MaxJumpPower")]
    public float MaxJumpPower;
    [XmlElement("TongueSpeed")]
    public float TongueSpeed;
    [XmlElement("TongueRange")]
    public float TongueRange;
    [XmlElement("AbillityCooldown")]
    public float AbillityCooldown;
}
[XmlRoot("Slime")]
public class SlimeData
{
    [XmlElement("Speed")]
    public float Speed;
    [XmlElement("JumpPower")]
    public float JumpPower;
    [XmlElement("ThrowingCooldown")]
    public float ThrowingCooldown;
}
[XmlRoot("Gun")]
public class GunData
{
    [XmlElement("Speed")]
    public float Speed;
    [XmlElement("JumpPower")]
    public float JumpPower;
    [XmlElement("ShotCoolDown")]
    public float ShotCoolDown;
    [XmlElement("ShotSpeed")]
    public float ShotSpeed;
    [XmlElement("BulletRange")]
    public float BulletRange;

}
