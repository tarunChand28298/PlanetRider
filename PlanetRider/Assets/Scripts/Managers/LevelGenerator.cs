using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject planetPrefab;

    private PlayerRocket _targetPlayer;
    private DifficultyManager difficultyManager;

    public PlayerRocket TargetPlayer
    {
        get { return _targetPlayer; }
        set 
        { 
            _targetPlayer = value;
            TargetPlayer.PlayerLanded += PlayerLandedOnPlanet;
        }
    }

    private void PlayerLandedOnPlanet(Vector3 planetPosition, float landAngle)
    {
        SpawnPlanet(planetPosition);
    }

    void Start()
    {
        difficultyManager = GetComponent<DifficultyManager>();
    }

    public void SpawnPlanet(Vector3 currentPlanetPosition)
    {
        float scale = Random.Range(1.0f, 2.5f) * (Mathf.Abs(difficultyManager.GetSizeMultiplier() - 1.0f));
        float rotSpeed = Random.Range(80.0f, 140.0f) * (1.0f + difficultyManager.GetSpeedMultiplier());
        int rotDir = Random.Range(0, 2) * 2 - 1;

        GameObject newPlanet = Instantiate(planetPrefab);
        float cameraHorizontalExtents = Camera.main.orthographicSize * Screen.width / Screen.height;
        float cameraVerticalExtents = Camera.main.orthographicSize;
        float cameraVerticalOffset = Camera.main.GetComponent<FollowCamera>().vOffset + currentPlanetPosition.y;

        float leftExtreme = -cameraHorizontalExtents;
        float rightExtreme = cameraHorizontalExtents;
        float topExtreme = cameraVerticalExtents + cameraVerticalOffset;
        float bottomExtreme = cameraVerticalOffset;

        float leftBound = leftExtreme + (scale);
        float rightBound = rightExtreme - (scale);
        float topBound = topExtreme - (scale);
        float bottomBound = bottomExtreme + (scale);

        float horizontal = Random.Range(leftBound, rightBound);
        float vertical = Random.Range(bottomBound, topBound);
        newPlanet.transform.position = new Vector3(horizontal, vertical, 0.0f);
        newPlanet.transform.localScale = Vector3.zero;
        LeanTween.scale(newPlanet, new Vector3(scale, scale, scale), 0.5f).setEaseInOutCubic();
        newPlanet.GetComponent<Planet>().rotationSpeed = rotSpeed * rotDir;
        newPlanet.GetComponent<Planet>().timed = difficultyManager.GetTimedProbability();

        newPlanet.GetComponent<Renderer>().material.SetFloat("UvXOffset", Random.Range(0.0f, 200.0f));
        newPlanet.GetComponent<Renderer>().material.SetFloat("UvYOffset", Random.Range(0.0f, 200.0f));
    }
}
