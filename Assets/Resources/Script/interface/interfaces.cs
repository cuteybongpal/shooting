using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damaged
{
    public void damaged(int atk);
}
public interface Atk
{
    public int Atk { get; set; }
}