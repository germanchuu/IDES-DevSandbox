using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PopUpHandler : MonoBehaviour
{
    private Action onClickCallback;

    private UIDocument popUpDoc;
    private Label popUpTitle;
    private Label popUpMessage;
    private Button popUpConfirm;
    private Button popUpCancel;

    private void Start()
    {        
        popUpDoc = GetComponent<UIDocument>();
        VisualElement root = popUpDoc.rootVisualElement;
        popUpTitle = root.Q<Label>("lblTitle");
        popUpMessage = root.Q<Label>("lblMessage");
        popUpConfirm = root.Q<Button>("btnConfirm");
        popUpCancel = root.Q<Button>("btnCancel");

        popUpCancel.RegisterCallback<ClickEvent>(e =>
        {
            HidePopUp();            
        });

        popUpDoc.rootVisualElement.style.display = DisplayStyle.None;
    }

    public void ShowPopUp(string title, string message, Action method)
    {
        onClickCallback = method;

        popUpDoc.rootVisualElement.style.display = DisplayStyle.Flex;
        popUpTitle.text = title;
        popUpMessage.text = message;

        popUpConfirm.UnregisterCallback<ClickEvent>(OnConfirmButtonClick);
        popUpConfirm.RegisterCallback<ClickEvent>(OnConfirmButtonClick);
    }

    public void HidePopUp()
    {
        popUpDoc.rootVisualElement.style.display = DisplayStyle.None;
        popUpConfirm.UnregisterCallback<ClickEvent>(OnConfirmButtonClick);
    }

    private void OnConfirmButtonClick(ClickEvent evt)
    {
        onClickCallback?.Invoke();
        HidePopUp();
    }
}
