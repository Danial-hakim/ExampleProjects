using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Mirror;
using System.Reflection;

public class GoalsPointsTests : BaseTest
{
    private RunnerPlayer _player;
    private PickupController _pickUpController;
    private GoalController _goalController;


    [SetUp]
    public override void Setup()
    {
        base.Setup();

        Debug.Log("Setup");
        _player = game.GetRunner();
        _pickUpController = game.GetPickupController();
        _goalController = game.GetGoalController();

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

    //[UnityTest]
    //public IEnumerator TestAllPickUpGoalsApples()
    //{
    //    if (_goalController.isAppleGoal() == true)
    //    {
    //        int PickupsToSpawn = _goalController.GetCurrentCollectionGoal();
    //        GameObject[] _pickUpArr;
    //        _pickUpArr = new GameObject[PickupsToSpawn];
    //        int PointsForPickUps = 5;
    //        Debug.Log("Test Pickups : " + PickupsToSpawn);

    //        for (int i = 0; i < _pickUpArr.Length; i++)
    //        {
    //            Debug.Log("Test Apples : " + (i + 1));
    //            _pickUpArr[i] = GameObject.Instantiate(Resources.Load("Pickups/Apple") as GameObject);
    //            _pickUpArr[i].SetActive(true);
    //            _pickUpArr[i].transform.position = new Vector3(0, 2, 1000);
    //        }
    //        yield return new WaitForSeconds(1.0f);

    //        for (int i = 0; i < _pickUpArr.Length; i++)
    //        {
    //            Assert.AreNotEqual(null, _pickUpArr[i]);
    //        }

    //        PickupController _pickUp = game.GetPickupController();

    //        yield return new WaitForSeconds(0.1f);
    //        Assert.AreNotEqual(null, _pickUp);

    //        _player.GetComponent<Score>().ResetCombo();
    //        _player.GetComponent<Score>().ResetScore();
    //        _player.transform.position = new Vector3(0, 0, 10);
    //        for (int i = 0; i < _pickUpArr.Length; i++)
    //        {
    //            _pickUpArr[i].transform.position = new Vector3(0, 2, 11 + 1);
    //        }
    //        yield return new WaitForSeconds(1f);

    //        int tempScore = _player.GetComponent<Score>().GetScore();
    //        int _scoreFromCombo = 0;

    //        for (int i = 0; i < _pickUpArr.Length; i++)
    //        {
    //            Debug.Log("Test Points For PickUps : " + PointsForPickUps);
    //            _scoreFromCombo += (i + 1) * PointsForPickUps;
    //        }

    //        int expectedScore = (_pickUpArr.Length * _goalController.GetGoalMultiplier()) + _scoreFromCombo;
    //        yield return new WaitForSeconds(0.1f);
    //        Assert.AreEqual(expectedScore, tempScore);
    //    }
    //    else
    //    {
    //        Debug.Log("Not Apple");
    //        Assert.AreEqual(false, _goalController.isAppleGoal());
    //    }
    //}

    //[UnityTest]
    //public IEnumerator TestAllPickUpGoalsPoms()
    //{
    //    if (_goalController.isPomGoal() == true)
    //    {
    //        int PickupsToSpawn = _goalController.GetCurrentCollectionGoal();
    //        GameObject[] _pickUpArr;
    //        _pickUpArr = new GameObject[PickupsToSpawn];
    //        int PointsForPickUps = 5;
    //        Debug.Log("Test Pickups : " + PickupsToSpawn);

    //        for (int i = 0; i < _pickUpArr.Length; i++)
    //        {
    //            Debug.Log("Test Poms : " + (i + 1));
    //            _pickUpArr[i] = GameObject.Instantiate(Resources.Load("Pickups/Pomegranate") as GameObject);
    //            _pickUpArr[i].SetActive(true);
    //            _pickUpArr[i].transform.position = new Vector3(0, 2, 1000);
    //        }
    //        yield return new WaitForSeconds(1.0f);

    //        for (int i = 0; i < _pickUpArr.Length; i++)
    //        {
    //            Assert.AreNotEqual(null, _pickUpArr[i]);
    //        }

    //        PickupController _pickUp = game.GetPickupController();

    //        yield return new WaitForSeconds(0.1f);
    //        Assert.AreNotEqual(null, _pickUp);

    //        _player.GetComponent<Score>().ResetCombo();
    //        _player.GetComponent<Score>().ResetScore();
    //        _player.transform.position = new Vector3(0, 0, 10);
    //        for (int i = 0; i < _pickUpArr.Length; i++)
    //        {
    //            _pickUpArr[i].transform.position = new Vector3(0, 2, 11 + 1);
    //        }
    //        yield return new WaitForSeconds(1f);

    //        int tempScore = _player.GetComponent<Score>().GetScore();
    //        int _scoreFromCombo = 0;

    //        for (int i = 0; i < _pickUpArr.Length; i++)
    //        {
    //            Debug.Log("Test Points For PickUps : " + PointsForPickUps);
    //            _scoreFromCombo += (i + 1) * PointsForPickUps;
    //        }

    //        int expectedScore = (_pickUpArr.Length * _goalController.GetGoalMultiplier()) + _scoreFromCombo;
    //        yield return new WaitForSeconds(0.1f);
    //        Assert.AreEqual(expectedScore, tempScore);
    //    }
    //    else
    //    {
    //        Debug.Log("Not Poms");

    //        Assert.AreEqual(false, _goalController.isPomGoal());
    //    }
    //}

    //[UnityTest]
    //public IEnumerator TestAllPickUpGoalsCoins()
    //{
    //    if (_goalController.isCoinGoal() == true)
    //    {
    //        int PickupsToSpawn = _goalController.GetCurrentCollectionGoal();
    //        GameObject[] _pickUpArr;
    //        _pickUpArr = new GameObject[PickupsToSpawn];
    //        int PointsForPickUps = 5;
    //        Debug.Log("Test Pickups : " + PickupsToSpawn);

    //        for (int i = 0; i < _pickUpArr.Length; i++)
    //        {
    //            Debug.Log("Test Coins : " + (i+1));
    //            _pickUpArr[i] = GameObject.Instantiate(Resources.Load("Pickups/Coin") as GameObject);
    //            _pickUpArr[i].SetActive(true);
    //            _pickUpArr[i].transform.position = new Vector3(0, 2, 1000);
    //        }
    //        yield return new WaitForSeconds(1.0f);

    //        for (int i = 0; i < _pickUpArr.Length; i++)
    //        {
    //            Assert.AreNotEqual(null, _pickUpArr[i]);
    //        }

    //        PickupController _pickUp = game.GetPickupController();

    //        yield return new WaitForSeconds(0.1f);
    //        Assert.AreNotEqual(null, _pickUp);

    //        _player.GetComponent<Score>().ResetCombo();
    //        _player.GetComponent<Score>().ResetScore();
    //        _player.transform.position = new Vector3(0, 0, 10);
    //        for (int i = 0; i < _pickUpArr.Length; i++)
    //        {
    //            _pickUpArr[i].transform.position = new Vector3(0, 2, 11 + 1);
    //        }
    //        yield return new WaitForSeconds(1f);

    //        int tempScore = _player.GetComponent<Score>().GetScore();
    //        int _scoreFromCombo = 0;

    //        for (int i = 0; i < _pickUpArr.Length; i++)
    //        {
    //            Debug.Log("Test Points For PickUps : " + PointsForPickUps);
    //            _scoreFromCombo += (i + 1) * PointsForPickUps;
    //        }

    //        int expectedScore = (_pickUpArr.Length * _goalController.GetGoalMultiplier()) + _scoreFromCombo;
    //        yield return new WaitForSeconds(0.1f);
    //        Assert.AreEqual(expectedScore, tempScore);
    //    }
    //    else
    //    {
    //        Debug.Log("Not Coins");

    //        Assert.AreEqual(false, _goalController.isCoinGoal());
    //    }
    //}

    [UnityTest]
    public IEnumerator TestAllPickUpGoalsAndPointsReceived()
    {
        if (_goalController.isCoinGoal() == true || _goalController.isPomGoal() == true || _goalController.isAppleGoal() == true)
        {
            int PickupsToSpawn = _goalController.GetCurrentCollectionGoal();
            GameObject[] _pickUpArr;
            _pickUpArr = new GameObject[PickupsToSpawn];
            int PointsForPickUps = 5;
            Debug.Log("Test Pickups : " + PickupsToSpawn);

            for (int i = 0; i < _pickUpArr.Length; i++)
            {
                if (_goalController.isAppleGoal())
                {
                    _pickUpArr[i] = GameObject.Instantiate(Resources.Load("Pickups/Apple") as GameObject);
                    Debug.Log("Test Apples ");
                    Debug.Log("Test Points For PickUps one of these : " + PointsForPickUps);

                }
                else if (_goalController.isPomGoal())
                {
                    _pickUpArr[i] = GameObject.Instantiate(Resources.Load("Pickups/Pomegranate") as GameObject);
                    Debug.Log("Test Poms ");
                    Debug.Log("Test Points For PickUps one of these : " + PointsForPickUps);

                }
                else if (_goalController.isCoinGoal())
                {
                    _pickUpArr[i] = GameObject.Instantiate(Resources.Load("Pickups/Coin") as GameObject);
                    Debug.Log("Test Coins ");
                    Debug.Log("Test Points For PickUps one of these : " + PointsForPickUps);

                }
                _pickUpArr[i].SetActive(true);
                _pickUpArr[i].transform.position = new Vector3(0, 2, 1000);
            }


            yield return new WaitForSeconds(1.0f);

            for (int i = 0; i < _pickUpArr.Length; i++)
            {
                Assert.AreNotEqual(null, _pickUpArr[i]);
            }

            PickupController _pickUp = game.GetPickupController();

            yield return new WaitForSeconds(0.1f);
            Assert.AreNotEqual(null, _pickUp);

            _player.GetComponent<Score>().ResetCombo();
            _player.GetComponent<Score>().ResetScore();
            _player.transform.position = new Vector3(0, 0, 10);
            for (int i = 0; i < _pickUpArr.Length; i++)
            {
                _pickUpArr[i].transform.position = new Vector3(0, 2, 11 + 1);
            }
            yield return new WaitForSeconds(1f);

            int tempScore = _player.GetComponent<Score>().GetScore();
            int _scoreFromCombo = 0;

            for (int i = 0; i < _pickUpArr.Length; i++)
            {
                Debug.Log("Test Points For PickUps : " + PointsForPickUps);
                _scoreFromCombo += (i + 1) * PointsForPickUps;
            }

            int expectedScore = (_pickUpArr.Length * _goalController.GetGoalMultiplier()) + _scoreFromCombo;
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(expectedScore, tempScore);
        }
        else
        {
            if (_goalController.isMaxHealthGoal() == true)
            {
                Debug.Log("isHealth");
                Assert.AreEqual(true, _goalController.isMaxHealthGoal());
            }
            else if (_goalController.isSpeedUpGoal() == true)
            {
                Debug.Log("isSpeedUp");
                Assert.AreEqual(true, _goalController.isSpeedUpGoal());
            }
            else if (_goalController.isSpeedDownGoal() == true)
            {
                Debug.Log("isSpeedDown");
                Assert.AreEqual(true, _goalController.isSpeedDownGoal());
            }
        }
    }

}
