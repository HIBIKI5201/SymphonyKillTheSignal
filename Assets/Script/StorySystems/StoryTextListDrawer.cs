#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StoryTextList))]
public class StoryTextListDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty characterTypeProp = property.FindPropertyRelative("characterType");
        SerializedProperty kindProp = property.FindPropertyRelative("kind");
        SerializedProperty textProp = property.FindPropertyRelative("text");

        position.height = EditorGUIUtility.singleLineHeight;

        StorySystem storySystem = (StorySystem)property.serializedObject.targetObject;

        if (storySystem != null && storySystem.characterList != null)
        {
            string[] options = storySystem.characterList.Select(c => c.characterName).ToArray();
            characterTypeProp.intValue = EditorGUI.Popup(position, "Character Type", characterTypeProp.intValue, options);
        }

        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        EditorGUI.PropertyField(position, kindProp, new GUIContent("Kind"));

        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        EditorGUI.PropertyField(position, textProp, new GUIContent("Text"));

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing;
    }
}
#endif
