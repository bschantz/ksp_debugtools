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
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using ImprovedAddonLoader;
using ConfigTools;
using LogTools;
using DebugTools;
using ResourceTools;


namespace DebugTools
{
   


    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class GuiTest : MonoBehaviour
    {
        UIButton button;
        UIButton clone;

        void Start()
        {
            var exit = EditorLogic.fetch.exitBtn;

            Log.Write("editor btn position: {0}, local {1}", exit.transform.position, exit.transform.localPosition);
            Log.Write("dimensions: {0}, {1}", exit.width, exit.height);
            Log.Write("scale: {0}", exit.transform.localScale);
            Log.Write("animations: {0}", exit.animations.Length);
            Log.Write("uvs: {0}", exit.GetUVs().ToString());

            if (exit.gameObject.transform.parent != null)
            {
                Log.Error("exit has parent!");
                GameObject parent = exit.transform.parent.gameObject;

                while (parent.transform.parent != null)
                    parent = parent.transform.parent.gameObject;

                //parent.PrintComponents();

            }
            //exit.gameObject.PrintComponents();
            
            if (exit.Managed)
            {
                Log.Write("is managed");
            }

            //Log.Write("second btn position: {0}", EditorLogic.fetch.flagBrowserButton.transform.position);

            string str;

            button = UIButton.Create("TestButton", new Vector3(-100f, 800f, 90f));
            button.enabled = true;
            button.gameObject.layer = exit.gameObject.layer;
            Log.Write("exit layer: {0}", exit.gameObject.layer);
            Log.Write("cam layer: {0}", exit.renderCamera.cullingMask);

            foreach (var c in Camera.allCameras)
                Log.Write("camera name: {0}, ortho = {1}", c.name, c.orthographic);


            button.Setup(128f, 128f, exit.GetPackedMaterial(out str));
            button.RenderCamera = exit.renderCamera;

            Log.Write("our components:");
            button.gameObject.PrintComponents();
            button.SetDrawLayer(exit.drawLayer);
            button.SetAnchor(SpriteRoot.ANCHOR_METHOD.MIDDLE_CENTER);
            button.SetPlane(exit.plane);

            button.gameObject.SetActive(true);
            button.Hide(false);
            button.InitUVs();

            button.AddValueChangedDelegate(delegate(IUIObject obj) { Log.Write("ezbutton click!"); });


            // above works!


            int normalState = 0;
            Rect firstTextureUV = exit.GetUVs();

            CSpriteFrame frame = new CSpriteFrame();
            frame.uvs = firstTextureUV;
            button.animations[normalState].SetAnim(new SPRITE_FRAME[] { frame.ToStruct() });

            frame.uvs = new Rect(0f, 0f, 0.25f, 0.25f);
            button.animations[1].SetAnim(new SPRITE_FRAME[] { frame.ToStruct() });

            var temp = exit.GetUVs();
            frame.uvs = new Rect(temp.x + temp.width, temp.y + temp.height, temp.width, temp.height);
            button.animations[2].SetAnim(new SPRITE_FRAME[] { frame.ToStruct() });

            var mat = new Material(Shader.Find("KSP/Diffuse")) { mainTexture = button.renderer.sharedMaterial.mainTexture };
            button.renderer.sharedMaterial = mat;

            button.SetPixelToUV(button.renderer.sharedMaterial.mainTexture);
            button.SetCamera(EditorLogic.fetch.exitBtn.renderCamera);

            Log.Write("cam ortho? {0}", exit.renderCamera.orthographic);
            Log.Write("wxh: {0}x{1}", exit.renderCamera.GetScreenWidth(), exit.renderCamera.GetScreenHeight());

            // works ;\
            clone = ((GameObject)GameObject.Instantiate(exit.gameObject)).GetComponent<UIButton>();
            clone.transform.position = new Vector3(0f, 800f, 90f);

        }

        void Update()
        {
            EditorLogic.fetch.exitBtn.transform.position = new Vector3(EditorLogic.fetch.exitBtn.transform.position.x - 10f * Time.deltaTime, EditorLogic.fetch.exitBtn.transform.position.y - 10f * Time.deltaTime, EditorLogic.fetch.exitBtn.transform.position.z);
            //Log.Write("editor btn position: {0}", EditorLogic.fetch.exitBtn.transform.position);
        }
    }


    

    //[KSPAddon(KSPAddon.Startup.EditorAny, false)]
    //public class ClickthroughTest : MonoBehaviour
    //{
    //    UIButton myButton;

    //    void Start()
    //    {
    //        myButton = UIButton.Create("Clickthrough Test", Vector3.zero );
    //        myButton.scriptWithMethodToInvoke = this;
    //        myButton.methodToInvoke = "ClickthroughButton";
    //        myButton.width = 128f;
    //        myButton.height = 128f;
    //        myButton.gameObject.layer = 25;
    //        myButton.gameObject.SetActive(true);
    //        myButton.drawLayer = 0;
    //        myButton.Setup(128f, 128f);
            
            

    //        var btn = EditorLogic.fetch.actionPanelBtn;
    //        myButton.SetMaterial(new Material(Shader.Find("Diffuse")));
    //        myButton.dropMask = btn.dropMask;
    //        //UIManager.instance.
    //        myButton.SetPlane(btn.plane);
    //        myButton.renderCamera = btn.renderCamera;

    //        Log.Write("autosize = {0}", btn.autoResize);

    //        Log.Write("billboarded = {0}", btn.billboarded);
    //        Log.Write("drawLayer = {0}", btn.drawLayer);

    //        Log.Write("Action button layer: {0}", btn.gameObject.layer);

    //        foreach (var l in btn.layers)
    //            Log.Write("Another layer: {0}", l.drawLayer);

    //        btn.gameObject.PrintComponents();
            
    //    }

    //    public void ClickthroughButton()
    //    {
    //        Log.Write("Clickthrough button pressed!");
    //    }
    //}



    //[KSPAddon(KSPAddon.Startup.Flight, false)]
//    public class ScatterMachine : MonoBehaviour
//    {
//        void OnGUI()
//        {
//            if (FlightGlobals.ready)
//                if (FlightGlobals.ActiveVessel != null)
//                    if (FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED ||
//                        FlightGlobals.ActiveVessel.situation == Vessel.Situations.SPLASHED ||
//                        FlightGlobals.ActiveVessel.situation == Vessel.Situations.PRELAUNCH)
//                    {
//                        if (GUI.Button(new Rect(Screen.width - 256, 200f, 128f, 32f), "Some scatter"))
//                            GenerateScatter(FlightGlobals.ActiveVessel.GetWorldPos3D(), 500f, 25);

//                        if (GUI.Button(new Rect(Screen.width - 256, 234f, 128f, 32f), "Some lots of scatter"))
//                            GenerateScatter(FlightGlobals.ActiveVessel.GetWorldPos3D(), 2500, 500);
//                    }
//        }

//        private void DumpGameObject(GameObject go, int indent = 0)
//        {
//            Log.Write("GO: {0}", go.name);
//            foreach (var c in go.GetComponents<Component>())
//                Log.Write(new string(' ', indent + 1) + "has {0}", c.GetType().FullName);


//            for (int i = 0; i < go.transform.childCount; ++i)
//                DumpGameObject(go.transform.GetChild(i).gameObject, indent + 1);
//        }


//        private void GenerateScatter(Vector3 originPoint, float rangeMeters, int count = 25)
//        {


//            //var gos = GameObject.FindObjectsOfType<GameObject>();
//            //foreach (var go in gos)
//            //    Log.Write("GO: {0}", go.name);

//            /*
//[LOG 15:17:37.340] DebugTools, GO: Scatter boulder
//[LOG 15:17:37.340] DebugTools, GO: Scatter stalactite
//[LOG 15:17:37.341] DebugTools, GO: Scatter boulder
//[LOG 15:17:37.341] DebugTools, GO: Scatter boulder
//[LOG 15:17:37.342] DebugTools, GO: Scatter boulder
//[LOG 15:17:37.343] DebugTools, GO: Scatter boulder
//[LOG 15:17:37.343] DebugTools, GO: Scatter BrownRock
//[LOG 15:17:37.344] DebugTools, GO: Scatter boulder
//[LOG 15:17:37.344] DebugTools, GO: Scatter iceboulder
//[LOG 15:17:37.345] DebugTools, GO: Scatter boulder
//[LOG 15:17:37.345] DebugTools, GO: Scatter Rock00
//[LOG 15:17:37.346] DebugTools, GO: Scatter Rock00
//[LOG 15:17:37.346] DebugTools, GO: Scatter cactus
//[LOG 15:17:37.347] DebugTools, GO: Scatter Pine00
//[LOG 15:17:37.348] DebugTools, GO: Scatter boulder
//[LOG 15:17:37.349] DebugTools, GO: Scatter Grass00
//[LOG 15:17:37.349] DebugTools, GO: Scatter Tree00
//[LOG 15:17:37.350] DebugTools, GO: Scatter boulder
//[LOG 15:17:37.350] DebugTools, GO: Scatter boulder
//             */

//            var cactus = GameObject.Find("Scatter cactus");

//            DumpGameObject(cactus);

//            for (int i = 0; i < cactus.transform.childCount; ++i)
//                Log.Write("mf tris: {0}", cactus.transform.GetChild(i).GetComponent<PQSMod_LandClassScatterQuad>().mf.sharedMesh.triangles.Length);

//            Mesh mesh = cactus.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
//            Log.Write("submeshes = {0}", mesh.subMeshCount);

//            //MeshToFile(cactus.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh, "combined", "CactusExported.obj", cactus.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials);


//        }

//        // http://wiki.unity3d.com/index.php?title=ObjExporter
//        public static string MeshToString(string name, Mesh m, Material[] mats)
//        {
//            StringBuilder sb = new StringBuilder();

//            sb.Append("g ").Append(name).Append("\n");
//            foreach (Vector3 v in m.vertices)
//            {
//                sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
//            }
//            sb.Append("\n");
//            foreach (Vector3 v in m.normals)
//            {
//                sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
//            }
//            sb.Append("\n");
//            foreach (Vector3 v in m.uv)
//            {
//                sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
//            }
//            for (int material = 0; material < m.subMeshCount; material++)
//            {
//                sb.Append("\n");
//                sb.Append("usemtl ").Append(mats[material % mats.Length].name).Append("\n");
//                sb.Append("usemap ").Append(mats[material % mats.Length].name).Append("\n");

//                int[] triangles = m.GetTriangles(material);
//                for (int i = 0; i < triangles.Length; i += 3)
//                {
//                    sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
//                        triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1));
//                }
//            }
//            return sb.ToString();
//        }

//        public static void MeshToFile(Mesh m, string objName, string filename, Material[] mats)
//        {
//            using (StreamWriter sw = new StreamWriter(filename))
//            {
//                sw.Write(MeshToString(objName, m, mats));
//            }
//        }
//    }




#if DEBUG
    //This will kick us into the save called default and set the first vessel active
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class Debug_AutoLoadQuicksaveOnStartup : UnityEngine.MonoBehaviour
    {
        public static bool first = true;
        public void Start()
        {
            if (first)
            {
                first = false;
                HighLogic.SaveFolder = "debug";
                var game = GamePersistence.LoadGame("quicksave", HighLogic.SaveFolder, true, false);
                if (game != null && game.flightState != null && game.compatible)
                {
                    FlightDriver.StartAndFocusVessel(game, game.flightState.activeVesselIdx);
                }
                CheatOptions.InfiniteFuel = true;
            }
        }
    }

    //[KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class Debug_AutoSwitchVab : MonoBehaviour
    {
        public static bool first = true;

        public void Start()
        {
            if (first)
            {
                first = false;
                HighLogic.SaveFolder = "debug";
                var game = GamePersistence.LoadGame("quicksave", HighLogic.SaveFolder, true, false);

                if (game != null && game.compatible)
                    HighLogic.LoadScene(GameScenes.EDITOR);
            }
        }
    }

    //[KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class Debug_AutoLoadMainMenuOnStartup : UnityEngine.MonoBehaviour
    {
        public static bool first = true;

        public void Start()
        {
            if (first)
            {
                first = false;
                //HighLogic.SaveFolder = "debug";
                //var game = GamePersistence.LoadGame("quicksave", HighLogic.SaveFolder, true, false);
                //if (game != null && game.flightState != null && game.compatible)
                //{
                //    FlightDriver.StartAndFocusVessel(game, game.flightState.activeVesselIdx);
                //}
                //CheatOptions.InfiniteFuel = true;

                var mm = GameObject.FindObjectOfType<MainMenu>();

                mm.envLogic.GoToStage(1);
                MainMenu.UnlockEverything();
                mm.continueBtn.onPressed();
            }
        }

        void Update()
        {


        }
    }
#endif
}
