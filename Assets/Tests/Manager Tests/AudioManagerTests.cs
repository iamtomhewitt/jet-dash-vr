using Manager;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;
using static Manager.AudioManager;

namespace Tests
{
    public class AudioManagerTests
    {
        private AudioManager am;
        private Sound s;

        [SetUp]
        public void Setup()
        {
            am = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Tests/Managers/Audio Manager")).GetComponent<AudioManager>();
            s = am.GetSound(SoundNames.MUSIC);
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(am.gameObject);
        }

        [UnityTest]
        public IEnumerator ShouldPlaySound()
        {
            am.Play(s.name);
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.True(s.source.isPlaying);
        }

        [UnityTest]
        public IEnumerator ShouldPauseSound()
        {
            am.Play(s.name);
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            am.Pause(s.name);
            Assert.False(s.source.isPlaying);
        }

        [UnityTest]
        public IEnumerator ShouldGetSound()
        {
            Sound s = am.GetSound(SoundNames.BONUS_POINTS);
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.NotNull(s);
        }

        [UnityTest]
        public IEnumerator AllSoundsShouldHaveAnAudioSource()
        {
            Sound[] s = am.GetSounds();
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.NotNull(s);
            foreach (Sound sound in s)
            {
                if (sound.source == null || sound.source.clip == null)
                {
                    Assert.Fail(sound.name + " has a missing AudioSource or audio clip");
                }
            }
        }
    }
}