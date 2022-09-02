using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerSM : MonoBehaviour
{
    [SerializeField] PlayerState currentState;
    [SerializeField] LayerMask groundLayer;


    [SerializeField] float sneakingSpeed = 3;
    [SerializeField] float joggingSpeed = 5;
    [SerializeField] float runningSpeed = 10;

    [SerializeField] Camera mainCam;

    [SerializeField] Transform graphics;

    [SerializeField] Animator animator;

    CharacterController cc;


    Vector3 dirInput;
    Vector3 dirCam;
    Vector3 dirMove;

    float lastSpeed;
    float currentVelocity;
    float finalAngle;
    float velocityY;

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
        Cursor.lockState = CursorLockMode.Locked;
        
        cc = GetComponent<CharacterController>();

        currentState = PlayerState.IDLE;
        OnStateEnter();

    }
    
    
    void Update()
    {
        //if(currentState != PlayerState.FALLING)
        dirInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));


        MovementDirection();


        OnStateUpdate();



    }


    void OnStateEnter()
    {

        switch (currentState)
        {
            case PlayerState.IDLE:
                animator.SetBool("IDLE", true);
                break;
            case PlayerState.RUNNING:

                lastSpeed = runningSpeed;
                animator.SetBool("RUN", true);
                break;
            case PlayerState.JOGGING:
                lastSpeed = joggingSpeed;
                animator.SetBool("JOGGING", true);

                break;
            case PlayerState.FALLING:
                animator.SetBool("FALLING", true);
                break;
            case PlayerState.SNEAKING:
                lastSpeed = sneakingSpeed;
                animator.SetBool("SNEAKING", true);
                break;
            case PlayerState.JUMPING:
                animator.SetBool("JUMPING", true);
                break;
            default:
                break;
        }
    }

    void OnStateUpdate()
    {

        // TO FALLING
        if (!IsGrounded() && currentState != PlayerState.FALLING)
            TransitionToState(PlayerState.FALLING);

        

        switch (currentState)
        {
            case PlayerState.IDLE:
                if (dirInput.magnitude > 0 && !Input.GetKey(KeyCode.LeftShift))
                {
                    TransitionToState(PlayerState.JOGGING);
                }

                if (dirInput.magnitude > 0 && Input.GetKey(KeyCode.LeftShift))
                {
                    TransitionToState(PlayerState.RUNNING);
                }

                // TO JUMP
                if (Input.GetKeyDown(KeyCode.Space))
                    TransitionToState(PlayerState.JUMPING);

                break;
            case PlayerState.JOGGING:

                cc.Move(dirMove * joggingSpeed * Time.deltaTime);


                if (dirInput.magnitude == 0)
                {
                    TransitionToState(PlayerState.IDLE);
                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    TransitionToState(PlayerState.RUNNING);
                }

                // TO JUMP
                if (Input.GetKeyDown(KeyCode.Space))
                    TransitionToState(PlayerState.JUMPING);

                break;
            case PlayerState.RUNNING:

                cc.Move(dirMove * runningSpeed * Time.deltaTime);


                if (dirInput.magnitude == 0)
                {
                    TransitionToState(PlayerState.IDLE);
                }

                if(!Input.GetKey(KeyCode.LeftShift))
                    TransitionToState(PlayerState.JOGGING);

                // TO JUMP
                if (Input.GetKeyDown(KeyCode.Space))
                    TransitionToState(PlayerState.JUMPING);


                break;
            case PlayerState.FALLING:

                velocityY += Physics.gravity.y* Time.deltaTime;

                cc.Move(((dirMove * lastSpeed) + new Vector3(0, velocityY, 0)) * Time.deltaTime);

                if (IsGrounded() && dirInput.magnitude == 0)
                    TransitionToState(PlayerState.IDLE);
                if (IsGrounded() && dirInput.magnitude > 0)
                    TransitionToState(PlayerState.JOGGING);

                break;
            case PlayerState.SNEAKING:

                break;
            case PlayerState.JUMPING:

                velocityY = Mathf.Sqrt(-2 * 5 * Physics.gravity.y);

                cc.Move(((dirMove * lastSpeed) + new Vector3(0, velocityY, 0)) * Time.deltaTime);

                break;
            default:
                break;
        }


        

    }

    private void MovementDirection()
    {
        if (dirInput.magnitude > 0)
        {
            // CALCUL DE L'ANGLE QUE DOIT AVOIR LE PERSONNAGE
            float angle = Mathf.Atan2(dirInput.x, dirInput.z) * Mathf.Rad2Deg; // ANGLE REPRESENTER PAR MES TOUCHES DIRECTIONNELLES
            angle += mainCam.transform.eulerAngles.y; // ON RAJOUTE L'ANGLE DE LA CAMERA


            // SMOOTH DU RESULTAT
            finalAngle = Mathf.SmoothDampAngle(finalAngle, angle, ref currentVelocity, .1f);


            // ON APPLIQUE LE RESULTAT AU TRANSFORM
            transform.eulerAngles = new Vector3(0, finalAngle, 0); // ON TOURNE LE PERSONNAGE

            // ON RECUPERE LA DIRECTION DANS LAQUELLE DOIT BOUGER LE PERSONNAGE
            dirMove = Quaternion.Euler(0, transform.eulerAngles.y, 0) * Vector3.forward; // ROTATION TO DIRECTION


            
        }
    }

    void OnStateExit()
    {
        switch (currentState)
        {
            case PlayerState.IDLE:
                animator.SetBool("IDLE", false);
                break;
            case PlayerState.RUNNING:
                animator.SetBool("RUN", false);
                break;
            case PlayerState.JOGGING:
                animator.SetBool("JOGGING", false);

                break;
            case PlayerState.FALLING:
                animator.SetBool("FALLING", false);
                velocityY = 0;
                break;
            case PlayerState.SNEAKING:
                animator.SetBool("SNEAKING", false);
                break;
            case PlayerState.JUMPING:
                animator.SetBool("JUMPING", false);
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

    bool IsGrounded()
    {
        Collider[] col = Physics.OverlapBox(transform.position, new Vector3(.3f, 0.05f, .3f), transform.rotation, groundLayer);
        return col.Length > 0;
    }

}
