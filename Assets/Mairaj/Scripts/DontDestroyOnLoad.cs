//Mairaj Muhammad ->2415831
using UnityEngine;
public class DontDestroyOnLoadObject : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}