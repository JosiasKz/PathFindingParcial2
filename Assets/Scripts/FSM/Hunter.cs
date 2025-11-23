using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

//Tanto Hunter como Boid heredan de SteeringAgent, lo que les permite poder moverse con seek, persuit, evade, etc.
public class Hunter : MonoBehaviour
{
    FiniteStateMachine fsm;
    public float energy;
    public float maxEnergy=100;
    Vector3 lastPosition;
    public Image energyBar;
    public int currentPatrolPoint = 0;
    public bool resting=false;
    public float regenPorSegundo = 0.6f;
    public TextMeshProUGUI stateText;
    public float separationRadius;
    public Animator anim;
        
    // Start is called before the first frame update


    void Start()
    {

        //Inicializamos state machine
        fsm = new FiniteStateMachine(this);
        fsm.AddState(PlayerState.Idle,new IdleState());
        fsm.AddState(PlayerState.Patrol,new Patrolstate());
        fsm.AddState(PlayerState.Hunt,new HuntState());
        //Arranca el idle, y este mismo delegará luego a patrol
        fsm.ChangeState(PlayerState.Idle);
        lastPosition=transform.position;
        energy = maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {        
        fsm.Update();

    }
}
