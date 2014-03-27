/******************************************************************************
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
using System.Collections.Generic;
using System.Text;
using DebugTools;
using UnityEngine;

namespace ResourceTools
{
    public static class ResourceUtil
    {
        /// <summary>
        /// Saves texture into plugin dir with supplied name.
        /// Precondition: texture must be readable
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="name"></param>
        public static bool SaveToDisk(this UnityEngine.Texture2D texture, string pathInGameData)
        {
            if (pathInGameData.StartsWith("/"))
                pathInGameData = pathInGameData.Substring(1);

            pathInGameData = "/GameData/" + pathInGameData;

            if (!pathInGameData.EndsWith(".png"))
                pathInGameData += ".png";

            try
            {
                Log.Verbose("Saving a {0}x{1} texture as '{2}'", texture.width, texture.height, pathInGameData);

                System.IO.FileStream file = new System.IO.FileStream(KSPUtil.ApplicationRootPath + pathInGameData, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                System.IO.BinaryWriter writer = new System.IO.BinaryWriter(file);
                writer.Write(texture.EncodeToPNG());
                writer.Close();
                file.Close();

                Log.Verbose("Texture saved as {0} successfully.", pathInGameData);
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Failed to save texture '{0}' due to {1}", pathInGameData, e);
                return false;
            }
        }



        /// <summary>
        /// Retrieves a texture embedded in the DLL, given its resource url
        /// Returns null if failed
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static Texture2D GetEmbeddedTexture(string resource, bool compress = false, bool mip = false)
        {
            var input = GetEmbeddedContentsStream(resource);

            if (input == null)
            {
                Log.Error("Failed to locate embedded texture '{0}'", resource);
                return null;
            }
            else
            {
                // success!  now to turn the string string into a usable
                // texture
                byte[] buffer = new byte[16 * 1024];
                int read = 0;
                var ms = new MemoryStream();

                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);

                Texture2D texture = new Texture2D(1, 1, compress ? TextureFormat.DXT5 : TextureFormat.ARGB32, mip);

                if (texture.LoadImage(ms.ToArray()))
                {
                    Log.Verbose("Loaded embedded texture '{0}', dimensions {1}x{2}", resource, texture.width, texture.height);
                    return texture;
                }
                else
                {
                    Log.Error("GetEmbeddedTexture: Failed to create Texture2D out of {0} bytes", ms.ToArray().Length);
                }
            }

            return null;
        }



        /// <summary>
        /// Get the contents of a resource embedded in the running DLL given its
        /// resource url
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static bool GetEmbeddedContents(string resource, out string contents)
        {
            contents = string.Empty;

            try
            {
                var stream = GetEmbeddedContentsStream(resource);

                if (stream != null)
                {
                    var reader = new System.IO.StreamReader(stream);

                    if (reader != null)
                    {
                        contents = reader.ReadToEnd();

                        return contents.Length > 0;
                    } 
                }
            } catch (Exception e)
            {
                Log.Error("GetEmbeddedContents: {0}", e);
            }

            return false;
        }



        public static Stream GetEmbeddedContentsStream(string resource)
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
        }



        public static Material LocateMaterial(string resourceName, params string[] backups)
        {
            var possibilities = new Dictionary<string /* resource name */, string /* what to display in log */>();
            possibilities.Add(resourceName, string.Format("LocateMaterial: Creating shader material '{0}'", resourceName));

            foreach (var backup in backups)
                possibilities.Add(backup, string.Format("LocateMaterial: Attempting backup shader '{0}'", backup));

            foreach (var resource in possibilities)
            {
                Log.Verbose(resource.Value);

                // check embedded resources
                //var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resource.Key);

                string contents;

                if (GetEmbeddedContents(resource.Key, out contents))
                {
                    return new Material(contents);

                    // found in our resources.  Extract and create
                    //try
                    //{
                    //    return new Material(new System.IO.StreamReader(stream).ReadToEnd());
                    //}
                    //catch (Exception e)
                    //{
                    //    Log.Error("LocateMaterial: Failed to generate '{0}', although it was found!  Exception: {1}", resource.Key, e);
                    //    // bad news!  continue looking
                    //}
                }
                else
                {
                    // not in our resources.  Check Shader lib
                    var shader = Shader.Find(resource.Key);

                    if (shader != null)
                    {
                        try
                        {
                            return new Material(shader);
                        }
                        catch (Exception e)
                        {
                            Log.Error("Failed to create material with non-embedded shader '{0}'!  Exception: {1}", resource.Key, e);
                        }
                    }
                    else
                    {
                        Log.Error("LocateMaterial: Failed to find '{0}'.  Proceeding to next fallback.", resource.Key);
                    }
                }
            }

            // if we manage to get to this point, no appropriate material was found at all!
            Log.Error("LocateMaterial: Failed to find any appropriate shader!");
            return null;
        }
    }
}
