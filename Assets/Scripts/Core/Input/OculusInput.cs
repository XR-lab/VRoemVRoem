using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;

public class OculusInput : MonoBehaviour
{
    public delegate void ConfirmPress();
    public ConfirmPress OnConfirmPress;
    private OculusBindings oculusBindings;

    private void Start()
    {
        oculusBindings = new OculusBindings();
        oculusBindings.UI.Confirm.performed += c => OnConfirmPress();

        oculusBindings.Enable();
    }
}
