using System.Collections;
using UnityEngine;

public class TurretTriger : MonoBehaviour
{
    public delegate void Detect(GameObject enemy);
    public event Detect EnemyDetect;
    [SerializeField] string EnemyTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == EnemyTag)
        {
            EnemyDetect(other.gameObject);
            Debug.Log("������� ���������");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyDetect(null);
    }

    
}

