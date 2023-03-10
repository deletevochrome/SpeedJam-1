using UnityEngine;
using UnityEngine.UI;
using System;
using CodeMonkey.Utils;
public class _WindowPointer : MonoBehaviour
{

    [SerializeField] private Camera uiCamera;
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite crossSprite;
    [SerializeField] private float _additionalAngle = 45;
    private Vector3 targetPosition;
    private RectTransform pointerRectTransform;
    private Image pointerImage;
    [SerializeField] private ArrowAnimation _arrowAnimation;
    public event Action OnLeftSceen;
    public event Action OnGetIntoScreen;
    private bool _isOffScreen;
    private void Awake()
    {
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
        pointerImage = transform.Find("Pointer").GetComponent<Image>();

        Hide();
    }

    private void Update()
    {

        float BorderSize = 45f;
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool isOffScreen = targetPositionScreenPoint.x <= BorderSize || targetPositionScreenPoint.x >= Screen.width - BorderSize || targetPositionScreenPoint.y <= BorderSize || targetPositionScreenPoint.y >= Screen.height - BorderSize;
        if (isOffScreen != _isOffScreen)
        {
            if (isOffScreen)
            {
                _arrowAnimation.PlayArrow();
            }
            else
            {
                _arrowAnimation.PlayCross();
            }
        }
        if (isOffScreen)
        {
            RotateToPointerTargetPosition();
            // pointerImage.sprite = arrowSprite;
            Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
            if (cappedTargetScreenPosition.x <= BorderSize) cappedTargetScreenPosition.x = BorderSize;
            if (cappedTargetScreenPosition.x >= Screen.width - BorderSize) cappedTargetScreenPosition.x = Screen.width - BorderSize;
            if (cappedTargetScreenPosition.y <= BorderSize) cappedTargetScreenPosition.y = BorderSize;
            if (cappedTargetScreenPosition.y >= Screen.height - BorderSize) cappedTargetScreenPosition.y = Screen.height - BorderSize;

            Vector3 pointerInWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
            pointerRectTransform.position = pointerInWorldPosition;
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
        }
        else
        {
            //  pointerImage.sprite = crossSprite;
            Vector3 pointerInWorldPosition = uiCamera.ScreenToWorldPoint(targetPositionScreenPoint);
            pointerRectTransform.position = pointerInWorldPosition;
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
            pointerRectTransform.localEulerAngles = Vector3.zero;
        }
        _isOffScreen = isOffScreen;
    }
    private void RotateToPointerTargetPosition()
    {
        Vector3 toPosition = targetPosition;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = UtilsClass.GetAngleFromVectorFloat(dir);
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle + _additionalAngle);
    }

    public void Hide()
    {
        Debug.Log("HIDE!!");
        gameObject.SetActive(false);
    }

    public void Show(Vector3 targetPosition)
    {
        gameObject.SetActive(true);
        this.targetPosition = targetPosition;
    }
}
