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

        // Draw CharacterType Popup
        position.height = EditorGUIUtility.singleLineHeight;
        StoryTextDataBase storySystem = (StoryTextDataBase)property.serializedObject.targetObject;

        if (storySystem != null && storySystem._characterList != null)
        {
            string[] options = storySystem._characterList.Select(c => c.characterName).ToArray();
            characterTypeProp.intValue = EditorGUI.Popup(position, "Character Type", characterTypeProp.intValue, options);
        }

        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        // Draw Kind Property
        EditorGUI.PropertyField(position, kindProp, new GUIContent("Kind"));

        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        // Draw Text Property as TextArea
        // Adjust height for TextArea
        var textAreaHeight = Mathf.Max(30, EditorGUI.GetPropertyHeight(textProp));
        Rect textRect = new(position.x, position.y, position.width, textAreaHeight);
        EditorGUI.PropertyField(textRect, textProp, new GUIContent("Text"), true); // Set 'true' to make it a TextArea

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing + 75; // Adjusted height to fit TextArea
    }
}
#endif
