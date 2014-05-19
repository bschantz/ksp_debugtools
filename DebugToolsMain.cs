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
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using ImprovedAddonLoader;
using ConfigTools;

namespace DebugTools
{

    //[KSPAddonImproved(KSPAddonImproved.Startup.Flight, false)]
//    [KSPAddon(KSPAddon.Startup.Flight, false)]
//    public class CustomFontLoader : MonoBehaviour
//    {
//void OnGUI()
//{
//    GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
//        scrll = GUILayout.BeginScrollView(scrll);

//        GUILayout.BeginVertical(GUILayout.MinHeight(256f));
//            // this will leave a blank area for your text
//        GUILayout.EndVertical();

//        var textRect = GUILayoutUtility.GetLastRect();
                
//            // your text rendering here, kept inside textRect

//        GUILayout.EndScrollView();
//    GUILayout.EndVertical();
//}
//        static Font LoadFont(string xmlFilename, Texture2D fontTexture)
//        {
//            if (xmlFilename.StartsWith("/")) xmlFilename = xmlFilename.Substring(1);

//            XmlReaderSettings settings = new XmlReaderSettings();
//            settings.ConformanceLevel = ConformanceLevel.Fragment;
//            settings.IgnoreWhitespace = true;

//            Font font = new Font();

//            if (font == null)
//            {
//                Log.Error("you fuxed");
//                return null;
//            }
//            try
//            {
//                using (XmlReader reader = XmlReader.Create(KSPUtil.ApplicationRootPath + xmlFilename, settings))
//                {
//                    var doc = new XmlDocument();
//                    doc.Load(reader);

//                    var fontDescriptor = doc.SelectSingleNode("Font");
//                    var descAttr = fontDescriptor.Attributes;

//                    // family name
//                    font.name = descAttr["family"].InnerText;
                    
//                    // font size
//                    int fontSize = int.Parse(descAttr["size"].InnerText);
//                    int fontHeight = int.Parse(descAttr["height"].InnerText);

//                    Log.Write("family = {0}", fontDescriptor.Attributes["family"].InnerText);
//                    Log.Write("size = {0}", fontDescriptor.Attributes["size"].InnerText);
//                    Log.Write("height = {0}", fontDescriptor.Attributes["height"].InnerText);
//                    Log.Write("style = {0}", fontDescriptor.Attributes["style"].InnerText);


//                    var charInfo = new List<CharacterInfo>();

//                    foreach (XmlNode node in fontDescriptor.SelectNodes("Char"))
//                    {
//                        var charAttr = node.Attributes;

//                        Log.Write("  width = {0}", node.Attributes["width"].InnerText);
//                        Log.Write("  offset = {0}", node.Attributes["offset"].InnerText);
//                        Log.Write("  rect = {0}", node.Attributes["rect"].InnerText);
//                        Log.Write("  code = {0}", node.Attributes["code"].InnerText);

//                        //-----------------------
//                        // set up info for this char
//                        //-----------------------
//                        var ci = new CharacterInfo();

//                            // unicode value
//                            int unicode = 0;
                            
//                            if (!int.TryParse(charAttr["code"].InnerText, out unicode))
//                            {
//                                Log.Write("Failed to parse unicode value \"{0}\"; ignoring", charAttr["code"].InnerText);
//                                continue;
//                            }
//                            ci.index = unicode;

//                            // character width
//                            int width = 0;
//                            ci.width = int.TryParse(charAttr["width"].InnerText, out width) ? width : 0;

//                            ci.size = fontSize;

//                        ci.style = FontStyle.Normal;
                        

//                    }
//                }
//            } catch (Exception e)
//            {
//                Log.Error("Exception in LoadFont: {0}", e);
//                return null;
//            }

//            return font; 
//        }


//        static Font LoadFontSheet(string xmlFilename)
//        {
//            Log.Warning("LoadfontSheet");

//            //foreach (var t in GameDatabase.Instance.databaseTexture)
//            //    Log.Write("Texture: {0}", t.name);

//            while (xmlFilename.StartsWith("/"))
//            {
//                xmlFilename = xmlFilename.Substring(1);

//                if (xmlFilename.StartsWith("GameData"))
//                    xmlFilename = xmlFilename.Substring("GameData".Length + 1);
//            }
                

//            string fullFilename = KSPUtil.ApplicationRootPath + "GameData/" + xmlFilename;
//            string imgFilename = System.IO.Path.GetDirectoryName(xmlFilename) + "/" + System.IO.Path.GetFileNameWithoutExtension(xmlFilename);

            
//            Log.Write("full = {0}, img = {1}", fullFilename, imgFilename);

//            // we need both the image and the xml descriptor to exist
//                if (!System.IO.File.Exists(fullFilename))
//                {
//                    Log.Error("{0} does not exist!", fullFilename);
//                    return null;
//                }

//                var tex2D = GameDatabase.Instance.GetTexture(imgFilename, false);

//                // confirm instance of the image sheet
//                if (tex2D == null)
//                {
//                    Log.Error("Did not find image associated with {0}!", xmlFilename);
//                    return null;
//                }

//                Log.Write("creating xml reader");

//                XmlReaderSettings settings = new XmlReaderSettings();
//                settings.ConformanceLevel = ConformanceLevel.Fragment;
//                settings.IgnoreWhitespace = true;

//                using (XmlReader reader = XmlReader.Create(fullFilename, settings))
//                {
//                    var doc = new XmlDocument();
//                    doc.Load(reader);

//                    var fontDescriptor = doc.SelectSingleNode("Font");

//                    //Log.Write("descriptor = {0}", fontDescriptor.Value);
//                    //Log.Write("xml desc = {0}", fontDescriptor.OuterXml);

//                    Log.Write("family = {0}", fontDescriptor.Attributes["family"].InnerText);
//                    Log.Write("size = {0}", fontDescriptor.Attributes["size"].InnerText);
//                    Log.Write("height = {0}", fontDescriptor.Attributes["height"].InnerText);
//                    Log.Write("style = {0}", fontDescriptor.Attributes["style"].InnerText);


//                    foreach (XmlNode node in fontDescriptor.SelectNodes("Char"))
//                    {
//                        Log.Write("  width = {0}", node.Attributes["width"].InnerText);
//                        Log.Write("  offset = {0}", node.Attributes["offset"].InnerText);
//                        Log.Write("  rect = {0}", node.Attributes["rect"].InnerText);
//                        Log.Write("  code = {0}", node.Attributes["code"].InnerText);
//                    }
//                }

//                Log.Write("Finished xml read");
//            return new Font();
//        }

//        void Awake()
//        {
//            //var f = LoadFontSheet("GameData/DebugTools/liberation_mono_bold_14.xml");
//        }
//        void OnGUI()
//        {
//            if (GUI.Button(new Rect(150, 150, 64, 32), "Font"))
//            {
//                var f = LoadFontSheet("DebugTools/liberation_mono_bold_14.xml");
//            }
//        }
//    }


    //[KSPAddonImproved(KSPAddonImproved.Startup.EditorVAB, true)]
    //public class SingleTest : MonoBehaviour
    //{
    //    private List<T> FindPartModelComponents<T>(Part target) where T : Component
    //    {
    //        // find attached parts to target ..
    //        List<GameObject> ignoreList = new List<GameObject>();

    //        foreach (Part child in target.children)
    //            ignoreList.Add(child.gameObject);

    //        //List<T> potentials = target.FindModelComponents<T>().Distinct().ToList(); // nope, doesn't get engine fairings like it should
    //        List<T> potentials = target.GetComponentsInChildren<T>().Distinct().ToList();
    //        List<T> components = new List<T>();

    //        foreach (T potential in potentials)
    //            if (ignoreList.All(ignoreItem => !potential.transform.IsChildOf(ignoreItem.transform)))
    //                components.Add(potential);

    //        return components;
    //    }

    //    public void Start()
    //    {
    //        int totalTriangles = 0;

    //        foreach (var part in PartLoader.LoadedPartsList)
    //        {
    //            int partTris = 0;
    //            if (part.partPrefab == null)
    //                Log.Error("part {0} doesn't have a prefab (??!@)", part.name);

    //            Log.Write("getting mfs");
    //            //var mfs = FindPartModelComponents<MeshFilter>(part.partPrefab);
    //            //var mfs = part.partPrefab.FindModelComponents<MeshFilter>();
                
    //            foreach (var mf in mfs)
    //                partTris += mf.sharedMesh.triangles.Length;

    //            Log.Write("getting smfs");
    //            //var smfs = FindPartModelComponents<SkinnedMeshRenderer>(part.partPrefab);
    //            var smfs = part.partPrefab.FindModelComponents<SkinnedMeshRenderer>();

    //            foreach (var smf in smfs)
    //                partTris += smf.sharedMesh.triangles.Length;

    //            //var comps = part.partPrefab.GetComponents<Component>();

    //            //Log.Write("{0} components:", part.name);
    //            //foreach (var c in comps)
    //            //    Log.Write("  has {0}", c.name);


    //            //var alt = part.partPrefab.FindModelComponents<MeshFilter>();

    //            Log.Write("{0}: mfs {1}, smfs {2}, partTris {3}", part.name, mfs.Length, smfs.Length, partTris);


    //            totalTriangles += partTris;
    //        }

    //        Log.Write("Average triangles per part: {0}", totalTriangles / (float)(PartLoader.LoadedPartsList.Count));
    //    }
    //}

    //[KSPAddon(KSPAddon.Startup.Flight /*| KSPAddon.Startup.TrackingStation*/, false)]
//    [KSPAddonImproved(KSPAddonImproved.Startup.None, false)]
//    public class DebugToolsMain : MonoBehaviour
//    {
//        public void Start()
//        {
//            Log.Warning("Debug tools started");
//            Log.Warning("our gameobject: {0}", gameObject.name);
//            Log.Warning("has parent? {0}", gameObject.transform.parent == null ? "nope" : "yep");

//            var comps = gameObject.GetComponents<Component>();

//            foreach (var c in comps)
//                Log.Write("  has comp: {0} of type {1}", c.name, c.GetType().FullName);

//            //ImprovedAddonLoader.AddAddon(typeof(SingleTest), GameScenes.EDITOR);

//            //FlightGlobals.ActiveVessel.rootPart.gameObject.AddComponent<Orientation>();
//            //Log.Write("part count = {0}", FlightGlobals.ActiveVessel.parts.Count);

//            //foreach (var part in FlightGlobals.ActiveVessel.parts)
//            //{
//            //    var orientation = part.gameObject.AddComponent<Orientation>();

//            //    if (part != FlightGlobals.ActiveVessel.rootPart)
//            //    {
//            //        orientation.Length *= .25f;
//            //        orientation.MinLineWidth = 0.25f;
//            //        orientation.MaxLineWidth = 0f;
//            //        orientation.Alpha = 0.25f;
//            //    }


//            //}

//            //StartCoroutine(DelayedStart());

//            //var containers = FlightGlobals.ActiveVessel.FindPartModulesImplementing<ModuleScienceContainer>();
//            //var scienceSources = FlightGlobals.ActiveVessel.FindPartModulesImplementing<IScienceDataContainer>();
           
//            //// this also includes the containers we just got; don't need those
//            ////      note: logic here is: if an IScienceContainer is on the same
//            ////            part as a ModuleScienceContainer, that IScienceContainer
//            ////            will not be taken from
//            //scienceSources.RemoveAll(science => containers.Any(can => ((PartModule)science).part == ((PartModule)can).part));

//            //// move science from experiments into the first ModuleScienceContainer we found
//            //if (containers.Count > 0)
//            //{
//            //    containers[0].StoreData(scienceSources, true);
//            //}
//            //else Log.Error("Didn't find any containers to use for data storage");

//            //var containers = FlightGlobals.ActiveVessel.FindPartModulesImplementing<ModuleScienceContainer>();
//            //var scienceSources = FlightGlobals.ActiveVessel.FindPartModulesImplementing<IScienceDataContainer>();

////var containers = FlightGlobals.ActiveVessel.FindPartModulesImplementing<ModuleScienceContainer>();
////var sourceData = FlightGlobals.ActiveVessel.FindPartModulesImplementing<ModuleScienceExperiment>().Cast<IScienceDataContainer>().ToList();

////containers[0].StoreData(sourceData, true);
           

//        }


//        public void Update()
//        {
//            if (Input.GetKeyDown(KeyCode.P))
//                Start();
//        }

//        //public System.Collections.IEnumerator DelayedStart()
//        //{
//        //    while (!FlightGlobals.ready || HighLogic.CurrentGame.scenarios[0].moduleRef == null)
//        //        yield return 0;

//        //    ScenarioDiscoverableObjects discov = (ScenarioDiscoverableObjects)HighLogic.CurrentGame.scenarios.Find(scenario => scenario.moduleRef is ScenarioDiscoverableObjects).moduleRef;

//        //    if (discov == null)
//        //        Log.Error("Did not find ScenarioDiscoverableObjects!");

//        //    GameEvents.onVesselCreate.Add(CatchAsteroidSpawn);

//        //    for (int i = 0; i < 100; ++i)
//        //        discov.SpawnAsteroid();

//        //    GameEvents.onVesselCreate.Remove(CatchAsteroidSpawn);
//        //}



//        //public void CatchAsteroidSpawn(Vessel vessel)
//        //{
//        //    vessel.DiscoveryInfo.SetLastObservedTime(Planetarium.GetUniversalTime());

//        //    // track it by default, else its orbit won't be visible in mapview
//        //    vessel.DiscoveryInfo.SetLevel(DiscoveryLevels.StateVectors | DiscoveryLevels.Name | DiscoveryLevels.Presence);

//        //    Orbit newOrbit = new Orbit((UnityEngine.Random.Range(0f, 360f)), 0f, 1060053.49854083, 217.714701468054, 126.848000556171, 0.52911447506945, Planetarium.GetUniversalTime(), FlightGlobals.Bodies.Find(body => body.name == "Eve"));
//        //    newOrbit.UpdateFromUT(Planetarium.GetUniversalTime());

//        //    vessel.orbitDriver.orbit = newOrbit;
//        //}
        
        


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
    //public class Debug_AutoSwitchVab : MonoBehaviour
    //{
    //    public static bool first = true;

    //    public void Start()
    //    {
    //        if (first)
    //        {
    //            first = false;
    //            HighLogic.SaveFolder = "debug";
    //            var game = GamePersistence.LoadGame("quicksave", HighLogic.SaveFolder, true, false);

    //            if (game != null && game.compatible)
    //                HighLogic.LoadScene(GameScenes.EDITOR);
    //        }
    //    }
    //}
#endif
}
