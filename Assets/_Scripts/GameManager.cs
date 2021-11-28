using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyState
{
    IDLE, PATROL, FOLLOW, ALERT, COMBAT
}

public class GameManager : MonoBehaviour
{
    [Header("Enemy AI Config")]
    public float idleWaitTime;
    public int percPatrol; // qual a chance de entrar em patrulha
    public float patrolWaitTime; // tempo de espera em cada waipoint, caso ele pare
    public float alertWaitTime; // tempo que ele fica no estagio de alerta antes de seguir adiante

    
    // QUAL A CHANCE DE ENTRAR EM PATRULHA


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region MEUS METODOS

    public bool randomSystem(int perc)
    {
        int rand = Random.Range(0, 100);
        bool retorno = rand <= perc ? true : false;
        return retorno;
    }

    #endregion

}
