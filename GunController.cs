/* Gun controller created by David Numa for the Tiger Tail project: https://github.com/dnuma
 * This class controls the swapping gun animation.
 * 
 * Date: 01/04/2022 
 */

using UnityEngine;

namespace TigerTail.FPSController
{
    [DisallowMultipleComponent]
    public class GunController : MonoBehaviour
    {
       [Tooltip("Game Manager"), SerializeField] private Player player;
       public void SwapEnded() => player.SwappingGun = false;
       public void SwapStarted() => player.SwappingGun = true;
    }
}
