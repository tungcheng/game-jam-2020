using System;
using System.Collections;
using System.Collections.Generic;

public static class Pool
{
    static Dictionary<Type, object> m_Dict = new Dictionary<Type, object>();

    static Pool()
    {
    }

    public static void Set<T>(T t) where T : class, new()
    {
        var type = typeof(T);
        m_Dict[type] = t;
    }

    public static T Get<T>() where T : class, new()
    {
        var type = typeof(T);
        if (!m_Dict.ContainsKey(type))
        {
            m_Dict[type] = new T();
        }
        return m_Dict[type] as T;
    }
}