using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class CameraAngleControl : MonoBehaviour
{
    [SerializeField] float moveSpeedX = 250f;
    [SerializeField] float moveSpeedY = 3f;
    [SerializeField] CinemachineFreeLook FreeLookCamera;

    private Touch touch;
    private Vector2 moveValue;
    private Vector2 prePos;
    private Vector2 nowPos;

    private PointerEventData pointerEventData;
    private List<RaycastResult> pointerResults = new List<RaycastResult>();

    private bool isMove = false;


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        isMove = false;

        FreeLookCamera.m_XAxis.m_InputAxisName = string.Empty;
        FreeLookCamera.m_YAxis.m_InputAxisName = string.Empty;
        FreeLookCamera.m_XAxis.m_InputAxisValue = 0;
        FreeLookCamera.m_YAxis.m_InputAxisValue = 0;
        FreeLookCamera.m_XAxis.m_MaxSpeed = 0f;
        FreeLookCamera.m_YAxis.m_MaxSpeed = 0f;
    }

    private void SetMove()
    {
        FreeLookCamera.m_XAxis.m_InputAxisValue = moveValue.x;
        FreeLookCamera.m_YAxis.m_InputAxisValue = moveValue.y;
        FreeLookCamera.m_XAxis.m_MaxSpeed = moveSpeedX;
        FreeLookCamera.m_YAxis.m_MaxSpeed = moveSpeedY;
    }

    private void LateUpdate()
    {
        // 화면을 눌렀을 때 움직임 O (UI 검사)
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        if (Input.GetMouseButtonUp(0))
        {
            Init();
        }
        else
        {
            EditorInputCheck();
        }
#elif UNITY_ANDROID
        if (Input.touchCount <= 0 || Input.touchCount > 2)
        {
            Init();
        }
        else
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                MobileTouchCheck(Input.GetTouch(i));
            }
        }
#endif

    }

    private void EditorInputCheck()
    {
        if (!IsPointerOverUIObject(Input.mousePosition))
        {
            if (Input.GetMouseButtonDown(0))
            {
                isMove = true;
                prePos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0) && isMove)
            {
                nowPos = Input.mousePosition;
                moveValue = prePos - nowPos;
                prePos = Input.mousePosition;
                SetMove();
            }
            else
            {
                Init();
            }
        }
    }

    private void MobileTouchCheck(Touch _touch)
    {
        if (!IsPointerOverUIObject(_touch.position))
        {
            switch (_touch.phase)
            {
                case TouchPhase.Began:      // 손가락이 화면을 터치 시작.
                    isMove = true;
                    prePos = _touch.position - _touch.deltaPosition;
                    break;
                case TouchPhase.Moved:      // 화면에서 손가락이 움직임.
                case TouchPhase.Stationary: // 손가락이 화면을 터치하고 있지만 움직이지 않음.
                    if (isMove)
                    {
                        nowPos = _touch.position - _touch.deltaPosition;
                        moveValue = prePos - nowPos;
                        prePos = _touch.position - _touch.deltaPosition;

                        SetMove();
                    }
                    break;
                case TouchPhase.Ended:      // 화면에서 손가락이 들어 올려짐 터치 끝.
                case TouchPhase.Canceled:   // 시스템이 터치 추적을 취소함.
                default:
                    Init();
                    break;
            }
        }
    }

    /// <summary> UI Event가 들어왔는지 보는 함수 </summary>
    private bool IsPointerOverUIObject(Vector2 touchPos)
    {
        pointerResults.Clear();

        pointerEventData = new PointerEventData(EventSystem.current);

        pointerEventData.position = touchPos;

        EventSystem.current.RaycastAll(pointerEventData, pointerResults);

        return pointerResults.Count > 0;
    }
}
