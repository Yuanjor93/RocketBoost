using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    
    [SerializeField] float rcsThrust = 75f; //sets the rotation thrust
    [SerializeField] float mainThrust = 25f; //sets the main thrust

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    float loadDelay = 1f;

    static Rigidbody rigidBody;
    static AudioSource audioSource;


    enum State { Alive, Dying, Transcending }

    State state = State.Alive;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive){
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", loadDelay);
    }
    private void StartDeathSequence()
    {      
        state = State.Dying;
        deathParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        Invoke("LoadFirstLevel", loadDelay);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); // allow more than 2 lvls
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }
     
    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; // take manual control of the rotation
        float rotateFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
       
            rigidBody.transform.Rotate(Vector3.forward * rotateFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigidBody.transform.Rotate(-Vector3.forward * rotateFrame);
        }
        rigidBody.freezeRotation = false; //resume physics rotation
    
    }
}
