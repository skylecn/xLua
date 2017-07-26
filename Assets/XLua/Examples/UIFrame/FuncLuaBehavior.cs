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

    LuaTable dataTable; // Lua table，包括C#创建的和Lua中创建的数据

    LuaTable funcTable; // Lua实现的事件响应函数和功能函数

    public LuaTable luaData
    {
        get { return dataTable; }
    }

    void Awake()
    {
        //加载Lua文件
        luaEnv.DoString(string.Format("require '{0}'", luaFile ));

        //定义dataTable
        dataTable = luaEnv.NewTable();
        //插入需要处理的UI组件到dataTable中
        dataTable.Set("gameObject", gameObject);
        foreach (var injection in injections)
        {
            dataTable.Set(injection.name, injection.value);
        }

        //获取lua文件中函数table的引用
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
