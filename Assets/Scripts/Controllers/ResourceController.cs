using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class ResourceController : MonoBehaviour, IResourceController
{
    [SerializeField]
    private int startMoneyAmount = 5000;
    [SerializeField]
    private float moneyCalculationInterval = 2; // every 2 second the player income will be calculated
    [SerializeField]
    private int removeCost = 20; // cost of removing an object
    MoneyHelper moneyHelper;

    [SerializeField]
    private int startLevel = 0;
    [SerializeField]
    private int experience = 0;
    [SerializeField]
    private int experienceToNextLevel = 100;
    LevelHelper levelHelper;

    private EnergySystemObjectController purchasingObjectController;
    private ApplianceObjectController applianceObjectController;
    public UIController uIController;
    public TimeController timeController;
    public int StartMoneyAmount { get => startMoneyAmount; }
    public float MoneyCalculationInterval { get => moneyCalculationInterval;}
    int IResourceController.removeCost { get => removeCost; }
    public float DieselCo2Produced { get => dieselCo2Produced; set => dieselCo2Produced = value; }

    private float dieselCo2Produced = 0;

    private string startTime;
    private float currentPoA;
    private float previousPoA; 

    private PowerHelper powerHelper;
    private EmissionHelper emissionHelper;
    public MeterHelper meterHelper;

    //public NotificationManager popupNotification;
    //bool isPopNotiDone = false;

    void Awake()
    {
        timeController.PrepareTimeController();
        startTime = GetTime();
        currentPoA = GetPoA();

    }
        
    void Start()
    {
        moneyHelper = new MoneyHelper(startMoneyAmount);
        levelHelper = new LevelHelper(startLevel,experience, experienceToNextLevel);
        powerHelper = new PowerHelper();
        emissionHelper = new EmissionHelper(powerHelper);
        UpdateMoneyValueUI();
        UpdateExperienceValueUI();
        levelHelper.OnExperienceChanged += LevelHelper_OnExperienceChanged;
        levelHelper.OnLevelChanged += LevelHelper_OnLevelChanged;
        uIController.systemInfoPanelHelper.fuelPurchaseBtn.onClick.AddListener(PurchaseFuel);
        meterHelper.TargetLoadRate = uIController.breakerPanelHelper.Load;
    }


    public void PrepareResourceController(EnergySystemObjectController purchasingObjectController, ApplianceObjectController applianceObjectController)
    {
        this.purchasingObjectController = purchasingObjectController;
        this.applianceObjectController = applianceObjectController;
      
        InvokeRepeating("TimePeriod", 0, 1);
        //InvokeRepeating("CalculateRenewablesOutput", 0, 1);
        //InvokeRepeating("CalculatePropertyIncome", 0, moneyCalculationInterval);
    }


    // Update is called once per frame
    private void Update()
    {
        timeController.UpdateTimeDateString();
        powerHelper.GetBreakerSwitchesValue(uIController.breakerPanelHelper.IsInterverSwitchOn, uIController.breakerPanelHelper.IsMainLoadSwitchOn, uIController.breakerPanelHelper.IsDGSwitchOn, GetCurrentTotalLoad(), uIController.breakerPanelHelper.IsMainSwitchOn);
        if (powerHelper.LoadDiff != 0f)
        {
            powerHelper.LoadDiff = 0f;
            
        }

        meterHelper.TargetLoadRate = GetCurrentTotalLoad();
        meterHelper.TargetPowerRate = powerHelper.TotalOutputRate;

        uIController.systemInfoPanelHelper.BatteryWarning = powerHelper.BatteryWarningText;


        //if (!isPopNotiDone)
        //{
        //    if (powerHelper.pop)
        //    {
        //        popupNotification.OpenNotification();
        //        isPopNotiDone = true;

        //    }
        //}
    }

    private void IncreaseDieselCo2()
    {
        foreach (var energySystem in purchasingObjectController.GetAllObjects())
        {
            if (energySystem.objectName.Equals("Diesel Generator") && energySystem.isRunning)
            {

            }
        }
    }

    public float GetCurrentTotalLoad()
    {
        float totalLoad = 0;
        foreach (var appliance in applianceObjectController.GetListOfAllAppliances())
        {
            if (appliance.isTurnedOn)
            {
                totalLoad += appliance.powerNeededRate;
            }
        }
        return totalLoad;
    }



    public void PurchaseFuel()
    {
        foreach (var item in purchasingObjectController.GetAllObjects())
        {
            if (item.GetType() == typeof(DieselGeneratorSO))
            {
                float costOfFuelNeed = (60f - item.fuelAmount)*2;

                if (SpendMoney((int)costOfFuelNeed))
                {
                    item.fuelAmount = 60f;
                }
                else
                {
                    Debug.Log("You don't have enough money to buy fuel.");
                }

                
            }
        }
    }




    public string GetTime()
    {
        return timeController.GetTime();
    }

    public float GetPoA()
    {
        return timeController.SolarRadiation;
    }

    /*public void CalculateRenewablesOutput()
    {
        foreach (var obj in purchasingObjectController.GetAllObjects())
        {
            if (IsInvertorOn() && obj.objectName.Equals("Solar Panel") || obj.objectName.Equals("Wind Turbine") && obj.isTurnedOn)
            {
                obj.powerGeneratedAmount += obj.powerGeneratedRate / 60;
            }
        }
    }*/

    private bool IsInvertorOn()
    {
        foreach (var obj in purchasingObjectController.GetAllObjects())
        {
            if (obj.objectName.Equals("Invertor") && obj.isTurnedOn)
            {
                return true;
            }
        }
        return false;
    }

    public void TimePeriod()
    {
        currentPoA = GetPoA();
        string endTime = GetTime();

        //Debug.Log(startTime + ", " + endTime);
        if(startTime!=endTime)
            CalculateTimePeriod(startTime, endTime);
        startTime = endTime;
        previousPoA = currentPoA;

    }

    public void CalculateTimePeriod(string start, string end)
    {
        string[] startResult = start.Split(char.Parse("_"));
        string[] endResult = end.Split(char.Parse("_"));

        float startHr = int.Parse(startResult[0]);
        float startMin = int.Parse(startResult[1]);
        float endHr = int.Parse(endResult[0]);
        float endMin = int.Parse(endResult[1]);
        //Debug.Log(startHr + ", " + startMin + ", " + endHr + ", " + endMin);

        if (startHr != endHr)
        {
            float previousHr = (60f - startMin)/60f;
            //Debug.Log("previous hr: " + previousHr);
            //Debug.Log("end min / 60f: " + endMin / 60f);
            //powerHelper.CalculatePowerOutput(purchasingObjectController.GetAllObjects(), previousHr, previousPoA);
            //powerHelper.CalculatePowerOutput(purchasingObjectController.GetAllObjects(), endMin / 60f, previousPoA);
            
        }
        else
        {
            float period = (endMin-startMin)/60;
            //Debug.Log("period: " + period);
            powerHelper.CalculateRenewablesOutput(purchasingObjectController.GetAllObjects(), period, currentPoA);
            emissionHelper.CalculateEmissions(purchasingObjectController.GetAllObjects(), period);
            moneyFromGrid(period);
            //powerHelper.CalculatePowerOutput(purchasingObjectController.GetAllObjects(), period, currentPoA);
        }
        //powerHelper.CalculateSolaPanelToMainLoadOutputRate(purchasingObjectController.GetAllObjects());
    }


    private void moneyFromGrid(float period)
    {
        if (powerHelper.CanRenewableSystemHandleLoad && !powerHelper.IsPowerLinesRunning)
        {
            AddMoney((int)(100 * period));
        }
    }

    #region Level
    private void LevelHelper_OnLevelChanged(object sender, EventArgs e)
    {
        UpdateExperienceValueUI();
        uIController.levelUpNotificationManager.OpenNotification();
    }

    private void LevelHelper_OnExperienceChanged(object sender, EventArgs e)
    {
        UpdateExperienceValueUI();
    }

    private void UpdateExperienceValueUI()
    {
        uIController.SetExperienceValue(levelHelper.Level, levelHelper.Experience, levelHelper.ExperienceToNextLevel);
    }
    public void AddExperience(int amount)
    {
        levelHelper.AddExperience(amount);
    }
    #endregion

    #region Money
    public void AddMoney(int amount)
    {
        moneyHelper.AddMoney(amount);
        UpdateMoneyValueUI();
    }



    private void UpdateMoneyValueUI()
    {
        uIController.SetMoneyValue(moneyHelper.Money);

    }

    public void CalculatePropertyIncome()
    {
        try
        {
            moneyHelper.CalculateMoney(purchasingObjectController.GetAllObjects());
            UpdateMoneyValueUI();
        }
        catch (MoneyException)
        {
            ReloadGame();
        }
    }



    public bool CanIBuyIt(int amount)
    {
        if (moneyHelper.Money >= amount)
        {
            return true;
        }
        return false;
    }

    public bool SpendMoney(int amount)
    {
        if (CanIBuyIt(amount))
        {
            try
            {
                moneyHelper.ReduceMoney(amount);
                UpdateMoneyValueUI();
                return true;
            }
            catch (MoneyException)
            {
                ReloadGame();
            }
        }
        return false;
    }
    #endregion

    private void ReloadGame()
    {
        Debug.Log("End the game");
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
