using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    enum SoundFXIndex
    {
        UNDERWATERSWIM = 0,
        FLOATINGSWIM = 1,
        SANDWALKING = 2,
        WOODWALKING = 3,
        STONEWALKING = 4,
    };


    public float Speed = 5.0f;
    public float RotationSpeed = 5.0f;
    public float JumpForce = 6.2f;
    public float WaterDrag = 0f;
    public float FloatingForce = 30.0f;
    public float GroundDistance = 0.2f;
    public float MaxVelocity = 5.0f;
    public float UnderWaterHealthDamage = 0.12f;
    public float UnderWaterAirMeterDamage = 0.08f;
    public float AirMeterRecovery = 1.2f;
    public float UnderWaterSoundPitch = -0.2f;
    public float UnderWaterVolume = 0.3f;
    public float SwimSoundFXDelay = 2.5f;
    public float SharkDamage = 15.0f;

    private float PlayerHealth = 100f;
    private float PlayerAir = 100f;
    public Slider playerHealthMeter;
    public Slider playerAirMeter;
    public GameObject BubbleEffect;
    public AudioSource OceanAudioSource;

    private AudioSource PlayerAudioSource;
    public AudioClip[]  PlayerAudioClips;

    public CapsuleCollider GroundTrigger;
    private Rigidbody m_RigidBody;
    private Vector3 m_Input;   
    private bool m_isGrounded = true; 
    private bool m_isFloating = false;
    private bool m_isJumping  = false;    
    private bool m_onWaterSurface = false;
    private bool m_PlaySwimSound = true;
    private bool m_PlaySandWalkSound = true;
   /// private bool m_PlayWoodWalkSound = true;
    
    private AnimationController animController;
    private CustomTypes.AnimationStateController currentState;
    private UnderWaterProfileController waterBehaviourScript;
    private Transform PlayerCameraTransform;

    public  Transform CameraPosition;   
    
    public TerrainSurface TerrainInformationScript;

    public GameManager GameManagerScript;

    void Start()
    {
        m_Input = Vector3.zero;
        m_RigidBody = GetComponent<Rigidbody>();
        animController = GetComponent<AnimationController>();
        currentState = CustomTypes.AnimationStateController.IDLE;
        waterBehaviourScript = FindObjectOfType<UnderWaterProfileController>();
        PlayerAudioSource = GetComponent<AudioSource>();
        PlayerCameraTransform = this.transform.Find("Main Camera");       
    }

    void Update()
    {
       UpdateAnimationState();   

       if(PlayerHealth <= 0.0f)
        {
            GameManagerScript.LoadScene(2);
        }
    }

    void FixedUpdate()
    {
        if (!GroundTrigger.enabled)
            Invoke("ActivateTrigger", 0.5f);       

        //ActivateTrigger();
        InputCheck();
        Jump();
        PlayerMovement();
        FloatOnWater();

        m_RigidBody.velocity = Vector3.ClampMagnitude(m_RigidBody.velocity, MaxVelocity);
    }

    void InputCheck()
    {
        m_Input = Vector3.zero;
        m_Input.x = Input.GetAxis("Horizontal") * (RotationSpeed - WaterDrag);
        m_Input.z = Input.GetAxis("Vertical") * (Speed - WaterDrag);
    }

    void PlayerMovement()
    {
        //Rotation Movement
        Vector3 m_EulerAngleVelocity = new Vector3(0, m_Input.x, 0);
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity);
        m_RigidBody.MoveRotation(m_RigidBody.rotation * deltaRotation);

        //Forward Movement
        Vector3 forward = m_Input.z * transform.forward;
        forward.y = m_RigidBody.velocity.y;
        m_RigidBody.velocity = forward;
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && m_isGrounded)
        {
            GroundTrigger.enabled = false;
            m_isGrounded = false;
            m_isJumping = true;
            MaxVelocity = 6.5f;
            m_RigidBody.AddForce(Vector3.up * JumpForce, ForceMode.VelocityChange);           
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_isFloating)
        {
            m_isFloating = false;
            m_onWaterSurface = false;
            m_isJumping = true;
            m_RigidBody.AddForce(Vector3.up * JumpForce, ForceMode.VelocityChange);
            m_RigidBody.useGravity = true;            
        }
    }

    void FloatOnWater()
    {
        if (m_isFloating)
        {                    
            if (Input.GetKey(KeyCode.LeftShift))
            {
                m_isFloating = false;
                m_RigidBody.useGravity = true;           
            }
            else
            {
                m_RigidBody.useGravity = false;
                m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0.0f, m_RigidBody.velocity.z);
            }

            if (m_Input != Vector3.zero && m_PlaySwimSound)
            {
                m_PlaySwimSound = false;
                PlayerAudioSource.volume = 0.292f;
                PlayerAudioSource.clip = PlayerAudioClips[(int)SoundFXIndex.FLOATINGSWIM];
                PlayerAudioSource.Play();
                Invoke("SwimSoundDelay", SwimSoundFXDelay);
            }
        }

        if (waterBehaviourScript.IsUnderWater())
        {
            GameManagerScript.SetTipsText(0);
            m_onWaterSurface = true;
            m_isGrounded = false;
            m_isJumping = false;

            if(m_Input != Vector3.zero && m_PlaySwimSound)
            {
                m_PlaySwimSound = false;
                PlayerAudioSource.volume = 0.292f;
                PlayerAudioSource.clip = PlayerAudioClips[(int)SoundFXIndex.UNDERWATERSWIM];
                PlayerAudioSource.Play();
                Invoke("SwimSoundDelay", SwimSoundFXDelay);
            }

            BubbleEffect.SetActive(true);

            OceanAudioSource.pitch = UnderWaterSoundPitch;
            OceanAudioSource.volume = UnderWaterVolume;

            if (PlayerAir > 0.0f)
            {
                PlayerAir -= UnderWaterAirMeterDamage;
                playerAirMeter.value = PlayerAir;
            }
            else if (PlayerAir < 0.0f)
            {
                PlayerHealth -= UnderWaterHealthDamage;
                playerHealthMeter.value = PlayerHealth;
            }           

            WaterDrag = 3.2f;
            
            if(!Input.GetKey(KeyCode.LeftShift))
            {
                m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, m_RigidBody.velocity.y * 0.5f, m_RigidBody.velocity.z);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                m_RigidBody.AddForce(FloatingForce * Vector3.up);
            }                           
        }
        else
        {
            if(PlayerAir < 100f)
            {
                PlayerAir += AirMeterRecovery;
                playerAirMeter.value = PlayerAir;
            }
            BubbleEffect.SetActive(false);
            WaterDrag = 0.0f;
            OceanAudioSource.pitch = 1;
            OceanAudioSource.volume = 0.2f;
        }

        if (m_onWaterSurface && !waterBehaviourScript.IsUnderWater())
        {
            //If we were underwater and reached the surface then float on top
            m_isFloating = true;
            m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0.0f, m_RigidBody.velocity.z);
        }
    }   

    public void EnableDisableAudioSource(bool enable)
    {
        PlayerAudioSource.enabled = enable;
    }

    private void SwimSoundDelay()
    {
       m_PlaySwimSound = true;
    }

    public bool IsOnWater()
    {
        return waterBehaviourScript.IsUnderWater();
    }

    void ActivateTrigger()
    {
        GroundTrigger.enabled = true;
        
    }

    public void SharkAttkDamage()
    {
        PlayerHealth -= SharkDamage;
        playerHealthMeter.value = PlayerHealth;       
    }

    private void OnTriggerStay(Collider other)
    {       
        //Check on ground collision
        if (other.tag == "Land" || other.tag == "Boat" || other.tag == "Ship")
        {
            MaxVelocity = 5.0f;
            m_isGrounded = true;
            m_isJumping = false;
            m_isFloating = false;
            m_onWaterSurface = false;
            m_RigidBody.useGravity = true;
        }

        if(other.tag == "Land" && m_Input != Vector3.zero && other.gameObject.layer == 9 && m_PlaySandWalkSound)
        {            
            m_PlaySandWalkSound = false;
            PlayerAudioSource.volume = 1.0f;
            if(TerrainInformationScript.GetActiveTerrainTextureIdx(transform.position) == 0)
            {
                PlayerAudioSource.clip = PlayerAudioClips[(int)SoundFXIndex.SANDWALKING];
            }
            else
            {
                PlayerAudioSource.clip = PlayerAudioClips[(int)SoundFXIndex.STONEWALKING];
            }
           
            PlayerAudioSource.Play();
            Invoke("SandWalkingDelay", 0.7f);
        }

        if ((other.tag == "Boat" || other.tag == "Ship") && m_Input.z != 0.0f && m_PlaySandWalkSound && !waterBehaviourScript.IsUnderWater())
        {
            m_PlaySandWalkSound = false;
            PlayerAudioSource.volume = 1.0f;
            PlayerAudioSource.clip = PlayerAudioClips[(int)SoundFXIndex.WOODWALKING];
            PlayerAudioSource.Play();
            Invoke("SandWalkingDelay", 0.5f);
        }
    }

    private void SandWalkingDelay()
    {
        m_PlaySandWalkSound = true;
    }

    public void JumpOnBoat()
    {
        BubbleEffect.SetActive(false);
        waterBehaviourScript.JumpOnBoat();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Land" || other.tag == "Boat" || other.tag == "Ship")
        {
            m_isGrounded = false;
        }
    }

    public void UpdateAnimation(CustomTypes.AnimationStateController animationState)
    {
        currentState = animationState;
        animController.AnimationStateChange(currentState);
    }

    void UpdateAnimationState()
    {
        if (m_Input == Vector3.zero && m_isGrounded && currentState != CustomTypes.AnimationStateController.IDLE)
        {
            currentState = CustomTypes.AnimationStateController.IDLE;
            animController.AnimationStateChange(CustomTypes.AnimationStateController.IDLE);
        }
        
        if(m_Input != Vector3.zero && m_isGrounded && currentState != CustomTypes.AnimationStateController.WALK)
        {
            currentState = CustomTypes.AnimationStateController.WALK;
            animController.AnimationStateChange(CustomTypes.AnimationStateController.WALK);
        }

        if(m_isJumping && currentState != CustomTypes.AnimationStateController.JUMP)
        {
            currentState = CustomTypes.AnimationStateController.JUMP;
            animController.AnimationStateChange(CustomTypes.AnimationStateController.JUMP);
        }

        if(m_Input == Vector3.zero && m_onWaterSurface && currentState != CustomTypes.AnimationStateController.FLOATING)
        {
            currentState = CustomTypes.AnimationStateController.FLOATING;
            animController.AnimationStateChange(CustomTypes.AnimationStateController.FLOATING);
        }

        if (m_Input != Vector3.zero && m_onWaterSurface && currentState != CustomTypes.AnimationStateController.SWIM)
        {
            currentState = CustomTypes.AnimationStateController.SWIM;
            animController.AnimationStateChange(CustomTypes.AnimationStateController.SWIM);
        }

        if(m_onWaterSurface && !m_isFloating && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift)) && currentState != CustomTypes.AnimationStateController.DIVING)
        {
            currentState = CustomTypes.AnimationStateController.DIVING;
            animController.AnimationStateChange(CustomTypes.AnimationStateController.DIVING);
        }
    }

    public AnimationController GetAnimationControlScript()
    {
        return animController;
    }

    public ref Transform GetPlayerCameraTransform()
    {
        return ref PlayerCameraTransform;
    }

    public void ReturnPlayerCameraToPosition()
    {
        this.PlayerCameraTransform.position = CameraPosition.position;
        this.PlayerCameraTransform.rotation = CameraPosition.rotation;
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, transform.rotation.eulerAngles.y, 0.0f));
    }
}
