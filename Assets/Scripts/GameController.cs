using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Helpers;
using JetBrains.Annotations;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Transform puzzleTransform;
    
    private LevelLoader _levelLoader;
    private Grid _grid;

    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            return _instance;
        }
    }

    public static event Action<BaseTile> OnUserTapped;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _grid = new Grid(width, height);
        ServiceLocator.Register<Grid>(_grid);
        _levelLoader = new LevelLoader(_grid, puzzleTransform);
    }

    public void OnTapPerformed(BaseTile tile = null)
    {
        OnUserTapped?.Invoke(tile);
    }
}
