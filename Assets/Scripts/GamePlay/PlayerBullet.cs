using UnityEngine;

namespace GamePlay
{
    public class PlayerBullet : Bullet
    {
        public override void Fire(Vector3 direction)
        {
            base.Fire(direction);
            transform.rotation = Quaternion.LookRotation(direction);
            transform.localEulerAngles += new Vector3(90, 0, 0);
        }
    }
}