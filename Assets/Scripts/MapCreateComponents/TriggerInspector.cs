using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TriggerInspector : MapCreateInspector
{
    public TMP_InputField triggerId;
    public TMP_Dropdown condition;
    public Scrollbar HPRatio;

    public GameObject inputFieldPrefab;

    public List<GameObject> nodeInputFields = new();
    public List<GameObject> nextInputFields = new();

    public Button AddNodeIds;
    public Button AddNextIds;

    public void AddInputFieldNodeList()
    {
        GameObject inputField = Instantiate(inputFieldPrefab, components[1].transform);
        nodeInputFields.Add(inputField);
        controller.DataShowObj.GetComponent<Trigger>().nodeIds.Add("");

        int index = nodeInputFields.Count - 1;
        TMP_InputField input = inputField.GetComponent<TMP_InputField>();
        input.onValueChanged.AddListener((string text) => UpdateTriggerList(index, text));

        Button deleteButton = inputField.transform.Find("DeleteButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => RemoveInputField(index));

        inputField.GetComponent<RectTransform>().localPosition = new Vector3(25, -(components[1].sizeY + 30), 0);
        components[1].AddSize(30);

        SetComponentsRect();
    }

    public void AddInputFieldNextList()
    {
        GameObject inputField = Instantiate(inputFieldPrefab, components[2].transform);
        nextInputFields.Add(inputField);
        controller.DataShowObj.GetComponent<Trigger>().nextTriggerId.Add("");

        int index = nextInputFields.Count - 1;
        TMP_InputField input = inputField.GetComponent<TMP_InputField>();
        input.onValueChanged.AddListener((string text) => UpdateTriggerListNext(index, text));

        Button deleteButton = inputField.transform.Find("DeleteButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => RemoveInputFieldNext(index));

        inputField.GetComponent<RectTransform>().localPosition = new Vector3(25, -(components[2].sizeY + 30), 0);
        components[2].AddSize(30);

        SetComponentsRect();
    }

    public void UpdateTriggerList(int index, string value)
    {
        if (index >= 0 && index < controller.DataShowObj.GetComponent<Trigger>().nodeIds.Count)
        {
            controller.DataShowObj.GetComponent<Trigger>().nodeIds[index] = value;
        }
    }

    public void UpdateTriggerListNext(int index, string value)
    {
        if (index >= 0 && index < controller.DataShowObj.GetComponent<Trigger>().nextTriggerId.Count)
        {
            controller.DataShowObj.GetComponent<Trigger>().nextTriggerId[index] = value;
        }
    }

    public void RemoveInputField(int index)
    {
        // 리스트와 UI에서 해당 인덱스 삭제
        Destroy(nodeInputFields[index]);
        nodeInputFields.RemoveAt(index);
        controller.DataShowObj.GetComponent<Trigger>().nodeIds.RemoveAt(index);

        // 인덱스 재설정 (남아 있는 필드들의 인덱스를 다시 맞춰줌)
        for (int i = 0; i < nodeInputFields.Count; i++)
        {
            int currentIndex = i; // 지역 변수를 사용해 인덱스를 고정
            TMP_InputField input = nodeInputFields[i].GetComponent<TMP_InputField>();
            input.onValueChanged.RemoveAllListeners(); // 기존 리스너 제거
            input.onValueChanged.AddListener((string text) => UpdateTriggerList(currentIndex, text));

            Button deleteButton = nodeInputFields[i].transform.Find("DeleteButton").GetComponent<Button>();
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() => RemoveInputField(currentIndex));

            nodeInputFields[i].GetComponent<RectTransform>().localPosition = new Vector3(25, -(25 + 30 * (i + 1)));
        }
        components[1].RemoveSize(30);
        SetComponentsRect();
    }
    public void RemoveInputFieldNext(int index)
    {
        // 리스트와 UI에서 해당 인덱스 삭제
        Destroy(nextInputFields[index]);
        nextInputFields.RemoveAt(index);
        controller.DataShowObj.GetComponent<Trigger>().nextTriggerId.RemoveAt(index);

        // 인덱스 재설정 (남아 있는 필드들의 인덱스를 다시 맞춰줌)
        for (int i = 0; i < nextInputFields.Count; i++)
        {
            int currentIndex = i; // 지역 변수를 사용해 인덱스를 고정
            TMP_InputField input = nextInputFields[i].GetComponent<TMP_InputField>();
            input.onValueChanged.RemoveAllListeners(); // 기존 리스너 제거
            input.onValueChanged.AddListener((string text) => UpdateTriggerListNext(currentIndex, text));

            Button deleteButton = nextInputFields[i].transform.Find("DeleteButton").GetComponent<Button>();
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() => RemoveInputFieldNext(currentIndex));

            nextInputFields[i].GetComponent<RectTransform>().localPosition = new Vector3(25, -(25 + 30 * (i + 1)));
        }
        components[2].RemoveSize(30);

        SetComponentsRect();
    }

    public virtual void SetTriggerData(Trigger trigger)
    {
        triggerId.text = trigger.data.id;
        HPRatio.value = trigger.condi.HPRatio;
        condition.value = ((int)trigger.condi.condition);
        for(int i = 0; i < trigger.nodeIds.Count; i++)
        {
            AddNodeInputFieldOnLoad();
            nodeInputFields[i].GetComponent<TMP_InputField>().text = trigger.nodeIds[i];
        }
        for(int i = 0; i < trigger.nextTriggerId.Count; i++)
        {
            AddNextInputFieldOnLoad();
            nextInputFields[i].GetComponent<TMP_InputField>().text = trigger.nextTriggerId[i];
        }

        AddNodeIds.onClick.AddListener(() => AddInputFieldNodeList());
        AddNextIds.onClick.AddListener(() => AddInputFieldNextList());
    }

    public override void SetData(GameObject go)
    {
        base.SetData(go);
        SetTriggerData(go.GetComponent<Trigger>());
    }

    public void AddNodeInputFieldOnLoad()
    {
        GameObject inputField = Instantiate(inputFieldPrefab, components[1].transform);
        nodeInputFields.Add(inputField);

        int index = nodeInputFields.Count - 1;
        TMP_InputField input = inputField.GetComponent<TMP_InputField>();
        input.onValueChanged.AddListener((string text) => UpdateTriggerList(index, text));

        Button deleteButton = inputField.transform.Find("DeleteButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => RemoveInputField(index));

        inputField.GetComponent<RectTransform>().localPosition = new Vector3(25, -(components[1].sizeY + 30), 0);
        components[1].AddSize(30);

        SetComponentsRect();
    }

    public void AddNextInputFieldOnLoad()
    {
        GameObject inputField = Instantiate(inputFieldPrefab, components[2].transform);
        nextInputFields.Add(inputField);

        int index = nextInputFields.Count - 1;
        TMP_InputField input = inputField.GetComponent<TMP_InputField>();
        input.onValueChanged.AddListener((string text) => UpdateTriggerListNext(index, text));

        Button deleteButton = inputField.transform.Find("DeleteButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => RemoveInputFieldNext(index));

        inputField.GetComponent<RectTransform>().localPosition = new Vector3(25, -(components[2].sizeY + 30), 0);
        components[2].AddSize(30);

        SetComponentsRect();
    }
}
