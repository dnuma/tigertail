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
 * NOTE: not used at the moment since the ice was removed in this version of the game.
 * 
 * Date: 01/04/2022 
 */

using TigerTail.FPSController;
using UnityEngine;

namespace TigerTail
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public class Ice : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out FPSMovement movement))
            {
                movement.IsSliding = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out FPSMovement movement))
            {
                movement.IsSliding = false;
            }
        }
    }
}
