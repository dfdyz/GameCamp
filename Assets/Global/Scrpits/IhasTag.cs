using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ǩϵͳ�����ڴ�������
/// </summary>
[AddComponentMenu("Utils/Tags")]
public class IhasTag : MonoBehaviour
{
    //��ǩϵͳ�����ڶ����������뽻��
    //����MC��NBTϵͳ
    //ʹ�ó������洢����Ѫ����

    //��ʼ����ǩֵ �ַ����б���ʹ�� ':'
    //��ʽ   ����(int/float/string/bool):��ǩ��:ֵ
    //ʾ��: int a = 114514;  ->  int:a:114514
    [SerializeField]
    private string[] Initializations;

    private Hashtable tags = new Hashtable();
    public void putInt(string key, int value)
    {
        tags["int:" + key] = value;
    }
    public void putFloat(string key, float value)
    {
        tags["float:" + key] = value;
    }
    public void putString(string key, string value)
    {
        tags["string:" + key] = value;
    }
    public void putBool(string key, bool value)
    {
        tags["bool:" + key] = value;
    }
    public int getInt(string key)
    {
        object v = tags["int:" + key];
        if (v == null) return 0;
        return (int)v;
    }
    public float getFloat(string key)
    {
        object v = tags["float:" + key];
        if (v == null) return 0f;
        return (float)v;
    }
    public string getString(string key)
    {
        object v = tags["string:" + key];
        if (v == null) return "";
        return (string)v;
    }
    public bool getBool(string key)
    {
        object v = tags["bool:" + key];
        if (v == null) return false;
        return (bool)v;
    }

    public bool hasVar(string type,string key)
    {
        return tags[type + ":" + key] != null;
    }

    public override string ToString()
    {
        string str = "";
        foreach(object o in tags.Keys)
        {
            if(o.GetType() == typeof(string))
            {
                string[] v = o.ToString().Split(':');
                str += v[0] + " " + v[1] + " = "+tags[o].ToString()+"\n";
            }
        }
        return str;
    }
    private void Interpret()
    {
        foreach(string str in Initializations)
        {
            if(str.Length > 0)
            {
                string[] args = str.Split(':',' ');
                switch (args[0])
                {
                    
                    case "int":
                        putInt(args[1], Convert.ToInt32(args[2]));
                        break;
                    case "float":
                        putFloat(args[1], (float)Convert.ToDouble(args[2]));
                        break;
                    case "string":
                        putString(args[1], args[2]);
                        break;
                    case "bool":
                        putBool(args[1], args[2] =="true" ? true:false);
                        break;
                }
            }
        }
    }

    public string Serialize()
    {
        string str = "";
        foreach (object o in tags.Keys)
        {
            if (o.GetType() == typeof(string))
            {
                string[] v = o.ToString().Split(':');
                str += v[0] + ':' + v[1] + ':' + tags[o].ToString() + "\n";
            }
        }
        return str;
    }

    public void Unserialize(string strs)
    {
        string[] s = strs.Split('\n');
        foreach (string str in Initializations)
        {
            if (str.Length > 0 && str != "")
            {
                string[] args = str.Split(':', ' ');
                switch (args[0])
                {

                    case "int":
                        putInt(args[1], Convert.ToInt32(args[2]));
                        break;
                    case "float":
                        putFloat(args[1], (float)Convert.ToDouble(args[2]));
                        break;
                    case "string":
                        putString(args[1], args[2]);
                        break;
                    case "bool":
                        putBool(args[1], args[2] == "true" ? true : false);
                        break;
                }
            }
        }
    }


    void Awake()
    {
        Interpret();
    }
}
