using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameAnalyticsConfig : BaseAnalyticsConfig
{
    public List<string> availableResourceCurrencies = new List<string>();
    public List<string> availableResourceItemTypes = new List<string>();
}
