using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
using System.IO;
using UnityEngine;

namespace ReeperCommon
{
    /// <summary>
    /// General purpose audio utilities
    /// </summary>
    internal class AudioUtil
    {
        private static AudioUtil _instance = null;
        private Dictionary<string /* sound name */, PlayableSound> sounds = new Dictionary<string, PlayableSound>();
        private const float LOADING_TIMEOUT = 5f;
        private GameObject gameObject = new GameObject("ReeperCommon.AudioUtil", typeof(AudioSource));


        internal class PlayableSound
        {
            private AudioClip clip;
            private AudioSource source;

            private float lastPlayedTime;
            private float delay;

            internal PlayableSound(AudioClip aclip, AudioSource player)
            {
                clip = aclip;
                source = player;
                lastPlayedTime = delay = 0f;
            }


            public bool Play(float volume = 1f, float delay = 0f)
            {
                if (clip != null && Time.realtimeSinceStartup - lastPlayedTime > delay)
                {
                    if (!source.gameObject.activeSelf)
                        source.gameObject.SetActive(true);

                    try
                    {
                        source.PlayOneShot(clip, Mathf.Clamp(volume, 0f, 1f));
                        lastPlayedTime = Time.realtimeSinceStartup;
                        this.delay = delay;
                        return true;
                    } catch (Exception e)
                    {
                        Log.Error("AudioUtil.Play exception while playing '{0}': {1}", clip.name, e);
                        return false;
                    }
                }
                else return false;
            }
        }

//-----------------------------------------------------------------------------
// Begin implementation
//-----------------------------------------------------------------------------
        internal AudioUtil()
        {
            gameObject.transform.parent = Camera.main.transform;
        }

        public static AudioUtil Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("ReeperCommon.AudioUtil");
                    _instance = new AudioUtil();
                }

                return _instance;
            }
        }

        public static void EnsureLoaded()
        {
            var audio = Instance;
        }


        public PlayableSound this[string name]
        {
            get
            {
                if (sounds.ContainsKey(name))
                {
                    return sounds[name];
                } else 
                {
                    Log.Error("AudioUtil: Did not find sound '{0}'", name);
                    return null;
                }
            }
        }



        /// <summary>
        /// Locates all files with the specified extension in the specified
        /// directory relative to GameData (with empty string being inside
        /// GameData itself) and attempts to load them.
        /// <param name="relToGameData">Relative directory to load sounds from</param>
        /// <param name="type">Type of sound to load</param>
        /// </summary>
        /// <returns>Number of unique sounds loaded</returns>
        public static int LoadSoundsFrom(string relToGameData, AudioType type)
        {
            return Instance._LoadSoundsFrom(relToGameData, type);
        }



        private int _LoadSoundsFrom(string relToGameData, AudioType type)
        {
            if (relToGameData.StartsWith("/")) relToGameData = relToGameData.Substring(1);
            string soundDir = KSPUtil.ApplicationRootPath + "GameData/" + relToGameData;

            Log.Debug("Locating audio {0} files in '{1}'", type.ToString(), soundDir);
            if (!Directory.Exists(soundDir))
            {
                Log.Error("Directory '{0}' does not exist!", soundDir);
                return 0;
            }

            if (type == AudioType.UNKNOWN)
            {
                Log.Error("AudioUtil: \"Unknown\" is not a valid AudioType");
                return 0;
            }

            string extension = string.Format(".{0}", type.ToString()).ToLower();


            string[] files = Directory.GetFiles(soundDir, "*" + extension);
            int loadCounter = 0;

            Log.Debug("Found {0} sound ({2}) files in directory {1}", files.Length, soundDir, extension);
            foreach (var f in files)
                Log.Debug("File: {0}", f);

            foreach (var file in files)
            {
                AudioClip newClip = LoadSound(file, type, false);

                if (newClip != null)
                {
                    //string name = file.Substring(file.LastIndexOf('/') + 1);
                    string name = Path.GetFileNameWithoutExtension(file);

                    if (name.EndsWith(extension))
                        name = name.Substring(0, name.Length - extension.Length);

                    Log.Debug("Loaded sound; adding as {0}", name);

                    try
                    {
                        sounds.Add(name, new PlayableSound(newClip, gameObject.audio));
                        ++loadCounter;
                    }
                    catch (Exception e)
                    {
                        Log.Error("AudioController: cannot add {0} due to {1}", name, e);
                    }
                }
                else
                {
                    Log.Error("AudioUtil: Failed to load '{0}'", file);
                }
            }
            
            return loadCounter;
        }


        /// <summary>
        /// Play a sound with given key, volume and delay
        /// </summary>
        /// <param name="name"></param>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static bool PlayVolume(string name, float volume = 1f, float delay = 0f)
        {
            var sound = Instance[name];

            if (sound == null) return false;

            return sound.Play(volume, delay);
        }



        /// <summary>
        /// Play a sound with given key and delay. Volume is determined
        /// by game's ui setting.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static bool Play(string name, float delay = 0f)
        {
            return PlayVolume(name, GameSettings.UI_VOLUME, delay);
        }



        /// <summary>
        /// The GameDatabase audio clips all seem to be intended for use with
        /// 3D. It causes a problem with our UI sounds because the player's
        /// viewpoint is moving. Even if we attach an audio source to the
        /// player camera, strange effects due to that movement (like much
        /// louder in one ear in certain orientations) seem to occur.
        /// 
        /// This allows us to load the sounds ourselves with the parameters
        /// we want.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private AudioClip LoadSound(string path, AudioType type = AudioType.WAV, bool relativeToGameData = true)
        {
            if (relativeToGameData)
            {
                if (path.StartsWith("/"))
                    path = path.Substring(1);

                path = KSPUtil.ApplicationRootPath + "GameData/" + path;
            }
            Log.Verbose("Loading sound {0}", path);

            // windows requires three slashes.  see:
            // http://docs.unity3d.com/Documentation/ScriptReference/WWW.html
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                if (!path.StartsWith("file:///"))
                    path = "file:///" + path;
            }
            else if (!path.StartsWith("file://")) path = "file://" + path;

            Log.Debug("sound path: {0}, escaped {1}", path, System.Uri.EscapeUriString(path));

            // WWW.EscapeURL doesn't seem to work all that great.  I couldn't get
            // AudioClips to come out of it correctly.  Non-escaped local urls
            // worked just fine but the docs say they should be escaped and this
            // works so I think it's the best solution currently
            //WWW clipData = new WWW(WWW.EscapeURL(path));
            WWW clipData = new WWW(System.Uri.EscapeUriString(path));

            float start = Time.realtimeSinceStartup;

            while (!clipData.isDone && Time.realtimeSinceStartup - start < LOADING_TIMEOUT)
            {
            }

            if (!clipData.isDone)
                Log.Error("Audio.LoadSounds() - timed out in {0} seconds", Time.realtimeSinceStartup - start);

            return clipData.GetAudioClip(false, false, type);
        }
    }
}
