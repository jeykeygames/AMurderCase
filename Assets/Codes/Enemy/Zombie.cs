using UnityEngine;
using UnityEngine.AI;

namespace Assets.Codes.Enemy
{
    public enum AIState
    {
        Idle,
        Running,
        Attacking,
        Raging
    }

    [RequireComponent(typeof(HealthSystem))]
    public class Zombie : MonoBehaviour
    {
        [SerializeField]
        private float movementSpeed = 1.0f;
        [SerializeField]
        private AIState aiState;

        private HealthSystem healthSystem;
        private NavMeshAgent aiAgent;
        private Animator animator;
        private Vector3 destination;
        private bool isMoving = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            healthSystem = GetComponent<HealthSystem>();
            aiAgent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();

            aiState = AIState.Idle;
        }

        // Update is called once per frame
        void Update()
        {
            AIStateMachine();
            UpdateAnimator();
        }

        private void AIStateMachine()
        {
            switch (aiState)
            {
                case AIState.Idle:
                    break;
                case AIState.Running:
                    destination = CalculateDestination();
                    Move();
                    break;
                case AIState.Attacking:
                    break;
                case AIState.Raging:
                    break;
            }
        }

        private void Move()
        {
            if (CanMove())
            {
                aiAgent.speed = movementSpeed;
                aiAgent.SetDestination(destination);
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
        }

        private bool CanMove()
        {
            return aiAgent != null;
        }

        private Vector3 CalculateDestination()
        {
            return aiAgent.destination;
        }

        private void UpdateAnimator()
        {
            animator.SetBool("IsRunning", isMoving);
        }

        private void Attack()
        {
            animator.SetTrigger("Attack");
        }

        private void Die()
        {
            animator.SetTrigger("Die");
        }

        private void Rage()
        {
            animator.SetTrigger("Rage");
        }
    }
}