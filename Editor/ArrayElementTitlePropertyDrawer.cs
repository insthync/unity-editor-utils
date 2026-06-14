using UnityEngine;
using UnityEditor;

namespace Insthync.UnityEditorUtils.Editor
{
    [CustomPropertyDrawer(typeof(ArrayElementTitleAttribute))]
    public class ArrayElementTitleDrawer : PropertyDrawer
    {
        protected ArrayElementTitleAttribute Attribute
        {
            get { return (ArrayElementTitleAttribute)attribute; }
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
            string variablePropertyPath = property.propertyPath + "." + Attribute.variableName;
            _titleProperty = property.serializedObject.FindProperty(variablePropertyPath);
            _isNull = false;
            if (_titleProperty == null)
                return string.Empty;
            switch (_titleProperty.propertyType)
            {
                case SerializedPropertyType.Generic:
                    break;
                case SerializedPropertyType.Integer:
                    return _titleProperty.intValue.ToString();
                case SerializedPropertyType.Boolean:
                    return _titleProperty.boolValue.ToString();
                case SerializedPropertyType.Float:
                    return _titleProperty.floatValue.ToString();
                case SerializedPropertyType.String:
                    break;
                case SerializedPropertyType.Color:
                    return _titleProperty.colorValue.ToString();
                case SerializedPropertyType.ObjectReference:
                    if (_titleProperty.objectReferenceValue != null)
                        return _titleProperty.objectReferenceValue.name;
                    _isNull = true;
                    return "None";
                case SerializedPropertyType.LayerMask:
                    break;
                case SerializedPropertyType.Enum:
                    return _titleProperty.enumNames[_titleProperty.enumValueIndex];
                case SerializedPropertyType.Vector2:
                    return _titleProperty.vector2Value.ToString();
                case SerializedPropertyType.Vector3:
                    return _titleProperty.vector3Value.ToString();
                case SerializedPropertyType.Vector4:
                    return _titleProperty.vector4Value.ToString();
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
            return string.Empty;
        }
    }
}
