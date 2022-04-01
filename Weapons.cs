/* Gun controller created by David Numa for the Tiger Tail project: https://github.com/dnuma
 * This class enables the weapon and plays the shot sound after the user pass through the gun dummy.
 * 
 * Date: 01/04/2022 
 */

using UnityEngine;

namespace TigerTail.FPSController
{
    [DisallowMultipleComponent]
    public class Weapons : MonoBehaviour
    {
        [Tooltip("Audio source"), SerializeField] private AudioSource gunShots;

        public bool isActive = false;
        public void ActivateWeapon() => isActive = true;
        public void PlaySound() => gunShots.Play(); 
    }
}

