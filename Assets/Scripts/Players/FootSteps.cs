using GameManagers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField] private GameObject dustSmoke;
    public void OnPlayMoveSE()
    {
        AudioManager.Instance.PlaySE("Test2_Ashioto_SE");
        if (dustSmoke != null)
        {
            Transform transform = this.transform.parent;
            GameObject effect = Instantiate(dustSmoke,this.transform);
            
            ParticleSystem particleSystem = effect.GetComponentInChildren<ParticleSystem>();
            particleSystem.Play();
            //fix me クローンが子オブジェクトで残り続ける        
        }
    }
}
