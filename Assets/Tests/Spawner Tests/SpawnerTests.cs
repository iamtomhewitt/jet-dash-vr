using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;
using Utility;

namespace Tests
{
    public class SpawnerTests
    {
        private GameObject obstacle;
        private GameObject obstacleSpawner;
        private GameObject obstacleParent;
        private GameObject player;

        [SetUp]
        public void Setup()
        {
            obstacle = MonoBehaviour.Instantiate(TestConstants.GetResource("Obstacle"));
            obstacleSpawner = MonoBehaviour.Instantiate(TestConstants.GetResource("Spawners/Obstacle Spawner"));
            obstacleParent = MonoBehaviour.Instantiate(new GameObject("Obstacles"));
            player = MonoBehaviour.Instantiate(new GameObject("Player"));

            player.tag = Tags.PLAYER;
            obstacleSpawner.GetComponent<MatchTransformPosition>().SetTarget(player.transform);
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(obstacle);
            Object.Destroy(obstacleSpawner.gameObject);
            Object.Destroy(obstacleParent);
            Object.Destroy(player);
        }

        [UnityTest]
        public IEnumerator ShouldRelocateObstacle()
        {
            Vector3 pos = obstacle.transform.position;
            obstacle.transform.position = obstacleSpawner.transform.position;
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.AreNotEqual(pos, obstacle.transform.position);
        }
    }
}