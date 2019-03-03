using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CheckSwipe : MonoBehaviour {
	
	private Touch _myTouch;
	private Vector2 _startTouchPosition;
	private Vector2 _finalTouchPosition;
	[FormerlySerializedAs("swipeDirection")] private Vector2 _swipeDirection;

	private bool _shouldCallFunc;
	private float _timeStartedTouching;

	private void Start()
	{
		_shouldCallFunc = false;
		_swipeDirection = Vector2.zero;
	}

	// Update is called once per frame
	private void Update () {
		if (Input.touchCount <= 0) return;
		_myTouch = Input.GetTouch(0);
		switch (_myTouch.phase)
		{
			case TouchPhase.Began:
				_timeStartedTouching = Time.time;
				_startTouchPosition = _myTouch.position;
				break;
			case TouchPhase.Ended:
				_finalTouchPosition = _myTouch.position;
				
				// checking the delta
				var direction = _finalTouchPosition - _startTouchPosition;
				if (direction.magnitude > 60)
				{
					_shouldCallFunc = true;
				}
				if (direction.x > 0)
				{
					_swipeDirection = Vector2.right;
				}
				else if (direction.x < 0)
				{
					_swipeDirection = Vector2.left;
				}

				break;
			case TouchPhase.Moved:
				break;
			case TouchPhase.Stationary:
				if (Time.time - _timeStartedTouching >= 1.5f && Input.touchCount == 1)
				{
					GetComponent<UpgradesSceneController>().ResetUpgradeLevels();
					GetComponent<UpgradesSceneController>().ResetHits();
				}
				break;
			case TouchPhase.Canceled:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		if (Input.touchCount >= 3)
		{
			GetComponent<UpgradesSceneController>().GetHits();
		}
		
		if (!_shouldCallFunc) return;
		GetComponent<UpgradesSceneController>().ChangeUpgradeOnScreen(_swipeDirection);
		_shouldCallFunc = false;
	}
		
		
}
