using UnityEngine;
using System.Reflection;
using System.Text;

namespace com.PlugStudio
{
    public class Utility
    {
        /// <summary>
        /// Get Texture2D from System.IO.File
        /// </summary>
        /// <param name="_path">Application.streamingAssetsPath/_path</param>
        /// <returns></returns>
        public static Texture2D LoadTexture2DFile(string _path)
        {
            string path = System.IO.Path.Combine(Application.streamingAssetsPath, _path);

            byte[] textureBytes = System.IO.File.ReadAllBytes(path);
            var texture = new Texture2D(0, 0);

            if (textureBytes.Length > 0)
            {
                texture.LoadImage(textureBytes);
            }

            return texture;
        }

        /// <summary>
        /// Get Texture2D from Resources.Load
        /// </summary>
        /// <param name="_path"></param>
        /// <returns></returns>
        public static Texture2D LoadTexture2DResources(string _path)
        {
            var texture = new Texture2D(0, 0);

            texture = Resources.Load<Texture2D>(_path);

            return texture;
        }

        public static int CheckProbility(float[] _probilities)
        {
            return CheckProbility(_probilities, 100);
        }

        public static int CheckProbility(float[] _probilities, int _priction)
        {
            float r = Random.Range(0, 1.0f) * _priction;
            float cumulative = 0f;

            int lastIndex = _probilities.Length - 1;

            for (int i = 0; i < _probilities.Length; i++)
            {
                cumulative += _probilities[i];

                if(r <= cumulative)
                {
                    return lastIndex - i;
                }
            }

            return lastIndex;
        }

        public static StringBuilder StringBuilder
        {
            get
            {
                return new StringBuilder();
            }
        }

        public static void ClearConsole()
        {
#if UNITY_EDITOR
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");

            method.Invoke(new object(), null);
#elif UNITY_ANDROID || UNITY_IOS
            // Do Nothing;
#endif
        }
    }
}
