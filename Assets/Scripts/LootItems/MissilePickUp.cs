using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePickUp : LootItem
{
    protected override void PickUp()
    {
        //������ҵĵ�����ȡ����
        player.PickUpMissile();
        base.PickUp();
    }
}
