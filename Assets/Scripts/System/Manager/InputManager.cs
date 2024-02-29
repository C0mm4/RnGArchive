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
            // ��ũ���� ���
            CaptureScreenshot();
        }
    }
    void CaptureScreenshot()
    {
        // Screenshot ���� ��� ����
        string folderPath = Path.Combine(Application.dataPath, screenshotFolder);

        // Screenshot ������ ������ ����
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // ���� ��¥�� �ð��� �̿��Ͽ� ���ϸ� ����
        string screenshotName = "Screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

        // ��ũ���� ���
        string filePath = Path.Combine(folderPath, screenshotName);
        ScreenCapture.CaptureScreenshot(filePath);

        // �ֿܼ� �޽��� ���
        Debug.Log("Screenshot captured: " + screenshotName);
        Debug.Log("Saved to: " + filePath);
    }


}
