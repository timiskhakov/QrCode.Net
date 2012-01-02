﻿using System.Linq;
using Gma.QrCodeNet.Encoding.Positioning;

namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
    internal static class MatrixScoreCalculator
    {
        internal static BitMatrix GetLowestPenaltyMatrix(this TriStateMatrix matrix, ErrorCorrectionLevel errorlevel)
        {
            PatternFactory patternFactory = new PatternFactory();
            int score = int.MaxValue;
            int tempScore;
            TriStateMatrix result = new TriStateMatrix(matrix.Width);
            TriStateMatrix triMatrix = new TriStateMatrix(matrix.Width);
            foreach(Pattern pattern in patternFactory.AllPatterns())
            {
            	triMatrix = matrix.Apply(pattern, errorlevel);
            	tempScore = triMatrix.PenaltyScore();
            	if(tempScore < score)
            	{
            		score = tempScore;
            		result = triMatrix;
            	}
            }
            
            return result;
//            return 
//                patternFactory
//                    .AllPatterns()
//                    .Select(pattern => matrix.Apply(pattern, errorlevel))
//            		.OrderByDescending(patternedMatrix => patternedMatrix.PenaltyScore())
//                    .Last();
        }


        internal static int PenaltyScore(this BitMatrix matrix)
        {
            PenaltyFactory penaltyFactory = new PenaltyFactory();
            return
            	penaltyFactory
            	.AllRules()
            	.Sum(penalty => penalty.PenaltyCalculate(matrix));
        }
    }
}
