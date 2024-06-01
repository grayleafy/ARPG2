using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepCloneTest : MonoBehaviour
{
    public List<int> obj1;
    [SerializeReference, SubclassSelector]
    public Skill skill;
    public Skill skill2;
    public int i;



    // Start is called before the first frame update
    void Start()
    {
        //var obj = DeepCloner.GetInstance().DeepClone(obj1);
        skill2 = DeepCloner.GetInstance().DeepCopy(skill);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
