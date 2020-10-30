using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private PasswordController passwordController;
    [SerializeField] private int thisNumber;

    private void Start() {
        passwordController = GetComponentInParent<PasswordController>();
    }

    public void ButtonPushed() {
        passwordController.ButtonPushed(thisNumber);
    }
}
