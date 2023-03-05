using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Mirror;
using System.Reflection;
using System;

public class ComboTests : BaseTest
{
    private RunnerPlayer _player;
    private PickupController _pickUpController;

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        Debug.Log("Setup");
        _player = game.GetRunner();
        _pickUpController = game.GetPickupController();
        var moveForward = _player.GetComponent<MoveForward>();

        var netIdentity = _player.GetComponent<NetworkIdentity>();
        Debug.Log($"isLocalPlayer before: {netIdentity.isLocalPlayer}");
        netIdentity.GetType()
            .GetProperty("isLocalPlayer")?
            .SetValue(netIdentity, true);

        Debug.Log($"isLocalPlayer after: {netIdentity.isLocalPlayer}");
        Debug.Log($"isLocalPlayer after: {moveForward.isLocalPlayer}");
        Debug.Log($"isLocalPlayer after: {moveForward.GetComponent<RunnerPlayer>().isLocalPlayer}");
    }

    /// <summary>
    /// Test to make sure the pickup controller is not null
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator PickUpControllerFound()
    {
        yield return new WaitForSeconds(0.1f);
        Assert.AreNotEqual(null, _pickUpController);
    }

    /// <summary>
    /// Test to make sure a coin spawns
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator SpawnCoin()
    {
        GameObject _coin = GameObject.Instantiate(Resources.Load("Pickups/Coin") as GameObject);
        _coin.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        Assert.AreNotEqual(null, _coin);
    }

    /// <summary>
    /// Test the combo for picking up 1 coin.
    /// Combo should be 1 points should be 5
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator CoinComboOne()
    {
        _player.GetComponent<Score>().ResetCombo();
        _player.GetComponent<Score>().ResetScore();
        GameObject _coin = GameObject.Instantiate(Resources.Load("Pickups/Coin") as GameObject);
        _coin.SetActive(true);
        _coin.transform.position = new Vector3(0, 2, 1000);
        yield return new WaitForSeconds(0.1f);
        Assert.AreNotEqual(null, _coin);

        PickupController _pickUp = game.GetPickupController();
        yield return new WaitForSeconds(0.1f);
        Assert.AreNotEqual(null, _pickUp);

        _player.GetComponent<Score>().ResetScore();
        _coin.transform.position = new Vector3(0, 2, 11);
        _player.transform.position = new Vector3(0, 0, 10);
        yield return new WaitForSeconds(0.1f);

        int tempScore = _player.GetComponent<Score>().GetScore();
        int tempCombo = _player.GetComponent<Score>().GetCurrentCombo();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(5, tempScore);
        Assert.AreEqual(1, tempCombo);
    }

    /// <summary>
    /// Test the combo for picking up 2 coins.
    /// Combo should be 2 points should be 5 + 5 x 2 = 15
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator CoinComboTwo()
    {
        GameObject[] _coinArr;
        _coinArr = new GameObject[2];

        for (int i = 0; i < _coinArr.Length; i++)
        {
            _coinArr[i] = GameObject.Instantiate(Resources.Load("Pickups/Coin") as GameObject);
            _coinArr[i].SetActive(true);
            _coinArr[i].transform.position = new Vector3(0, 2, 1000);
        }

        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < _coinArr.Length; i++)
        {
            Assert.AreNotEqual(null, _coinArr[i]);
        }

        PickupController _pickUp = game.GetPickupController();

        yield return new WaitForSeconds(0.1f);
        Assert.AreNotEqual(null, _pickUp);

        _player.GetComponent<Score>().ResetCombo();
        _player.GetComponent<Score>().ResetScore();
        _player.transform.position = new Vector3(0, 0, 10);
        for (int i = 0; i < _coinArr.Length; i++)
        {
            _coinArr[i].transform.position = new Vector3(0, 2, 11 + (i + 1));
        }
        yield return new WaitForSeconds(1f);

        int tempScore = _player.GetComponent<Score>().GetScore();
        int tempCombo = _player.GetComponent<Score>().GetCurrentCombo();

        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(15, tempScore);
        Assert.AreEqual(2, tempCombo);
    }

    /// <summary>
    /// Test the combo for picking up 10 coins.
    /// Combo should be 2 points should be 5 + 5 x 2 + 5 x 3 ... + 5 x 10 = 275
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator CoinComboTen()
    {
        GameObject[] _coinArr;
        _coinArr = new GameObject[10];

        for (int i = 0; i < _coinArr.Length; i++)
        {
            _coinArr[i] = GameObject.Instantiate(Resources.Load("Pickups/Coin") as GameObject);
            _coinArr[i].SetActive(true);
            _coinArr[i].transform.position = new Vector3(0, 2, 1000);
        }

        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < _coinArr.Length; i++)
        {
            Assert.AreNotEqual(null, _coinArr[i]);
        }

        PickupController _pickUp = game.GetPickupController();
        yield return new WaitForSeconds(0.1f);
        Assert.AreNotEqual(null, _pickUp);

        _player.GetComponent<Score>().ResetCombo();
        _player.GetComponent<Score>().ResetScore();
        for (int i = 0; i < _coinArr.Length; i++)
        {
            _coinArr[i].transform.position = new Vector3(0, 2, 11 + (i + 1f));
        }
        _player.transform.position = new Vector3(0, 0, 11);

        yield return new WaitForSeconds(2f);

        int tempScore = _player.GetComponent<Score>().GetScore();
        int tempCombo = _player.GetComponent<Score>().GetCurrentCombo();

        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(275, tempScore);
        Assert.AreEqual(10, tempCombo);
    }

    /// <summary>
    /// Test the maximum multiplier for the coin combo 20
    /// Combo should be 20 points should be 1050
    /// Expected points 5 x current combo so (5 x 1) + (5 x 2) + (5 x 3) .... + (5 x 20)
    /// so 5 + 10 +15 + 20 ... + 100 = 1050
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator CoinComboTwentyMaxMultiplier()
    {
        GameObject[] _coinArr;
        _coinArr = new GameObject[20];

        for (int i = 0; i < _coinArr.Length; i++)
        {
            _coinArr[i] = GameObject.Instantiate(Resources.Load("Pickups/Coin") as GameObject);
            _coinArr[i].SetActive(true);
            _coinArr[i].transform.position = new Vector3(0, 2, 1000);
        }

        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < _coinArr.Length; i++)
        {
            Assert.AreNotEqual(null, _coinArr[i]);
        }

        PickupController _pickUp = game.GetPickupController();
        yield return new WaitForSeconds(0.1f);
        Assert.AreNotEqual(null, _pickUp);

        _player.GetComponent<Score>().ResetCombo();
        _player.GetComponent<Score>().ResetScore();
        for (int i = 0; i < _coinArr.Length; i++)
        {
            _coinArr[i].transform.position = new Vector3(0, 2, 11 + (i +1f));
        }
        _player.transform.position = new Vector3(0, 0, 11);
  
        yield return new WaitForSeconds(3f);

        int tempScore = _player.GetComponent<Score>().GetScore();
        int tempCombo = _player.GetComponent<Score>().GetCurrentCombo();
        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(20, tempCombo);
        Assert.AreEqual(1050, tempScore);
    }

    /// <summary>
    /// Test the maximum multiplier for the coin combo plus one so 21
    /// Combo should be 21 points should be 1150
    /// After a combo is greater than 20 the point multpiplier should still be 20
    /// Expected points 5 x current combo so (5 x 1) + (5 x 2) + (5 x 3) .... + (5 x 20) + (5 x 20) again
    /// so 5 + 10 +15 + 20 ... + 100 + 100 again = 1150
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator CoinComboTwentyOneMaxMultiplierPlusOne()
    {
        GameObject[] _coinArr;
        _coinArr = new GameObject[21];

        for (int i = 0; i < _coinArr.Length; i++)
        {
            _coinArr[i] = GameObject.Instantiate(Resources.Load("Pickups/Coin") as GameObject);
            _coinArr[i].SetActive(true);
            _coinArr[i].transform.position = new Vector3(0, 2, 1000);
        }

        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < _coinArr.Length; i++)
        {
            Assert.AreNotEqual(null, _coinArr[i]);
        }

        PickupController _pickUp = game.GetPickupController();
        yield return new WaitForSeconds(0.1f);
        Assert.AreNotEqual(null, _pickUp);

        _player.GetComponent<Score>().ResetCombo();
        _player.GetComponent<Score>().ResetScore();
        for (int i = 0; i < _coinArr.Length; i++)
        {
            _coinArr[i].transform.position = new Vector3(0, 2, 11 + (i + 1f));
        }
        _player.transform.position = new Vector3(0, 0, 11);

        yield return new WaitForSeconds(3f);


        int tempScore = _player.GetComponent<Score>().GetScore();
        int tempCombo = _player.GetComponent<Score>().GetCurrentCombo();
        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(21, tempCombo);
        Assert.AreEqual(1150, tempScore);
    }
}