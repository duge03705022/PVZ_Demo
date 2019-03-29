using UnityEngine;
using System.Collections;

public class BucketheadZombieHealthy : ZombieHealthy {

    public AudioClip normalSound;

    public override void Damage(int val) {
        if (hp < 271 && damageSound != normalSound) {
            damageSound = normalSound;
        }
        base.Damage(val);
    }
}
