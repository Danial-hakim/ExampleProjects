// Coders:
// Caroline Percy
// ...

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// The class that will be in charge of all tests relating to the Runner.
/// </summary>

public class RunnerTests : BaseTest
{
    /// <summary>
    /// Test to make sure the swipe left and right are working.
    /// </summary>
    /// <returns>The time to wait until the script should continue the test.</returns>
    [UnityTest]
    public IEnumerator RunnerSwipeLeftAndRightTest()
    {
        RunnerPlayer runner = game.GetRunner();
        float oldX = runner.transform.position.x;

        // Check the left swipe
        runner.SwipeStart(new Vector2(0, 0), 1);

        yield return null;

        runner.SwipeEnd(new Vector2(-10, 0), 1);

        yield return new WaitForSeconds(1);

        Assert.IsTrue(oldX > runner.transform.position.x);

        // Check the right swipe

        oldX = runner.transform.position.x;

        runner.SwipeStart(new Vector2(0, 0), 1);

        yield return null;

        runner.SwipeEnd(new Vector2(10, 0), 1);

        yield return new WaitForSeconds(1);

        Assert.IsTrue(oldX < runner.transform.position.x);
    }

    /// <summary>
    /// Test to make sure the jump is working.
    /// </summary>
    /// <returns>The time to wait until the script should continue the test.</returns>
    [UnityTest]
    public IEnumerator RunnerJumpTest()
    {
        RunnerPlayer runner = game.GetRunner();

        float oldY = runner.transform.position.y;

        runner.Jump();

        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(oldY < runner.transform.position.y);
    }

    /// <summary>
    /// Test to make sure the sliding is working.
    /// </summary>
    /// <returns>The time to wait until the script should continue the test.</returns>
    [UnityTest]
    public IEnumerator RunnerSlidingTest()
    {
        RunnerPlayer runner = game.GetRunner();

        // Check the left swipe
        runner.SwipeStart(new Vector2(0, 0), 1);

        yield return null;

        runner.SwipeEnd(new Vector2(0, -10), 1);

        yield return new WaitForSeconds(1);

        Assert.IsTrue(runner.Sliding);
    }

    /// <summary>
    /// Test to make sure the AI jumps when it should.
    /// </summary>
    /// <returns>The time to wait until the script should continue the test.</returns>
    [UnityTest]
    public IEnumerator RunnerAIJumpTest()
    {
        RunnerPlayer runner = game.GetRunner();

        AIBrain ai = game.GetRunnerAI();
        ObstacleSpawner spawner = game.GetObstacleSpawner();

        ai.enabled = true;
        runner.enabled = false;
        runner.transform.GetChild(1).gameObject.SetActive(true);

        float oldY = runner.transform.position.y;

        GameObject temp = spawner.SpawnWideObstacle();

        temp.gameObject.transform.position = new Vector3(0,1, 15);

        yield return new WaitForSeconds(2);

        Object.Destroy(temp);
        Assert.IsTrue(oldY < runner.transform.position.y);
    }

    [UnityTest]
    public IEnumerator RunnerAILaneChangeTest()
    {
        RunnerPlayer runner = game.GetRunner();

        AIBrain ai = game.GetRunnerAI();
        ObstacleSpawner spawner = game.GetObstacleSpawner();

        ai.enabled = true;
        runner.enabled = false;
        runner.transform.GetChild(1).gameObject.SetActive(true);

        GameObject temp = spawner.SpawnSmallObstacle();

        temp.gameObject.transform.position = new Vector3(runner.gameObject.transform.position.x, 1, 15);
        yield return new WaitForSeconds(2);

        float posX = temp.gameObject.transform.position.x;
        Object.Destroy(temp);
        Assert.IsFalse(posX == ai.transform.position.x);

    }

    /// <summary>
    /// Test to make sure the AI slides when it should.
    /// </summary>
    /// <returns>The time to wait until the script should continue the test.</returns>
    [UnityTest]
    public IEnumerator RunnerAISlidingTest()
    {
        RunnerPlayer runner = game.GetRunner();

        AIBrain ai = game.GetRunnerAI();
        ObstacleSpawner spawner = game.GetObstacleSpawner();

        ai.enabled = true;
        runner.enabled = false;
        runner.transform.GetChild(1).gameObject.SetActive(true);

        GameObject temp = spawner.SpawnHighObstacle();

        temp.gameObject.transform.position = new Vector3(0, 2, 15);

        yield return new WaitForSeconds(2);

        Object.Destroy(temp);
        Assert.IsTrue(ai.sliding);
    }

    /// <summary>
    /// Test to make sure the powerups work as they should
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator PowerUpTest()
    {
        RunnerPlayer runner = game.GetRunner();
        RunnerHealthController health = game.GetHealthController();
        MoveForward moveF = game.GetMoveForward();

        health.TestDamage(20);

        Assert.IsTrue(health.GetCurrentHealth() < health.GetMaxHealth());
        yield return new WaitForSeconds(0.5f);


        health.TestMax();

        Assert.IsTrue(health.GetCurrentHealth() == health.GetMaxHealth());
        yield return new WaitForSeconds(0.5f);

        health.TestDamage(20);
        health.TestDamage(20);
        int afterDamage = health.GetCurrentHealth();
        health.TestPartial();

        Assert.IsTrue(health.GetCurrentHealth() < health.GetMaxHealth());
        Assert.IsTrue(health.GetCurrentHealth() > afterDamage);
        yield return new WaitForSeconds(0.5f);

        int prevSpeed = moveF.TestGetSpeed();
        moveF.TestSetSpeed(2);

        Assert.IsTrue(prevSpeed < moveF.TestGetSpeed());

        prevSpeed = moveF.TestGetSpeed();
        moveF.TestSetSpeed(-2);

        Assert.IsTrue(prevSpeed > moveF.TestGetSpeed());
    }

    [UnityTest]
    public IEnumerator BoundaryCheck()
    {
        int oldX = 2;
        RunnerPlayer runner = game.GetRunner();

        // Check the right boundary
        runner.TestChangePos(3, oldX);

        runner.SwipeStart(new Vector2(0, 0), 1);

        yield return null;

        runner.SwipeEnd(new Vector2(10, 0), 1);

        yield return new WaitForSeconds(1);

        float x = Mathf.Round(runner.transform.position.x);
        Assert.IsTrue(oldX == Mathf.Round(runner.transform.position.x));

        // Check the left boundary
        oldX = -2;
        runner.TestChangePos(1, oldX);
        
        runner.SwipeStart(new Vector2(0, 0), 1);

        yield return null;

        runner.SwipeEnd(new Vector2(-10, 0), 1);

        yield return new WaitForSeconds(1);

        x = Mathf.Round(runner.transform.position.x);
        Assert.IsTrue(oldX == Mathf.Round(runner.transform.position.x));
	}
    /// <summary>
    /// Test to make sure the player health work as they should
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator HealthBarTest()
    {
        RunnerPlayer runner = game.GetRunner();
        RunnerHealthController health = game.GetHealthController();
        MoveForward moveF = game.GetMoveForward();

        health.TestDamage(20);

        Assert.IsTrue(health.GetCurrentHealth() < health.GetMaxHealth());
        yield return new WaitForSeconds(0.5f);

        health.TestMax();

        Assert.IsTrue(health.GetCurrentHealth() == health.GetMaxHealth());
        yield return new WaitForSeconds(0.5f);

        health.TestDamage(20);
        health.TestDamage(20);
        int afterDamage = health.GetCurrentHealth();
        health.TestPartial();

        Assert.IsTrue(health.GetCurrentHealth() < health.GetMaxHealth());
        Assert.IsTrue(health.GetCurrentHealth() > afterDamage);
    }
}
