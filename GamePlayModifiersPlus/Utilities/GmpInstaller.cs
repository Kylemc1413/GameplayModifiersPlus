using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiraUtil.Zenject;
using Zenject;
namespace GamePlayModifiersPlus.Utilities
{
    class GmpInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameObjects>().AsSingle().NonLazy();
        }
    }
}
