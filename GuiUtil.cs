using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using ReeperCommon;


namespace ReeperCommon
{
    

    public class UvAtlas
    {
        private Dictionary<string, Rect> uvs = new Dictionary<string, Rect>();
        public delegate string AtlasFrameFunc(int frame);
        private int atlasWidth = 0;
        private int atlasHeight = 0;

        public static UvAtlas LoadFromResource(string resource, int width, int height)
        {
            var atlas = new UvAtlas(width, height);

            var stream = ResourceUtil.GetEmbeddedContentsStream(resource);
            if (stream == null)
            {
                Log.Error("Failed to load '{0}' from resource", resource);
            }
            else
            {
                atlas.LoadFromStream(stream);
            }

            return atlas;
        }


        public static UvAtlas LoadFromDllDir(string pathRelToDll, int width, int height)
        {
            var atlas = new UvAtlas(width, height);

            if (pathRelToDll.StartsWith("/")) pathRelToDll = pathRelToDll.Substring(1);

            string path = ConfigUtil.GetDllDirectoryPath() + pathRelToDll;
            

            if (!File.Exists(path))
            {
                Log.Error("UvAtlas '{0}' not found", path);
            }
            else
            {
                Log.Verbose("Loading UvAtlas '{0}'", path);
                atlas.LoadFromStream(new System.IO.FileStream(path, FileMode.Open));
            }

            return atlas;
        }


        private UvAtlas(int width, int height)
        {
            atlasWidth = width;
            atlasHeight = height;
        }

        private void LoadFromStream(Stream stream)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(stream);
            string line;

            while ((line = file.ReadLine()) != null)
                ParseLine(line);

            file.Close();

        }

        private bool ParseLine(string line)
        {
            // value = x y w h
            // normal = 117 351 38 38

            int eq = line.LastIndexOf('=');

            string name = line.Substring(0, eq - 1);
            string nums = line.Substring(eq + 1);

            if (uvs.ContainsKey(name))
            {
                Log.Error("UvAtlas: key '{0}' is duplicate", name);
                return false;
            }

            Log.Debug("name = {0}", name);
            Log.Debug("nums = {0}", nums);

            var split = nums.Trim().Split(' ');

            if (split.Length == 4)
            {
                try
                {
                    int x = int.Parse(split[0]);
                    int y = int.Parse(split[1]);
                    int w = int.Parse(split[2]);
                    int h = int.Parse(split[3]);

                    uvs.Add(name.Trim(), new Rect(x, y, w, h));
                    return true;

                }catch (Exception e)
                {
                    Log.Error("Exception occurred while splitting '{0}': {1}", nums, e);
                }
            }
            else
            {
                Log.Error("Failed to split " + nums);
            }

            return false;
        }

        public bool Good
        {
            get { return uvs.Keys.Count > 0; }
        }

        public Rect GetUV(string name)
        {
            if (!uvs.ContainsKey(name))
            {
                Log.Error("UvAtlas does not contain key '{0}'", name);
                return new Rect();
            }
            else
            {
                var pixelRect = uvs[name];

                //Rect adjusted = pixelRect;

                return new Rect()
                {
                    x = (float)pixelRect.x / atlasWidth,
                    y = 1f - (float)pixelRect.height / atlasHeight - (float)pixelRect.y / atlasHeight,
                    width = (float)pixelRect.width / atlasWidth,
                    height = (float)pixelRect.height / atlasHeight
                };
            }
        }

        public UVAnimation LoadAnimation(int frameCount, AtlasFrameFunc formatter)
        {
            UVAnimation anim = new UVAnimation();

            SPRITE_FRAME[] frames = new SPRITE_FRAME[frameCount];

            for (int i = 0; i < frameCount; ++i)
            {
                frames[i] = new SPRITE_FRAME(0);
                frames[i].uvs = GetUV(formatter(i));
            }

            anim.SetAnim(frames);
            return anim;
        }


    }


    public static class GuiUtil
    {
        private const int EZGUI_LAYER = 25;
        private const float EZGUI_DEPTH = 90f;

        //public static Texture2D CreateAtlas(int width, int height, out ConfigNode node, List<Texture2D> textures, int padding = 0)
        //{
        //    node = new ConfigNode("ATLAS_UV_DATA");

        //    if (textures.Count == 0)
        //    {
        //        Log.Error("GuiUtil.CreateAtlas: no textures provided");
        //        return null;
        //    }

        //    var texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        //    var rects = texture.PackTextures(textures.ToArray(), padding);

        //    if (rects == null || rects.Length == 0)
        //    {
        //        Log.Error("GuiUtil: CreateAtlas failed to generate any textures");
        //        return null;
        //    }

        //    for (int i = 0; i < rects.Length; ++i)
        //        node.AddValue(i.ToString(), rects[i].AsVector().ToString());

        //    return texture;
        //}



        /// <summary>
        /// Retrieve the first camera that renders EzGui GUI elements
        /// </summary>
        /// <returns></returns>
        public static Camera GetCamera(string name)
        {
            foreach (var c in Camera.allCameras)
                if (c.name == name)
                    return c;

            return null;
        }

        public static Camera GetGuiCamera()
        {
            return GetCamera("EZGUI Cam") ?? GetCamera("UI camera"); ;
        }


        //public static Vector2 PixelCoordToUVCoord(Texture texture, Vector2 xy)
        //{
        //    return new Vector2(xy.x / ((float)texture.width - 1), 1.0f - (xy.y / ((float)texture.height - 1)));
        //}

        //public static Vector2 PixelSpaceToUVSpace(Texture texture, Vector2 xy)
        //{
        //    return new Vector2(xy.x / ((float)texture.width), xy.y / ((float)texture.height));
        //}

        public static Vector2 PixelCoordToUVCoord(Texture texture, Vector2 xy)
        {
            return new Vector2(xy.x / ((float)texture.width), 1.0f - (xy.y / ((float)texture.height)));
        }

        public static Vector2 PixelSpaceToUVSpace(Texture texture, Vector2 xy)
        {
            return new Vector2(xy.x / ((float)texture.width), xy.y / ((float)texture.height));
        }

        public static Vector2 PixelCoordToUVCoord(this Vector2 xy, Texture texture)
        {
            return PixelCoordToUVCoord(texture, xy);
        }

        public static Vector2 PixelSpaceToUVSpace(this Vector2 xy, Texture texture)
        {
            return PixelSpaceToUVSpace(texture, xy);
        }

        /// <summary>
        /// Basic version; still needs a material and UV data
        /// </summary>
        /// <param name="name"></param>
        /// <param name="screenPos"></param>
        /// <returns></returns>
        //public static UIButton CreateButton(string name)
        //{

        //    UIButton button = UIButton.Create(name, Vector3.zero);
        //    button.gameObject.layer = LayerMask.NameToLayer("EzGUI_UI");
        //    button.plane = SpriteRoot.SPRITE_PLANE.XY;
        //    button.SetAnchor(SpriteRoot.ANCHOR_METHOD.MIDDLE_CENTER);
        //    button.renderCamera = GetEzGuiCamera();

        //    return button;
        //}



        //public static UIButton LoadButton(string 



        public static UIButton CreateBlocker(Rect screenRect, float zPos = float.NaN, string name = "GuiUtil.Blocker")
        {

            var blocker = UIButton.Create("ButtonTest.Blocker", Vector3.zero);
            //var uiCamera = Camera.allCameras.ToList().Find(c => c.name == "EZGUI Cam");
            var uiCamera = GetGuiCamera();

            if (uiCamera == null)
            {
                Log.Error("GuiUtil.CreateBlocker: failed to find GUI camera!");
                Log.Write("... begin list");
                Camera.allCameras.ToList().ForEach(c => Log.Write("Camera: {0}", c.name));
                Log.Write("... end list");

                return null;
            }

            blocker.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
            blocker.renderCamera = uiCamera;
            blocker.gameObject.layer = LayerMask.NameToLayer("EzGUI_UI");

            var texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
#if DEBUG
            texture.SetPixels(new Color[] { new Color(1f, 0f, 1f, .5f) });
#else
            texture.SetPixels(new Color[] { Color.clear });
#endif
            texture.Apply();

            if (float.IsNaN(zPos)) zPos = UIManager.instance.transform.position.z;

            blocker.transform.position = new Vector3(screenRect.x, screenRect.y, zPos);
            blocker.Setup(screenRect.width, screenRect.height, new Material(Shader.Find("Sprite/Vertex Colored")) { mainTexture = texture });


            RepositionButton(blocker, screenRect);

            return blocker;
        }



        public static void RepositionButton(UIButton button, Vector2 screenPos)
        {
            button.transform.position = new Vector3(
                UIManager.instance.transform.position.x + screenPos.x - Screen.width * 0.5f,
                UIManager.instance.transform.position.y - screenPos.y + Screen.height * 0.5f,
                button.transform.position.z);
        }

        public static void RepositionButton(UIButton button, Rect screenRect)
        {
            button.Setup(screenRect.width, screenRect.height);
            RepositionButton(button, new Vector2(screenRect.x, screenRect.y));
        }

        public static void Reposition(this UIButton button, Rect screenRect)
        {
            RepositionButton(button, screenRect);
        }

        public static void Reposition(this UIButton button, Vector2 screenPos)
        {
            RepositionButton(button, screenPos);
        }

        public static void Move(this UIButton button, Rect r)
        {
            RepositionButton(button, r);
        }

        public static void Move(this UIButton button, Vector2 pos)
        {
            RepositionButton(button, pos);
        }
    }
}
