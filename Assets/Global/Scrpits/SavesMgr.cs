using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Global.Scrpits
{
    public static class SavesMgr
    {
        public static bool StartSave = false;
        public static string readStr(string file)
        {
            string Path = Application.dataPath + "/" + file;
            try
            {
                using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    var bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, (int)fs.Length);
                    string Data = Encoding.UTF8.GetString(bytes);
                    fs.Flush();
                    fs.Dispose();
                    fs.Close();
                    Debug.Log(Path + "\n\n" + Data);
                    return Data;
                }
                
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Debug.LogError("文件异常：" + Path);
                return string.Empty;
            }
        }

        public static void saveStr(string file,string str)
        {
            string Path = Application.dataPath + "/" + file;
            try
            {
                using (FileStream fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    Debug.Log(Path);
                    fs.Seek(0, SeekOrigin.Begin);
                    byte[] bytes = Encoding.UTF8.GetBytes(str);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }

            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Debug.LogError("文件异常：" + Path);
            }
        }


    }
}
