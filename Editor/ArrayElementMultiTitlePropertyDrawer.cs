using UnityEngine;
using UnityEditor;
using System.Text;

namespace Insthync.UnityEditorUtils
{
    [CustomPropertyDrawer(typeof(ArrayElementMultiTitleAttribute))]
    public class ArrayElementMultiTitleDrawer : PropertyDrawer
    {
        protected ArrayElementMultiTitleAttribute Attribute
        {
            get { return (ArrayElementMultiTitleAttribute)attribute; }
        }

        private SerializedProperty _titleProperty;
        private bool _isNull;
        private Color _changingColor;
        private GUIStyle _style;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string newlabel = GetTitle(property);
            if (string.IsNullOrEmpty(newlabel))
            {
                // Element 0, Element 1, Element 2, ... Element N
                newlabel = label.text;
            }
            else
            {
                // 0: XXX
                if (!string.IsNullOrEmpty(label.text))
                    newlabel = $"{label.text.Split(' ')[1]}: {newlabel}";
                else
                    newlabel = $"      {newlabel}";
            }

            _changingColor = _isNull ? Attribute.nullColor : Attribute.notNullColor;
            _style = SetStyleColor(new GUIStyle(), _changingColor);
            EditorGUI.PropertyField(position, property, GUIContent.none, true);
            EditorGUI.LabelField(position, new GUIContent(newlabel, label.tooltip), _style);
        }

        private GUIStyle SetStyleColor(GUIStyle style, Color color)
        {
            style.normal.textColor =
                style.onNormal.textColor =
                style.active.textColor =
                style.onActive.textColor =
                style.focused.textColor =
                style.onFocused.textColor =
                style.hover.textColor =
                style.onHover.textColor = color;
            return style;
        }

        private string GetTitle(SerializedProperty property)
        {
            if (Attribute.variableNames == null || Attribute.variableNames.Length == 0)
                return string.Empty;

            _isNull = false;
            StringBuilder resultTitle = new StringBuilder();
            for (int i = 0; i < Attribute.variableNames.Length; ++i)
            {
                string variablePropertyPath = property.propertyPath + "." + Attribute.variableNames[i];
                _titleProperty = property.serializedObject.FindProperty(variablePropertyPath);
                if (_titleProperty == null)
                {
                    resultTitle.Append("Unknow");
                    if (i < Attribute.variableNames.Length - 1)
                        resultTitle.Append(Attribute.separator);
                    continue;
                }
                switch (_titleProperty.propertyType)
                {
                    case SerializedPropertyType.Generic:
                        break;
                    case SerializedPropertyType.Integer:
                        resultTitle.Append(_titleProperty.intValue.ToString());
                        break;
                    case SerializedPropertyType.Boolean:
                        resultTitle.Append(_titleProperty.boolValue.ToString());
                        break;
                    case SerializedPropertyType.Float:
                        resultTitle.Append(_titleProperty.floatValue.ToString());
                        break;
                    case SerializedPropertyType.String:
                        break;
                    case SerializedPropertyType.Color:
                        resultTitle.Append(_titleProperty.colorValue.ToString());
                        break;
                    case SerializedPropertyType.ObjectReference:
                        if (_titleProperty.objectReferenceValue != null)
                        {
                            resultTitle.Append(_titleProperty.objectReferenceValue.name);
                        }
                        else
                        {
                            _isNull = true;
                            resultTitle.Append("None");
                        }
                        break;
                    case SerializedPropertyType.LayerMask:
                        break;
                    case SerializedPropertyType.Enum:
                        resultTitle.Append(_titleProperty.enumNames[_titleProperty.enumValueIndex]);
                        break;
                    case SerializedPropertyType.Vector2:
                        resultTitle.Append(_titleProperty.intValue.ToString());
                        break;
                    case SerializedPropertyType.Vector3:
                        resultTitle.Append(_titleProperty.vector3Value.ToString());
                        break;
                    case SerializedPropertyType.Vector4:
                        resultTitle.Append(_titleProperty.vector4Value.ToString());
                        break;
                    case SerializedPropertyType.Rect:
                        break;
                    case SerializedPropertyType.ArraySize:
                        break;
                    case SerializedPropertyType.Character:
                        break;
                    case SerializedPropertyType.AnimationCurve:
                        break;
                    case SerializedPropertyType.Bounds:
                        break;
                    case SerializedPropertyType.Gradient:
                        break;
                    case SerializedPropertyType.Quaternion:
                        break;
                    default:
                        break;
                }
                if (i < Attribute.variableNames.Length - 1)
                    resultTitle.Append(Attribute.separator);
            }
            return resultTitle.ToString();
        }
    }
}
