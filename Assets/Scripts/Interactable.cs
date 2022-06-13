using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    enum Options
    {
        Held,
        Pressed
    };

    [SerializeField]
    Options _interactType = Options.Held;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        transform.position += new Vector3(0f, 10f, 0f);
    }
}
