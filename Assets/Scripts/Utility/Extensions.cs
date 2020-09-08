using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public static class Extensions
    {
        public static void RotateOnZAxisByAccelerometer(this Transform t, bool inputInDeadzone, float limit)
        {
            float z = 0f;
            if (!inputInDeadzone)
            {
                z = Input.acceleration.x * 30f;
                z = Mathf.Clamp(z, -limit, limit);
            }
            else
            {
                z = 0f;
            }
            t.localEulerAngles = new Vector3(t.localEulerAngles.x, t.localEulerAngles.y, -z);
        }

        public static void SetYPosition(this Transform t, float y)
        {
            t.position = new Vector3(t.position.x, y, t.position.z);
        }

        public static void SetPosition(this Transform t, Vector3 position)
        {
            t.position = position;
        }

        public static string StripNonLatinLetters(this string s)
        {
            string newName = "";
            foreach (char c in s)
            {
                int code = (int)c;

                // Only english numbers, letters, symbols
                if ((code >= 32 && code <= 127))
                {
                    newName += c;
                }
            }
            return newName;
        }

        public static void SetText(this Text text, string str)
        {
            text.text = str;
        }

        public static void SetText(this InputField text, string str)
        {
            text.text = str;
        }

        public static Component GetInParentIfNotOnThisComponent(this Component c, string behaviour)
        {
            return c.GetComponent(behaviour) == null ?
                           c.GetComponentInParent(System.Type.GetType(behaviour)) :
                           c.GetComponent(behaviour);
        }
    }
}