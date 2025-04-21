using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPbar : UI_Base
{
    BossController boss;
    Slider _slider;
    private void Start()
    {
        _slider.value = boss.CurrentHP;
    }
    public void Set(BossController _boss)
    {
        boss = _boss;
        boss.Damaged += OnHPChanged;
        _slider = Bind<Slider>("HP_Bar");
        _slider.maxValue = boss.MaxHP;
        _slider.value = boss.CurrentHP;
    }
    void OnHPChanged()
    {
        _slider.value = boss.CurrentHP;
    }
}
