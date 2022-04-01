/* Object Pooler created by David Numa for the Tiger Tail project: https://github.com/dnuma
 * This class controls the animation of the voyager (space ship) and its particle effects
 * 
 * Date: 01/04/2022 
 */

using System.Collections;
using UnityEngine;

namespace TigerTail.FPSController
{
    [DisallowMultipleComponent]

    
    public class Voyager : MonoBehaviour
    {
        [Tooltip("Ship"), SerializeField] private GameObject ship;
        [Tooltip("Game Manager"), SerializeField] private GameManager gameManager;
        [Tooltip("Portal"), SerializeField] private ParticleSystem[] portal;

        // Animator triggers
        private const string TO_UNLOAD = "To Unload";
        private const string TO_BLACKHOLE = "To Blackhole";

        public bool readyToUnload = false;
        public void EnableVoyager() => ship.SetActive(true);
        public void ToUnload() => GetComponent<Animator>().SetTrigger(TO_UNLOAD);
        public void ToBlackhole() => GetComponent<Animator>().SetTrigger(TO_BLACKHOLE);
        public void ReadyToUnload() => gameManager.startSpawning = true;
        public void BatchUnloaded() => gameManager.startSpawning = false;
        public void StartPortalAnim() { foreach (ParticleSystem p in portal) { p.Play(); } }

        /// <summary> Disable the ship, increase user lvl and resummon it again </summary>
        public void DisableVoyager()
        {
            ship.SetActive(false);
            StartCoroutine(ResendShip());
        }
        IEnumerator ResendShip()
        {
            yield return new WaitForSeconds(5f);
            gameManager.IncreaseUserLevel();
            ToUnload();
        }
    }
}

