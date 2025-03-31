using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using System;

public class UI_CloseButton : VisualElement
{
    // Permet d'afficher le composant dans UI Toolkit
    public new class UxmlFactory : UxmlFactory<UI_CloseButton, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits { }

    private Button _button;
    private VisualElement _parentElement;

    public UI_CloseButton()
    {
        _button = new Button { text = "X" };
        _button.AddToClassList("close-button");
        _button.clicked += OnCloseButtonClicked;
        Add(_button);
    }

    private void OnCloseButtonClicked()
    {
        _parentElement = this.parent;

        if (_parentElement != null)
        {
            _parentElement.style.display = DisplayStyle.None;
        }
    }
}
