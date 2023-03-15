//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;
using SimpleJSON;



namespace cfg.Map
{ 

public sealed partial class MapCfg :  Bright.Config.BeanBase 
{
    public MapCfg(JSONNode _json) 
    {
        { if(!_json["id"].IsNumber) { throw new SerializationException(); }  Id = _json["id"]; }
        { if(!_json["name"].IsString) { throw new SerializationException(); }  Name = _json["name"]; }
        { if(!_json["type"].IsString) { throw new SerializationException(); }  Type = _json["type"]; }
        { if(!_json["isCopyMap"].IsNumber) { throw new SerializationException(); }  IsCopyMap = _json["isCopyMap"]; }
        { if(!_json["desc"].IsString) { throw new SerializationException(); }  Desc = _json["desc"]; }
        PostInit();
    }

    public MapCfg(int id, string name, string type, int isCopyMap, string desc ) 
    {
        this.Id = id;
        this.Name = name;
        this.Type = type;
        this.IsCopyMap = isCopyMap;
        this.Desc = desc;
        PostInit();
    }

    public static MapCfg DeserializeMapCfg(JSONNode _json)
    {
        return new Map.MapCfg(_json);
    }

    /// <summary>
    /// 这是id
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// 类型
    /// </summary>
    public string Type { get; private set; }
    /// <summary>
    /// 类型
    /// </summary>
    public int IsCopyMap { get; private set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; private set; }

    public const int __ID__ = 618094202;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "Name:" + Name + ","
        + "Type:" + Type + ","
        + "IsCopyMap:" + IsCopyMap + ","
        + "Desc:" + Desc + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}
