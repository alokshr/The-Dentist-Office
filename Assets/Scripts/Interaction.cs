using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
    
public class Interaction : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Camera being used to display UI")]
    private Camera _camera;
    [SerializeField]
    [Tooltip("Layer that interactable objects will be on")]
    private LayerMask _layerMask;
    [SerializeField]
    [Tooltip("How long the interact key must be held")]
    private float _holdTime;
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

    private bool _interacted = false;

    // Start is called before the first frame update
    private void Start()
    {
        _camera = Camera.main;
        _layerMask = LayerMask.NameToLayer("Everything");
        _imageRoot = GameObject.Find("Borderless Image").GetComponent<RectTransform>();
        _progressImage = GameObject.Find("Bordered Image").GetComponent<Image>();
        _objNameText = GameObject.Find("Object Text").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void Update()
    {
        SelectObject();

        if (HasObjectSelected())
        {
            _holdTime = _objectToInteract.interactTime;

            Debug.Log(_objectToInteract);

            if (_objectToInteract.disabled)
            {
                return;
            }

            _imageRoot.gameObject.SetActive(true);

            Press(_objectToInteract.interactType);

            UpdateProgressImage(_currentInteractTimerElapsed);
        } else
        {
            _imageRoot.gameObject.SetActive(false);
        }
    }

    private void SelectObject()
    {
        Ray ray = _camera.ViewportPointToRay(Vector3.one / 2f);
        Debug.DrawRay(ray.origin, ray.direction / 2f, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _interactDistance, _layerMask))
        {
            var hitObj = hit.collider.GetComponentInParent<Interactable>();

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

        if (_currentInteractTimerElapsed >= _holdTime && !_interacted)
        {
            _objectToInteract.Interact();
            _interacted = true;
            if (_objectToInteract.oneTimeUse)
            {
                _objectToInteract.disabled = true;
            }
        }
    }

    private void Press(Interactable.Options option)
    {
        switch (option){
            case Interactable.Options.Pressed:
                _currentInteractTimerElapsed = 0f;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    _objectToInteract.Interact();
                }
                break;

            case Interactable.Options.Held:
                if (Input.GetKey(KeyCode.E))
                {
                    IncrementTimer();
                }
                else
                {
                    _currentInteractTimerElapsed = 0f;
                    _interacted = false;
                }
                break;

            default:
                break;
        }
    }

    private void UpdateProgressImage(float time)
    {
        float progress = time / _holdTime;
        _progressImage.fillAmount = progress;
    }


}
