using UnityEngine;

public class BigBoss : MonoBehaviour
{
    private AsteroidsSpawn _asteroidsSpawn;
    private Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _asteroidsSpawn = GetComponent<AsteroidsSpawn>();  
        if (_asteroidsSpawn == null)
        {
            Debug.LogError("BigBoss::AsteroidSpawn is null");
        }
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("BigBoss::Animator is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            _anim.SetBool("Attacking", true);
            _asteroidsSpawn.UseAsteroidAttack();
        }
    }



}
