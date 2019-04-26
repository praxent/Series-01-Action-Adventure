using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour 
{
    public float TimeUntilExplosion;
    public float ExplosionRadius;
    public GameObject ExplosionPrefab;
    private bool hasExploded = false;

    AudioSource bombSound;

    float m_CreationTime;

    void Awake()
    {
        bombSound = GetComponent(typeof(AudioSource)) as AudioSource;
    }

    void Start() 
    {
        m_CreationTime = Time.time;
    }
    
    void Update() 
    {
        float elapsedTime = Time.time - m_CreationTime;

        if( elapsedTime >= TimeUntilExplosion && !hasExploded )
        {
            OnExplode();
        }
    }

    void DestroyDestructablesInRadius()
    {
        Collider2D[] collidersInRadius = Physics2D.OverlapCircleAll( transform.position, ExplosionRadius );

        for( int i = 0; i < collidersInRadius.Length; ++i )
        {
            DestructableByBomb destructable = collidersInRadius[ i ].GetComponent<DestructableByBomb>();
            
            if( destructable != null )
            {
                destructable.OnDestroyedByBomb();
            }
        }
    }


    void OnExplode()
    {
        hasExploded = true;
        StartCoroutine(DestroyRoutine());
        StartCoroutine(CreateExplosionFXRoutine());


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere( transform.position, ExplosionRadius );
    }

    IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    IEnumerator CreateExplosionFXRoutine()
    {
        yield return new WaitForSeconds(.1f);
        if (bombSound)
        {
            bombSound.Play();
        }
        else
        {
            Debug.Log("No sound Available!");
        }
        yield return new WaitForSeconds(1.4f);
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        DestroyDestructablesInRadius();
        //var c_all = m_Control.gameObject.GetComponentsInChildren<Collider2D>();
        //foreach (var c in c_all)
        //{
        //    DestroyImmediate(c);
        //}


    }

}
