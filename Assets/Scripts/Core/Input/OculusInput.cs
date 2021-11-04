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

        //InputAction confirm = playerInput.actions.FindAction(oculusBindings.UI.Confirm.id);

        oculusBindings.UI.Confirm.performed += c => OnConfirmPress();

        oculusBindings.Enable();
    }
}
