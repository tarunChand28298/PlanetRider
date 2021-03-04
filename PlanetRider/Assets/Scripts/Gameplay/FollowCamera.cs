using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private PlayerRocket _playerToFollow;

    public PlayerRocket PlayerToFollow
    {
        get { return _playerToFollow; }
        set 
        { 
            _playerToFollow = value;
            PlayerToFollow.PlayerLanded += MoveCameraToPlanet;
        }
    }

    public float vOffset;

    private void MoveCameraToPlanet(Vector3 newPlanet, float landAngle)
    {
        LeanTween.moveY(gameObject, newPlanet.y + vOffset, 0.5f).setEaseInOutCubic();
    }

}
