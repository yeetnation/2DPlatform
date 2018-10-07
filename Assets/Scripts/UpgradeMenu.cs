using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour {

    
    private PlayerStats stats;
    private StatusIndicator statusIndicator;
    [SerializeField] public Weapon pistol;
    [SerializeField] public Weapon rifle;
    [Header("Player")]
    [Header("Health")]
    [SerializeField] private Text healthText;
    [SerializeField] private Text healthCostText;
    [SerializeField] private int healthUpgradeCost = 50;
    [SerializeField] private int healthAddition = 20;

    [Header("Regeneration")]
    [SerializeField] private Text regenText;
    [SerializeField] private Text regenCostText;
    [SerializeField] private int regenUpgradeCost = 50;
    [SerializeField] private int regenAddition = 1;

    [Header("Pistol")]
    [Header("Damage")]
    [SerializeField] private Text pistolDamageText;
    [SerializeField] private Text pistolCostText;
    [SerializeField] private int pistolUpgradeCost = 50;
    [SerializeField] private int pistolDamageAddition = 10;

    [Header("Rifle")]
    [Header("Damage")]
    [SerializeField] private Text rifleDamageText;
    [SerializeField] private Text rifleDamageCostText;
    [SerializeField] private int rifleDamageUpgradeCost = 50;
    [SerializeField] private int rifleDamageAddition = 10;
    [Header("Firerate")]
    [SerializeField] private Text rifleFireRateText;
    [SerializeField] private Text rifleFireRateCostText;
    [SerializeField] private int rifleFireRateUpgradeCost = 50;
    [SerializeField] private int rifleFireRateAddition = 10;
    private void OnEnable()
    {
        stats = PlayerStats.instance;
        statusIndicator = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<StatusIndicator>();
        pistol = GameObject.Find("Arm").transform.GetChild(0).GetComponent<Weapon>();
        rifle = GameObject.Find("Arm").transform.GetChild(1).GetComponent<Weapon>();
        UpdateValues();
    }
    void UpdateValues()
    {
        healthText.text = "Health: "+ stats.maxHealth.ToString();
        healthCostText.text = "Cost: " + healthUpgradeCost.ToString();

        regenText.text = "Regeneration: " + stats.healthRegenRate.ToString();
        regenCostText.text = "Cost: " + regenUpgradeCost.ToString();

        pistolDamageText.text = "Damage: " + pistol.damage.ToString();
        pistolCostText.text = "Cost: " + pistolUpgradeCost.ToString();

        rifleDamageText.text = "Damage: " + rifle.damage.ToString();
        rifleDamageCostText.text = "Cost: " + rifleDamageUpgradeCost.ToString();

        rifleFireRateText.text = "Firerate: " + rifle.fireRate.ToString() + "/s";
        rifleFireRateCostText.text = "Cost: " + rifleFireRateUpgradeCost.ToString();
    }
    public void UpgradeHealth()
    {
        if (GameMaster.Money < healthUpgradeCost)
        {
            SoundManager.instance.PlaySound("NoMoney");
            return;
        }
        stats.maxHealth += healthAddition;
        stats.curHealth += healthAddition;
        GameMaster.Money -= healthUpgradeCost;
        SoundManager.instance.PlaySound("Money");
        UpdateValues();
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }
    public void UpgradeRegen()
    {
        if (GameMaster.Money < regenUpgradeCost)
        {
            SoundManager.instance.PlaySound("NoMoney");
            return;
        }
        stats.healthRegenRate += regenAddition;
        GameMaster.Money -= regenUpgradeCost;
        SoundManager.instance.PlaySound("Money");
        UpdateValues();
    }
    public void UpgradePistolDamage()
    {
        if (GameMaster.Money < pistolUpgradeCost)
        {
            SoundManager.instance.PlaySound("NoMoney");
            return;
        }
        pistol.damage += pistolDamageAddition;
        GameMaster.Money -= pistolUpgradeCost;
        SoundManager.instance.PlaySound("Money");
        UpdateValues();
    }
    public void UpgradeRifleDamage()
    {
        if (GameMaster.Money < rifleDamageUpgradeCost)
        {
            SoundManager.instance.PlaySound("NoMoney");
            return;
        }
        rifle.damage += rifleDamageAddition;
        GameMaster.Money -= rifleDamageUpgradeCost;
        SoundManager.instance.PlaySound("Money");
        UpdateValues();
    }
    public void UpgradeRifleFireRate()
    {
        if (GameMaster.Money < rifleFireRateUpgradeCost)
        {
            SoundManager.instance.PlaySound("NoMoney");
            return;
        }
        rifle.fireRate += rifleFireRateAddition;
        GameMaster.Money -= rifleFireRateUpgradeCost;
        SoundManager.instance.PlaySound("Money");
        UpdateValues();
    }
}
