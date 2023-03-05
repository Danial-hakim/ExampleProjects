using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.TestTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class GodCameraTest
    {
        private GameObject _groundSpawner;
        private GameObject _runner;
        private GodCamera _godCamera;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            
            _groundSpawner = Object.Instantiate(Resources.Load<GameObject>("GroundSpawner"));
            
            _runner = Object.Instantiate(Resources.Load<GameObject>("Players/Runner/RunnerPlayer"));

            _godCamera = Object.Instantiate(Resources.Load<GodCamera>("Players/God/GodCamera"));

            yield return null;
        }

        [Test]
        public void GodCameraSeesPlayer()
        { 
            var camera = _godCamera.GetComponent<Camera>();
            Assert.NotNull(camera);

            var runnerCameraPos = camera.WorldToViewportPoint(_runner.transform.position);
            
            // Check that the camera correctly see the player
            Assert.Greater(runnerCameraPos.z, 0);
            Assert.That(runnerCameraPos.x, Is.InRange(0, 1));
            Assert.That(runnerCameraPos.y, Is.InRange(0, 1));
        }
    }
}