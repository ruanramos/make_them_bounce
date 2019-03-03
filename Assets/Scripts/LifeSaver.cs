using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class LifeSaver : MonoBehaviour
{

	private GameObject _watcher;
	
	// Use this for initialization
	private void Start () {
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<BoxCollider2D>().enabled = false;
		_watcher = GameObject.Find("Watcher");
	}
	
	// Update is called once per frame
	private void Update ()
	{
		var life = _watcher.GetComponent<GlobalInfos>().LifeSaverLife;
		if (life == 0)
		{
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<BoxCollider2D>().enabled = false;
		}
		else
		{
			GetComponent<SpriteRenderer>().enabled = true;
			GetComponent<BoxCollider2D>().enabled = true;	
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.gameObject.CompareTag("Ball")) return;
		_watcher.GetComponent<GlobalInfos>().LifeSaverLife--;
		StartCoroutine(OneWay());
		//other.gameObject.GetComponent<Circle>().ChargeUp();
		
	}

	private IEnumerator OneWay()
	{
		foreach (var i in FindObjectsOfType<PlatformEffector2D>())
		{
			i.useOneWay = true;
		}
		yield return new WaitForSeconds(1f);
		foreach (var i in FindObjectsOfType<PlatformEffector2D>())
		{
			i.useOneWay = false;
		}
	}

}
