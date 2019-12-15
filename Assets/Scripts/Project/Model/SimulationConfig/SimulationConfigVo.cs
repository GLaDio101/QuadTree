using UnityEngine;

namespace Project.Model.SimulationConfig
{
    public class SimulationConfigVo
    {
        public Vector2 WorldSize = new Vector2(40, 40);
        public int MaxBoxCount = 20;
        public int BoxStartLife = 5;
        public bool DrawGizmos = true;
    }
}