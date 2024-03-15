using System.IO;
using UnityEditor;
using UnityEngine;

public class MatrixDataEditorWindow : EditorWindow
{
    private MatrixData matrixData;

    [MenuItem("Window/Matrix Data Editor")]
    public static void ShowWindow()
    {
        GetWindow<MatrixDataEditorWindow>("Matrix Data");
    }

    private void OnEnable()
    {
        matrixData = MatrixData.Instance;
    }

    private void OnGUI()
    {
        GUILayout.Label("Matrix Data Editor", EditorStyles.boldLabel);

        if (matrixData == null)
        {
            GUILayout.Label("No Matrix Data found. Create or assign Matrix Data asset.");
            return;
        }

        // Display matrix dimensions
        EditorGUILayout.LabelField("Rows: " + matrixData.rows.ToString());
        EditorGUILayout.LabelField("Columns: " + matrixData.columns.ToString());

        // Display matrix values
        EditorGUILayout.LabelField("Matrix:");
        if (matrixData.matrix != null)
        {
            for (int i = 0; i < matrixData.rows; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < matrixData.columns; j++)
                {
                    matrixData.matrix[i, j] = EditorGUILayout.IntField(matrixData.matrix[i, j]);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        // Button to save matrix data to JSON file
        if (GUILayout.Button("Save Matrix Data"))
        {
            SaveMatrixDataToJson();
        }
    }

    private void SaveMatrixDataToJson()
    {
        if (matrixData == null || matrixData.matrix == null)
        {
            Debug.LogWarning("Matrix Data is not initialized.");
            return;
        }

        string filePath = EditorUtility.SaveFilePanel("Save Matrix Data", "", "MatrixData", "json");
        if (!string.IsNullOrEmpty(filePath))
        {
            MatrixDataSerializable serializableData = new(matrixData);
            string jsonData = JsonUtility.ToJson(serializableData);
            File.WriteAllText(filePath, jsonData);
            Debug.Log("Matrix Data saved to: " + filePath);
        }
    }
}
