using System;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public partial class InputManager
{

    public float _MenuCloseT;
    public float MenuCloseT { set { _MenuCloseT = value; } get { return _MenuCloseT; } }
    private string screenshotFolder = "Screenshots";

    public KeySetting _keySettings;


    // Key Input Event Check

    public void Initialize()
    {
        _keySettings = GameManager.Instance.keysetting;
    }

    // Check Mouse position and key inputs
    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.F12))
        {
            // 스크린샷 찍기
            CaptureScreenshot();
        }
    }
    void CaptureScreenshot()
    {
        // Screenshot 폴더 경로 생성
        string folderPath = Path.Combine(Application.dataPath, screenshotFolder);

        // Screenshot 폴더가 없으면 생성
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // 현재 날짜와 시간을 이용하여 파일명 생성
        string screenshotName = "Screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

        // 스크린샷 찍기
        string filePath = Path.Combine(folderPath, screenshotName);
        ScreenCapture.CaptureScreenshot(filePath);

        // 콘솔에 메시지 출력
        Debug.Log("Screenshot captured: " + screenshotName);
        Debug.Log("Saved to: " + filePath);
    }


}
