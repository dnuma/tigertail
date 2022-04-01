/* Gun controller created by David Numa for the Tiger Tail project: https://github.com/dnuma
 * This class controls the zombie gameobjects parameters: speed, type, sounds, particle effects, damage per collision
 * It also decreases the players health in the player.cs class
 * 
 * Date: 01/04/2022 
 */

using System.Collections;
using UnityEngine;

namespace TigerTail.FPSController
{
    [DisallowMultipleComponent]
    public class Zombie : MonoBehaviour
    {
        [Tooltip("Impact Particle Effect"), SerializeField] private ParticleSystem impactParticleEffect;
        [Tooltip("Beam Particle Effect"), SerializeField] private ParticleSystem beamParticleEffect;
        [Tooltip("Spawn Particle Effect"), SerializeField] private SkinnedMeshRenderer zombieMesh;
        [Tooltip("Roar Sound"), SerializeField] private AudioSource roarAudioSource;
        [Tooltip("Roar Clip 1"), SerializeField] private AudioClip roarClip1;
        [Tooltip("Roar Clip 2"), SerializeField] private AudioClip roarClip2;
        [Tooltip("Eat Clip"), SerializeField] private AudioClip attackClip;
        [Tooltip("Animation clips"), SerializeField] public AnimationClip[] zombieAnimationClip;

        public enum TypeOfZombie
        {
            Walker,
            Runner,
            Idle
        }

        [Tooltip("Select a type of zombie"), SerializeField] public TypeOfZombie typeOfZombie;

        /// <summary> Used to start/stop moving the zombie </summary>
        public bool isAlive;
        /// <summary> Used to run the dead method once </summary>
        public bool isSpawned;
        
        private GameObject player;

        // Zombie speed
        private float speed;
        private readonly float walking = 0.7f;
        private readonly float running = 2.3f;
        // Zombie audio speed
        private readonly float slowRoar = 1f;
        private readonly float fastRoar = 1.7f;
        private readonly float attackRoar = 1f;
        // Zombie damage
        private readonly int zombieDmg = 5;

        // Animator triggers
        private const string ATTACK = "Start Attacking";
        private const string WALK = "Start Walking";
        private const string RUN = "Start Running";
        private const string STOP = "Stop";

        //Stop state animatios
        private const int STOP_ANIM = 4;

        // Tags
        private const string PLAYER_TAG = "Player";
        private const string SNOWBALL_TAG = "ThrowSB";

        private void Awake()
        {
            isAlive = true;
            isSpawned = false;
        }

        private void Start()
        {
            SetupZombie(true);

        }

        void Update()
        {
            if (isAlive)
            {
                transform.LookAt(player.transform.position);
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                isSpawned = true;
            } 
            else
            {
                // Run this once
                if (isSpawned)
                {
                    // Randomly select any stop state
                    gameObject.GetComponent<Animator>().SetInteger(STOP, Random.Range(0, STOP_ANIM));
                    roarAudioSource.Stop();
                    isSpawned = false;
                }

            }

            // Stop the animations
            if(player.GetComponent<Player>().isDead)
                isAlive = false;
        }

        /// <summary> Disable the zombie when a snowball hit it or start the attacking animation </summary>
        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.CompareTag(SNOWBALL_TAG))
            {
                SetupZombie(false);
                impactParticleEffect.Play();
                beamParticleEffect.Play();
                roarAudioSource.Stop();
                StartCoroutine(DestroyEnemy());
            } else if (collision.gameObject.CompareTag(PLAYER_TAG))
            {
                gameObject.GetComponent<Animator>().SetTrigger(ATTACK);
                roarAudioSource.clip = attackClip;
                roarAudioSource.pitch = attackRoar;
                roarAudioSource.Play();
                Player player = collision.gameObject.GetComponent<Player>();
                player.DecreaseHealth(zombieDmg);
            }                
        }

        /// <summary> Start the animation of the movement again </summary>
        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag(PLAYER_TAG))
            {
                StartTheZombie();
            }
        }

        /// <summary> Destroy the zombie in 2 seconds </summary>
        IEnumerator DestroyEnemy()
        {
            GameManager.score++;
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
        }

        /// <summary> Enable the zombie parameters (collider, bools, etc.) </summary>
        public void SetupZombie(bool state)
        {
            var collider = GetComponent<Collider>();
            collider.enabled = state;
            isAlive = state;
            zombieMesh.enabled = state;

            // If 'state' is activating a new zombie, generate a new type of zombie
            if(state)
            {
                player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
                var temp = Random.Range(0, 2);
                if(temp == 0)
                    typeOfZombie = TypeOfZombie.Walker;
                else
                    typeOfZombie = TypeOfZombie.Runner;
                
                StartTheZombie();
            }
        }

        /// <summary> Start the animation of the spawned zombie </summary>
        private void StartTheZombie()
        {
            switch (typeOfZombie)
            {
                case TypeOfZombie.Walker:
                    gameObject.GetComponent<Animator>().SetTrigger(WALK);
                    speed = walking;
                    roarAudioSource.clip = roarClip1;
                    roarAudioSource.pitch = slowRoar;                    
                    break;
                case TypeOfZombie.Runner:
                    gameObject.GetComponent<Animator>().SetTrigger(RUN);
                    speed = running;
                    roarAudioSource.clip = roarClip2;
                    roarAudioSource.pitch = fastRoar;
                    break;
            }
            roarAudioSource.Play();
        }
    }
}