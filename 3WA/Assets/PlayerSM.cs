using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSM : MonoBehaviour
{
    [SerializeField] PlayerState currentState;

    [SerializeField] float sneakingSpeed = 3;
    [SerializeField] float joggingSpeed = 5;
    [SerializeField] float runningSpeed = 10;

    CharacterController cc;

    Vector3 dirInput;


    public enum PlayerState
    {
        IDLE,
        JOGGING,
        RUNNING,
        FALLING,
        SNEAKING,
        JUMPING
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }
    
    
    void Update()
    {
        OnStateUpdate();
    }


    void OnStateEnter()
    {

        switch (currentState)
        {
            case PlayerState.IDLE:
                break;
            case PlayerState.RUNNING:
                break;
            case PlayerState.JOGGING:

                break;
            case PlayerState.FALLING:
                break;
            case PlayerState.SNEAKING:

                break;
            case PlayerState.JUMPING:
                break;
            default:
                break;
        }
    }


    void OnStateUpdate()
    {
        dirInput  = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

   
        

        //Debug.Log(playreInput);
        //Debug.Log(jumpButton);
        //Debug.Log(runButton);

        switch (currentState)
        {
            case PlayerState.IDLE:
                if (dirInput.magnitude > 0)
                {
                    TransitionToState(PlayerState.JOGGING);
                }


                break;
            case PlayerState.JOGGING:
                cc.Move(dirInput.normalized * joggingSpeed * Time.deltaTime); ;
                

                if (dirInput.magnitude == 0)
                {
                    TransitionToState(PlayerState.IDLE);
                }

                break;
            case PlayerState.RUNNING:
                
                break;
            case PlayerState.FALLING:
                
                break;
            case PlayerState.SNEAKING:
                
                break;
            case PlayerState.JUMPING:
                

                break;
            default:
                break;
        }
    }

    void OnStateExit()
    {
        switch (currentState)
        {
            case PlayerState.IDLE:
                break;
            case PlayerState.RUNNING:
                break;
            case PlayerState.JOGGING:
                break;
            case PlayerState.FALLING:
                break;
            case PlayerState.SNEAKING:
                break;
            case PlayerState.JUMPING:
                break;
            default:
                break;
        }
    }

    void TransitionToState(PlayerState nextState)
    {
        OnStateExit();

        currentState = nextState;

        OnStateEnter();
    }


}
