using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;

[LuaCallCSharp]
public class FuncLuaBehavior : MonoBehaviour {
    public string luaFile;
    public Injection[] injections;

    internal static LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!

    LuaTable dataTable; // lua数据，包括C#的userdata和lua自定义的数据

    LuaTable funcTable; // lua实现的事件响应函数和功能函数

    public LuaTable luaData
    {
        get { return dataTable; }
    }

    void Awake()
    {
        luaEnv.DoString(string.Format("require '{0}'", luaFile ));


        dataTable = luaEnv.NewTable();
        dataTable.Set("gameObject", gameObject);
        foreach (var injection in injections)
        {
            dataTable.Set(injection.name, injection.value);
        }

        funcTable = luaEnv.Global.Get<LuaTable>(luaFile);
        if (funcTable != null)
        {
            var luaAwake = funcTable.Get<Action<LuaTable>>("Awake");
            if (luaAwake != null)
            {
                luaAwake(dataTable);
            }
        }
    }

    // Use this for initialization
    void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDestroy()
    {
        dataTable.Dispose();
        if (funcTable != null)
        {
            funcTable.Dispose();
        }
    }
}
