using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Mirror;
using System.Reflection;

public class ScoreTests : BaseTest
{
    private RunnerPlayer _player;

    //[SetUp]
    //public override void Setup()
    //{
    //    base.Setup();

    //    _player = game.GetRunner();
    //    Debug.Log($"isLocalPlayer: {_player.isLocalPlayer}");
    //    var playerTypeProp = typeof(RunnerPlayer).GetField("<isLocalPlayer>k__BackingField", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
    //    Debug.Log(playerTypeProp);
    //    playerTypeProp.SetValue(_player, true);

    //    Debug.Log($"isLocalPlayer: {_player.isLocalPlayer}");
    //}

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        Debug.Log("Setup");
        _player = game.GetRunner();
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

    [UnityTest]
    public IEnumerator BonusPointsForTwelveSpeedTest()
    {
        //RunnerPlayer runner = game.GetRunner();
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<MoveForward>().SetSpeed(+4);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(12, _player.GetComponent<MoveForward>().TestGetSpeed());
        _player.GetComponent<Score>().CheckSpeedBonusPoints(12);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(150, tempScore);
    }

    [UnityTest]
    public IEnumerator BonusPointsForSixteenSpeedTest()
    {
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<MoveForward>().SetSpeed(+8);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(16, _player.GetComponent<MoveForward>().TestGetSpeed());
        _player.GetComponent<Score>().CheckSpeedBonusPoints(16);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(200, tempScore);
    }

    [UnityTest]
    public IEnumerator BonusPointsForTwentyFourSpeedTest()
    {
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<MoveForward>().SetSpeed(+16);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(24, _player.GetComponent<MoveForward>().TestGetSpeed());
        _player.GetComponent<Score>().CheckSpeedBonusPoints(24);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(400, tempScore);
    }

    [UnityTest]
    public IEnumerator BonusPointsForThirtySpeedTest()
    {
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<MoveForward>().SetSpeed(+22);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(30, _player.GetComponent<MoveForward>().TestGetSpeed());
        _player.GetComponent<Score>().CheckSpeedBonusPoints(30);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(600, tempScore);
    }

    [UnityTest]
    public IEnumerator BonusPointsForOneHundredMetresTest()
    {
        _player.transform.position = new Vector3(0,0,100);
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<Score>().CheckDistanceBonusPoints(100);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(50, tempScore);
    }

    [UnityTest]
    public IEnumerator BonusPointsForTwoHundredFiftyMetresTest()
    {
        _player.transform.position = new Vector3(0, 0, 250);
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<Score>().CheckDistanceBonusPoints(250);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(100, tempScore);
    }

    [UnityTest]
    public IEnumerator BonusPointsForFiveHundredMetresTest()
    {
        _player.transform.position = new Vector3(0, 0, 500);
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<Score>().CheckDistanceBonusPoints(500);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(200, tempScore);
    }

    [UnityTest]
    public IEnumerator BonusPointsForSevenHundredFiftyMetresTest()
    {
        _player.transform.position = new Vector3(0, 0, 750);
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<Score>().CheckDistanceBonusPoints(750);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(300, tempScore);
    }

    [UnityTest]
    public IEnumerator BonusPointsForOneThousandMetresTest()
    {
        _player.transform.position = new Vector3(0, 0, 1000);
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<Score>().CheckDistanceBonusPoints(1000);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(400, tempScore);
    }

    [UnityTest]
    public IEnumerator BonusPointsForOneThousandFiveHundredMetresTest()
    {
        _player.transform.position = new Vector3(0, 0, 1500);
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<Score>().CheckDistanceBonusPoints(1500);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(600, tempScore);
    }

    [UnityTest]
    public IEnumerator BonusPointsForTwoThousandMetresTest()
    {
        _player.transform.position = new Vector3(0, 0, 2000);
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<Score>().CheckDistanceBonusPoints(2000);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(800, tempScore);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator NoPointsForOneHundredOneMetresTest()
    {
        _player.transform.position = new Vector3(0, 0, 101);
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<Score>().CheckDistanceBonusPoints(101);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(0, tempScore);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator NoPointsForNinetyNineMetresTest()
    {
        _player.transform.position = new Vector3(0, 0, 99);
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<Score>().CheckDistanceBonusPoints(99);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(0, tempScore);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator BonusPointsForThirtySecondsTest()
    {
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<Score>().CheckTimeAliveBonusPoints(30);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(200, tempScore);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator BonusPointsForSixtySecondsTest()
    {
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<Score>().CheckTimeAliveBonusPoints(60);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(500, tempScore);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator BonusPointsForTwoMinutesTest()
    {
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<Score>().CheckTimeAliveBonusPoints(120);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(1000, tempScore);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator NoPointsForTwentyNineSecondsTest()
    {
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<Score>().CheckTimeAliveBonusPoints(29);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(0, tempScore);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator NoPointsForThirtyOneSecondsTest()
    {
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<Score>().CheckTimeAliveBonusPoints(31);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(0, tempScore);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator NoPointsForFourteenSpeedTest()
    {
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<MoveForward>().SetSpeed(+6);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(14, _player.GetComponent<MoveForward>().TestGetSpeed());
        _player.GetComponent<Score>().CheckSpeedBonusPoints(14);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(0, tempScore);
    }

    [UnityTest]
    public IEnumerator NoPointsForTwentySixSpeedTest()
    {
        _player.GetComponent<Score>().ResetScore();
        _player.GetComponent<MoveForward>().SetSpeed(+18);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(26, _player.GetComponent<MoveForward>().TestGetSpeed());
        _player.GetComponent<Score>().CheckSpeedBonusPoints(26);
        int tempScore = _player.GetComponent<Score>().GetScore();
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(0, tempScore);
    }

}
