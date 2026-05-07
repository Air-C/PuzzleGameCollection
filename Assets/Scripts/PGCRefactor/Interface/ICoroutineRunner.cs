using System.Collections;
using UnityEngine;

namespace PGCRefactor.Interface
{
    public interface ICoroutineRunner
    {
        Coroutine Run(IEnumerator routine);
    }
}