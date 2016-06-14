using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS_Clustering
{
    enum Initialization { NORMAL, HIERARCHICHAL,RHIE };
    enum Normalization { NONE, MIN_MAX };
    enum DataType { DISTANCE, MATRIX_2D, MATRIX_3D };
    enum LogType { MOVE, CLUSTER, ELEMENT };
}
