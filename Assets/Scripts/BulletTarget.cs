using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTarget : MonoBehaviour
{
    Block block = null;
    public void SetBlock(Block newBlock){
        block = newBlock;
    }
    public Block GetBlock(){
        return block;
    }
}
