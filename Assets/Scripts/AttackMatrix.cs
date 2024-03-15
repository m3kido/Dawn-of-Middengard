using System;
using System.IO;
using UnityEngine;

public class AttackMatrix  
{
    const int rows = 2;
    const int cols = 2;

    public static int[,] matrix = new int[rows,cols];

    public string file = "please_work";

    public void Main()
    {
        for (int i = 0; i < rows; i++)
        {
            for(int j = 0; j < cols; j++)
            {
                Console.WriteLine($"{(UnitType)i} attacks {(UnitType)j}");
                matrix[i,j] = Convert.ToInt32(Console.ReadLine());
            }
        }

        string s = JsonUtility.ToJson(matrix);
        File.WriteAllText(file, s);
    }
    
}
