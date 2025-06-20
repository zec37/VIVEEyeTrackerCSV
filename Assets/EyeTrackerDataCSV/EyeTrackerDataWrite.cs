using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables.Primitives;
using VIVE.OpenXR;
using VIVE.OpenXR.EyeTracker;
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine.Android;
#endif


public class DataLogger : MonoBehaviour
{
    //private List<DataEntry> dataEntries = new List<DataEntry>();
    private List<EyeDataEntry> dataEntries = new List<EyeDataEntry>();

    public Transform headTransform;

    //[System.Serializable]
    //private class DataEntry
    //{
    //    public string timestamp;
    //    public float posX, posY, posZ;
    //}

    [System.Serializable]
    private class EyeDataEntry
    {
        public string timestamp;
        
        public UInt32 isLeftGazeValid, isRightGazeValid;
        public float leftGazePosX, leftGazePosY, leftGazePosZ;
        public float leftGazeRotX, leftGazeRotY, leftGazeRotZ, leftGazeRotW;
        public float rightGazePosX, rightGazePosY, rightGazePosZ;
        public float rightGazeRotX, rightGazeRotY, rightGazeRotZ, rightGazeRotW;

        public UInt32 isLeftPupilDiameterValid, isRightPupilDiameterValid;
        public float leftPupilDiameter, rightPupilDiameter;
        public UInt32 isLeftPupilPositionValid, isRightPupilPositionValid;
        public float leftPupilPositionX, leftPupilPositionY;
        public float rightPupilPositionX, rightPupilPositionY;

        public UInt32 isleftGeometricValid, isRightGeometricValid;
        public float leftOpenness, rightOpenness;
        public float leftWide, rightWide;
        public float leftSqueeze, rightSqueeze;

        public float headPosX, headPosY, headPosZ;
        public float headRotX, headRotY, headRotZ, headRotW;
    }

    void Update()
    {
        //dataEntries.Add(new DataEntry()
        //{
        //    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
        //    posX = transform.position.x,
        //    posY = transform.position.y,
        //    posZ = transform.position.z
        //});
        
        XR_HTC_eye_tracker.Interop.GetEyeGazeData(out XrSingleEyeGazeDataHTC[] out_gazes);
        XrSingleEyeGazeDataHTC leftGaze = out_gazes[(int)XrEyePositionHTC.XR_EYE_POSITION_LEFT_HTC];
        XrSingleEyeGazeDataHTC rightGaze = out_gazes[(int)XrEyePositionHTC.XR_EYE_POSITION_RIGHT_HTC];

        XR_HTC_eye_tracker.Interop.GetEyePupilData(out XrSingleEyePupilDataHTC[] out_pupils);
        XrSingleEyePupilDataHTC leftPupil = out_pupils[(int)XrEyePositionHTC.XR_EYE_POSITION_LEFT_HTC];
        XrSingleEyePupilDataHTC rightPupil = out_pupils[(int)XrEyePositionHTC.XR_EYE_POSITION_RIGHT_HTC];

        XR_HTC_eye_tracker.Interop.GetEyeGeometricData(out XrSingleEyeGeometricDataHTC[] out_geometrics);
        XrSingleEyeGeometricDataHTC leftGeometric = out_geometrics[(int)XrEyePositionHTC.XR_EYE_POSITION_LEFT_HTC];
        XrSingleEyeGeometricDataHTC rightGeometric = out_geometrics[(int)XrEyePositionHTC.XR_EYE_POSITION_RIGHT_HTC];

        dataEntries.Add(new EyeDataEntry()
        {
            // Timestamp
            timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
            
            // Eye Gaze Data
            isLeftGazeValid = leftGaze.isValid,
            isRightGazeValid = rightGaze.isValid,

            leftGazePosX = leftGaze.gazePose.position.x,
            leftGazePosY = leftGaze.gazePose.position.y,
            leftGazePosZ = leftGaze.gazePose.position.z,

            leftGazeRotX = leftGaze.gazePose.orientation.x,
            leftGazeRotY = leftGaze.gazePose.orientation.y,
            leftGazeRotZ = leftGaze.gazePose.orientation.z,
            leftGazeRotW = leftGaze.gazePose.orientation.w,

            rightGazePosX = rightGaze.gazePose.position.x,
            rightGazePosY = rightGaze.gazePose.position.y,
            rightGazePosZ = rightGaze.gazePose.position.z,

            rightGazeRotX = rightGaze.gazePose.orientation.x,
            rightGazeRotY = rightGaze.gazePose.orientation.y,
            rightGazeRotZ = rightGaze.gazePose.orientation.z,
            rightGazeRotW = rightGaze.gazePose.orientation.w,

            // Eye Pupil Data
            isLeftPupilDiameterValid = leftPupil.isDiameterValid,
            isRightPupilDiameterValid = rightPupil.isDiameterValid,

            leftPupilDiameter = leftPupil.pupilDiameter,
            rightPupilDiameter = rightPupil.pupilDiameter,

            isLeftPupilPositionValid = leftPupil.isPositionValid,
            isRightPupilPositionValid = rightPupil.isPositionValid,

            leftPupilPositionX = leftPupil.pupilPosition.x,
            leftPupilPositionY = leftPupil.pupilPosition.y,

            rightPupilPositionX = rightPupil.pupilPosition.x,
            rightPupilPositionY = rightPupil.pupilPosition.y,

            // Eye Geometry Data
            isleftGeometricValid = leftGeometric.isValid,
            isRightGeometricValid = rightGeometric.isValid,

            leftOpenness = leftGeometric.eyeOpenness,
            rightOpenness = rightGeometric.eyeOpenness,

            leftWide = leftGeometric.eyeWide,
            rightWide = rightGeometric.eyeWide,

            leftSqueeze = leftGeometric.eyeSqueeze,
            rightSqueeze = rightGeometric.eyeSqueeze,

            // Head Pose
            headPosX = headTransform.position.x,
            headPosY = headTransform.position.y,
            headPosZ = headTransform.position.z,

            headRotX = headTransform.rotation.x,
            headRotY = headTransform.rotation.y,
            headRotZ = headTransform.rotation.z,
            headRotW = headTransform.rotation.w
        });
    }

    void OnApplicationQuit()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string downloadsPath = GetAndroidDownloadsPath();
        string filePath = Path.Combine(downloadsPath, $"eyetrackerdata_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
#else
        string filePath = Path.Combine(Application.persistentDataPath, $"eyetrackerdata_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
#endif

        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                writer.WriteLine("Timestamp," +
                    "isLeftGazeValid, isRightGazeValid," +
                    "leftGazePosX,leftGazePosY,leftGazePosZ," +
                    "leftGazeRotX,leftGazeRotY,leftGazeRotZ,leftGazeRotW," +
                    "rightGazePosX,rightGazePosY,rightGazePosZ," +
                    "rightGazeRotX,rightGazeRotY,rightGazeRotZ,rightGazeRotW," +
                    "isLeftPupilDiameterValid, isRightPupilDiameterValid," +
                    "leftPupilDiameter,rightPupilDiameter," +
                    "isLeftPupilPositionValid, isLRightPupilPositionValid" +
                    "leftPupilPositionX,leftPupilPositionY," +
                    "rightPupilPositionX,rightPupilPositionY," +
                    "isleftGeometricValid,isRightGeometricValid," +
                    "leftOpenness,rightOpenness," +
                    "leftWide,rightWide," +
                    "leftSqueeze,rightSqueeze," +
                    "headPosX,headPosY,headPosZ," +
                    "headRotX,headRotY,headRotZ,headRotW");
                foreach (var entry in dataEntries)
                {
                    writer.WriteLine($"{entry.timestamp}," +
                        $"{entry.isLeftGazeValid},{entry.isRightGazeValid}," +
                        $"{entry.leftGazePosX},{entry.leftGazePosY},{entry.leftGazePosZ}," +
                        $"{entry.leftGazeRotX},{entry.leftGazeRotY},{entry.leftGazeRotZ},{entry.leftGazeRotW}," +
                        $"{entry.rightGazePosX},{entry.rightGazePosY},{entry.rightGazePosZ}" +
                        $"{entry.rightGazeRotX},{entry.rightGazeRotY},{entry.rightGazeRotZ},{entry.rightGazeRotW}," +
                        $"{entry.isLeftPupilDiameterValid},{entry.isRightPupilDiameterValid}," +
                        $"{entry.leftPupilDiameter},{entry.rightPupilDiameter}," +
                        $"{entry.isLeftPupilPositionValid},{entry.isRightPupilPositionValid}," +
                        $"{entry.leftPupilPositionX},{entry.leftPupilPositionY}," +
                        $"{entry.rightPupilPositionX},{entry.rightPupilPositionY}," +
                        $"{entry.isleftGeometricValid},{entry.isRightGeometricValid}," +
                        $"{entry.leftOpenness},{entry.rightOpenness}," +
                        $"{entry.leftWide},{entry.rightWide}," +
                        $"{entry.leftSqueeze},{entry.rightSqueeze}," +
                        $"{entry.headPosX},{entry.headPosY},{entry.headPosZ}," +
                        $"{entry.headRotX},{entry.headRotY},{entry.headRotZ},{entry.headRotW}");
                }
            }
            Debug.Log($"CSV save to: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Save failure: {e.Message}");
        }
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private string GetAndroidDownloadsPath()
    {
        AndroidJavaClass environment = new AndroidJavaClass("android.os.Environment");
        AndroidJavaObject downloadsDir = environment.CallStatic<AndroidJavaObject>(
            "getExternalStoragePublicDirectory",
            environment.GetStatic<string>("DIRECTORY_DOWNLOADS")
        );
        return downloadsDir.Call<string>("getAbsolutePath");
    }
#endif
}