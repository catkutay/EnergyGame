public interface IResourceController
{
    int StartMoneyAmount { get;  }
    float MoneyCalculationInterval { get; }
    int removeCost { get;}

    void AddMoney(int amount);
    void CalculatePropertyIncome();
    bool CanIBuyIt(int amount);
    bool SpendMoney(int amount);
    void AddExperience(int amount);

    void PrepareResourceController(EnergySystemObjectController purchasingObjectController, ApplianceObjectController applianceObjectController);
}