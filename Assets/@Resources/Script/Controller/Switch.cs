using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Switch : MonoBehaviour
{
    enum State
    {
        Pressed,
        UnPressed
    }
    [SerializeField] MonoBehaviour _interactableObject;
    IInteractable _interactable;
    GameObject _pressed;
    GameObject _default;

    State _state = State.UnPressed;

    State CurrentState
    {
        get { return _state; }
        set {
            _state = value;
            switch (_state)
            {
                case State.Pressed:
                    _pressed.SetActive(true);
                    _default.SetActive(false);
                    _interactable.Interact();
                    break;
                case State.UnPressed:
                    _pressed.SetActive(false);;
                    _default.SetActive(true);
                    _interactable.DeInteract();
                    break;
            }
        }
    }
    void Start()
    {
        _pressed = transform.Find("Pressed").gameObject;
        _default = transform.Find("Default").gameObject;

        _interactable = _interactableObject as IInteractable;
    }
    private void OnTriggerEnter(Collider other)
    {
        CurrentState = State.Pressed;
    }
    private void OnTriggerExit(Collider other)
    {
        CurrentState = State.UnPressed;
    }
}
