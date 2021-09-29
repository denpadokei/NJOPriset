using SiraUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace NJOPriset.Installers
{
    public class NJOPMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<NJOPrisetController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }
    }
}
