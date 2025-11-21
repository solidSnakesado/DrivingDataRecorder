using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class DrivingDataRecorder : MonoBehaviour
{
    public  Camera          cam             = null;
    public  int             width           = 224;
    public  int             height          = 224;

    private string          datasetPath     = string.Empty;
    private string          csvPath         = string.Empty;

    private int             frameIndex      = 1;
    private RenderTexture   rt              = null;
    private Texture2D       tex             = null;

    private float           steering        = 0f;
    private float           throttle        = 0f;
    private float           brake           = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        datasetPath = Path.Combine(Application.dataPath, "DrivingDataset");
        Directory.CreateDirectory(datasetPath);

        csvPath = Path.Combine(datasetPath, "label.csv");

        // csv 헤더 생성
        File.WriteAllText(csvPath, "frame,steering,throttle,break\n");

        // 카메라 렌더링 준비
        rt  = new RenderTexture(width, height, 24);
        tex = new Texture2D(width, height, TextureFormat.RGB24, false);
    }

    private void LateUpdate()
    {
        // 사용자 입력 읽기
        steering  = InputUtility.GetSteering();
        throttle  = InputUtility.GetThrottle();
        brake     = InputUtility.GetBrake();

        SaveCameraImage();
        SaveLabelCsv();

        ++frameIndex;
    }

    private void SaveCameraImage()
    {
        cam.targetTexture = rt;
        cam.Render();
        RenderTexture.active = rt;

        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;

        byte[] bytes = tex.EncodeToPNG();
        string filename = $"frame_{frameIndex:D6}.png";
        File.WriteAllBytes(Path.Combine(datasetPath, filename), bytes);
    }

    private void SaveLabelCsv()
    {
        string line = $"{frameIndex},{steering:F4},{throttle:F4},{brake:F4}\n";
        File.AppendAllText(csvPath, line);
    }
}
