using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

[TestFixture]
public class PlayerStateTests
{
    UIController uiController;
    GameController gameControllerComponent;

    [SetUp]
    public void Init()
    {
        GameObject gameControllerObject = new GameObject();
        var cameraMovementComponent = gameControllerObject.AddComponent<CameraMovement>();
        gameControllerObject.AddComponent<ResourceControllerTests>();


        uiController = Substitute.For<UIController>();
        gameControllerComponent = gameControllerObject.AddComponent<GameController>();
        gameControllerComponent.resourceControllerGameObject = gameControllerObject;
        gameControllerObject.AddComponent<PlacementController>();
        gameControllerComponent.placementControllerGameObject = gameControllerObject;
        gameControllerComponent.cameraMovementController = cameraMovementComponent;
        gameControllerComponent.uiController = uiController;
    }

    [UnityTest]
    public IEnumerator PlayerStatsPlayerSelectionStateTestWithEnumeratorPasses()
    {
        yield return new WaitForEndOfFrame(); // awake
        yield return new WaitForEndOfFrame(); // start
        Assert.IsTrue(gameControllerComponent.State is PlayerSelectionState);
    }

    [UnityTest]
    public IEnumerator PlayerStatsPlayerPurchasingSolarPanelStateTestWithEnumeratorPasses()
    {
        yield return new WaitForEndOfFrame(); // awake
        yield return new WaitForEndOfFrame(); // start
        gameControllerComponent.State.OnPuchasingEnergySystem("Solar Panel");
        yield return new WaitForEndOfFrame();
        Assert.IsTrue(gameControllerComponent.State is PlayerPurchasingSolarPanelState);
    }

    [UnityTest]
    public IEnumerator PlayerStatsPlayerPurchasingBatteryStateTestWithEnumeratorPasses()
    {
        yield return new WaitForEndOfFrame(); // awake
        yield return new WaitForEndOfFrame(); // start
        gameControllerComponent.State.OnPuchasingEnergySystem("Battery");
        yield return new WaitForEndOfFrame();
        Assert.IsTrue(gameControllerComponent.State is PlayerPurchasingBatteryState);
    }

    [UnityTest]
    public IEnumerator PlayerStatsPlayerPurchasingDieselGeneratorStateTestWithEnumeratorPasses()
    {
        yield return new WaitForEndOfFrame(); // awake
        yield return new WaitForEndOfFrame(); // start
        gameControllerComponent.State.OnPuchasingEnergySystem("Diesel Generator");
        yield return new WaitForEndOfFrame();

        Assert.IsTrue(gameControllerComponent.State is PlayerPurchasingDieselGeneratorState);
    }

    [UnityTest]
    public IEnumerator PlayerStatsPlayerPurchasingInvertorStateTestWithEnumeratorPasses()
    {
        yield return new WaitForEndOfFrame(); // awake
        yield return new WaitForEndOfFrame(); // start
        gameControllerComponent.State.OnPuchasingEnergySystem("Invertor");
        yield return new WaitForEndOfFrame();
        Assert.IsTrue(gameControllerComponent.State is PlayerPurchasingInvertorState);
    }

    [UnityTest]
    public IEnumerator PlayerStatsPlayerPurchasingChargeControllerStateTestWithEnumeratorPasses()
    {
        yield return new WaitForEndOfFrame(); // awake
        yield return new WaitForEndOfFrame(); // start
        gameControllerComponent.State.OnPuchasingEnergySystem("Charge Controller");
        yield return new WaitForEndOfFrame();
        Assert.IsTrue(gameControllerComponent.State is PlayerPurchasingChargeControllerState);
    }

    [UnityTest]
    public IEnumerator PlayerStatsPlayerPurchasingWindTurbineStateTestWithEnumeratorPasses()
    {
        yield return new WaitForEndOfFrame(); // awake
        yield return new WaitForEndOfFrame(); // start
        gameControllerComponent.State.OnPuchasingEnergySystem("Wind Turbine");
        yield return new WaitForEndOfFrame();
        Assert.IsTrue(gameControllerComponent.State is PlayerPurchasingWindTurbineState);
    }


    [UnityTest]
    public IEnumerator PlayerStatsPlayerSellingObjectStateTestWithEnumeratorPasses()
    {
        yield return new WaitForEndOfFrame(); // awake
        yield return new WaitForEndOfFrame(); // start
        gameControllerComponent.State.OnSellingObject();
        yield return new WaitForEndOfFrame();
        Assert.IsTrue(gameControllerComponent.State is PlayerSellingObjectState);
    }


}
