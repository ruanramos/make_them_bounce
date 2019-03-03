using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishBalls : MonoBehaviour
{
	private GlobalInfos _watcherGlobalInfosScript;
	private float _cooldown;
	private float _timeLastBallRemoved;
	
	// Use this for initialization
	private void Start ()
	{
		_watcherGlobalInfosScript = GameObject.Find("Watcher").GetComponent<GlobalInfos>();
		_cooldown = _watcherGlobalInfosScript.VanishWithBallUpgradeCooldown;
		_timeLastBallRemoved = Time.time;
	}
	
	// Update is called once per frame
	private void Update () {
		if (Time.time - _timeLastBallRemoved >= _cooldown && PlayerPrefs.GetInt("VanishBallUpgradeLevel", 0) != 0)
		{
			RemoveBall();
		}
	}

	public void RemoveBall()
	{
		var balls = GameObject.FindGameObjectsWithTag("Ball");
		var numOfBalls = balls.Length;
		// look for ball in camera
		if (numOfBalls <= 0) return;

		foreach (var ball in balls)
		{
			if (!IsBallOnCamera(ball)) continue;
			ball.GetComponentInChildren<ParticleSystem>().Play();
			Destroy(ball, 0.3f);
			_timeLastBallRemoved = Time.time;
			return;
		}
	}

	private static bool IsBallOnCamera(GameObject ball)
	{
		return ball.transform.position.y > -8.4f && ball.transform.position.y < 8.50f &&
		       ball.transform.position.x > -4.05f && ball.transform.position.x < 4.1f;
	}
}
