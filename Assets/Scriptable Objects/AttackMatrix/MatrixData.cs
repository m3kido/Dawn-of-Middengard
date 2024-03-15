using UnityEngine;

[CreateAssetMenu(fileName = "MatrixData", menuName = "Custom/MatrixData", order = 1)]
public class MatrixData : ScriptableObject
{
    public int rows;
    public int columns;
    public int[,] matrix;

    private static MatrixData instance;

    public static MatrixData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<MatrixData>("MatrixData");
            }
            return instance;
        }
    }

    public void InitializeMatrix()
    {
        matrix = new int[rows, columns];
    }
}
