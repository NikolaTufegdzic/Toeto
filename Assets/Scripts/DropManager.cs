using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    public static DropManager Instance { get; private set;}

    [SerializeField] private Drop drop;

    private void Awake(){
        Instance = this;
    }

    public void DropCreator(Vector3 position, Quaternion spawnRotation){
        Drop newDrop = Instantiate(drop, position, spawnRotation);
        newDrop.transform.SetParent(GameManager.Instance.GetStage().transform);
        newDrop.RandomDrop();
    }

    public void DropHandler(GameObject dropObject){
        Drop drop = dropObject.GetComponent<Drop>();
        SoundController.Instance.PlaySoundDrop(); 
        switch (drop.dropType)
        {
            case DropTypesEnumSO.DropType.ExtendPlayerPad:
                PlayerPad.Instance.ExtendPad();
            break;
            case DropTypesEnumSO.DropType.ReducePlayerPad:
                PlayerPad.Instance.ReducePad();
            break;
            case DropTypesEnumSO.DropType.SlowPlayerPad:
                PlayerPad.Instance.SlowerPad();
            break;
            case DropTypesEnumSO.DropType.FastPlayerPad:
                PlayerPad.Instance.FasterPad();
            break;
            case DropTypesEnumSO.DropType.SlowPlayerBall:
                PlayerBall.Instance.SlowerBall();
            break;
            case DropTypesEnumSO.DropType.FastPlayerBall:
                PlayerBall.Instance.FasterBall();
            break;
            case DropTypesEnumSO.DropType.OneGold:
                GameManager.Instance.AddGold(1);
            break;
            case DropTypesEnumSO.DropType.TwoGold:
                GameManager.Instance.AddGold(2);
            break;
            default:
            break;
        }
        PlayerBall.Instance.AddBallPower(GameManager.Instance.GetDropPower());
        GameManager.Instance.DropCollected();
        Destroy(dropObject,1f);
    }

}
