using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEnemy : MonoBehaviour
{
    //privates


    // publics
    public int HP = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int damage)
    {
        HP -= damage;
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }


}
