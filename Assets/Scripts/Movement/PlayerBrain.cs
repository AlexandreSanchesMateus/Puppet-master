using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    [SerializeField, BoxGroup("Dependencies")] private PuppetMovement _puppetMovement;
    [SerializeField, BoxGroup("Dependencies")] private PuppetInteraction _puppetInteraction;

    [SerializeField, BoxGroup("Inputs")] private InputActionProperty _rightArm;
    [SerializeField, BoxGroup("Inputs")] private InputActionProperty _leftArm;

    [SerializeField, BoxGroup("Inputs")] private InputActionProperty _rightLeg;
    [SerializeField, BoxGroup("Inputs")] private InputActionProperty _leftLeg;

    [SerializeField, BoxGroup("Inputs")] private InputActionProperty _rightHand;
    [SerializeField, BoxGroup("Inputs")] private InputActionProperty _leftHand;

    private void Start()
    {
        // Legs Inputs Actions
        _leftLeg.action.performed += LeftLegAction;
        _leftLeg.action.canceled += LeftLegStop;

        _rightLeg.action.performed += RightLegAction;
        _rightLeg.action.canceled += RightLegStop;

        // interaction
        _leftArm.action.performed += LeftArmAction;
        _leftArm.action.canceled += LeftArmStop;

        _rightArm.action.performed += RightArmAction;
        _rightArm.action.canceled += RightArmStop;
    }

    private void OnDestroy()
    {
        // Legs Inputs Actions
        _leftLeg.action.performed -= LeftLegAction;
        _leftLeg.action.canceled -= LeftLegStop;

        _rightLeg.action.performed -= RightLegAction;
        _rightLeg.action.canceled -= RightLegStop;

        // interaction
        _leftArm.action.performed -= LeftArmAction;
        _leftArm.action.canceled -= LeftArmStop;

        _rightArm.action.performed -= RightArmAction;
        _rightArm.action.canceled -= RightArmStop;
    }

    // Leg
    private void LeftLegAction(InputAction.CallbackContext callback) => _puppetMovement.SetLeftLegValue(callback.ReadValue<Vector2>());
    private void LeftLegStop(InputAction.CallbackContext callback) => _puppetMovement.SetLeftLegValue(Vector2.zero);

    private void RightLegAction(InputAction.CallbackContext callback) => _puppetMovement.SetRightLegValue(callback.ReadValue<Vector2>());
    private void RightLegStop(InputAction.CallbackContext callback) => _puppetMovement.SetRightLegValue(Vector2.zero);

    // Arm
    private void LeftArmAction(InputAction.CallbackContext callback) => _puppetInteraction.SetLeftArmAction(callback.ReadValue<Vector2>());
    private void LeftArmStop(InputAction.CallbackContext callback) => _puppetInteraction.SetLeftArmAction(Vector2.zero);

    private void RightArmAction(InputAction.CallbackContext callback) => _puppetInteraction.SetRightArmAction(callback.ReadValue<Vector2>());
    private void RightArmStop(InputAction.CallbackContext callback) => _puppetInteraction.SetRightArmAction(Vector2.zero);

    // Hand
}
