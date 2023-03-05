// Coders:
// Caroline Percy
// ...


using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

/// <summary>
/// Test script to test anything related to the UI.
/// </summary>
public class UITest : BaseTest
{
    /// <summary>
    /// Tests whether clicking will spawn a trail
    /// </summary>
    /// <returns>Time to wait for the trail to spawn.</returns>
   [UnityTest]
    public IEnumerator DrawTrail()
    {
        GameObject.Find("MouseTrailCanvas").GetComponent<FollowMouse>().SpawnTrail();

        yield return new WaitForEndOfFrame();

        GameObject trail = GameObject.Find("TouchTrail(Clone)");

        Assert.IsNotNull(trail);

        Object.Destroy(trail);
        
    }

}
