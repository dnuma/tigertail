/*This source code was originally provided by the Digital Simulations Lab (VARLab) at Conestoga College in Ontario, Canada.
 * The source code was modified by David Numa for the Tiger Tail project: https://github.com/dnuma
 * It was provided as a foundation of learning for participants of our 2022 Introduction to Unity Boot Camp.
 * Participants are welcome to use, extend and share projects derived from this code under the Creative Commons Attribution-NonCommercial 4.0 International license as linked below:
        Summary: https://creativecommons.org/licenses/by-nc/4.0/
        Full: https://creativecommons.org/licenses/by-nc/4.0/legalcode
 * You may not sell works derived from this code, but we hope you learn from it and share that learning with others.
 * We hope it inspires you to make more games or consider a career in game development.
 * To learn more about the opportunities for computer science and software engineering at Conestoga College please visit https://www.conestogac.on.ca/applied-computer-science-and-information-technology 
 * 
 * Date: 01/04/2022 
 */

using UnityEngine;

namespace TigerTail
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class ThrowableSnowball : MonoBehaviour
    {
        [Tooltip("Prefab for the particle effect to play when this snowball impacts after throwing.")]
        [SerializeField] private GameObject impactEffectPrefab;

        // Tags
        private const string PLAYER_TAG = "Player";

        private void Update()
        {
            // Disable the snowball if it goes beyond limits
            if(transform.position.y < -50f)
                DisableSnowball();
        }
        /// <summary> Disable the snowball if it hits anything but the player </summary>
        private void OnCollisionEnter(Collision collision)
        {
            if(!collision.gameObject.CompareTag(PLAYER_TAG))
            {
                DisableSnowball();
            }
        }

        /// <summary> Disable the pooled snowball, enable the particle effect and reset velocity </summary>
        private void DisableSnowball()
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            var rb = gameObject.GetComponent<Rigidbody>().velocity;
            rb.x = 0;
            rb.y = 0;
            rb.z = 0;
            gameObject.GetComponent<Rigidbody>().velocity = rb;
            gameObject.SetActive(false);
        }
    }
}
