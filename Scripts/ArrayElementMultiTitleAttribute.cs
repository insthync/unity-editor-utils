#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Insthync.UnityEditorUtils
{
    public class ArrayElementMultiTitleAttribute : PropertyAttribute
    {
        public string[] variableNames { get; private set; }
        public string separator { get; private set; }
#if UNITY_EDITOR
        public float[] nullColorValue { get; private set; }
        public float[] notNullColorValue { get; private set; }
        public float[] proNullColorValue { get; private set; }
        public float[] proNotNullColorValue { get; private set; }
#endif

        public Color nullColor
        {
            get
            {
#if UNITY_EDITOR
                return EditorGUIUtility.isProSkin ? GetColor(proNullColorValue) : GetColor(nullColorValue);
#else
                return Color.black;
#endif
            }
        }

        public Color notNullColor
        {
            get
            {
#if UNITY_EDITOR
                return EditorGUIUtility.isProSkin ? GetColor(proNotNullColorValue) : GetColor(notNullColorValue);
#else
                return Color.black;
#endif
            }
        }

        public ArrayElementMultiTitleAttribute(string[] variableNames) :
            this(variableNames, " | ", new float[] { 1, 0, 0, 1 }, new float[] { 0, 0, 1, 1 }, new float[] { 1, 0, 0, 1 }, new float[] { 0, 1, 0, 1 })
        {
        }

        public ArrayElementMultiTitleAttribute(string[] variableNames, string separator) :
            this(variableNames, separator, new float[] { 1, 0, 0, 1 }, new float[] { 0, 0, 1, 1 }, new float[] { 1, 0, 0, 1 }, new float[] { 0, 1, 0, 1 })
        {
        }

        public ArrayElementMultiTitleAttribute(string[] variableNames, float[] nullColorValue, float[] notNullColorValue) :
            this(variableNames, " | ", nullColorValue, notNullColorValue, nullColorValue, notNullColorValue)
        {
        }

        public ArrayElementMultiTitleAttribute(string[] variableNames, string separator, float[] nullColorValue, float[] notNullColorValue, float[] proNullColorValue, float[] proNotNullColorValue)
        {
            this.variableNames = variableNames;
            this.separator = separator;
#if UNITY_EDITOR
            this.nullColorValue = nullColorValue;
            this.notNullColorValue = notNullColorValue;
            this.proNullColorValue = proNullColorValue;
            this.proNotNullColorValue = proNotNullColorValue;
#endif
        }

        private Color GetColor(float[] arr)
        {
            if (arr.Length > 3)
                return new Color(arr[0], arr[1], arr[2], arr[3]);
            else if (arr.Length == 3)
                return new Color(arr[0], arr[1], arr[2]);
            return Color.black;
        }
    }
}
