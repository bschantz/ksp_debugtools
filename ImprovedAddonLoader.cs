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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ImprovedAddonLoader
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class KSPAddonImproved : Attribute
    {
        [Flags]
        public enum Startup
        {
            // KSPAddon.Startup values:
            /*  Instantly = -2,
                EveryScene,
                EditorAny = -3,
                MainMenu = 2,
                Settings,
                SpaceCentre = 5,
                Credits = 4,
                EditorVAB = 6,
                EditorSPH = 9,
                Flight = 7,
                TrackingStation,
                PSystemSpawn = 10
            */

            None = 0,
            MainMenu = 1 << 0,
            Settings = 1 << 1,
            SpaceCenter = 1 << 2,
            Credits = 1 << 3,
            EditorVAB = 1 << 4,
            EditorSPH = 1 << 5,
            Flight = 1 << 6,
            TrackingStation = 1 << 7,
            PSystemSpawn = 1 << 8,
            Instantly = 1 << 9,

            EditorAny = EditorVAB | EditorSPH,
            TimeElapses = Flight | TrackingStation | SpaceCenter,
            RealTime = TimeElapses,
            EveryScene = ~0
        }


        public bool runOnce;
        public Startup scenes;

        public KSPAddonImproved(Startup mask, bool once = false)
        {
            runOnce = once;
            scenes = mask;
        }
    }

    /// <summary>
    /// KSPAddon with equality checking using an additional type parameter. Fixes the issue where AddonLoader prevents multiple start-once addons with the same start scene.
    /// By Majiir
    /// </summary>
    public class KSPAddonFixed : KSPAddon, IEquatable<KSPAddonFixed>
    {
        private readonly Type type;

        public KSPAddonFixed(KSPAddon.Startup startup, bool once, Type type)
            : base(startup, once)
        {
            this.type = type;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) { return false; }
            return Equals((KSPAddonFixed)obj);
        }

        public bool Equals(KSPAddonFixed other)
        {
            if (this.once != other.once) { return false; }
            if (this.startup != other.startup) { return false; }
            if (this.type != other.type) { return false; }
            return true;
        }

        public override int GetHashCode()
        {
            return this.startup.GetHashCode() ^ this.once.GetHashCode() ^ this.type.GetHashCode();
        }
    }

    // note: this needs to be KSPAddonFixed; don't change it
    [KSPAddonFixed(KSPAddon.Startup.Instantly, true, typeof(CustomAddonLoader))]
    internal class CustomAddonLoader : MonoBehaviour
    {
        // What's improved? The KSPAddon.Startup is now a bitmask so you can
        // use logical operations to specify which scenes you want your addon
        // to be loaded in


        // master list to keep track of addons in our assembly
        List<AddonInfo> addons = new List<AddonInfo>();
        private string _identifier;

        // Mainly required so we can flag addons when they've
        // been created in the case of runOnce = true
        class AddonInfo
        {
            public readonly Type type;
            public readonly KSPAddonImproved addon;
            public bool created;

            internal AddonInfo(Type t, KSPAddonImproved add)
            {
                type = t;
                created = false;

                addon = add;
            }

            internal bool RunOnce
            {
                get
                {
                    return addon.runOnce;
                }
            }

            internal KSPAddonImproved.Startup Scenes
            {
                get
                {
                    return addon.scenes;
                }
            }
        }



        void Awake()
        {
            DontDestroyOnLoad(this);

            // multiple plugins using this source will create their own instances
            // of the loader; the log can get confusing pretty fast without some
            // way of telling them apart
            _identifier = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "." + GetType().ToString();

            // examine our assembly for loaded types
            foreach (var ourType in System.Reflection.Assembly.GetExecutingAssembly().GetTypes())
            {
                var attr = ((KSPAddonImproved[])ourType.GetCustomAttributes(typeof(KSPAddonImproved), true)).SingleOrDefault();
                if (attr != null)
                {
                    Debug.Log(string.Format("Found KSPAddonImproved in {0}", ourType.FullName));
                    addons.Add(new AddonInfo(ourType, attr));
                }
            }

            // special case here: since we're already in the first scene,
            // OnLevelWasLoaded won't be invoked so we need to fire off any
            // "instant" loading addons now
            OnLevelWasLoaded((int)GameScenes.LOADING);
        }



        void OnLevelWasLoaded(int level)
        {
            GameScenes scene = (GameScenes)level;
            KSPAddonImproved.Startup mask = 0;

            if (scene == GameScenes.LOADINGBUFFER)
                return;

            Debug.Log(string.Format("{1}: {0} was loaded; instantiating addons...", scene.ToString(), _identifier));

            // Convert GameScenes => SceneMask
            switch (scene)
            {
                case GameScenes.EDITOR:
                    mask = KSPAddonImproved.Startup.EditorVAB;
                    break;

                case GameScenes.SPH:
                    mask = KSPAddonImproved.Startup.EditorSPH;
                    break;

                case GameScenes.CREDITS:
                    mask = KSPAddonImproved.Startup.Credits;
                    break;

                case GameScenes.FLIGHT:
                    mask = KSPAddonImproved.Startup.Flight;
                    break;

                case GameScenes.LOADING:
                    mask = KSPAddonImproved.Startup.Instantly;
                    break;

                case GameScenes.MAINMENU:
                    mask = KSPAddonImproved.Startup.MainMenu;
                    break;

                case GameScenes.SETTINGS:
                    mask = KSPAddonImproved.Startup.Settings;
                    break;

                case GameScenes.SPACECENTER:
                    mask = KSPAddonImproved.Startup.SpaceCenter;
                    break;

                case GameScenes.TRACKSTATION:
                    mask = KSPAddonImproved.Startup.TrackingStation;
                    break;

                case GameScenes.PSYSTEM:
                    mask = KSPAddonImproved.Startup.PSystemSpawn;
                    break;

                case GameScenes.LOADINGBUFFER:
                    // intentionally left unset
                    break;

                default:
                    Debug.LogError(string.Format("{1} unrecognized scene: {0}", scene.ToString(), _identifier));
                    break;
            }

            int counter = 0;

            for (int i = 0; i < addons.Count; ++i)
            {
                var addon = addons[i];

                if (addon.created && addon.RunOnce)
                    continue; // this addon was already loaded

                // should this addon be initialized in current scene?
                if ((addon.Scenes & mask) != 0)
                {
                    Debug.Log(string.Format("ImprovedAddonLoader: Creating addon '{0}'", addon.type.Name));
                    GameObject go = new GameObject(addon.type.Name);
                    go.AddComponent(addon.type);

                    addon.created = true;
                    ++counter;
                }
            }

            if (counter > 0)
            {
                Debug.Log(string.Format("{1} finished; created {0} addons", counter, _identifier));
            }
            else Debug.Log(string.Format("{0} finished; no addons created.", _identifier));
        }
    }
}
