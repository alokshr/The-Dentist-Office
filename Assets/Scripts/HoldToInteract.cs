using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HoldToInteract : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Camera being used to display UI")]
    private Camera _camera;
    [SerializeField]
    [Tooltip("Layer that interactable objects will be on")]
    private LayerMask _layerMask;
    [SerializeField]
    [Tooltip("How long the interact key must be held")]
    private float _holdTime = 2f;
    [SerializeField]
    [Tooltip("The root of the images")]
    private RectTransform _imageRoot;
    [SerializeField]
    [Tooltip("The outline that shows interaction progress")]
    private Image _progressImage;
    [SerializeField]
    private TextMeshProUGUI _objNameText;

    private Interactable _objectToInteract;
    private float _currentInteractTimerElapsed;
    private float _interactDistance = 100f;

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
            _imageRoot.gameObject.SetActive(true);

            if (Input.GetKey(KeyCode.E)) {
                IncrementTimer();
            }
            else
            {
                _currentInteractTimerElapsed = 0f;
            }

            UpdateProgressImage();
        } else
        {
            _imageRoot.gameObject.SetActive(false);
        }
    }

    private void SelectObject()
    {
        Ray ray = _camera.ViewportPointToRay(Vector3.one);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _interactDistance, _layerMask))
        {
            var hitObj = hit.collider.GetComponent<Interactable>();

            if (hitObj == null)
            {
                _objectToInteract = null;
            } else if (hitObj != null && hitObj != _objectToInteract)
            {
                _objectToInteract = hitObj;
                _objNameText.text = "Use " + _objectToInteract.gameObject.name;
            }
        } else
        {
            _objectToInteract = null;
        }
    }

    private bool HasObjectSelected()
    {
        return _objectToInteract != null;
    }

    private void IncrementTimer()
    {
        _currentInteractTimerElapsed += Time.deltaTime;

        if (_currentInteractTimerElapsed >= _holdTime)
        {
            _objectToInteract.Interact();
        }
    }

    private void UpdateProgressImage()
    {
        float progress = _currentInteractTimerElapsed / _holdTime;
        _progressImage.fillAmount = progress;
    }


}
