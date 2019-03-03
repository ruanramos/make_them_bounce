using UnityEngine;

public class Spawner : MonoBehaviour {
	private float _xMin;
	private float _yMin;
	private float _xMax;
	private float _yMax;

	private float _spawnTime;
	private float _lastSpawnTime;

    // Use this for initialization
	private void Start () {
        // coordinates for spawn
		_xMin = -4.5f;
        _yMin = 9.3f;
        _xMax = 4.5f;
        _yMax = 13f;

		// TODO add dificulties, making balls spawn faster at higher dificulties
        _spawnTime = 2f;
        _lastSpawnTime = Time.time - 5;
	}
	
	// Update is called once per frame
	private void Update () {
		if (!(Time.time - _lastSpawnTime >= _spawnTime)) return;
		var randomX = Random.Range(_xMin, _xMax);
		var randomY = Random.Range(_yMin, _yMax);
		Instantiate(Resources.Load("Circle", typeof(GameObject)), new Vector2(randomX, randomY), transform.rotation);
		_lastSpawnTime = Time.time;
	}
}
