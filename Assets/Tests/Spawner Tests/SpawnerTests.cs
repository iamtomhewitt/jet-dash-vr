using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;
using Utility;

namespace Tests
{
    public class SpawnerTests
    {
        private GameObject animatedObstacle;
        private GameObject obstacleParent;
        private GameObject obstacleSpawner;
        private GameObject player;
        private GameObject stationaryObstacle;

        [SetUp]
        public void Setup()
        {
            animatedObstacle = MonoBehaviour.Instantiate(TestConstants.GetResource("Animated Obstacle"));
            obstacleParent = MonoBehaviour.Instantiate(new GameObject("Obstacles"));
            obstacleSpawner = MonoBehaviour.Instantiate(TestConstants.GetResource("Spawners/Obstacle Spawner"));
            player = MonoBehaviour.Instantiate(new GameObject("Player"));
            stationaryObstacle = MonoBehaviour.Instantiate(TestConstants.GetResource("Stationary Obstacle"));

            player.tag = Tags.PLAYER;
            obstacleSpawner.GetComponent<MatchTransformPosition>().SetTarget(player.transform);
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(animatedObstacle);
            Object.Destroy(obstacleParent);
            Object.Destroy(obstacleSpawner.gameObject);
            Object.Destroy(player);
            Object.Destroy(stationaryObstacle);
        }

        [UnityTest]
        public IEnumerator ShouldRelocateStationaryObstacle()
        {
            Vector3 pos = stationaryObstacle.transform.position;
            stationaryObstacle.transform.position = obstacleSpawner.transform.position;
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.AreNotEqual(pos, stationaryObstacle.transform.position);
        }

        [UnityTest]
        public IEnumerator ShouldRelocateAnimatedObstacle()
        {
            Vector3 pos = animatedObstacle.transform.position;
            animatedObstacle.transform.position = obstacleSpawner.transform.position;
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.AreNotEqual(pos, animatedObstacle.transform.position);
        }
    }
}