
using UnityEngine;

namespace SkillSystem
{
    public class SkillSwordThrow : SkillBase
    {
        [Header("Trajectory Prediction")] 
        [SerializeField] private GameObject predictionDot;
        [SerializeField] private int numberOfDots = 20;
        [SerializeField] private float spaceBetweenDots = 0.1f;
        [Header("Regular Sword Upgrade")]
        [SerializeField] private GameObject swordThrowPrefab;
        [Range(0f, 20f)]
        [SerializeField] private float regularThrowPower = 6f;
        [Header("Pierce Sword Upgrade")]
        [SerializeField] private GameObject pierceSwordPrefab;
        [SerializeField] public int pierceAmountOfTargets = 2;
        [Range(0f, 40f)]
        [SerializeField] private float pierceThrowPower = 6f;
        [Header("Spin Sword Upgrade")]
        [SerializeField] private GameObject spinSwordPrefab;
        public int maxDistance = 5;
        public float attackPerSecond = 7f;
        public float maxSpeedDuration = 3f;
        [Range(0f, 20f)]
        [SerializeField] private float spinThrowPower = 6f;
        [Header("Bounce Sword Upgrade")]
        [SerializeField] private GameObject bounceSwordPrefab;
        public float bounceSpeed = 12f;
        public int bounceCount = 2;
        [Range(0f, 20f)]
        [SerializeField] private float bounceThrowPower = 6f;

        private SkillObjectSword currentSword;
        private float swordGravity;
        private Transform[] dots;
        private Vector2 confirmedDirection;
        private float currentThrowPower;

        protected override void Awake()
        {
            base.Awake();
            swordGravity = swordThrowPrefab.GetComponent<Rigidbody2D>().gravityScale;
            dots = GenerateDots();
        }

        public override bool CanUseSkill()
        {
            UpdateThrowPower();
            if (currentSword == null) return base.CanUseSkill();
            currentSword.GetSwordBackToPlayer();
            return false;
        }

        public void ThrowSword()
        {
            GameObject swordPrefab = GetSwordPrefab();
            GameObject newSword = Instantiate(swordPrefab, dots[1].position, Quaternion.identity);
            currentSword = newSword.GetComponent<SkillObjectSword>();
            currentSword.SetupSword(this, GetThrowPower());
        }

        public void PredictTrajectory(Vector2 direction)
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].position = GetTrajectoryPoint(direction, i * spaceBetweenDots);
            }
        }

        public void ConfirmTrajectory(Vector2 direction) => confirmedDirection = direction;

        public void EnableDots(bool enable)
        {
            foreach (Transform dot in dots)
            {
                dot.gameObject.SetActive(enable);
            }
        }

        private Transform[] GenerateDots()
        {
            var newDots = new Transform[numberOfDots];
            
            for (int i = 0; i < numberOfDots; i++)
            {
                newDots[i] = Instantiate(
                    predictionDot, 
                    transform.position, 
                    Quaternion.identity, 
                    transform).transform;
                newDots[i].gameObject.SetActive(false);
            }
            return newDots;
        }
        
        private Vector2 GetTrajectoryPoint(Vector2 direction, float t)
        {
            // starting speed and direction of the throw
            Vector2 initVelocity = direction * currentThrowPower;
            // gravity pulls down over time
            Vector2 gravityEffect = Physics2D.gravity * (0.5f * swordGravity * (t * t));
            Vector2 predictedPoint = (initVelocity * t) + gravityEffect;
            Vector2 playerPosition = transform.root.position;
            
            return playerPosition + predictedPoint;
        }

        private Vector2 GetThrowPower() => confirmedDirection * currentThrowPower;

        private void UpdateThrowPower()
        {
            currentThrowPower = upgradeType switch
            {
                SkillUpgradeType.SwordThrow => regularThrowPower,
                SkillUpgradeType.SwordThrowPierce => pierceThrowPower,
                SkillUpgradeType.SwordThrowSpin => spinThrowPower,
                SkillUpgradeType.SwordThrowBounce => bounceThrowPower,
                _ => currentThrowPower
            };
        }

        private GameObject GetSwordPrefab()
        {
            if (UpgradeIsUnlocked(SkillUpgradeType.SwordThrow)) return swordThrowPrefab; 
            if (UpgradeIsUnlocked(SkillUpgradeType.SwordThrowPierce)) return pierceSwordPrefab;
            if (UpgradeIsUnlocked(SkillUpgradeType.SwordThrowSpin)) return spinSwordPrefab;
            if (UpgradeIsUnlocked(SkillUpgradeType.SwordThrowBounce)) return bounceSwordPrefab;
            Debug.Log("No valid throw sword upgrade found");
            return null;
        }
    }
}