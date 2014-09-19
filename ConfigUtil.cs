﻿/******************************************************************************
This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
 * ***************************************************************************/
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ReeperCommon
{
    public static class ConfigUtil
    {
        public static T ParseEnum<T>(this ConfigNode node, string valueName, T defaultValue)
        {
            try
            {
                var value = node.GetValue(valueName);

                if (string.IsNullOrEmpty(value))
                {
                    Log.Error("Settings: Value enum '{0}' does not exist in given ConfigNode", valueName);
                    return defaultValue;
                }

                var values = Enum.GetValues(typeof(T));

                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch (Exception e)
            {
                Log.Error("Settings: Failed to parse value '{0}' from ConfigNode, resulted in an exception {1}", valueName, e);
            }

            return defaultValue;
        }

        public static string Parse(this ConfigNode node, string valueName, string defaultValue = "")
        {
            try
            {
                if (!node.HasValue(valueName))
                    return defaultValue;

                return node.GetValue(valueName);
            } catch (Exception e)
            {
                Log.Error("Settings: Failed to parse string value '{0}' from ConfigNode, resulted in an exception {1}", valueName, e);
            }

            return defaultValue;
        }

        public static T Parse<T>(this ConfigNode node, string valueName, T defaultValue)
        {
            try
            {
                if (!node.HasValue(valueName))
                {
                    Log.Error("Settings: Value '{0}' does not exist in given ConfigNode", valueName);
                    return defaultValue;
                }

                var value = node.GetValue(valueName);

                var method = typeof(T).GetMethod("TryParse", new[] {
                    typeof (string),
                    typeof(T).MakeByRefType()
                });

                if (method == null)
                {
                    Log.Error("Failed to locate TryParse in {0}", typeof(T).FullName);
                }
                else
                {
                    object[] args = new object[] { value, default(T) };

                    if ((bool)method.Invoke(null, args))
                    {
                        //Log.Debug("Examined {0}, parse returned {1}", value, args[1]);
                        return (T)args[1];
                    }
                    else
                    {
                        Log.Error("Settings: TryParse failed with node name '{0}' (returned value '{1}'", valueName, string.IsNullOrEmpty(valueName) ? "[null]" : value);
                        return defaultValue;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Settings: Failed to parse value '{0}' from ConfigNode, resulted in an exception {1}", valueName, e);
            }

            return defaultValue;
        }

        public static string ReadString(this ConfigNode node, string valueName, string defaultValue = "")
        {
            if (node == null || !node.HasValue(valueName)) return defaultValue;

            return node.GetValue(valueName);
        }

        //public static void Set(this ConfigNode node, string valueName, string value)
        //{
        //    if (!node.SetValue(valueName, value)) node.SetValue(valueName, value);
        //}

        //public static bool ParseRect(string strRect, out Rect rect)
        //{
        //    rect = new Rect();

        //    // format: (x:0.00, y:0.00, width:0.25, height:0.25)
        //    if (!strRect.StartsWith("Rect("))
        //    {
        //        Log.Error("ParseRect: '{0}' does not appear to be a rect string", strRect);
        //    }

        //    return true;
        //}

        /// <summary>
        /// Returns an absolute path to the directory this DLL resides in
        /// ex: C:/program files (x86)/steam/steamapps/common/kerbal space program/GameData/NavBallTextureExport
        /// </summary>
        /// <returns></returns>
        public static string GetDllDirectoryPath()
        {
            //string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            //UriBuilder uri = new UriBuilder(codeBase);
            //string path = Uri.UnescapeDataString(uri.Path);
            //return Path.GetDirectoryName(path);
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }



        /// <summary>
        /// This function, given a directory path, will attempt to find a Uri relative
        /// to GameData that will be appropriate to use for the GetTexture function.
        /// 
        /// ex: given C:/program files (x86)/steam/steamapps/common/kerbal space program/GameData/NavBallTextureExport
        /// result would be "NavBallTextureExport" as its parent is GameData
        /// 
        /// if instead it were given "GameData/foo/bar", it would return "foo/bar"
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetRelativeToGameData(string path)
        {
            // just in case the path we're given doesn't actually
            // contain a GameData folder
            if (!path.Contains("GameData"))
            {
                Debug.LogError(string.Format("GetRelativeToGameData: Given path '{0}' does not reside in GameData.  The plugin does not appear to be installed correctly.", path));
                throw new FormatException(string.Format("GetRelativeToGameData: path '{0}' does not contain 'GameData'", path));
            }

            int index = path.IndexOf("GameData");

            string relative = "";

            if (path.Length > index + "GameData".Length + 1)
                relative = path.Substring(index + "GameData".Length + 1);

            Log.Debug(string.Format("Relative path: {0}", relative));

            return relative;
        }


        public static Rect ReadRect(this ConfigNode node, string name, Rect defaultValue = new Rect())
        {
            if (!node.HasValue(name))
            {
                Log.Error("ConfigUtil.ReadRect: value '{0}' does not exist", name);
            }
            else
            {
                try
                {
                    var parsed = KSPUtil.ParseVector4(node.GetValue(name));
                    return new Rect(parsed.x, parsed.y, parsed.z, parsed.w);
                } catch (Exception e)
                {
                    Log.Error("ConfigUtil.ReadRect: exception while reading value '{0}': {1}", name, e);
                }
            }

            return defaultValue;
        }


        public static Vector4 AsVector(this UnityEngine.Rect rect)
        {
            return new Vector4(rect.x, rect.y, rect.width, rect.height);
        }

        // Works but turned out to be unnecessary
        //public static void LoadFieldsFromConfig(this BaseFieldList fields, ConfigNode node)
        //{
        //    for (int i = 0; i < fields.Count; ++i)
        //    {
        //        BaseField field = fields[i];

        //        if (node.HasValue(field.name))
        //        {
        //            Log.Write("type of {0} = {1}", field.name, field.FieldInfo.FieldType.Name);
        //            var ft = field.FieldInfo.FieldType;

        //            try {
        //                Log.Debug("Creating params");
        //                object[] methodParams = new object[] { node, field.name, Activator.CreateInstance(ft) };

        //                Log.Debug("Invoking method");

        //                var parseMethod = typeof(ConfigUtil).GetMethod("Parse");
        //                if (parseMethod == null) Log.Error("Failed to find parse method!");

        //                var parseT = parseMethod.MakeGenericMethod(ft);
        //                if (parseT == null) Log.Error("Failed to create generic method!");

        //                var result = typeof(ConfigUtil).GetMethod("Parse").MakeGenericMethod(ft).Invoke(null, methodParams);

        //                Log.Debug("Setting value");
        //                field.SetValue(result, field.host);
        //                field.SetOriginalValue();

        //                Log.Write("Set '{0}' to value '{1}'", field.name, result.ToString());
        //            } 
        //            catch (Exception e)
        //            {
        //                Log.Error("Exception occurred in LoadFieldsFromConfig: {0}", e);
        //            }
        //        }
        //        else Log.Debug("LoadFieldsFromConfig: Node does not have a value for '{0}'", field.name);
        //    }
        //}

    }
}
