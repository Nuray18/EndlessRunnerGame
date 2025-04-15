using UnityEngine;

public class DisablePlatform : MonoBehaviour
{
    private Transform player;
    public float disableDistance = 30f;

    public enum PlatformType
    {
        Center,
        Left,
        Right
    }

    public PlatformType platformType = PlatformType.Center;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null)
            return;

        if (player.position.z - transform.position.z > disableDistance)
        {
            Debug.Log("Platform disabled and returned to pool: " + gameObject.name);

            switch (platformType)
            {
                case PlatformType.Center:
                    ObjectPooler.Instance.ReturnToPool(gameObject);
                    break;
                case PlatformType.Left:
                    ObjectPooler.Instance.ReturnLeftPool(gameObject);
                    break;
                case PlatformType.Right:
                    ObjectPooler.Instance.ReturnRightPool(gameObject);
                    break;
            }
            //obj.name = "PooledPlatform";
        }
    }
}
