/* First Aid Kit controller created by David Numa for the Tiger Tail project: https://github.com/dnuma
 * This class controls the health kit that spawns in the game, healing the player.
 * The spawn is controlled by the Game Manager.
 * 
 * Date: 01/04/2022 
 */

using UnityEngine;

namespace TigerTail.FPSController
{
    [DisallowMultipleComponent]
    public class FirstAidKit : MonoBehaviour
    {
        [Tooltip("Player Class"), SerializeField] private Player player;
        [Tooltip("Audio source"), SerializeField] private AudioSource dummyAudioSource;

        // Tags
        private const string PLAYER_TAG = "Player";

        private int HealthInc = 30;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PLAYER_TAG))
            {
                player.IncreaseHealth(HealthInc);
                gameObject.SetActive(false);
            }
        }

        public void PlaySound() => dummyAudioSource.Play();
    }
}

