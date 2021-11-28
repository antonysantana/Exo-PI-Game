using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AprendaUnity;


public class EnemyAI : MonoBehaviour
{
    private GameManager _GameManager;
    private fov _fov;
    public enemyState curretState;
    private Animator anim;
    private NavMeshAgent agent;

    [Header("PatrolWayPoints")]
    public bool isWaitWayPoint;
    public int idWayPoints;
    public Transform[] waypoints;
    private Transform target;
    private Vector3 lookInto; 
    private bool isEndPatrol;
    private bool isWaitPatrol;

    [Header("Ray")]
    private RayCastEnemy weapon;

    // Start is called before the first frame update
    void Start()
    {
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        _fov = FindObjectOfType(typeof(fov)) as fov;
        weapon = GetComponentInChildren<RayCastEnemy>();

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        SetDestinationAgent(transform.position);

        OnStateEnter(curretState);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemyState(); // ATUALIZA O MÉTODO QUE CONTROLA OS ESTADOS DO INIMIGO
    }

    #region MEUS MÉTODOS

    private void SetDestinationAgent(Vector3 destination)
    {
        agent.destination = destination;
    }

    void OnStateEnter(enemyState newEnemyState)
    {
        StopAllCoroutines(); // PARAR TODAS AS CORROUTINES
        curretState = newEnemyState;
        switch (curretState)
        {
            case enemyState.IDLE:

                StartCoroutine("Idle");
                
                break;

            case enemyState.PATROL:
                //StartCoroutine("Patrol");
                /*idWayPoints = 1;
                isEndPatrol = false;*/
                SetDestinationAgent(waypoints[idWayPoints].position);
                agent.stoppingDistance = 0.5f;
                
                break;

            case enemyState.ALERT:
                SetDestinationAgent(transform.position);
                target = _fov.visibleTargetsB[0];
                lookInto = target.position;
                StartCoroutine("Alert");

                break;

            case enemyState.COMBAT:
                //SetDestinationAgent(transform.position);
                agent.stoppingDistance = 8f;
                weapon.raycastDestination = target;
                StartCoroutine("Combat");
                //SetDestinationAgent(_fov.visibleTargets[0].position);
                break;
        }
    }

    void UpdateEnemyState()
    {
        if(agent.desiredVelocity.magnitude != 0)
        {
            anim.SetInteger("idAnimation", 1);
        }
        else
        {
            anim.SetInteger("idAnimation", 0);
        }

        switch (curretState)
        {
            case enemyState.PATROL:

                if(agent.remainingDistance <= agent.stoppingDistance && isEndPatrol == false && isWaitPatrol == false)
                {
                    if (isWaitWayPoint == true)
                    {
                        StartCoroutine("Patrol");
                    }
                    else
                    {
                        SetNewDestination();
                    }
                }
                else if (agent.remainingDistance <= agent.stoppingDistance && isEndPatrol == true)
                {
                    OnStateEnter(enemyState.IDLE);
                    print("CHAMOU IDLE");
                }

                break;

            case enemyState.FOLLOW:

                break;

            case enemyState.COMBAT:
                SetDestinationAgent(target.position);

                break;

        }
    }

    

    private void SetNewDestination()
    {
        idWayPoints = (idWayPoints + 1) % waypoints.Length;
        SetDestinationAgent(waypoints[idWayPoints].position);
        
        if (idWayPoints == 0)
        {
            isEndPatrol = true;
            agent.stoppingDistance = 0;
        }
    }

    private void IsVisible(viewState vState)
    {
        switch (vState)
        {
            case viewState.Primary:
                if (curretState != enemyState.COMBAT)
                {
                    //OnStateEnter(enemyState.COMBAT);
                }
                    
                break;

            case viewState.Secondary:
                if (curretState != enemyState.ALERT && curretState != enemyState.COMBAT)
                {
                    OnStateEnter(enemyState.ALERT);
                }
                break;

        }
    }

    IEnumerator Idle()
    {
        SetDestinationAgent(transform.position);
        yield return new WaitForSeconds(_GameManager.idleWaitTime); // AGUARDA O TEMPO DE ESPERA PARADO

        if (_GameManager.randomSystem(_GameManager.percPatrol))
        {
            idWayPoints = 1;
            isEndPatrol = false;
            OnStateEnter(enemyState.PATROL);
        }
        else
        {
            OnStateEnter(enemyState.IDLE);
        }
    }

    IEnumerator Patrol()
    {
        isWaitPatrol = true;
        yield return new WaitForSeconds(_GameManager.patrolWaitTime);
        SetNewDestination();
        isWaitPatrol = false;
    }

    IEnumerator Combat()
    {
        //anim.SetInteger("idAnimation", 4);
        while (true)
        {

            yield return new WaitForSeconds(0.3f);
            weapon.StartFiring();
        }
    }

    IEnumerator Alert()
    {
        yield return new WaitForSeconds(_GameManager.alertWaitTime);
        agent.stoppingDistance = 8;
        SetDestinationAgent(lookInto);
        yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);

        if(_fov.visibleTargetsB.Count > 0)
        {
            OnStateEnter(enemyState.COMBAT);
        }
        else
        {
            yield return new WaitForSeconds(_GameManager.patrolWaitTime);
            agent.stoppingDistance = 5;
            yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);
            yield return new WaitForSeconds(_GameManager.patrolWaitTime);
            OnStateEnter(enemyState.PATROL);
        }
        //yield return new WaitForSeconds(_GameManager.patrolWaitTime);
        
    }
        
    }

    #endregion
