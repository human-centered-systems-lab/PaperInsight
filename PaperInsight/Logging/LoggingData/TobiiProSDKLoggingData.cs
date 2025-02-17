using System;
using System.Collections.Generic;
using Tobii.Research;

namespace PaperInsight.Logging.LoggingData
{
    internal class TobiiProSDKLoggingData : AbstractLoggingData
    {
        internal DateTime SystemTime;
        internal long DeviceTime;

        //Pupil [LEFT/RIGHT]
        internal Validity[] PupilValidity = new Validity[2] { Validity.Invalid, Validity.Invalid };
        internal float[] PupilSize = new float[2] { float.NaN, float.NaN };

        //GazePoint [LEFT/RIGHT] [[X,Y,Z],[X,Y,Z]]
        internal Validity[] GazePointValidity = new Validity[2] { Validity.Invalid, Validity.Invalid };
        internal float[,] GazePointPositionOnDisplayArea = new float[2, 2] { { float.NaN, float.NaN }, { float.NaN, float.NaN } };
        internal float[,] GazePointPositionInUserCoordinates = new float[2, 3] { { float.NaN, float.NaN, float.NaN }, { float.NaN, float.NaN, float.NaN } };

        //GazeOrigin [LEFT/RIGHT] [[X,Y,Z],[X,Y,Z]]
        internal Validity[] GazeOriginValidity = new Validity[2] { Validity.Invalid, Validity.Invalid };
        internal float[,] GazeOriginPositionInTrackBoxCoordinates = new float[2, 3] { { float.NaN, float.NaN, float.NaN }, { float.NaN, float.NaN, float.NaN } };
        internal float[,] GazeOriginPositionInUserCoordinates = new float[2, 3] { { float.NaN, float.NaN, float.NaN }, { float.NaN, float.NaN, float.NaN } };

        internal TobiiProSDKLoggingData(DateTime devicetime, long time, EyeData left, EyeData right)
        {
            SystemTime = devicetime;
            DeviceTime = time;

            if (left == null || right == null) return;

            PupilValidity = new Validity[2] { left.Pupil.Validity, right.Pupil.Validity };
            PupilSize = new float[2] { left.Pupil.PupilDiameter, right.Pupil.PupilDiameter };

            GazePointValidity = new Validity[2] { left.GazePoint.Validity, right.GazePoint.Validity };

            var l_PointDisplay = left.GazePoint.PositionOnDisplayArea;
            var r_PointDisplay = right.GazePoint.PositionOnDisplayArea;
            GazePointPositionOnDisplayArea = new float[2, 2]
            {
            { l_PointDisplay.X, l_PointDisplay.Y },
            { r_PointDisplay.X, r_PointDisplay.Y }
            };

            var l_PointUser = left.GazePoint.PositionInUserCoordinates;
            var r_PointUser = right.GazePoint.PositionInUserCoordinates;
            GazePointPositionInUserCoordinates = new float[2, 3]
            {
            { l_PointUser.X, l_PointUser.Y, l_PointUser.Z },
            { r_PointUser.X, r_PointUser.Y, r_PointUser.Z }
            };

            GazeOriginValidity = new Validity[2]
            {
            left.GazeOrigin.Validity, right.GazeOrigin.Validity
            };

            var l_OriginUser = left.GazeOrigin.PositionInUserCoordinates;
            var r_OriginUser = right.GazeOrigin.PositionInUserCoordinates;
            GazeOriginPositionInUserCoordinates = new float[2, 3]
            {
            { l_OriginUser.X, l_OriginUser.Y, l_OriginUser.Z },
            { r_OriginUser.X, r_OriginUser.Y, r_OriginUser.Z }
            };

            var l_OriginTrackBox = left.GazeOrigin.PositionInTrackBoxCoordinates;
            var r_OriginTrackBox = right.GazeOrigin.PositionInTrackBoxCoordinates;
            GazeOriginPositionInTrackBoxCoordinates = new float[2, 3]
            {
            { l_OriginTrackBox.X, l_OriginTrackBox.Y, l_OriginTrackBox.Z },
            { r_OriginTrackBox.X, r_OriginTrackBox.Y, r_OriginTrackBox.Z }
            };
        }

        internal static string CSV_Header => new string[]
        {
            "SystemTime",
            "DeviceTime",

            "Pupil_Validity_Left", "PupilValidity_Right",
            "PupilSize_Left", "PupilSize_Right",

            "GazePoint_Validity_Left", "GazePoint_Validity_Right",
            "GazePoint_PositionOnDisplayArea_Left_X", "GazePoint_PositionOnDisplayArea_Left_Y",
            "GazePoint_PositionOnDisplayArea_Right_X", "GazePoint_PositionOnDisplayArea_Right_Y",
            "GazePoint_PositionInUserCoordinates_Left_X","GazePoint_PositionInUserCoordinates_Left_Y","GazePoint_PositionInUserCoordinates_Left_Z",
            "GazePoint_PositionInUserCoordinates_Right_X","GazePoint_PositionInUserCoordinates_Right_Y","GazePoint_PositionInUserCoordinates_Right_Z",

            "GazeOrigin_Validity_Left", "GazeOrigin_Validity_Right",
            "GazeOrigin_PositionInTrackBoxCoordinates_Left_X", "GazeOrigin_PositionInTrackBoxCoordinates_Left_Y","GazeOrigin_PositionInTrackBoxCoordinates_Left_Z",
            "GazeOrigin_PositionInTrackBoxCoordinates_Right_X","GazeOrigin_PositionInTrackBoxCoordinates_Right_Y","GazeOrigin_PositionInTrackBoxCoordinates_Right_Z",
            "GazeOrigin_PositionInUserCoordinates_Left_X","GazeOrigin_PositionInUserCoordinates_Left_Y", "GazeOrigin_PositionInUserCoordinates_Left_Z",
            "GazeOrigin_PositionInUserCoordinates_Right_X","GazeOrigin_PositionInUserCoordinates_Right_Y","GazeOrigin_PositionInUserCoordinates_Right_Z"
        }.ToCSVString();

        internal override string ToCSVString()
        {
            string[] content = new string[]
            {
                $"{SystemTime:dd/MM/yyyy HH:mm:ss}",
                DeviceTime.ToString(),

                PupilValidity.ToCSVString(),
                PupilSize.ToCSVString(),

                GazePointValidity.ToCSVString(),
                GazePointPositionOnDisplayArea.ToCSVString(),
                GazePointPositionInUserCoordinates.ToCSVString(),

                GazeOriginValidity.ToCSVString(),
                GazeOriginPositionInUserCoordinates.ToCSVString(),
                GazeOriginPositionInTrackBoxCoordinates.ToCSVString(),
            };
            return content.ToCSVString();
        }
    }   
}
