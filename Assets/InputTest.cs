using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{

    public InputActionProperty TestActionValue;
    public InputActionProperty TestActionButton;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float value = TestActionValue.action.ReadValue<float>();
        Debug.Log("VALUE:" + value);

        bool button = TestActionButton.action.IsPressed();
        Debug.Log("Button:" + button);
    }
}
