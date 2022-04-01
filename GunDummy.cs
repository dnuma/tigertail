/* Gun dummy controller created by David Numa for the Tiger Tail project: https://github.com/dnuma
 * This class controls the gun dummies that enables the user to interact with new weapons.
 * The spawn is controlled by the Game Manager.
 * 
 * Date: 01/04/2022 
 */

using UnityEngine;

namespace TigerTail.FPSController
{
    [DisallowMultipleComponent]
    public class GunDummy : MonoBehaviour
    {
        [Tooltip("The weapon gameobject"), SerializeField] private Weapons weapon;
        [Tooltip("Audio source"), SerializeField] private AudioSource dummyAudioSource;

        // Tags
        private const string PLAYER_TAG = "Player";

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PLAYER_TAG))
            {
                // Activate the weapon for the player to use it, deactivate the dummmy
                weapon.ActivateWeapon();
                gameObject.SetActive(false);
            }
                
        }
        public void PlaySound() => dummyAudioSource.Play();
    }
}

