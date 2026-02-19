using CodeBase.Infrastructure.SceneLoad;
using UnityEngine;
using VContainer;

namespace CodeBase.Infrastructure
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private IStartupSequence _startup;

        [Inject]
        public void Construct(IStartupSequence startup) => _startup = startup;

        public void Run() => StartCoroutine(_startup.Run());
    }
}