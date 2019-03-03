using System.Collections;
using UnityEngine;
using EZCameraShake;
using UnityEngine.Serialization;

public class Circle : MonoBehaviour {
    private GlobalInfos _watcherGlobalInfos;
    private VanishBalls _watcherVanishBalls;
    [FormerlySerializedAs("sprites")] public Sprite[] Sprites = new Sprite[4];

    private bool _isOutOfScreen;
    private bool _hasInstantiatedIndicator;
    private GameObject _indicator;
    private readonly float _indicatorY = 8.11f;

    private int _life = 20;

    // Use this for initialization
    private void Start () {
        _watcherGlobalInfos = GameObject.Find("Watcher").GetComponent<GlobalInfos>();
        _watcherVanishBalls = GameObject.Find("Watcher").GetComponent<VanishBalls>();
        GetComponent<SpriteRenderer>().sprite = Sprites[2];
        GetComponent<SpriteRenderer>().color = new Color(255f / 255, 204f / 255, 204f / 255);
    }

    private void Update()
    {
        if(GetComponent<Rigidbody2D>().velocity.magnitude >= 24)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y) * 0.9f;
        }

        if (transform.position.y > 9f)
        {
            _isOutOfScreen = true;
        }
        else
        {
            _isOutOfScreen = false;
        }

        if (_isOutOfScreen)
        {
            if (_hasInstantiatedIndicator)
            {
                _indicator.transform.position = new Vector2(transform.position.x, _indicatorY);
            }
            else
            {
                _indicator = Instantiate(Resources.Load("prefabSeta"), new Vector2(transform.position.x, _indicatorY), transform.rotation) as GameObject;
                _hasInstantiatedIndicator = true;    
            }
        }
        else
        {
            Destroy(_indicator);
            _hasInstantiatedIndicator = false;
        }

        if (_life <= 0)
        {
            BallDeath();
            Debug.Log("MORREU UMA BOLA: " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "End")
        {
            StartCoroutine(_watcherGlobalInfos.GameOverFunc());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _life--;
        
        if (other.gameObject.name == "prefab" || other.gameObject.name == "prefab 1(Clone)" || other.gameObject.CompareTag("Wall"))
        {
            StartCoroutine(ChangeSpriteToWhat());
            _watcherGlobalInfos.NumberOfHits++;
        }

        else if (other.gameObject.CompareTag("Ball"))
        {
            _watcherGlobalInfos.NumberOfHits++;
            _watcherGlobalInfos.NumberOfHits -= 1;
            StartCoroutine(ChangeSpriteToHappy());
        }
        
        else if (other.gameObject.CompareTag("Wall"))
        {
            //ChargeUpWall();
        }
    }

    private IEnumerator ChangeSpriteToWhat()
    {
        GetComponent<SpriteRenderer>().sprite = Sprites[1];
        GetComponent<SpriteRenderer>().color = new Color(212f/255, 235f / 255, 242f / 255);
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().sprite = Sprites[2];
        GetComponent<SpriteRenderer>().color = new Color(255f / 255, 204f / 255, 204f / 255);
    }

    private IEnumerator ChangeSpriteToHappy()
    {
        GetComponent<SpriteRenderer>().sprite = Sprites[0];
        GetComponent<SpriteRenderer>().color = new Color(255f / 255, 255f / 255, 204f / 255);
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().sprite = Sprites[2];
        GetComponent<SpriteRenderer>().color = new Color(255f / 255, 204f / 255, 204f / 255);
    }

    public void ChargeUp()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        float rand = Random.Range(-700, 700);
        CameraShaker.Instance.ShakeOnce(3f, 3f, .1f, 1f);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(rand, 900));
    }
    
    public void ChargeUpWall()
    {
        float rand = Random.Range(-400, 700);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(rand, 1100));
    }

    private void BallDeath()
    {
        GetComponentInChildren<ParticleSystem>().Play();
        Destroy(gameObject, 0.3f);
    }
}
