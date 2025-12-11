using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class PopUpQuickAccessHandler : MonoBehaviour
{
    private QuickAccessManager manager;
    private UIDocument popUpDoc;

    private Label popUpMessage;
    private Label[] lblQuickAccess = new Label[3];
    private Button popUpConfirm;
    private Button popUpCancel;

    private int selectedSentence = -1;
    private string sentenceToChange;

    void Awake()
    {        
        manager = new QuickAccessManager();
        InsertInitialData();

        popUpDoc = GetComponent<UIDocument>();
        VisualElement root = popUpDoc.rootVisualElement;

        popUpMessage = root.Q<Label>("lblMessage");

        for (int i = 0; i < lblQuickAccess.Length; i++)
        {
            lblQuickAccess[i] = root.Q<Label>("lblQuickAccess" + (i + 1));
            int index = i;
            lblQuickAccess[i].RegisterCallback<ClickEvent>(e =>
            {
                selectedSentence = index;
                popUpConfirm.SetEnabled(true);
                lblQuickAccess[index].AddToClassList("textSelected");
                for (int j = 0; j < lblQuickAccess.Length; j++)
                {
                    if (j != index)
                        lblQuickAccess[j].RemoveFromClassList("textSelected");
                }
            });
        }

        popUpConfirm = root.Q<Button>("btnConfirm");
        popUpCancel = root.Q<Button>("btnCancel");
        popUpCancel.clicked += HidePopUp;

        popUpDoc.rootVisualElement.style.display = DisplayStyle.None;
    }

    private void InsertInitialData()
    {
        string[] s = manager.GetData();
        if (s != null)
            return;

        manager.InsertData("Hola, buenas tardes");
        manager.InsertData("Muchas gracias");
        manager.InsertData("¿Cómo estás?");
    }

    public void ShowPopUp(string message, string sentence)
    {
        popUpDoc.rootVisualElement.style.display = DisplayStyle.Flex;
        popUpConfirm.SetEnabled(false);
        popUpMessage.text = message;

        sentenceToChange = sentence;
        string[] sentences = manager.GetData();

        for (int i = 0; i < lblQuickAccess.Length; i++)
        {
            lblQuickAccess[i].text = sentences[i];
        }

        popUpConfirm.UnregisterCallback<ClickEvent>(OnConfirmButtonClick);
        popUpConfirm.RegisterCallback<ClickEvent>(OnConfirmButtonClick);
    }

    public void HidePopUp()
    {
        selectedSentence = -1;

        for (int i = 0; i < lblQuickAccess.Length; i++)
        {            
            lblQuickAccess[i].RemoveFromClassList("textSelected");
        }

        popUpDoc.rootVisualElement.style.display = DisplayStyle.None;
        popUpConfirm.UnregisterCallback<ClickEvent>(OnConfirmButtonClick);
    }

    private void OnConfirmButtonClick(ClickEvent evt)
    {
        if (selectedSentence == -1)
            return;

        string[] sentences = manager.GetData();
        sentences[selectedSentence] = sentenceToChange;
        manager.UpdateData(sentences);                

        HidePopUp();
    }
}
