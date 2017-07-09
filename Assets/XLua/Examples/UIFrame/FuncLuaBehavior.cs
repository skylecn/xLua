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

    LuaTable dataTable;

    LuaTable funcTable;

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
