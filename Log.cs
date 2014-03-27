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
#define VERBOSE
using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace DebugTools
{
    public class Log
    {

        #region Assembly/Class Information
        /// <summary>
        /// Name of the Assembly that is running this MonoBehaviour
        /// </summary>
        internal static String _AssemblyName
        { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }

        /// <summary>
        /// Name of the Class - including Derivations
        /// </summary>
        internal String _ClassName
        { get { return this.GetType().Name; } }
        #endregion


        private static String FormatMessage(String msg)
        {
            return String.Format("{1}, {0}", msg, _AssemblyName);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        internal static void Debug(String Message, params object[] strParams)
        {
            Write(Message, strParams);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        internal static void Debug(String message)
        {
            Write(message);
        }


        internal static void Verbose(String Message, params object[] strParams)
        {
            Verbose(string.Format(Message, strParams));
        }

        internal static void Verbose(String message)
        {
#if VERBOSE
                Write("(info): " + message);
#endif
        }

        internal static void Write(String Message, params object[] strParams)
        {
            Write(String.Format(Message, strParams));
        }

        internal static void Write(String Message)
        {
            UnityEngine.Debug.Log(FormatMessage(Message));
        }

        internal static void Warning(String message, params object[] strParams)
        {
            Warning(String.Format(message, strParams));
        }

        internal static void Warning(String message)
        {
            UnityEngine.Debug.LogWarning(FormatMessage(message));
        }

        internal static void Error(String message, params object[] strParams)
        {
            Error(String.Format(message, strParams));
        }

        internal static void Error(String message)
        {
            UnityEngine.Debug.LogError(FormatMessage(message));
        }

    }
}
