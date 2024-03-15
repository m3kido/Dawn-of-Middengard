using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MatrixData))]
public class MatrixDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MatrixData matrixData = (MatrixData)target;

        EditorGUI.BeginChangeCheck();

        matrixData.rows = EditorGUILayout.IntField("Rows", matrixData.rows);
        matrixData.columns = EditorGUILayout.IntField("Columns", matrixData.columns);

        if (GUILayout.Button("Initialize Matrix"))
        {
            matrixData.InitializeMatrix();
        }

        if (matrixData.matrix != null)
        {
            EditorGUILayout.LabelField("Matrix:");

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

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(matrixData);
        }
    }
}

