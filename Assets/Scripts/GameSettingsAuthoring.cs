using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEditor;

public class GameSettingsAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{

    [Header("Field Boundaries")] 
    [SerializeField] private float _fieldWidth;
    [SerializeField] private float _fieldHeight;

    [Header("Player Properties")] 
    [SerializeField] private float _playerSpeed;
    [SerializeField] private int _playerHealth;

    [Header("Asteroids Properties")]
    [SerializeField] private int _asteroidSpeed;
    [SerializeField] private int _asteroidRotation;
    [SerializeField] private int _asteroidHealth;
    [SerializeField] private int _asteroidSpawnRate;
    [SerializeField] private int _asteroidDensity;
    [SerializeField] private float _asteroidSpawnTimer;

    [Header("Enemy Properties")]
    [SerializeField] private int _enemySpeed;
    [SerializeField] private int _enemyHealth;
    [SerializeField] private int _enemySpawnRate;
    [SerializeField] private int _enemyDensity;
    [SerializeField] private float _enemySpawnTimer;

    [Header("Projectile Properties")] 
    [SerializeField] private int _projectileSpeed;
    [SerializeField] private float _projectilesPerSecond;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var gameSettings = default(GameSettingsComponent);

        gameSettings.FieldWidth = _fieldWidth;
        gameSettings.FieldHeight = _fieldHeight;

        gameSettings.PlayerSpeed = _playerSpeed;
        gameSettings.PlayerHealth = _playerHealth;

        gameSettings.AsteroidSpeed = _asteroidSpeed;
        gameSettings.AsteroidRotation = _asteroidRotation;
        gameSettings.AsteroidHealth = _asteroidHealth;
        gameSettings.AsteroidSpawnRate = _asteroidSpawnRate;
        gameSettings.AsteroidDensity = _asteroidDensity;
        gameSettings.AsteroidSpawnTimer = gameSettings.ASpawnSetting = _asteroidSpawnTimer;

        gameSettings.EnemySpeed = _enemySpeed;
        gameSettings.EnemyHealth = _enemyHealth;
        gameSettings.EnemySpawnRate = _enemySpawnRate;
        gameSettings.EnemyDensity = _enemyDensity;
        gameSettings.EnemySpawnTimer = gameSettings.ESpawnSetting =_enemySpawnTimer;

        gameSettings.ProjectileSpeed = _projectileSpeed;
        gameSettings.ProjectilesPerSecond = _projectilesPerSecond; 
        
        dstManager.AddComponentData(entity, gameSettings); 

    }
}
