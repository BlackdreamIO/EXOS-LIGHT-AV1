using UnityEditor;
using UnityEngine;

/*

[CustomEditor(typeof(PostEngine))]
public class EditorPostEngineLabel : Editor
{
    SerializedProperty scriptProperty;

    private void OnEnable()
    {
        scriptProperty = serializedObject.FindProperty("m_Script");
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
        
        headerStyle.normal.textColor = Color.cyan;
        headerStyle.alignment = TextAnchor.MiddleCenter;
        headerStyle.fontSize = 20;


        Texture2D backgroundTexture = new Texture2D(1, 1);
        backgroundTexture.SetPixel(0, 0, Color.black);
        backgroundTexture.Apply();
        headerStyle.normal.background = backgroundTexture;
        EditorGUILayout.LabelField("POST ENGINE", headerStyle);

        EditorGUILayout.Space();

        // Display the rest of the variables and functionality
        DrawDefaultInspectorExcept("m_Script");
    }

    private void DrawDefaultInspectorExcept(string propertyName)
    {
        var serializedProperty = serializedObject.GetIterator();
        bool enterChildren = true;
        while (serializedProperty.NextVisible(enterChildren))
        {
            if (serializedProperty.name != propertyName)
            {
                EditorGUILayout.PropertyField(serializedProperty, true);
            }
            enterChildren = false;
        }
    }
}

*/