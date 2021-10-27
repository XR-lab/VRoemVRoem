// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input/OculusBindings.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @OculusBindings : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @OculusBindings()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""OculusBindings"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""6e491709-92e6-45b1-a4dc-2bb8256da178"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""18aa6924-e90b-4289-b921-ef3378606bcb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3cfb2031-57e2-4d1f-807b-3c3357032ce0"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""792bb506-9fd4-42f0-86f7-b93fe78452af"",
            ""actions"": [
                {
                    ""name"": ""Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""e44cb706-f0bd-46d2-8084-777daca1f4bf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Clicking"",
                    ""type"": ""Button"",
                    ""id"": ""8ab304e0-2205-4512-ade9-c5dd5f92faa3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b54f77f3-95fc-4159-93e0-5e08adfd6760"",
                    ""path"": ""<XRController>{LeftHand}/primaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ead542d-1e90-4e6a-b604-52ff1898209c"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Clicking"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Newaction = m_Player.FindAction("New action", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Confirm = m_UI.FindAction("Confirm", throwIfNotFound: true);
        m_UI_Clicking = m_UI.FindAction("Clicking", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Newaction;
    public struct PlayerActions
    {
        private @OculusBindings m_Wrapper;
        public PlayerActions(@OculusBindings wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_Player_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Confirm;
    private readonly InputAction m_UI_Clicking;
    public struct UIActions
    {
        private @OculusBindings m_Wrapper;
        public UIActions(@OculusBindings wrapper) { m_Wrapper = wrapper; }
        public InputAction @Confirm => m_Wrapper.m_UI_Confirm;
        public InputAction @Clicking => m_Wrapper.m_UI_Clicking;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Confirm.started -= m_Wrapper.m_UIActionsCallbackInterface.OnConfirm;
                @Confirm.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnConfirm;
                @Confirm.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnConfirm;
                @Clicking.started -= m_Wrapper.m_UIActionsCallbackInterface.OnClicking;
                @Clicking.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnClicking;
                @Clicking.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnClicking;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Confirm.started += instance.OnConfirm;
                @Confirm.performed += instance.OnConfirm;
                @Confirm.canceled += instance.OnConfirm;
                @Clicking.started += instance.OnClicking;
                @Clicking.performed += instance.OnClicking;
                @Clicking.canceled += instance.OnClicking;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    public interface IPlayerActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnConfirm(InputAction.CallbackContext context);
        void OnClicking(InputAction.CallbackContext context);
    }
}
