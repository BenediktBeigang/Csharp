// This file was auto-generated by ML.NET Model Builder. 
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Detection
{
    public partial class MLModel
    {
        /// <summary>
        /// model input class for MLModel.
        /// </summary>
        #region model input class
        public class ModelInput
        {
            [ColumnName(@"col0")]
            public float Col0 { get; set; }

            [ColumnName(@"col1")]
            public float Col1 { get; set; }

            [ColumnName(@"col2")]
            public float Col2 { get; set; }

            [ColumnName(@"col3")]
            public float Col3 { get; set; }

            [ColumnName(@"col4")]
            public float Col4 { get; set; }

            [ColumnName(@"col5")]
            public float Col5 { get; set; }

            [ColumnName(@"col6")]
            public float Col6 { get; set; }

            [ColumnName(@"col7")]
            public float Col7 { get; set; }

            [ColumnName(@"col8")]
            public float Col8 { get; set; }

            [ColumnName(@"col9")]
            public float Col9 { get; set; }

            [ColumnName(@"col10")]
            public float Col10 { get; set; }

            [ColumnName(@"col11")]
            public float Col11 { get; set; }

            [ColumnName(@"col12")]
            public float Col12 { get; set; }

            [ColumnName(@"col13")]
            public float Col13 { get; set; }

            [ColumnName(@"col14")]
            public float Col14 { get; set; }

            [ColumnName(@"col15")]
            public float Col15 { get; set; }

            [ColumnName(@"col16")]
            public float Col16 { get; set; }

            [ColumnName(@"col17")]
            public float Col17 { get; set; }

            [ColumnName(@"col18")]
            public float Col18 { get; set; }

            [ColumnName(@"col19")]
            public float Col19 { get; set; }

            [ColumnName(@"col20")]
            public float Col20 { get; set; }

            [ColumnName(@"col21")]
            public float Col21 { get; set; }

            [ColumnName(@"col22")]
            public float Col22 { get; set; }

        }

        #endregion

        /// <summary>
        /// model output class for MLModel.
        /// </summary>
        #region model output class
        public class ModelOutput
        {
            public float Score { get; set; }
        }
        #endregion

        private static string MLNetModelPath = Path.GetFullPath("MLModel.zip");

        /// <summary>
        /// Use this method to predict on <see cref="ModelInput"/>.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static ModelOutput Predict(ModelInput input)
        {
            MLContext mlContext = new MLContext();

            // Load model & create prediction engine
            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var modelInputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            ModelOutput result = predEngine.Predict(input);
            return result;
        }
    }
}
