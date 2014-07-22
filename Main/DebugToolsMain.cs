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
#define AUTOLOAD_QUICKSAVE
//#define AUTOLOAD_VAB
//#define AUTOLOAD_MAINMENU
//#define AUTOLOAD_SPACECENTER


using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using ImprovedAddonLoader;
using ReeperCommon;
using Toolbar;

namespace DebugTools
{
    //[KSPAddon(KSPAddon.Startup.MainMenu, false)]
    //public class DisableModStatistics : MonoBehaviour
    //{
    //    bool enabled = true;

    //    void OnGUI()
    //    {
    //        if (HighLogic.LoadedScene == GameScenes.MAINMENU)
    //            enabled = GUI.Toggle(new Rect(0, Screen.height - 32f, 128f, 24f), enabled, "Collect anonymous usage information", HighLogic.Skin.toggle);
    //    }
    //}

    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class TestPopup : MonoBehaviour
    {
        IButton button;
        int itemCount = 3;

        void Start()
        {
            Log.Write("creating toolbar button");
            button = Toolbar.ToolbarManager.Instance.add("test", "test2");
            button.Text = "Hi";
            button.TexturePath = "GameData/DebugTools/test2";
            button.OnClick += c => {
                if (button.Drawable == null) 
                {
                    createPopupMenu(button);
                } else destroyPopupMenu(button);};

        }

        private void destroyPopupMenu(IButton button)
        {
            // PopupMenuDrawable must be destroyed explicitly
            ((PopupMenuDrawable)button.Drawable).Destroy();

            // unhook drawable
            button.Drawable = null;
        }

        void createPopupMenu(IButton button) 
        {
            // create menu drawable
            PopupMenuDrawable menu = new PopupMenuDrawable();

            // create menu options
            IButton option1 = menu.AddOption("Increase button count");
            option1.OnClick += (e2) =>
            {
                ++itemCount;
                destroyPopupMenu(button);
                createPopupMenu(button);
            };

            IButton option2 = menu.AddOption("Decrease button count");
            option2.OnClick += (e2) =>
                {
                    --itemCount;
                    destroyPopupMenu(button);
                    createPopupMenu(button);
                };

            menu.AddSeparator();

            for (int i = 0; i < itemCount; ++i)
            {
                IButton option3 = menu.AddOption("Option " + i);
                option3.OnClick += (e2) => Debug.Log("menu option clicked");
            }
            
            // auto-close popup menu when any option is clicked
            //menu.OnAnyOptionClicked += () => destroyPopupMenu(button);

            // hook drawable to button
            button.Drawable = menu;
        }
    }

    //[KSPAddon(KSPAddon.Startup.Flight, false)]
    public class TestButton : MonoBehaviour
    {
        IButton button;
        
        void Start()
        {
            Log.Write("creating toolbar button");
            button = Toolbar.ToolbarManager.Instance.add("test", "test2");
            button.Text = "Hi";
            button.TexturePath = "GameData/DebugTools/test2";
            button.OnClick += c => Log.Write("click!");


            var texture = GameDatabase.Instance.GetTextureIn("DebugTools/", "test", false);
            if (texture == null) Log.Error("failed to find test button texture");

            ApplicationLauncher.Instance.AddModApplication(Button, Button, Button, Button, Button, Button, ApplicationLauncher.AppScenes.SPACECENTER, texture);
        }

        void OnDestroy()
        {
            button.Destroy();
        }

        void Button()
        {
            Log.Write("Button!");
        }
    }



    //[KSPAddon(KSPAddon.Startup.EditorAny, false)]
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
            exit.gameObject.PrintComponents();

            if (exit.Managed)
            {
                Log.Write("is managed");
            }

            //Log.Write("second btn position: {0}", EditorLogic.fetch.flagBrowserButton.transform.position);

            string str;

            button = UIButton.Create("TestButton", new Vector3(0f, 1000f, 90f));
            button.enabled = true;
            button.gameObject.layer = exit.gameObject.layer;
            Log.Write("exit layer: {0}", exit.gameObject.layer);
            Log.Write("cam layer: {0}", exit.renderCamera.cullingMask);
            Log.Write("cam position: {0}", exit.renderCamera.transform.position);
            if (exit.renderCamera.transform.parent != null) Log.Write("cam has a parent!");

            Transform p = exit.renderCamera.transform;
            while (p.parent != null)
                p = p.parent;

            Log.Write("----------- dumping rendercamera components ------------");
            p.gameObject.PrintComponents();

            Log.Write("----------- end rendercamera dump ------------");


            foreach (var c in Camera.allCameras)
                Log.Write("camera name: {0}, ortho = {1}", c.name, c.orthographic);


            button.Setup(128f, 128f, exit.GetPackedMaterial(out str));
            button.RenderCamera = exit.renderCamera;

            Log.Write("our components:");
            button.gameObject.PrintComponents();
            button.SetDrawLayer(exit.drawLayer);
            Log.Write("exit.drawLayer = {0}", exit.drawLayer);
            button.SetAnchor(SpriteRoot.ANCHOR_METHOD.MIDDLE_CENTER);
            button.SetPlane(exit.plane);

            switch (exit.plane)
            {
                case SpriteRoot.SPRITE_PLANE.XY:
                    Log.Write("Plane = XY");
                    break;
                case SpriteRoot.SPRITE_PLANE.XZ:
                    Log.Write("Plane = XZ");
                    break;
                case SpriteRoot.SPRITE_PLANE.YZ:
                    Log.Write("Plane = YZ");
                    break;
            }
            
            button.gameObject.SetActive(true);
            button.Hide(false);
            //button.InitUVs();

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

            var mat = new Material(Shader.Find("Sprite/Vertex Colored")) { mainTexture = button.renderer.sharedMaterial.mainTexture };
            button.renderer.sharedMaterial = mat;

            button.SetPixelToUV(button.renderer.sharedMaterial.mainTexture);
            button.SetCamera(EditorLogic.fetch.exitBtn.renderCamera);
            button.InitUVs();

            Log.Write("cam ortho? {0}", exit.renderCamera.orthographic);
            Log.Write("wxh: {0}x{1}", exit.renderCamera.GetScreenWidth(), exit.renderCamera.GetScreenHeight());
            Log.Write("exit shader: {0}", exit.gameObject.renderer.sharedMaterial.shader.name);

            // works ;\
            clone = ((GameObject)GameObject.Instantiate(exit.gameObject)).GetComponent<UIButton>();
            clone.transform.position = new Vector3(-200f, 800f, 90f);

            gameObject.AddComponent<CenterScreenViewer>();


            Log.Write("exit.WorldToScreen center = {0}", exit.renderCamera.WorldToScreenPoint(new Vector3(0f, 1000f, 0f)));
            Log.Write("exit.WorldToScreen back to world = {0}", exit.renderCamera.ScreenToWorldPoint(exit.renderCamera.WorldToScreenPoint(new Vector3(0f, 1000f, 0f))));
            TestButton();
        }




        void Update()
        {
            EditorLogic.fetch.exitBtn.transform.position = new Vector3(EditorLogic.fetch.exitBtn.transform.position.x - 10f * Time.deltaTime, EditorLogic.fetch.exitBtn.transform.position.y - 10f * Time.deltaTime, EditorLogic.fetch.exitBtn.transform.position.z);
            //Log.Write("editor btn position: {0}", EditorLogic.fetch.exitBtn.transform.position);

            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                button.transform.Translate(new Vector3(-10f, 0f, 0f));
                Log.Write("new button position: {0}", button.transform.position);

                if (ScreenSafeUI.fetch.centerAnchor == null) Log.Error("center anchor is null!");

                Log.Write("centerAnchor.center = {0}", ScreenSafeUI.fetch.leftAnchor.center.position);

            }

            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                button.transform.Translate(new Vector3(10f, 0f, 0f));
                Log.Write("new button position: {0}", button.transform.position);
            }
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                button.transform.Translate(new Vector3(0, 10f, 0f));
                Log.Write("new button position: {0}", button.transform.position);
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                button.transform.Translate(new Vector3(0f, -10f, 0f));
                Log.Write("new button position: {0}", button.transform.position);
            }
        }

        void TestButton()
        {
            // not ezgui types
            //var buttons = GameObject.FindObjectsOfType<ScreenSafeUIButton>();

            //Log.Write("Found {0} buttons", buttons.Length);

            //foreach (var b in buttons)
            //    b.gameObject.PrintComponents();

            //Log.Warning("--------------------------------");

            // doesn't seem to exist in editor screen
            //var ui = GameObject.FindObjectOfType<ScreenSafeUI>();
            //if (ui == null)
            //{
            //    Log.Error("Failed to find ScreenSafeUI object!");
            //}
            //else
            //{
            //    if (ui.centerAnchor == null) Log.Error("ui.centeranchor = null!");
            //    if (ui.centerAnchor.center == null) Log.Error("ui.centeranchor.center = null!");

            //    Log.Write("center anchor position: {0}", ui.centerAnchor.center.position);
            //}


        }
    }


    public class CenterScreenViewer : MonoBehaviour
    {
        Texture2D cTexture;
        //Material material;

        void Start()
        {
            cTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
            var pixels = cTexture.GetPixels32();

            for (int y = 0; y < cTexture.height; ++y)
                for (int x = 0; x < cTexture.width; ++x)
                {
                    Color clr = Color.clear;
                    //clr = Color.red;

                    if (Math.Abs(Screen.width / 2 - x) < 3 || Math.Abs(Screen.height / 2 - y) < 3)
                        clr = new Color(1f, 0f, 0f, 0.25f);
                    pixels[y * cTexture.width + x] = clr;
                }

            cTexture.SetPixels32(pixels);
            cTexture.Apply();
            cTexture.Compress(true);
            cTexture.SaveToDisk("ctexture.png");

            //material = new Material(Shader.Find("KSP/Alpha/Unlit Transparent"));
            //if (material == null) Log.Error("Failed to locate shader for CenterScreenViewer!");

            RenderingManager.AddToPostDrawQueue(5, PostRenderFunction);
        }

        void PostRenderFunction()
        {
            //Log.Write("OnPostrender");
            //var oldRt = RenderTexture.active;
            //RenderTexture.active = camera.targetTexture;
            Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), cTexture);
            //RenderTexture.active = oldRt;
        }
    }



    //[KSPAddon(KSPAddon.Startup.Flight, false)]
    //public class PointDown : MonoBehaviour
    //{
    //    GameObject line = new GameObject();
    //    Quaternion targetHeading = Quaternion.identity;
    //    bool haveTarget = false;

    //    void Start()
    //    {
    //        var vessel = FlightGlobals.ActiveVessel;

    //        line.transform.position = vessel.rootPart.transform.position + vessel.rootPart.transform.forward * 10f;
    //        line.transform.parent = vessel.rootPart.transform;

    //        var ln = DebugVisualizer.Line(new List<Transform> { vessel.rootPart.transform, line.transform }, 0.2f, 0.01f);
    //        ln.renderer.material = ResourceUtil.LocateMaterial("DebugTools.XrayShader.shader");
    //        ln.renderer.material.color = Color.yellow;

    //        //FlightGlobals.ActiveVessel.VesselSAS.LockHeading(Quaternion.LookRotation(FlightGlobals.ActiveVessel.rootPart.transform.forward), true);
    //    }

    //    void LateUpdate()
    //    {
    //        var sas = FlightGlobals.ActiveVessel.VesselSAS;

    //        if (Input.GetKeyDown(KeyCode.O))
    //        {
    //            //var sas = FlightGlobals.ActiveVessel.FindPartModulesImplementing<VesselSAS>().Single();
    //            //if (sas == null) Log.Error("failed to find sas!");

    //            //FlightGlobals.ActiveVessel.gameObject.PrintComponents();


    //            Log.Write("Locking sas direction downwards");
    //            //sas.LockHeading(Quaternion.FromToRotation(sas.referenceRotation, -FlightGlobals.ActiveVessel.ReferenceTransform.up) * sas.currentRotation);
    //            //sas.LockHeading(Quaternion.LookRotation(-FlightGlobals.ActiveVessel.ReferenceTransform.up), false);
    //            //sas.lockedHeading = Quaternion.LookRotation(-FlightGlobals.ActiveVessel.ReferenceTransform.up);
    //            //sas.LockHeading(Quaternion.LookRotation(-FlightGlobals.ActiveVessel.ReferenceTransform.up), false);

    //            var surfaceNormal = FlightGlobals.currentMainBody.GetRelSurfaceNVector(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude);
    //            var relativeRotation = Quaternion.FromToRotation(Vector3.up, FlightGlobals.upAxis);

    //            //sas.LockHeading(Quaternion.FromToRotation(FlightGlobals.ActiveVessel.ReferenceTransform.rotation * Vector3.up, relativeRotation * -surfaceNormal) * FlightGlobals.ActiveVessel.ReferenceTransform.rotation, true);


    //            //sas.LockHeading(Quaternion.LookRotation(FlightGlobals.ActiveVessel.rootPart.transform.forward), true);
    //            //sas.LockHeading(Quaternion.FromToRotation(sas.currentRotation * Vector3.up, surfaceNormal) * sas.currentRotation, true);

    //            //FlightGlobals.ActiveVessel.srfRelRotation

    //            // works
    //            //sas.LockHeading(Quaternion.FromToRotation(sas.lockedHeading * Vector3.up, FlightGlobals.upAxis) * sas.lockedHeading, true);

    //            var newHeading = FlightGlobals.ActiveVessel.ReferenceTransform.rotation * -Vector3.up;

    //            //sas.LockHeading(Quaternion.FromToRotation(sas.lockedHeading * Vector3.up, newHeading) * sas.lockedHeading);
    //            targetHeading = Quaternion.FromToRotation(sas.lockedHeading * Vector3.up, newHeading) * sas.lockedHeading;
                

    //            haveTarget = true;

    //        }

    //        if (haveTarget)
    //        {
    //            sas.LockHeading(targetHeading, true);
    //            //sas.lockedHeading = targetHeading;
    //        }

    //        //Log.Write("sas reference rotation: {0}", sas.referenceRotation.FString());
    //        Log.Write("locked heading: {0}", sas.lockedHeading);

    //        var vr = FlightGlobals.ActiveVessel.ReferenceTransform.rotation;
    //        var inv = ((Quaternion)FlightGlobals.ActiveVessel.mainBody.rotation).Inverse();

    //        Log.Write("locked heading up: {0}", sas.lockedHeading * Vector3.up);

    //        Log.Write("up axis: {0}", vr * Vector3.up);
    //        Log.Write("forward axis: {0}", vr * Vector3.forward);
    //        Log.Write("target heading: {0}", targetHeading * Vector3.up);

    //    }
    //}


    //[KSPAddon(KSPAddon.Startup.Flight, false)]
    //public class OceanTest : MonoBehaviour
    //{
    //    void Start()
    //    {
    //        FlightGlobals.ActiveVessel.rootPart.AddModule("ModuleWaterSlider");
    //    }
    //}

    //public class ModuleWaterSlider : PartModule
    //{
    //    GameObject _collider = new GameObject("ModuleWaterSlider.Collider", typeof(BoxCollider), typeof(Rigidbody));
    //    float triggerDistance = 50f; // avoid moving every frame

    //    void Start()
    //    {
    //        Log.Write("WaterSlider start");

    //        var box = _collider.collider as BoxCollider;
    //        box.size = new Vector3(200f, 5f, 200f); // probably should encapsulate other colliders in real code

    //        var rb = _collider.rigidbody;
    //        rb.isKinematic = true;

    //        _collider.SetActive(true);

    //        var visible = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //        visible.transform.parent = _collider.transform;
    //        visible.transform.localScale = box.size;
    //        visible.renderer.enabled = false; // enable to see collider

    //        // Debug stuff
    //        Action<Vector3, Color> createAxis = delegate(Vector3 axis, Color c)
    //        {
    //            var go = new GameObject();
    //            go.transform.parent = _collider.transform;
    //            go.transform.position = _collider.transform.position + axis * 25f;

    //            var line = DebugVisualizer.Line(new List<Transform> { go.transform, _collider.transform }, 0.2f, 0.1f);
    //            line.renderer.material = ResourceUtil.LocateMaterial("DebugTools.XrayShader.shader", "Particles/Additive");
    //            line.renderer.material.color = c;
    //        };


    //        createAxis(Vector3.up, Color.red);
    //        createAxis(Vector3.right, Color.blue);
    //        createAxis(Vector3.forward, Color.green);

    //        part.vessel.parts.ForEach(p =>
    //        {
    //            if (part.FindModelComponent<WheelCollider>() != null)
    //                PartLoader.StripComponent<PartBuoyancy>(part.gameObject);
    //        });

    //        UpdatePosition();
    //    }



    //    void UpdatePosition()
    //    {
    //        Log.Write("Moving plane");

    //        var cb = part.vessel.mainBody;
    //        var oceanNormal = cb.GetSurfaceNVector(vessel.latitude, vessel.longitude);

    //        _collider.rigidbody.position = (vessel.ReferenceTransform.position - oceanNormal * (FlightGlobals.getAltitudeAtPos(vessel.ReferenceTransform.position) + 2.6f));
    //        _collider.rigidbody.rotation = (Quaternion.LookRotation(oceanNormal) * Quaternion.AngleAxis(90f, Vector3.right));
    //    }


    //    void FixedUpdate()
    //    {
    //        if (Vector3.Distance(_collider.transform.position, vessel.ReferenceTransform.position) > triggerDistance)
    //            UpdatePosition();
    //    }
    //}

    //public class ModuleWaterGlider : PartModule
    //{
    //    Transform wheel;

    //    void Start()
    //    {
    //        wheel = part.gameObject.GetComponentInChildren<WheelCollider>().transform;
    //    }

    //    void Update()
    //    {
    //        var cb = part.vessel.mainBody;

    //        if (cb.ocean)
    //        {
    //            var oceanNormal = cb.GetSurfaceNVector(vessel.latitude, vessel.longitude);
    //            float oceanDistance = FlightGlobals.getAltitudeAtPos(wheel.position);
    //            Vector3 geeForce = FlightGlobals.getGeeForceAtPosition(wheel.position);
    //            Ray ray = new Ray(wheel.position, -oceanNormal);
    //            RaycastHit hit;
    //            bool terrainHit = Physics.Raycast(ray, out hit, oceanDistance + 10f, 1 << 15);

    //            if (terrainHit && hit.distance > oceanDistance || !terrainHit)
    //            {
    //                // we're over water
    //                Plane ocean = new Plane(oceanNormal, oceanNormal * oceanDistance);
    //                WheelCollider wc = wheel.collider as WheelCollider;

    //                if (!ocean.GetSide(wheel.position + wheel.rotation * Vector3.down * wc.radius))
    //                {
    //                    // wheel below water
    //                    part.rigidbody.AddForce(-geeForce * 1.5f, ForceMode.Acceleration);
    //                }
    //            }
    //        }
    //    }
    //}

    


    

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
#if AUTOLOAD_QUICKSAVE
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
                StartCoroutine(DelayedStart());
            }
        }

        public System.Collections.IEnumerator DelayedStart()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            HighLogic.SaveFolder = "debug";
            var game = GamePersistence.LoadGame("quicksave", HighLogic.SaveFolder, true, false);
            if (game != null && game.flightState != null && game.compatible)
            {
                FlightDriver.StartAndFocusVessel(game, game.flightState.activeVesselIdx);
            }
            CheatOptions.InfiniteFuel = true;
        }
    }
#endif

#if AUTOLOAD_VAB
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
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
                {
                    game.startScene = GameScenes.EDITOR;
                    HighLogic.LoadScene(GameScenes.EDITOR);
                }
            }
        }
    }
#endif

#if AUTOLOAD_MAINMENU
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class Debug_AutoLoadMainMenuOnStartup : UnityEngine.MonoBehaviour
    {
        public static bool first = true;

        public void Start()
        {
            if (first)
            {
                first = false;

                var mm = GameObject.FindObjectOfType<MainMenu>();

                mm.envLogic.GoToStage(1);
                MainMenu.UnlockEverything();
                mm.continueBtn.onPressed();
            }
        }
    }
#endif

#if AUTOLOAD_SPACECENTER
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class Debug_AutoLoadSpaceCenterOnStartUp : MonoBehaviour
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
                {
                    game.startScene = GameScenes.SPACECENTER;
                    game.Load();
                    game.Start();
                }
            }
        }

    }
#endif
#endif
}

/*
[LOG 21:04:29.355] 0: Default
[LOG 21:04:29.355] 1: TransparentFX
[LOG 21:04:29.356] 2: Ignore Raycast
[LOG 21:04:29.356] 3: 
[LOG 21:04:29.356] 4: Water
[LOG 21:04:29.356] 5: 
[LOG 21:04:29.357] 6: 
[LOG 21:04:29.357] 7: 
[LOG 21:04:29.357] 8: PartsList_Icons
[LOG 21:04:29.358] 9: Atmosphere
[LOG 21:04:29.358] 10: Scaled Scenery
[LOG 21:04:29.358] 11: UI_Culled
[LOG 21:04:29.359] 12: UI_Main
[LOG 21:04:29.359] 13: UI_Mask
[LOG 21:04:29.359] 14: Screens
[LOG 21:04:29.360] 15: Local Scenery
[LOG 21:04:29.360] 16: kerbals
[LOG 21:04:29.360] 17: Editor_UI
[LOG 21:04:29.361] 18: SkySphere
[LOG 21:04:29.361] 19: Disconnected Parts
[LOG 21:04:29.361] 20: Internal Space
[LOG 21:04:29.362] 21: Part Triggers
[LOG 21:04:29.362] 22: KerbalInstructors
[LOG 21:04:29.362] 23: ScaledSpaceSun
[LOG 21:04:29.363] 24: MapFX
[LOG 21:04:29.363] 25: EzGUI_UI
[LOG 21:04:29.363] 26: WheelCollidersIgnore
[LOG 21:04:29.364] 27: WheelColliders
[LOG 21:04:29.364] 28: TerrainColliders
[LOG 21:04:29.364] 29: 
[LOG 21:04:29.365] 30: 
[LOG 21:04:29.365] 31: Vectors
*/