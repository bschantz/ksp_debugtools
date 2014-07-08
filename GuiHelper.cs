using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using LogTools;
using ConfigTools;
using ImprovedAddonLoader;
#if DEBUG
using ResourceTools;
#endif
namespace GuiTools
{
#if DEBUG
    // intended only for debug use
    //[KSPAddonFixed(KSPAddon.Startup.Instantly, true, typeof(CreateUIAtlasInDebugMode))]
    internal class CreateUIAtlasInDebugMode : MonoBehaviour
    {
        void Start()
        {
            AtlasGenerator.CompileAtlasConfigs();
        }
    }
#endif



    internal class AtlasGenerator
    {
        private const string ATLAS_SRC_HEADER = "GUI_ATLAS_DEFINITION";
        private const string ATLAS_COMPILED_HEADER = "GUI_ATLAS_COMPILED";
        private const string ATLAS_EXT = "atlas";
        
        private const string BUTTON_HEADER = "BUTTON";
        
        private const int DEFAULT_MAX_SIZE = 1024;
        private const int DEFAULT_PADDING = 0;

        private const int CHUNK_SIZE = 1024;

        private class Atlas_Button
        {
            public ConfigNode node;

            public List<Texture2D> frames = new List<Texture2D>();

            internal Atlas_Button(ConfigNode n, Texture2D normal = null, Texture2D mouseover = null, Texture2D clicked = null)
            {
                node = n;

                if (normal != null) frames.Add(normal);
                if (mouseover != null) frames.Add(mouseover);
                if (clicked != null) frames.Add(clicked);
            }
        }

        private struct AtlasDescriptor
        {
            //byte[4]    magicNumber;

        }

        public static void CompileAtlasConfigs()
        {
            // search for all cfgs in our plugin dir or subdir
            var files = System.IO.Directory.GetFiles(ConfigUtil.GetDllDirectoryPath(), "*.cfg");
            Log.Debug("Found {0} configs in our plugin dir", files.Length);

            files.ToList().ForEach(file =>
            {
                ConfigNode node = ConfigNode.Load(file);

                if (node == null)
                {
                    Log.Write("Failed to load ConfigNode: {0}", file);
                }
                else
                {
                    //foreach (var n in node.GetNodes(ATLAS_SRC_HEADER))
                        //Log.Write("Atlas node: {0}", n.ToString());

                    // allow for multiple atlas definitions in one file
                    node.GetNodes(ATLAS_SRC_HEADER).ToList().ForEach(atlasCfg =>
                    {
                        string atlasName = ConfigUtil.ReadString(atlasCfg, "name", System.IO.Path.GetFileName(file)) + "." + ATLAS_EXT;
                        int maxSize = ConfigUtil.Parse(atlasCfg, "maxSize", DEFAULT_MAX_SIZE);
                        int padding = ConfigUtil.Parse(atlasCfg, "padding", DEFAULT_PADDING);

                        Log.Write("Building atlas '{0}' with maxSize {1} and padding {2}", atlasName, maxSize, padding);
                        //Log.Debug("Config = {0}", atlasCfg.ToString());

                        ConfigNode compiledResult = new ConfigNode();
                        ConfigNode compiledAtlas = compiledResult.AddNode(ATLAS_COMPILED_HEADER);

                        var buttons = new List<Atlas_Button>();

                        #region buttons

                        Log.Write("Found {0} button definitions in atlas '{1}'", atlasCfg.GetNodes(BUTTON_HEADER).Length, atlasName);

                        atlasCfg.GetNodes(BUTTON_HEADER).ToList().ForEach(btnCfg =>
                        {
                            //Log.Write("Button def: {0}", btnCfg.ToString());

                            var normal = LoadFromDisk(ConfigUtil.ReadString(btnCfg, "normal", string.Empty));
                            var mouseover = LoadFromDisk(ConfigUtil.ReadString(btnCfg, "mouseover", string.Empty));
                            var clicked = LoadFromDisk(ConfigUtil.ReadString(btnCfg, "clicked", string.Empty));
                            var name = ConfigUtil.ReadString(btnCfg, "name", string.Empty);

                            if (string.IsNullOrEmpty(name))
                            {
                                Log.Error("All buttons must be named!");
                            }
                            else if (normal == null)
                            {
                                Log.Error("Did not find a normal button image");
                            }
                            else
                            {
                                ConfigNode compiledButton = new ConfigNode(BUTTON_HEADER);
                                compiledButton.AddValue("name", name);
                                Log.Write("Adding button {0} to atlas {1}", name, atlasName);

                                buttons.Add(new Atlas_Button(compiledButton, normal, mouseover, clicked));
                            }
                        });
                        #endregion

                        // build atlas
                        var atlas = new Texture2D(maxSize, maxSize, TextureFormat.ARGB32, false);

                            // gather up button textures
                            var buttonTextures = new List<Texture2D>();

                            foreach (var button in buttons)
                                buttonTextures.AddRange(button.frames);


                        var uvs = atlas.PackTextures(buttonTextures.ToArray(), padding, maxSize);
                        
                        if (uvs == null)
                        {
                            Log.Error("UV packing failed for atlas {0}", atlasName);
                        }
                        else
                        {
                            Log.Write("Packed {0} textures into atlas {1}", uvs.Length, atlasName);

                            int index = 0;

                            foreach (var btn in buttons)
                            {
                                foreach (var frame in btn.frames)
                                {
                                    btn.node.AddValue("frame", uvs[index++].ToString());
                                }

                                compiledAtlas.AddNode(btn.node);
                            }

                            string compiledFilename = ConfigUtil.GetDllDirectoryPath() + "/" + atlasName;

                            Log.Write("CompiledResult = {0}", compiledResult);

                            compiledResult.Save(compiledFilename + ".cfg");//, System.IO.Path.GetFileNameWithoutExtension(compiledFilename));
                            Log.Write("Saved atlas '{0}'", compiledFilename);
                            atlas.SaveToDisk(System.IO.Path.GetFileName(compiledFilename) + ".png");

                        }
                    });
                }
            });
        }

        private static Texture2D LoadFromDisk(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return null;

            if (!filename.StartsWith("/")) filename = "/" + filename;
            filename = ConfigUtil.GetDllDirectoryPath() + filename; 

            
            Texture2D texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);

            if (File.Exists(filename))
            {
                var bytes = System.IO.File.ReadAllBytes(filename);
                texture.LoadImage(bytes);
                return texture;
            }
            else
            {
                Log.Error("Error creating atlas: filename '{0}' does not exist!");
            }
            return null;
        }
    }


    public class UI
    {
        private const int EZGUI_LAYER = 25;
        private const string EZGUI_CAMERA = "EZGUI Cam";


        /// <summary>
        /// Basic version; still needs a material and UV data
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static UIButton CreateButton(string name, Vector3 pos, float width, float height)
        {
            UIButton button = UIButton.Create(name, pos);
            button.gameObject.layer = EZGUI_LAYER;

            foreach (var cam in Camera.allCameras)
                if (string.Equals(cam.name, EZGUI_CAMERA))
                {
                    button.renderCamera = cam;
                    break;
                }
#if DEBUG
            if (button.renderCamera == null)
            {
                Log.Error("Alert: {0} does not have a render camera assigned", name);
            } else if ((button.renderCamera.cullingMask & (1 << EZGUI_LAYER)) == 0)
                Log.Error("Alert: {0} cullingMask does not include EzGUI layer {1}", EZGUI_CAMERA, EZGUI_LAYER);
#endif

            return button;
        }



        public static UIButton CreateButton(string name, Vector3 pos, float width, float height, Material mat)
        {
            var button = CreateButton(name, pos, width, height);
            button.SetMaterial(mat);

            return button;
        }



        //public static UIButton LoadButton(string 
    }
}
