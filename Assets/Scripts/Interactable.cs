using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    public enum Options
    {
        Held,
        Pressed
    };

    public Options interactType = Options.Held;
    public float interactTime = 1f;
    public bool oneTimeUse = false;
    public bool disabled = false;

    public virtual void Interact()
    {
        transform.position += new Vector3(0f, 1f, 0f);
    }
}
