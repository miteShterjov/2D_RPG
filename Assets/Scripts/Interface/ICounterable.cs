using UnityEngine;

public interface ICounterable
{
      public void HandleCounterAttack();
      public bool CanBeCountered { get; }
}
