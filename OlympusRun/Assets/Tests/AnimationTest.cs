using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AnimationTest : BaseTest
{
    [UnityTest]
    public IEnumerator IsAnimationPlaying()
    {
        GameObject runner = GameObject.FindGameObjectWithTag("Runner");

        Animator animator = runner.transform.GetChild(0).GetChild(0).GetComponent<Animator>();

        MoveForward moveF = game.GetMoveForward();

        yield return new WaitForSeconds(1.5f);

        // Replace "animationName" with the name of the animation you want to test
        bool isPlaying = animator.GetCurrentAnimatorStateInfo(0).IsName("Running");

        Assert.IsTrue(isPlaying, "The animation is not playing");
    }
}
