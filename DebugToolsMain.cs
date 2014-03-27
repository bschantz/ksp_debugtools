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
using System.Linq;
using System.Text;
using UnityEngine;

namespace DebugTools
{
    //[KSPAddon(KSPAddon.Startup.EveryScene & ~(KSPAddon.Startup.Instantly | KSPAddon.Startup.MainMenu), false)]
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class DebugToolsMain : MonoBehaviour
    {
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Log.Debug("Dumping all GameObjects ...");

                var gos = GameObject.FindObjectsOfType(typeof(GameObject));
                foreach (var go in gos)
                    Log.Debug("GO: {0}, type {1}", go.name, go.GetType().Name);
            }

            //if (Input.GetKey(KeyCode.C))
            //{
            //    var editor = EditorLogic.fetch;

            //    foreach (var p in editor.ship.Parts)
            //        p.customPartData = "Reeper was here";
            //}

            // dump all texture urls
            if (Input.GetKeyDown(KeyCode.T))
            {
                Log.Debug("Dumping texture urls...");

                foreach (var t in GameDatabase.Instance.databaseTexture)
                    Log.Debug("Texture: {0}", t.name);
            }
        }
    }

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
                HighLogic.SaveFolder = "debugtest";
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
}
