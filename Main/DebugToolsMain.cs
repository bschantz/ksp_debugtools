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

#define DISABLE_ASTEROID_SPAWNER

using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using UnityEngine;
using ImprovedAddonLoader;
using ReeperCommon;
//using Toolbar;
using System.Runtime.InteropServices;

namespace DebugTools
{
    //class CloakingDevice : PartModule
    //{
    //    [KSPEvent(active = true, guiActive = true, guiActiveEditor = true, guiName = "Cloak")]
    //    void Cloak()
    //    {
    //        part.FindModelTransform("Capsule001").gameObject.SetActive(false);

    //        Events["Uncloak"].active = true;
    //        Events["Cloak"].active = false;
    //    }

    //    [KSPEvent(active = false, guiActive = false, guiActiveEditor = true, guiName = "Uncloak")]
    //    void Uncloak()
    //    {
    //        part.FindModelTransform("Capsule001").gameObject.SetActive(true);
    //        Events["Uncloak"].active = false;
    //        Events["Cloak"].active = true;
    //    }
    //}

    //[KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    //class ModelDumper : MonoBehaviour
    //{
    //    private void Start()
    //    {
    //        PartLoader.getPartInfoByName("GooExperiment").partPrefab.AddModule("CloakingDevice");
    //        //GameDatabase.Instance.databaseModel.ForEach(go =>
    //        //{
    //        //    Log.Debug("Printing hierarchy for Model '{0}'", go.name);
    //        //    go.PrintComponents();

    //        //    go.GetComponentsInChildren<MeshFilter>(true).ToList().ForEach(mf => Log.Debug("MF on {0} is called {1}", mf.gameObject.name, mf.name));
    //        //});

    //        Log.Debug("==============================");
    //        GameDatabase.Instance.GetModelIn("Squad/Parts/Science/GooExperiment").PrintComponents();

    //        Log.Debug("==============================");
    //        PartLoader.getPartInfoByName("GooExperiment").partPrefab.gameObject.PrintComponents();
    //    }
    //}

//[KSPAddon(KSPAddon.Startup.Flight, false)]
class LineDrawer : MonoBehaviour
{
    LineRenderer up, right, forward, sky;
    Transform owner;

    private System.Collections.IEnumerator Start()
    {
        while (!FlightGlobals.ready) yield return 0;

        var rt = FlightGlobals.ActiveVessel.rootPart.transform;

        forward = MakeLine(rt, Color.blue);
        right = MakeLine(rt, Color.red);
        up = MakeLine(rt, Color.green);
        sky = MakeLine(rt, Color.yellow);

        sky.SetWidth(0.2f, 0.1f);

        owner = rt;
    }

    private LineRenderer MakeLine(Transform parent, Color color)
    {
        GameObject line = new GameObject();
        line.transform.parent = transform;

        var r = line.AddComponent<LineRenderer>();

        r.useWorldSpace = true; // ignores own position, just use coordinates in worldspace as given
        r.SetWidth(1f, 0.01f);
        r.SetVertexCount(2);

        r.material = new Material(Shader.Find("Particles/Additive"));
        r.SetColors(color, color);

        return r;
    }

    private void Update()
    {
        if (!FlightGlobals.ready) return;

        up.SetPosition(0, owner.position);
        up.SetPosition(1, owner.position + owner.up * 5f);

        right.SetPosition(0, owner.position);
        right.SetPosition(1, owner.position + owner.right * 5f);

        forward.SetPosition(0, owner.position);
        forward.SetPosition(1, owner.position + owner.forward * 5f);

        sky.SetPosition(0, owner.position);
        sky.SetPosition(1, owner.position + FlightGlobals.upAxis * 10f);

    }
}

//class JellyListener : MonoBehaviour
//{
//    private void MethodSentOnAnimationsGameObject()
//    {
//        // just forward it
//        GetComponentInParent<JellyfishDish>().SendMessage("JellyfishOpen");
//    }
//}

//class JellyfishDish : PartModule
//{
//    new Animation animation;

//    public override void OnAwake()
//    {
//        base.OnAwake();
//        print("Jellyfish awake");

//        animation = part.FindModelAnimators().SingleOrDefault();
//        int frames = (int)(animation.clip.length * animation.clip.frameRate);

//        animation.AddClip(animation.clip, "Jellyfish", (int)(frames * 0.7) /* selected arbitrarily */, frames);
            
//        AnimationState jellyfish = animation["Jellyfish"];
            
//        jellyfish.normalizedSpeed *= 3f;
//        jellyfish.clip.AddEvent(new AnimationEvent()
//        {
//            time = 0f,
//            functionName = "MethodSentOnAnimationsGameObject"
//        });

//        jellyfish.wrapMode = WrapMode.PingPong;
//        animation.gameObject.AddComponent<JellyListener>(); // listens for our event
//    }

//    [KSPEvent(active = true, guiActive = true, guiActiveEditor = true, guiName = "Deploy the Jellyfish")]
//    public void JellyfishDeploy()
//    {
//        animation.Play("Jellyfish");
//    }

//    private void JellyfishOpen()
//    {
//        ScreenMessages.PostScreenMessage("Jellyfish!", .5f, (ScreenMessageStyle)UnityEngine.Random.Range(0, 3));
//        print("JellyfishOpen event");
//    }
//}

//[KSPAddon(KSPAddon.Startup.MainMenu, true)]
//class InsertPartModule : MonoBehaviour
//{
//    private void Start()
//    {
//        PartLoader.getPartInfoByName("commDish").partPrefab.AddModule("JellyfishDish");
//    }
//}

    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class CamDumper : MonoBehaviour
    {
        void Start()
        {
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                Log.Warning("Dumping all scene cameras:");

                foreach (Camera c in Camera.allCameras)
                    Log.Normal("Camera: Name '{0}', tag '{1}'", c.name, c.tag);
            }
        }
    }

    //[KSPAddon(KSPAddon.Startup.Flight, false)]
    public class FrictionRemover : MonoBehaviour
    {
        void Update()
        {
            
            Vessel vessel = FlightGlobals.ActiveVessel;
            Part root = vessel.rootPart;

            if (Input.GetKeyDown(KeyCode.KeypadMultiply))
            {
                Log.Normal("Removing friction from {0}", FlightGlobals.ActiveVessel.rootPart.partInfo.partPrefab.name);




                //root.collider.material
                root.gameObject.GetComponentsInChildren<Collider>().ToList().ForEach(c => 
                    {
                        c.material = new PhysicMaterial("Frictionless") 
                            { 
                                dynamicFriction = 0f,
                                dynamicFriction2 = 0f,
                                staticFriction = 0f,
                                staticFriction2 = 0f,
                                bounciness = 1f,
                                bounceCombine = PhysicMaterialCombine.Minimum,
                                frictionCombine = PhysicMaterialCombine.Minimum
                            };
                    });

                root.rigidbody.drag = 0f;
                root.rigidbody.angularDrag = 0f;
                root.angularDrag = 0f;
                root.minimum_drag = 0f;
                root.maximum_drag = 0f;
                   
                
                Log.Normal("done");
            }

            Log.Normal("Drag = " + root.rigidbody.drag + ", angular = " + root.rigidbody.angularDrag);
            Log.Normal("friction = " + root.gameObject.GetComponentInChildren<Collider>().material.dynamicFriction + ", dynamic = " + root.gameObject.GetComponentInChildren<Collider>().material.dynamicFriction2);
            Log.Normal("drag model = " + root.dragModel.ToString());
        }

        void LateUpdate()
        {
            //FlightGlobals.ActiveVessel.rootPart.rigidbody.angularDrag = 0f;
        }
    }

    //[KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class DomainCreator : MonoBehaviour
    {
        AppDomain domain = null;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                //Assembly.GetExecutingAssembly().
                Log.Normal("Creating AppDomain...");

                //var sec = new System.Security.Policy.Evidence(
                Log.Normal("setup");
                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;

                Log.Normal("Create");
                if (AppDomain.CurrentDomain.DomainManager == null) Log.Error("domain manager not found!");
                if (System.AppDomain.CurrentDomain.DomainManager == null) Log.Error("other method not found either");

                //domain = AppDomain.CreateDomain("NewDomain", new System.Security.Policy.Evidence(null, null), setup);
                //domain = AppDomain.CreateDomain("NewDomain");

                Log.Normal("Domain created!");

                if (domain == null) Log.Error("nope, domain is null");
                
            }
        }

        //public static IList<AppDomain> GetAppDomains()
        //{
        //    IList<AppDomain> _IList = new List<AppDomain>();
        //    IntPtr enumHandle = IntPtr.Zero;
        //    CorRuntimeHostClass host = new mscoree.CorRuntimeHostClass();
        //    try
        //    {
        //        host.EnumDomains(out enumHandle);
        //        object domain = null;
        //        while (true)
        //        {
        //            host.NextDomain(enumHandle, out domain);
        //            if (domain == null) break;
        //            AppDomain appDomain = (AppDomain)domain;
        //            _IList.Add(appDomain);
        //        }
        //        return _IList;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //        return null;
        //    }
        //    finally
        //    {
        //        host.CloseEnum(enumHandle);
        //        Marshal.ReleaseComObject(host);
        //    }
        //} 
    }


    //[KSPAddon(KSPAddon.Startup.EditorAny, false)]
    class DuplicatePanel : MonoBehaviour
    {
        System.Collections.IEnumerator Start()
        {
            yield return new WaitForSeconds(2f);

            //Transform t;

            //if (UIPanelManager.instance == null) Log.Error("uip is null");
            //Log.Write("panel count: " + UIPanelManager.instance.GetPanelCount());

            //try
            //{
            //    UIPanelManager.instance.GetPanels().ToList().ForEach(uip => Log.Write("Panel: " + uip.name));

            //    t = UIPanelManager.instance.GetPanels().ToList().Single().transform;

            //    while (t.parent != null) t = t.parent;

            //    t.gameObject.PrintComponents();
            //} catch {}

            //var panel = FindObjectOfType<UIPanel>();
            //if (panel == null) Log.Error("no panels found");
            //Log.Write("found panel " + panel.name);

            //t = panel.transform;
            //while (t.parent != null) t = t.parent;

            //t.gameObject.PrintComponents();

            // UIPanel PanelPartList
            
            // look in prefabs for likely suspects
            // note: found one, "PanelPartList" (same name as one in scene)
            foreach (var possible in Resources.FindObjectsOfTypeAll<GameObject>().Where(go => go.GetComponent<UIPanel>() != null).ToList())
                Log.Write("Possible: " + possible.name);

            CreateFromPrefab();
        }

        /// <summary>
        /// Find the prefab for the part list and use it to construct a new one (note to self: does
        /// not include any parts so it's ready to go)
        /// </summary>
        /// <returns></returns>
        GameObject CreateFromPrefab()
        {
            var prefab = Resources.FindObjectsOfTypeAll<GameObject>().Where(go => go.name == "PanelPartList").SingleOrDefault();
            if (prefab == null) Log.Error("problem locating prefab");

            if (prefab == EditorPanels.Instance.partsEditor)
                Log.Error("no, no good at all ....");

            // clone it
            //GameObject cloned = (GameObject)Instantiate(prefab);

            // it comes with some bits we don't have a need for:
            // child "EditorFooter"
            // child "Tabs"
            // any child that starts with "Icon" or "ShipTemplate"
            // an extra EditorPartList component

            //return cloned;
            return null;
/*
[LOG 20:44:44.700] DebugTools, --->PanelPartList has components:
[LOG 20:44:44.700] DebugTools, ......c: UnityEngine.Transform
[LOG 20:44:44.701] DebugTools, ......c: EditorPartList
[LOG 20:44:44.701] DebugTools, ......c: UIPanel
[LOG 20:44:44.702] DebugTools, ------>Background has components:
[LOG 20:44:44.702] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:44.703] DebugTools, .........c: UIButton
[LOG 20:44:44.704] DebugTools, .........c: UnityEngine.MeshFilter
[LOG 20:44:44.704] DebugTools, .........c: UnityEngine.MeshRenderer
[LOG 20:44:44.705] DebugTools, .........c: EditorPartButton
[LOG 20:44:44.705] DebugTools, .........c: UnityEngine.BoxCollider
[LOG 20:44:44.706] DebugTools, ------>EditorFooter has components:
[LOG 20:44:44.706] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:44.707] DebugTools, --------->ButtonsCenterOfStuff has components:
[LOG 20:44:44.708] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.708] DebugTools, ------------>ButtonCenterOfLift has components:
[LOG 20:44:44.709] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:44.709] DebugTools, ...............c: UIButton
[LOG 20:44:44.710] DebugTools, ...............c: UnityEngine.MeshFilter
[LOG 20:44:44.711] DebugTools, ...............c: UnityEngine.MeshRenderer
[LOG 20:44:44.711] DebugTools, ...............c: EditorPartButton
[LOG 20:44:44.712] DebugTools, ...............c: EditorShowTooltip
[LOG 20:44:44.712] DebugTools, ...............c: UnityEngine.BoxCollider
[LOG 20:44:44.713] DebugTools, ------------>ButtonCenterOfMass has components:
[LOG 20:44:44.713] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:44.714] DebugTools, ...............c: UIButton
[LOG 20:44:44.715] DebugTools, ...............c: UnityEngine.MeshFilter
[LOG 20:44:44.715] DebugTools, ...............c: UnityEngine.MeshRenderer
[LOG 20:44:44.716] DebugTools, ...............c: EditorPartButton
[LOG 20:44:44.716] DebugTools, ...............c: EditorShowTooltip
[LOG 20:44:44.717] DebugTools, ...............c: UnityEngine.BoxCollider
[LOG 20:44:44.717] DebugTools, ------------>ButtonCenterOfThrust has components:
[LOG 20:44:44.718] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:44.719] DebugTools, ...............c: UIButton
[LOG 20:44:44.719] DebugTools, ...............c: UnityEngine.MeshFilter
[LOG 20:44:44.720] DebugTools, ...............c: UnityEngine.MeshRenderer
[LOG 20:44:44.721] DebugTools, ...............c: EditorPartButton
[LOG 20:44:44.721] DebugTools, ...............c: EditorShowTooltip
[LOG 20:44:44.722] DebugTools, ...............c: UnityEngine.BoxCollider
[LOG 20:44:44.722] DebugTools, --------->ButtonsSymmetrySnaps has components:
[LOG 20:44:44.723] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.723] DebugTools, ------------>ButtonSnap has components:
[LOG 20:44:44.724] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:44.725] DebugTools, ...............c: UIButton
[LOG 20:44:44.725] DebugTools, ...............c: UnityEngine.MeshFilter
[LOG 20:44:44.726] DebugTools, ...............c: UnityEngine.MeshRenderer
[LOG 20:44:44.726] DebugTools, ...............c: EditorPartButton
[LOG 20:44:44.727] DebugTools, ...............c: EditorShowTooltip
[LOG 20:44:44.727] DebugTools, ...............c: UnityEngine.BoxCollider
[LOG 20:44:44.728] DebugTools, --------------->StatesSnap has components:
[LOG 20:44:44.729] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.729] DebugTools, ..................c: PackedSprite
[LOG 20:44:44.730] DebugTools, ..................c: UnityEngine.MeshFilter
[LOG 20:44:44.731] DebugTools, ..................c: UnityEngine.MeshRenderer
[LOG 20:44:44.731] DebugTools, ------------>ButtonSymmetry has components:
[LOG 20:44:44.732] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:44.733] DebugTools, ...............c: UIButton
[LOG 20:44:44.733] DebugTools, ...............c: UnityEngine.MeshFilter
[LOG 20:44:44.734] DebugTools, ...............c: UnityEngine.MeshRenderer
[LOG 20:44:44.734] DebugTools, ...............c: EditorPartButton
[LOG 20:44:44.735] DebugTools, ...............c: EditorShowTooltip
[LOG 20:44:44.736] DebugTools, ...............c: UnityEngine.BoxCollider
[LOG 20:44:44.736] DebugTools, --------------->StatesMirror has components:
[LOG 20:44:44.737] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.737] DebugTools, ..................c: PackedSprite
[LOG 20:44:44.738] DebugTools, ..................c: UnityEngine.MeshFilter
[LOG 20:44:44.739] DebugTools, ..................c: UnityEngine.MeshRenderer
[LOG 20:44:44.739] DebugTools, --------------->StatesSymmetry has components:
[LOG 20:44:44.740] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.741] DebugTools, ..................c: PackedSprite
[LOG 20:44:44.741] DebugTools, ..................c: UnityEngine.MeshFilter
[LOG 20:44:44.742] DebugTools, ..................c: UnityEngine.MeshRenderer
[LOG 20:44:44.742] DebugTools, --------->CreateSubassembly has components:
[LOG 20:44:44.743] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.744] DebugTools, ............c: UIButton
[LOG 20:44:44.744] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.745] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.745] DebugTools, ............c: EditorShowTooltip
[LOG 20:44:44.746] DebugTools, ............c: EditorSubassemblyButton
[LOG 20:44:44.746] DebugTools, --------->CurrencyWidgets_Editor has components:
[LOG 20:44:44.747] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.748] DebugTools, ............c: NestedPrefabSpawner
[LOG 20:44:44.748] DebugTools, ------------>FundsWidget has components:
[LOG 20:44:44.749] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:44.749] DebugTools, ...............c: FundsWidget
[LOG 20:44:44.750] DebugTools, --------------->fundsGreen has components:
[LOG 20:44:44.751] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.751] DebugTools, ..................c: UnityEngine.MeshFilter
[LOG 20:44:44.752] DebugTools, ..................c: UnityEngine.MeshRenderer
[LOG 20:44:44.752] DebugTools, --------------->fundsRef has components:
[LOG 20:44:44.753] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.754] DebugTools, ..................c: UnityEngine.MeshFilter
[LOG 20:44:44.754] DebugTools, ..................c: UnityEngine.MeshRenderer
[LOG 20:44:44.755] DebugTools, --------------->Bg has components:
[LOG 20:44:44.755] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.756] DebugTools, ..................c: UnityEngine.MeshFilter
[LOG 20:44:44.757] DebugTools, ..................c: UnityEngine.MeshRenderer
[LOG 20:44:44.757] DebugTools, --------------->Frame has components:
[LOG 20:44:44.758] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.758] DebugTools, ..................c: UnityEngine.MeshFilter
[LOG 20:44:44.759] DebugTools, ..................c: UnityEngine.MeshRenderer
[LOG 20:44:44.760] DebugTools, --------------->tumblerMaskBottom has components:
[LOG 20:44:44.760] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.761] DebugTools, ..................c: UnityEngine.MeshFilter
[LOG 20:44:44.761] DebugTools, ..................c: UnityEngine.MeshRenderer
[LOG 20:44:44.762] DebugTools, --------------->tumblerMaskTop has components:
[LOG 20:44:44.763] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.763] DebugTools, ..................c: UnityEngine.MeshFilter
[LOG 20:44:44.764] DebugTools, ..................c: UnityEngine.MeshRenderer
[LOG 20:44:44.764] DebugTools, --------------->tumblers has components:
[LOG 20:44:44.765] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.766] DebugTools, ..................c: Tumblers
[LOG 20:44:44.766] DebugTools, ------------------>tumbler0 has components:
[LOG 20:44:44.767] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:44.767] DebugTools, .....................c: UnityEngine.MeshFilter
[LOG 20:44:44.768] DebugTools, .....................c: UnityEngine.MeshRenderer
[LOG 20:44:44.769] DebugTools, .....................c: Tumbler
[LOG 20:44:44.769] DebugTools, ------------------>tumbler1 has components:
[LOG 20:44:44.770] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:44.771] DebugTools, .....................c: UnityEngine.MeshFilter
[LOG 20:44:44.771] DebugTools, .....................c: UnityEngine.MeshRenderer
[LOG 20:44:44.772] DebugTools, .....................c: Tumbler
[LOG 20:44:44.772] DebugTools, ------------------>tumbler2 has components:
[LOG 20:44:44.773] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:44.774] DebugTools, .....................c: UnityEngine.MeshFilter
[LOG 20:44:44.774] DebugTools, .....................c: UnityEngine.MeshRenderer
[LOG 20:44:44.775] DebugTools, .....................c: Tumbler
[LOG 20:44:44.775] DebugTools, ------------------>tumbler3 has components:
[LOG 20:44:44.776] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:44.777] DebugTools, .....................c: UnityEngine.MeshFilter
[LOG 20:44:44.777] DebugTools, .....................c: UnityEngine.MeshRenderer
[LOG 20:44:44.778] DebugTools, .....................c: Tumbler
[LOG 20:44:44.778] DebugTools, ------------------>tumbler4 has components:
[LOG 20:44:44.779] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:44.780] DebugTools, .....................c: UnityEngine.MeshFilter
[LOG 20:44:44.780] DebugTools, .....................c: UnityEngine.MeshRenderer
[LOG 20:44:44.781] DebugTools, .....................c: Tumbler
[LOG 20:44:44.781] DebugTools, ------------------>tumbler5 has components:
[LOG 20:44:44.782] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:44.783] DebugTools, .....................c: UnityEngine.MeshFilter
[LOG 20:44:44.784] DebugTools, .....................c: UnityEngine.MeshRenderer
[LOG 20:44:44.784] DebugTools, .....................c: Tumbler
[LOG 20:44:44.785] DebugTools, ------------------>tumbler6 has components:
[LOG 20:44:44.785] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:44.786] DebugTools, .....................c: UnityEngine.MeshFilter
[LOG 20:44:44.787] DebugTools, .....................c: UnityEngine.MeshRenderer
[LOG 20:44:44.787] DebugTools, .....................c: Tumbler
[LOG 20:44:44.788] DebugTools, ------------------>tumbler7 has components:
[LOG 20:44:44.788] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:44.789] DebugTools, .....................c: UnityEngine.MeshFilter
[LOG 20:44:44.790] DebugTools, .....................c: UnityEngine.MeshRenderer
[LOG 20:44:44.790] DebugTools, .....................c: Tumbler
[LOG 20:44:44.791] DebugTools, ------------>CostWidget has components:
[LOG 20:44:44.791] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:44.792] DebugTools, ...............c: CostWidget
[LOG 20:44:44.793] DebugTools, --------------->Frame has components:
[LOG 20:44:44.793] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.794] DebugTools, ..................c: UnityEngine.MeshFilter
[LOG 20:44:44.794] DebugTools, ..................c: UnityEngine.MeshRenderer
[LOG 20:44:44.795] DebugTools, --------------->valueText has components:
[LOG 20:44:44.796] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.796] DebugTools, ..................c: UnityEngine.MeshRenderer
[LOG 20:44:44.797] DebugTools, ..................c: UnityEngine.TextMesh
[LOG 20:44:44.797] DebugTools, --------------->costIcon has components:
[LOG 20:44:44.798] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.799] DebugTools, ..................c: UnityEngine.MeshFilter
[LOG 20:44:44.799] DebugTools, ..................c: UnityEngine.MeshRenderer
[LOG 20:44:44.800] DebugTools, --------->Frame has components:
[LOG 20:44:44.800] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.801] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.802] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.802] DebugTools, --------->FrameBg has components:
[LOG 20:44:44.803] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.803] DebugTools, ............c: UIButton
[LOG 20:44:44.804] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.804] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.805] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:44.806] DebugTools, --------->PageNext has components:
[LOG 20:44:44.806] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.807] DebugTools, ............c: UIButton
[LOG 20:44:44.807] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.808] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.809] DebugTools, ............c: EditorPartButton
[LOG 20:44:44.809] DebugTools, ............c: EditorShowTooltip
[LOG 20:44:44.810] DebugTools, --------->PagePrev has components:
[LOG 20:44:44.811] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.811] DebugTools, ............c: UIButton
[LOG 20:44:44.812] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.812] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.813] DebugTools, ............c: EditorPartButton
[LOG 20:44:44.813] DebugTools, ............c: EditorShowTooltip
[LOG 20:44:44.814] DebugTools, ------>Tabs has components:
[LOG 20:44:44.815] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:44.815] DebugTools, --------->TabControl has components:
[LOG 20:44:44.816] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.816] DebugTools, ............c: UIStateToggleBtn
[LOG 20:44:44.817] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.817] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.818] DebugTools, ............c: EditorPartButton
[LOG 20:44:44.818] DebugTools, ............c: EditorShowTooltip
[LOG 20:44:44.819] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:44.820] DebugTools, --------->TabScience has components:
[LOG 20:44:44.820] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.821] DebugTools, ............c: UIStateToggleBtn
[LOG 20:44:44.821] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.822] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.823] DebugTools, ............c: EditorPartButton
[LOG 20:44:44.823] DebugTools, ............c: EditorShowTooltip
[LOG 20:44:44.824] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:44.824] DebugTools, --------->TabAero has components:
[LOG 20:44:44.825] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.826] DebugTools, ............c: UIStateToggleBtn
[LOG 20:44:44.826] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.827] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.827] DebugTools, ............c: EditorPartButton
[LOG 20:44:44.828] DebugTools, ............c: EditorShowTooltip
[LOG 20:44:44.828] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:44.829] DebugTools, --------->TabPropulsion has components:
[LOG 20:44:44.830] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.830] DebugTools, ............c: UIStateToggleBtn
[LOG 20:44:44.831] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.831] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.832] DebugTools, ............c: EditorPartButton
[LOG 20:44:44.833] DebugTools, ............c: EditorShowTooltip
[LOG 20:44:44.833] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:44.834] DebugTools, --------->TabSubassembly has components:
[LOG 20:44:44.834] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.835] DebugTools, ............c: UIStateToggleBtn
[LOG 20:44:44.836] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.836] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.837] DebugTools, ............c: EditorPartButton
[LOG 20:44:44.837] DebugTools, ............c: EditorShowTooltip
[LOG 20:44:44.838] DebugTools, --------->TabStructural has components:
[LOG 20:44:44.838] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.839] DebugTools, ............c: UIStateToggleBtn
[LOG 20:44:44.840] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.840] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.841] DebugTools, ............c: EditorPartButton
[LOG 20:44:44.842] DebugTools, ............c: EditorShowTooltip
[LOG 20:44:44.842] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:44.843] DebugTools, --------->TabUtility has components:
[LOG 20:44:44.843] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.844] DebugTools, ............c: UIStateToggleBtn
[LOG 20:44:44.844] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.845] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.846] DebugTools, ............c: EditorPartButton
[LOG 20:44:44.846] DebugTools, ............c: EditorShowTooltip
[LOG 20:44:44.847] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:44.847] DebugTools, --------->TabPod has components:
[LOG 20:44:44.848] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.848] DebugTools, ............c: UIStateToggleBtn
[LOG 20:44:44.849] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.850] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.850] DebugTools, ............c: EditorPartButton
[LOG 20:44:44.851] DebugTools, ............c: EditorShowTooltip
[LOG 20:44:44.851] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:44.852] DebugTools, ------>IconTopLeft has components:
[LOG 20:44:44.853] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:44.853] DebugTools, ------>IconTransform 0 0 has components:
[LOG 20:44:44.854] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:44.854] DebugTools, --------->PartIcon(Clone) has components:
[LOG 20:44:44.855] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.856] DebugTools, ............c: UIButton
[LOG 20:44:44.856] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.857] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.857] DebugTools, ............c: EditorPartIcon
[LOG 20:44:44.858] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:44.858] DebugTools, ------------>cupola(Clone) icon(Clone) has components:
[LOG 20:44:44.859] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:44.860] DebugTools, --------------->cupola(Clone) has components:
[LOG 20:44:44.860] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.861] DebugTools, ------------------>model has components:
[LOG 20:44:44.862] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:44.862] DebugTools, --------------------->collider_base has components:
[LOG 20:44:44.863] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:44.863] DebugTools, --------------------->collider_top has components:
[LOG 20:44:44.864] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:44.865] DebugTools, --------------------->flagTransform has components:
[LOG 20:44:44.865] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:44.866] DebugTools, --------------------->node_hatch has components:
[LOG 20:44:44.867] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:44.867] DebugTools, --------------------->node_ladder has components:
[LOG 20:44:44.868] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:44.868] DebugTools, --------------------->obj_base has components:
[LOG 20:44:44.869] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:44.870] DebugTools, ........................c: UnityEngine.MeshFilter
[LOG 20:44:44.870] DebugTools, ........................c: UnityEngine.MeshRenderer
[LOG 20:44:44.871] DebugTools, ------>IconTransform 1 0 has components:
[LOG 20:44:44.872] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:44.872] DebugTools, --------->PartIcon(Clone) has components:
[LOG 20:44:44.873] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.873] DebugTools, ............c: UIButton
[LOG 20:44:44.874] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.874] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.875] DebugTools, ............c: EditorPartIcon
[LOG 20:44:44.876] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:44.876] DebugTools, ------------>landerCabinSmall(Clone) icon(Clone) has components:
[LOG 20:44:44.877] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:44.877] DebugTools, --------------->landerCabinSmall(Clone) has components:
[LOG 20:44:44.878] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.879] DebugTools, ------------------>model has components:
[LOG 20:44:44.879] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:44.880] DebugTools, --------------------->collider_center has components:
[LOG 20:44:44.881] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:44.881] DebugTools, ........................c: UnityEngine.MeshFilter
[LOG 20:44:44.882] DebugTools, --------------------->collider_outer has components:
[LOG 20:44:44.883] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:44.883] DebugTools, ........................c: UnityEngine.MeshFilter
[LOG 20:44:44.884] DebugTools, --------------------->node_airlock has components:
[LOG 20:44:44.885] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:44.885] DebugTools, --------------------->node_ladder has components:
[LOG 20:44:44.886] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:44.887] DebugTools, --------------------->obj_base has components:
[LOG 20:44:44.887] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:44.888] DebugTools, ........................c: UnityEngine.MeshFilter
[LOG 20:44:44.889] DebugTools, ........................c: UnityEngine.MeshRenderer
[LOG 20:44:44.889] DebugTools, --------------------->flagTransform has components:
[LOG 20:44:44.890] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:44.890] DebugTools, ------>IconTransform 2 0 has components:
[LOG 20:44:44.891] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:44.892] DebugTools, --------->PartIcon(Clone) has components:
[LOG 20:44:44.892] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.893] DebugTools, ............c: UIButton
[LOG 20:44:44.893] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.894] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.895] DebugTools, ............c: EditorPartIcon
[LOG 20:44:44.895] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:44.896] DebugTools, ------------>Mark1Cockpit(Clone) icon(Clone) has components:
[LOG 20:44:44.896] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:44.897] DebugTools, --------------->Mark1Cockpit(Clone) has components:
[LOG 20:44:44.898] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.898] DebugTools, ------------------>model has components:
[LOG 20:44:44.899] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:44.899] DebugTools, --------------------->C7A_Cockpit_1-1 has components:
[LOG 20:44:44.900] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:44.901] DebugTools, ------------------------>airlock has components:
[LOG 20:44:44.901] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.902] DebugTools, ------------------------>Cockpitmk1 has components:
[LOG 20:44:44.903] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.903] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:44.904] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:44.905] DebugTools, ------------------------>ladder has components:
[LOG 20:44:44.905] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.906] DebugTools, ------------------------>HandHold has components:
[LOG 20:44:44.906] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.907] DebugTools, ------------------------>node_collider has components:
[LOG 20:44:44.908] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.908] DebugTools, ------------------------>flagTransform has components:
[LOG 20:44:44.909] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.910] DebugTools, ------>IconTransform 0 1 has components:
[LOG 20:44:44.910] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:44.911] DebugTools, --------->PartIcon(Clone) has components:
[LOG 20:44:44.912] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.912] DebugTools, ............c: UIButton
[LOG 20:44:44.913] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.913] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.914] DebugTools, ............c: EditorPartIcon
[LOG 20:44:44.914] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:44.915] DebugTools, ------------>Mark2Cockpit(Clone) icon(Clone) has components:
[LOG 20:44:44.916] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:44.916] DebugTools, --------------->Mark2Cockpit(Clone) has components:
[LOG 20:44:44.917] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.917] DebugTools, ------------------>model has components:
[LOG 20:44:44.918] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:44.919] DebugTools, --------------------->C7A_Cockpit_1-2 has components:
[LOG 20:44:44.919] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:44.920] DebugTools, ------------------------>airlock has components:
[LOG 20:44:44.921] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.921] DebugTools, ------------------------>Cylinder001 has components:
[LOG 20:44:44.922] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.922] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:44.923] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:44.924] DebugTools, ------------------------>flagTransform has components:
[LOG 20:44:44.924] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.925] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:44.926] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:44.926] DebugTools, ------------------------>ladder has components:
[LOG 20:44:44.927] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.927] DebugTools, ------------------------>node_collider has components:
[LOG 20:44:44.928] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.929] DebugTools, ------------------------>rung has components:
[LOG 20:44:44.929] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.930] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:44.931] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:44.931] DebugTools, ------------------------>rung has components:
[LOG 20:44:44.932] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.933] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:44.933] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:44.934] DebugTools, ------------------------>rung has components:
[LOG 20:44:44.935] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.935] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:44.936] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:44.937] DebugTools, ------------------------>rung has components:
[LOG 20:44:44.937] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.938] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:44.938] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:44.939] DebugTools, ------------------------>rung has components:
[LOG 20:44:44.940] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.940] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:44.941] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:44.942] DebugTools, ------>IconTransform 1 1 has components:
[LOG 20:44:44.942] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:44.943] DebugTools, --------->PartIcon(Clone) has components:
[LOG 20:44:44.944] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.944] DebugTools, ............c: UIButton
[LOG 20:44:44.945] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.945] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.946] DebugTools, ............c: EditorPartIcon
[LOG 20:44:44.946] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:44.947] DebugTools, ------------>mark3Cockpit(Clone) icon(Clone) has components:
[LOG 20:44:44.948] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:44.948] DebugTools, --------------->mark3Cockpit(Clone) has components:
[LOG 20:44:44.949] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.949] DebugTools, ------------------>model has components:
[LOG 20:44:44.950] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:44.951] DebugTools, --------------------->C7FuselageMk3C has components:
[LOG 20:44:44.951] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:44.952] DebugTools, ------------------------>airlock has components:
[LOG 20:44:44.953] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.953] DebugTools, ------------------------>handhold has components:
[LOG 20:44:44.954] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.954] DebugTools, ------------------------>C7FuselageMk3FBXASC045ArFBXASC0452 has components:
[LOG 20:44:44.955] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.956] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:44.956] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:44.957] DebugTools, ------------------------>node_collider has components:
[LOG 20:44:44.958] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.958] DebugTools, ------------------------>flagTransform has components:
[LOG 20:44:44.959] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.960] DebugTools, ------------------------>rung has components:
[LOG 20:44:44.960] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.961] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:44.962] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:44.962] DebugTools, ------------------------>rung has components:
[LOG 20:44:44.963] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.963] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:44.964] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:44.965] DebugTools, ------------------------>rung has components:
[LOG 20:44:44.965] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.966] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:44.967] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:44.967] DebugTools, ------------------------>rung has components:
[LOG 20:44:44.968] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.969] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:44.969] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:44.970] DebugTools, ------>IconTransform 2 1 has components:
[LOG 20:44:44.971] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:44.971] DebugTools, --------->PartIcon(Clone) has components:
[LOG 20:44:44.972] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:44.972] DebugTools, ............c: UIButton
[LOG 20:44:44.973] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:44.973] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:44.974] DebugTools, ............c: EditorPartIcon
[LOG 20:44:44.975] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:44.975] DebugTools, ------------>Mark1-2Pod(Clone) icon(Clone) has components:
[LOG 20:44:44.976] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:44.976] DebugTools, --------------->Mark1-2Pod(Clone) has components:
[LOG 20:44:44.977] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:44.978] DebugTools, ------------------>model has components:
[LOG 20:44:44.978] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:44.979] DebugTools, --------------------->Mk1-2Pod has components:
[LOG 20:44:44.980] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:44.980] DebugTools, ------------------------>airlock has components:
[LOG 20:44:44.981] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.981] DebugTools, ------------------------>external shell has components:
[LOG 20:44:44.982] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:44.983] DebugTools, --------------------------->flagTransform has components:
[LOG 20:44:44.983] DebugTools, ..............................c: UnityEngine.Transform
[LOG 20:44:44.984] DebugTools, ..............................c: UnityEngine.MeshFilter
[LOG 20:44:44.985] DebugTools, ..............................c: UnityEngine.MeshRenderer
[LOG 20:44:44.985] DebugTools, --------------------------->FrontWindow has components:
[LOG 20:44:44.986] DebugTools, ..............................c: UnityEngine.Transform
[LOG 20:44:44.987] DebugTools, ..............................c: UnityEngine.MeshFilter
[LOG 20:44:44.987] DebugTools, ..............................c: UnityEngine.MeshRenderer
[LOG 20:44:44.988] DebugTools, --------------------------->OuterShell has components:
[LOG 20:44:44.989] DebugTools, ..............................c: UnityEngine.Transform
[LOG 20:44:44.989] DebugTools, ..............................c: UnityEngine.MeshFilter
[LOG 20:44:44.990] DebugTools, ..............................c: UnityEngine.MeshRenderer
[LOG 20:44:44.991] DebugTools, --------------------------->rung has components:
[LOG 20:44:44.991] DebugTools, ..............................c: UnityEngine.Transform
[LOG 20:44:44.992] DebugTools, ..............................c: UnityEngine.MeshFilter
[LOG 20:44:44.992] DebugTools, ..............................c: UnityEngine.MeshRenderer
[LOG 20:44:44.993] DebugTools, --------------------------->rung has components:
[LOG 20:44:44.994] DebugTools, ..............................c: UnityEngine.Transform
[LOG 20:44:44.994] DebugTools, ..............................c: UnityEngine.MeshFilter
[LOG 20:44:44.995] DebugTools, ..............................c: UnityEngine.MeshRenderer
[LOG 20:44:44.996] DebugTools, --------------------------->rung has components:
[LOG 20:44:44.996] DebugTools, ..............................c: UnityEngine.Transform
[LOG 20:44:44.997] DebugTools, ..............................c: UnityEngine.MeshFilter
[LOG 20:44:44.998] DebugTools, ..............................c: UnityEngine.MeshRenderer
[LOG 20:44:44.998] DebugTools, --------------------------->SideWindow has components:
[LOG 20:44:44.999] DebugTools, ..............................c: UnityEngine.Transform
[LOG 20:44:45.000] DebugTools, ..............................c: UnityEngine.MeshFilter
[LOG 20:44:45.000] DebugTools, ..............................c: UnityEngine.MeshRenderer
[LOG 20:44:45.001] DebugTools, ------------------------>ladder has components:
[LOG 20:44:45.001] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:45.002] DebugTools, ------------------------>mesh collider has components:
[LOG 20:44:45.003] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:45.003] DebugTools, ------>IconTransform 0 2 has components:
[LOG 20:44:45.004] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.005] DebugTools, --------->PartIcon(Clone) has components:
[LOG 20:44:45.005] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:45.006] DebugTools, ............c: UIButton
[LOG 20:44:45.006] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:45.007] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:45.007] DebugTools, ............c: EditorPartIcon
[LOG 20:44:45.008] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:45.009] DebugTools, ------------>mk1pod(Clone) icon(Clone) has components:
[LOG 20:44:45.009] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:45.010] DebugTools, --------------->mk1pod(Clone) has components:
[LOG 20:44:45.010] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:45.011] DebugTools, ------------------>model has components:
[LOG 20:44:45.012] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:45.012] DebugTools, --------------------->capsule has components:
[LOG 20:44:45.013] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:45.014] DebugTools, ........................c: UnityEngine.MeshFilter
[LOG 20:44:45.014] DebugTools, ........................c: UnityEngine.MeshRenderer
[LOG 20:44:45.015] DebugTools, ------------------------>flagTransform has components:
[LOG 20:44:45.015] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:45.016] DebugTools, ------------------------>hatch has components:
[LOG 20:44:45.017] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:45.017] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:45.018] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:45.019] DebugTools, ------------------------>rung has components:
[LOG 20:44:45.019] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:45.020] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:45.021] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:45.021] DebugTools, ------------------------>rung has components:
[LOG 20:44:45.022] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:45.022] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:45.023] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:45.024] DebugTools, ------------------------>window has components:
[LOG 20:44:45.024] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:45.025] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:45.026] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:45.026] DebugTools, --------------------->ladder has components:
[LOG 20:44:45.027] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:45.028] DebugTools, --------------------->airlock has components:
[LOG 20:44:45.028] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:45.029] DebugTools, ------>IconTransform 1 2 has components:
[LOG 20:44:45.030] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.030] DebugTools, --------->PartIcon(Clone) has components:
[LOG 20:44:45.031] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:45.031] DebugTools, ............c: UIButton
[LOG 20:44:45.032] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:45.033] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:45.033] DebugTools, ............c: EditorPartIcon
[LOG 20:44:45.034] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:45.034] DebugTools, ------------>mk2LanderCabin(Clone) icon(Clone) has components:
[LOG 20:44:45.035] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:45.036] DebugTools, --------------->mk2LanderCabin(Clone) has components:
[LOG 20:44:45.036] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:45.037] DebugTools, ------------------>model has components:
[LOG 20:44:45.037] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:45.038] DebugTools, --------------------->bottom_collider has components:
[LOG 20:44:45.039] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:45.039] DebugTools, --------------------->cabin has components:
[LOG 20:44:45.040] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:45.041] DebugTools, ........................c: UnityEngine.MeshFilter
[LOG 20:44:45.041] DebugTools, ........................c: UnityEngine.MeshRenderer
[LOG 20:44:45.042] DebugTools, ------------------------>airlock has components:
[LOG 20:44:45.042] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:45.043] DebugTools, ------------------------>ladder has components:
[LOG 20:44:45.044] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:45.045] DebugTools, ------------------------>rung has components:
[LOG 20:44:45.045] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:45.046] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:45.046] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:45.047] DebugTools, ------------------------>rung has components:
[LOG 20:44:45.048] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:45.048] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:45.049] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:45.050] DebugTools, ------------------------>rung has components:
[LOG 20:44:45.050] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:45.051] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:45.051] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:45.052] DebugTools, --------------------->Collider has components:
[LOG 20:44:45.053] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:45.053] DebugTools, --------------------->window_collider has components:
[LOG 20:44:45.054] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:45.055] DebugTools, --------------------->flagTransform has components:
[LOG 20:44:45.055] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:45.056] DebugTools, ------>IconTransform 2 2 has components:
[LOG 20:44:45.056] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.057] DebugTools, --------->PartIcon(Clone) has components:
[LOG 20:44:45.057] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:45.058] DebugTools, ............c: UIButton
[LOG 20:44:45.059] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:45.059] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:45.060] DebugTools, ............c: EditorPartIcon
[LOG 20:44:45.061] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:45.061] DebugTools, ------------>probeCoreCube(Clone) icon(Clone) has components:
[LOG 20:44:45.062] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:45.062] DebugTools, --------------->probeCoreCube(Clone) has components:
[LOG 20:44:45.063] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:45.064] DebugTools, ------------------>model has components:
[LOG 20:44:45.064] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:45.065] DebugTools, --------------------->cube has components:
[LOG 20:44:45.065] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:45.066] DebugTools, ........................c: UnityEngine.MeshFilter
[LOG 20:44:45.067] DebugTools, ........................c: UnityEngine.MeshRenderer
[LOG 20:44:45.067] DebugTools, ------>IconTransform 0 3 has components:
[LOG 20:44:45.068] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.068] DebugTools, --------->PartIcon(Clone) has components:
[LOG 20:44:45.069] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:45.070] DebugTools, ............c: UIButton
[LOG 20:44:45.070] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:45.071] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:45.071] DebugTools, ............c: EditorPartIcon
[LOG 20:44:45.072] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:45.072] DebugTools, ------------>probeCoreHex(Clone) icon(Clone) has components:
[LOG 20:44:45.073] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:45.074] DebugTools, --------------->probeCoreHex(Clone) has components:
[LOG 20:44:45.074] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:45.075] DebugTools, ------------------>model has components:
[LOG 20:44:45.075] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:45.076] DebugTools, --------------------->collider_base has components:
[LOG 20:44:45.077] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:45.077] DebugTools, ........................c: UnityEngine.MeshFilter
[LOG 20:44:45.078] DebugTools, ------------------------>obj_base has components:
[LOG 20:44:45.079] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:45.079] DebugTools, ...........................c: UnityEngine.MeshFilter
[LOG 20:44:45.080] DebugTools, ...........................c: UnityEngine.MeshRenderer
[LOG 20:44:45.081] DebugTools, ------>IconTransform 1 3 has components:
[LOG 20:44:45.081] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.082] DebugTools, --------->PartIcon(Clone) has components:
[LOG 20:44:45.082] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:45.083] DebugTools, ............c: UIButton
[LOG 20:44:45.084] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:45.084] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:45.085] DebugTools, ............c: EditorPartIcon
[LOG 20:44:45.085] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:45.086] DebugTools, ------------>probeCoreOcto(Clone) icon(Clone) has components:
[LOG 20:44:45.087] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:45.087] DebugTools, --------------->probeCoreOcto(Clone) has components:
[LOG 20:44:45.088] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:45.089] DebugTools, ------------------>model has components:
[LOG 20:44:45.089] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:45.090] DebugTools, --------------------->octo has components:
[LOG 20:44:45.091] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:45.091] DebugTools, ........................c: UnityEngine.MeshFilter
[LOG 20:44:45.092] DebugTools, ........................c: UnityEngine.MeshRenderer
[LOG 20:44:45.092] DebugTools, ------>IconTransform 2 3 has components:
[LOG 20:44:45.093] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.094] DebugTools, --------->PartIcon(Clone) has components:
[LOG 20:44:45.094] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:45.095] DebugTools, ............c: UIButton
[LOG 20:44:45.095] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:45.096] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:45.097] DebugTools, ............c: EditorPartIcon
[LOG 20:44:45.097] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:45.098] DebugTools, ------------>probeCoreOcto2(Clone) icon(Clone) has components:
[LOG 20:44:45.098] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:45.099] DebugTools, --------------->probeCoreOcto2(Clone) has components:
[LOG 20:44:45.100] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:45.100] DebugTools, ------------------>model has components:
[LOG 20:44:45.101] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:45.102] DebugTools, --------------------->octo2 has components:
[LOG 20:44:45.102] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:45.103] DebugTools, ........................c: UnityEngine.MeshFilter
[LOG 20:44:45.103] DebugTools, ........................c: UnityEngine.MeshRenderer
[LOG 20:44:45.104] DebugTools, ------>IconTransform 0 4 has components:
[LOG 20:44:45.105] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.105] DebugTools, --------->PartIcon(Clone) has components:
[LOG 20:44:45.106] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:45.106] DebugTools, ............c: UIButton
[LOG 20:44:45.107] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:45.108] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:45.108] DebugTools, ............c: EditorPartIcon
[LOG 20:44:45.109] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:45.109] DebugTools, ------------>probeCoreSphere(Clone) icon(Clone) has components:
[LOG 20:44:45.110] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:45.111] DebugTools, --------------->probeCoreSphere(Clone) has components:
[LOG 20:44:45.111] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:45.112] DebugTools, ------------------>model has components:
[LOG 20:44:45.112] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:45.113] DebugTools, --------------------->sphere has components:
[LOG 20:44:45.114] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:45.114] DebugTools, ........................c: UnityEngine.MeshFilter
[LOG 20:44:45.115] DebugTools, ........................c: UnityEngine.MeshRenderer
[LOG 20:44:45.115] DebugTools, ------------------------>basecol has components:
[LOG 20:44:45.116] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:45.117] DebugTools, ------>IconTransform 1 4 has components:
[LOG 20:44:45.117] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.118] DebugTools, --------->PartIcon(Clone) has components:
[LOG 20:44:45.119] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:45.119] DebugTools, ............c: UIButton
[LOG 20:44:45.120] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:45.120] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:45.121] DebugTools, ............c: EditorPartIcon
[LOG 20:44:45.122] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:45.122] DebugTools, ------------>probeStackLarge(Clone) icon(Clone) has components:
[LOG 20:44:45.123] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:45.123] DebugTools, --------------->probeStackLarge(Clone) has components:
[LOG 20:44:45.124] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:45.125] DebugTools, ------------------>model has components:
[LOG 20:44:45.125] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:45.126] DebugTools, --------------------->collider_base has components:
[LOG 20:44:45.126] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:45.127] DebugTools, ........................c: UnityEngine.MeshFilter
[LOG 20:44:45.128] DebugTools, --------------------->obj_base has components:
[LOG 20:44:45.128] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:45.129] DebugTools, ........................c: UnityEngine.MeshFilter
[LOG 20:44:45.130] DebugTools, ........................c: UnityEngine.MeshRenderer
[LOG 20:44:45.130] DebugTools, ------>IconTransform 2 4 has components:
[LOG 20:44:45.131] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.131] DebugTools, --------->PartIcon(Clone) has components:
[LOG 20:44:45.132] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:45.133] DebugTools, ............c: UIButton
[LOG 20:44:45.133] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:45.134] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:45.134] DebugTools, ............c: EditorPartIcon
[LOG 20:44:45.135] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:45.136] DebugTools, ------------>probeStackSmall(Clone) icon(Clone) has components:
[LOG 20:44:45.136] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:45.137] DebugTools, --------------->probeStackSmall(Clone) has components:
[LOG 20:44:45.137] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:45.138] DebugTools, ------------------>model has components:
[LOG 20:44:45.139] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:45.139] DebugTools, --------------------->Cylinder has components:
[LOG 20:44:45.140] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:45.141] DebugTools, ........................c: UnityEngine.MeshFilter
[LOG 20:44:45.141] DebugTools, ........................c: UnityEngine.MeshRenderer
[LOG 20:44:45.142] DebugTools, ------>IconTransform 0 5 has components:
[LOG 20:44:45.142] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.143] DebugTools, --------->PartIcon(Clone) has components:
[LOG 20:44:45.144] DebugTools, ............c: UnityEngine.Transform
[LOG 20:44:45.144] DebugTools, ............c: UIButton
[LOG 20:44:45.145] DebugTools, ............c: UnityEngine.MeshFilter
[LOG 20:44:45.145] DebugTools, ............c: UnityEngine.MeshRenderer
[LOG 20:44:45.146] DebugTools, ............c: EditorPartIcon
[LOG 20:44:45.146] DebugTools, ............c: UnityEngine.BoxCollider
[LOG 20:44:45.147] DebugTools, ------------>seatExternalCmd(Clone) icon(Clone) has components:
[LOG 20:44:45.148] DebugTools, ...............c: UnityEngine.Transform
[LOG 20:44:45.148] DebugTools, --------------->seatExternalCmd(Clone) has components:
[LOG 20:44:45.149] DebugTools, ..................c: UnityEngine.Transform
[LOG 20:44:45.149] DebugTools, ------------------>model has components:
[LOG 20:44:45.150] DebugTools, .....................c: UnityEngine.Transform
[LOG 20:44:45.151] DebugTools, --------------------->kerbalchair has components:
[LOG 20:44:45.151] DebugTools, ........................c: UnityEngine.Transform
[LOG 20:44:45.152] DebugTools, ........................c: UnityEngine.MeshFilter
[LOG 20:44:45.153] DebugTools, ........................c: UnityEngine.MeshRenderer
[LOG 20:44:45.153] DebugTools, ------------------------>seatPivot has components:
[LOG 20:44:45.154] DebugTools, ...........................c: UnityEngine.Transform
[LOG 20:44:45.155] DebugTools, ------>IconTransform 1 5 has components:
[LOG 20:44:45.155] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.156] DebugTools, ------>IconTransform 2 5 has components:
[LOG 20:44:45.156] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.157] DebugTools, ------>IconTransform 0 6 has components:
[LOG 20:44:45.157] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.158] DebugTools, ------>IconTransform 1 6 has components:
[LOG 20:44:45.159] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.159] DebugTools, ------>IconTransform 2 6 has components:
[LOG 20:44:45.160] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.160] DebugTools, ------>ShipTemplate 0 has components:
[LOG 20:44:45.161] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.162] DebugTools, ------>ShipTemplate 1 has components:
[LOG 20:44:45.162] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.163] DebugTools, ------>ShipTemplate 2 has components:
[LOG 20:44:45.163] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.164] DebugTools, ------>ShipTemplate 3 has components:
[LOG 20:44:45.164] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.165] DebugTools, ------>ShipTemplate 4 has components:
[LOG 20:44:45.166] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.166] DebugTools, ------>ShipTemplate 5 has components:
[LOG 20:44:45.167] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.167] DebugTools, ------>ShipTemplate 6 has components:
[LOG 20:44:45.168] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.169] DebugTools, ------>ShipTemplate 7 has components:
[LOG 20:44:45.169] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.170] DebugTools, ------>ShipTemplate 8 has components:
[LOG 20:44:45.170] DebugTools, .........c: UnityEngine.Transform
[LOG 20:44:45.171] DebugTools, ------>ShipTemplate 9 has components:
[LOG 20:44:45.171] DebugTools, .........c: UnityEngine.Transform
 */
        }
    }

    class PanelLogic : MonoBehaviour
    {
    }

//[KSPAddon(KSPAddon.Startup.Flight, true)]
//class DumpShaderList : MonoBehaviour
//{
//    void Start()
//    {
//        HashSet<string> shaders = new HashSet<string>();

//        FindObjectsOfType<Shader>().ToList().ForEach(sh => shaders.Add(sh.name));
//        Resources.FindObjectsOfTypeAll<Shader>().ToList().ForEach(sh => shaders.Add(sh.name));

//        Log.Normal("{0} loaded shaders", shaders.Count);
//        List<string> sorted = new List<string>(shaders); sorted.Sort();

//        using (System.IO.StreamWriter file = new System.IO.StreamWriter(KSPUtil.ApplicationRootPath + "/shaders.txt"))
//            foreach (var sh in sorted)
//                file.WriteLine(sh);
//    }
//}

//[KSPAddon(KSPAddon.Startup.Flight, false)]
//class StopAnnoyingAsteroids : MonoBehaviour
//{
    
//    public void LogCallback(string condition, string stackTrace, LogType type)
//    {
//        Log.Warning("callback: " + condition);
//    }

//    void CrewKilled(EventReport data)
//    {
//        Log.Warning("CrewKilled! " + data.msg);
//    }

//    void CrewStatusChange(ProtoCrewMember crew, ProtoCrewMember.RosterStatus oldStatus, ProtoCrewMember.RosterStatus newStatus)
//    {
//        Log.Write("status change for " + crew.name + " to " + newStatus);
//    }

//    void Start()
//    {
//        GameEvents.onCrewKilled.Add(CrewKilled);
//        GameEvents.onKerbalStatusChange.Add(CrewStatusChange);

//    }

//    void Destroy()
//    {
//        GameEvents.onCrewKilled.Remove(CrewKilled);
//        GameEvents.onKerbalStatusChange.Remove(CrewStatusChange);
//    }
//}

    //[KSPAddon(KSPAddon.Startup.Flight, false)]
    //class FlightIdTest : MonoBehaviour
    //{
    //    void Update()
    //    {
    //        if (Input.GetKeyDown(KeyCode.F))
    //        {
    //            Log.Write("FlightId = " + FlightGlobals.ActiveVessel.rootPart.flightID);
    //            Log.Write("mid = " + FlightGlobals.ActiveVessel.rootPart.missionID);
    //            Log.Write("id = " + FlightGlobals.ActiveVessel.id.GetHashCode());
    //            Log.Write("uid = " + FlightGlobals.ActiveVessel.rootPart.uid);
    //            Log.Write("constructID = " + FlightGlobals.ActiveVessel.rootPart.ConstructID);

    //            Log.Write("Part.Flightids for " + FlightGlobals.ActiveVessel.vesselName);
    //            FlightGlobals.ActiveVessel.parts.ForEach(p => Log.Write("{0}: {1}", p.partInfo.partPrefab.name, p.flightID));

    //        }
    //    }
    //}

    //[KSPAddon(KSPAddon.Startup.EditorAny, false)]
    //class TestFont : MonoBehaviour
    //{

    //    void Start()
    //    {
    //        var text = gameObject.AddComponent<SpriteText>();
    //        gameObject.layer = 25;

    //        var rcam = Camera.allCameras.Where(cam => (cam.cullingMask & (1 << gameObject.layer)) != 0).Single();

    //        Vector2 screenPos = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

    //        transform.position = new Vector3(UIManager.instance.transform.position.x + screenPos.x - Screen.width * 0.5f,
    //                                         UIManager.instance.transform.position.y - screenPos.y + Screen.height * 0.5f,
    //                                         rcam.nearClipPlane);

    //        text.Text = "The quick brown fox jumps over the lazy brown dog six\n times before the fox more text \nmore text even more text blah";
    //        text.alignment = SpriteText.Alignment_Type.Right;
    //        text.font = UIManager.instance.defaultFont;
    //        text.renderer.sharedMaterial = UIManager.instance.defaultFontMaterial;
    //        text.SetColor(Color.white);
    //        text.SetAnchor(SpriteText.Anchor_Pos.Middle_Center);
    //        text.SetCharacterSize(24f);
    //        //text.SetLineSpacing(4f);
    //        text.maxWidthInPixels = true;
    //        text.maxWidth = Screen.width * 0.5f - 25f;

    //        Log.Write("default shader: " + text.renderer.sharedMaterial.shader.name);

    //        var tex = ResourceUtil.LocateTexture("bmfont_exocet_0.png", false);
    //        Material mat = new Material(Shader.Find("Sprite/Vertex Colored")) { mainTexture = tex };
    //        var asset = new TextAsset();

    //        Log.Write("path = " + ConfigUtil.GetDllDirectoryPath() + "/bmfont_exocet.fnt");

    //        var bundle = AssetBundle.CreateFromFile(ConfigUtil.GetDllDirectoryPath() + "/bmfont_exocet.fnt");
    //        if (bundle != null) Log.Warning("bundle worked!");

    //        //asset.text = System.IO.File.ReadAllBytes(ConfigUtil.GetDllDirectoryPath() + "bmfont_exocet.fnt");
            

    //    }
    //}

//[KSPAddon(KSPAddon.Startup.Flight, false)]
//class DuplicateRepWidget : MonoBehaviour
//{
//    Gauge gauge;

//    IEnumerator Start()
//    {
//        var widget = FindObjectOfType<CurrencyWidgetsApp>();
//        while (!widget.widgetSpawner.Spawned) yield return 0;

//        var stockRep = widget.widgetSpawner.transform.Find("RepWidget");

//        var newRep = (GameObject)Instantiate(stockRep.gameObject, stockRep.transform.position, stockRep.transform.rotation);
//        newRep.SetActive(true);
//        newRep.transform.Translate(new Vector3(-Screen.width * 0.5f, 0f, 0f));

//        Component.Destroy(newRep.GetComponent<ReputationWidget>());

//        gauge = newRep.GetComponentInChildren<Gauge>();

//        StartCoroutine(TickTock());
//    }

//    IEnumerator TickTock()
//    {
//        gauge.minValue = 0f;
//        gauge.maxValue = 100f;
//        float delta = 33f;

//        while (true)
//        {
//            float newValue = gauge.Value + delta * Time.deltaTime;
//            if (newValue > 100f || newValue < 0f)
//                delta = -delta;

//            gauge.setValue(Mathf.Clamp(newValue, gauge.minValue, gauge.maxValue));

//            yield return 0;
//        }
//    }
//}

//[KSPAddon(KSPAddon.Startup.EditorAny, false)]
//class EditPartListPrefab : MonoBehaviour
//{
//    class ClickListener : MonoBehaviour
//    {
//        AvailablePart myPart;
//        EditorPartIcon icon;
//        SpriteText text;
//        int amount = 0;
//        bool mouseFlag = false;



//        void Start()
//        {
//            GetComponent<UIButton>().AddValueChangedDelegate(OnClick);
//            GetComponent<UIButton>().AddInputDelegate(OnInput);
//            myPart = GetComponent<EditorPartIcon>().partInfo;
//            icon = GetComponent<EditorPartIcon>();
//            text = transform.Find("CounterLabel").GetComponent<SpriteText>();

//            amount = EditPartListPrefab.Instance.GetPartQuantity(myPart);
//            UpdateCounter();
//        }

//        void OnClick(IUIObject obj)
//        {
//            print("User clicked " + myPart.partPrefab.name);

//            if (amount > 0)
//            {
//                --amount;
//                EditPartListPrefab.Instance.SetPartQuantity(myPart, amount);
//                UpdateCounter();
//            }
//        }

//        void UpdateCounter()
//        {
//            text.Text = amount.ToString();

//            if (amount == 0)
//                icon.SetGrey("Ran out of this item");
//        }


//        void OnInput(ref POINTER_INFO ptr)
//        {
//            switch (ptr.evt)
//            {
//                case POINTER_INFO.INPUT_EVENT.MOVE:
//                    if (!mouseFlag)
//                    {
//                        mouseFlag = true;
//                        print("User moused over " + myPart.partPrefab.name);
//                    }
//                    break;
//                case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
//                    mouseFlag = false;
//                    break;
//            }
//        }
//    }



//    public static EditPartListPrefab Instance { private set; get; }
//    private Dictionary<AvailablePart, int> quantities = new Dictionary<AvailablePart, int>();


//    int GetPartQuantity(AvailablePart part)
//    {
//        return quantities[part];
//    }

//    void SetPartQuantity(AvailablePart part, int qty)
//    {
//        quantities[part] = qty;
//    }

//    void Start()
//    {
//        Instance = this;

//        // edit icon prefab
//        var iconPrefab = EditorPartList.Instance.iconPrefab.gameObject;

//        if (iconPrefab.GetComponent<ClickListener>() == null)
//        {
//            iconPrefab.AddComponent<ClickListener>();

//            GameObject labelHolder = new GameObject("CounterLabel");
//            var label = labelHolder.AddComponent<SpriteText>();
//            label.RenderCamera = Camera.allCameras.Where(c => (c.cullingMask & (1 << labelHolder.layer)) != 0).Single();

//            labelHolder.layer = LayerMask.NameToLayer("EzGUI_UI");
//            labelHolder.transform.parent = iconPrefab.transform;
//            labelHolder.transform.localPosition = Vector3.zero;
//            labelHolder.transform.Translate(new Vector3(EditorPartList.Instance.iconSize * 0.35f, EditorPartList.Instance.iconSize * -0.425f, label.RenderCamera.nearClipPlane - labelHolder.transform.position.z - 1f), Space.Self);

//            label.Text = "[count]";
//            label.alignment = SpriteText.Alignment_Type.Right;
//            label.font = UIManager.instance.defaultFont;
//            label.renderer.sharedMaterial = UIManager.instance.defaultFontMaterial;
//            label.SetColor(Color.white);
//            label.SetAnchor(SpriteText.Anchor_Pos.Lower_Right);
//            label.SetCharacterSize(12f);
//        }

//        // generate some random quantity for each part
//        PartLoader.LoadedPartsList.ForEach(ap => quantities.Add(ap, UnityEngine.Random.Range(0, 15)));
//    }
//}

//[KSPAddon(KSPAddon.Startup.Flight, false)]
//[RequireComponent(typeof(AudioSource))]
//class TestAudio : MonoBehaviour
//{
//    void Start()
//    {
//        audio.clip = GameDatabase.Instance.GetAudioClip("folder/clip");
//        audio.panLevel = 0f;

//        GameEvents.onStageSeparation.Add(StageSeparated);
//        GameEvents.onGamePause.Add(OnPause);
//    }

//    void OnDestroy() { 
//        GameEvents.onStageSeparation.Remove(StageSeparated);
//        GameEvents.onGamePause.Remove(OnPause);
//    }

//    void StageSeparated(EventReport report) { audio.Play(); }

//    void OnPause() { audio.Play(); }
//}
    //[KSPAddon(KSPAddon.Startup.Flight, false)]
    //[RequireComponent(typeof(AudioSource)]
    public class KerbalComms : MonoBehaviour
    {

        public string SoundFile_OnLaunch = "DebugTools/clip";
        public FXGroup SoundGroup = null;

        public void Awake() { }

        public void Start()
        {
            print("KerbalComms: Plugin started correctly!!");
            //GameEvents.onLaunch.Add(OnLaunch);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                print("KerbalComms: Aaaaaand... LIFTOFF !!! (OnLaunch)");
                ScreenMessages.PostScreenMessage("Aaaaaand... LIFTOFF !!!", 5.0f, ScreenMessageStyle.UPPER_CENTER);

                if (!GameDatabase.Instance.ExistsAudioClip(SoundFile_OnLaunch))
                {
                    Debug.LogError("KerbalComms: Audio file not found: " + SoundFile_OnLaunch);
                    return;
                }

                SoundGroup = new FXGroup("somesound");
                SoundGroup.audio = gameObject.AddComponent<AudioSource>();
                SoundGroup.audio.panLevel = 0f;
                SoundGroup.audio.clip = GameDatabase.Instance.GetAudioClip(SoundFile_OnLaunch);
                SoundGroup.audio.Play();
                SoundGroup.audio.loop = false;
            }
        }

    }

    //[KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class SkinnedMeshScaleFix : MonoBehaviour
    {
        System.Collections.IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            
        }


        void Update()
        {
            //Log.Write("size = " + EditorPartList.Instance.iconSize);
            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                
                var batteryLarge = FindObjectsOfType<EditorPartIcon>().ToList().Find(epi => epi.partInfo.name == "batteryBankLarge");

                foreach (var c in Camera.allCameras)
                    if ((c.cullingMask & (1 << batteryLarge.transform.GetChild(0).gameObject.layer)) != 0)
                    {
                        Log.Write("closer ...");

                        //var camPos = c.transform.position;
                        //var batPos = batteryLarge.transform.GetChild(0).position;
                        //var dir = camPos - batPos;

                        //Log.Write("camPos = " + camPos);
                        //Log.Write("batPos = " + batPos);
                        //Log.Write("dist = " + dir);

                        //batteryLarge.transform.GetChild(0).Translate((c.transform.position - batteryLarge.transform.GetChild(0).position) * 0.05f);
                        //batteryLarge.transform.GetChild(0).Translate(dir * 0.05f, Space.World);

                        //batteryLarge.transform.GetChild(0).position += c.transform.rotation * Vector3.forward * -10f;

                        Log.Write("orthosize = " + c.orthographicSize);
                        Log.Write("viewdims: {0}x{1}", c.aspect * c.orthographicSize * 2f, c.orthographicSize * 2f);

                        // iconSize = 80f

                        break;
                    }
                
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                foreach (var icon in FindObjectsOfType<EditorPartIcon>())
                {
                    Log.Write("icon = " + icon.partInfo.name + ", scale " + icon.transform.GetChild(0).localScale.FString());
                   
                }

                // bad one
                var target = FindObjectsOfType<EditorPartIcon>().ToList().Find(epi => epi.partInfo.name == "MoleTrack");
                target.gameObject.PrintComponents();

                Log.Write("40560 = " + StringDumper.GetKSPString(40560));
                Log.Write("50071 = " + StringDumper.GetKSPString(50071));

                // calculate actual bounds
                // note: ksp already modified its overall scale so we mustn't
                // forget to reset it first 
                var strippedPart = target.transform.GetChild(0).gameObject; // this appears to be a part prefab, but stripped of
                                                                            // unnecessary bits


                ///////////////////////////////////////////////////////////////////
                // Moment of truth
                ///////////////////////////////////////////////////////////////////

                //var strippedModel = target.transform.GetChild(0).GetChild(0).gameObject;
                //var originalPos = strippedModel.transform.position; // we'll be needing this to preserve the icon's
                //// screen x/y coordinates

                //Log.Write("originalPos = " + originalPos.FString());

                //var oldParent = strippedModel.transform.parent;
                //strippedModel.transform.parent = null;

                //strippedModel.transform.localScale = Vector3.one;
                

                //strippedModel.transform.PrintLocalScales();

                //var modelBounds = CalculateBounds(strippedModel);
                //float d = Mathf.Max(modelBounds.size.x, modelBounds.size.y, modelBounds.size.z);

                //strippedModel.transform.position = originalPos;
                //strippedModel.transform.localScale = Vector3.one * d * 0.5f;

                //Log.Write("modelBounds = " + modelBounds);
                //Log.Write("model diameter = " + d);

                //// iconSize = 80 units, don't forget there's a 40 scalar in there already

                //var debugPoint = DebugVisualizer.Point(strippedModel.transform, d / 0.5f * 2f);
                ////var debugPoint = DebugVisualizer.Point(originalPos, 80f / d); // works
                ////var debugPoint = DebugVisualizer.Point(strippedModel.transform, 1f);
                //debugPoint.renderer.material = ResourceUtil.LocateMaterial("DebugTools.XrayShader.shader");
                //debugPoint.layer = 25;
                //debugPoint.AddComponent<EditorPartIconTestResizer>();
                //debugPoint.SetActive(true);
                //debugPoint.transform.position = originalPos;

                //strippedModel.transform.parent = oldParent;




                var prefab = PartLoader.getPartInfoByName(target.partInfo.partPrefab.name).partPrefab.gameObject;
                var newBounds = CalculateBounds(prefab);

                

                Action<GameObject, Vector3> recursiveScale = null;

                recursiveScale = delegate(GameObject go, Vector3 newScale)
                {
                    go.transform.localScale = newScale;

                    foreach (Transform child in go.transform)
                        recursiveScale(child.gameObject, newScale);
                };

                Action<GameObject, EditorPartIcon> recursiveScalePrinter = null;
                Func<GameObject, EditorPartIcon, string> FindPath = null;

                FindPath = delegate(GameObject go, EditorPartIcon overallOwner)
                 {
                     if (go.transform.parent == null || go == overallOwner.gameObject)
                         return go.transform.name;

                     if (go.transform.parent.name == "model")
                         return go.transform.name;

                     return FindPath(go.transform.parent.gameObject, overallOwner) + "/" + go.transform.name;
                 };

                recursiveScalePrinter = delegate(GameObject go, EditorPartIcon epi)
                {
                    Log.Write("{0} scale: {1}", FindPath(go, epi), go.transform.localScale.FString());

                    foreach (Transform t in go.transform)
                        recursiveScalePrinter(t.gameObject, epi);
                };

                var batteryLarge = FindObjectsOfType<EditorPartIcon>().ToList().Find(epi => epi.partInfo.name == "batteryBankLarge");
                var batterySmall = FindObjectsOfType<EditorPartIcon>().ToList().Find(epi => epi.partInfo.name == "batteryPack");


                //strippedPart.transform.localScale = Vector3.one;

                //strippedPart.transform.GetChild(0).localScale = Vector3.one;
                //recursiveScale(strippedPart, Vector3.one);
                Log.Write("-------- prefab ----------");
                //recursiveScalePrinter(prefab, target);

                Log.Write("-------- stripped ----------");
                //recursiveScalePrinter(strippedPart, target);
                target.transform.PrintLocalScales();

                Log.Write("--------- batteryLarge --------");
                //recursiveScalePrinter(batteryLarge.gameObject, batteryLarge);


                batteryLarge.transform.PrintLocalScales();
                


                //Log.Write("prefab scale: " + prefab.transform.localScale);
                //Log.Write("stripped parent scale: " + strippedPart.transform.parent.localScale);

                //// it's probably clumsy but let's just use a sphere to approximate total size
                //float sphereBounds = Mathf.Max(newBounds.size.x, newBounds.size.y, newBounds.size.z);
                //Log.Write("prefab bounds: " + newBounds.size);

                ////newBounds = CalculateBounds(strippedPart);
                ////Log.Write("stripped bounds: " + newBounds.size);

                //Log.Write("approximate size: {0} m", sphereBounds);
                //Log.Write("stripped size: {0} m", Mathf.Max(newBounds.size.x, newBounds.size.y, newBounds.size.z));
                //Log.Write("stripped layer: " + strippedPart.layer);

                


                // toggle off all editor icons except the ones we're working on
                foreach (var item in FindObjectsOfType<EditorPartIcon>())
                {
                    if ((item != target) && (item.partInfo.name != "batteryBankLarge"))
                        item.gameObject.SetActive(false);

                    if (item.partInfo.name == "batteryBankLarge")
                    {
                        // let's test out a sphere
                        var model = item.transform.GetChild(0).GetChild(0).gameObject;
                        var oldPos = model.transform.position;

                        Log.Write("batteryBankLarge model: " + model.name);

                        var b = GetPartBounds(model);
                        model.transform.position = oldPos;

                        Log.Write("battery bounds: " + b.ToString());

                        

                        float diameter = Mathf.Max(b.size.x, b.size.y, b.size.z);

                        Log.Write("diameter: " + diameter);

                        var sphere = DebugVisualizer.Point(model.transform, diameter);
                        sphere.renderer.material = ResourceUtil.LocateMaterial("DebugTools.XrayShader.shader", "Particles/Additive");
                        //sphere.AddComponent<EditorPartIconTestResizer>();

                        sphere.layer = 25;

                        sphere.SetActive(true);
                    }
                }

                foreach (var c in Camera.allCameras)
                    if ((c.cullingMask & (1 << strippedPart.layer)) != 0)
                    {
                        Log.Warning("Stripped camera: " + c.name);
                        c.CaptureSingleFrame("forced_frame.png");
                    }

                FindObjectsOfType<EditorPartIcon>().ToList().ForEach(epi => Log.Write("epi: " + epi.partInfo.name));

                // compare with a good one
                

                Log.Write("batteryLarge position: " + batteryLarge.transform.GetChild(0).position);
                Log.Write("batterySmall position: " + batterySmall.transform.GetChild(0).position);



                //good.gameObject.GetComponent<UIButton>().renderer.Cutout(good.gameObject.GetComponent<UIButton>().GetUVs()).SaveToDisk("button_cutout.png");
                //((Texture2D)good.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture).CreateReadable().SaveToDisk("other_tex.png");




                //Log.Write("good bounds: {0}", good.gameObject.GetComponent<BoxCollider>().bounds.ToString());
                //Log.Write("good position: {0}", good.gameObject.transform.position);

                //target.gameObject.transform.position = good.gameObject.transform.position;

                //var bounds = (Bounds)target.GetType().GetMethod("GetPartBounds", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(target, new object[] { target.transform.GetChild(0).gameObject });
                //Log.Write("calculated target bounds: {0}", bounds.ToString());
                //Log.Write("recalc: {0}", RecalculateBounds(target.transform.GetChild(0).gameObject));

                //var goodbounds = (Bounds)target.GetType().GetMethod("GetPartBounds", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(good, new object[] { good.transform.GetChild(0).gameObject });
                //Log.Write("calculated good bounds: {0}", goodbounds.ToString());
                //Log.Write("recalc good (fresh prefab): {0}", RecalculateBounds(good.partInfo.partPrefab.gameObject));
                //good.transform.GetChild(0).localScale = good.partInfo.partPrefab.transform.localScale;
                //Log.Write("recalc good (attached prefab): {0}", RecalculateBounds(good.transform.GetChild(0).gameObject));
                //Log.Write("recalc good (partloader): {0}", RecalculateBounds(PartLoader.getPartInfoByName(good.partInfo.partPrefab.name).partPrefab.gameObject));
                //goodbounds = (Bounds)target.GetType().GetMethod("GetPartBounds", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(good, new object[] { good.transform.GetChild(0).gameObject });
                //Log.Write("calculated good bounds, with scale reset: {0}", goodbounds);
                //Log.Write("good scale: " + good.transform.GetChild(0).localScale);
                //Log.Write("good pos: " + good.transform.position);

                //var original = good.partInfo.partPrefab.gameObject;
                //var active = good.transform.GetChild(0).gameObject;

                //if (!original.activeSelf) Log.Warning("original inactive");

                //original.SetActive(true);

                //active.transform.localScale = original.transform.localScale;

                //Log.Write("original scale: " + original.transform.localScale);
                //Log.Write("active scale: " + active.transform.localScale);

                //original.transform.position = Vector3.zero;

                ////PrintResult("Active GO, stock method, scale 1", StockMethod(active, good));
                //PrintResult("Active GO, custom method, scale 1", RecalculateBounds(active));
                //PrintResult("Active GO, copyapasta, scale 1", GetPartBounds(active));
                ////PrintResult("Original GO, stock method, scale 1", StockMethod(original, good));
                ////PrintResult("Original GO, custom method, scale 1", RecalculateBounds(original));



            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                foreach (var c in Camera.allCameras)
                {
                    Log.Write("Camera: {0}", c.name);

                    if (c.name == "tooltip camera")
                    {
                        c.gameObject.PrintComponents();
                        Log.Write("cam pos: {0}", c.transform.position);
                    }
                }
            }

            
        }

        void PrintResult(string message, Bounds bounds)
        {
            Log.Write("{0}: c{1} sz{2}", message, bounds.center.FString(), bounds.size.FString());
        }


        Bounds StockMethod(GameObject go, EditorPartIcon epi)
        {
            return (Bounds)epi.GetType().GetMethod("GetPartBounds", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(epi, new object[] { go });
        }


        /// <summary>
        /// Given a part prefab, come up with a bounding box that completely encompasses
        /// all individual mesh bounds. We'll be using this to come up with a rough idea
        /// of how large the part is so we can position it properly in the editor icon
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        Bounds CalculateBounds(GameObject part)
        {
            Bounds bounds = new Bounds();

            part.transform.position = Vector3.zero;

            // for sake of avoiding code duplication
            //
            // each individual mesh has its own bounding area, but it's
            // only useful if we know the actual world area it'll be occuping
            // relative to the others
            Func<Bounds, Transform, Bounds> toWorldSpace = delegate(Bounds local, Transform t) 
            {
                var worldBounds = default(Bounds);

                worldBounds.center = t.TransformPoint(local.center);
                worldBounds.size = t.TransformPoint(local.size);

                //if (Mathf.Max(worldBounds.size.x, worldBounds.size.y, worldBounds.size.z) > 500f)
                //{
                //    Log.Error("abnormal size: " + t.name);
                //    t.gameObject.PrintComponents();

                //    var smr = t.gameObject.GetComponent<SkinnedMeshRenderer>();
                //    Mesh mesh = new Mesh();

                //    smr.BakeMesh(mesh);

                //    Log.Write("set bounds: " + smr.sharedMesh.bounds);
                //    mesh.RecalculateBounds();
                //    Log.Write("recalculated bounds: " + mesh.bounds);

                //}

                //Log.Write("worldBounds: " + worldBounds.size + "; scale " + t.localScale);

                return worldBounds;
            };

            // MeshFilters
            part.GetComponentsInChildren<MeshFilter>(true).ToList().ForEach(mf =>
            {
                mf.mesh.RecalculateBounds();

                //Log.Write("Mesh bounds: " + mf.mesh.bounds);

                bounds.Encapsulate(toWorldSpace(mf.mesh.bounds, mf.transform));
            });
            

            // SkinnedMeshRenderers
            //    These are a little more complex. The mesh bounds seem to be wrong for some
            //    reason (in some instances); we'll have to recalculate those
            part.GetComponentsInChildren<SkinnedMeshRenderer>(true).ToList().ForEach(smr =>
            {
                Mesh baked = new Mesh();
                smr.BakeMesh(baked);

                baked.RecalculateBounds();

                bounds.Encapsulate(toWorldSpace(baked.bounds, smr.transform));

                DestroyImmediate(baked);
            });


            // todo: colliders as well? might make mesh too small if a model uses
            // a large invisible collider for something that isn't used as part of the model


            return bounds;
        }



        private Bounds GetPartBounds(GameObject part)
        {
            Bounds result = default(Bounds);
            bool flag = false;
            part.transform.position = Vector3.zero;
            MeshFilter[] componentsInChildren = part.GetComponentsInChildren<MeshFilter>();
            MeshFilter[] array = componentsInChildren;
            for (int i = 0; i < array.Length; i++)
            {
                MeshFilter meshFilter = array[i];
                if (meshFilter.mesh == null)
                {

                }
                else
                {
                    Matrix4x4 localToWorldMatrix = meshFilter.transform.localToWorldMatrix;
                    Bounds bounds = default(Bounds);
                    bounds.center = localToWorldMatrix.MultiplyPoint(meshFilter.mesh.bounds.center);
                    bounds.size = localToWorldMatrix.MultiplyVector(meshFilter.mesh.bounds.size);
                    if (flag)
                    {

                        result.Encapsulate(bounds);
                        Log.Debug("stock expanded: " + result.ToString());
                    }
                    else
                    {
                        result = new Bounds(bounds.center, bounds.size);
                        Log.Write("set new stock part bounds: " + result.ToString());
                        flag = true;
                    }
                }
            }
            while (true)
            {
                switch (2)
                {
                    case 0:
                        continue;
                }
                break;
            }
            return result;
        }
    }


    public class EditorPartIconTestResizer : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                transform.localScale *= 0.5f;
                Log.Write("localScale set to " + transform.localScale.FString());
            }
            else if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                transform.localScale *= 2f;
                Log.Write("localScale set to " + transform.localScale.FString());
            }
        }
    }


    //[KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    public class AppLauncherCreate : MonoBehaviour
    {
        System.Collections.IEnumerator Start()
        {
            Log.Write("Kicking AppLauncher now ...");

            ApplicationLauncher.Instance.gameObject.SendMessage("OnSceneLoaded", (int)GameScenes.EDITOR);
            ApplicationLauncher.Instance.Show();
            while (!ApplicationLauncher.Ready) yield return 0;
        }
    }

    //[KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class AppLauncherInvestigate : MonoBehaviour
    {
        System.Collections.IEnumerator Start()
        {
            while (!ApplicationLauncher.Ready) yield return 0;

            ApplicationLauncher.Instance.gameObject.PrintComponents();
        }
    }

    //[KSPAddon(KSPAddon.Startup.Flight, false)]
    public class DaMichelQuestion : MonoBehaviour
    {
        Rect rect;
        Texture2D green;

        void Start()
        {
            Log.Write("Started");

            var t = gameObject.AddComponent<GUIText>();

            gameObject.layer = 12; // navball layer

            Log.Write("finding navball");
            NavBall flightNavball = GameObject.FindObjectOfType<NavBall>();

            if (flightNavball == null) Log.Error("failed to find navball");
            Log.Write("navball layer: {0}", flightNavball.gameObject.layer);
            flightNavball.gameObject.PrintComponents();

            
            Log.Write("setting position");
            Log.Write("navball position = {0}", flightNavball.transform.position);

            gameObject.transform.parent = flightNavball.transform;
            //Camera cam = Camera.allCameras.FirstOrDefault(c =>
            //{
            //    if ((c.cullingMask & (1 << 12)) != 0)
            //    {
            //        Log.Write("Found camera: {0}", c.name);
            //        return true;
            //    }
            //    return false;
            //});


            Log.Write("finished listing cameras");
            Camera cam = Camera.allCameras.FirstOrDefault(c => (c.cullingMask & (1 << 12)) != 0);
            gameObject.transform.position = cam.WorldToViewportPoint(flightNavball.transform.position);// -new Vector3(0.5f, 0f);
            

            Log.Write("screen position: {0}", gameObject.transform.position);

            t.text = "====*====";
            t.alignment = TextAlignment.Center;
            t.anchor = TextAnchor.MiddleCenter;

            t.material.color = Color.red;

            var child = flightNavball.transform.FindChild("frame");
            //gameObject.transform.position = cam.WorldToViewportPoint(child.position);// -new Vector3(0.5f, 0f);
            var tex = ((Texture2D)child.renderer.material.mainTexture).CreateReadable();
            tex.SaveToDisk("frame_tex.png");

            Log.Write("dimensions: {0}x{1}", tex.width, tex.height);

            var pixels = tex.GetPixels32();

            // 256x211
            // y=211/256x

            // 
            for (int y = 0; y < tex.height; ++y)
                for (int x = 0; x < tex.width; ++x)
                {
                    if (y == (int)((tex.height / (float)tex.width) * x) || y == (int)((tex.height / (float)tex.width) * (tex.width - x)) || x == 0 || y == 0 || x == tex.width - 1 || y == tex.height - 1)
                    {
                        //pixels[y * tex.width + x] = Color.clear;
                        pixels[y * tex.width + x] = Color.red;
                        //Log.Write("hiding {0}x{1}", x, y);
                    } else pixels[y * tex.width + x] = Color.clear;
                }

            tex.SetPixels32(pixels);
            tex.Apply();

            child.renderer.material.mainTexture = tex;

            // vectorsPivot 
            // shading 
            // navball 
            // headingText 
            // heading
            // cursor 

            //foreach (Transform ch in flightNavball.transform)
            //{
            //    if (ch.name != "frame" && ch.name != "DaMichelQuestion")
            //    {
            //        ch.gameObject.SetActive(false);
            //        Log.Write("Hiding {0}", ch.name);
            //    }
            //}

            rect = t.GetScreenRect();
            green = new Texture2D(1, 1);
            green.SetPixels32(new Color32[] { new Color(0f, 1f, 0f, 0.25f) });
            green.Apply();

            GameObject go = new GameObject("RectDrawer");
            var rd = go.AddComponent<RectDrawer>();

            rect.y = Screen.height - rect.y - rect.height;

            rd.rect = rect;
            rd.green = green;

            Log.Write("screenRect = {0}", rect);
        }

        
    }


    public class RectDrawer : MonoBehaviour
    {
        public Rect rect;
        public Texture2D green;

        void OnGUI()
        {
            if (Event.current.type == EventType.Repaint)
            {
                //Graphics.DrawTexture(rect, green);
                GUI.DrawTexture(rect, green);
                //Log.Write("drew texture");
            }
        }
    }

    //[KSPAddon(KSPAddon.Startup.EditorAny, true)]
    public class GuiButtonTestEditor : MonoBehaviour
    {
        UIButton blocker;

        void Start()
        {
            blocker = GuiUtil.CreateBlocker(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, Screen.width * 0.5f, Screen.height * 0.5f), EditorLogic.fetch.exitBtn.transform.position.z);
            //blocker.transform.position = new Vector3(blocker.transform.position.x, blocker.transform.position.y, EditorLogic.fetch.exitBtn.transform.position.z);

            ////blocker.transform.position = new Vector3(UIManager.instance.transform.position.x - Screen.width * 0.5f, UIManager.instance.transform.position.y + Screen.height * 0.5f, EditorLogic.fetch.exitBtn.transform.position.z + 1f);
            //var correct = blocker.transform.position = new Vector3(UIManager.instance.transform.position.x - Screen.width * 0.5f, UIManager.instance.transform.position.y + Screen.height * 0.5f, EditorLogic.fetch.exitBtn.transform.position.z + 1f);
            //Log.Write("Blocker positioned at {0}", blocker.transform.position);
            //Log.Write("correct = {0}", correct);
            //Log.Write("UImanager.position = {0}", UIManager.instance.transform.position);
            //Log.Write("exit button at {0}", EditorLogic.fetch.exitBtn.transform.position);
        }
    }

    //[KSPAddonImproved(KSPAddonImproved.Startup.SpaceCenter, true)]
    public class GuiButtonTest : MonoBehaviour
    {
        UIButton blocker;

        void Start()
        {
            blocker = GuiUtil.CreateBlocker(new Rect(0, 0, Screen.width * 0.5f, Screen.height * 0.5f));        
        }
    }

   

    //[KSPAddon(KSPAddon.Startup.Flight, false)]
    public class ContractTest : MonoBehaviour
    {
        bool ready = false;


        void ContractsWereLoaded()
        {
            ready = true;
        }

        IEnumerator Start()
        {
            
            while (Contracts.ContractSystem.Instance == null)
                yield return 0;

            GameEvents.Contract.onContractsLoaded.Add(ContractsWereLoaded);

            while (!ready) yield return 0;

            
            var cs = Contracts.ContractSystem.Instance;

            //var contracts = cs.GetCurrentContracts<Contracts.Templates.RescueKerbal>();
            //var contracts = cs.GetCurrentContracts<Contracts.Contract>().ToList().Where(ct => ct is Contracts.Templates.RescueKerbal).ToList(); // nope
            var contracts = cs.Contracts.Where(ct => ct is Contracts.Templates.RescueKerbal).ToList();

            Log.Write("Listing active rescue contracts:");

            foreach (var c in contracts)
            {
                Log.Write("Active contract: {0}", c.Description);
            }

            Contracts.ContractSystem.ParameterTypes.ForEach(pt => Log.Write("ParameterType: {0}", pt.FullName));
            Contracts.ContractSystem.PredicateTypes.ForEach(pt => Log.Write("PredicateType: {0}", pt.FullName));

            FlightGlobals.Vessels.ForEach(v =>
            {
                if (v.isEVA)
                {
                    Log.Write("EVA: {0}", v.vesselName);
                    Log.Write("was controllable: {0}", v.protoVessel.wasControllable);

                    v.protoVessel.GetVesselCrew().ForEach(pcm =>
                    {
                        Log.Write(" crew: {0}", pcm.name);
                        Log.Write(" type: {0}", pcm.type.ToString());
                    });
                    //v.gameObject.PrintComponents();
                }
            });

            cs.Contracts.ForEach(c => Log.Write("Contract: {0}", c.GetType()));
        }

        void OnDestroy()
        {
            GameEvents.Contract.onContractsLoaded.Remove(ContractsWereLoaded);
        }
    }

    //[KSPAddon(KSPAddon.Startup.Flight, false)]
    public class FontTest : MonoBehaviour
    {
        string text = string.Empty;
        Font myFont;
        GUIStyle myStyle;

        void Start()
        {
            Log.Write("FontTest running");

            myStyle = new GUIStyle(HighLogic.Skin.textArea);

            myFont = FontUtil.LoadFont("DebugTools/test2.kfnt");
            if (myFont != null)
            {
                Log.Warning("Loaded KSPFONT: {0}", myFont.name);

                myStyle.font = myFont;
                myStyle.fontStyle = FontStyle.Normal;
                
            }
            else
            {
                Log.Error("Failed to create kspfont!");
            }
        }

        void OnGUI()
        {
            GUILayout.Window(32432, new Rect(200f, 200f, 400f, 400f), window, "Test Font");
        }

        void window(int winid)
        {
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            {
                text = GUILayout.TextArea(text, myStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            }
            GUILayout.EndVertical();
        }
    }


    //[KSPAddon(KSPAddon.Startup.Flight, false)]
    //public class HorizontalSpeedTest : MonoBehaviour
    //{
    //    void Update()
    //    {
    //        var vessel = FlightGlobals.ActiveVessel;
    //        if (vessel == null) return;

    //        var verticalSpeed = vessel.verticalSpeed;

    //        Log.Debug("srf_velocity = {0}", vessel.srf_velocity);
    //        Log.Debug("horizontalSrfSpeed = {0}", vessel.horizontalSrfSpeed);
    //        Log.Debug("orbit velocity = {0}", vessel.obt_velocity.ToString());
    //    }
    //}


    //[KSPAddon(KSPAddon.Startup.EditorAny, false)]
    //public class GuiTest : MonoBehaviour
    //{
    //    UIButton button;
    //    UIButton clone;

    //    void Start()
    //    {
    //        var exit = EditorLogic.fetch.exitBtn;

    //        Log.Write("editor btn position: {0}, local {1}", exit.transform.position, exit.transform.localPosition);
    //        Log.Write("dimensions: {0}, {1}", exit.width, exit.height);
    //        Log.Write("scale: {0}", exit.transform.localScale);
    //        Log.Write("animations: {0}", exit.animations.Length);
    //        Log.Write("uvs: {0}", exit.GetUVs().ToString());

    //        if (exit.gameObject.transform.parent != null)
    //        {
    //            Log.Error("exit has parent!");
    //            GameObject parent = exit.transform.parent.gameObject;

    //            while (parent.transform.parent != null)
    //                parent = parent.transform.parent.gameObject;

    //            //parent.PrintComponents();

    //        }
    //        exit.gameObject.PrintComponents();

    //        if (exit.Managed)
    //        {
    //            Log.Write("is managed");
    //        }

    //        //Log.Write("second btn position: {0}", EditorLogic.fetch.flagBrowserButton.transform.position);

    //        string str;

    //        button = UIButton.Create("TestButton", new Vector3(0f, 1000f, 90f));
    //        button.enabled = true;
    //        button.gameObject.layer = exit.gameObject.layer;
    //        Log.Write("exit layer: {0}", exit.gameObject.layer);
    //        Log.Write("cam layer: {0}", exit.renderCamera.cullingMask);
    //        Log.Write("cam position: {0}", exit.renderCamera.transform.position);
    //        if (exit.renderCamera.transform.parent != null) Log.Write("cam has a parent!");

    //        Transform p = exit.renderCamera.transform;
    //        while (p.parent != null)
    //            p = p.parent;

    //        Log.Write("----------- dumping rendercamera components ------------");
    //        p.gameObject.PrintComponents();

    //        Log.Write("----------- end rendercamera dump ------------");


    //        foreach (var c in Camera.allCameras)
    //            Log.Write("camera name: {0}, ortho = {1}", c.name, c.orthographic);


    //        button.Setup(128f, 128f, exit.GetPackedMaterial(out str));
    //        button.RenderCamera = exit.renderCamera;

    //        Log.Write("our components:");
    //        button.gameObject.PrintComponents();
    //        button.SetDrawLayer(exit.drawLayer);
    //        Log.Write("exit.drawLayer = {0}", exit.drawLayer);
    //        button.SetAnchor(SpriteRoot.ANCHOR_METHOD.MIDDLE_CENTER);
    //        button.SetPlane(exit.plane);

    //        switch (exit.plane)
    //        {
    //            case SpriteRoot.SPRITE_PLANE.XY:
    //                Log.Write("Plane = XY");
    //                break;
    //            case SpriteRoot.SPRITE_PLANE.XZ:
    //                Log.Write("Plane = XZ");
    //                break;
    //            case SpriteRoot.SPRITE_PLANE.YZ:
    //                Log.Write("Plane = YZ");
    //                break;
    //        }
            
    //        button.gameObject.SetActive(true);
    //        button.Hide(false);
    //        //button.InitUVs();

    //        button.AddValueChangedDelegate(delegate(IUIObject obj) { Log.Write("ezbutton click!"); });


    //        // above works!


    //        int normalState = 0;
    //        Rect firstTextureUV = exit.GetUVs();

    //        CSpriteFrame frame = new CSpriteFrame();
    //        frame.uvs = firstTextureUV;
    //        button.animations[normalState].SetAnim(new SPRITE_FRAME[] { frame.ToStruct() });

    //        frame.uvs = new Rect(0f, 0f, 0.25f, 0.25f);
    //        button.animations[1].SetAnim(new SPRITE_FRAME[] { frame.ToStruct() });

    //        var temp = exit.GetUVs();
    //        frame.uvs = new Rect(temp.x + temp.width, temp.y + temp.height, temp.width, temp.height);
    //        button.animations[2].SetAnim(new SPRITE_FRAME[] { frame.ToStruct() });

    //        var mat = new Material(Shader.Find("Sprite/Vertex Colored")) { mainTexture = button.renderer.sharedMaterial.mainTexture };
    //        button.renderer.sharedMaterial = mat;

    //        button.SetPixelToUV(button.renderer.sharedMaterial.mainTexture);
    //        button.SetCamera(EditorLogic.fetch.exitBtn.renderCamera);
    //        button.InitUVs();

    //        Log.Write("cam ortho? {0}", exit.renderCamera.orthographic);
    //        Log.Write("wxh: {0}x{1}", exit.renderCamera.GetScreenWidth(), exit.renderCamera.GetScreenHeight());
    //        Log.Write("exit shader: {0}", exit.gameObject.renderer.sharedMaterial.shader.name);

    //        // works ;\
    //        clone = ((GameObject)GameObject.Instantiate(exit.gameObject)).GetComponent<UIButton>();
    //        clone.transform.position = new Vector3(-200f, 800f, 90f);

    //        gameObject.AddComponent<CenterScreenViewer>();


    //        Log.Write("exit.WorldToScreen center = {0}", exit.renderCamera.WorldToScreenPoint(new Vector3(0f, 1000f, 0f)));
    //        Log.Write("exit.WorldToScreen back to world = {0}", exit.renderCamera.ScreenToWorldPoint(exit.renderCamera.WorldToScreenPoint(new Vector3(0f, 1000f, 0f))));
    //        TestButton();
    //    }




    //    void Update()
    //    {
    //        EditorLogic.fetch.exitBtn.transform.position = new Vector3(EditorLogic.fetch.exitBtn.transform.position.x - 10f * Time.deltaTime, EditorLogic.fetch.exitBtn.transform.position.y - 10f * Time.deltaTime, EditorLogic.fetch.exitBtn.transform.position.z);
    //        //Log.Write("editor btn position: {0}", EditorLogic.fetch.exitBtn.transform.position);

    //        if (Input.GetKeyDown(KeyCode.Keypad4))
    //        {
    //            button.transform.Translate(new Vector3(-10f, 0f, 0f));
    //            Log.Write("new button position: {0}", button.transform.position);

    //            if (ScreenSafeUI.fetch.centerAnchor == null) Log.Error("center anchor is null!");

    //            Log.Write("centerAnchor.center = {0}", ScreenSafeUI.fetch.leftAnchor.center.position);

    //        }

    //        if (Input.GetKeyDown(KeyCode.Keypad6))
    //        {
    //            button.transform.Translate(new Vector3(10f, 0f, 0f));
    //            Log.Write("new button position: {0}", button.transform.position);
    //        }
    //        if (Input.GetKeyDown(KeyCode.Keypad8))
    //        {
    //            button.transform.Translate(new Vector3(0, 10f, 0f));
    //            Log.Write("new button position: {0}", button.transform.position);
    //        }
    //        if (Input.GetKeyDown(KeyCode.Keypad2))
    //        {
    //            button.transform.Translate(new Vector3(0f, -10f, 0f));
    //            Log.Write("new button position: {0}", button.transform.position);
    //        }
    //    }

    //    void TestButton()
    //    {
    //        // not ezgui types
    //        //var buttons = GameObject.FindObjectsOfType<ScreenSafeUIButton>();

    //        //Log.Write("Found {0} buttons", buttons.Length);

    //        //foreach (var b in buttons)
    //        //    b.gameObject.PrintComponents();

    //        //Log.Warning("--------------------------------");

    //        // doesn't seem to exist in editor screen
    //        //var ui = GameObject.FindObjectOfType<ScreenSafeUI>();
    //        //if (ui == null)
    //        //{
    //        //    Log.Error("Failed to find ScreenSafeUI object!");
    //        //}
    //        //else
    //        //{
    //        //    if (ui.centerAnchor == null) Log.Error("ui.centeranchor = null!");
    //        //    if (ui.centerAnchor.center == null) Log.Error("ui.centeranchor.center = null!");

    //        //    Log.Write("center anchor position: {0}", ui.centerAnchor.center.position);
    //        //}


    //    }
    //}


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
#if DISABLE_ASTEROID_SPAWNER
    // disables asteroid spawner
    [KSPAddonImproved(KSPAddonImproved.Startup.Flight | KSPAddonImproved.Startup.SpaceCenter | KSPAddonImproved.Startup.TrackingStation, false)]
    public class Debug_DisableSpawner : MonoBehaviour
    {
        IEnumerator Start()
        {
            while (HighLogic.CurrentGame.scenarios.All(psm => psm.moduleRef == null))
                yield return 0;
            yield return 0;

            var sm = ScenarioRunner.fetch.GetComponent<ScenarioDiscoverableObjects>();
            if (sm == null) { Log.Error("Failed to find SDO!"); yield break; }

            Log.Warning("Disabling ScenarioDiscoverableObjects");
            sm.StopAllCoroutines();
        }
    }
#endif

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
                HighLogic.SaveFolder = "debughard";
                var game = GamePersistence.LoadGame("quicksave", HighLogic.SaveFolder, true, false);
                if (game != null && game.flightState != null && game.compatible)
                {
                    FlightDriver.StartAndFocusVessel(game, game.flightState.activeVesselIdx);
                }
                CheatOptions.InfiniteFuel = true;
            }
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