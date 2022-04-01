/* Game Manager created by David Numa for the Tiger Tail project: https://github.com/dnuma
 * This class spawns elements in the hieararchy (health kit, dummy guns, texts) and activate the pooled zombie gameobjects.
 * The spawn is controlled by the Game Manager.
 * 
 * Date: 01/04/2022 
 */

using System.Collections;
using TMPro;
using UnityEngine;

namespace TigerTail.FPSController
{
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        [Tooltip("Zombie spawn positions"), SerializeField] private Transform[] zombiesSpawnPos;
        [Tooltip("Score Text Mesh Pro"), SerializeField] private TextMeshProUGUI scoreText;
        [Tooltip("Intro Text Mesh Pro"), SerializeField] private TextMeshProUGUI announcementText;
        [Tooltip("User Level Text Mesh Pro"), SerializeField] private TextMeshProUGUI roundText;
        [Tooltip("Health Text Mesh Pro"), SerializeField] private TextMeshProUGUI healthText;
        [Tooltip("The Zombies"), SerializeField] private Zombie zombie;
        [Tooltip("The voyager"), SerializeField] private Voyager voyager;
        [Tooltip("The weapon dummies"), SerializeField] private GameObject[] weaponDummies;
        [Tooltip("The health pack dummie"), SerializeField] private GameObject healthPack;
        [Tooltip("The player"), SerializeField] private Player player;
        

        // Pool class for the zombies
        public ObjectPool objectPool;

        // General score
        public static int score;

        // Used to detect if the user have pressed the 'N' key
        private bool gameStarted = false;

        private string introText =
            "You're stuck on a really cold moon and the only way to defeat all the astronaut zombies is with... " +
            "snowballs! (it's a kid friendly game, what do you expect?)\n\nControls:" +
            "\n * AWSD to walk\n * Mouse to look around and shoot\n * Left Shift to run\n * Space bar to jump" +
            "\n * Number keys to change weapons(when you earn them!)\n\n" +
            "Press 'N' to start the game...and remember this spawn location, you'll need it later.";

        private string newWeaponText = "New weapon available at spawn location!";

        private string newHealthText = "Health recovery pack available!";

        private string gameOverText = "Game Over!";

        // Index of the spawn position
        private int spawnIndex;
        // Max size of the spawn position
        private int maxSpawnPos;
        // Used to detect if the ship can start dropping zombies
        public bool startSpawning = false;
        // Counter for the zombie spawn
        private float startTime;
        // Counter for the weapon annoucments
        private float weaponTimer;
        // Counter for the health annoucements
        private float healthTimer;
        // Weapon index
        private int weaponIndex;    
        // Use to detect if all weapons have been enabled
        private bool allWeaponsSpawned = false;

        public bool gameOver;

        public int userLevel;
        public int IncreaseUserLevel() => userLevel++;

        private void Awake()
        {
            score = 0;
            startTime = 0f;
            healthTimer = 0f;
            weaponTimer = 0f;
            weaponIndex = 0;
            userLevel = 1;
            announcementText.text = introText;
            announcementText.gameObject.SetActive(true);
            scoreText.gameObject.SetActive(false);
            roundText.gameObject.SetActive(false);
            healthText.gameObject.SetActive(false);
            spawnIndex = 0;
            maxSpawnPos = zombiesSpawnPos.Length;
            gameOver = false;
        }

        private void Update()
        {
            
            // Press N to start the game
            if (!gameStarted && !gameOver)
            {
                if (Input.GetKey(KeyCode.N))
                {
                    gameStarted = true;
                    voyager.ToUnload();
                    announcementText.gameObject.SetActive(false);
                    scoreText.gameObject.SetActive(true);
                    roundText.gameObject.SetActive(true);
                    healthText.gameObject.SetActive(true );
                }
            }

            if (gameStarted && !gameOver)
            {
                scoreText.text = "Score: " + score;
                roundText.text = "Horde: " + userLevel;
                healthText.text = "Health:" + player.health;

                // Reset the spawn position of the ship
                if (spawnIndex > 1 || !startSpawning)
                    spawnIndex = 0;

                //Spawn zombies each 1 seconds
                if (Time.time - startTime > 1f && startSpawning)
                {
                    startTime = Time.time;
                    for (int i = 0; i < userLevel; i++)
                    {
                        ActivateZombie(zombiesSpawnPos[spawnIndex]);
                        // Change spawn position, reset if necesary
                        spawnIndex++;
                        if (spawnIndex >= maxSpawnPos)
                            spawnIndex = 0;
                    }
                }

                // Enable dummies and send annoucenments in-game
                if(Time.time - weaponTimer > 90f && !allWeaponsSpawned)
                {
                    EnableNewWeapon();
                    SendAnnoucement(true);
                    weaponTimer = Time.time;
                }
                if (Time.time - healthTimer > 60f)
                {
                    EnableNewHealthPack();
                    SendAnnoucement(true);
                    healthTimer = Time.time;
                }
            }


            // If the player dies, stop everything and announce it
            if (player.health <= 0)
            {
                gameOver = true;
                gameStarted = false;
                announcementText.text = gameOverText;
                SendAnnoucement(false);
                player.GetComponent<Collider>().enabled = false;
            }
        }

        /// <summary> Enable new weapon and send annoucement </summary>
        private void EnableNewWeapon()
        {
            announcementText.text = newWeaponText;
            weaponDummies[weaponIndex].gameObject.SetActive(true);
            weaponDummies[weaponIndex].gameObject.GetComponent<GunDummy>().PlaySound();
            weaponIndex++;
            
            if (weaponIndex >= weaponDummies.Length)
                allWeaponsSpawned = true; // stop spawning weapons
        }

        /// <summary> Enable new health and send annoucement </summary>
        private void EnableNewHealthPack()
        {
            announcementText.text = newHealthText;
            healthPack.gameObject.SetActive(true);
            healthPack.GetComponent<FirstAidKit>().PlaySound();
        }

        /// <summary> Send the annoucement to the user in the middle of the screen </summary>
        private void SendAnnoucement(bool startTimer)
        {
            announcementText.gameObject.SetActive(true);
            // Set the text on the center of the screen
            announcementText.alignment = TextAlignmentOptions.Center;
            announcementText.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            if(startTimer)
                StartCoroutine(WaitShortDuration());
        }

        IEnumerator WaitShortDuration()
        {
            yield return new WaitForSeconds(6f);
            announcementText.gameObject.SetActive(false);
        }

        /// <summary> Activate the pooled zombie in <param name="t">t location</param> </summary>
        private void ActivateZombie(Transform t)
        {
            GameObject zombiePooled = objectPool.GetPooledObject();
            if (zombiePooled != null)
            {
                // Reset zombie position and rotation before activating it
                zombiePooled.transform.position = t.position;
                zombiePooled.transform.rotation = Quaternion.identity;
                zombiePooled.SetActive(true);
                zombiePooled.GetComponent<Zombie>().SetupZombie(true);
            }
        }
    }
}