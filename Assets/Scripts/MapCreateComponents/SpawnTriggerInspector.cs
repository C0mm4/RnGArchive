using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class SpawnTriggerInspector : TriggerInspector
{
    public GameObject SpawnPPrefab;


    List<GameObject> spawnPInputs = new();
    List<GameObject> transTmp = new();

    public override void SetTriggerData(Trigger trigger)
    {
        base.SetTriggerData(trigger);
        var st = trigger.GetComponent<SpawnTrigger>();
        for (int i = 0; i < st.Trans.Count; i++)
        {
            AddSpawnPointOnLoad();
            spawnPInputs[i].GetComponent<SpawnTriggerInput>().id.text = st.mobs[i];
            spawnPInputs[i].GetComponent<SpawnTriggerInput>().x.text = st.Trans[i].position.x.ToString();
            spawnPInputs[i].GetComponent<SpawnTriggerInput>().y.text = st.Trans[i].position.y.ToString();
        }
    }

    public void AddSpawnPoint()
    {
        GameObject inputField = Instantiate(SpawnPPrefab, components[4].transform);
        spawnPInputs.Add(inputField);
        GameObject go = new GameObject();
        go.transform.SetParent(controller.DataShowObj.transform);
        transTmp.Add(go);
        controller.DataShowObj.GetComponent<SpawnTrigger>().Trans.Add(go.transform);
        controller.DataShowObj.GetComponent<SpawnTrigger>().mobs.Add("");

        int index = spawnPInputs.Count - 1;
        TMP_InputField input = inputField.transform.Find("IDInputField (TMP)").GetComponent<TMP_InputField>();
        input.onValueChanged.AddListener((string text) => UpdateSpawnID(index, text));

        input = inputField.transform.Find("XInputField (TMP)").GetComponent<TMP_InputField>();
        input.onValueChanged.AddListener((string text) => UpdateSpawnX(index, text));

        input = inputField.transform.Find("YInputField (TMP)").GetComponent<TMP_InputField>();
        input.onValueChanged.AddListener((string text) => UpdateSpawnY(index, text));

        Button deleteButton = inputField.transform.Find("DeleteButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => RemoveInputFieldNext(index));

        inputField.GetComponent<RectTransform>().localPosition = new Vector3(25, -(components[4].sizeY + 70), 0);
        components[4].AddSize(70);
        contents.GetComponent<RectTransform>().sizeDelta = new Vector2(contents.GetComponent<RectTransform>().sizeDelta.x, contents.GetComponent<RectTransform>().sizeDelta.y + 70);

        SetComponentsRect();
    }

    private void AddSpawnPointOnLoad()
    {
        GameObject inputField = Instantiate(SpawnPPrefab, components[4].transform);
        spawnPInputs.Add(inputField);
        GameObject go = new GameObject();
        go.transform.SetParent(controller.DataShowObj.transform);
        transTmp.Add(go);

        int index = spawnPInputs.Count - 1;
        TMP_InputField input = inputField.transform.Find("IDInputField (TMP)").GetComponent<TMP_InputField>();
        input.onValueChanged.AddListener((string text) => UpdateSpawnID(index, text));

        input = inputField.transform.Find("XInputField (TMP)").GetComponent<TMP_InputField>();
        input.onValueChanged.AddListener((string text) => UpdateSpawnX(index, text));

        input = inputField.transform.Find("YInputField (TMP)").GetComponent<TMP_InputField>();
        input.onValueChanged.AddListener((string text) => UpdateSpawnY(index, text));

        Button deleteButton = inputField.transform.Find("DeleteButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => RemoveInputFieldNext(index));

        inputField.GetComponent<RectTransform>().localPosition = new Vector3(25, -(components[4].sizeY + 70), 0);
        components[4].AddSize(70);
        contents.GetComponent<RectTransform>().sizeDelta = new Vector2(contents.GetComponent<RectTransform>().sizeDelta.x, contents.GetComponent<RectTransform>().sizeDelta.y + 70);
        SetComponentsRect();
    }
    public void RemoveSpawnPointField(int index)
    {
        // 리스트와 UI에서 해당 인덱스 삭제
        Destroy(spawnPInputs[index]);
        spawnPInputs.RemoveAt(index);
        controller.DataShowObj.GetComponent<SpawnTrigger>().Trans.RemoveAt(index);
        controller.DataShowObj.GetComponent<SpawnTrigger>().mobs.RemoveAt(index);
        transTmp.RemoveAt(index);

        // 인덱스 재설정 (남아 있는 필드들의 인덱스를 다시 맞춰줌)
        for (int i = 0; i < spawnPInputs.Count; i++)
        {
            int currentIndex = i; // 지역 변수를 사용해 인덱스를 고정
            TMP_InputField input = spawnPInputs[i].transform.Find("IDInputField (TMP)").GetComponent<TMP_InputField>();
            input.onValueChanged.RemoveAllListeners();
            input.onValueChanged.AddListener((string text) => UpdateSpawnID(index, text));

            input = spawnPInputs[i].transform.Find("XInputField (TMP)").GetComponent<TMP_InputField>();
            input.onValueChanged.RemoveAllListeners();
            input.onValueChanged.AddListener((string text) => UpdateSpawnX(index, text));

            input = spawnPInputs[i].transform.Find("YInputField (TMP)").GetComponent<TMP_InputField>();
            input.onValueChanged.RemoveAllListeners();
            input.onValueChanged.AddListener((string text) => UpdateSpawnY(index, text));

            Button deleteButton = spawnPInputs[i].transform.Find("DeleteButton").GetComponent<Button>();
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() => RemoveInputFieldNext(index));

            spawnPInputs[i].GetComponent<RectTransform>().localPosition = new Vector3(25, -(25 + 70 * (i + 1)));
        }
        components[4].RemoveSize(70);
        contents.GetComponent<RectTransform>().sizeDelta = new Vector2(contents.GetComponent<RectTransform>().sizeDelta.x, contents.GetComponent<RectTransform>().sizeDelta.y -70);
        SetComponentsRect();
    }

    public void UpdateSpawnID(int index, string text)
    {
        controller.DataShowObj.GetComponent<SpawnTrigger>().mobs[index] = text;
    }

    public void UpdateSpawnX(int index, string x)
    {
        controller.DataShowObj.GetComponent<SpawnTrigger>().Trans[index].position = new Vector3(float.Parse(x), controller.DataShowObj.GetComponent<SpawnTrigger>().Trans[index].position.y, 0);
    }

    public void UpdateSpawnY(int index, string y)
    {
        controller.DataShowObj.GetComponent<SpawnTrigger>().Trans[index].position = new Vector3(controller.DataShowObj.GetComponent<SpawnTrigger>().Trans[index].position.x, float.Parse(y), 0);
    }

}
