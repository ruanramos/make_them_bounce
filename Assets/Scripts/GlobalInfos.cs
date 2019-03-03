using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GlobalInfos : MonoBehaviour {

    [FormerlySerializedAs("Magic")] [FormerlySerializedAs("magic")] public float Charge;
    private float _chargeGainMultiplier;
    private Image _magicImage;
    [FormerlySerializedAs("timeToStartVanishing")] public float TimeToStartVanishing;
    [FormerlySerializedAs("timeBetweenPointsVanishing")] public float TimeBetweenPointsVanishing;

    [FormerlySerializedAs("_lifeSaverLife")] public int LifeSaverLife;
    
    private GameObject _hitsScoreText;
    private GameObject _timeText;

    private float _sessionTime;
    private static float _highscoreTime;

    private GameObject _panel;
    private GameObject _gameOverText;

    [FormerlySerializedAs("gameOver")] public bool GameOver;
    private Spawner _spawner;

    private GameObject _playAgainButton;
    private GameObject _playAgainButtonText;
    private GameObject _mainMenuButton;
    private GameObject _mainMenuButtonText;
    private GameObject _upgradesButton;
    private GameObject _upgradesButtonText;
    
    [FormerlySerializedAs("numberOfHits")] public int NumberOfHits;
    private int _totalHits;
    
    // upgrades
    //TODO implementar os outros dois upgrades que estão faltando aqui
    private int _chargeUpUpgradeLevel;
    private int _lifeSaverUpgradeLevel;
    private int _timeToLineStartVanishingUpgradeLevel;
    //private int _lifeSaverUpgradeLevel;
    private int _vanishWithBallUpgradeLevel;
    private int _chargeRatioUpgradeLevel;

    [FormerlySerializedAs("_numberOfUpgrades")] public int NumberOfUpgrades;
    [FormerlySerializedAs("_numberOfLevelsOfUpgrades")] public int NumberOfLevelsOfUpgrades = 5;
    [FormerlySerializedAs("upgradeCharge")] public float UpgradeCharge;
    public float ChargeUpCooldown;
    public bool ChargeUpOnCooldown;
    public float TimeLastChargeUpHappened;
    [FormerlySerializedAs("ChargeUpImage")] public Image ChargeUpImageRed;
    public Image ChargeUpImageBlue;

    public float VanishWithBallUpgradeCooldown;

    // Use this for initialization
    private void Start ()
    {
        ChargeUpOnCooldown = false;
        NumberOfUpgrades = 5;
        // getting upgrades levels and setting the values for each one
        _chargeUpUpgradeLevel = PlayerPrefs.GetInt("ChargeUpgradeLevel", 0);
        _lifeSaverUpgradeLevel = PlayerPrefs.GetInt("LifeSaverUpgradeLevel", 0);
        _timeToLineStartVanishingUpgradeLevel = PlayerPrefs.GetInt("TimeToLineStartVanishingUpgradeLevel", 0);
        _vanishWithBallUpgradeLevel = PlayerPrefs.GetInt("VanishBallUpgradeLevel", 0);
        _chargeRatioUpgradeLevel = PlayerPrefs.GetInt("ChargeRatioUpgradeLevel", 0);

        _totalHits = PlayerPrefs.GetInt("TotalHits", 0);

        if (SceneManager.GetActiveScene().name != "scene")
        {
            return;
        }
        
        switch (_vanishWithBallUpgradeLevel)
        {
            case 1:
                VanishWithBallUpgradeCooldown = 25f;
                break;
            case 2:
                VanishWithBallUpgradeCooldown = 20f;
                break;
            case 3:
                VanishWithBallUpgradeCooldown = 15f;
                break;
            case 4:
                VanishWithBallUpgradeCooldown = 10f;
                break;
            case 5:
                VanishWithBallUpgradeCooldown = 5f;
                break;
            default:
                VanishWithBallUpgradeCooldown = 0f;
                break;
                
        }
        
        switch (_chargeUpUpgradeLevel)
        {
            case 1:
                ChargeUpCooldown = 9;
                break;
            case 2:
                ChargeUpCooldown = 8;
                break;
            case 3:
                ChargeUpCooldown = 7;
                break;
            case 4:
                ChargeUpCooldown = 6;
                break;
            case 5:
                ChargeUpCooldown = 5;
                break;
            default:
                ChargeUpCooldown = 10;
                break;
                
        }
            
        switch (_lifeSaverUpgradeLevel)
        {
            case 1:
                LifeSaverLife = 1;
                break;
            case 2:
                LifeSaverLife = 2;
                break;
            case 3:
                LifeSaverLife = 3;
                break;
            case 4:
                LifeSaverLife = 4;
                break;
            case 5:
                LifeSaverLife = 5;
                break;
            default:
                LifeSaverLife = 0;
                break;
        }
        
        switch (_timeToLineStartVanishingUpgradeLevel)
        {
            case 0:
                TimeToStartVanishing = 2.3f;
                break;
            case 1:
                TimeToStartVanishing = 2.6f;
                break;
            case 2:
                TimeToStartVanishing = 3.3f;
                break;
            case 3:
                TimeToStartVanishing = 3.4f;
                break;
            case 4:
                TimeBetweenPointsVanishing = 4.0f;
                break;
            default:
                TimeToStartVanishing = 4.5f;
                break;
        }
        
        switch (_chargeRatioUpgradeLevel)
        {
            case 1:
                _chargeGainMultiplier = 35f;
                break;
            case 2:
                _chargeGainMultiplier = 45f;
                break;
            case 3:
                _chargeGainMultiplier = 55f;
                break;
            case 4:
                _chargeGainMultiplier = 65f;
                break;
            case 5:
                _chargeGainMultiplier = 80f;
                break;
            default:
                _chargeGainMultiplier = 25f;
                break;
        }
        
        _sessionTime = 0;
        Charge = 100;
        ChargeUpImageRed = GameObject.Find("ChargeUpImageRed").GetComponent<Image>();
        ChargeUpImageBlue = GameObject.Find("ChargeUpImageBlue").GetComponent<Image>();
        _magicImage = GameObject.Find("Magic").GetComponent<Image>();
        
        _hitsScoreText = GameObject.Find("ScoreText");
        _timeText = GameObject.Find("TimeText");
    
        _panel = GameObject.Find("Panel");
        _gameOverText = GameObject.Find("GameOverText");
        _spawner = GameObject.Find("Watcher").GetComponent<Spawner>();
    
        _playAgainButton = GameObject.Find("PlayAgainButton");
        _playAgainButtonText = GameObject.Find("Text");
        _mainMenuButton = GameObject.Find("MainMenuButton");
        _mainMenuButtonText = GameObject.Find("Text1");
        _upgradesButton = GameObject.Find("UpgradesButton");
        _upgradesButtonText = GameObject.Find("Text2");
        _playAgainButton.SetActive(false);
        _mainMenuButton.SetActive(false);
        _upgradesButton.SetActive(false);
            
        UpgradeCharge = 0;
    }
	
	// Update is called once per frame
    private void Update () {
        
        if (SceneManager.GetActiveScene().name != "scene") return;
        
        // calculate magic fill amount and adds magic amount
        _magicImage.fillAmount = Charge/100;
        if(Charge <= 100)
        {
            Charge += _chargeGainMultiplier * Time.deltaTime;
        }

        // calculate session time
        _sessionTime += Time.deltaTime;

        if (GameOver) return;
        _hitsScoreText.GetComponent<Text>().text = "Hits: " + NumberOfHits;
        _timeText.GetComponent<Text>().text = "Time: " + _sessionTime.ToString("F2");
    }

    public IEnumerator GameOverFunc()
    {
        GameOver = true;
        
        // updates highscore or not
        if (PlayerPrefs.GetFloat("Highscore", 0) < _sessionTime)
        {
            PlayerPrefs.SetFloat("Highscore", _sessionTime);
        }
        

        _totalHits += NumberOfHits;
        PlayerPrefs.SetInt("TotalHits", _totalHits);
        
        var balls = GameObject.FindGameObjectsWithTag("Ball");
        _spawner.enabled = false;
        _playAgainButton.SetActive(true);
        _mainMenuButton.SetActive(true);
        _upgradesButton.SetActive(true);
        _playAgainButton.GetComponent<Button>().interactable = false;
        _mainMenuButton.GetComponent<Button>().interactable = false;
        _upgradesButton.GetComponent<Button>().interactable = false;

        // turning off ball collider
        foreach (var ball in balls)
        {
            ball.GetComponent<CircleCollider2D>().enabled = false;
            ball.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

        // slow fade out of the screen
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            _playAgainButton.GetComponent<Image>().color = new Color(1, 1, 1, i);
            _mainMenuButton.GetComponent<Image>().color = new Color(1, 1, 1, i);
            _upgradesButton.GetComponent<Image>().color = new Color(1, 1, 1, i);
            _mainMenuButtonText.GetComponent<Text>().color = new Color(0, 0, 0, i);
            _upgradesButtonText.GetComponent<Text>().color = new Color(0, 0, 0, i);
            _playAgainButtonText.GetComponent<Text>().color = new Color(0, 0, 0, i);
            _panel.GetComponent<Image>().color = new Color(0, 0, 0, i);
            _gameOverText.GetComponent<Text>().color = new Color(1, 1, 1, i);
            yield return new WaitForEndOfFrame();
        }
        _playAgainButton.GetComponent<Button>().interactable = true;
        _mainMenuButton.GetComponent<Button>().interactable = true;
        _upgradesButton.GetComponent<Button>().interactable = true;

        _gameOverText.GetComponent<Text>().text = "Game Over\n\nHighscore: " + PlayerPrefs.GetFloat("Highscore").ToString("F2") + "s\nTotal Hits: " + PlayerPrefs.GetInt("TotalHits", 0);
    }
}
