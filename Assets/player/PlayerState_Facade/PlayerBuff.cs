using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Buff
{
    void BuffUpdate();//update
    void StopBuff();
}

public class PlayerBuff : MonoBehaviour
{
    List<Buff> buffs=new();

}
