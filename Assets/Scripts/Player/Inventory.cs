using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int _maxCountHealthPotions;
    [SerializeField] private int _maxCoutnSingTables;

    private List<HealthPotion> _healthPotions=new List<HealthPotion>();
    private List<SingTable> _singTables = new List<SingTable>(2);

    public int CountHealthPotions => _healthPotions.Count;
    public int CountSingTables => _singTables.Count;

    public event UnityAction<int> HealthPotionCountChanged;
    public event UnityAction<int> SingTablesCountChanged;

    private void OnEnable()
    {
        HealthPotionCountChanged?.Invoke(CountHealthPotions);
        SingTablesCountChanged?.Invoke(CountSingTables);
    }

    public void AddItem(InventoryItemType type, int count)
    {
        if (type == InventoryItemType.Health)
            AddHealthPotion(count);
        else if (type == InventoryItemType.SingTable)
            AddSingTable(count);

    }

    public bool RemoveItem(InventoryItemType type)
    {
        if (type == InventoryItemType.Health)
            return GetHealingPotion();        
        else if (type == InventoryItemType.SingTable)
            return GetSingTable();

        return false;
    }


    private void AddHealthPotion(int count )
    {
        for (int i = 0; i < count; i++)
        {
            if (CountSingTables < _maxCoutnSingTables)
            {
                _healthPotions.Add(new HealthPotion());
                HealthPotionCountChanged?.Invoke(CountHealthPotions);
            }
        }
    }

    private void AddSingTable(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (CountHealthPotions < _maxCountHealthPotions)
            {
                _singTables.Add(new SingTable());
                SingTablesCountChanged?.Invoke(CountSingTables);
            }
        }
    }


    private  bool GetHealingPotion()
    {
        if (CountHealthPotions > 0)
        {
            _healthPotions.Remove(_healthPotions[0]);
            HealthPotionCountChanged?.Invoke(CountHealthPotions);
            return true;
        }
        return false;
    }
    
    private bool GetSingTable()
    {
        if (CountSingTables > 0)
        {
            _singTables.Remove(_singTables[0]);
            SingTablesCountChanged?.Invoke(CountSingTables);
            return true;
        }
        return false;
    }


    public enum InventoryItemType
    {
        Health,
        SingTable
    }
}