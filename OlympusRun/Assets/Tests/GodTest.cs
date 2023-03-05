// Coders:
// Caroline Percy
// ...


using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GodTest : BaseTest
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator GodAvoidAttack()
    {
        GodPlayer player = game.GetGod();

        game.GetRunner().gameObject.SetActive(false);

        player.ChooseAttack(1);

        player.PlayerAttack();

        yield return new WaitForSeconds(1);

        GameObject obstacle = GameObject.FindGameObjectWithTag("Inpenetrable");

        Assert.IsNotNull(obstacle);

        Object.Destroy(obstacle);

        game.GetRunner().gameObject.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator GodSlideAttack()
    {
        GodPlayer player = game.GetGod();

        game.GetRunner().gameObject.SetActive(false);

        player.ChooseAttack(2);

        player.PlayerAttack();

        yield return new WaitForSeconds(1);

        GameObject obstacle = GameObject.FindGameObjectWithTag("SlideObstacle");

        Assert.IsNotNull(obstacle);

        Object.Destroy(obstacle);

        game.GetRunner().gameObject.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator GodJumpAttack()
    {
        GodPlayer player = game.GetGod();

        game.GetRunner().gameObject.SetActive(false);

        player.ChooseAttack(3);

        player.PlayerAttack();

        yield return new WaitForSeconds(1);

        GameObject obstacle = GameObject.FindGameObjectWithTag("JumpObstacle");

        Assert.IsNotNull(obstacle);

        Object.Destroy(obstacle);

        game.GetRunner().gameObject.SetActive(true);
    }
}
