//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/GameInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @GameInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInputActions"",
    ""maps"": [
        {
            ""name"": ""ActionMap"",
            ""id"": ""5cde61de-4919-4bb9-a8c1-8ef12ff4c2b8"",
            ""actions"": [
                {
                    ""name"": ""Tap"",
                    ""type"": ""Button"",
                    ""id"": ""6293593d-f20e-45aa-9057-e7df5250b3a0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Swipe"",
                    ""type"": ""Value"",
                    ""id"": ""e717895b-f679-487d-9af6-6fff04267ee1"",
                    ""expectedControlType"": ""Delta"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""HoldToDrag"",
                    ""type"": ""Button"",
                    ""id"": ""dcb0d8d8-9e28-461a-8b44-5129ac59bafb"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""89d0cdbe-db9e-4e21-aea2-d1b197404c4d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb41a3fb-67af-4e0f-93ec-bc851a68a592"",
                    ""path"": ""<Touchscreen>/primaryTouch/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f27a087c-d5c4-448d-9cf2-a2a9ec507cca"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swipe"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""25804665-89f3-4071-a4b3-1f590359c79f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Hold(duration=0.1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoldToDrag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2fa1d995-ed35-4ee8-b12e-3d3a53cfd3be"",
                    ""path"": ""<Touchscreen>/primaryTouch/tap"",
                    ""interactions"": ""Hold(duration=0.1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoldToDrag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // ActionMap
        m_ActionMap = asset.FindActionMap("ActionMap", throwIfNotFound: true);
        m_ActionMap_Tap = m_ActionMap.FindAction("Tap", throwIfNotFound: true);
        m_ActionMap_Swipe = m_ActionMap.FindAction("Swipe", throwIfNotFound: true);
        m_ActionMap_HoldToDrag = m_ActionMap.FindAction("HoldToDrag", throwIfNotFound: true);
    }

    ~@GameInputActions()
    {
        UnityEngine.Debug.Assert(!m_ActionMap.enabled, "This will cause a leak and performance issues, GameInputActions.ActionMap.Disable() has not been called.");
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // ActionMap
    private readonly InputActionMap m_ActionMap;
    private List<IActionMapActions> m_ActionMapActionsCallbackInterfaces = new List<IActionMapActions>();
    private readonly InputAction m_ActionMap_Tap;
    private readonly InputAction m_ActionMap_Swipe;
    private readonly InputAction m_ActionMap_HoldToDrag;
    public struct ActionMapActions
    {
        private @GameInputActions m_Wrapper;
        public ActionMapActions(@GameInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Tap => m_Wrapper.m_ActionMap_Tap;
        public InputAction @Swipe => m_Wrapper.m_ActionMap_Swipe;
        public InputAction @HoldToDrag => m_Wrapper.m_ActionMap_HoldToDrag;
        public InputActionMap Get() { return m_Wrapper.m_ActionMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ActionMapActions set) { return set.Get(); }
        public void AddCallbacks(IActionMapActions instance)
        {
            if (instance == null || m_Wrapper.m_ActionMapActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ActionMapActionsCallbackInterfaces.Add(instance);
            @Tap.started += instance.OnTap;
            @Tap.performed += instance.OnTap;
            @Tap.canceled += instance.OnTap;
            @Swipe.started += instance.OnSwipe;
            @Swipe.performed += instance.OnSwipe;
            @Swipe.canceled += instance.OnSwipe;
            @HoldToDrag.started += instance.OnHoldToDrag;
            @HoldToDrag.performed += instance.OnHoldToDrag;
            @HoldToDrag.canceled += instance.OnHoldToDrag;
        }

        private void UnregisterCallbacks(IActionMapActions instance)
        {
            @Tap.started -= instance.OnTap;
            @Tap.performed -= instance.OnTap;
            @Tap.canceled -= instance.OnTap;
            @Swipe.started -= instance.OnSwipe;
            @Swipe.performed -= instance.OnSwipe;
            @Swipe.canceled -= instance.OnSwipe;
            @HoldToDrag.started -= instance.OnHoldToDrag;
            @HoldToDrag.performed -= instance.OnHoldToDrag;
            @HoldToDrag.canceled -= instance.OnHoldToDrag;
        }

        public void RemoveCallbacks(IActionMapActions instance)
        {
            if (m_Wrapper.m_ActionMapActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IActionMapActions instance)
        {
            foreach (var item in m_Wrapper.m_ActionMapActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ActionMapActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ActionMapActions @ActionMap => new ActionMapActions(this);
    public interface IActionMapActions
    {
        void OnTap(InputAction.CallbackContext context);
        void OnSwipe(InputAction.CallbackContext context);
        void OnHoldToDrag(InputAction.CallbackContext context);
    }
}
