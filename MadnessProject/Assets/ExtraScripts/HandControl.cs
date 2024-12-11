using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandControl : MonoBehaviour
{
    public InputAction handTrigger;

    private void Update()
    {
        if (handTrigger != null)
        {
            Debug.Log("You pressed a button AAAAAAAAAAAA");
        }
    }
}
