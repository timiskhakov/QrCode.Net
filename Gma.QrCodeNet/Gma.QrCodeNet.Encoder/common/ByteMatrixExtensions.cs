﻿using Gma.QrCodeNet.Encoding.Positioning;

namespace Gma.QrCodeNet.Encoding.Common
{
    internal static class ByteMatrixExtensions
    {
        internal static TriStateMatrix ToBitMatrix(this Common.ByteMatrix byteMatrix) 
        {
            TriStateMatrix matrix = new TriStateMatrix(byteMatrix.Width);
            for (int i = 0; i < byteMatrix.Width; i++)
            {
                for (int j = 0; j < byteMatrix.Height; j++)
                {
                    if (byteMatrix[j, i] != -1)
                    {
                        matrix[i, j] = byteMatrix[j, i] != 0;
                    }
                }
            }
            return matrix;
        }
    }
}