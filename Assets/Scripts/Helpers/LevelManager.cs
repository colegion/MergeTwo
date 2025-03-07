using UnityEngine;
using System.IO;
using Pool;
using ScriptableObjects;

namespace Helpers
{
    public class LevelManager
    {
        private static string darkCellPath = "Prefabs/CellDark";
        private static string lightCellPath = "Prefabs/CellLight";
        private readonly string levelDataPath = "Levels/CurrentLevel.json";

        public LevelManager(Transform parent)
        {
            LoadLevel(parent);
        }

        private void LoadLevel(Transform parent)
        {
            var grid = ServiceLocator.Get<Grid>();
            var width = grid.Width;
            var height = grid.Height;
            LevelData levelData = null;
            if (File.Exists(levelDataPath))
            {
                string json = File.ReadAllText(levelDataPath);
                levelData = JsonUtility.FromJson<LevelData>(json);
                if (levelData != null)
                {
                    width = levelData.boardWidth;
                    height = levelData.boardHeight;
                    
                    CreateCells(width, height, parent);

                    //var poolController = ServiceLocator.Get<PoolController>();
                    var configManager = ServiceLocator.Get<ItemConfigManager>();
                    var itemFactory = ServiceLocator.Get<ItemFactory>();
                    foreach (var data in levelData.tiles)
                    {
                        //var tempTile = poolController.GetPooledObject(PoolableTypes.BaseTile);
                        //var tile = tempTile.GetGameObject().GetComponent<BaseTile>();
                        var config = configManager.GetItemConfig(data.itemType, data.itemLevel);
                        itemFactory.SpawnItemByConfig(config);
                        //tile.ConfigureSelf(config, data.xCoord, data.yCoord);
                    }
                }
            }
            else
            {
                CreateCells(width, height, parent);
            }
        }

        private void CreateCells(int width, int height, Transform parent)
        {
            var lightPrefabInstance = Resources.Load<BaseCell>(lightCellPath);
            var darkPrefabInstance = Resources.Load<BaseCell>(darkCellPath);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var prefabToUse = (j + i) % 2 == 0 ? lightPrefabInstance : darkPrefabInstance;
                    var cell = Object.Instantiate(prefabToUse, new Vector3(i, 0, j), Quaternion.identity, parent.transform);
                    cell.ConfigureSelf(i, j);
                }
            }
        }

        public void SaveLevel(LevelData levelData)
        {
            string json = JsonUtility.ToJson(levelData, true);
            File.WriteAllText(levelDataPath, json);
        }
    }
}
