using System.Globalization;
using Project.Model.SimulationConfig;
using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine.UI;

namespace Project.View.SimulationConfig
{
    public class SimulationConfigScreenView : EventView
    {
        public SimulationConfigVo Config { get; set; }

        public TMP_InputField WorldSizeX;
        public TMP_InputField WorldSizeY;

        public TMP_InputField MaxBoxCount;
        public TMP_InputField BoxStartLife;

        public Toggle DrawGizmosToggle;

        protected override void Start()
        {
            base.Start();

            WorldSizeX.text = Config.WorldSize.x.ToString(CultureInfo.InvariantCulture);
            WorldSizeY.text = Config.WorldSize.y.ToString(CultureInfo.InvariantCulture);

            MaxBoxCount.text = Config.MaxBoxCount.ToString();
            BoxStartLife.text = Config.BoxStartLife.ToString();

            DrawGizmosToggle.isOn = Config.DrawGizmos;
        }

        public void OnClickBack()
        {
            Save();
            dispatcher.Dispatch(SimulationConfigScreenEvent.Back);
        }

        private void Save()
        {
            var worldX = 40;
            var worldY = 40;
            var maxBoxCount = 20;
            var boxStartLife = 5;

            int.TryParse(WorldSizeX.text, out worldX);
            Config.WorldSize.x = worldX;

            int.TryParse(WorldSizeY.text, out worldY);
            Config.WorldSize.y = worldY;

            int.TryParse(MaxBoxCount.text, out maxBoxCount);
            Config.MaxBoxCount = maxBoxCount;

            int.TryParse(BoxStartLife.text, out boxStartLife);
            Config.BoxStartLife = boxStartLife;

            Config.DrawGizmos = DrawGizmosToggle.isOn;
        }
    }
}