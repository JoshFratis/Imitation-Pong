using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem; 
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public PlayerInput input;
    private InputAction up;
    private InputAction down;
    private InputAction confirm;

    public string menuTitle;
    [SerializeField] int titleSize;

    public string[] entries;
    public UnityEvent[] destinations;

    [SerializeField] Text text;

    int pointer;

    private void Awake()
    {
        input = new PlayerInput();
    }

    private void OnEnable()
    {
        pointer = 0;

        up = input.Menu.Up;
        down = input.Menu.Down;
        confirm = input.Menu.Confirm;
        
        up.Enable();
        down.Enable();
        confirm.Enable();

        up.performed += Up;
        down.performed += Down;
        confirm.performed += Confirm;
    }

    private void OnDisable()
    {
        up.Disable();
        down.Disable();
        confirm.Disable();
    }

    private void Up(InputAction.CallbackContext context)
    {
        pointer--;
        Debug.Log("Up");
    }

    private void Down(InputAction.CallbackContext context)
    {
        pointer++;
        Debug.Log("Down");
    }

    private void Confirm(InputAction.CallbackContext context)
    {
        Debug.Log("Confirm");
        destinations[pointer].Invoke();
        gameObject.SetActive(false);
    }

    void Update()
    {
        // Wrap pointer
        if (pointer < 0)
        {
            pointer = entries.Length - 1;
        }
        else if (pointer >= entries.Length)
        {
            pointer = 0;
        }

        // Display Title in specified size
        text.text = "<size=" + titleSize.ToString() + ">" + menuTitle + "</size>" + "\n";

        // List entries
        for (int i = 0; i < entries.Length; i++)
        {
            if (pointer == i)
            {
                // List pointed entry in white with pointer
                text.text += "> <color=white>"+entries[i]+"</color>" + "\n";
            }
            else 
            {
                // List all other entries in grey
                text.text += "<color=grey>"+entries[i]+"</color>" + "\n";
            }
        }
    }
}
