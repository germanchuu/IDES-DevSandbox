using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class PopUpNewDefaultSentenceHandler : MonoBehaviour
{
    private Action onClickCallback;

    private DefaultSentencesManager manager;
    private UIDocument popUpDoc;

    private TextField txtNewSentence;    
    private Button popUpConfirm;
    private Button popUpCancel;    

    void Awake()
    {        
        manager = new DefaultSentencesManager();

        popUpDoc = GetComponent<UIDocument>();
        VisualElement root = popUpDoc.rootVisualElement;

        txtNewSentence = root.Q<TextField>("txtNewSentence");

        txtNewSentence.RegisterCallback<ChangeEvent<string>>(OnBoolChangeEvent);

        popUpConfirm = root.Q<Button>("btnConfirm");
        popUpCancel = root.Q<Button>("btnCancel");
        popUpCancel.clicked += HidePopUp;

        popUpDoc.rootVisualElement.style.display = DisplayStyle.None;
    }

    private void OnBoolChangeEvent(ChangeEvent<string> evt)
    {
        if (!string.IsNullOrEmpty(txtNewSentence.value) && txtNewSentence.value.Any(c => char.IsLetterOrDigit(c)))
            popUpConfirm.SetEnabled(true);
        else
            popUpConfirm.SetEnabled(false);
    }

    public void ShowPopUp(Action method)
    {
        popUpDoc.rootVisualElement.style.display = DisplayStyle.Flex;
        popUpConfirm.SetEnabled(false);

        onClickCallback = method;

        txtNewSentence.value = "";
        txtNewSentence.Focus();        

        popUpConfirm.UnregisterCallback<ClickEvent>(OnConfirmButtonClick);
        popUpConfirm.RegisterCallback<ClickEvent>(OnConfirmButtonClick);
    }

    public void HidePopUp()
    {
        txtNewSentence.value = "";

        popUpDoc.rootVisualElement.style.display = DisplayStyle.None;
        popUpConfirm.UnregisterCallback<ClickEvent>(OnConfirmButtonClick);
    }

    private void OnConfirmButtonClick(ClickEvent evt)
    {
        manager.InsertData(txtNewSentence.value);
        onClickCallback?.Invoke();
        HidePopUp();
    }
}
