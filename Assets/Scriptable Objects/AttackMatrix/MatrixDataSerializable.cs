using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatrixDataSerializable
{
    public int rows;
    public int columns;
    public int[,] matrix;

    public MatrixDataSerializable(MatrixData matrixData)
    {
        rows = matrixData.rows;
        columns = matrixData.columns;
        matrix = matrixData.matrix;
    }
}
