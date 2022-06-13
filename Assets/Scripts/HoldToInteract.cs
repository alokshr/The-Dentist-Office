using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HoldToInteract : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Camera being used to display UI")]
    private Camera Camera;
    [SerializeField]
    [Tooltip("Layer that interactable objects will be on")]
    private LayerMask LayerMask;
    [SerializeField]
    [Tooltip("How long the interact key must be held")]
    private float HoldTime = 2f;
    [SerializeField]
    [Tooltip("The root of the images")]
    private RectTransform ImageRoot;
    [SerializeField]
    [Tooltip("The outline that shows interaction progress")]
    private Image ProgressImage;
    [SerializeField]
    private TextMeshProUGUI ObjNameText;

    private Interactable ObjectToInteract;
    private float CurrentInteractTimerElapsed;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        SelectObject();

        if (HasObjectSelected())
        {
            ImageRoot.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E)) {
                IncrementTimer();
            }
            else
            {
                CurrentInteractTimerElapsed = 0f;
            }

            UpdateProgressImage();
        } else
        {
            ImageRoot.gameObject.SetActive(false);
        }
    }

    private void SelectObject()
    {
        Ray ray = Camera.ViewportPointToRay(Vector3.one);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, LayerMask))
        {
            var hitObj = hit.collider.GetComponent<Interactable>();

            if (hitObj == null)
            {
                ObjectToInteract = null;
            } else if (hitObj != null && hitObj != ObjectToInteract)
            {
                ObjectToInteract = hitObj;
                ObjNameText.text = "Use" + ObjectToInteract.gameObject.name;
            }
        } else
        {
            ObjectToInteract = null;
        }
    }

    private bool HasObjectSelected()
    {
        return ObjectToInteract != null;
    }

    private void IncrementTimer()
    {
        CurrentInteractTimerElapsed += Time.deltaTime;

        if (CurrentInteractTimerElapsed >= HoldTime)
        {
            ObjectToInteract.Interact();
        }
    }

    private void UpdateProgressImage()
    {
        float progress = CurrentInteractTimerElapsed / HoldTime;
        ProgressImage.fillAmount = progress;
    }


}
