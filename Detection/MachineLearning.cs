using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Detection {
    static class MachineLearning {
        private static int timer = -5;
        public static int Timer {
            get { return timer; }
            set { timer = value; }
        }

        public static Point[] TransformData(Point[] pc) {
            int fillCount = 50 - pc.Length;
            Point[] newCloud = new Point[50];

            for (int i = 0; i < 50; i++) {
                if (i < pc.Length) {
                    newCloud[i] = pc[i];
                } else {
                    newCloud[i] = new Point(-1, -1);
                }
            }

            return newCloud;
        }

        public static float Predict2(Point[] pc) {

            //Load sample data
            var sampleData = new DetectionMachineLearning.ModelInput() {
                __1_2 = (float)pc[0].X,
                __1_3 = (float)pc[0].Y,
                __1_4 = (float)pc[1].X,
                __1_5 = (float)pc[1].Y,
                __1_6 = (float)pc[2].X,
                __1_7 = (float)pc[2].Y,
                __1_8 = (float)pc[3].X,
                __1_9 = (float)pc[3].Y,
                __1_10 = (float)pc[4].X,
                __1_11 = (float)pc[4].Y,
                __1_12 = (float)pc[5].X,
                __1_13 = (float)pc[5].Y,
                __1_14 = (float)pc[6].X,
                __1_15 = (float)pc[6].Y,
                __1_16 = (float)pc[7].X,
                __1_17 = (float)pc[7].Y,
                __1_18 = (float)pc[8].X,
                __1_19 = (float)pc[8].Y,
                __1_20 = (float)pc[9].X,
                __1_21 = (float)pc[9].Y,
                __1_22 = (float)pc[10].X,
                __1_23 = (float)pc[10].Y,
                __1_24 = (float)pc[11].X,
                __1_25 = (float)pc[11].Y,
                __1_26 = (float)pc[12].X,
                __1_27 = (float)pc[12].Y,
                __1_28 = (float)pc[13].X,
                __1_29 = (float)pc[13].Y,
                __1_30 = (float)pc[14].X,
                __1_31 = (float)pc[14].Y,
                __1_32 = (float)pc[15].X,
                __1_33 = (float)pc[15].Y,
                __1_34 = (float)pc[16].X,
                __1_35 = (float)pc[16].Y,
                __1_36 = (float)pc[17].X,
                __1_37 = (float)pc[17].Y,
                __1_38 = (float)pc[18].X,
                __1_39 = (float)pc[18].Y,
                __1_40 = (float)pc[19].X,
                __1_41 = (float)pc[19].Y,
                __1_42 = (float)pc[20].X,
                __1_43 = (float)pc[20].Y,
                __1_44 = (float)pc[21].X,
                __1_45 = (float)pc[21].Y,
                __1_46 = (float)pc[22].X,
                __1_47 = (float)pc[22].Y,
                __1_48 = (float)pc[23].X,
                __1_49 = (float)pc[23].Y,
                __1_50 = (float)pc[24].X,
                __1_51 = (float)pc[24].Y,
                __1_52 = (float)pc[25].X,
                __1_53 = (float)pc[25].Y,
                __1_54 = (float)pc[26].X,
                __1_55 = (float)pc[26].Y,
                __1_56 = (float)pc[27].X,
                __1_57 = (float)pc[27].Y,
                __1_58 = (float)pc[28].X,
                __1_59 = (float)pc[28].Y,
                __1_60 = (float)pc[29].X,
                __1_61 = (float)pc[29].Y,
                __1_62 = (float)pc[30].X,
                __1_63 = (float)pc[30].Y,
                __1_64 = (float)pc[31].X,
                __1_65 = (float)pc[31].Y,
                __1_66 = (float)pc[32].X,
                __1_67 = (float)pc[32].Y,
                __1_68 = (float)pc[33].X,
                __1_69 = (float)pc[33].Y,
                __1_70 = (float)pc[34].X,
                __1_71 = (float)pc[34].Y,
                __1_72 = (float)pc[35].X,
                __1_73 = (float)pc[35].Y,
                __1_74 = (float)pc[36].X,
                __1_75 = (float)pc[36].Y,
                __1_76 = (float)pc[37].X,
                __1_77 = (float)pc[37].Y,
                __1_78 = (float)pc[38].X,
                __1_79 = (float)pc[38].Y,
                __1_80 = (float)pc[39].X,
                __1_81 = (float)pc[39].Y,
                __1_82 = (float)pc[40].X,
                __1_83 = (float)pc[40].Y,
                __1_84 = (float)pc[41].X,
                __1_85 = (float)pc[41].Y,
                __1_86 = (float)pc[42].X,
                __1_87 = (float)pc[42].Y,
                __1_88 = (float)pc[43].X,
                __1_89 = (float)pc[43].Y,
                __1_90 = (float)pc[44].X,
                __1_91 = (float)pc[44].Y,
                __1_92 = (float)pc[45].X,
                __1_93 = (float)pc[45].Y,
                __1_94 = (float)pc[46].X,
                __1_95 = (float)pc[46].Y,
                __1_96 = (float)pc[47].X,
                __1_97 = (float)pc[47].Y,
                __1_98 = (float)pc[48].X,
                __1_99 = (float)pc[48].Y,
                __1_100 = (float)pc[49].X,
                __1_101 = (float)pc[49].Y,
            };

            //Load model and predict output
            var result = DetectionMachineLearning.Predict(sampleData);
            return result.Score;
        }

        public static float Predict(Point[] pc) {
            Stopwatch stopwatch = new Stopwatch();
            //Load sample data
            var sampleData = new MLModel.ModelInput() {
                Col1 = (float)pc[0].X,
                Col2 = (float)pc[0].Y,
                Col3 = (float)pc[1].X,
                Col4 = (float)pc[1].Y,
                Col5 = (float)pc[2].X,
                Col6 = (float)pc[2].Y,
                Col7 = (float)pc[3].X,
                Col8 = (float)pc[3].Y,
                Col9 = (float)pc[4].X,
                Col10 = (float)pc[4].Y,
                Col11 = (float)pc[5].X,
                Col12 = (float)pc[5].Y,
                Col13 = (float)pc[6].X,
                Col14 = (float)pc[6].Y,
                Col15 = (float)pc[7].X,
                Col16 = (float)pc[7].Y,
                Col17 = (float)pc[8].X,
                Col18 = (float)pc[8].Y,
                Col19 = (float)pc[9].X,
                Col20 = (float)pc[9].Y,
                Col21 = (float)pc[10].X,
                Col22 = (float)pc[10].Y,
            };

            //Load model and predict output
            stopwatch.Start();
            var result = MLModel.Predict(sampleData);
            stopwatch.Stop();
            TimeSpan stopwatchElapsed = stopwatch.Elapsed;
            timer = (int)stopwatchElapsed.TotalMilliseconds;

            float output = result.Score;
            return output;
        }
    }
}
