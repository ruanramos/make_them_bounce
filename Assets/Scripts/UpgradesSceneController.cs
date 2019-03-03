using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradesSceneController : MonoBehaviour
{
	private Text _numberText;
	private Text _totalHitsText;
	private int _selectedNumber;

	private GlobalInfos _watcherGlobalInfosScript;
	
	[FormerlySerializedAs("upgradesImage")] public Sprite[] UpgradesImage;
	
	private readonly int[] _chargeUpUpgradeCosts = {500, 1000, 1500, 2000, 3000, 10000};
	private int _upgradeChargeUpLevel;
	private int _upgradeChargeRatioSpeedLevel;
	private int _upgradeTimeToStartVanishingLevel;
	private int _upgradeLifeSaverLevel;
	private int _upgradeVanishWithBallsLevel;

	// Use this for initialization
	private void Start ()
	{
		_numberText = GameObject.Find("Number").GetComponent<Text>();
		_selectedNumber = 1;
		_totalHitsText = GameObject.Find("TotalHits").GetComponent<Text>();
		_watcherGlobalInfosScript = GameObject.Find("Watcher").GetComponent<GlobalInfos>();
	}
	
	// Update is called once per frame
	public void Update ()
	{
		// check witch update to show on screen
		switch (_selectedNumber)
		{
			case 1:
				// charge ratio speed upgrade
				_upgradeChargeRatioSpeedLevel = PlayerPrefs.GetInt("ChargeRatioUpgradeLevel", 0);

				GameObject.Find("Explanation").GetComponent<Text>().text = "Each level increases the ratio your charge to draw lines increase";
				GameObject.Find("Name").GetComponent<Text>().text = "DRAW MORE LINES!";
				
				if (_upgradeChargeRatioSpeedLevel != 5)
				{
					GameObject.Find("Cost").GetComponent<Text>().text = "Cost: " + _chargeUpUpgradeCosts[_upgradeChargeRatioSpeedLevel] + " Hits";
					MakeButtonInteractable();
				}
				else
				{
					GameObject.Find("Cost").GetComponent<Text>().text = "MAX LEVEL";
					MakeButtonNonInteractable();
				}
				
				GameObject.Find("Upgrade").GetComponent<Image>().sprite = UpgradesImage[_upgradeChargeRatioSpeedLevel];
				break;
			case 2:
				// time lines start vanishing upgrade
				_upgradeTimeToStartVanishingLevel = PlayerPrefs.GetInt("TimeToLineStartVanishingUpgradeLevel", 0);

				GameObject.Find("Explanation").GetComponent<Text>().text = "Each level increases the time before your lines vanish";
				GameObject.Find("Name").GetComponent<Text>().text = "REFUSE TO GO!";
				
				if (_upgradeTimeToStartVanishingLevel != 5)
				{
					GameObject.Find("Cost").GetComponent<Text>().text = "Cost: " + _chargeUpUpgradeCosts[_upgradeTimeToStartVanishingLevel] + " Hits";
					MakeButtonInteractable();
				}
				else
				{
					GameObject.Find("Cost").GetComponent<Text>().text = "MAX LEVEL";
					MakeButtonNonInteractable();
				}
				
				GameObject.Find("Upgrade").GetComponent<Image>().sprite = UpgradesImage[_upgradeTimeToStartVanishingLevel];
				break;
			case 3:
				// life saver upgrade
				_upgradeLifeSaverLevel = PlayerPrefs.GetInt("LifeSaverUpgradeLevel", 0);

				GameObject.Find("Explanation").GetComponent<Text>().text = "Each level gives you a one time chance to lose";
				GameObject.Find("Name").GetComponent<Text>().text = "LIFE SAVER!";
				
				if (_upgradeLifeSaverLevel != 5)
				{
					GameObject.Find("Cost").GetComponent<Text>().text = "Cost: " + _chargeUpUpgradeCosts[_upgradeLifeSaverLevel] + " Hits";
					MakeButtonInteractable();
				}
				else
				{
					GameObject.Find("Cost").GetComponent<Text>().text = "MAX LEVEL";
					MakeButtonNonInteractable();
				}
				
				GameObject.Find("Upgrade").GetComponent<Image>().sprite = UpgradesImage[_upgradeLifeSaverLevel];
				break;
			case 4:
				// chargeUp upgrade
				_upgradeChargeUpLevel = PlayerPrefs.GetInt("ChargeUpgradeLevel", 0);
				
				GameObject.Find("Explanation").GetComponent<Text>().text =
					"Hold your touch without moving your finger to charge your power and send all the balls flying. Each level reduces cooldown";
				GameObject.Find("Name").GetComponent<Text>().text = "CHARGE UP!";
				if (_upgradeChargeUpLevel != 5)
				{
					GameObject.Find("Cost").GetComponent<Text>().text = "Cost: " + _chargeUpUpgradeCosts[_upgradeChargeUpLevel] + " Hits";
					MakeButtonInteractable();
				}
				else
				{
					GameObject.Find("Cost").GetComponent<Text>().text = "MAX LEVEL";
					MakeButtonNonInteractable();
				}
				
				GameObject.Find("Upgrade").GetComponent<Image>().sprite = UpgradesImage[_upgradeChargeUpLevel];
				break;
			case 5:
				// delete balls upgrade
				_upgradeVanishWithBallsLevel = PlayerPrefs.GetInt("VanishBallUpgradeLevel", 0);
				
				GameObject.Find("Explanation").GetComponent<Text>().text =
					"A ball will disappear every 30 seconds at level 1 of the upgrade. Increasing levels makes it happen faster";
				GameObject.Find("Name").GetComponent<Text>().text = "WHERE IS IT?";
				if (_upgradeVanishWithBallsLevel != 5)
				{
					GameObject.Find("Cost").GetComponent<Text>().text = "Cost: " + _chargeUpUpgradeCosts[_upgradeVanishWithBallsLevel] + " Hits";
					MakeButtonInteractable();
				}
				else
				{
					GameObject.Find("Cost").GetComponent<Text>().text = "MAX LEVEL";
					MakeButtonNonInteractable();
				}
				
				GameObject.Find("Upgrade").GetComponent<Image>().sprite = UpgradesImage[_upgradeVanishWithBallsLevel];
				break;
		}

		_numberText.text = _selectedNumber + " / " + _watcherGlobalInfosScript.NumberOfUpgrades;
		_totalHitsText.text = "Total Hits: " + PlayerPrefs.GetInt("TotalHits", 0);
	}

	public void ResetUpgradeLevels()
	{
		PlayerPrefs.SetInt("ChargeUpgradeLevel", 0);
		PlayerPrefs.SetInt("ChargeRatioUpgradeLevel", 0);
		PlayerPrefs.SetInt("TimeToLineStartVanishingUpgradeLevel", 0);
		PlayerPrefs.SetInt("LifeSaverUpgradeLevel", 0);
		PlayerPrefs.SetInt("VanishBallUpgradeLevel", 0);
	}

	public void GetHits()
	{
		PlayerPrefs.SetInt("TotalHits", PlayerPrefs.GetInt("TotalHits") + 100);
	}
	
	public void ResetHits()
	{
		PlayerPrefs.SetInt("TotalHits", 0);
	}

	private void MakeButtonNonInteractable()
	{
		GameObject.Find("UpgradeButton").GetComponent<Button>().interactable = false;
	}
	
	private void MakeButtonInteractable()
	{
		GameObject.Find("UpgradeButton").GetComponent<Button>().interactable = true;
	}

	public void ChangeUpgradeOnScreen(Vector2 direction)
	{
		if (direction == Vector2.right)
		{
			if (_selectedNumber > 1)
			{
				_selectedNumber--;				
			}
			else
			{
				_selectedNumber = _watcherGlobalInfosScript.NumberOfUpgrades;
			}
			
		}
		else if (direction == Vector2.left)
		{
			if (_selectedNumber < _watcherGlobalInfosScript.NumberOfUpgrades)
			{
				_selectedNumber++;				
			}
			else
			{
				_selectedNumber = 1;
			}
		}
	}

	private void UpgradeLevel(int numberOfUpgrade)
	{
		var totalHits = PlayerPrefs.GetInt("TotalHits", 0);
		int cost;
		switch (numberOfUpgrade)
		{
				case 1:
					cost = _chargeUpUpgradeCosts[PlayerPrefs.GetInt("ChargeRatioUpgradeLevel", 0)];
					_upgradeChargeRatioSpeedLevel = PlayerPrefs.GetInt("ChargeRatioUpgradeLevel", 0);
					if (_upgradeChargeRatioSpeedLevel < 5 && totalHits >= cost)
					{
						PlayerPrefs.SetInt("ChargeRatioUpgradeLevel", _upgradeChargeRatioSpeedLevel + 1);
						PlayerPrefs.SetInt("TotalHits", totalHits - cost);
					}
					break;
				case 2:
					cost = _chargeUpUpgradeCosts[PlayerPrefs.GetInt("TimeToLineStartVanishingUpgradeLevel", 0)];
					_upgradeTimeToStartVanishingLevel = PlayerPrefs.GetInt("TimeToLineStartVanishingUpgradeLevel", 0);
					if (_upgradeTimeToStartVanishingLevel < 5 && totalHits >= cost)
					{
						PlayerPrefs.SetInt("TimeToLineStartVanishingUpgradeLevel", _upgradeTimeToStartVanishingLevel + 1);
						PlayerPrefs.SetInt("TotalHits", totalHits - cost);
					}
					break;
				case 3:
					cost = _chargeUpUpgradeCosts[PlayerPrefs.GetInt("LifeSaverUpgradeLevel", 0)];
					_upgradeLifeSaverLevel = PlayerPrefs.GetInt("LifeSaverUpgradeLevel", 0);
					if (_upgradeLifeSaverLevel < 5 && totalHits >= cost)
					{
						PlayerPrefs.SetInt("LifeSaverUpgradeLevel", _upgradeLifeSaverLevel + 1);
						PlayerPrefs.SetInt("TotalHits", totalHits - cost);
					}
					break;
				case 4:
					cost = _chargeUpUpgradeCosts[PlayerPrefs.GetInt("ChargeUpgradeLevel", 0)];
					_upgradeChargeUpLevel = PlayerPrefs.GetInt("ChargeUpgradeLevel", 0);
					if (_upgradeChargeUpLevel < 5 && totalHits >= cost)
					{
						PlayerPrefs.SetInt("ChargeUpgradeLevel", _upgradeChargeUpLevel + 1);
						PlayerPrefs.SetInt("TotalHits", totalHits - cost);
					}
					break;
			case 5:
				cost = _chargeUpUpgradeCosts[PlayerPrefs.GetInt("VanishBallUpgradeLevel", 0)];
				_upgradeVanishWithBallsLevel = PlayerPrefs.GetInt("VanishBallUpgradeLevel", 0);
				if (_upgradeVanishWithBallsLevel < 5 && totalHits >= cost)
				{
					PlayerPrefs.SetInt("VanishBallUpgradeLevel", _upgradeVanishWithBallsLevel + 1);
					PlayerPrefs.SetInt("TotalHits", totalHits - cost);
				}
				break;
		}
	}

	public void UpgradeButton()
	{
		var number = _selectedNumber;
		UpgradeLevel(number);
	}
}
