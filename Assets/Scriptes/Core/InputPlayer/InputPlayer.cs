using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace InputsPlayer.Drag
{
    public class InputPlayer 
    {
        public enum StateDirection
        {
            Up = 0, 
            Down = 1,
            Left = 2,
            Right = 3,
            None
        }

        private UnityAction<Vector3> OnGetRay;

        private Vector3 _startPosition;

        private Camera _camera;
        private Transform _target;
        private float _distanceToTarget = 10;

        private StateDirection _stateDirection;

        public Transform Target { get => _target; set => _target = value; }

        public InputPlayer(IGetRay getRay, Camera camera, Transform target, float distanceToTarget)
        {
            OnGetRay = getRay.GetRay;
            _camera = camera;
            _target = target;
            _distanceToTarget = distanceToTarget;
        }

        public void OnInput()
        {
            int countTouchs = Input.touchCount;
            if (countTouchs > 0)
            {
                if (countTouchs < 2) // one finger
                {
                    OnRotate();
                }
                else 
                if(countTouchs >= 2) // multi
                {
                    OnChangeDistance(countTouchs);
                }
            }    
        }

        private void OnRotate()
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startPosition = _camera.ScreenToViewportPoint(touch.position);
                    _stateDirection = StateDirection.None;
                    break;
                case TouchPhase.Moved:
                    Vector3 currentPosition = _camera.ScreenToViewportPoint(touch.position);
                    //Vector3 direction = _startPosition - currentPosition;

                    //float rotationAroundYAxis = -direction.x * 110;
                    //float rotationAroundXAxis = direction.y * 110;

                    //_camera.transform.position = _target.position;
                    _stateDirection = GetDirection(currentPosition);
                    //switch (_stateDirection)
                    //{
                    //    case StateDirection.Up:
                    //        if (_camera.transform.eulerAngles.x <= 89)
                    //        {
                    //            _camera.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
                    //        }
                    //        break;
                    //    case StateDirection.Down:
                    //        if (_camera.transform.eulerAngles.x >= 10)
                    //            _camera.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
                    //        break;
                    //    case StateDirection.Left:
                    //        if (_camera.transform.eulerAngles.y <=180)
                    //        {
                    //            _camera.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);
                    //        }
                    //        break;
                    //    case StateDirection.Right:
                    //        if (_camera.transform.eulerAngles.y >= 90)
                    //        {
                    //            _camera.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);
                    //        }
                    //        break;
                    //    case StateDirection.None:

                    //        break;
                    //}

                    //_camera.transform.Translate(new Vector3(0, 0, -_distanceToTarget));

                    //_startPosition = currentPosition;
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    if(_stateDirection == StateDirection.None)
                    OnGetRay(touch.position);
                    break;
                case TouchPhase.Canceled:
                    break;
            }
        }

        private StateDirection GetDirection(Vector3 currentPosition)
        {
            var x = Mathf.Abs(currentPosition.x - _startPosition.x);
            var y = Mathf.Abs(currentPosition.y - _startPosition.y);
            
            if ((currentPosition - _startPosition).magnitude > .004f)
            {
                if (x > y) // horizontal
                {
                    return OnGetDirection(_startPosition.x, currentPosition.x) ? StateDirection.Right : StateDirection.Left;
                }
                if (x < y) // vertical
                {
                    return OnGetDirection(_startPosition.y, currentPosition.y) ? StateDirection.Up : StateDirection.Down;
                }
            }

            return StateDirection.None;
        }

        private bool OnGetDirection(float at, float to)
        {
            return at > to ? true : false;
        }

        private void OnChangeDistance(int count)
        {
            if (count == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                Zoom(difference * .01f);
            }
        }

        private void Zoom(float increment)
        {
            _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView - increment, 30, 60);
        }
    }
}